namespace MauiUserApp.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string DayOfWeek { get; set; }
        public string Time { get; set; }
        public int Duration { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}