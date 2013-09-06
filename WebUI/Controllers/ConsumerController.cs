using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NPOI.HSSF.UserModel;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;
using dFrontierAppWizard.Data;
//using Microsoft.Office.Interop.Excel;
using dFrontierAppWizard.WebUI.Utils;

namespace dFrontierAppWizard.WebUI.Controllers
{
  
    public class ConsumerController : Cruder<Consumer, ConsumerInput>
    {
        private new readonly IRepo<Consumer> consumerService;
        private new readonly IRepo<ConsumerFeedback> consumerFeedbackService;
        public ConsumerController(ICrudService<Consumer> service, IMapper<Consumer, ConsumerInput> v, IRepo<Consumer> consumerService, IRepo<ConsumerFeedback> consumerFeedbackService)
            : base(service, v)
        {
            this.consumerService = consumerService;
            this.consumerFeedbackService = consumerFeedbackService;
        }

        protected override string RowViewName
        {
            get { return "ListItems/Consumer"; }
        }

        [HttpPost]
        public string RegisterConsumer(string name, string birthDate, int age, string gender, string email, long phone, string address, string deviceId, string country, string city)
        {
            var consumerDupEmail = consumerService.Where(o => o.Email.Equals(email)).AsEnumerable();

            if (consumerDupEmail.Any())
            {
                string sJson3 = "Account already exists. You may want to login instead.";
                sJson3 = @"{""Data"":" + sJson3 + @",""success"":false}";
                return sJson3;
            }


            if (name.Length > 0 && age > 0 && phone > 0 && address.Length > 0 && email.Length >0)
            { 
                DateTime dtBdate = Convert.ToDateTime(birthDate); 
                var cons = service.Create(new Consumer
                    {
                        Name = name,
                        Age = age,
                        BirthDate = dtBdate,
                        Gender = gender,
                        Email = email,
                        Phone = phone,
                        Address = address,                       
                        DateRegistered=DateTime.Now,
                        DeviceId = deviceId, 
                        City = city,
                        Country = country
                    });
              
                 service.Save();

                 using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString))
                 using (var command = new SqlCommand("sp_send_dbmail", conn)
                 {
                     CommandType = CommandType.StoredProcedure
                 })
                 {
                     conn.Open();
                     command.Parameters.Add(new SqlParameter("@profile_name", "DBMail Profile"));
                     command.Parameters.Add(new SqlParameter("@recipients", email));
                     command.Parameters.Add(new SqlParameter("@subject", "Registration for Application ..."));
                     command.Parameters.Add(new SqlParameter("@from_address", ConfigurationManager.AppSettings["EmailHost"]));
                     command.Parameters.Add(new SqlParameter("@Body", "Welcome to testAppname"));
                     command.ExecuteNonQuery();
                     conn.Close();
                 }
                
                var consumer = consumerService.Where(o => o.Email.Equals(email)).AsEnumerable();
               
                var query = from seletedConsumer in consumer
                            select new
                            {
                                seletedConsumer.Address,
                                seletedConsumer.Age,
                                BirthDate=DateConverter.GetJsonDate(seletedConsumer.BirthDate),
                                DateRegistered =DateConverter.GetJsonDate( seletedConsumer.DateRegistered),
                                seletedConsumer.Email,
                                seletedConsumer.Gender,
                                seletedConsumer.Name,
                                seletedConsumer.Phone,
                                seletedConsumer.Picture,
                                seletedConsumer.Token,
                                seletedConsumer.DeviceId,
                                seletedConsumer.Country,
                                seletedConsumer.City
                            };

