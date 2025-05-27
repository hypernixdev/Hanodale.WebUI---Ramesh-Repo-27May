using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hanodale.SyncService;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace SyncAPIConsole
{
   
    internal class Program
    {
        static SyncManager syncManager = new SyncManager();
        static bool _processCustomer = false;
        static async Task Main(string[] args)
        {
            try
            {
                Scheduler _schItem;
                List<Scheduler> _listToRun = new List<Scheduler>();
                WriteToFile(DateTime.Now.ToString() + ": Sync Program Started");


                DataTable _dtConfig = GetSchedulerConfig();
                if (_dtConfig != null)
                {
                    //Get orders schedules
                    var _listOrder = _dtConfig.AsEnumerable()
                     .Where(row => row.Field<string>("syncModule") == "Orders")
                     .Select(row => new Scheduler
                     {
                         Id = row.Field<int>("id"),
                         SyncModule = row.Field<string>("syncModule"),
                         TimeSlot = row.Field<string>("timeSlot"),
                         LastProcessTime = row.Field<string>("LastProcessTime"),
                     })
                     .ToList(); // Optional: Convert to a List or an IEnumerable
                    _schItem = GetProcessStatus("Orders", _listOrder);
                    if (_schItem != null)
                        _listToRun.Add(_schItem);

                    //Get master module schedules
                    var _listMaster = _dtConfig.AsEnumerable()
                     .Where(row => row.Field<string>("syncModule") == "Master_Modules")
                     .Select(row => new Scheduler
                     {
                         Id = row.Field<int>("id"),
                         SyncModule = row.Field<string>("syncModule"),
                         TimeSlot = row.Field<string>("timeSlot"),
                         LastProcessTime = row.Field<string>("LastProcessTime"),
                     })
                     .ToList(); // Optional: Convert to a List or an IEnumerable
                    _schItem = GetProcessStatus("Master_Modules", _listMaster);
                    if (_schItem != null)
                        _listToRun.Add(_schItem);


                }

                if(_listToRun.Count == 0)
                {
                    WriteToFile(DateTime.Now.ToString() + ": No Schedule Found!");
                    return;
                }

                //process each task in scheduler list
                foreach (var item in _listToRun)
                {
                    // Add the sync tasks to the list
                    if (item.SyncModule == "Orders")
                    {
                        await SyncOrders(item);
                    }
                    else if (item.SyncModule == "Master_Modules")
                    {
                        await SyncMasterModules(item);
                    }
                    //await RunSyncTasks(_listToRun);  
                }
            }
            catch (Exception ex)
            {
                WriteToFile("Error! " + ex.Message);
            }

        }

        private static Scheduler GetProcessStatus(string module, List<Scheduler> schedules)
        {
            foreach (Scheduler item in schedules)
            {
                DateTime dateTimeScheduled = DateTime.Parse(item.TimeSlot);
                TimeSpan scheduledTime = dateTimeScheduled.TimeOfDay;
                if (DateTime.Now.TimeOfDay < scheduledTime)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(item.LastProcessTime))
                {
                    DateTime dateTimeProcessed = DateTime.Parse(item.LastProcessTime);
                    TimeSpan processedTime = dateTimeProcessed.TimeOfDay;
                    if (item.LastProcessTime != null)
                    {
                        if (scheduledTime <= processedTime)
                        continue;
                    }
                }
                    if (DateTime.Now.TimeOfDay >= scheduledTime
                        && DateTime.Now.TimeOfDay <= scheduledTime.Add(new TimeSpan(2, 0, 0))
                        )
                    {
                    return item;
                }
            }
            return null;
        }

        static async Task SyncMasterModules(Scheduler item)
        {
            DateTime _startDate = DateTime.Now;
            try
            {
                //start all modules
                SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Master Modules - Started");

                //Sync Customer
                var task2 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Customer Started");
                    var success2 = syncManager.SyncEntity("Customer", "", "code", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Customer Response: " + success2.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync CustomerGroupPriceList
                var task3 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync CustomerGroupPriceList Started");
                    var success3 = syncManager.SyncEntity("CustomerGroupPriceList", "CustomerGroupPriceList", "sysRowID", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync CustomerGroupPriceList Response: " + success3.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync Product
                var task4 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Product Started");
                    var success4 = syncManager.SyncEntity("Product", "", "partNumber", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Product Response: " + success4.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync UomConv
                var task5 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync UomConv Started");
                    var success5 = syncManager.SyncEntity("UomConv", "UomConv", "uniqueField", true, "UomConv");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync UomConv Response: " + success5.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync PriceList
                var task6 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync PriceList Started");
                    var success6 = syncManager.SyncEntity("PriceList", "PriceList", "sysRowID", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync PriceList Response: " + success6.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync CustomerPriceList
                var task7 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync CustomerPriceList Started");
                    var success7 = syncManager.SyncEntity("CustomerPriceList", "CustomerPriceList", "sysRowID", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync CustomerPriceList Response: " + success7.Message +
                        " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync Price List Part
                var task8 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Price List Part Started");
                    var success8 = syncManager.SyncEntity("PriceListPart", "PriceListParts", "sysRowID", false, "", pageNumber: 1);
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Price List Part Response: " + success8.Message +
                          " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync Stock Balance
                var task9 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Stock Balance List Started");
                    var success9 = syncManager.SyncEntity("StockBalance", "stockbalance", "uniqueField", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Stock Balance Response: " + success9.Message +
                         " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync Brand
                var task10 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Brand Started");
                    var success10 = syncManager.SyncEntity("Brand", "brands", "sysRowID", true, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Brand Response: " + success10.Message +
                         " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync District
                var task11 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync District Started");
                    var success11 = syncManager.SyncEntity("District", "districts", "sysRowID", true, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync District Response: " + success11.Message +
                         " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync ShipToAddress
                var task12 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync ShipToAddress Started");
                    var success12 = syncManager.SyncEntity("ShipToAddress", "shipTo", "shippingCode", false, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync ShipToAddress Response: " + success12.Message +
                         " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync Store
                var task13 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Store Started");
                    var success13 = syncManager.SyncEntity("Store", "plants", "plant", true, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Store Response: " + success13.Message +
                       " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });

                //Sync PaymentReport
                var task14 = Task.Run(() =>
                {
                    SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync PaymentReport Started");
                    var success14 = syncManager.SyncEntity("PaymentReport", "paymentreport", "paymentDate", true, "");
                    SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync PaymentReport Response: " + success14.Message +
                       " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                });


                // Wait for both tasks to complete
                Task.WaitAll(task2, task3, task4, task7, task9, task10, task11, task12, task13, task14, task5,task6, task8);

                //complete the task
                SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Master Modules Completed" +
                    " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));

            }
            catch (Exception ex)
            {
                TimeSpan _timeSpan = DateTime.Now - _startDate;
                SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Master Modules: " + ex.Message +
                    " | Time Taken: " + _timeSpan.ToString(@"hh\:mm\:ss"));
            }
        }

        static async Task SyncOrders(Scheduler item)
        {
            DateTime _startDate = DateTime.Now;
            try
            {
                SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Orders - Started");
                //Sync Orders
                var success1 = syncManager.SyncAllOrdersToEpicore();
                SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Orders Response: " + (success1 == true ? "Success" : "Failed") +
                   " | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
            }
            catch (Exception ex)
            {
                TimeSpan _timeSpan = DateTime.Now - _startDate;
                SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Orders: " + ex.Message +
                    " | Time Taken: " + _timeSpan.ToString(@"hh\:mm\:ss"));
            }
        }

        #region not used
        static async Task RunSyncTasks(List<Scheduler> scheduleList)
        {
            // Create a list of tasks to run in parallel
            List<Task> tasks = new List<Task>();
            DateTime  _startDate = DateTime.Now;

            foreach (var item in scheduleList)
            {
                // Add the sync tasks to the list
                if (item.SyncModule == "Orders")
                {
                    tasks.Add(SyncOrders(item));
                    // tasks.Add(SyncDistrict());
                }

                // Add the sync tasks to the list
                if (item.SyncModule == "Master_Modules")
                {
                    SyncMasterModules(item);
                    //SetScheduleLog(item.Id, _startDate, null, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync Master Modules Started");
                    //tasks.Add(SyncCustomer(item));
                    //tasks.Add(SyncDistrict(item));
                    //tasks.Add(SyncCustomerGroupPriceList(item));
                    //SetScheduleLog(item.Id, _startDate, DateTime.Now, "[" + DateTime.Now.ToString("dd/MM/yyyy hh:mm ttt") + "]: Sync  Master Modules Completed" + 
                    //" | Time Taken: " + (DateTime.Now - _startDate).ToString(@"hh\:mm\:ss"));
                    // tasks.Add(SyncDistrict());


                }

              
            }
            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
            WriteToFile("All tasks completed.");
        }

        private static void Main2(string[] args)
        {
            try
            {
                // Create tasks for each method call
                WriteToFile("Sync EpiCorePosting");
                // Sync Orders
                var task1 = Task.Run(() =>
                {
                    var success1 = syncManager.SyncAllOrdersToEpicore();
                    WriteToFile("Task1:EpiCorePosting" + " | " + (success1 == true ? "Success" : "Failed"));
                });

                var task2 = Task.Run(() =>
                {
                    WriteToFile("Sync Customer Started");
                    var success2 = syncManager.SyncEntity("Customer", "", "code", false, "");
                    WriteToFile("Task2:Customer" + " | " + success2.Message);
                });

                //Sync CustomerGroupPriceList
                var task3 = Task.Run(() =>
                {
                    var success3 = syncManager.SyncEntity("CustomerGroupPriceList", "CustomerGroupPriceList", "sysRowID", false, "");
                    WriteToFile("Task3:CustomerGroupPriceList" + " | " + success3.Message);
                });

                //Sync Product
                var task4 = Task.Run(() =>
                {
                    var success4 = syncManager.SyncEntity("Product", "", "partNumber", false, "");
                    WriteToFile("Task4:Product" + " | " + success4.Message);
                });

                //Sync UomConv
                var task5 = Task.Run(() =>
                {
                    var success5 = syncManager.SyncEntity("UomConv", "UomConv", "uniqueField", true, "UomConv");
                    WriteToFile("Task5:UomConv" + " | " + success5.Message);
                });

                //Sync PriceList
                var task6 = Task.Run(() =>
                {
                    var success6 = syncManager.SyncEntity("PriceList", "PriceList", "sysRowID", false, "");
                    WriteToFile("Task6:PriceList" + " | " + success6.Message);
                });

                //Sync CustomerPriceList
                var task7 = Task.Run(() =>
                {
                    var success7 = syncManager.SyncEntity("CustomerPriceList", "CustomerPriceList", "sysRowID", false, "");
                    WriteToFile("Task7:CustomerPriceList" + " | " + success7.Message);
                });

                //Sync Price List Part
                var task8 = Task.Run(() =>
                {
                    var success8 = syncManager.SyncEntity("PriceListPart", "PriceListParts", "sysRowID", false, "", pageNumber: 1);
                    WriteToFile("Task8:PriceListPart" + " | " + success8.Message);
                });

                //Sync Stock Balance
                var task9 = Task.Run(() =>
                {
                    var success9 = syncManager.SyncEntity("StockBalance", "stockbalance", "uniqueField", false, "");
                    WriteToFile("Task9:StockBalance" + " | " + success9.Message);
                });

                //Sync Brand
                var task10 = Task.Run(() =>
                {
                    var success10 = syncManager.SyncEntity("Brand", "brands", "sysRowID", true, "");
                    WriteToFile("Task10:Brand" + " | " + success10.Message);
                });

                //Sync District
                var task11 = Task.Run(() =>
                {
                    var success11 = syncManager.SyncEntity("District", "districts", "sysRowID", true, "");
                    WriteToFile("Task11:District" + " | " + success11.Message);
                });

                //Sync ShipToAddress
                var task12 = Task.Run(() =>
                {
                    var success12 = syncManager.SyncEntity("ShipToAddress", "shipTo", "shippingCode", false, "");
                    WriteToFile("Task12:ShipToAddress" + " | " + success12.Message);
                });

                var task13 = Task.Run(() =>
                {
                    var success13 = syncManager.SyncEntity("Store", "plants", "plant", true, "");
                    WriteToFile("Task13:Plant" + " | " + success13.Message);
                });

                var task14 = Task.Run(() =>
                {
                    var success14 = syncManager.SyncEntity("PaymentReport", "paymentreport", "paymentDate", true, "");
                    WriteToFile("Task14:PaymentReport" + " | " + success14.Message);
                });


                // Wait for both tasks to complete
                Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14);
                WriteToFile("All sync operations completed.");
                //WriteToFile("-------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                WriteToFile("Error! " + ex.Message);
            }
        }

        #endregion

        private static DataTable GetSchedulerConfig()
        {
            var tb = new DataTable();
            try
            {

                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                string sqlselect = "Sch_GetSettings";// ConfigurationManager.AppSettings["InspectionQuery"];

                var conn = new SqlConnection(connectionString);
                conn.Open();
                var cmd = new SqlCommand(sqlselect, conn);
                cmd.CommandTimeout = 3200;
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    tb.Load(dr);
                }
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message + " | " + ex.InnerException ?? "");
                throw ex;
            }
            return tb;
        }

        private static bool SetScheduleLog(int @scheduleId, DateTime startdate, DateTime? enddate, string result)
        {
            try
            {
                Console.WriteLine(result);
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                string sqlCmd = "Sch_AddToLog";
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Create a SqlCommand to execute the stored procedure
                    using (var cmd = new SqlCommand(sqlCmd, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 3200;

                        // Add parameters to the command
                        cmd.Parameters.AddWithValue("@scheduleId", @scheduleId);
                        cmd.Parameters.AddWithValue("@startDate", startdate);
                        cmd.Parameters.AddWithValue("@endDate", enddate);
                        cmd.Parameters.AddWithValue("@result", result);

                        // Execute the command to insert data
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        static DataTable ConvertToDataTable<T>(List<T> models)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Loop through all the properties            
            // Adding Column to our datatable
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            // Adding Row
            foreach (T item in models)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows  
                    values[i] = Props[i].GetValue(item, null);
                }
                // Finally add value to datatable  
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }


        public static void WriteToFile(string text)
        {
            Console.WriteLine(text);
            string path = ConfigurationManager.AppSettings["SyncLogPath"] + DateTime.Now.ToString("MMyyyy") + ".txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " - " + text);
                writer.Close();
            }
        }

    }


}
