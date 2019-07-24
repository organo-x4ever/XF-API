
namespace Organo.Solutions.X4Ever.V1.DAL.Helper
{
    public static class CommonConstants
    {
        public static string YES => "yes";
        public static string NO => "no";
        public static string SPACE => "\n";
        public static double TargetDateCalculation => 0.33;

        public static string EMAIL_VALIDATION_REGEX =>
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static string Message => "message";
        public static string DATE_FORMAT_MMM_d_yyyy => "{0:MMM d, yyyy}";
        public static double KG_LB_CONVERT_VALUE => 2.20462262185;
        public static double KG_LB_CONVERT_VALUE_1 => 0.45359237;

        public static string FilterLogPath => "filterLogs";
        public static string AuthenticationFilterCollect => "authentication_filter:collect";
        public static string AuthenticationFilterEmails => "authentication_filter:identity";

        public static string LastTrackerDeleteOnly => "tracker:IsLastDeleteOnly";
        public static string TrackerDeleteAllowed => "tracker:IsDeleteAllowed";
        public static string IsRequireDeleted => "tracker:IsWeightRequiredAfterTrackerDelete";
        public static string TrackerViewAllowed => "tracker:IsViewAllowed";
        public static string TrackerDownloadAllowed => "tracker:IsDownloadAllowed";
        public static string TrackerWeightLoseWarning => "tracker:WeightLoseWarningPercent";
        public static string TrackerSkipPhotoOnSteps=>"tracker:SkipPhotoOnSteps";
        public static string RestOfTheLogs => "log:Rest";

        public static string X4EverBlogs => "weblink:x4ever_blogs";
        public static string X4EverBlogsSPA => "weblink:x4ever_blogs_es";
        public static string X4EverEMEABlogs => "weblink:x4ever_emeablogs";

        public static string X4EverWebMore => "weblink:x4ever_more";

        public static string FEEDBACK_DIR_PATH => "app_web:feedback";

        public static string AllowedUsersKey => "frontend:AllowedUsers";

        public static string ExcludingSubmitCurrentWeight => "excludingSubmitCurrentWeight";
        public static string WeightSubmitIntervalDays => "WeightSubmitIntervalDays";
        public static string UsernameLengthMin => "usernameLengthMin";
        public static string UsernameLengthMax => "usernameLengthMax";
        public static string PasswordLengthMin => "passwordLengthMin";
        public static string PasswordLengthMax => "passwordLengthMax";

        public static string WeightVolumeType => "weightvolumetype";
        public static string CurrentWeight => "currentweight";
        public static string CurrentWeightUI => "currentweight_ui";
        public static string ShirtSize => "shirtsize";
        public static string FrontImage => "frontimage";
        public static string SideImage => "sideimage";
        public static string AboutJourney => "aboutjourney";
        public static string EnglishUS => "en-US";
        public static string kilogram => "kg";
    }
}