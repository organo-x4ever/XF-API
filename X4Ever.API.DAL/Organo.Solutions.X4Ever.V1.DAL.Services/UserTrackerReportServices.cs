using System;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model.Statics;
using Organo.Solutions.X4Ever.V1.DAL.Helper;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class UserTrackerReportServices : IUserTrackerReportServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTrackerReportServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserTrackerReportV1Map>> GetAllAsync()
        {
            return await GetConvertedAsync(
                (from t in _unitOfWork.UserTrackerReportV1Repository.GetMany()
                 select new UserTrackerReportV1Map()
                 {
                     ID = t.ID,
                     UserID = t.UserID,
                     Address = t.Address,
                     City = t.City,
                     Country = t.Country,
                     EmailAddress = t.EmailAddress,
                     FirstName = t.FirstName,
                     FrontPhoto = t.FrontPhoto?.Clean(),
                     Gender = t.Gender,
                     LastName = t.LastName,
                     PostalCode = t.PostalCode,
                     SidePhoto = t.SidePhoto?.Clean(),
                     StartWeight = t.StartWeight,
                     State = t.State,
                     TShirtSize = t.TShirtSize,
                     Testimonials = t.Testimonials,
                     WeeklyWeightLost = t.WeeklyWeightLost,
                     WeightGoalReached = t.WeightGoalReached,
                     WeightToLose = t.WeightToLose,
                     ApplicationName = t.ApplicationName,
                     CreateDate = t.CreateDate,
                     WeightVolumeType = t.WeightVolumeType
                 }).ToList());
        }

        public async Task<IEnumerable<UserTrackerReportV1Map>> GetLatestAsync()
        {
            return await GetConvertedAsync(
                (from t in _unitOfWork.UserTrackerReportV1Repository.GetMany()
                 select new UserTrackerReportV1Map()
                 {
                     ID = t.ID,
                     UserID = t.UserID,
                     Address = t.Address,
                     City = t.City,
                     Country = t.Country,
                     EmailAddress = t.EmailAddress,
                     FirstName = t.FirstName,
                     FrontPhoto = t.FrontPhoto?.Clean(),
                     Gender = t.Gender,
                     LastName = t.LastName,
                     PostalCode = t.PostalCode,
                     SidePhoto = t.SidePhoto?.Clean(),
                     StartWeight = t.StartWeight,
                     State = t.State,
                     TShirtSize = t.TShirtSize,
                     Testimonials = t.Testimonials,
                     WeeklyWeightLost = t.WeeklyWeightLost,
                     WeightGoalReached = t.WeightGoalReached,
                     WeightToLose = t.WeightToLose,
                     ApplicationName = t.ApplicationName,
                     CreateDate = t.CreateDate,
                     WeightVolumeType = t.WeightVolumeType
                 }).ToList());
        }

        private async Task<IEnumerable<UserTrackerReportV1Map>> GetConvertedAsync(
            List<UserTrackerReportV1Map> list, bool dynamic = false)
        {
            var trackers =
                (await _unitOfWork.UserTrackerRepository.GetManyAsync(t => list.Any(u => u.UserID == t.UserID)))
                .ToList();
            foreach (var record in list)
            {
                record.UserTrackers = await ConvertToList(trackers.Where(u => u.UserID == record.UserID).ToList());
            }

            return list;
        }

        private async Task<List<UserTrackerMap>> ConvertToList(List<UserTracker> trackers)
        {
            var userTrackers = new List<UserTrackerMap>();
            if (trackers != null && trackers.Count > 0)
            {
                var trackerList = (from t in trackers
                                   group t by new { t.RevisionNumber }
                    into rn
                                   orderby rn.Key.RevisionNumber ascending
                                   select new
                                   {
                                       rn.Key.RevisionNumber,
                                       List = (from r in rn.ToList()
                                               select new UserTracker()
                                               {
                                                   AttributeLabel = r.AttributeLabel,
                                                   AttributeName = r.AttributeName,
                                                   AttributeValue = r.AttributeValue?.Clean(),
                                                   MediaLink = r.MediaLink,
                                                   ModifyDate = r.ModifyDate,
                                               }).Distinct().ToList()
                                   }).ToList();
                foreach (var tracker in trackerList)
                {
                    userTrackers.Add(await this.Convert(tracker.List, tracker.RevisionNumber));
                }
            }

            return userTrackers;
        }

        private async Task<UserTrackerMap> Convert(List<UserTracker> trackers, string revision = null)
        {
            if (trackers == null || trackers.Count == 0)
            {
                return null;
            }

            var userTracker = new UserTrackerMap();
            await Task.Run(() =>
            {
                var aboutYourJourney = trackers.FirstOrDefault(u =>
                    u.AttributeName.ToLower().Contains(TrackerConstants.ABOUT_JOURNEY));
                if (aboutYourJourney != null)
                    userTracker.AboutYourJourney = aboutYourJourney.AttributeValue;

                var currentWeight = trackers.FirstOrDefault(u =>
                    u.AttributeName.ToLower().Contains(TrackerConstants.CURRENT_WEIGHT));

                var frontImage =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.FRONT_IMAGE));
                if (frontImage != null)
                    userTracker.FrontImage = frontImage.AttributeValue?.Clean();

                var sideImage =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.SIDE_IMAGE));
                if (sideImage != null)
                    userTracker.SideImage = sideImage.AttributeValue?.Clean();

                if (revision == null || revision.Trim().Length == 0)
                {
                    var revisionNumber = trackers.FirstOrDefault();
                    if (revisionNumber != null)
                        userTracker.RevisionNumber = revisionNumber.RevisionNumber;
                }
                else
                    userTracker.RevisionNumber = revision;

                var modifyDate = trackers.FirstOrDefault();
                if (modifyDate != null)
                    userTracker.ModifyDate = modifyDate.ModifyDate;

                var shirtSize =
                    trackers.FirstOrDefault(u => u.AttributeName.ToLower().Contains(TrackerConstants.TSHIRT_SIZE));
                if (shirtSize != null)
                    userTracker.ShirtSize = shirtSize.AttributeValue;
            });
            return userTracker;
        }

        public async Task<IEnumerable<UserTrackerDetailReportV2>> GetTrackerDetailAsync(
            Func<IQueryable<UserTrackerDetailReportV2>, IOrderedQueryable<UserTrackerDetailReportV2>> orderBy = null)
        {
            return (from t in await _unitOfWork.UserTrackerDetailReportV2ReporRepository.GetManyAsync(null, orderBy)
                    select new UserTrackerDetailReportV2()
                    {
                        Address = t.Address,
                        State = t.State,
                        PostalCode = t.PostalCode,
                        Gender = t.Gender,
                        Country = t.Country,
                        City = t.City,
                        ApplicationName = t.ApplicationName,
                        CreateDate = t.CreateDate,
                        EmailAddress = t.EmailAddress,
                        FirstName = t.FirstName,
                        FrontPhoto = t.FrontPhoto.Clean(),
                        ID = t.ID,
                        LastName = t.LastName,
                        SidePhoto = t.SidePhoto.Clean(),
                        StartWeight = t.StartWeight,
                        Testimonials = t.Testimonials,
                        TrackerCreateDate = t.TrackerCreateDate,
                        TShirtSize = t.TShirtSize,
                        UserID = t.UserID,
                        WeeklyWeightLost = t.WeeklyWeightLost,
                        WeightGoalReached = t.WeightGoalReached,
                        WeightToLose = t.WeightToLose,
                        WeightVolumeType = t.WeightVolumeType
                    });
        }

        public async Task<IEnumerable<UserTrackerDetailReportV2>> GetTrackerDetailPeriodAsync(DateTime fromDate,
            DateTime toDate,
            Func<IQueryable<UserTrackerDetailReportV2>, IOrderedQueryable<UserTrackerDetailReportV2>> orderBy = null)
        {
            toDate = toDate.AddDays(1);
            return (from t in (await _unitOfWork.UserTrackerDetailReportV2ReporRepository.GetManyAsync(t =>
                    t.TrackerCreateDate >= fromDate && t.TrackerCreateDate <= toDate, orderBy))
                    select new UserTrackerDetailReportV2()
                    {
                        Address = t.Address,
                        State = t.State,
                        PostalCode = t.PostalCode,
                        Gender = t.Gender,
                        Country = t.Country,
                        City = t.City,
                        ApplicationName = t.ApplicationName,
                        CreateDate = t.CreateDate,
                        EmailAddress = t.EmailAddress,
                        FirstName = t.FirstName,
                        FrontPhoto = t.FrontPhoto.Clean(),
                        ID = t.ID,
                        LastName = t.LastName,
                        SidePhoto = t.SidePhoto.Clean(),
                        StartWeight = t.StartWeight,
                        Testimonials = t.Testimonials,
                        TrackerCreateDate = t.TrackerCreateDate,
                        TShirtSize = t.TShirtSize,
                        UserID = t.UserID,
                        WeeklyWeightLost = t.WeeklyWeightLost,
                        WeightGoalReached = t.WeightGoalReached,
                        WeightToLose = t.WeightToLose,
                        WeightVolumeType = t.WeightVolumeType
                    }).OrderByDescending(t => t.TrackerCreateDate);
        }
    }
}