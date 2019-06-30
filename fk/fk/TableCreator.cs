using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;

namespace fk
{
    public class TableCreator
    {
        static int row = 1;

        public static void CreateTable(List<Apartment> apartments)
        {
            Application excelApp = new Application();
            excelApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)excelApp.ActiveSheet;

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

        public static void AddList(List<Apartment> apartments, Worksheet workSheet)
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

