using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LongestCoWorkingPeriod.Models
{
    public class EmploymentDataModel
    {
        public EmploymentDataModel(int empId, int prjId, DateTime from, DateTime to)
        {
            EmployeeId = empId;
            ProjectId = prjId;
            DateFrom = from;
            DateTo = to;
        }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}