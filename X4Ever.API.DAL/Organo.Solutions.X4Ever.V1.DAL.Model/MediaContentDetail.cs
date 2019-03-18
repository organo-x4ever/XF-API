using System;

namespace Organo.Solutions.X4Ever.V1.DAL.Model
{
    public sealed class MediaContentDetail
    {
        public MediaContentDetail()
        {
            ID = 0;
            MediaCategoryID = 0;
            MediaTypeID = 0;
            MediaTitle = string.Empty;
            MediaUrl = string.Empty;
            SetsAndRepeats = string.Empty;
            PreviewImageUrl = string.Empty;
            DisplaySequence = 0;
            MediaDescription = string.Empty;
            CreateDate = new DateTime();
            CategoryTitle = string.Empty;
            CategoryDescription = string.Empty;
            MediaTypeTitle = string.Empty;
            MediaTypeShortTitle = string.Empty;
            WorkoutLevel = string.Empty;
            WorkoutWeek = string.Empty;
            WorkoutDay = string.Empty;
            Active = false;
        }

        public int ID { get; set; }
        public Int16 MediaCategoryID { get; set; }
        public Int16 MediaTypeID { get; set; }
        public string MediaTitle { get; set; }
        public string MediaUrl { get; set; }
        public string SetsAndRepeats { get; set; }
        public string PreviewImageUrl { get; set; }
        public Int16 DisplaySequence { get; set; }
        public string MediaDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryDescription { get; set; }
        public string MediaTypeTitle { get; set; }
        public string MediaTypeShortTitle { get; set; }
        public string WorkoutLevel { get; set; }
        public string WorkoutWeek { get; set; }
        public string WorkoutDay { get; set; }
        public bool Active { get; set; }
    }
}