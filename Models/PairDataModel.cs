using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LongestCoWorkingPeriod.Models
{
    public class PairDataModel
    {
        public PairDataModel(int firstEmployee, int secondEmployee, int days)
        {
            FirstEmployeeId = firstEmployee;
            SecondEmployeeId = secondEmployee;
            ProjectPeriods = new List<ProjectDataModel>();
            TotalDays = days;
        }
        public int FirstEmployeeId { get; set; }
        public int SecondEmployeeId { get; set; }
        public List<ProjectDataModel> ProjectPeriods { get; set; }
        public int TotalDays { get; set; }
    }
}