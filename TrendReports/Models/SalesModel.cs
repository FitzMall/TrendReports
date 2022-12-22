using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrendReports.Models
{
    public class SalesSummaryModel
    {
        public SalesSummary CurrentYearToDate { get; set; }
        public SalesSummary PreviousYearToDate { get; set; }
        public SalesSummary CurrentMonthToDate { get; set; }
        public SalesSummary PreviousMonthToDate { get; set; }
        public SalesSummary CurrentWeekToDate { get; set; }
        public SalesSummary PreviousWeekToDate { get; set; }
        public SalesSummary CurrentDate { get; set; }
        public SalesSummary PreviousDate { get; set; }
        public List<FactoryToDealerMoney> FactoryToDealerMoney { get; set; }
        public decimal PreviousMonthTotalDealCount { get; set; }
        public decimal PreviousYearTotalDealCount { get; set; }
    }

    public class SalesSummary
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime FiscalDate { get; set; }
        //public decimal FactoryToDealerMoney { get; set; }
        public decimal DealCount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal FinanceAmount { get; set; }
        public decimal ServiceContractAmount { get; set; }
        public decimal MaintenanceAmount { get; set; }
        public decimal ZurichAmount { get; set; }
        public decimal GapAmount { get; set; }
        public decimal BPPAmount { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal BackGrossAmount { get; set; }
        public decimal ServiceContractCount { get; set; }
        public decimal MaintenanceCount { get; set; }
        public decimal GapCount { get; set; }
        public decimal BPPCount { get; set; }
        public decimal FinanceCount {get; set;}
        public List<SalesData> SalesData { get; set; }
    }

    //public class SalesDataTotals
    //{
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public string Location { get; set; }
    //    public string DataPoint { get; set; }
    //    public List<SalesData> SalesData { get; set; }

    //}

    public class SalesDataTotals
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CompareStartDate { get; set; }
        public DateTime CompareEndDate { get; set; }
        public string[] SelectedLocations { get; set; }
        public string DataPoint { get; set; }
        public List<SalesData> SalesData { get; set; }
        public List<SalesData> CompareSalesData { get; set; }
    }

    public class SalesData
    {
        public string Location { get; set; }
        public DateTime DealDate { get; set; }
        public string DayOfWeek { get; set; }
        public int WeekOfYear { get; set; }
        public string Make { get; set; }
        public decimal DealCount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal FinanceAmount { get; set; }
        public decimal ServiceContractAmount { get; set; }
        public decimal MaintenanceAmount { get; set; }
        public decimal ZurichAmount { get; set; }
        public decimal GapAmount { get; set; }
        public decimal BPPAmount { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal BackGrossAmount { get; set; }
        public decimal ServiceContractCount { get; set; }
        public decimal MaintenanceCount { get; set; }
        public decimal GapCount { get; set; }
        public decimal BPPCount { get; set; }
        public decimal FinanceCount { get; set; }
    }

    public class FactoryToDealerMoney
    {
        public int MonthId {get;set;}
        public int YearId {get;set;}
        public string Location {get;set;}
        public string Brand {get;set;}
        public decimal FactoryToDealerAmount {get;set;}
        public decimal FactoryToDealerAmountUsed { get; set; }
        public DateTime InputDate {get;set;}

    }
    public class FiscalMonth
    {

        public int MonthId { get; set; }
        public int YearId { get; set; }
        public string LocationCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public int DaysInMonth { get; set; }
        public int CurrentDay { get; set; }
        public decimal Factor { get; set; }

    }
}