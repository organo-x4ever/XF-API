using Organo.Solutions.X4Ever.V1.DAL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    public interface IWeightVolumeServices : IDefaultServices<WeightVolume>
    {
        List<WeightVolume> GetWeightVolumes(string applicationID);
        Task<List<WeightVolume>> GetWeightVolumesAsync(string applicationID);
    }
}