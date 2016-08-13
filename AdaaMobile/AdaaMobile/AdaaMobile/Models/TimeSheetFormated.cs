using System.Collections.Generic;

namespace AdaaMobile.Models
{
    public class TimeSheetFormated
    {
        public double LoggedInHours { get; set; }
        public double RemainingHours { get; set; }
        public List<Project> Projects { get; set; }

    }

    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double TotalHours { get; set; }
        public string TotalHoursFormated { get { return TotalHours.ToString() + " H"; } }
        public List<ProjectTask> Tasks { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }
    public class ProjectTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AssigmentName { get; set; }
        public DayWithLoggedInHours Day { get; set; }
        public bool CanEdit { get; set; }
    }

    public class DayWithLoggedInHours
    {
        public double Hours { get; set; }
        public string HoursFormated { get { return Hours.ToString() + " H"; } }
        public string DayName { get; set; }
        public string Comment { get; set; }
    }
}