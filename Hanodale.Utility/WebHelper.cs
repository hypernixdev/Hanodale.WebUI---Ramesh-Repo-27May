using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Novacode;
using System.IO;
using System.Web.Mvc;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Security.Policy;


namespace Hanodale.Utility
{
    public static class WebHelper
    {
       
        public static class Mail
        {
            public static void SendMail(string from, string[] to, string title, string body)
            {
                if (to == null)
                {
                    throw new ArgumentNullException("to", "Mail recipients list is required to send an email.");
                }

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(from);
                    mail.IsBodyHtml = true;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.Subject = title;
                    mail.Body = body;

                    foreach (string recipient in to)
                    {
                        mail.To.Add(new MailAddress(recipient));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        public static class Placeholders
        {
            public static string ReplaceAll(string text, string token, string value)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                return text.Replace(token, value);
            }

            public static string ReplaceAll(string text, Users user)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                    return text
                    .Replace("$USER_FULL_NAME$", user.firstName ?? string.Empty)
                    .Replace("$USER_EMAIL$", user.email ?? string.Empty)
                 .Replace("$USERNAME$", user.userName ?? string.Empty);
            }
        }

        public static FileStreamResult DownloadDocumentWord(string sourcePath,
        string template, Dictionary<string, string> placeHolderDictionary)
        {
            using (DocX document = DocX.Load(sourcePath + template))
            {
                //replace the placeholder in template with generated content
                foreach (KeyValuePair<string, string> item in placeHolderDictionary)
                {
                    document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
                }

                //create document in memory based on the template & content. download it.
                var ms = new MemoryStream();
                document.SaveAs(ms);
                ms.Position = 0;

                var file = new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName = template
                };
                return file;
            }
        }

        public static FileStreamResult DownloadDocument(string sourcePath,
       string template, Dictionary<string, string> placeHolderDictionary)
        {
             string[] _name = template.Split('.').ToArray();

             var _outputStream = new MemoryStream();
             Spire.Doc.Document _spireDoc = new Spire.Doc.Document();
             _spireDoc.LoadFromFile(sourcePath + template, FileFormat.Docx2013);

             foreach (KeyValuePair<string, string> item in placeHolderDictionary)
             {
                 _spireDoc.Replace(item.Key, item.Value ?? string.Empty, false, false);
             }

             _spireDoc.SaveToFile(_outputStream, Spire.Doc.FileFormat.PDF);
             _outputStream.Position = 0;

             var file = new FileStreamResult(_outputStream, "application/pdf")
             {
                 FileDownloadName = _name[0] + ".pdf"
             };

             return file;
        //    using (DocX document = DocX.Load(sourcePath + template))
        //    {
        //        //replace the placeholder in template with generated content
        //        foreach (KeyValuePair<string, string> item in placeHolderDictionary)
        //        {
        //            document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
        //        }

        //        //create document in memory based on the template & content. download it.
        //        var ms = new MemoryStream();
        //        document.SaveAs(ms);
        //        //   document.SaveAs(sourcePath + _name[0] + "_test.docx");
        //        ms.Position = 0;



        //        var _outputStream = new MemoryStream();
        //        Spire.Doc.Document _spireDoc = new Spire.Doc.Document();
        //        _spireDoc.LoadFromStream(ms, FileFormat.Docx);
        //        _spireDoc.SaveToFile(_outputStream, Spire.Doc.FileFormat.PDF);
        //        _outputStream.Position = 0;

        //        var file = new FileStreamResult(_outputStream, "application/pdf")
        //          {
        //              FileDownloadName = _name[0] + ".pdf"
        //          };

        //        return file;

        //    }
        }

