using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organo.Solutions.X4Ever.V1.DAL.Model;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IOpenNotificationUserServices
    {
        Task<IEnumerable<OpenNotificationUser>> GetAsync();
        Task<IEnumerable<OpenNotificationOnlyUser>> GetUserOnlyAsync();
        Task<IEnumerable<OpenNotificationOnlyUser>> GetUserOnlyAsync(string[] emailIDs, string[] platforms);
        Task<IEnumerable<OpenNotificationUserMap>> GetWeeklyAsync();
        Task<IEnumerable<OpenNotificationUserRegistrationMap>> GetRegisteredUserAfterWeekAsync();
    }
}