                var oSerializer = new JavaScriptSerializer();
                string sJson = oSerializer.Serialize(query);
                sJson = @"{""Data"":" + sJson + @",""success"":true}";
                return sJson;
             }
            else
            {
                string sJson2 = "Some required fields are empty.";
                sJson2 = @"{""Data"":" + sJson2 + @",""success"":false}";
                return sJson2;
            }
            string sJson1 = "";
            sJson1 = @"{""Data"":" + sJson1 + @",""success"":false}";
            return sJson1;

        }

        [HttpPost]
        public string GetConsumer(string email)
        {
            var consumer = service.Where(o => o.Email.Equals(email)).SingleOrDefault();
            if (consumer != null)
            {
                var con = consumerService.Where(o => o.Email.Equals(email)).SingleOrDefault();
                string userToken = System.Guid.NewGuid().ToString();
             
                Consumer selectedConsumer= consumerService.Get(con.Id);
                selectedConsumer.Token = userToken;
                consumerService.Save();

                var list = service.Where(o => o.Email.Equals(email)).AsEnumerable();
                var query = from seletedConsumer in list
                            select new
                            {   
                                seletedConsumer.Address,
                                seletedConsumer.Age,
                                BirthDate=DateConverter.GetJsonDate(seletedConsumer.BirthDate),
                                DateRegistered = DateConverter.GetJsonDate(seletedConsumer.DateRegistered),
                                seletedConsumer.Email,
                                seletedConsumer.Gender,
                                seletedConsumer.Name,
                                seletedConsumer.Phone,
                                seletedConsumer.Picture,
                                seletedConsumer.Token,
                                seletedConsumer.DeviceId,
                                seletedConsumer.City,
                                seletedConsumer.Country
                               
                            };

                var oSerializer = new JavaScriptSerializer();
                string sJson = oSerializer.Serialize(query);
                sJson = @"{""Data"":" + sJson + @",""success"":true}";
                return sJson;
            }
            
            return null;
        }

        public ActionResult IndexNoHeader()
        {
            return View();
        }


        public ActionResult ExportConsumerProfile()
        {

            var exportConsumerProfile= service.GetAll().OrderBy(o => o.DateRegistered);

            ////Create new Excel workbook
            var workbook = new HSSFWorkbook();

            ////Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 40 * 256);
            sheet.SetColumnWidth(1, 15 * 256);
            sheet.SetColumnWidth(2, 15 * 256);
            sheet.SetColumnWidth(3, 25 * 256);
            sheet.SetColumnWidth(4, 25 * 256);
            sheet.SetColumnWidth(5, 35 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 200 * 256);
            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("Name");
            headerRow.CreateCell(1).SetCellValue("Age");
            headerRow.CreateCell(2).SetCellValue("Gender");
            headerRow.CreateCell(3).SetCellValue("Email");
            headerRow.CreateCell(4).SetCellValue("Phone");
            headerRow.CreateCell(5).SetCellValue("Address");
            headerRow.CreateCell(6).SetCellValue("DateRegistered");
            headerRow.CreateCell(7).SetCellValue("DeviceId");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var exportConsumer in exportConsumerProfile)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(exportConsumer.Name);
                row.CreateCell(1).SetCellValue(exportConsumer.Age);
                row.CreateCell(2).SetCellValue(exportConsumer.Gender);
                row.CreateCell(3).SetCellValue(exportConsumer.Email);
                row.CreateCell(4).SetCellValue(exportConsumer.Phone);
                row.CreateCell(5).SetCellValue(exportConsumer.Address);
                row.CreateCell(6).SetCellValue(exportConsumer.DateRegistered);
                row.CreateCell(7).SetCellValue(exportConsumer.DeviceId);
            }
            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ConsumerProfile.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
       
        }

        public ActionResult ExportConsumerFeedbacks()
        {

            var exportConsumerFeedback= consumerFeedbackService.GetAll().OrderBy(o => o.DatePosted);

            ////Create new Excel workbook
            var workbook = new HSSFWorkbook();

            ////Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 40 * 256);
            sheet.SetColumnWidth(1, 15 * 256);
            sheet.SetColumnWidth(2, 15 * 256);
            sheet.SetColumnWidth(3, 15 * 256);


            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("Consumer");
            headerRow.CreateCell(1).SetCellValue("Comment");
            headerRow.CreateCell(2).SetCellValue("Status");
            headerRow.CreateCell(2).SetCellValue("Date Created");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var expConsumerFeedback in exportConsumerFeedback)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(expConsumerFeedback.Consumer.Name);
                row.CreateCell(1).SetCellValue(expConsumerFeedback.Comments);
                row.CreateCell(1).SetCellValue(expConsumerFeedback.Status);
                row.CreateCell(2).SetCellValue(expConsumerFeedback.DatePosted);
                

            }
            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "ConsumerFeedback.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
    }
}
