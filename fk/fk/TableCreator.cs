using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace fk
{
    public class TableCreator
    {
        int row = 1;

        public void CreateTable(List<Apartment> apartments)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Workbooks.Add();
            Excel.Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            workSheet.Cells[1, "A"] = "Адрес";
            workSheet.Cells[1, "B"] = "Цена";
            workSheet.Cells[1, "C"] = "Площадь";
            workSheet.Cells[1, "D"] = "Количество комнат";
            workSheet.Cells[1, "E"] = "Район";

            AddList(apartments, workSheet);
            excelApp.DisplayAlerts = false;
            workSheet.Columns.AutoFit();

            workSheet.SaveAs(
                Directory.GetCurrentDirectory() + "/table.xlsx", 
                XlFileFormat.xlWorkbookDefault, 
                Type.Missing, 
                Type.Missing, 
                true, 
                false, 
                XlSaveAsAccessMode.xlNoChange, 
                XlSaveConflictResolution.xlLocalSessionChanges, 
                Type.Missing, 
                Type.Missing
            );

            excelApp.Quit();

        }

        public void AddList(List<Apartment> apartments, Excel.Worksheet workSheet)
        {

            foreach (Apartment ap in apartments)
            {
                row++;
                workSheet.Cells[row, "A"] = ap.Address;
                workSheet.Cells[row, "B"] = ap.Price;
                workSheet.Cells[row, "C"] = ap.Square;
                workSheet.Cells[row, "D"] = ap.Rooms;
                workSheet.Cells[row, "E"] = ap.District;
            }
        }



    }
}

