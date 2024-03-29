﻿namespace Organo.Solutions.X4Ever.V1.API.Security.Helpers
{
    using System;
    using System.Web.Script.Serialization;

    public static class JSONHelper
    {
        #region Public extension methods.

        /// <summary>
        /// Extened method of object class, Converts an object to a json string.
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToJSON(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            try
            {
                return serializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                var msg = ex;
                return "";
            }
        }

        #endregion Public extension methods.
    }
}