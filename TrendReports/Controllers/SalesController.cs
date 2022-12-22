using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendReports.Business;
using TrendReports.Models;
using System.Web.Mvc;

namespace TrendReports.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult Index()
        {
            var salesDataTotals = new SalesDataTotals();

            return View(salesDataTotals);
        }

        [HttpPost]
        public ActionResult Index(SalesDataTotals salesDataTotals)
        {

            salesDataTotals.SalesData = SqlQueries.GetSalesDataByDateRange(salesDataTotals.StartDate, salesDataTotals.EndDate,"ALL");

            if (salesDataTotals.CompareStartDate > new DateTime(2018, 1, 1) && salesDataTotals.CompareEndDate > new DateTime(2018, 1, 1))
            {
                salesDataTotals.CompareSalesData = SqlQueries.GetSalesDataByDateRange(salesDataTotals.CompareStartDate, salesDataTotals.CompareEndDate, "ALL");
            }
            //if(serviceDataTotals.Location != null && serviceDataTotals.Location != "" && serviceDataTotals.Location != "ALL")
            //{
            //    serviceDataTotals.ServiceData= serviceDataTotals.ServiceData.FindAll(x => serviceDataTotals.Location.Contains(x.Location));
            //    serviceDataTotals.CompareServiceData = serviceDataTotals.CompareServiceData.FindAll(x => serviceDataTotals.Location.Contains(x.Location));
            //}


            return View(salesDataTotals);
        }


        //public ActionResult SalesData(string startDate, string endDate, string dataPoint, string location)
        //{
        //    var salesDataTotals = new SalesDataTotals();

        //    salesDataTotals.StartDate = DateTime.Parse(startDate);
        //    salesDataTotals.EndDate = DateTime.Parse(endDate);
        //    salesDataTotals.Location = location;
        //    salesDataTotals.DataPoint = dataPoint;

        //    salesDataTotals.SalesData = SqlQueries.GetSalesDataByDateRange(salesDataTotals.StartDate, salesDataTotals.EndDate);

        //    if (location != null && location != "" && location != "ALL")
        //    {
        //        salesDataTotals.SalesData = salesDataTotals.SalesData.FindAll(x => location.Contains(x.Location));
        //    }

        //    return View(salesDataTotals);

        //}
    }
}