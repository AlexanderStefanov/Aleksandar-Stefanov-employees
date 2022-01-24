using LongestCoWorkingPeriod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LongestCoWorkingPeriod.Services
{
    public class LongestCoWorkingPairFinder
    {

        public PairDataModel FindLongestCoworkingPairFromCSVFile(HttpPostedFileBase file)
        {
            PairDataModel result = null;
            try
            {
                ReadEmploymentDataFromFile(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (employmentData.Count > 1)
            {
                employmentData.Sort((x, y) => DateTime.Compare(x.DateFrom, y.DateFrom));
                
                PairUpEmployeesToFindMaxPeriod();
                
                if (maxPair != (0, 0))
                {
                    result = pairsData[maxPair];
                }
            }
            return result;
        }
        #region private
        private void ReadEmploymentDataFromFile(HttpPostedFileBase file)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(file.InputStream))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        employmentData.Add(CSVLineToEmploymentData(reader.ReadLine()));
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        private EmploymentDataModel CSVLineToEmploymentData(string line)
        {
            var values = line.Split(',');
            int employeeId, projectId;
            DateTime dateFrom, dateTo;
            try
            {
                employeeId = Int32.Parse(values[0].Trim());
                projectId = Int32.Parse(values[1].Trim());
                dateFrom = DateTime.Parse(values[2].Trim()).Date;
                dateTo = values[3].Trim() == "NULL" ? DateTime.UtcNow.Date : DateTime.Parse(values[3].Trim()).Date;
                ValidateDates(dateFrom,dateTo);
            }
            catch
            {
                throw new Exception("Incorrect data!");
            }
            return new EmploymentDataModel(employeeId, projectId, dateFrom, dateTo);
        }

        private void ValidateDates(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
            {
                throw new Exception("Incorrect data!");
            }
        }

        private void PairUpEmployeesToFindMaxPeriod()
        {
            for (int i = 0; i < employmentData.Count - 1; i++)
            {
                for (int y = i; y < employmentData.Count; y++)
                {
                    var firstEmployee = employmentData.ElementAt(i);
                    var secondEmployee = employmentData.ElementAt(y);
                    var pairKey = GeneratePairKey(firstEmployee.EmployeeId, secondEmployee.EmployeeId);

                    if (firstEmployee.DateTo < secondEmployee.DateFrom)
                    {
                        break;
                    }
                    else if (firstEmployee.EmployeeId == secondEmployee.EmployeeId || firstEmployee.ProjectId != secondEmployee.ProjectId)
                    {
                        continue;
                    }
                    else
                    {
                        AddToPair(pairKey, secondEmployee.ProjectId, secondEmployee.DateFrom, GetEndTime(firstEmployee.DateTo, secondEmployee.DateTo));
                    }
                    UpdateMaxPair(pairKey);
                }
            }
            employmentData.Clear();
        }

        private (int, int) GeneratePairKey(int firstEmpId, int secondEmpId)
        {
            var keyOrder = new List<int>() { firstEmpId, secondEmpId };
            keyOrder.Sort();
            return (keyOrder[0], keyOrder[1]);
        }

        private void AddToPair((int, int) pairKey, int projectId, DateTime from, DateTime to)
        {
            if (!pairsData.ContainsKey(pairKey))
            {
                AddNewPair(pairKey, projectId, from, to);
            }
            else
            {
                AddToExistingPair(pairKey, projectId, from, to);
            }
        }

        private void AddToExistingPair((int, int) pairKey, int projectId, DateTime dateFrom, DateTime endTime)
        {
            pairsData[pairKey].TotalDays += (endTime - dateFrom).Days;
            pairsData[pairKey].ProjectPeriods.Add(new ProjectDataModel(projectId, dateFrom, endTime));
        }

        private void AddNewPair((int, int) pairKey, int projectId, DateTime dateFrom, DateTime endTime)
        {
            var pairData = new PairDataModel(pairKey.Item1, pairKey.Item2, (endTime - dateFrom).Days);
            var projectData = new ProjectDataModel(projectId, dateFrom, endTime);
            pairData.ProjectPeriods.Add(projectData);
            pairsData.Add(pairKey, pairData);
        }

        private DateTime GetEndTime(DateTime firstDateTo, DateTime secondDateTo)
        {
            return secondDateTo < firstDateTo ? secondDateTo : firstDateTo;
        }

        private void UpdateMaxPair((int, int) pairKey)
        {
            if (maxPair == (0, 0) || pairsData[maxPair].TotalDays < pairsData[pairKey].TotalDays)
            {
                maxPair = pairKey;
            }
        }

        private Dictionary<(int, int), PairDataModel> pairsData = new Dictionary<(int, int), PairDataModel>();
        private (int, int) maxPair = (0, 0);
        private static List<EmploymentDataModel> employmentData = new List<EmploymentDataModel>();
        #endregion
    }
}