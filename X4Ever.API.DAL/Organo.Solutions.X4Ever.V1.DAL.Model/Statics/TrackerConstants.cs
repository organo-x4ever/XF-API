using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organo.Solutions.X4Ever.V1.DAL.Model.Statics
{
    public static class TrackerConstants
    {
        public static string LABEL => TrackerEnum.UserTracker.ToString();
        public static string CURRENT_WEIGHT => TrackerEnum.currentweight.ToString();
        public static string TSHIRT_SIZE => TrackerEnum.shirtsize.ToString();
        public static string FRONT_IMAGE => TrackerEnum.frontimage.ToString();
        public static string SIDE_IMAGE => TrackerEnum.sideimage.ToString();
        public static string ABOUT_JOURNEY => TrackerEnum.aboutjourney.ToString();
    }

    public enum TrackerEnum
    {
        UserTracker,
        currentweight,
        shirtsize,
        frontimage,
        sideimage,
        aboutjourney
    }
}