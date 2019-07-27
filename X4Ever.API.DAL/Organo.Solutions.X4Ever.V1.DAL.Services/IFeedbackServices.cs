
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IFeedbackServices
    {
        ValidationErrors ValidationErrors { get; set; }
        Task<List<UserFeedback>> GetAllAsync(string filePath);
        Task<string> GetJsonAsync(string filePath);
        Task<List<UserFeedback>> GetByDateAsync(string filePath, DateTime date);
        Task<List<UserFeedback>> GetByDateRangeAsync(string filePath, DateTime fromDate, DateTime toDate);
        Task<bool> SaveAsync(string filePath, UserFeedback userFeedback);
        Task<bool> DeleteAsync(string filePath, string ID);
    }
}