using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using Hanodale.BusinessLogic;
using Hanodale.Domain.DTOs;
using System.ServiceModel;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace Hanodale.WebUI
{
    public partial class GenerateReports : System.Web.UI.Page
    {

        #region Declaration

        public string UserId { get; set; }

        public string ReportId { get; set; }

        public string OrganisationId { get; set; }

        public string SubCostId { get; set; }

        private IUserService svc;
        private ICommonService svcCommon;
        private IOrganizationService svcOrganization;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserId = Request.QueryString["UserId"];
                ReportId = Request.QueryString["ReportId"];
                SubCostId = Request.QueryString["SubCostId"];
                int mainCostCenter = Convert.ToInt32(WebConfigurationManager.AppSettings["mainCostCategoryId"]);
                int subCostCenter = Convert.ToInt32(WebConfigurationManager.AppSettings["subCostCategoryId"]);

                string reportName = Request.QueryString["ReportName"];//AssetTypeSummary
                lbltile.Text += reportName;
                reportName = reportName.Replace(" ", "");

                if (!string.IsNullOrEmpty(reportName))
                {
                    reportName = reportName.Replace(" ", "");
                }
                else
                {
                    reportName = reportName;
                }
                if (string.IsNullOrEmpty(UserId))
                {
                    UserId = "0";
                }
                //test

                svc = new UserService();
                string sessionId = Guid.NewGuid().ToString();
                var userId = Convert.ToInt32(this.User.Identity.Name);
                var user = svc.GetUserBySCId(userId, userId, subCostCenter);
                //
                if (reportName != null)
                    LoadReport();
             
            }
        }


        private void loadMainCostCenter(int mainCostCenter)
        {
            svcOrganization = new OrganizationService();
            List<Organizations> _lstOrganisation = new List<Organizations>();
            _lstOrganisation = svcOrganization.GerOrganisation(mainCostCenter);
        }

        private void loadSubCostCenter(int mainCostCenter, int UserId)
        {
            svcCommon = new CommonService();
            List<Organizations> _lstOrganisation = new List<Organizations>();
            _lstOrganisation = svcCommon.GetSubCostCenter(mainCostCenter, UserId);
        }

        private void LoadReport()
        {
            try
            {
                UserId = Request.QueryString["UserId"];
                ReportId = Request.QueryString["ReportId"];
                string reportName = Request.QueryString["ReportName"];
                string reportCode = Request.QueryString["ReportCode"] ?? "";
                SubCostId = Request.QueryString["SubCostId"];
                reportName = reportName.Replace(" ", "");

                int mainCostCenter = Convert.ToInt32(WebConfigurationManager.AppSettings["mainCostCategoryId"]);
                int subCostCenter = Convert.ToInt32(WebConfigurationManager.AppSettings["subCostCategoryId"]);
                //test

                svc = new UserService();
                string sessionId = Guid.NewGuid().ToString();
                var userId = Convert.ToInt32(this.User.Identity.Name);
                var user = svc.GetUserBySCId(userId, userId, subCostCenter);
                //

                if (reportName == "PPMTaskMatrix")
                {
                    reportName = reportName.Replace("PPMTaskMatrix", "PPMScheduleTaskMatrix");
                }
                else
                {
                    reportName = reportName;
                }

                string _ssrsReport = reportCode.Contains("USRRPT_") ? reportCode : reportName;
                //reportName = CheckReportExist(_ssrsReport, SubCostId);

                RptViewer.ShowParameterPrompts = false;
                RptViewer.ShowCredentialPrompts = false;
                RptViewer.ShowPrintButton = true;
                RptViewer.ServerReport.ReportServerCredentials = new ReportCredentials(
                                              ConfigurationManager.AppSettings["ReportUserName"].ToString(),
                                              ConfigurationManager.AppSettings["ReportPassword"].ToString(),
                                              ConfigurationManager.AppSettings["ReportDomain"].ToString());
                RptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                RptViewer.ServerReport.ReportServerUrl = new System.Uri(ConfigurationManager.AppSettings["ReportURL"].ToString());

                if (reportName.Contains("WOSummary"))
                {
                    RptViewer.ServerReport.ReportPath = ConfigurationManager.AppSettings["Path"].ToString() + reportName;
                }
                else
                {
                    RptViewer.ServerReport.ReportPath = ConfigurationManager.AppSettings["Path"].ToString() + _ssrsReport;
                }


                 if (reportName == "PPMMatrix" || reportName == "PPMScheduleMatrix" || reportName == "PPMScheduleTaskMatrix" || reportName == "PPMScheduleTask" || reportName == "PPMWeekly")
                {
                    ReportParameter[] _params;
                    _params = new ReportParameter[3];
                    _params[0] = new ReportParameter("userId", UserId, true);
                    _params[1] = new ReportParameter("reportId", ReportId, true);
                    _params[2] = new ReportParameter("costCenterId", SubCostId, true);
                    RptViewer.ServerReport.SetParameters(_params);
                    RptViewer.ShowParameterPrompts = true;
                }
                else if (reportName.Contains("WOSummary"))
                {
                    ReportParameter[] _params;
                    _params = new ReportParameter[4];
                    _params[0] = new ReportParameter("userId", UserId, true);
                    _params[1] = new ReportParameter("reportId", ReportId, true);
                    _params[2] = new ReportParameter("organizationId", SubCostId, true);
                    _params[3] = new ReportParameter("code", "", true);
                    RptViewer.ServerReport.SetParameters(_params);
                    RptViewer.ShowParameterPrompts = true;
                }
                else
                {
                    if (_ssrsReport.Contains("USRRPT_"))
                    {
                        ReportParameter[] _params;
                        _params = new ReportParameter[3];
                        _params[0] = new ReportParameter("userId", UserId, true);
                        _params[1] = new ReportParameter("reportId", ReportId, true);
                        _params[2] = new ReportParameter("costCenterId", SubCostId, true);
                        RptViewer.ServerReport.SetParameters(_params);
                        RptViewer.ShowParameterPrompts = true;
                    }
                    else
                    {
                        RptViewer.ShowParameterPrompts = true;
                    }
                }
           

                RptViewer.ServerReport.Refresh();
            }
            catch (Exception err)
            {

                throw;
            }
        }

        private List<string> GetReportFiles()
        {
            List<string> loReportlist = new List<string>();
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["SQlConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetReportFiles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (IDataReader loReader = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (loReader.Read())
                            {
                                string _fileName = loReader.GetValue(loReader.GetOrdinal("Name")).ToString();
                                loReportlist.Add(_fileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            loReader.Close();
                        }
                    }
                }
            }
            return loReportlist;
        }

        private string CheckReportExist(string fileName, string costCenterId)
        {
            string _fileName = fileName;
            List<string> ReportFileName = GetReportFiles();
            string FileExist = ReportFileName.Find(delegate (string FileName) { return FileName == fileName + "_" + costCenterId; });

            if (!string.IsNullOrEmpty(FileExist))
            {
                _fileName = fileName + "_" + costCenterId;
            }
            else
            {
                _fileName = fileName;
            }
            return _fileName;
        }

    }

    #region ReportClass

    public class ReportCredentials : IReportServerCredentials
    {
        string _userName, _password, _domain;
        public ReportCredentials(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                return new System.Net.NetworkCredential(_userName, _password, _domain);
            }
        }

        public bool GetFormsCredentials(out System.Net.Cookie authCoki, out string userName, out string password, out string authority)
        {
            userName = _userName;
            password = _password;
            authority = _domain;
            authCoki = null;
            userName = password = authority = null;
            return false;
        }


    }

    #endregion

}