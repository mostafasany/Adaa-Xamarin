using System;
using System.Collections.Generic;

namespace AdaaMobile.Models.Response
{
    public class TimeSheet
    {
		public object EmpWorkingHours{ get; set; }
		public List<TimeSheetDetails> TimeSheetRecords { get; set; }
    }

	public class TimeSheetDetails
	{
		public string AssignmentDate { get; set; }
		public string TaskID { get; set; }
		public string AssignmentID { get; set; }
		public string TaskTitle { get; set; }
		public string SubTaskTo { get; set; }
		public string Sunday { get; set; }
		public string Monday { get; set; }
		public string Tuesday { get; set; }
		public string Wednesday { get; set; }
		public string Thursday { get; set; }
		public string SundayComment { get; set; }
		public string MondayComment { get; set; }
		public string TuesdayComment { get; set; }
		public string WednesdayComment { get; set; }
		public string ThursdayComment { get; set; }
		public string PlannedWork { get; set; }
		public string ActualWork { get; set; }
		public string RemainingWork { get; set; }
		public string GridIndex { get; set; }

	}
}
