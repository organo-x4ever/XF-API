namespace Organo.Solutions.X4Ever.V1.API.Security.ErrorHelper
{
    using System.Net;

    /// <summary>
    /// IApiExceptions Interface
    /// </summary>
    public interface IApiExceptions
    {
        /// <summary>
        /// ErrorCode
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// ErrorDescription
        /// </summary>
        string ErrorDescription { get; set; }

        /// <summary>
        /// HttpStatus
        /// </summary>
        HttpStatusCode HttpStatus { get; set; }

        /// <summary>
        /// ReasonPhrase
        /// </summary>
        string ReasonPhrase { get; set; }
    }
}