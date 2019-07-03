using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Aspose.Cells;
using fk.Models;

namespace fk.Utils
{
    public class TableCreator
    {
        static int row = 0;

        public static byte[] CreateTable(List<Apartment> apartments)
        {
            Workbook workbook = new Workbook();
            workbook.Worksheets.Add(SheetType.Worksheet);
            Worksheet workSheet = workbook.Worksheets[0];

            workSheet.Cells[0, 0].Value = "Адрес";
            workSheet.Cells[0, 1].Value = "Цена";
            workSheet.Cells[0, 2].Value = "Площадь";
            workSheet.Cells[0, 3].Value = "Количество комнат";

            AddList(apartments, workSheet);
            workSheet.AutoFitColumn(0);
            workSheet.AutoFitColumn(1);
            workSheet.AutoFitColumn(2);
            workSheet.AutoFitColumn(3);

            MemoryStream ms = new MemoryStream();
            workbook.Save(ms, SaveFormat.Excel97To2003);
            ms.Seek(0, SeekOrigin.Begin);

            return ms.ToArray();
        }

        public static void AddList(List<Apartment> apartments, Worksheet workSheet)
        {
            foreach (Apartment ap in apartments)
            {
                row++;
                workSheet.Cells[row, 0].Value = ap.Address;
                workSheet.Cells[row, 1].Value = ap.Price;
                workSheet.Cells[row, 2].Value = ap.Square;
                workSheet.Cells[row, 3].Value = ap.Rooms;
            }
        }



    }
}

