using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using Omu.AwesomeMvc;
using dFrontierAppWizard.Core.Model;
using dFrontierAppWizard.Core.Repository;
using dFrontierAppWizard.Core.Service;
using dFrontierAppWizard.WebUI.Dto;
using dFrontierAppWizard.WebUI.Mappers;

namespace dFrontierAppWizard.WebUI.Controllers
{
    public class TransactionController : Cruder<Transaction, TransactionInput>
    {

        public TransactionController(ICrudService<Transaction> service, IMapper<Transaction, TransactionInput> v)
            : base(service, v)
        {
        
        }

        //public ActionResult Index()
        //{

        //    return View();
        //}

        protected override string RowViewName
        {
            get { return "ListItems/Transaction"; }
        }


        public ActionResult ExportTransaction()
        {

            var ExportTransaction = service.GetAll();

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


            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("PaymentModeId");
            //headerRow.CreateCell(1).SetCellValue("FirstName");
            //headerRow.CreateCell(2).SetCellValue("EmailAddress");
            //headerRow.CreateCell(3).SetCellValue("ReferredBy");
            //headerRow.CreateCell(4).SetCellValue("DateCreated");


            ////(Optional) freeze the header row so it is not scrolled
            //sheet.CreateFreezePane(0, 1, 0, 1);

            //int rowNumber = 1;

            ////Populate the sheet with values from the grid data
            //foreach (var exportLead in ExportLead)
            //{
            //    //Create a new row
            //    var row = sheet.CreateRow(rowNumber++);

            //    //Set values for the cells
            //    row.CreateCell(0).SetCellValue(exportLead.LastName);
            //    row.CreateCell(1).SetCellValue(exportLead.FirstName);
            //    row.CreateCell(2).SetCellValue(exportLead.EmailAddress);
            //    row.CreateCell(3).SetCellValue(exportLead.ReferredBy);
            //    row.CreateCell(4).SetCellValue(exportLead.DateCreated);

            //}
            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            ////Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "Transactions.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
        

       
    }
}
