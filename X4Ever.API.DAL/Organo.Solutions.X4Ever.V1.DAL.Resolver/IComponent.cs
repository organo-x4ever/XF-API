namespace Organo.Solutions.X4Ever.V1.DAL.Resolver
{
    /// <summary>
    /// Register underlying types with unity.
    /// </summary>
    public interface IComponent
    {
        void SetUp(IRegisterComponent registerComponent);
    }
}