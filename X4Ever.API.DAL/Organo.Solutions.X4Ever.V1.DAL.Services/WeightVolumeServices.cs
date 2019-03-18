using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class WeightVolumeServices : IWeightVolumeServices, IValidation
    {
        private IUnitOfWork _unitOfWork;

        public WeightVolumeServices(UnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, WeightVolume entity)
        {
            this._unitOfWork.WeightVolumeRepository.Delete(entity);
            return this._unitOfWork.Commit();
        }

        public IEnumerable<WeightVolume> Get()
        {
            return this._unitOfWork.WeightVolumeRepository.GetAll();
        }
        public async Task<IEnumerable<WeightVolume>> GetAsync()
        {
            return await this._unitOfWork.WeightVolumeRepository.GetAllAsync();
        }

        public IEnumerable<WeightVolume> Get(Expression<Func<WeightVolume, bool>> filter = null,
            Func<IQueryable<WeightVolume>, IOrderedQueryable<WeightVolume>> orderBy = null,
            string includeProperties = "")
        {
            return this._unitOfWork.WeightVolumeRepository.GetMany(filter, orderBy, includeProperties);
        }
        
        public async Task<IEnumerable<WeightVolume>> GetAsync(Expression<Func<WeightVolume, bool>> filter = null, Func<IQueryable<WeightVolume>, IOrderedQueryable<WeightVolume>> orderBy = null, string includeProperties = "")
        {
            return await this._unitOfWork.WeightVolumeRepository.GetManyAsync(filter, orderBy, includeProperties);
        }

        public WeightVolume Get(string ID)
        {
            return this._unitOfWork.WeightVolumeRepository.GetByID(ID);
        }

        public async Task<WeightVolume> GetAsync(string ID)
        {
            return await this._unitOfWork.WeightVolumeRepository.GetByIDAsync(ID);
        }

        public List<WeightVolume> GetWeightVolumes(string applicationID)
        {
            return _unitOfWork.WeightVolumeRepository.GetMany(w =>
                    _unitOfWork.ApplicationRepository
                        .GetMany(a => a.ApplicationKey == applicationID && a.ID == w.ApplicationID).Any())
                .OrderBy(w => w.VolumeName).ToList();
        }

        public async Task<List<WeightVolume>> GetWeightVolumesAsync(string applicationID)
        {
            return (await _unitOfWork.WeightVolumeRepository.GetManyAsync(w =>
                    _unitOfWork.ApplicationRepository
                        .GetMany(a => a.ApplicationKey == applicationID && a.ID == w.ApplicationID).Any()))
                .OrderBy(w => w.VolumeName).ToList();
        }

        public bool Insert(ref ValidationErrors validationErrors, WeightVolume entity)
        {
            var success = false;
            dynamic[] obj = { entity };
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.WeightVolumeRepository.Insert(entity);
                bool result = _unitOfWork.Commit();
                if (result)
                {
                    success = true;
                }
            }

            return success;
        }

        public bool Update(ref ValidationErrors validationErrors, WeightVolume entity)
        {
            dynamic[] obj = { entity };
            if (this.Validate(ref validationErrors, obj))
            {
                _unitOfWork.WeightVolumeRepository.Update(entity);
                return _unitOfWork.Commit();
            }

            return false;
        }

        public bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue)
        {
            WeightVolume obj = (WeightVolume)objValue[0];
            if (obj.VolumeCode.Trim().Length == 0)
                validationErrors.Add("MessageVolumeCodeRequired");
            if (obj.VolumeName.Trim().Length == 0)
                validationErrors.Add("MessageVolumeNameRequired");
            return validationErrors.Count() == 0;
        }
    }
}