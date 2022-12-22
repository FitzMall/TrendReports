using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load the data and then call stored procedure to update it...

            string path = @"Z:\DATAPUSH\Service\rocount.csv";

            if (File.Exists(path))
            {
                // Open the file to read from.
                string[] serviceRecords = File.ReadAllLines(path, Encoding.UTF8);
                //Console.WriteLine(serviceRecords);
                var index = 0;
                foreach (var record in serviceRecords)
                {
                    var columns = record.Split(',');

                    if (columns[0] != "\"SYS.STBR\"")
                    {
                        var serviceRecord = new ServiceRecord();

                        var branch = columns[0].Replace("\"","");
                        var location = "";

                        switch (branch)
                        {
                            case "02/01":
                                location = "FLP";
                                break;
                            case "04/01":
                                location = "LFO";
                                break;
                            case "05/01":
                                location = "LFT";
                                break;
                            case "06/01":
                                location = "FHT";
                                break;
                            case "07/01":
                                location = "FHG";
                                break;
                            case "09/01":
                                location = "FOC";
                                break;
                            case "10/01":
                                location = "FAM";
                                break;
                            case "11/01":
                                location = "WDC";
                                break;
                            case "12/01":
                                location = "CDO";
                                break;
                            case "13/01":
                                location = "FBS";
                                break;
                            case "16/01":
                                location = "FTN";
                                break;
                            case "17/01":
                                location = "CHY";
                                break;
                            case "17/02":
                                location = "CJE";
                                break;
                            default:
                                location = "XXX";
                                break;

                        }

                        try
                        {
                            serviceRecord.Location = location;
                            serviceRecord.CloseDate = DateTime.Parse(columns[1].Replace("\"", ""));
                            serviceRecord.RONumber = columns[2];
                            serviceRecord.ROCount = columns[3];
                            serviceRecord.CustomerAmount = decimal.Parse(columns[4]);
                            serviceRecord.WarrantyAmount = decimal.Parse(columns[5]);
                            serviceRecord.CustomerLaborAmount = decimal.Parse(columns[6]);
                            serviceRecord.WarrantyLaborAmount = decimal.Parse(columns[7]);
                        }
                        catch(Exception ex)
                        {
                            var wtf = ex.Message;
                        }
                    }
                    index += 1;
                }


            }

        }
    }

    public class ServiceRecord
    {
        public string Location { get; set; }
        public DateTime CloseDate { get; set; }
        public string RONumber { get; set; }
        public string ROCount { get; set; }
        public decimal CustomerAmount { get; set; }
        public decimal WarrantyAmount { get; set; }
        public decimal CustomerLaborAmount { get; set; }
        public decimal WarrantyLaborAmount { get; set; }
    }

}
