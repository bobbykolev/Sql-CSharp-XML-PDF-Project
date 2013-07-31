using iTextSharp.text;
using iTextSharp.text.pdf;
using MusicStore.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFExporter
{
    public class PDFExporter
    {
        static void Main()
        {
           
        }
        public static void ExportDataToPDF(string startDate, string endDate)
        {
            using (var context = new MusicStoreEntities())
            {
                Document doc = new Document();
                FileStream file = File.Create("../../../AlbumsQuery.pdf");
                PdfWriter.GetInstance(doc, file);
                doc.Open();

                //set table
                PdfPTable table = new PdfPTable(4);
                table.TotalWidth = 440f;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 160f, 120f, 70f };
                table.SetWidths(widths);

                PdfPCell cell = new PdfPCell();

                //set fonts
                BaseFont bfTimes = BaseFont.CreateFont("../../../segoeui.ttf", BaseFont.CP1252, false);
                Font normal = new Font(bfTimes, 10);
                Font bold = new Font(bfTimes, 11, Font.BOLD);

                //title cell
                cell = new PdfPCell(new Phrase("Albums Between: " + startDate +" - " + endDate , bold));
                cell.Colspan = 4;
                cell.HorizontalAlignment = 1;
                cell.BackgroundColor = new BaseColor(135, 196, 28);
                cell.PaddingTop = 10f;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);

                DateTime sD = DateTime.ParseExact(startDate, "yyyy", CultureInfo.InvariantCulture);
                DateTime eD = DateTime.ParseExact(endDate, "yyyy", CultureInfo.InvariantCulture);
                //the query from the DB                   
                var query = (from al in context.Albums
                             join art in context.Artists on al.ArtistID equals art.ArtistID
                             join p in context.Producers on al.ProducerID equals p.ProducerID
                             join s in context.Songs on al.AlbumID equals s.AlbumID
                             where al.Date >= sD && al.Date <= eD
                             select new
                             {
                                 al.AlbumName,
                                 al.Date,
                                 al.Price,
                                 art.ArtistName,
                                 p.ProducerName,
                                 s.SongName,
                                 s.Duration,
                                 Genres = al.Genres.Select(g => g.GenreName)
                             })
                             .OrderBy(x => x.ArtistName).ThenBy(x => x.Date);

                //helpers
                bool isFirst = true;
                string albName = string.Empty;
                TimeSpan totalDur = new TimeSpan(0, 0, 0);
                TimeSpan lastTotalDur = new TimeSpan(0, 0, 0);
                decimal? lastPrice = 0.00m;
                int index = 1;
                List<string> firstGenres = new List<string>();

                foreach (var item in query)
                {
                    //only for firs header
                    if (isFirst)
                    {    
                        //helpers
                        albName = item.AlbumName;
                        isFirst = false;

                        //empty row
                        cell = new PdfPCell(new Phrase(" "));
                        cell.Colspan = 4;
                        cell.BackgroundColor = new BaseColor(220, 237, 191);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //artist info
                        cell = new PdfPCell(new Phrase("Artist: " + item.ArtistName, normal));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //album info
                        cell = new PdfPCell(new Phrase("Album: " + item.AlbumName, normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //date 
                        cell = new PdfPCell(new Phrase("Date: " + item.Date.ToString("dd-mm-yyyy"), normal));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //producer
                        cell = new PdfPCell(new Phrase("Producer: " + item.ProducerName, normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"N"
                        cell = new PdfPCell(new Phrase("N", normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"Tracks"
                        cell = new PdfPCell(new Phrase("Tracks", normal));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"Duration"
                        cell = new PdfPCell(new Phrase("Duration", normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);
                    }

                    //all data from single album 
                    if (albName == item.AlbumName)
                    {
                        //tracks data
                        //index
                        cell = new PdfPCell(new Phrase(index.ToString(), normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        //song name
                        cell = new PdfPCell(new Phrase(item.SongName, normal));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        //song duration
                        cell = new PdfPCell(new Phrase(item.Duration.ToString(@"mm\:ss"), normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        table.AddCell(cell);
                        index++;
                        totalDur += TimeSpan.Parse(item.Duration.ToString());

                        //helpers
                        lastPrice = item.Price;
                        lastTotalDur = totalDur;

                        //genres 
                        firstGenres = string.Join(", ", item.Genres.ToList()).Split(',').ToList();
                        for (int i = 0; i < firstGenres.Count; i++)
                        {
                            firstGenres[i] = firstGenres[i].Trim();
                        }
                    }
                    //end of an album (footer) and start of a new one
                    else
                    {
                        index = 1;

                        //---------footer except the last one
                        //price info
                        cell = new PdfPCell(new Phrase("Price: " + lastPrice.Value.ToString("00.00"), normal));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //total duration info
                        cell = new PdfPCell(new Phrase("Total Duration: " + totalDur.ToString(@"hh\:mm\:ss"), normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //genres 
                        cell = new PdfPCell(new Phrase("Genre: " + string.Join(", ", firstGenres), normal));
                        cell.Colspan = 4;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        albName = item.AlbumName;
                        totalDur = new TimeSpan(0, 0, 0);

                        //-------------start new album info----------------------
                        //air info
                        cell = new PdfPCell(new Phrase(" "));
                        cell.Colspan = 4;
                        cell.BackgroundColor = new BaseColor(250, 250, 250);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //artist info
                        cell = new PdfPCell(new Phrase("Artist: " + item.ArtistName, normal));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //album info
                        cell = new PdfPCell(new Phrase("Album: " + item.AlbumName, normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //date 
                        cell = new PdfPCell(new Phrase("Date: " + item.Date.ToString("dd-mm-yyyy"), normal));
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.HorizontalAlignment = 0;
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //producer
                        cell = new PdfPCell(new Phrase("Producer: " + item.ProducerName, normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"N"
                        cell = new PdfPCell(new Phrase("N", normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"Tracks"
                        cell = new PdfPCell(new Phrase("Tracks", normal));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 2;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //"Duration"
                        cell = new PdfPCell(new Phrase("Duration", normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        cell.BackgroundColor = new BaseColor(135, 196, 28);
                        cell.PaddingBottom = 5f;
                        cell.PaddingLeft = 5f;
                        table.AddCell(cell);

                        //----------------first row of data on evry album except the first one
                        //tracks data
                        //index
                        cell = new PdfPCell(new Phrase(index.ToString(), normal));
                        cell.HorizontalAlignment = 1;
                        cell.Colspan = 1;
                        table.AddCell(cell);

                        //song name
                        cell = new PdfPCell(new Phrase(item.SongName, normal));
                        cell.HorizontalAlignment = 0;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        //song duration
                        cell = new PdfPCell(new Phrase(item.Duration.ToString(@"mm\:ss"), normal));
                        cell.HorizontalAlignment = 2;
                        cell.Colspan = 2;
                        table.AddCell(cell);

                        index++;
                        totalDur += TimeSpan.Parse(item.Duration.ToString());

                        //the genres except for first album
                        firstGenres = string.Join(", ", item.Genres.ToList()).Split(',').ToList();
                        for (int i = 0; i < firstGenres.Count; i++)
                        {
                            firstGenres[i] = firstGenres[i].Trim();
                        }
                    }
                }
                //the final price, duration and genre (last footer)
                //price info
                cell = new PdfPCell(new Phrase("Price: " + lastPrice.Value.ToString("00.00"), normal));
                cell.Colspan = 2;
                cell.BackgroundColor = new BaseColor(135, 196, 28);
                cell.HorizontalAlignment = 0;
                cell.PaddingBottom = 5f;
                cell.PaddingLeft = 5f;
                table.AddCell(cell);

                //total duration info
                cell = new PdfPCell(new Phrase("Total Duration: " + lastTotalDur.ToString(@"hh\:mm\:ss"), normal));
                cell.HorizontalAlignment = 2;
                cell.Colspan = 2;
                cell.BackgroundColor = new BaseColor(135, 196, 28);
                cell.PaddingBottom = 5f;
                cell.PaddingLeft = 5f;
                table.AddCell(cell);

                //ganres info
                cell = new PdfPCell(new Phrase("Genre: " + string.Join(", ", firstGenres), normal));
                cell.Colspan = 4;
                cell.BackgroundColor = new BaseColor(135, 196, 28);
                cell.HorizontalAlignment = 0;
                cell.PaddingBottom = 5f;
                cell.PaddingLeft = 5f;
                table.AddCell(cell);

                doc.Add(table);
                doc.Close();
            }
        }
    }
}
