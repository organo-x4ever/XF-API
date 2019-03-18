
using Organo.Solutions.X4Ever.V1.DAL.Helper;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Model;
    using Repository;

    public class UserTrackerPivotServices : IUserTrackerPivotServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserTokensServices _tokensServices;
        private readonly IConverter _converter;

        public UserTrackerPivotServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _tokensServices = new UserTokensServices(unitOfWork);
            _converter = new Converter();
        }

        public TrackerPivot GetLatestTracker(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail == null)
                return null;
            return GetLatestTracker(tokenDetail.UserID);
        }

        public TrackerPivot GetLatestTracker(long userId)
        {
            var lastestTracker = _unitOfWork.UserTrackerRepository.GetLast(ut => ut.UserID == userId);
            return _unitOfWork.UserTrackerRepository.GetMany(ut =>
                    ut.UserID == userId && ut.RevisionNumber == lastestTracker?.RevisionNumber)
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    }).FirstOrDefault();
        }

        public async Task<TrackerPivot> GetLatestTrackerAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            if (tokenDetail == null)
                return null;
            return await GetLatestTrackerAsync(tokenDetail.UserID);
        }

        public async Task<TrackerPivot> GetLatestTrackerAsync(long userId)
        {
            var lastestTracker = _unitOfWork.UserTrackerRepository.GetLast(ut => ut.UserID == userId);
            return (await _unitOfWork.UserTrackerRepository.GetManyAsync(ut =>
                    ut.UserID == userId && ut.RevisionNumber == lastestTracker?.RevisionNumber))
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    }).FirstOrDefault();
        }

        public IEnumerable<TrackerPivot> GetTrackers(string token)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            if (tokenDetail == null)
                return null;
            return GetTrackers(tokenDetail.UserID);
        }

        public IEnumerable<TrackerPivot> GetTrackers(long userId)
        {
            return _unitOfWork.UserTrackerRepository.GetMany(ut => ut.UserID == userId)
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    });
        }

        public async Task<IEnumerable<TrackerPivot>> GetTrackersAsync(string token)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            if (tokenDetail == null)
                return null;
            return await GetTrackersAsync(tokenDetail.UserID);
        }

        public async Task<IEnumerable<TrackerPivot>> GetTrackersAsync(long userId)
        {
            return (await _unitOfWork.UserTrackerRepository.GetManyAsync(ut => ut.UserID == userId))
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    });
        }

        public async Task<IEnumerable<TrackerPivot>> GetTrackersAsync(long userId, string weightVolumeType)
        {
            var trackers = (await _unitOfWork.UserTrackerRepository.GetManyAsync(ut => ut.UserID == userId))
                .GroupBy(ut => new
                    {ut.UserID, ut.RevisionNumber, ut.ModifyDate.Year, ut.ModifyDate.Month, ut.ModifyDate.Day}).Select(
                    ut => new TrackerPivot
                    {
                        UserId = ut.Key.UserID,
                        RevisionNumber = ut.Key.RevisionNumber,
                        ModifyDate = new DateTime(ut.Key.Year, ut.Key.Month, ut.Key.Day),
                        WeightVolumeType =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "weightvolumetype")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        CurrentWeight =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "currentweight")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        CurrentWeightUI =
                            ut.Where(u =>
                                u.RevisionNumber == ut.Key.RevisionNumber &&
                                u.AttributeName.ToLower() == "currentweight_ui")?.FirstOrDefault()?.AttributeValue ??
                            "",
                        ShirtSize = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "shirtsize")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        FrontImage =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "frontimage")
                                ?.FirstOrDefault()?.AttributeValue ?? "",
                        SideImage = ut.Where(u =>
                                        u.RevisionNumber == ut.Key.RevisionNumber &&
                                        u.AttributeName.ToLower() == "sideimage")?.FirstOrDefault()?.AttributeValue ??
                                    "",
                        AboutJourney =
                            ut.Where(u =>
                                    u.RevisionNumber == ut.Key.RevisionNumber &&
                                    u.AttributeName.ToLower() == "aboutjourney")
                                ?.FirstOrDefault()?.AttributeValue ?? ""
                    });

            if (weightVolumeType.ToLower().Contains("lb"))
            {
                trackers = (from t in trackers
                    select new TrackerPivot()
                    {
                        UserId = t.UserId,
                        RevisionNumber = t.RevisionNumber,
                        ModifyDate = t.ModifyDate,
                        WeightVolumeType = t.WeightVolumeType,
                        CurrentWeight = t.CurrentWeightUI.Trim().Length == 0
                            ? _converter.ConvertKilogramToPound(t.CurrentWeight)
                            : t.CurrentWeightUI,
                        CurrentWeightUI = t.CurrentWeightUI,
                        ShirtSize = t.ShirtSize,
                        FrontImage = t.FrontImage,
                        SideImage = t.SideImage,
                        AboutJourney = t.AboutJourney
                    });
            }

            return trackers;
        }

        public async Task<IEnumerable<UserTracker>> GetByAttributeAsync(string token, string attributeName)
        {
            var tokenDetail = await _tokensServices.GetDetailByTokenAsync(token);
            if (tokenDetail == null)
                return null;
            return await GetByAttributeAsync(tokenDetail.UserID, attributeName);
        }

        public async Task<IEnumerable<UserTracker>> GetByAttributeAsync(long userId, string attributeName)
        {
            return (await _unitOfWork.UserTrackerRepository
                    .GetManyAsync(u => u.UserID == userId && u.AttributeName == attributeName))
                .OrderBy(u => u.ModifyDate).ToList();
        }

        public bool Insert(ref ValidationErrors validationErrors, string token, ICollection<UserTracker> entity)
        {
            var tokenDetail = _tokensServices.GetDetailByToken(token);
            return Insert(ref validationErrors, tokenDetail?.UserID ?? 0, entity);
        }

        public bool Insert(ref ValidationErrors validationErrors, long userId, ICollection<UserTracker> entity)
        {
            var success = false;
            short revisionNumber = 0;

            var latestTracker = _unitOfWork.UserTrackerRepository.GetMany(t => t.UserID == userId)
                .OrderByDescending(t => t.ModifyDate).FirstOrDefault();
            int.TryParse(entity?.FirstOrDefault()?.RevisionNumber ?? "0", out int rn);
            if (latestTracker != null)
            {
                double.TryParse(new Helper.Helper().GetAppSetting("WeightSubmitIntervalDays"), out double interval);
                DateTime.TryParse(latestTracker.ModifyDate.AddDays(interval).ToShortDateString(),
                    out DateTime submitDate);
                if (rn != 10000 && DateTime.Compare(submitDate, DateTime.Today) == 1)
                {
                    validationErrors.Add("MessageInvalidObject");
                    return false;
                }

                short.TryParse(latestTracker.RevisionNumber, out revisionNumber);
            }

            if (rn == 10000)
                revisionNumber = 0;

            revisionNumber++;
            foreach (var record in entity)
            {
                success = false;
                record.RevisionNumber = revisionNumber.ToString();
                record.UserID = userId;
                record.ModifyDate = DateTime.Now;
                dynamic[] obj = {record, false, true, false};
                if (Validate(ref validationErrors, obj))
                {

                    record.UserID = userId;
                    _unitOfWork.UserTrackerRepository.Insert(record);
                    bool result = _unitOfWork.Commit();
                    if (result)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            var userTokenRequired = (bool) objValue[1];
            string token = "";
            UserTracker obj = new UserTracker();
            if (userTokenRequired)
                token = objValue[0];
            else
                obj = (UserTracker) objValue[0];
            var basic = (bool) objValue[2];
            var basicValue = (bool) objValue[3];
            if (userTokenRequired)
                if (token.Trim().Length == 0)
                    validationErrors.Add("UserTokenRequired");
            if (basic)
                if (obj.AttributeName.Trim().Length == 0)
                    validationErrors.Add("AttributeNameRequired");
            if (basicValue)
                if (obj.AttributeValue.Trim().Length == 0)
                    validationErrors.Add("AttributeValueRequired");
            return validationErrors.Count() == 0;
        }
    }
}