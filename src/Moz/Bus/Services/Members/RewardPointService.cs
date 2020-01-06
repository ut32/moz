using System;
using Moz.Bus.Models.Members;
using Moz.Common;
using Moz.Events;
using Moz.Utils;

namespace Moz.Core.Service.Members
{
    /// <summary>
    /// </summary>
    public class RewardPointService : IRewardPointService
    {
        #region Ctor

        /// <summary>
        /// </summary>
        /// <param name="rphRepository"></param>
        /// <param name="rewardPointsSettings"></param>
        /// <param name="eventPublisher"></param>
        public RewardPointService( //IRepository rphRepository,
            RewardPointsSettings rewardPointsSettings,
            IEventPublisher eventPublisher)
        {
            //this._rphRepository = rphRepository;
            _rewardPointsSettings = rewardPointsSettings;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Fields

        //private readonly IRepository _rphRepository;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Utilities

        /*
        /// <summary>
        /// Update reward points balance if necessary
        /// </summary>
        /// <param name="query">Input query</param>
        protected void UpdateRewardPointsBalance(IQueryable<RewardPointsHistory> query)
        {
            //order history by date
            query = query.OrderBy(rph => rph.CreatedOnUtc).ThenBy(rph => rph.Id);

            //get has not yet activated points, but it's time to do it
            //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
            //That's why we pass the date value
            var nowUtc = DateTime.UtcNow;
            var notActivatedRph = query.Where(rph => !rph.PointsBalance.HasValue && rph.CreatedOnUtc < nowUtc).ToList();

            //nothing to update
            if (!notActivatedRph.Any())
                return;

            //get current points balance, LINQ to entities does not support Last method, thus order by desc and use First one
            var lastActive = query.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id).FirstOrDefault(rph => rph.PointsBalance.HasValue);
            var currentPointsBalance = lastActive != null ? lastActive.PointsBalance : 0;

            //update appropriate records
            foreach (var rph in notActivatedRph)
            {
                rph.PointsBalance = currentPointsBalance + rph.Points;
                UpdateRewardPointsHistoryEntry(rph);
                currentPointsBalance = rph.PointsBalance;
            }
        }
        */

        #endregion

        #region Methods

        /*
        /// <summary>
        /// Load reward point history records
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records (filter by current store if possible)</param>
        /// <param name="showNotActivated">A value indicating whether to show reward points that did not yet activated</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Reward point history records</returns>
        public virtual IPagedList<RewardPointsHistory> GetRewardPointsHistory(int customerId = 0, bool showHidden = false,
            bool showNotActivated = false, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _rphRepository.Table;
            if (customerId > 0)
                query = query.Where(rph => rph.CustomerId == customerId);
            if (!showHidden && !_rewardPointsSettings.PointsAccumulatedForAllStores)
            {
                //filter by store
                var currentStoreId = _storeContext.CurrentStore.Id;
                query = query.Where(rph => rph.StoreId == currentStoreId);
            }
            if (!showNotActivated)
            {
                //show only the points that already activated

                //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
                //That's why we pass the date value
                var nowUtc = DateTime.UtcNow;
                query = query.Where(rph => rph.CreatedOnUtc < nowUtc);
            }

            //update points balance
            UpdateRewardPointsBalance(query);

            query = query.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id);

            var records = new PagedList<RewardPointsHistory>(query, pageIndex, pageSize);
            return records;
        }
        */

        /// <summary>
        /// </summary>
        /// <param name="member"></param>
        /// <param name="points"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual long AddRewardPointsHistoryEntry(Member member, int points, string message = "")
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            var rph = new RewardPointsHistory
            {
                Points = points,
                PointsBalance = 0,
                Message = message,
                AddTime = DateTime.UtcNow,
                MemberId = member.Id
            };

            var obj = MutexHelper.Instance.GetOrAdd(member.Id.ToString(), () => new object());
            lock (obj)
            {
                var newPointsBalance = GetRewardPointsBalance(member.Id) + points;
                rph.PointsBalance = newPointsBalance;
                //var id = _rphRepository.Insert(rph);
                rph.Id = 0;
            }

            //event notification
            _eventPublisher.EntityCreated(rph);

            return rph.Id;
        }

        /// <summary>
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public virtual int GetRewardPointsBalance(long memberId)
        {
            throw new Exception("xx");
            /*
            return _rphRepository.UseWriteDb(db =>
            {
                return db.Queryable<RewardPointsHistory>()
                    .Where(o => o.MemberId == memberId)
                    .OrderBy(o => o.Id, OrderByType.Desc)
                    .Select(t => t.PointsBalance)
                    .First();
            });
            */
            /*
            var query = _rphRepository.Table;
            if (customerId > 0)
                query = query.Where(rph => rph.CustomerId == customerId);
            if (!_rewardPointsSettings.PointsAccumulatedForAllStores)
                query = query.Where(rph => rph.StoreId == storeId);

            //show only the points that already activated
            //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
            //That's why we pass the date value
            var nowUtc = DateTime.UtcNow;
            query = query.Where(rph => rph.CreatedOnUtc < nowUtc);

            //first update points balance
            UpdateRewardPointsBalance(query);

            query = query.OrderByDescending(rph => rph.CreatedOnUtc).ThenByDescending(rph => rph.Id);

            var lastRph = query.FirstOrDefault();
            return lastRph != null && lastRph.PointsBalance.HasValue ? lastRph.PointsBalance.Value : 0;
            */
        }

        /// <summary>
        ///     Gets a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <returns>Reward point history entry</returns>
        public virtual RewardPointsHistory GetRewardPointsHistoryEntryById(long rewardPointsHistoryId)
        {
            throw new Exception("xx");
            /*
            if (rewardPointsHistoryId == 0)
                return null;

            return _rphRepository.GetById<RewardPointsHistory>(rewardPointsHistoryId);
            */
        }

        /// <summary>
        ///     Delete the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        public virtual void DeleteRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory)
        {
            throw new Exception("xx");
            /*
            if (rewardPointsHistory == null)
                throw new ArgumentNullException(nameof(rewardPointsHistory));

            _rphRepository.Delete(rewardPointsHistory);

            //event notification
            _eventPublisher.EntityDeleted(rewardPointsHistory);
            */
        }

        /// <summary>
        ///     Updates the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        public virtual void UpdateRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory)
        {
            throw new Exception("xx");
            /*
            if (rewardPointsHistory == null)
                throw new ArgumentNullException(nameof(rewardPointsHistory));

            _rphRepository.Update(rewardPointsHistory);
            */

            //event notification
            _eventPublisher.EntityUpdated(rewardPointsHistory);
        }

        #endregion
    }
}