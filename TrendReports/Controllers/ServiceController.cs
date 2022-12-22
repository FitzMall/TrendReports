using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendReports.Business;
using TrendReports.Models;
using System.Web.Mvc;

namespace TrendReports.Controllers
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult Index()
        {
            var serviceDataTotals = new ServiceDataTotals();
            return View(serviceDataTotals);
        }

        [HttpPost]
        public ActionResult Index(ServiceDataTotals serviceDataTotals)
        {

            serviceDataTotals.ServiceData = SqlQueries.GetServiceDataByDateRange(serviceDataTotals.StartDate, serviceDataTotals.EndDate);

            if (serviceDataTotals.CompareStartDate > new DateTime(2018,1,1) && serviceDataTotals.CompareEndDate > new DateTime(2018,1,1))
            {
                serviceDataTotals.CompareServiceData = SqlQueries.GetServiceDataByDateRange(serviceDataTotals.CompareStartDate, serviceDataTotals.CompareEndDate);
            }
            //if(serviceDataTotals.Location != null && serviceDataTotals.Location != "" && serviceDataTotals.Location != "ALL")
            //{
            //    serviceDataTotals.ServiceData= serviceDataTotals.ServiceData.FindAll(x => serviceDataTotals.Location.Contains(x.Location));
            //    serviceDataTotals.CompareServiceData = serviceDataTotals.CompareServiceData.FindAll(x => serviceDataTotals.Location.Contains(x.Location));
            //}


            return View(serviceDataTotals);
        }


        //public ActionResult ServiceData(string startDate, string endDate, string dataPoint, string location)
        //{
        //    var serviceDateTotals = new ServiceDataTotals();

        //    serviceDateTotals.StartDate = DateTime.Parse(startDate);
        //    serviceDateTotals.EndDate = DateTime.Parse(endDate);
        //    serviceDateTotals.Location = location;
        //    serviceDateTotals.DataPoint = dataPoint;

        //    serviceDateTotals.ServiceData = SqlQueries.GetServiceDataByDateRange(serviceDateTotals.StartDate, serviceDateTotals.EndDate);

        //    if(location != null && location != "" && location != "ALL")
        //    {
        //        serviceDateTotals.ServiceData= serviceDateTotals.ServiceData.FindAll(x => location.Contains(x.Location));
        //    }

        //    return View(serviceDateTotals);
            
        //}

    }
}