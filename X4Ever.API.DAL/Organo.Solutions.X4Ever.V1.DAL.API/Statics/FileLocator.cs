
using Organo.Solutions.X4Ever.V1.DAL.Helper;
using System;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.API.Statics
{
    public class FileLocator
    {
        public static Helper.Helper HELPER = new Helper.Helper();
        private static string SKIP_PHOTO_FILE_NAME = HELPER.GetAppSetting(CommonConstants.TRACKER_SKIP_PHOTO_FILE_NAME);
        public static string TRACKER_PHOTO_SKIP_JSON_PATH = HttpContext.Current.Request.MapPath("~/" + HELPER.GetAppSetting(CommonConstants.RestOfTheLogs) + "/" + SKIP_PHOTO_FILE_NAME);
        public static string TODAY = $"{DateTime.Now:yyyy-MM-dd}";
        private static string FEEDBACK_FILE_NAME = HELPER.GetAppSetting(CommonConstants.FEEDBACK_FILE_NAME);
        public static string FEEDBACK_FILE_PATH = HttpContext.Current.Request.MapPath("~/" + HELPER.GetAppSetting(CommonConstants.FEEDBACK_DIR_PATH) + "/" + FEEDBACK_FILE_NAME);
        public static string EMAIL_LOG_PATH = HttpContext.Current.Request.MapPath("~/" + HELPER.GetAppSetting(CommonConstants.EMAIL_ERROR_LOG) + "/" + $"{TODAY}-debug.log");
        public static string DEBUG_LOG_PATH = HttpContext.Current.Request.MapPath("~/" + HELPER.GetAppSetting(CommonConstants.DEBUG_LOG) + "/" + $"{TODAY}-debug.log");
        public static string ERROR_LOG_PATH = HttpContext.Current.Request.MapPath("~/" + HELPER.GetAppSetting(CommonConstants.ERROR_LOG) + "/" + $"{TODAY}-exception.log");
    }
}