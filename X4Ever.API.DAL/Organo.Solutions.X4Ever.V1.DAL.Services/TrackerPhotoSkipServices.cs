using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class TrackerPhotoSkipServices : ITrackerPhotoSkipServices
    {
        private Helper.IHelper _helper;
        public TrackerPhotoSkipServices()
        {
            _helper = new Helper.Helper();
        }

        public ValidationErrors ValidationErrors { get; set; }

        public async Task<int> CountByUserIDAsync(string filePath, long userID)
        {
            var result = await GetByUserIDAsync(filePath, userID);
            return result.Count;
        }

        public async Task<List<TrackerPhotoSkip>> GetAllAsync(string filePath)
        {
            var data = await GetJsonAsync(filePath);
            var photoSkipLogData = JsonConvert.DeserializeObject<List<TrackerPhotoSkip>>(data);
            if (photoSkipLogData is List<TrackerPhotoSkip>)
            {
                return photoSkipLogData;
            }
            return new List<TrackerPhotoSkip>();
        }

        public async Task<List<TrackerPhotoSkip>> GetByDateAsync(string filePath, DateTime date)
        {
            var response = await this.GetAllAsync(filePath);
            return response.Where(f => Convert.ToDateTime(f.modify_date) == date).ToList();
        }

        public async Task<List<TrackerPhotoSkip>> GetByDateRangeAsync(string filePath, DateTime fromDate, DateTime toDate)
        {
            var response = await this.GetAllAsync(filePath);
            return response.Where(f => Convert.ToDateTime(f.modify_date) > fromDate && Convert.ToDateTime(f.modify_date) <= toDate).ToList();
        }

        public async Task<List<TrackerPhotoSkip>> GetByUserIDAsync(string filePath, long userID)
        {
            var response = await this.GetAllAsync(filePath);
            return response.Where(f => f.user_id == userID).ToList();
        }

        public async Task<string> GetJsonAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            return string.Empty;
        }

        public async Task<bool> SaveAsync(string filePath, TrackerPhotoSkip trackerPhotoSkip)
        {
            if (trackerPhotoSkip != null)
            {
                var photoSkipLogs = new List<TrackerPhotoSkip>();
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        var data = await reader.ReadToEndAsync();
                        var photoSkipLogData = JsonConvert.DeserializeObject<List<TrackerPhotoSkip>>(data);
                        if (photoSkipLogData is List<TrackerPhotoSkip>)
                        {
                            photoSkipLogs = photoSkipLogData;
                        }
                    }
                }

                return SaveFile(filePath, photoSkipLogs, trackerPhotoSkip);
            }
            return false;
        }

        public async Task<bool> SaveAsync(string filePath, long userID, string token, string base64String)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                var optionList = Encoding.Default.GetString(Convert.FromBase64String(base64String));
                var list = optionList.Split(':');
                if (list.Count() == 2)
                {
                    var photoSkipLogs = new List<TrackerPhotoSkip>();
                    if (File.Exists(filePath))
                    {
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            var data = await reader.ReadToEndAsync();
                            var photoSkipLogData = JsonConvert.DeserializeObject<List<TrackerPhotoSkip>>(data);
                            if (photoSkipLogData is List<TrackerPhotoSkip>)
                            {
                                photoSkipLogs = photoSkipLogData;
                            }
                        }
                    }
                    bool.TryParse(list?[1] ?? "0", out bool skip);
                    var trackerPhotoSkip = new TrackerPhotoSkip()
                    {
                        user_id = userID,
                        user_email = list[0],
                        skip_photo = skip,
                        user_token = token,
                        modify_date = ""
                    };

                    return SaveFile(filePath, photoSkipLogs, trackerPhotoSkip);
                }
            }
            return false;
        }

        private bool SaveFile(string filePath, List<TrackerPhotoSkip> photoSkipLogs, TrackerPhotoSkip trackerPhotoSkip)
        {
            ValidationErrors = new ValidationErrors();
            try
            {
                var dateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                trackerPhotoSkip.modify_date = dateString;
                photoSkipLogs.Add(trackerPhotoSkip);
                var data = JsonConvert.SerializeObject(photoSkipLogs);
                File.WriteAllText(filePath, data);
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(_helper.GetExceptionDetail(ex));
            }

            return ValidationErrors.Count() == 0;
        }
    }
}