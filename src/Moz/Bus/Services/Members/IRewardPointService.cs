using Moz.Bus.Models.Members;
using Moz.CMS.Models.Members;
using Moz.Models.Members;

namespace Moz.Core.Service.Members
{
    public interface IRewardPointService
    {
        /// <summary>
        /// Load reward point history records
        /// </summary>
        /// <param name="customerId">Customer identifier; 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records (filter by current store if possible)</param>
        /// <param name="showNotActivated">A value indicating whether to show reward points that did not yet activated</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Reward point history records</returns>
        //IPagedList<RewardPointsHistory> GetRewardPointsHistory(int customerId = 0, bool showHidden = false,
        //    bool showNotActivated = false, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        ///     Adds the reward points history entry.
        /// </summary>
        /// <returns>The reward points history entry.</returns>
        /// <param name="member">Member.</param>
        /// <param name="points">Points.</param>
        /// <param name="message">Message.</param>
        long AddRewardPointsHistoryEntry(Member member, int points, string message = "");

        /// <summary>
        ///     Gets the reward points balance.
        /// </summary>
        /// <returns>The reward points balance.</returns>
        /// <param name="memberId">Member identifier.</param>
        int GetRewardPointsBalance(long memberId);

        /// <summary>
        ///     Gets a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <returns>Reward point history entry</returns>
        RewardPointsHistory GetRewardPointsHistoryEntryById(long rewardPointsHistoryId);

        /// <summary>
        ///     Delete the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        void DeleteRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory);

        /// <summary>
        ///     Updates the reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistory">Reward point history entry</param>
        void UpdateRewardPointsHistoryEntry(RewardPointsHistory rewardPointsHistory);
    }
}