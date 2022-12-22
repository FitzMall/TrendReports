using System;
using System.Globalization;
using TrendReports.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace TrendReports.Business
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public class SqlQueries
    {

        public static ServiceSummary GetServiceSummary(DateTime startDate, DateTime endDate)
        {
            var summary = new ServiceSummary();
            summary.StartDate = startDate;
            summary.EndDate = endDate;

            var serviceSummary = SqlMapperUtil.StoredProcWithParams<ServiceSummary>("sp_TrendAnalysisGetServiceSummaryByDate", new { StartDate = startDate, EndDate = endDate }, "TrendAnalysis");
            
            if(serviceSummary != null && serviceSummary.Count > 0)
            {
                if (serviceSummary.Count > 1)
                {
                    var totalServiceSummary = new ServiceSummary();

                    totalServiceSummary.CustomerAmount = serviceSummary[0].CustomerAmount + serviceSummary[1].CustomerAmount;
                    totalServiceSummary.CustomerLaborAmount = serviceSummary[0].CustomerLaborAmount + serviceSummary[1].CustomerLaborAmount;
                    totalServiceSummary.InternalLaborAmount = serviceSummary[0].InternalLaborAmount + serviceSummary[1].InternalLaborAmount;
                    totalServiceSummary.RepairOrderCount = serviceSummary[0].RepairOrderCount + serviceSummary[1].RepairOrderCount;
                    totalServiceSummary.TotalLaborAmount = serviceSummary[0].TotalLaborAmount + serviceSummary[1].TotalLaborAmount;
                    totalServiceSummary.WarrantyAmount = serviceSummary[0].WarrantyAmount + serviceSummary[1].WarrantyAmount;
                    totalServiceSummary.WarrantyLaborAmount = serviceSummary[0].WarrantyLaborAmount + serviceSummary[1].WarrantyLaborAmount;
                    totalServiceSummary.StartDate = serviceSummary[0].StartDate;
                    totalServiceSummary.EndDate = serviceSummary[0].EndDate;
                    
                    summary = totalServiceSummary;
                }
                else
                {
                    summary = serviceSummary[0];
                }
                summary.StartDate = startDate;
                summary.EndDate = endDate;
            }

            return summary;

        }

        public static ServiceSummaryModel GetServiceSummary(int MonthId, int YearId)
        {

            var currentMonth = DateTime.Now.AddDays(-1).Month;
            var currentYear = DateTime.Now.AddDays(-1).Year;

            var serviceSummaryModel = new ServiceSummaryModel();

            var currentDate = DateTime.Now.AddDays(-1);
            var currentDatePreviousYear = currentDate.AddYears(-1);

            var startOfYear = new DateTime(currentDate.Year, 1, 1);
            var startOfPreviousYear = new DateTime(currentDate.AddYears(-1).Year, 1, 1);

            var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var startOfPreviousYearMonth = new DateTime(currentDate.AddYears(-1).Year, currentDate.Month, 1);


            if (currentMonth != MonthId || currentYear != YearId)
            {
                DateTime endOfMonth = new DateTime(YearId, MonthId, DateTime.DaysInMonth(YearId, MonthId));

                currentDate = endOfMonth;//new DateTime(YearId, MonthId, DateTime.Now.Day);

                currentDatePreviousYear = currentDate.AddYears(-1);

                startOfYear = new DateTime(YearId, 1, 1);
                startOfPreviousYear = new DateTime((YearId - 1), 1, 1);

                startOfMonth = new DateTime(YearId, MonthId, 1);
                startOfPreviousYearMonth = new DateTime(currentDate.AddYears(-1).Year, currentDate.Month, 1);
            }

            //var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            //var startOfPreviousYearWeek = currentDatePreviousYear.StartOfWeek(DayOfWeek.Sunday);

            //Now get all the calculations
            serviceSummaryModel.CurrentYearToDate = GetServiceSummary(startOfYear, currentDate);
            serviceSummaryModel.CurrentYearToDate.ServiceData = GetServiceDataByDateRange(startOfYear, currentDate);

            serviceSummaryModel.CurrentMonthToDate = GetServiceSummary(startOfMonth, currentDate);
            serviceSummaryModel.CurrentMonthToDate.ServiceData = GetServiceDataByDateRange(startOfMonth, currentDate);

            //serviceSummaryModel.CurrentWeekToDate = GetServiceSummary(startOfWeek, currentDate);
            serviceSummaryModel.CurrentDate = GetServiceSummary(currentDate, currentDate);
            serviceSummaryModel.CurrentDate.ServiceData = GetServiceDataByDateRange(currentDate, currentDate);

            serviceSummaryModel.PreviousYearToDate = GetServiceSummary(startOfPreviousYear, currentDatePreviousYear);
            serviceSummaryModel.PreviousYearToDate.ServiceData = GetServiceDataByDateRange(startOfPreviousYear, currentDatePreviousYear);

            serviceSummaryModel.PreviousMonthToDate = GetServiceSummary(startOfPreviousYearMonth, currentDatePreviousYear);
            serviceSummaryModel.PreviousMonthToDate.ServiceData = GetServiceDataByDateRange(startOfPreviousYearMonth, currentDatePreviousYear);

            //serviceSummaryModel.PreviousWeekToDate = GetServiceSummary(startOfPreviousYearWeek, currentDatePreviousYear);
            serviceSummaryModel.PreviousDate = GetServiceSummary(currentDatePreviousYear, currentDatePreviousYear);
            serviceSummaryModel.PreviousDate.ServiceData = GetServiceDataByDateRange(currentDatePreviousYear, currentDatePreviousYear);

            return serviceSummaryModel;
        }

        public static List<ServiceData> GetServiceDataByDateRange(DateTime startDate, DateTime endDate)
        {
            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var serviceData = SqlMapperUtil.StoredProcWithParams<ServiceData>("sp_TrendAnalysisGetServiceByDateRange", new { StartDate = startDate, EndDate = endDate }, "TrendAnalysis");

            foreach (var sale in serviceData)
            {
                var saleDate = sale.CloseDate;

                sale.DayOfWeek = saleDate.DayOfWeek.ToString();
                sale.WeekOfYear = myCal.GetWeekOfYear(saleDate, myCWR, myFirstDOW);
            }


            return serviceData;
        }



        public static SalesSummary GetSalesSummary(DateTime startDate, DateTime endDate, string vehicleCondition)
        {
            var summary = new SalesSummary();
            summary.StartDate = startDate;
            summary.EndDate = endDate;

            var salesSummary = SqlMapperUtil.StoredProcWithParams<SalesSummary>("sp_TrendAnalysisGetSalesSummaryByDate", new { StartDate = startDate, EndDate = endDate, Condition = vehicleCondition }, "TrendAnalysis");

            if (salesSummary != null && salesSummary.Count > 0)
            {
                summary = salesSummary[0];
                summary.StartDate = startDate;
                summary.EndDate = endDate;

            }
            return summary;
        }

        public static SalesSummary GetSalesMonthlySummary(DateTime startDate, DateTime endDate, DateTime fiscalMonthDate, string vehicleCondition)
        {
            var summary = new SalesSummary();
            summary.StartDate = startDate;
            summary.EndDate = endDate;

            var monthEndDate = startDate.AddDays(15);
            var salesSummary = SqlMapperUtil.StoredProcWithParams<SalesSummary>("sp_TrendAnalysisGetSalesMonthlySummaryByDate", new { StartDate = startDate, EndDate = endDate, MonthEndDate = fiscalMonthDate, Condition = vehicleCondition }, "TrendAnalysis");

            if (salesSummary != null && salesSummary.Count > 0)
            {
                summary = salesSummary[0];
                summary.StartDate = startDate;
                summary.EndDate = endDate;

            }
            return summary;
        }

        public static List<SalesData> GetSalesDataByDateRange(DateTime startDate, DateTime endDate, string vehicleCondition)
        {

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var salesData = SqlMapperUtil.StoredProcWithParams<SalesData>("sp_TrendAnalysisGetSalesByDateRange", new { StartDate = startDate, EndDate = endDate }, "TrendAnalysis");

            foreach(var sale in salesData)
            {
                var saleDate = sale.DealDate;

                sale.DayOfWeek = saleDate.DayOfWeek.ToString();
                sale.WeekOfYear = myCal.GetWeekOfYear(saleDate, myCWR, myFirstDOW);
            }


            return salesData;
        }

        public static List<SalesData> GetBrandSalesDataByDateRange(DateTime startDate, DateTime endDate, string vehicleCondition)
        {

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var salesData = SqlMapperUtil.StoredProcWithParams<SalesData>("sp_TrendAnalysisGetBrandSalesByDateRange", new { StartDate = startDate, EndDate = endDate, Condition = vehicleCondition }, "TrendAnalysis");

            foreach (var sale in salesData)
            {
                var saleDate = sale.DealDate;

                sale.DayOfWeek = saleDate.DayOfWeek.ToString();
                sale.WeekOfYear = myCal.GetWeekOfYear(saleDate, myCWR, myFirstDOW);
            }


            return salesData;
        }

        public static decimal GetPreviousYearTotalDealCount(DateTime startDate, DateTime fiscalMonthDate, string vehicleCondition)
        {
            decimal dealCount = 0;

            var salesCount = SqlMapperUtil.StoredProcWithParams<SalesData>("sp_TrendAnalysisGetSalesYearlyDealCountByDate", new { StartDate = startDate, MonthEndDate = fiscalMonthDate, Condition = vehicleCondition }, "TrendAnalysis");

            if (salesCount != null && salesCount.Count > 0)
            {
                dealCount = salesCount[0].DealCount;
            }

            return dealCount;
        }

        public static decimal GetPreviousMonthTotalDealCount(DateTime fiscalMonthDate, string vehicleCondition)
        {
            decimal dealCount = 0;

            var salesCount = SqlMapperUtil.StoredProcWithParams<SalesData>("sp_TrendAnalysisGetSalesMonthlyDealCountByDate", new { MonthEndDate = fiscalMonthDate, Condition = vehicleCondition }, "TrendAnalysis");

            if(salesCount != null && salesCount.Count > 0)
            {
                dealCount = salesCount[0].DealCount;
            }

            return dealCount;
        }


        public static List<SalesData> GetBrandSalesMonthlyDataByDateRange(DateTime startDate, DateTime endDate, DateTime fiscalMonthDate, string vehicleCondition)
        {

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            var monthEndDate = startDate.AddDays(15);

            var salesData = SqlMapperUtil.StoredProcWithParams<SalesData>("sp_TrendAnalysisGetBrandSalesMonthlyByDateRange", new { StartDate = startDate, EndDate = endDate, MonthEndDate = fiscalMonthDate, Condition = vehicleCondition }, "TrendAnalysis");

            foreach (var sale in salesData)
            {
                var saleDate = sale.DealDate;

                sale.DayOfWeek = saleDate.DayOfWeek.ToString();
                sale.WeekOfYear = myCal.GetWeekOfYear(saleDate, myCWR, myFirstDOW);
            }


            return salesData;
        }

        public static SalesSummaryModel GetSalesSummary(int MonthId, int YearId, string vehicleCondition = "ALL")
        {

            var currentMonth = DateTime.Now.AddDays(-1).Month;
            var currentYear = DateTime.Now.AddDays(-1).Year;

            var salesSummaryModel = new SalesSummaryModel();
            salesSummaryModel.FactoryToDealerMoney = GetFactoryToDealerMoney();

            var currentDate = DateTime.Now.AddDays(-1);
            var currentDatePreviousYear = currentDate.AddYears(-1);

            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var startOfPreviousYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);

            var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var startOfPreviousYearMonth = new DateTime(currentDate.AddYears(-1).Year, currentDate.Month, 1);

            //var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            //var startOfPreviousYearWeek = currentDatePreviousYear.StartOfWeek(DayOfWeek.Sunday);

            if(currentMonth != MonthId || currentYear != YearId)
            {
                DateTime endOfMonth = new DateTime(YearId, MonthId, DateTime.DaysInMonth(YearId,MonthId));

                currentDate = endOfMonth;//new DateTime(YearId, MonthId, DateTime.Now.Day);
                currentDatePreviousYear = currentDate.AddYears(-1);

                startOfYear = new DateTime(YearId, 1, 1);
                startOfPreviousYear = new DateTime((YearId-1), 1, 1);

                startOfMonth = new DateTime(YearId,MonthId, 1);
                startOfPreviousYearMonth = new DateTime(currentDate.AddYears(-1).Year, currentDate.Month, 1);
            }

            var fiscalMonths = SqlQueries.GetCurrentFiscalMonth(currentDate);
            var fiscalMonthDate = new DateTime();
            var fiscalMonthEndDate = new DateTime();
            if(fiscalMonths != null)
            {
                var currentFiscal = fiscalMonths[0];
                fiscalMonthDate = new DateTime(currentFiscal.YearId, currentFiscal.MonthId, 5);
                fiscalMonthEndDate = currentFiscal.EndDate;
            }

            //Now get all the calculations
            salesSummaryModel.CurrentYearToDate = GetSalesSummary(startOfYear, currentDate, vehicleCondition);
            salesSummaryModel.CurrentYearToDate.SalesData = GetBrandSalesDataByDateRange(startOfYear, currentDate, vehicleCondition);
            //salesSummaryModel.CurrentYearToDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(startOfYear, currentDate);

            if (currentMonth != MonthId || currentYear != YearId)
            {
                salesSummaryModel.CurrentMonthToDate = GetSalesMonthlySummary(startOfMonth, fiscalMonthEndDate, fiscalMonthDate, vehicleCondition);
                salesSummaryModel.CurrentMonthToDate.SalesData = GetBrandSalesMonthlyDataByDateRange(startOfMonth, fiscalMonthEndDate, fiscalMonthDate, vehicleCondition);

            }
            else
            {
                salesSummaryModel.CurrentMonthToDate = GetSalesMonthlySummary(startOfMonth, currentDate, fiscalMonthDate, vehicleCondition);
                salesSummaryModel.CurrentMonthToDate.SalesData = GetBrandSalesMonthlyDataByDateRange(startOfMonth, currentDate, fiscalMonthDate, vehicleCondition);
            }

            salesSummaryModel.CurrentMonthToDate.FiscalDate = fiscalMonthDate;
            //salesSummaryModel.CurrentMonthToDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(startOfMonth, currentDate);

            //salesSummaryModel.CurrentWeekToDate = GetSalesSummary(startOfWeek, currentDate);

            salesSummaryModel.CurrentDate = GetSalesSummary(currentDate, currentDate, vehicleCondition);
            salesSummaryModel.CurrentDate.SalesData = GetBrandSalesDataByDateRange(currentDate, currentDate, vehicleCondition);
            //salesSummaryModel.CurrentDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(currentDate, currentDate);

            var previousYearMonthFiscalDate = fiscalMonthDate.AddYears(-1);


            salesSummaryModel.PreviousYearToDate = GetSalesSummary(startOfPreviousYear, currentDatePreviousYear, vehicleCondition);
            salesSummaryModel.PreviousYearToDate.SalesData = GetBrandSalesDataByDateRange(startOfPreviousYear, currentDatePreviousYear, vehicleCondition);
            salesSummaryModel.PreviousYearTotalDealCount = GetPreviousYearTotalDealCount(startOfPreviousYear, currentDatePreviousYear, vehicleCondition);
            //salesSummaryModel.PreviousYearToDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(startOfPreviousYear, currentDatePreviousYear);

            salesSummaryModel.PreviousMonthToDate = GetSalesMonthlySummary(startOfPreviousYearMonth, currentDatePreviousYear, previousYearMonthFiscalDate, vehicleCondition);
            salesSummaryModel.PreviousMonthToDate.SalesData = GetBrandSalesMonthlyDataByDateRange(startOfPreviousYearMonth, currentDatePreviousYear, previousYearMonthFiscalDate, vehicleCondition);
            salesSummaryModel.PreviousMonthToDate.FiscalDate = previousYearMonthFiscalDate;
            salesSummaryModel.PreviousMonthTotalDealCount = GetPreviousMonthTotalDealCount(previousYearMonthFiscalDate, vehicleCondition);
            //salesSummaryModel.PreviousMonthToDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(startOfPreviousYearMonth, currentDatePreviousYear);

            //salesSummaryModel.PreviousWeekToDate = GetSalesSummary(startOfPreviousYearWeek, currentDatePreviousYear);

            salesSummaryModel.PreviousDate = GetSalesSummary(currentDatePreviousYear, currentDatePreviousYear, vehicleCondition);
            salesSummaryModel.PreviousDate.SalesData = GetBrandSalesDataByDateRange(currentDatePreviousYear, currentDatePreviousYear, vehicleCondition);
            //salesSummaryModel.PreviousDate.FactoryToDealerMoney = GetFactoryToDealerMoneyByDateRange(currentDatePreviousYear, currentDatePreviousYear);

            return salesSummaryModel;
        }

        public static List<FactoryToDealerMoney> GetFactoryToDealerMoney()
        {

            var sql = @"Select
              sl_MonthId as MonthId,
              sl_YearId as YearId,
              sl_StoreID as Location,
              sl_BrandID as Brand,
              [sl_FTD] as FactoryToDealerAmount,
            [sl_FTDCPO] as FactoryToDealerAmountUsed,
              sl_DateCreated as InputDate
              from[SalesCommission].[dbo].[SalesLog_FTD] where sl_YearID >= 2019 order by sl_YearId, sl_MonthId";

            var ftdMoney = SqlMapperUtil.SqlWithParams<FactoryToDealerMoney>(sql, new { }, "SalesCommission");

            return ftdMoney;
        }

        public static List<FiscalMonth> GetCurrentFiscalMonth(DateTime currentDate)
        {

            var fiscalMonth = SqlMapperUtil.SqlWithParams<FiscalMonth>("Select * FROM [SalesCommission].[dbo].[FiscalMonths] where convert(date,@CurrentDate) >= StartDate and convert(date,@CurrentDate) <= EndDate", new { CurrentDate= currentDate }, "SalesCommission");

            return fiscalMonth;

        }

    }

}