namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public interface IValidation
    {
        bool Validate(ref ValidationErrors validationErrors, dynamic[] objValue);
    }
}