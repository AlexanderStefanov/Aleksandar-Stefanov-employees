using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LongestCoWorkingPeriod.Models
{
    public class ProjectDataModel
    {
        public ProjectDataModel(int id, DateTime from, DateTime to)
        {
            ProjectId = id;
            DateFrom = from;
            DateTo = to;
        }
        public int ProjectId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}