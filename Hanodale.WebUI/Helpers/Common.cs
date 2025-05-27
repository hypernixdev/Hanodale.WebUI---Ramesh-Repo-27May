using Hanodale.WebUI.Models;
using System;
using System.IO;
using System.Web.Mvc;
using Hanodale.BusinessLogic;
using System.ServiceModel;
using Hanodale.Utility.Globalize;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using CrystalDecisions.Shared;
using System.ComponentModel;
using Hanodale.WebUI.Logging.Elmah;
using System.Data;
using System.Data.OleDb;
using System.Runtime.Remoting.Messaging;

namespace Hanodale.WebUI.Helpers
{
    public class Common
    {
        public readonly static string emailAddressPattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
        public readonly static string dataFormatString = "{0:dd/MM/yyyy}";
        public readonly static string defaultSystemLanguageCulture = "en";
        public readonly static string cookieCultrueName = "culture";

        public static string GetPath(string folderName)
        {
            string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
            if (APP_PATH == "/")      //a site
                APP_PATH = "/";
            else if (!APP_PATH.EndsWith(@"/")) //a virtual
                APP_PATH += @"/";

            string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
            if (!it.EndsWith(@"\"))
                it += @"\";

            //return it;
            string subPath = folderName;// "Images_Repository\\"; // your code goes here
            string path = it + subPath;
            return path;
        }
        /// <summary>
        /// This method is to get the partial view from given controller view
        /// </summary>
        /// <param name="controller">Controller Name</param>
        /// <param name="viewName">View Name</param>
        /// <param name="model">optional - can be null</param>
        /// <returns>string</returns>
        /// 
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Message);
            }
        }

