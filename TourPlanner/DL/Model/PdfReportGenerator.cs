using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DL.DB;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

public class PdfReportGenerator
{
    public void GenerateReport(string filePath, TourPlannerDbContext context)
    {
        using (var pdfWriter = new PdfWriter(filePath))
        {
            using (var pdfDocument = new PdfDocument(pdfWriter))
            {
                var document = new Document(pdfDocument);

                // Add title 
                var title = new Paragraph("Tour and TourLog Report")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                    .SetBold();
                document.Add(title);

                // Add Tours section
                var toursHeader = new Paragraph("Tours")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetBold();
                document.Add(toursHeader);

                var toursTable = new Table(4)
                    .UseAllAvailableWidth()
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                // Add table headers
                toursTable.AddHeaderCell("Tour ID");
                toursTable.AddHeaderCell("Tour Name");
                toursTable.AddHeaderCell("Distance");
                toursTable.AddHeaderCell("Description");

                var tours = context.Tours.ToList();
                foreach (var tour in tours)
                {
                    toursTable.AddCell(tour.Tour_id.ToString());
                    toursTable.AddCell(tour.Name);
                    toursTable.AddCell(tour.Distance.ToString());
                    toursTable.AddCell(tour.Description);
                }

                document.Add(toursTable);

                // Add TourLogs section
                var tourLogsHeader = new Paragraph("TourLogs")
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(14)
                    .SetBold();
                document.Add(tourLogsHeader);

                var tourLogsTable = new Table(6)
                    .UseAllAvailableWidth()
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                // Add table headers
                tourLogsTable.AddHeaderCell("Log ID");
                tourLogsTable.AddHeaderCell("Tour ID");
                tourLogsTable.AddHeaderCell("Comment");
                tourLogsTable.AddHeaderCell("Difficulty");
                tourLogsTable.AddHeaderCell("Rating");
                tourLogsTable.AddHeaderCell("DateTime");

                var tourLogs = context.TourLogs.ToList();
                foreach (var tourLog in tourLogs)
                {
                    tourLogsTable.AddCell(tourLog.TourLog_id.ToString());
                    tourLogsTable.AddCell(tourLog.Tour_id.ToString());
                    tourLogsTable.AddCell(tourLog.Comment);
                    tourLogsTable.AddCell(tourLog.Difficulty);
                    tourLogsTable.AddCell(tourLog.Rating.ToString());
                    tourLogsTable.AddCell(tourLog.DateTime.ToString());
                }

                document.Add(tourLogsTable);

                document.Close();
            }
        }
    }
}
