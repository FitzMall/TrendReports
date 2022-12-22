using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrendReports.Models;
using TrendReports.Business;
using System.Web.Mvc;

namespace TrendReports.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var summaryModel = new SummaryModel();

            summaryModel.MonthId = DateTime.Now.Month;
            summaryModel.YearId = DateTime.Now.Year;
            summaryModel.VehicleCondition = "ALL";

            summaryModel.ServiceSummary = SqlQueries.GetServiceSummary(summaryModel.MonthId, summaryModel.YearId);
            summaryModel.SalesSummary = SqlQueries.GetSalesSummary(summaryModel.MonthId, summaryModel.YearId);

            return View(summaryModel);
        }

        [HttpPost]
        public ActionResult Index(SummaryModel summaryModel)
        {
            //summaryModel.MonthId = DateTime.Now.Month;
            //summaryModel.YearId = DateTime.Now.Year;
            //summaryModel.VehicleCondition = "ALL";

            summaryModel.ServiceSummary = SqlQueries.GetServiceSummary(summaryModel.MonthId, summaryModel.YearId);
            summaryModel.SalesSummary = SqlQueries.GetSalesSummary(summaryModel.MonthId, summaryModel.YearId,summaryModel.VehicleCondition);

            
            return View(summaryModel);
        }

    }
}