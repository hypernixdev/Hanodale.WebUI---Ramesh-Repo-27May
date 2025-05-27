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
using Hanodale.WebUI.MailService;
using Hanodale.Utility;
using Hanodale.Domain.DTOs;
using System.Data.SqlClient;
using System.Data;

namespace Hanodale.WebUI.Helpers
{
    public class MailSetting 
    {
        public static void SendMailData(int typeId, string subject, string description, string emailTo, string emailCC, int statusId, int moduleId)
        {
            Email objData = new Email();
            try
            {
                objData.Subject = subject;
                objData.Description = description;
                objData.ToId = emailTo;
                objData.CcId = emailCC;
                objData.createdBy = "System";
                objData.createdDate = DateTime.Now;
                bool _ret = SaveEmail(objData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static void SendMailUserData(Users entity)
        {
            Email objData = new Email();
            
            
            try
            {
                var MailSubjectSubmit = string.Empty;
                int MailModuleId = Convert.ToInt32(ConfigurationManager.AppSettings["MailModuleId"]);
                string MailURL = ConfigurationManager.AppSettings["URL"].ToString();
                int ApprovalSubmissionEmail = Convert.ToInt32(WebConfigurationManager.AppSettings["ApprovalSubmissionEmail"]);

                IBusinessService svcBusiness = new BusinessService();

                var userData = svcBusiness.GetListBusinessUser(entity);

                if (userData.email != null)
                {
                    if (userData.status == "Active")
                    {
                        var subject = WebHelper.Placeholders.ReplaceAll(Emails.ActiveBusinessMasterSubject, userData);
                        subject = WebHelper.Placeholders.ReplaceAll(subject, "$CODE$", userData.code);

                        //var body = WebHelper.Placeholders.ReplaceAll(Emails.ActiveBusinessMasterBody, userData);
                        string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "ActiveBusinessMasterBody.txt");
                        body = WebHelper.Placeholders.ReplaceAll(body, userData);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$BUSINESSNAME$", userData.businessName);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$SUPPLIERUSEREMAIL$", userData.email);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$USERNAME$", userData.userName);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$PASSWORD$", userData.passwordHash);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", MailURL);

                        objData.Subject = subject;
                        objData.Description = body;
                        objData.ToId = userData.email;
                        objData.CcId = "";
                        objData.createdBy = "System";
                        objData.createdDate = DateTime.Now;
                        bool _ret = SaveEmail(objData);
                    }
                    else
                    {
                        var subject = WebHelper.Placeholders.ReplaceAll(Emails.InActiveBusinessMasterSubject, userData);
                        subject = WebHelper.Placeholders.ReplaceAll(subject, "$CODE$", userData.code);

                        //var body = WebHelper.Placeholders.ReplaceAll(Emails.InActiveBusinessMasterBody, userData);
                        string body = Helpers.MailSetting.OpenFile(ConfigurationManager.AppSettings["EmailPath"] + @"\" + "InActiveBusinessMasterBody.txt");
                        body = WebHelper.Placeholders.ReplaceAll(body, userData);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$BUSINESSNAME$", userData.businessName);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$SUPPLIERUSEREMAIL$", userData.email);
                        body = WebHelper.Placeholders.ReplaceAll(body, "$URL$", MailURL);

                        objData.Subject = subject;
                        objData.Description = body;
                        objData.ToId = userData.email;
                        objData.CcId = "";
                        objData.createdBy = "System";
                        objData.createdDate = DateTime.Now;
                        bool _ret = SaveEmail(objData);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static bool SaveEmail(Email obj)
        {
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SQlConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("EmailSchedules_Ins", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@subject", SqlDbType.VarChar).Value = obj.Subject;
                        cmd.Parameters.Add("@emailContent", SqlDbType.VarChar).Value = obj.Description;
                        cmd.Parameters.Add("@emailTo", SqlDbType.VarChar).Value = obj.ToId;
                        cmd.Parameters.Add("@emailCC", SqlDbType.VarChar).Value = obj.CcId;
                        cmd.Parameters.Add("@filePath", SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@fileName", SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@status_Id", SqlDbType.VarChar).Value = "NEW";
                        cmd.Parameters.Add("@remark", SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@createdBy", SqlDbType.VarChar).Value = obj.createdBy;
                        cmd.Parameters.Add("@createdDate", SqlDbType.DateTime).Value = obj.createdDate;

                        con.Open();

                        var _obj = cmd.ExecuteNonQuery();
                        if (_obj > 0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return false;
        }

        public static string OpenFile(string FileName)
        {
            string contents = "";

            /// Check File Existance
            if (!File.Exists(FileName))
                return "Error, No File Found : " + FileName;

            /// Stream reder
            StreamReader streamReader = File.OpenText(FileName);

            /// Read  All text
            contents = streamReader.ReadToEnd();

            /// Retun What it Read
            return contents;
        }
    }
}