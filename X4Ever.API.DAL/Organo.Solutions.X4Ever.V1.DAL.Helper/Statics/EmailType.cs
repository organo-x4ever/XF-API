using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organo.Solutions.X4Ever.V1.DAL.Helper.Statics
{
    public enum EmailType
    {
        NEW_ACCOUNT = 0,
        WEIGHT_GOAL_SETUP = 1,
        AFTER_7_DAYS_ACCOUNT_CREATION = 3,
        AFTER_EVERY_7_DAYS = 4,
        LOSING_10_LBS = 5,
        LOSING_25_LBS = 6,
        LOSING_50_LBS = 7,
        LOSING_100_LBS = 8,
        ACHIEVED_GOAL = 9,
        FORGOT_PASSWORD = 10
    }
}