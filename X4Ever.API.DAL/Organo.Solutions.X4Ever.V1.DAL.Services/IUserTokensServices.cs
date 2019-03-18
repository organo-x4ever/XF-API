using System;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Services
{
    using Organo.Solutions.X4Ever.V1.DAL.Model;

    public interface IUserTokensServices
    {
        #region Interface member methods.

        /// <summary>
        /// Function to generate unique token with expiry against the provided userId. Also add a
        /// record in database for generated token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        UserToken GenerateToken(long userId);

        /// <summary>
        /// Function to generate unique token with expiry against the provided userId. Also add a
        /// record in database for generated token.
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        Task<UserToken> GenerateTokenAsync(long userId);
        
        /// <summary>
        /// Function to validate token againt expiry and existance in database.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        long Validate(string tokenId);

        /// <summary>
        /// Function to validate token againt expiry and existance in database.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        bool ValidateToken(string tokenId);

        /// <summary>
        /// Function to validate token againt expiry and existance in database.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        Task<bool> ValidateTokenAsync(string tokenId);

        /// <summary>
        /// Function to get token detail after validating.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        UserToken GetDetailByToken(string tokenId);

        /// <summary>
        /// Function to get token detail after validating.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        /// <returns>
        /// </returns>
        Task<UserToken> GetDetailByTokenAsync(string tokenId);

        /// <summary>
        /// Method to update the provided token id.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        bool Update(string tokenId);

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">
        /// </param>
        bool Kill(string tokenId);

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId">
        /// </param>
        /// <returns>
        /// </returns>
        bool DeleteByUserId(long userId);

        /// <summary>
        /// Token will be valid till this datetime.
        /// </summary>
        /// <returns>
        /// </returns>
        DateTime TokenSessionTime();

        #endregion Interface member methods.
    }
}