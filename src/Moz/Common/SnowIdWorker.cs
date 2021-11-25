using System;

namespace Moz.Common
{
    /// <summary>
    /// 雪花ID
    /// </summary>
    public class SnowIdWorker
    {
        //基准时间
        private const long Twepoch = 1288834974657L;
        //机器标识位数
        const int WorkerIdBits = 5;
        //数据标志位数
        const int DatacenterIdBits = 5;
        //序列号识位数
        const int SequenceBits = 12;
        //机器ID最大值
        const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        //数据标志ID最大值
        const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        //序列号ID最大值
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);
        //机器ID偏左移12位
        private const int WorkerIdShift = SequenceBits;
        //数据ID偏左移17位
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        //时间毫秒左移22位
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;

        private long WorkerId { get; }
        private long DatacenterId { get; }
        
        public long Sequence
        {
            get => _sequence;
            internal set => _sequence = value;
        }
 
        public SnowIdWorker(long workerId, long datacenterId, long sequence = 0L)
        {
            // 如果超出范围就抛出异常
            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id 必须大于0，且不能大于MaxWorkerId： {MaxWorkerId}");
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"region Id 必须大于0，且不能大于MaxWorkerId： {MaxDatacenterId}");
            }

            //先检验再赋值
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;
        }

        private readonly object _lock = new();
        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();
                if (timestamp < _lastTimestamp)
                {
                    throw new Exception($"时间戳必须大于上一次生成ID的时间戳.  拒绝为{_lastTimestamp - timestamp}毫秒生成id");
                }

                //如果上次生成时间和当前时间相同,在同一毫秒内
                if (_lastTimestamp == timestamp)
                {
                    //sequence自增，和sequenceMask相与一下，去掉高位
                    _sequence = (_sequence + 1) & SequenceMask;
                    //判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                    if (_sequence == 0)
                    {
                        //等待到下一毫秒
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    //如果和上次生成时间不同,重置sequence，就是下一毫秒开始，sequence计数重新从0开始累加,
                    //为了保证尾数随机性更大一些,最后一位可以设置一个随机数
                    _sequence = 0;//new Random().Next(10);
                }

                _lastTimestamp = timestamp;
                return ((timestamp - Twepoch) << TimestampLeftShift) | (DatacenterId << DatacenterIdShift) | (WorkerId << WorkerIdShift) | _sequence;
            }
        }

        // 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        // 获取当前的时间戳
        private long TimeGen()
        {
            return TimeExtensions.CurrentTimeMillis();
        }
    }
    
    public static class TimeExtensions
    {
        private static Func<long> _currentTimeFunc = InternalCurrentTimeMillis;
 
        public static long CurrentTimeMillis()
        {
            return _currentTimeFunc();
        }

        public static IDisposable StubCurrentTime(Func<long> func)
        {
            _currentTimeFunc = func;
            return new DisposableAction(() =>
            {
                _currentTimeFunc = InternalCurrentTimeMillis;
            });  
        }

        public static IDisposable StubCurrentTime(long millis)
        {
            _currentTimeFunc = () => millis;
            return new DisposableAction(() =>
            {
                _currentTimeFunc = InternalCurrentTimeMillis;
            });
        }

        private static readonly DateTime Jan1St1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static long InternalCurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
        }        
    }
    
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Dispose()
        {
            _action();
        }
    }
}