        public static FileStreamResult GeneratePFI(string sourcePath,
     string template, Dictionary<string, object> dictObj, Dictionary<string, string> dictValue)
        {
            string[] _name = template.Split('.').ToArray();
            using (DocX document = DocX.Load(sourcePath + template))
            {
                //replace the placeholder in template with generated content
                foreach (KeyValuePair<string, string> item in dictValue)
                {
                    document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
                }

                //Load item list
                int _idx = 0;
                Novacode.Table _table0 = document.Tables[2];
               // _table0.AutoFit = AutoFit.Contents;
                //Add Contractor List
                var _order = (ViewOrder)dictObj["Order"];
                decimal _subTotal = 0;
                if (_order != null)
                {
                    foreach (var _item in _order.OrderItems)
                    {
                        _idx++;
                        _subTotal = _subTotal + Math.Round(_item.unitPrice * (_item.scannedQty > 0? _item.scannedQty : _item.orderQty),2);

                        //define row
                        Row _myRow = _table0.InsertRow();
                        _myRow.Cells[0].Width = 30;
                        _myRow.Cells[0].Paragraphs.First().Append(_idx.ToString()).FontSize(9);
                        _myRow.Cells[1].Width = 250;
                        _myRow.Cells[1].Paragraphs.First().Append(_item.partNum + "\n" + _item.partName + "\n").FontSize(9);
                        _myRow.Cells[2].Width = 100;
                        _myRow.Cells[2].Paragraphs.First().Append(_item.orderQty.ToString() + _item.salesUm).FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[3].Width = 100;
                        _myRow.Cells[3].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice),2).ToString()) + " /1").FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[4].Width = 100;
                        _myRow.Cells[4].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice * (_item.scannedQty > 0 ? _item.scannedQty : _item.orderQty)), 2)).ToString()).FontSize(9).Alignment = Alignment.right;
                        _table0.Rows.Add(_myRow);
                        
                    }

                    //Load Customer
                    var _customer = (Customers)dictObj["Customer"];
                    if (_customer != null)
                    {
                        string _csutomerAddress = GetFormattedAddress(_customer.name,
                            _customer.address1,
                            _customer.address2,
                            _customer.address3,
                            _customer.city,
                            "",
                            _customer.state,
                            _customer.country);
                        
                        document.ReplaceText("$CustomerAddress$", _csutomerAddress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$CustomerAddress$", "", false);
                    }

                    //Load ShipTo
                    var _shipTo = (ShipToAddresses)dictObj["ShipTo"];
                    if (_shipTo != null)
                    {
                        string _shipToAdress = GetFormattedAddress(_shipTo.shippingCode,
                            _shipTo.address1,
                            _shipTo.address2,
                            _shipTo.address3,
                            _shipTo.cityName,
                            _shipTo.zip,
                            _shipTo.stateName,
                            _shipTo.countryName);

                        document.ReplaceText("$ShipTo$", _shipToAdress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$ShipTo$", "", false);
                    }

                    document.ReplaceText("$lineSubTotal$", String.Format("{0:N2}",Math.Round(_subTotal,2)), false);
                }

                //create document in memory based on the template & content. download it.
                var ms = new MemoryStream();
                document.SaveAs(ms);
                //   document.SaveAs(sourcePath + _name[0] + "_test.docx");
                ms.Position = 0;

                var _outputStream = new MemoryStream();
                Spire.Doc.Document _spireDoc = new Spire.Doc.Document();
                _spireDoc.LoadFromStream(ms, FileFormat.Docx);
                _spireDoc.SaveToFile(_outputStream, Spire.Doc.FileFormat.PDF);
                _outputStream.Position = 0;

                var file = new FileStreamResult(_outputStream, "application/pdf")
                {
                    FileDownloadName = _name[0] + ".pdf"
                };

                return file;

                  }
            }

        public static FileStreamResult GenerateOrder(string sourcePath,
    string template, Dictionary<string, object> dictObj, Dictionary<string, string> dictValue)
        {
            string[] _name = template.Split('.').ToArray();
            using (DocX document = DocX.Load(sourcePath + template))
            {
                //replace the placeholder in template with generated content
                foreach (KeyValuePair<string, string> item in dictValue)
                {
                    document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
                }

                //Load item list
                int _idx = 0;
                Novacode.Table _table0 = document.Tables[1];
                // _table0.AutoFit = AutoFit.Contents;
                //Add Contractor List
                var _order = (ViewOrder)dictObj["Order"];
                decimal _subTotal = 0;
                if (_order != null)
                {
                    foreach (var _item in _order.OrderItems)
                    {
                        _idx++;
                        _subTotal = _subTotal + Math.Round(_item.unitPrice * _item.orderQty, 2);

                        //define row
                        Row _myRow = _table0.InsertRow();
                        _myRow.Cells[0].Width = 30;
                        _myRow.Cells[0].Paragraphs.First().Append(_idx.ToString()).FontSize(9);
                        _myRow.Cells[1].Width = 250;
                        _myRow.Cells[1].Paragraphs.First().Append(_item.partNum + "\n" + _item.partName + "\n").FontSize(9);
                        _myRow.Cells[2].Width = 100;
                        _myRow.Cells[2].Paragraphs.First().Append(_item.orderQty.ToString() + _item.salesUm).FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[3].Width = 100;
                        _myRow.Cells[3].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice), 2)).ToString() + " /1").FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[4].Width = 100;
                        _myRow.Cells[4].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice * _item.orderQty), 2)).ToString()).FontSize(9).Alignment = Alignment.right;
                        _table0.Rows.Add(_myRow);

                    }

                    //Load Customer
                    var _customer = (Customers)dictObj["Customer"];
                    if (_customer != null)
                    {
                        string _csutomerAddress = GetFormattedAddress(_customer.name,
                            _customer.address1,
                            _customer.address2,
                            _customer.address3,
                            _customer.city,
                            "",
                            _customer.state,
                            _customer.country);

                        document.ReplaceText("$CustomerAddress$", _csutomerAddress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$CustomerAddress$", "", false);
                    }

                    //Load ShipTo
                    var _shipTo = (ShipToAddresses)dictObj["ShipTo"];
                    if (_shipTo != null)
                    {
                        string _shipToAdress = GetFormattedAddress(_shipTo.shippingCode,
                            _shipTo.address1,
                            _shipTo.address2,
                            _shipTo.address3,
                            _shipTo.cityName,
                            _shipTo.zip,
                            _shipTo.stateName,
                            _shipTo.countryName);

                        document.ReplaceText("$ShipTo$", _shipToAdress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$ShipTo$", "", false);
                    }

                    document.ReplaceText("$lineSubTotal$", String.Format("{0:N2}", Math.Round(_subTotal,2)), false);
                }

                //create document in memory based on the template & content. download it.
                var ms = new MemoryStream();
                document.SaveAs(ms);
                //   document.SaveAs(sourcePath + _name[0] + "_test.docx");
                ms.Position = 0;

                var _outputStream = new MemoryStream();
                Spire.Doc.Document _spireDoc = new Spire.Doc.Document();
                _spireDoc.LoadFromStream(ms, FileFormat.Docx);
                _spireDoc.SaveToFile(_outputStream, Spire.Doc.FileFormat.PDF);
                _outputStream.Position = 0;

                var file = new FileStreamResult(_outputStream, "application/pdf")
                {
                    FileDownloadName = _name[0] + ".pdf"
                };

                return file;

            }
        }
        public static FileStreamResult GenerateReceipt(string sourcePath,
 string template, Dictionary<string, object> dictObj, Dictionary<string, string> dictValue)
        {
            string[] _name = template.Split('.').ToArray();
            using (DocX document = DocX.Load(sourcePath + template))
            {
                //replace the placeholder in template with generated content
                foreach (KeyValuePair<string, string> item in dictValue)
                {
                    document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
                }

                //Load item list
                int _idx = 0;
                Novacode.Table _table0 = document.Tables[2];
                // _table0.AutoFit = AutoFit.Contents;
                //Add Contractor List
                var _order = (ViewOrder)dictObj["Order"];
                decimal _subTotal = 0;
                if (_order != null)
                {
                    foreach (var _item in _order.OrderItems)
                    {
                        _idx++;
                        _subTotal = _subTotal + Math.Round(_item.unitPrice * _item.scannedQty,2);

                        //define row
                        Row _myRow = _table0.InsertRow();
                        _myRow.Cells[0].Width = 30;
                        _myRow.Cells[0].Paragraphs.First().Append(_idx.ToString()).FontSize(9);
                        _myRow.Cells[1].Width = 250;
                        _myRow.Cells[1].Paragraphs.First().Append(_item.partNum + "\n" + _item.partName + "\n").FontSize(9);
                        _myRow.Cells[2].Width = 100;
                        _myRow.Cells[2].Paragraphs.First().Append(_item.orderQty.ToString() + _item.salesUm).FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[3].Width = 100;
                        _myRow.Cells[3].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice), 2)) + " /1").FontSize(9).Alignment = Alignment.right;
                        _myRow.Cells[4].Width = 80;
                        _myRow.Cells[4].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice * _item.scannedQty), 2))).FontSize(9).Alignment = Alignment.right;
                        _table0.Rows.Add(_myRow);

                    }
                    document.ReplaceText("$lineSubTotal$", String.Format("{0:N2}", _subTotal), false);

                    //Load Payment list
                    _idx = 0;
                    Novacode.Table _table1 = document.Tables[4];
                    _subTotal = 0;
                    foreach (var _item in _order.OrderPayments)
                    {
                        _idx++;
                        _subTotal = _subTotal + Math.Round(_item.Amount,2);

                        //define row
                        Row _myRow = _table1.InsertRow();
                        _myRow.Cells[0].Width = 30;
                        _myRow.Cells[0].Paragraphs.First().Append(_idx.ToString()).FontSize(9);
                        _myRow.Cells[1].Width = 80;
                        _myRow.Cells[1].Paragraphs.First().Append(_item.PaymentType).FontSize(9);
                        _myRow.Cells[2].Width = 80;
                        _myRow.Cells[2].Paragraphs.First().Append(_item.Bank??"").FontSize(9);
                        _myRow.Cells[3].Width = 100;
                        _myRow.Cells[3].Paragraphs.First().Append(_item.RefNumber??"").FontSize(9);
                        _myRow.Cells[4].Width = 100;
                        _myRow.Cells[4].Paragraphs.First().Append(String.Format("{0:N2}", _item.Amount)).FontSize(9).Alignment = Alignment.right;
                        _table1.Rows.Add(_myRow);
                    }
                    document.ReplaceText("$PaymentTotal$", String.Format("{0:N2}", Math.Round(_subTotal,2)), false);
                        

                        //Load Customer
                        var _customer = (Customers)dictObj["Customer"];
                    if (_customer != null)
                    {
                        string _csutomerAddress = GetFormattedAddress(_customer.name,
                            _customer.address1,
                            _customer.address2,
                            _customer.address3,
                            _customer.city,
                            "",
                            _customer.state,
                            _customer.country);

                        document.ReplaceText("$CustomerAddress$", _csutomerAddress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$CustomerAddress$", "", false);
                    }

                    //Load ShipTo
                    var _shipTo = (ShipToAddresses)dictObj["ShipTo"];
                    if (_shipTo != null)
                    {
                        string _shipToAdress = GetFormattedAddress(_shipTo.shippingCode,
                            _shipTo.address1,
                            _shipTo.address2,
                            _shipTo.address3,
                            _shipTo.cityName,
                            _shipTo.zip,
                            _shipTo.stateName,
                            _shipTo.countryName);

                        document.ReplaceText("$ShipTo$", _shipToAdress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$ShipTo$", "", false);
                    }

                   
                }

                //create document in memory based on the template & content. download it.
                var ms = new MemoryStream();
                document.SaveAs(ms);
                //   document.SaveAs(sourcePath + _name[0] + "_test.docx");
                ms.Position = 0;

                var _outputStream = new MemoryStream();
                Spire.Doc.Document _spireDoc = new Spire.Doc.Document();
                _spireDoc.LoadFromStream(ms, FileFormat.Docx);
                _spireDoc.SaveToFile(_outputStream, Spire.Doc.FileFormat.PDF);
                _outputStream.Position = 0;

                var file = new FileStreamResult(_outputStream, "application/pdf")
                {
                    FileDownloadName = _name[0] + ".pdf"
                };

                return file;

            }
        }

        public static FileStreamResult GenerateInvoice(string sourcePath,
 string template, Dictionary<string, object> dictObj, Dictionary<string, string> dictValue,string watermark)
        {
            string[] _name = template.Split('.').ToArray();
            using (DocX document = DocX.Load(sourcePath + template))
            {
                //replace the placeholder in template with generated content
                foreach (KeyValuePair<string, string> item in dictValue)
                {
                    document.ReplaceText(item.Key, item.Value ?? string.Empty, false);
                }

                //Load item list
                int _idx = 0;
                Novacode.Table _table0 = document.Tables[1];
                // _table0.AutoFit = AutoFit.Contents;
                //Add Contractor List
                var _order = (ViewOrder)dictObj["Order"];
                decimal _subTotal = 0;
                decimal _uomTotal = 0;
                if (_order != null)
                {
                    foreach (var _item in _order.OrderItems)
                    {
                        _idx++;
                        _subTotal = _subTotal + Math.Round(_item.unitPrice * (_item.scannedQty > 0 ? _item.scannedQty : _item.orderQty), 2);
                        _uomTotal = _uomTotal + (_item.scannedQty > 0 ? _item.scannedQty : _item.orderQty);
                        //define row
                        Row _myRow = _table0.InsertRow();
                        //column1
                        _myRow.Cells[0].Width = 30;
                        _myRow.Cells[0].Paragraphs.First().Append(_idx.ToString()).FontSize(9);
                        //column2
                        _myRow.Cells[1].Width = 250;
                        _myRow.Cells[1].Paragraphs.First().Append(_item.partName + "\n" + _item.country + "\n" + _item.scannedQtyStr + "\n").FontSize(9);            
                        //column3
                        _myRow.Cells[2].Width = 80;
                        _myRow.Cells[2].Paragraphs.First().Append(_item.orderQty.ToString() + _item.salesUm).FontSize(9).Alignment = Alignment.center;
                        //column 4
                        _myRow.Cells[3].Width = 80;
                        _myRow.Cells[3].Paragraphs.First().Append(_item.orderQty.ToString() + _item.salesUm).FontSize(9).Alignment = Alignment.center;
                        //column 5
                        _myRow.Cells[4].Width = 80;
                        _myRow.Cells[4].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice), 2).ToString())).FontSize(9).Alignment = Alignment.right;
                        //column 6
                        _myRow.Cells[5].Width = 80;
                        _myRow.Cells[5].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice * (_item.scannedQty > 0 ? _item.scannedQty : _item.orderQty)), 2)).ToString()).FontSize(9).Alignment = Alignment.right;
                        //column7
                        _myRow.Cells[6].Width = 80;
                        _myRow.Cells[6].Paragraphs.First().Append("0.00").FontSize(9).Alignment = Alignment.right;
                        //column8
                        _myRow.Cells[7].Width = 80;
                        _myRow.Cells[7].Paragraphs.First().Append(String.Format("{0:N2}", Math.Round((_item.unitPrice * (_item.scannedQty > 0 ? _item.scannedQty : _item.orderQty)), 2)).ToString()).FontSize(9).Alignment = Alignment.right;
                       
                        _table0.Rows.Add(_myRow);

                    }

                    document.ReplaceText("$Order_No$", _order.orderNum.ToString() ?? string.Empty, false);
                    document.ReplaceText("$Order_Date$", _order.orderDate.GetValueOrDefault().ToString("dd/MM/yyyy") ?? string.Empty, false);
                    document.ReplaceText("$Cashier$", string.IsNullOrEmpty(_order.verifiedBy) ? (_order.entryPerson??"") : "", false);
                    document.ReplaceText("$UOM_TOTAL$", Math.Round(_uomTotal,3).ToString(), false);

                    //Load Customer
                    var _customer = (Customers)dictObj["Customer"];
                    if (_customer != null)
                    {
                        string _csutomerAddress = GetFormattedAddress(_customer.name,
                            _customer.address1,
                            _customer.address2,
                            _customer.address3,
                            _customer.city,
                            "",
                            _customer.state,
                            _customer.country);

                        document.ReplaceText("$CustNum$", _customer.custID.ToString() ?? string.Empty, false);
                        document.ReplaceText("$CustomerAddress$", _csutomerAddress ?? string.Empty, false);
                        document.ReplaceText("$CUSTOMER_NAME$", _customer.name.ToUpper() ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$CustomerAddress$", "", false);
                    }

                    //Load ShipTo
                    var _shipTo = (ShipToAddresses)dictObj["ShipTo"];
                    if (_shipTo != null)
                    {
                        string _shipToAdress = GetFormattedAddress(_shipTo.shippingCode,
                            _shipTo.address1,
                            _shipTo.address2,
                            _shipTo.address3,
                            _shipTo.cityName,
                            _shipTo.zip,
                            _shipTo.stateName,
                            _shipTo.countryName);

                        document.ReplaceText("$ShipTo$", _shipToAdress ?? string.Empty, false);
                    }
                    else
                    {
                        document.ReplaceText("$ShipTo$", "", false);
                    }

                    document.ReplaceText("$lineSubTotal$", String.Format("{0:N2}", Math.Round(_subTotal, 2)), false);
                }
                // Save document to a MemoryStream
                MemoryStream docStream = new MemoryStream();
                document.SaveAs(docStream);
                docStream.Position = 0;

                // Create a Spire.Doc Document instance from the MemoryStream
                Spire.Doc.Document spireDoc = new Spire.Doc.Document();
                spireDoc.LoadFromStream(docStream, FileFormat.Docx);

                // Add watermark
                TextWatermark watermarks = new TextWatermark
                {
                    Text = watermark,
                    FontName = "Arial", // Specify the font name as a string
                    FontSize = 100,
                    Color = System.Drawing.Color.Red, // Set the color to Red
                    Layout = WatermarkLayout.Diagonal, // Optional: Diagonal layout
                     


                };
                spireDoc.Watermark = watermarks;

                // Save document to PDF
                MemoryStream outputStream = new MemoryStream();
                spireDoc.SaveToStream(outputStream, FileFormat.PDF);
                outputStream.Position = 0;

                var file = new FileStreamResult(outputStream, "application/pdf")
                {
                    FileDownloadName = _name[0] + ".pdf"
                };

                

                return file;

            }
        }

        public static string GetFormattedAddress(string name, string address1, string address2, string address3, string city, string postcode,
            string state, string country)
        {
            string _address = "";
            char[] MyChar = { ',', ' ', '-' };

            address1 = address1 == null ? "" : address1.TrimStart(MyChar).TrimEnd(MyChar);
            address2 = address2 == null ? "" : address2.TrimStart(MyChar).TrimEnd(MyChar);
            address3 = address3 == null ? "" : address3.TrimStart(MyChar).TrimEnd(MyChar);
            city = city == null ? "" : city.TrimStart(MyChar).TrimEnd(MyChar);
            postcode = postcode == null ? "" : postcode.TrimStart(MyChar).TrimEnd(MyChar);
            state = state == null ? "" : state.TrimStart(MyChar).TrimEnd(MyChar);
            country = country == null ? "" : country.TrimStart(MyChar).TrimEnd(MyChar);

            if (!String.IsNullOrEmpty(name))
                _address += name.ToUpper() + "\n";

            if (!String.IsNullOrEmpty(address1))
                _address += address1.ToUpper() + "\n";

            if (!String.IsNullOrEmpty(address2))
                _address += address2 + "\n";

            if (!String.IsNullOrEmpty(address3))
                _address += address3 + " ";

            if (city != state)
            {
                if (!String.IsNullOrEmpty(city))
                    _address += postcode + " " + city + "\n";

                if (state != "HQ")
                {
                    if (!String.IsNullOrEmpty(state))
                        _address += ", " + state + "\n";
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(city))
                    _address += postcode + " " + state + "\n";
            }

            _address += country + "\n";
            _address = _address.TrimEnd(MyChar).ToUpper();

            return _address;

        }

    }
}
