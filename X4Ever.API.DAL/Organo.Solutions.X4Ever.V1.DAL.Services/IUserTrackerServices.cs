
namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using System.Threading.Tasks;
    using Organo.Solutions.X4Ever.V1.DAL.Model;
    using System.Collections.Generic;

    public interface IUserTrackerServices
    {
        bool Delete(ref ValidationErrors validationErrors, string token);
        bool Delete(ref ValidationErrors validationErrors, long userId);
        bool Delete(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity);
        bool Delete(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity);
        IEnumerable<UserTrackerMedia> GetImages(long userId);
        Task<IEnumerable<UserTrackerMedia>> GetImagesAsync(long userId);
        Task<IEnumerable<UserTrackerMedia>> GetSpecificImagesAsync(string key);
        Task<IEnumerable<UserTrackerMedia>> GetSpecificImagesAsync(long userId);
        IEnumerable<UserTrackerMedia> GetSpecificImages(long userId);
        IEnumerable<UserTrackerMedia> GetSpecificImages(string key);
        IEnumerable<UserTracker> Get(string token);
        Task<IEnumerable<UserTracker>> GetAsync(string token);
        IEnumerable<UserTracker> Get(long userId);
        Task<IEnumerable<UserTracker>> GetAsync(long userId);
        IEnumerable<TrackerPivot> GetAsPivot(string token);
        Task<IEnumerable<TrackerPivot>> GetAsPivotAsync(string token);
        IEnumerable<TrackerPivot> GetAsPivot(long userId);
        Task<IEnumerable<TrackerPivot>> GetAsPivotAsync(long userId);
        IEnumerable<UserTracker> GetLatest(string token);
        Task<IEnumerable<UserTracker>> GetLatestAsync(string token);
        IEnumerable<UserTracker> GetLatest(long userId);
        Task<IEnumerable<UserTracker>> GetLatestAsync(long userId);
        IEnumerable<UserTracker> GetFirstAndLast(string token);
        Task<IEnumerable<UserTracker>> GetFirstAndLastAsync(string token);
        IEnumerable<UserTracker> GetFirstAndLast(long userId);
        Task<IEnumerable<UserTracker>> GetFirstAndLastAsync(long userId);
        IEnumerable<UserTracker> GetByAttribute(string token, string attributeName);
        Task<IEnumerable<UserTracker>> GetByAttributeAsync(string token, string attributeName);
        IEnumerable<UserTracker> GetByAttribute(long userId, string attributeName);
        Task<IEnumerable<UserTracker>> GetByAttributeAsync(long userId, string attributeName);
        IEnumerable<UserTracker> GetByAttributeLabel(string token, string attributeLabel);
        Task<IEnumerable<UserTracker>> GetByAttributeLabelAsync(string token, string attributeLabel);
        IEnumerable<UserTracker> GetByAttributeLabel(long userId, string attributeLabel);
        Task<IEnumerable<UserTracker>> GetByAttributeLabelAsync(long userId, string attributeLabel);
        bool Insert(ref ValidationErrors validationErrors, ICollection<UserTracker> entity);

        bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity);

        bool Insert(ref ValidationErrors validationErrors, string token, UserTracker entity);
        bool Insert(ref ValidationErrors validationErrors, long userId, UserTracker entity);

        bool Update(ref ValidationErrors validationErrors, ICollection<UserTracker> entity);
        Task<IEnumerable<UserTrackerPivot>> GetUserRowAsync(long userId);
    }
}