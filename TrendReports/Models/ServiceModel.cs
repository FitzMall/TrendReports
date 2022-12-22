using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrendReports.Models
{

    public class SummaryModel
    {
        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string VehicleCondition { get; set; }
        public ServiceSummaryModel ServiceSummary { get; set; }
        public SalesSummaryModel SalesSummary { get; set; }
        //public PartsSummaryModel PartsSummary { get; set; }
    }
    public class ServiceSummaryModel
    {
        public ServiceSummary CurrentYearToDate { get; set; }
        public ServiceSummary PreviousYearToDate { get; set; }
        public ServiceSummary CurrentMonthToDate { get; set; }
        public ServiceSummary PreviousMonthToDate { get; set; }
        public ServiceSummary CurrentWeekToDate { get; set; }
        public ServiceSummary PreviousWeekToDate { get; set; }
        public ServiceSummary CurrentDate { get; set; }
        public ServiceSummary PreviousDate { get; set; }
    }

    public class ServiceSummary
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal CustomerAmount { get; set; }
        public decimal WarrantyAmount { get; set; }
        public decimal CustomerLaborAmount { get; set; }
        public decimal WarrantyLaborAmount { get; set; }
        public decimal InternalLaborAmount { get; set; }
        public decimal TotalLaborAmount { get; set; }
        public int RepairOrderCount { get; set; }
        public List<ServiceData> ServiceData { get; set; }
    }

    //public class ServiceDataTotals
    //{
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public string Location { get; set; }
    //    public string DataPoint { get; set; }
    //    public List<ServiceData> ServiceData { get; set; }

    //}

    public class ServiceDataTotals
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CompareStartDate { get; set; }
        public DateTime CompareEndDate { get; set; }
        public string[] SelectedLocations { get; set; }
        public string DataPoint { get; set; }
        public List<ServiceData> ServiceData { get; set; }
        public List<ServiceData> CompareServiceData { get; set; }
    }

    public class ServiceData
    {
        public string Location { get; set; }
        public DateTime CloseDate { get; set; }
        public string Prefix { get; set; }
        public string DayOfWeek { get; set; }
        public int WeekOfYear { get; set; }
        public decimal CustomerAmount { get; set; }
        public decimal WarrantyAmount { get; set; }
        public decimal CustomerLaborAmount { get; set; }
        public decimal WarrantyLaborAmount { get; set; }
        public int RepairOrderCount { get; set; }
        public decimal InternalLaborAmount { get; set; }
        public decimal TotalLaborAmount { get; set; }
    }

}