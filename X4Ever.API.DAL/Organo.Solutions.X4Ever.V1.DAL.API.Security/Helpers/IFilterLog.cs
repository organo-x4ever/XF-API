namespace Organo.Solutions.X4Ever.V1.API.Security.Helpers
{
    public interface IFilterLog
    {
        void Save(LogType logType, string[] values, string email);
    }
}