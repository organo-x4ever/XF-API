
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class FeedbackServices : IFeedbackServices
    {
        private Helper.IHelper _helper;
        public FeedbackServices()
        {
            _helper = new Helper.Helper();
        }

        public ValidationErrors ValidationErrors { get; set; }

        public async Task<List<UserFeedback>> GetAllAsync(string filePath)
        {
            // This text is added only once to the file.
            var data = await GetJsonAsync(filePath);
            var photoSkipLogData = JsonConvert.DeserializeObject<List<UserFeedback>>(data);
            if (photoSkipLogData is List<UserFeedback>)
            {
                return photoSkipLogData;
            }
            return new List<UserFeedback>();
        }

        public async Task<string> GetJsonAsync(string filePath)
        {
            // This text is added only once to the file.
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            return string.Empty;
        }

        public async Task<List<UserFeedback>> GetByDateAsync(string filePath, DateTime date)
        {
            var response = await this.GetAllAsync(filePath);
            return response.Where(f => Convert.ToDateTime(f.Date) == date).ToList();
        }

        public async Task<List<UserFeedback>> GetByDateRangeAsync(string filePath, DateTime fromDate, DateTime toDate)
        {
            var response = await this.GetAllAsync(filePath);
            return response.Where(f => Convert.ToDateTime(f.Date) > fromDate && Convert.ToDateTime(f.Date) <= toDate).ToList();
        }

        public async Task<bool> SaveAsync(string filePath, UserFeedback userFeedback)
        {
            this.ValidationErrors = new ValidationErrors();
            try
            {
                var userFeedbacks = new List<UserFeedback>();
                // This text is added only once to the file.
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var data = await reader.ReadToEndAsync();
                        var photoSkipLogData = JsonConvert.DeserializeObject<List<UserFeedback>>(data);
                        if (photoSkipLogData is List<UserFeedback>)
                        {
                            userFeedbacks = photoSkipLogData;
                        }
                    }
                }

                // Create/Append text to json file.
                SaveFile(userFeedbacks, userFeedback, filePath);
            }
            catch (Exception ex)
            {
                this.ValidationErrors.Add(_helper.GetExceptionDetail(ex));
            }
            return this.ValidationErrors.Count() == 0;
        }

        private void SaveFile(List<UserFeedback> userFeedbacks, UserFeedback userFeedback, string filePath)
        {
            if(userFeedback!=null) {
            userFeedback.ID = Guid.NewGuid() + "." + Guid.NewGuid();
            userFeedback.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            userFeedbacks.Add(userFeedback);
            }
            var data = JsonConvert.SerializeObject(userFeedbacks);
            File.WriteAllText(filePath, data);
        }

        public async Task<bool> DeleteAsync(string filePath, string ID)
        {
            this.ValidationErrors = new ValidationErrors();
            try
            {
                var userFeedbacks = new List<UserFeedback>();
                // This text is added only once to the file.
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var data = await reader.ReadToEndAsync();
                        var photoSkipLogData = JsonConvert.DeserializeObject<List<UserFeedback>>(data);
                        if (photoSkipLogData is List<UserFeedback>)
                        {
                            var feedback = photoSkipLogData.FirstOrDefault(f=>f.ID==ID);
                            photoSkipLogData.Remove(feedback);
                            userFeedbacks = photoSkipLogData;
                        }
                    }
                }

                // Create/Append text to json file.
                SaveFile(userFeedbacks, null, filePath);
            }
            catch (Exception ex)
            {
                this.ValidationErrors.Add(_helper.GetExceptionDetail(ex));
            }
            return this.ValidationErrors.Count() == 0;
        }
    }
}