        /// <summary>
        /// This method is to get the access rights for the given page
        /// </summary>
        /// <returns>AccessRights</returns>
        public static AccessRightsModel GetUserRights(int userId, string pageUrl)
        {
            AccessRightsModel _userRights = new AccessRightsModel();
            try
            {
                IUserRightsService svcUserRight = new BusinessLogic.UserRightsService();

                var userRights = svcUserRight.GetUserAccess(userId, pageUrl);

                if (userRights != null)
                {
                    _userRights.canView = userRights.canView;
                    _userRights.canAdd = userRights.canAdd;
                    _userRights.canEdit = userRights.canEdit;
                    _userRights.canDelete = userRights.canDelete;

                    if (userRights.subMenus != null)
                    {
                        _userRights.pageName = userRights.subMenus.subMenuName;
                        _userRights.pageId = userRights.subMenus.id;
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user rights");
            }
            return _userRights;
        }

        public static Stream GetPDFFile(string FilePath, string ApplicationPath, string subCostCenterId, string UserId)
        {
            Stream stPdf = null;
            if (System.IO.File.Exists(ApplicationPath))
            {
                System.IO.File.Delete(ApplicationPath);
            }
            System.IO.File.Copy(FilePath, ApplicationPath);
            ReportDocument crystalReport = new ReportDocument(); // creating object of crystal report
            crystalReport.Load(ApplicationPath); // path of report 
            crystalReport.SetParameterValue("SubcostcenterId", subCostCenterId);
            crystalReport.SetParameterValue("UserId", UserId);
            crystalReport.SetDatabaseLogon(ConfigurationManager.AppSettings["username"], ConfigurationManager.AppSettings["password"], ConfigurationManager.AppSettings["DBServer"], ConfigurationManager.AppSettings["Database"]);
            //crystalReport.SetDataSource(dt); // binding datatable
            stPdf = crystalReport.ExportToStream(ExportFormatType.PortableDocFormat);
            System.IO.File.Delete(ApplicationPath);
            return stPdf;
        }

        public static string GetEnumDescription(Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public enum Status
        {
            Success,
            Error,
            Warning,
            Denied
        }

        public enum RecordStatus
        {
            Active,
            InActive
        }

        public enum AdminStatus
        {
            Admin,
            User
        }

        public enum ChartDashboardType
        {
            request,
            priority,
            ontime,
            overdue,
            rootcause,
            assettype,
            section
        }

        public enum PurchaseChartDashboardType
        {
            request,
            prService,
            prStock,
            prWorkCategory,
        }

        public enum RFQChartDashboardType
        {
            rfq,
            released,
            recalled,
            newRFQ,
            closed,
        }

        public enum InspectionChartDashboardType
        {
            totalToiletInspection,
            totalInspection,
            totalTickets,
            toiletComplianceMHB,
            toiletComplianceSAT,
            toiletComplianceCP,
            toiletNonComplianceMHB,
            toiletNonComplianceSAT,
            toiletNonComplianceCP,
        }

        public enum InspectionChartDashboardSectionType
        {
            compliance,
            nonCompliance,
            none
        }

        public enum TicketChartDashboardType
        {
            ticketStatus,
            totalTicketByCRFSS,
            totalTickets,
            totalCRFSS,
            totalCompleted,
            totalTicketOverdue,
        }

        public enum TicketChartDashboardSectionType
        {
            none
        }

        public enum DashboardSectionType
        {
            [Description("Ticket")]
            Ticket,
            [Description("Order")]
            Order
        }

        public enum ExceptionMessage
        {
            [Description("Your ID is not valid.")]
            InvalidLengthBase64,
            [Description("Access Denied. You don't have access right to this section. Please contact with Administrator")]
            AllowGETRequest,
            [Description("There is occurred an exception in executing. (Your object is null)")]
            ObjectReferenceNull,
        }

        public enum WorkOrderSectionType
        {
            [Description("PPM")]
            PPM,
            [Description("Work Order")]
            WO
        }

        public enum ChartType
        {
            Pie,
            Bar,
        }

        public class DateSet
        {
            public const string MM_DD_YYYY = "MM/dd/yyyy";
            public const string DD_MM_YYYY = "dd/MM/yyyy";
            public const string YYYY_MM_DD = "dd/MM/yyyy";
            public const string DD_MMM_YYYY = "dd-MMM-yyyy";

            public const string DDD_DD_MMM_YYYY = "ddd,dd MMM yyyy";
            public const string DDDD_MMM_DD_YYYY = "dddd,MMMM dd,yyyy";
            public const string H_MM_TT = " h:mm tt";
            public const string DDDD_DD_MM_YYYY_TT = "dddd, dd-MMM-yyyy hh:mm tt";

            public static string FormateDate(DateTime date, string dateFormate)
            {
                return date.ToString(dateFormate);
            }

            public static DateTime GetTodayDate()
            {
                return System.DateTime.Now;
            }

            public static string GetTodayDate(string dateFormate)
            {
                DateTime dt = System.DateTime.Now;
                return String.Format("{0:" + dateFormate + "}", dt);
            }

            public static DateTime DefaultDate
            {
                get
                {
                    return Convert.ToDateTime("01/01/1999");
                }
            }
        }

        public static string SetDateFormat(string DateToFormat)
        {
            DateTime? UpdatedDate;
            UpdatedDate = DateTime.Parse(DateToFormat);
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "am";
            fi.PMDesignator = "pm";

            TimeSpan Updiff = DateTime.Now - UpdatedDate.Value;
            string Upformatted = string.Format(System.Globalization.CultureInfo.CurrentCulture, "{0} days, {1} hours, {2} minutes, {3} seconds", Updiff.Days, Updiff.Hours, Updiff.Minutes, Updiff.Seconds);
            string _date = "";

            if (Updiff.Days > 1)
            {
                _date = Convert.ToDateTime(UpdatedDate).ToString("dd/MM/yyyy hh:mm tt", fi);
            }
            else
            {
                if (Updiff.Hours == 0)
                {
                    if (Updiff.Minutes == 0)
                    {
                        _date = Updiff.Seconds + " Seconds" + " Ago";
                        if (_date.Contains("-"))
                        {
                            _date = Updiff.Seconds + " Seconds" + " Left";
                            _date = _date.Replace("-", "");
                        }
                    }
                    else
                    {

                        _date = Updiff.Minutes + " Minutes, " + Updiff.Seconds + " Seconds" + " Ago";
                        if (_date.Contains("-"))
                        {
                            _date = Updiff.Minutes + " Minutes, " + Updiff.Seconds + " Seconds" + " Left";
                            _date = _date.Replace("-", "");
                        }
                    }
                }
                else if (Updiff.Hours > 0)
                {
                    _date = +Updiff.Hours + " Hours, " + Updiff.Minutes + " Minutes" + " Ago";
                }
                else
                {
                    if (Updiff.Days == 0)
                    {
                        _date = Updiff.Hours + " Hours, " + Updiff.Minutes + " Minutes" + " Ago";
                        if (_date.Contains("-"))
                        {
                            _date = +Updiff.Hours + " Hours, " + Updiff.Minutes + " Minutes" + " Left";
                            _date = _date.Replace("-", "");
                        }
                    }
                    else
                    {
                        _date = +Updiff.Days + " Days," + Updiff.Hours + " Hours, " + Updiff.Minutes + " Minutes" + " Ago";
                        if (_date.Contains("-"))
                        {
                            _date = +Updiff.Days + " Days," + Updiff.Hours + " Hours, " + Updiff.Minutes + " Minutes" + " Left";
                            _date = _date.Replace("-", "");
                        }
                    }
                }
            }
            return _date;
        }

        #region Encryption / Decryption
        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        static string hashKey = WebConfigurationManager.AppSettings["Encryption"];

        private static string EncryptSecure<T>(string salt, string value)
        where T : SymmetricAlgorithm, new()
        {
            //return "asdhegferh" + value;
            DeriveBytes rgb = new Rfc2898DeriveBytes(hashKey, Encoding.Unicode.GetBytes("Hanodale" + salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                string appendString = "ElAdOnAh";
                string returnstring = Convert.ToBase64String(buffer.ToArray());

                returnstring = returnstring.Replace("/", "--" + appendString + "2F" + appendString + "--");
                returnstring = returnstring.Replace("!", "--" + appendString + "21" + appendString + "--");
                returnstring = returnstring.Replace("#", "--" + appendString + "23" + appendString + "--");
                returnstring = returnstring.Replace("$", "--" + appendString + "25" + appendString + "--");
                returnstring = returnstring.Replace("&", "--" + appendString + "26" + appendString + "--");
                returnstring = returnstring.Replace("'", "--" + appendString + "27" + appendString + "--");
                returnstring = returnstring.Replace("(", "--" + appendString + "28" + appendString + "--");
                returnstring = returnstring.Replace(")", "--" + appendString + "29" + appendString + "--");
                returnstring = returnstring.Replace("*", "--" + appendString + "2A" + appendString + "--");
                returnstring = returnstring.Replace("+", "--" + appendString + "2B" + appendString + "--");
                returnstring = returnstring.Replace(",", "--" + appendString + "2C" + appendString + "--");
                returnstring = returnstring.Replace(":", "--" + appendString + "3A" + appendString + "--");
                returnstring = returnstring.Replace(";", "--" + appendString + "3B" + appendString + "--");
                //returnstring = returnstring.Replace("=","--" + appendString + "3D" + appendString + "--");
                returnstring = returnstring.Replace("?", "--" + appendString + "3F" + appendString + "--");
                returnstring = returnstring.Replace("@", "--" + appendString + "40" + appendString + "--");
                returnstring = returnstring.Replace("[", "--" + appendString + "5B" + appendString + "--");
                returnstring = returnstring.Replace("]", "--" + appendString + "5D" + appendString + "--");


                return returnstring;
            }
        }

        private static string DecryptSecure<T>(string salt, string value)
           where T : SymmetricAlgorithm, new()
        {
            string stringToDecrypt = value;
            // salt = (salt == "3988" || salt == "3857") ? "42" : salt;
            string appendString = "ElAdOnAh";

            stringToDecrypt = stringToDecrypt.Replace("+", " ");

            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "2F" + appendString + "--", "/");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "21" + appendString + "--", "!");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "23" + appendString + "--", "#");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "24" + appendString + "--", "$");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "26" + appendString + "--", "&");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "27" + appendString + "--", "'");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "28" + appendString + "--", "(");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "29" + appendString + "--", ")");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "2A" + appendString + "--", "*");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "2B" + appendString + "--", "+");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "2C" + appendString + "--", ",");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "3A" + appendString + "--", ":");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "3B" + appendString + "--", ";");
            //stringToDecrypt = stringToDecrypt.Replace( "--" + appendString + "5D" + appendString + "--","=");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "3F" + appendString + "--", "?");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "40" + appendString + "--", "@");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "5B" + appendString + "--", "[");
            stringToDecrypt = stringToDecrypt.Replace("--" + appendString + "5D" + appendString + "--", "]");

            //return value.Remove(0,10);
            DeriveBytes rgb = new Rfc2898DeriveBytes(hashKey, Encoding.Unicode.GetBytes("Hanodale" + salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(stringToDecrypt)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public static string Encrypt(string salt, string value)
        {
            try
            {
                //salt = (salt == "3988" || salt == "3857") ? "42" : salt;
                return EncryptSecure<RijndaelManaged>(salt, value);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public static string Decrypt(string salt, string value)
        {
            try
            {
                return DecryptSecure<RijndaelManaged>(salt, value);
            }
            catch (ArgumentNullException)
            {
                return null;

            }
        }

        public static int DecryptToID(string salt, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;
                string result = Decrypt(salt, value);

                return Convert.ToInt32(result);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }


        //private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        //{

        //    MemoryStream ms = new MemoryStream();

        //    Rijndael alg = Rijndael.Create();
        //    alg.Key = Key;

        //    alg.IV = IV;
        //    CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

        //    cs.Write(clearData, 0, clearData.Length);
        //    cs.Close();
        //    byte[] encryptedData = ms.ToArray();
        //    return encryptedData;
        //}

        //private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        //{

        //    MemoryStream ms = new MemoryStream();
        //    Rijndael alg = Rijndael.Create();
        //    alg.Key = Key;
        //    alg.IV = IV;
        //    CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
        //    cs.Write(cipherData, 0, cipherData.Length);
        //    cs.Close();
        //    byte[] decryptedData = ms.ToArray();
        //    return decryptedData;
        //}


        //public static string Decrypt(string salt, string value, int Bits)
        //{

        //    byte[] cipherBytes = Convert.FromBase64String(value);

        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(salt,

        //        new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

        //    if (Bits == 128)
        //    {
        //        byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else if (Bits == 192)
        //    {
        //        byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else if (Bits == 256)
        //    {
        //        byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else
        //    {
        //        return string.Concat(Bits);
        //    }

        //}

        //public static string Encrypt(string salt, string value, int Bits)
        //{

        //    byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(value);

        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(salt,

        //        new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

        //    if (Bits == 128)
        //    {
        //        byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else if (Bits == 192)
        //    {
        //        byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else if (Bits == 256)
        //    {
        //        byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else
        //    {
        //        return string.Concat(Bits);
        //    }
        //}

        #endregion

        public static Dictionary<string, string> GetAppSettingItem(string value)
        {
            var result = new Dictionary<string, string>();
            var lst = ConfigurationManager.AppSettings.AllKeys.Where(key => key.StartsWith(value)).Select(key => new { Key = key, Value = ConfigurationManager.AppSettings[key] });
            foreach (var item in lst)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, bool isXlsx)
        {
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

            if (isXlsx)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }

            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();

                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                var Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + Sheet1 + "]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch
            {
                // Handle exceptions
            }
            finally
            {
                oledbConn.Close();
            }

            return dt;

        }

        public static DataTable ConvertCSVtoDataTable(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            using (var reader = new StreamReader(file.InputStream))
            {
                var line = reader.ReadLine();

                var headers = ParseCsvLine(line);
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                // Skip the header row

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    var rows = ParseCsvLine(line);
                    DataRow dr = dt.NewRow();

                    if (rows.Any(p => !string.IsNullOrEmpty(p)))
                        {
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }


        private static string[] ParseCsvLine(string line)
        {
            var inQuotes = false;
            var sb = new StringBuilder();
            var fields = new List<string>();

            for (int i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // If double quotes within a quoted field, add a single quote and skip the next character
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        // Toggle the inQuotes flag when encountering a quote
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    // If a comma is found and we are not inside quotes, add the current field to the list
                    fields.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    // Add character to the current field
                    sb.Append(ch);
                }
            }
            // Add the last field
            fields.Add(sb.ToString());

            return fields.ToArray();
        }

    }

    public class CustomModelMetadata
    {
        public static void ConfigureMetadata(ModelMetadata metadata)
        {
            metadata.DisplayName = "Full Name"; // Set display name
            metadata.Description = "Enter your full name"; // Set description
            metadata.IsRequired = true; // Set required
            metadata.Order = 1; // Set order
            metadata.TemplateHint = "Text"; // Set template hint (e.g., for UI)
            metadata.DataTypeName = "Text"; // Set data type name
        }

        // Add more metadata properties and methods as needed
    }
}