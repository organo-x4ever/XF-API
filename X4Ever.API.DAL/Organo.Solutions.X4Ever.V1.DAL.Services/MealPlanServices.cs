using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MealPlanServices : IMealPlanServices
    {
        private IUnitOfWork _unitOfWork;

        public MealPlanServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<MealPlan> Get()
        {
            return this._unitOfWork.MealPlanRepository.GetAll();
        }

        public async Task<IEnumerable<MealPlan>> GetAsync()
        {
            return await this._unitOfWork.MealPlanRepository.GetAllAsync();
        }

        public IEnumerable<MealPlan> GetDetail(string languageCode)
        {
            return GetDetailByCondition(languageCode, false, false);
        }

        public async Task<IEnumerable<MealPlan>> GetDetailAsync(string languageCode)
        {
            return await GetDetailByConditionAsync(languageCode, false, false);
        }

        public IEnumerable<MealPlan> GetDetail(string languageCode, bool active)
        {
            return GetDetailByCondition(languageCode, true, active);
        }

        public async Task<IEnumerable<MealPlan>> GetDetailAsync(string languageCode, bool active)
        {
            return await GetDetailByConditionAsync(languageCode, true, active);
        }

        private IEnumerable<MealPlan> GetDetailByCondition(string languageCode, bool activeRequired, bool active)
        {
            return (from mp in _unitOfWork.MealPlanRepository.GetMany(mp =>
                    mp.LanguageCode == languageCode && mp.Active == (activeRequired ? active : mp.Active))
                orderby mp.DisplaySequence
                select new MealPlan()
                {
                    ID = mp.ID,
                    LanguageCode = mp.LanguageCode,
                    ModifyDate = mp.ModifyDate,
                    DisplaySequence = mp.DisplaySequence,
                    Active = mp.Active,
                    MealPlanPhoto = mp.MealPlanPhoto,
                    MealTitle = mp.MealTitle,
                    ModifiedBy = mp.ModifiedBy,
                    ViewType = mp.ViewType,
                    MealPlanOptionDetails = (from mpo in _unitOfWork.MealPlanOptionRepository.GetMany(mpo =>
                            mpo.LanguageCode == languageCode && mpo.MealPlanID == mp.ID &&
                            mpo.Active == (activeRequired ? active : mpo.Active))
                        select new MealPlanOption()
                        {
                            ID = mpo.ID,
                            LanguageCode = mpo.LanguageCode,
                            ModifyDate = mpo.ModifyDate,
                            MealPlanID = mpo.MealPlanID,
                            Active = mpo.Active,
                            ModifiedBy = mpo.ModifiedBy,
                            MealOptionDesc = mpo.MealOptionDesc,
                            MealOptionPhoto = mpo.MealOptionPhoto,
                            MealOptionSubtitle = mpo.MealOptionSubtitle,
                            MealOptionTitle = mpo.MealOptionTitle,
                            MealPlanOptionGridDetails = (from mpog in _unitOfWork.MealPlanOptionGridRepository.GetMany(
                                    mpog => mpog.LanguageCode == languageCode && mpog.MealPlanOptionID == mpo.ID &&
                                            mpog.Active == (activeRequired ? active : mpog.Active))
                                select new MealPlanOptionGrid()
                                {
                                    ID = mpog.ID,
                                    LanguageCode = mpog.LanguageCode,
                                    ModifyDate = mpog.ModifyDate,
                                    Active = mpog.Active,
                                    ModifiedBy = mpog.ModifiedBy,
                                    MealPlanOptionID = mpog.MealPlanOptionID,
                                    MealOptionDetailPhoto = mpog.MealOptionDetailPhoto,
                                    MealOptionShakeTitle = mpog.MealOptionShakeTitle,
                                    MealOptionVolume = mpog.MealOptionVolume,
                                    MealOptionVolumeType = mpog.MealOptionVolumeType
                                }).ToList(),
                            MealPlanOptionListDetails = (from mpol in _unitOfWork.MealPlanOptionListRepository.GetMany(
                                    mpol =>
                                        mpol.LanguageCode == languageCode && mpol.MealPlanOptionID == mpo.ID &&
                                        mpol.Active == (activeRequired ? active : mpol.Active))
                                select new MealPlanOptionList()
                                {
                                    ID = mpol.ID,
                                    LanguageCode = mpol.LanguageCode,
                                    ModifyDate = mpol.ModifyDate,
                                    Active = mpol.Active,
                                    ModifiedBy = mpol.ModifiedBy,
                                    MealPlanOptionID = mpol.MealPlanOptionID,
                                    MealOptionDetailPhoto = mpol.MealOptionDetailPhoto,
                                    MealOptionDesc = mpol.MealOptionDesc,
                                    MealOptionDetail = mpol.MealOptionDetail
                                }).ToList()
                        }).ToList()
                }).ToList();
        }

        private async Task<IEnumerable<MealPlan>> GetDetailByConditionAsync(string languageCode, bool activeRequired,
            bool active)
        {
            return (from mp in await _unitOfWork.MealPlanRepository.GetManyAsync(mp =>
                    mp.LanguageCode == languageCode && mp.Active == (activeRequired ? active : mp.Active))
                orderby mp.DisplaySequence
                select new MealPlan()
                {
                    ID = mp.ID,
                    LanguageCode = mp.LanguageCode,
                    ModifyDate = mp.ModifyDate,
                    DisplaySequence = mp.DisplaySequence,
                    Active = mp.Active,
                    MealPlanPhoto = mp.MealPlanPhoto,
                    MealTitle = mp.MealTitle,
                    ModifiedBy = mp.ModifiedBy,
                    ViewType = mp.ViewType,
                    MealPlanOptionDetails = (from mpo in _unitOfWork.MealPlanOptionRepository.GetMany(mpo =>
                            mpo.LanguageCode == languageCode && mpo.MealPlanID == mp.ID &&
                            mpo.Active == (activeRequired ? active : mpo.Active))
                        select new MealPlanOption()
                        {
                            ID = mpo.ID,
                            LanguageCode = mpo.LanguageCode,
                            ModifyDate = mpo.ModifyDate,
                            MealPlanID = mpo.MealPlanID,
                            Active = mpo.Active,
                            ModifiedBy = mpo.ModifiedBy,
                            MealOptionDesc = mpo.MealOptionDesc,
                            MealOptionPhoto = mpo.MealOptionPhoto,
                            MealOptionSubtitle = mpo.MealOptionSubtitle,
                            MealOptionTitle = mpo.MealOptionTitle,
                            MealPlanOptionGridDetails = (from mpog in _unitOfWork.MealPlanOptionGridRepository.GetMany(
                                    mpog => mpog.LanguageCode == languageCode && mpog.MealPlanOptionID == mpo.ID &&
                                            mpog.Active == (activeRequired ? active : mpog.Active))
                                select new MealPlanOptionGrid()
                                {
                                    ID = mpog.ID,
                                    LanguageCode = mpog.LanguageCode,
                                    ModifyDate = mpog.ModifyDate,
                                    Active = mpog.Active,
                                    ModifiedBy = mpog.ModifiedBy,
                                    MealPlanOptionID = mpog.MealPlanOptionID,
                                    MealOptionDetailPhoto = mpog.MealOptionDetailPhoto,
                                    MealOptionShakeTitle = mpog.MealOptionShakeTitle,
                                    MealOptionVolume = mpog.MealOptionVolume,
                                    MealOptionVolumeType = mpog.MealOptionVolumeType
                                }).ToList(),
                            MealPlanOptionListDetails = (from mpol in _unitOfWork.MealPlanOptionListRepository.GetMany(
                                    mpol =>
                                        mpol.LanguageCode == languageCode && mpol.MealPlanOptionID == mpo.ID &&
                                        mpol.Active == (activeRequired ? active : mpol.Active))
                                select new MealPlanOptionList()
                                {
                                    ID = mpol.ID,
                                    LanguageCode = mpol.LanguageCode,
                                    ModifyDate = mpol.ModifyDate,
                                    Active = mpol.Active,
                                    ModifiedBy = mpol.ModifiedBy,
                                    MealPlanOptionID = mpol.MealPlanOptionID,
                                    MealOptionDetailPhoto = mpol.MealOptionDetailPhoto,
                                    MealOptionDesc = mpol.MealOptionDesc,
                                    MealOptionDetail = mpol.MealOptionDetail
                                }).ToList()
                        }).ToList()
                }).ToList();
        }
    }
}