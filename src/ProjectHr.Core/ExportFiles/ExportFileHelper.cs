﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using ProjectHr.Enums;

namespace ProjectHr.ExportFiles;

public class ExportFileHelper
{
    public byte[] ExportFile<T>(DataConverter dataConverter, List<T> datas, string[] columnsName, string[] columns) where T : class
    {
        if (dataConverter == DataConverter.Excel)
            return ExportExcel(datas, columns, columnsName);

        if (dataConverter == DataConverter.Csv)
            return ExportCsv(datas, columns, columnsName);

        return ExportPdf(datas, columns, columnsName);
        // return null;
    }

    private byte[] ExportExcel<T>(List<T> datas, string[] columns, string[] columnsName)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("SampleSheet");

            // Write column headers
            for (int columnIndex = 0; columnIndex < columnsName.Length; columnIndex++)
            {
                var columnHeader = columnsName[columnIndex];
                worksheet.Cell(1, columnIndex + 1).Value = columnHeader;
            }

            // Write data rows
            for (int rowIndex = 0; rowIndex < datas.Count; rowIndex++)
            {
                var dataItem = datas[rowIndex];
                for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                {
                    var cellValue = GetValueByPath(datas[rowIndex], columns[columnIndex]);
                    worksheet.Cell(rowIndex + 2, columnIndex + 1).Value = cellValue;
                }
            }

            // Save the Excel workbook to a stream
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                // Set the position of the stream to the beginning
                stream.Position = 0;
                var bytes = stream.ToArray();

                return bytes;
            }
        }
    }

    private byte[] ExportCsv<T>(List<T> datas, string[] columns, string[] columnsName)
    {
        // CSV içeriğini oluşturma
        var csvContent = new StringBuilder();
        csvContent.AppendLine(ConvertTurkishChars(string.Join(" ", columnsName)));

        foreach (var data in datas)
        {
            foreach (var column in columns)
            {
                string value = GetValueByPath(data, column);
                csvContent.Append($"{ConvertTurkishChars(value)} ");
            }

            csvContent.AppendLine();
        }

        var bytes = Encoding.UTF8.GetBytes(csvContent.ToString());

        return bytes;
    }

    public byte[] ExportPdf<T>(List<T> datas, string[] columns, string[] columnsName)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont headerFont = new XFont("Arial", 12, XFontStyle.Bold);
            XFont dataFont = new XFont("Arial", 12, XFontStyle.Regular);

            // Draw column headers
            double xPosition = 50;
            double yPosition = 50;

            foreach (string column in columnsName)
            {
                gfx.DrawString(column, headerFont, XBrushes.Black, xPosition, yPosition);
                xPosition += 85;
            }

            yPosition += 20;

            // Draw data rows
            foreach (T data in datas)
            {
                xPosition = 50;

                foreach (string column in columns)
                {
                    string value = GetValueByPath(data, column);
                    gfx.DrawString(value, dataFont, XBrushes.Black, xPosition, yPosition);
                    xPosition += 70;
                }

                yPosition += 20;
            }

            // Save the document to memory stream
            document.Save(ms);
            ms.Position = 0;

            return ms.ToArray();
        }
    }

    private static string GetValueByPath(object source, string path)
    {
        string[] segments = path.Split('.');
        object current = source;

        foreach (string segment in segments)
        {
            PropertyInfo property = current.GetType().GetProperty(segment);
            if (property == null)
            {
                return null; 
            }
            current = property.GetValue(current, null);
        }

        return current.ToString();
    }
    
    private static string ConvertTurkishChars(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        string result = input
            .Replace("ğ", "g")
            .Replace("İ", "I")
            .Replace("Ö", "O")
            .Replace("ö", "o")
            .Replace("ü", "u")
            .Replace("Ü", "U")
            .Replace("Ğ", "G")
            .Replace("ı", "i")
            .Replace("Ç", "C")
            .Replace("ç", "c")
            .Replace("Ş", "S")
            .Replace("ş", "s");

        return result;
    }
}