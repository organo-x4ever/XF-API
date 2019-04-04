
namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using System.Threading.Tasks;
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using System.Collections.Generic;

    public interface IUserTrackerPivotServices
    {
        Task<bool> Delete(long userId, int revisionNumber);
        TrackerPivot GetLatestTracker(string token);
        Task<TrackerPivot> GetLatestTrackerAsync(string token);
        TrackerPivot GetLatestTracker(long userId);
        Task<TrackerPivot> GetLatestTrackerAsync(long userId);
        IEnumerable<TrackerPivot> GetTrackers(string token);
        Task<IEnumerable<TrackerPivot>> GetTrackersAsync(string token);
        IEnumerable<TrackerPivot> GetTrackers(long userId);
        Task<IEnumerable<TrackerPivot>> GetTrackersAsync(long userId);
        Task<IEnumerable<TrackerPivot>> GetTrackersAsync(long userId, string weightVolumeType);
        Task<IEnumerable<TrackerPivot>> GetTrackersAsync(long userId, string weightVolumeType, double intervalDays);
        Task<IEnumerable<UserTracker>> GetByAttributeAsync(string token, string attributeName);
        Task<IEnumerable<UserTracker>> GetByAttributeAsync(long userId, string attributeName);
        bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity);
        bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue);
    }
}