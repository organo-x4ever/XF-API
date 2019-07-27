
using Organo.Solutions.X4Ever.V1.DAL.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface ITrackerPhotoSkipServices
    {
        ValidationErrors ValidationErrors { get; set; }
        Task<List<TrackerPhotoSkip>> GetAllAsync(string filePath);
        Task<string> GetJsonAsync(string filePath);
        Task<List<TrackerPhotoSkip>> GetByUserIDAsync(string filePath, long userID);
        Task<int> CountByUserIDAsync(string filePath, long userID);
        Task<List<TrackerPhotoSkip>> GetByDateAsync(string filePath, DateTime date);
        Task<List<TrackerPhotoSkip>> GetByDateRangeAsync(string filePath, DateTime fromDate, DateTime toDate);
        Task<bool> SaveAsync(string filePath, TrackerPhotoSkip userFeedback);
        Task<bool> SaveAsync(string filePath, long userID, string token, string base64String);
    }
}