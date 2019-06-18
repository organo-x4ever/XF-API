using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;
using Organo.Solutions.X4Ever.V1.DAL.Model.EnumerationTypes;
using Organo.Solutions.X4Ever.V1.DAL.Repository;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public class MenuServices : IMenuServices
    {
        private IUnitOfWork _unitOfWork;

        public MenuServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Delete(ref ValidationErrors validationErrors, Menu entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Menu> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Menu> Get(Expression<Func<Menu, bool>> filter = null,
            Func<IQueryable<Menu>, IOrderedQueryable<Menu>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public Menu Get(string ID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Menu>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Menu>> GetAsync(Expression<Func<Menu, bool>> filter = null,
            Func<IQueryable<Menu>, IOrderedQueryable<Menu>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public Task<Menu> GetAsync(string ID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Menu> GetByApplication(string applicationKey)
        {
            var appMenu = _unitOfWork.ApplicationMenuRepository.GetMany(a => a.ApplicationKey == applicationKey);
            return _unitOfWork.MenuRepository.GetMany(m => m.MenuActive && appMenu.Any(a => a.MenuID == m.ID));
        }

        public async Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey)
        {
            var appMenu =
                await _unitOfWork.ApplicationMenuRepository.GetManyAsync(a => a.ApplicationKey == applicationKey);
            return await _unitOfWork.MenuRepository.GetManyAsync(m =>
                m.MenuActive && appMenu.Any(a => a.MenuID == m.ID));
        }

        public IEnumerable<Menu> GetByApplication(string applicationKey, PlatformType platformType)
        {
            var platform = platformType.ToString();
            var appMenu =
                _unitOfWork.ApplicationMenuRepository.GetMany(a =>
                    a.ApplicationKey == applicationKey && (a.PlatformExcluded != null
                        ? !a.PlatformExcluded.ToLower().Contains(platform.ToLower())
                        : true));
            return _unitOfWork.MenuRepository.GetMany(m => m.MenuActive && appMenu.Any(a => a.MenuID == m.ID));
        }

        public async Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey, PlatformType platformType)
        {
            var platform = platformType.ToString();
            var appMenus = await _unitOfWork.ApplicationMenuRepository.GetManyAsync(a => a.ApplicationKey == applicationKey);
            var appMenu = appMenus.Where(a => (a.PlatformExcluded != null ? !a.PlatformExcluded.ToLower().Contains(platform.ToLower()) : true));
            return await _unitOfWork.MenuRepository.GetManyAsync(m => m.MenuActive && appMenu.Any(a => a.MenuID == m.ID));
        }

        public async Task<IEnumerable<Menu>> GetByApplicationAsync(string applicationKey, PlatformType platformType, int version)
        {
            var platform = platformType.ToString();
            var appMenus = await _unitOfWork.ApplicationMenuRepository.GetManyAsync(a => a.ApplicationKey == applicationKey);
            var appMenu = version > 300 ? appMenus.Where(a => a.MenuID != 9 ? (a.PlatformExcluded != null ? !a.PlatformExcluded.ToLower().Contains(platform.ToLower()) : true) : true)
                 : appMenus.Where(a => (a.PlatformExcluded != null ? !a.PlatformExcluded.ToLower().Contains(platform.ToLower()) : true));
            return await _unitOfWork.MenuRepository.GetManyAsync(m => m.MenuActive && appMenu.Any(a => a.MenuID == m.ID));
        }

        public bool Insert(ref ValidationErrors validationErrors, Menu entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(ref ValidationErrors validationErrors, Menu entity)
        {
            throw new NotImplementedException();
        }
    }
}