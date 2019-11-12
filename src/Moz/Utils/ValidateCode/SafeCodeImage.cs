using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace Moz.Common.ValidateCode
{
    /// <summary>
    ///     生成验证码图片
    /// </summary>
    public class SafeCodeImage
    {
        #region 主处理函数

        /// <summary>
        ///     处理HTTP请求
        /// </summary>
        /// <param name="context"></param>
        public byte[] GetImage(string code)
        {
            //初始化颜色集合
            _colors = new List<Color>();
            InitColors();

            //初始化背景
            _bgColor = Color.FromArgb(61, 110, 159);

            return CreateValidateCodeImage(code);
        }

        #endregion

        #region 字段

        /// <summary>
        ///     颜色集合
        /// </summary>
        private List<Color> _colors;

        /// <summary>
        ///     背景色
        /// </summary>
        private Color _bgColor;

        /// <summary>
        ///     是否创建干扰线
        /// </summary>
        private bool _isGenereateDisturbLine = false;

        #endregion

        #region 功能子函数

        #region 初始化颜色集合

        /// <summary>
        ///     初始化颜色集合
        /// </summary>
        private void InitColors()
        {
            _colors.Add(Color.Black);
            _colors.Add(Color.Red);
            _colors.Add(Color.Green);
            _colors.Add(Color.Blue);
            _colors.Add(Color.Brown);
            _colors.Add(Color.DarkMagenta);
            _colors.Add(Color.DarkRed);
            _colors.Add(Color.Indigo);
            _colors.Add(Color.DarkBlue);
            _colors.Add(Color.Maroon);
        }

        #endregion

        #region 创建验证码图片并输出

        /// <summary>
        ///     创建验证码图片并输出
        /// </summary>
        /// <param name="validateCode">验证码</param>
        private byte[] CreateValidateCodeImage(string validateCode)
        {
            byte[] result = null;
            //创建随机数
            var random = new Random();

            //随机前景色
            var fcolor = Color.Black; // _colors[DateTime.Now.Millisecond % 10];

            //定义画笔
            var brush = new SolidBrush(fcolor);
            using (var image = new Bitmap(80, 28))
            {
                using (var g = Graphics.FromImage(image))
                {
                    //填充背景
                    g.Clear(_bgColor);

                    //画边框
                    //g.DrawRectangle(new Pen(brush), 0, 0, 79, 21);

                    //创建干扰线
                    //if (_isGenereateDisturbLine)
                    //{
                    //    int x1, x2, y1, y2;
                    //    for (int i = 0; i < validateCode.Length; i++)
                    //    {
                    //        Pen p = new Pen(_colors[DateTime.Now.Second % 10], 1.5f);
                    //        x1 = random.Next(image.Width);
                    //        y1 = random.Next(image.Height);
                    //        x2 = random.Next(image.Width);
                    //        y2 = random.Next(image.Height);
                    //        g.DrawLine(p, x1, y1, x2, y2);
                    //    }
                    //}

                    //生成随机噪点
                    //for (int i = 0; i < image.Width * 2; i++)
                    //{
                    //    int x = random.Next(image.Width);
                    //    int y = random.Next(image.Height);
                    //    image.SetPixel(x, y, fcolor);
                    //}

                    //画文本
                    for (var i = 0; i < validateCode.Length; i++)
                    {
                        var c = validateCode.Substring(i, 1);
                        var v = c[0];
                        var f = (v + DateTime.Now.Millisecond) % 2 == 0
                            ? new Font("Verdana", 14 - DateTime.Now.Second % 2 / 2)
                            : new Font("Verdana", 14 + DateTime.Now.Second % 2 / 2, FontStyle.Italic);
                        g.DrawString(c, f, brush, 4 + i * 17 - DateTime.Now.Millisecond % 2, DateTime.Now.Second % 2);
                    }

                    //输出图片
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Png);
                        result = ms.ToArray();
                    }
                }
            }

            return result;
        }

        #endregion

        #endregion
    }
}