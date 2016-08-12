using System.Collections.Generic;

namespace AdaaMobile.Models
{
    public class TimeSheetFormated
    {
        public string LoggedInHours { get; set; }
        public string RemainingHours { get; set; }
        public List<Project> Projects { get; set; }

    }

    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double TotalHours { get; set; }
        public List<ProjectTask> Tasks { get; set; }

    }
    public class ProjectTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DayWithLoggedInHours Day { get; set; }
        public bool CanEdit { get; set; }
    }

    public class DayWithLoggedInHours
    {
        public double Hours { get; set; }
        public string DayName { get; set; }
        public string Comment { get; set; }
    }
}