using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using ArtBiathlon.DataEntity;
using System.Linq;
using ArtBiathlon.Models;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using iTextSharp.tool.xml.html.table;

namespace ArtBiathlon.Controllers
{
    public class PdfController : Controller
    {

        ApplicationDbContext _context;
        static int? _id = null;
        static TimeInterval? _timeInterval = null;
        public PdfController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult HrvIndicatorsResults()
        {   
            var userHrvIndicators = GetUserHrvIndicators();
            var userHrvIndicatorsResults = GetHRVIndicatorResults();

            ViewBag.UserHrvIndicators = userHrvIndicators;
            ViewBag.UserHrvIndicatorsResults = userHrvIndicatorsResults;
            return View();
        }

        [HttpGet]
        public IActionResult TrainingsList()
        {
            var selectTrainingList = SelectTrainingList(null);
            var trainingsTypes = _context.TrainingTypes.ToList();

            ViewBag.TrainingsTypeList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(trainingsTypes, "Id", "NameType");
            ViewBag.SelectTrainingList = selectTrainingList;

            return View();
        }

        [HttpPost]
        public IActionResult TrainingsList(int id)
        {
            _id = id;
            var selectTrainingList = SelectTrainingList(_id);

            var trainingsTypes = _context.TrainingTypes.ToList();

            ViewBag.TrainingsTypeList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(trainingsTypes, "Id", "NameType");
            ViewBag.SelectTrainingList = selectTrainingList;
            
            return View();
        }

        [NonAction]
        private List<TrainingViewModel> SelectTrainingList(int ? id)
        {
            var trainingsViewModelList =
               _context.Trainings.Select
                (x => new TrainingViewModel()
                {
                    Id = x.Id,
                    NameTraining = x.NameTraining,
                    NameType = _context.TrainingTypes.FirstOrDefault(y => y.Id == x.IdType).NameType

                }).ToList();

            if (id is null)
            {
                return trainingsViewModelList;
            }

            var selectTrainingList = trainingsViewModelList
                .Where(x => x.NameType == _context.TrainingTypes.FirstOrDefault( y => y.Id == id)?.NameType)
                .ToList();

            return selectTrainingList;
        }

        [HttpPost]
        public FileResult ExportHrvIndicatorsPdf()
        {
            Document Doc = new Document(PageSize.LETTER);

            using (var fs = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                Doc.NewPage();

                string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");

                BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font f = new Font(bf, 12, Font.NORMAL);

                var userHrvIndicators = GetUserHrvIndicators();
                var userHrvIndicatorsResults = GetHRVIndicatorResults();
            

                int columnCount = typeof(HrvIndicator).GetProperties().Length - 1;
                PdfPTable table = new PdfPTable(columnCount);
                table.SetWidths(new float[]{6f, 6f, 4f, 5f, 4f, 4f, 5f, 5f, 5f, 3f, 3f});
                table.WidthPercentage = 110f;
                
                PdfPTable table1 = new PdfPTable(1);
                Font font2 = new Font(bf, 20, Font.BOLD);
                //Set the biography  
                PdfPCell cell1 = new PdfPCell()
                {
                    BorderWidthBottom = 0f,
                    BorderWidthTop = 0f,
                    BorderWidthLeft = 0f,
                    BorderWidthRight = 0f
                };
                cell1.PaddingTop = 5f;
                Phrase bioPhrase = new Phrase();
                Chunk bioChunk = new Chunk("ArtBiathlon | Данные по HRV-показателям спортсмена", font2);
                bioPhrase.Add(bioChunk);
                bioPhrase.Add(new Chunk(Environment.NewLine));

                bioPhrase.Add(new Chunk(Environment.NewLine));

                cell1.AddElement(bioPhrase);

                table1.AddCell(cell1);
                Doc.Add(table1);

                PdfPCell cell = new PdfPCell(new Phrase("HRV-показатели спортсмена", f));
                cell.Colspan = columnCount;
               
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                
                
                table.AddCell("Date");
                table.AddCell("READINESS");
                table.AddCell("RMSSD");
                table.AddCell("RR");
                table.AddCell("SDNN");
                table.AddCell("SD1");
                table.AddCell("TP");
                table.AddCell("HF");
                table.AddCell("LF");
                table.AddCell("SI");
                table.AddCell("LFHF");


                foreach (var hrvIndicator in userHrvIndicators)
                {
                    table.AddCell(hrvIndicator.Date.ToShortDateString());
                    table.AddCell(hrvIndicator.READINESS.ToString("0.00"));
                    table.AddCell(hrvIndicator.RMSSD.ToString());
                    table.AddCell(hrvIndicator.RR.ToString());
                    table.AddCell(hrvIndicator.SDNN.ToString());
                    table.AddCell(hrvIndicator.SD1.ToString());
                    table.AddCell(hrvIndicator.TP.ToString());
                    table.AddCell(hrvIndicator.HF.ToString());
                    table.AddCell(hrvIndicator.LF.ToString());
                    table.AddCell(hrvIndicator.SI.ToString("0.00"));
                    table.AddCell(hrvIndicator.LFHF.ToString("0.00"));
                }

                int i = 0;
                foreach (var hrvIndicator in userHrvIndicatorsResults)
                {
                    table.AddCell((i == 0) ? "Min" : (i == 1) ? "Avg" : "Max");
                    table.AddCell(hrvIndicator.READINESS.ToString("0.00"));
                    table.AddCell(hrvIndicator.RMSSD.ToString("0.00"));
                    table.AddCell(hrvIndicator.RR.ToString("0.00"));
                    table.AddCell(hrvIndicator.SDNN.ToString("0.00"));
                    table.AddCell(hrvIndicator.SD1.ToString("0.00"));
                    table.AddCell(hrvIndicator.TP.ToString("0.00"));
                    table.AddCell(hrvIndicator.HF.ToString("0.00"));
                    table.AddCell(hrvIndicator.LF.ToString("0.00"));
                    table.AddCell(hrvIndicator.SI.ToString("0.00"));
                    table.AddCell(hrvIndicator.LFHF.ToString("0.00"));

                    i++;
                }

                Doc.Add(table);
                Doc.Close();

                return File(fs.ToArray(), "application/pdf", "ExportData.pdf");
             
            }
        }

        [HttpPost]
        public FileResult ExportTrainingListPdf()
        {
            Document Doc = new Document(PageSize.LETTER);

            using (var fs = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                Doc.NewPage();

                string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");

                BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font f = new Font(bf, 12, Font.NORMAL);

                var selectTrainingList = SelectTrainingList(_id);
               
                int columnCount = typeof(TrainingViewModel).GetProperties().Length;
                PdfPTable table = new PdfPTable(columnCount);


                PdfPTable table1 = new PdfPTable(1);
                Font font2 = new Font(bf, 20, Font.BOLD);
                //Set the biography  
                PdfPCell cell1 = new PdfPCell()
                {
                    BorderWidthBottom = 0f,
                    BorderWidthTop = 0f,
                    BorderWidthLeft = 0f,
                    BorderWidthRight = 0f
                };
                cell1.PaddingTop = 5f;
                Phrase bioPhrase = new Phrase();
                Chunk bioChunk = new Chunk("ArtBiathlon | Список доступных тренировок по выбранному типу", font2);
                bioPhrase.Add(bioChunk);
                bioPhrase.Add(new Chunk(Environment.NewLine));

                bioPhrase.Add(new Chunk(Environment.NewLine));

                cell1.AddElement(bioPhrase);

                table1.AddCell(cell1);
                Doc.Add(table1);


                PdfPCell cell = new PdfPCell(new Phrase("Список Тренировок", f));
                cell.Colspan = columnCount;
                
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell(new PdfPCell(new Phrase("№", f)));
                table.AddCell(new PdfPCell(new Phrase("Вид Тренировки", f)));
                table.AddCell(new PdfPCell(new Phrase("Тип Тренировки", f)));
                foreach (var training in selectTrainingList)
                {
                    table.AddCell(new PdfPCell(new Phrase(training.Id.ToString(), f)));
                    table.AddCell(new PdfPCell(new Phrase(training.NameType, f)));
                    table.AddCell(new PdfPCell(new Phrase(new Phrase(training.NameTraining, f))));
                }

                Doc.Add(table);
                Doc.Close();

                return File(fs.ToArray(), "application/pdf", "ExportData.pdf");
            }
        }

        [NonAction]
        private List<HrvIndicator> GetUserHrvIndicators()
        {
            var userHrvIndicators = _context.HrvIndicators
                .Where(x => x.IdBiathlete == _context.Users.FirstOrDefault(y => y.Login == User.Identity.Name).Id)
                .ToList();
            return userHrvIndicators;
        }

        [NonAction]
        private List<HRVIndicatorResultsViewModel> GetHRVIndicatorResults()
        {
            var userHrvIndicators = GetUserHrvIndicators();

            HRVIndicatorResultsViewModel hrvIndicatorMinResults = new HRVIndicatorResultsViewModel()
            {
                READINESS = userHrvIndicators.Min(y => y.READINESS),
                RMSSD = userHrvIndicators.Min(y => y.RMSSD),
                RR = userHrvIndicators.Min(y => y.RR),
                SDNN = userHrvIndicators.Min(y => y.SDNN),
                SD1 = userHrvIndicators.Min(y => y.SD1),
                TP = userHrvIndicators.Min(y => y.TP),
                HF = userHrvIndicators.Min(y => y.HF),
                LF = userHrvIndicators.Min(y => y.LF),
                SI = userHrvIndicators.Min(y => y.SI),
                LFHF = userHrvIndicators.Min(y => y.LFHF),
            };

            HRVIndicatorResultsViewModel hrvIndicatorAvgResults = new HRVIndicatorResultsViewModel()
            {
                READINESS = userHrvIndicators.Average(y => y.READINESS),
                RMSSD = userHrvIndicators.Average(y => y.RMSSD),
                RR = userHrvIndicators.Average(y => y.RR),
                SDNN = userHrvIndicators.Average(y => y.SDNN),
                SD1 = userHrvIndicators.Average(y => y.SD1),
                TP = userHrvIndicators.Average(y => y.TP),
                HF = userHrvIndicators.Average(y => y.HF),
                LF = userHrvIndicators.Average(y => y.LF),
                SI = userHrvIndicators.Average(y => y.SI),
                LFHF = userHrvIndicators.Average(y => y.LFHF),
            };

            HRVIndicatorResultsViewModel hrvIndicatorMaxResults = new HRVIndicatorResultsViewModel()
            {
                READINESS = userHrvIndicators.Max(y => y.READINESS),
                RMSSD = userHrvIndicators.Max(y => y.RMSSD),
                RR = userHrvIndicators.Max(y => y.RR),
                SDNN = userHrvIndicators.Max(y => y.SDNN),
                SD1 = userHrvIndicators.Max(y => y.SD1),
                TP = userHrvIndicators.Max(y => y.TP),
                HF = userHrvIndicators.Max(y => y.HF),
                LF = userHrvIndicators.Max(y => y.LF),
                SI = userHrvIndicators.Max(y => y.SI),
                LFHF = userHrvIndicators.Max(y => y.LFHF),
            };

            List<HRVIndicatorResultsViewModel> hrvIndicatorsResults = new List<HRVIndicatorResultsViewModel>()
            {
                hrvIndicatorMinResults,
                hrvIndicatorAvgResults,
                hrvIndicatorMaxResults
            };

            return hrvIndicatorsResults;
        }
        
        [NonAction]
        private List<TotalTrainingTypesDurationViewModel> GetTotalTrainingTypesTime(int campId, TimeInterval ? timeInterval = null)
        {
            
            var list = _context.TrainingTypes
                .Join(_context.Trainings, x => x.Id, y => y.IdType, (x, y) => new { x, y })
                .Join(_context.TrainingsSchedules, z => z.y.Id, p => p.IdTraining, (z, p) => new
                { z, p }
              )
                .Where(x => x.p.IdCamp == campId && (timeInterval != null) ? (x.p.Date >= timeInterval.StartIntervalTime && x.p.Date <= timeInterval.EndIntervalTime) : true)
                .GroupBy(x => x.z.x.NameType, x => x.p)
                .Select(x => new TotalTrainingTypesDurationViewModel
                {
                    TypeName = x.Key,
                    TotalDuration = new TimeSpan(0, (int)x.Sum(y => y.Duration), 0),
                }).ToList();

            return list;
        }

        [NonAction]
        private Dictionary<CampPeriod, List<TotalTrainingTypesDurationViewModel>> GetAllTotalTrainingTypesTime(TimeInterval ?timeInterval = null)
        {
            
            var camps = _context.CampsPeriods
                .Where(x => (timeInterval != null) ? (x.Start >= timeInterval.StartIntervalTime && x.End <= timeInterval.EndIntervalTime) : true)
                .ToList();

            var dictionary = new Dictionary<CampPeriod, List<TotalTrainingTypesDurationViewModel>>();
            foreach (var camp in camps)
            {
                dictionary.Add(camp, GetTotalTrainingTypesTime((int)camp.Id, timeInterval));
            }


            return dictionary;
        }

      
        [HttpGet]
        public IActionResult TotalTrainingTypesTime(TimeInterval? timeInterval = null)
        {
            _timeInterval =
                timeInterval != null &&
                (timeInterval.StartIntervalTime == DateTime.MinValue &&
                 timeInterval.EndIntervalTime == DateTime.MinValue)
                    ? null
                    : timeInterval;
            var getAllTotalTrainingTypesTime = GetAllTotalTrainingTypesTime(_timeInterval);

            ViewBag.GetAllTotalTrainingTypesTime = getAllTotalTrainingTypesTime;
            return View();
        }

        [HttpPost]
        public FileResult ExportTotalTrainingTypesTimePdf()
        {
            Document Doc = new Document(PageSize.LETTER);

            using (var fs = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();
                Doc.NewPage();

                string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");

                BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font f = new Font(bf, 12, Font.NORMAL);

                var getAllTotalTrainingTypesTime = GetAllTotalTrainingTypesTime(_timeInterval);

                TimeSpan totalTimeInCampPeriod;
                TimeSpan totalTimeForAllCampPeriods = new TimeSpan();
                
                PdfPTable table1 = new PdfPTable(1);
                Font font2 = new Font(bf, 20, Font.BOLD);
                //Set the biography  
                PdfPCell cell1 = new PdfPCell()
                {
                    BorderWidthBottom = 0f,
                    BorderWidthTop = 0f,
                    BorderWidthLeft = 0f,
                    BorderWidthRight = 0f
                };
                cell1.PaddingTop = 5f;
                Phrase bioPhrase = new Phrase();
                Chunk bioChunk = new Chunk("ArtBiathlon | Данные по общему тренировочному времени во время сборов", font2);
                bioPhrase.Add(bioChunk);
                bioPhrase.Add(new Chunk(Environment.NewLine));

                bioPhrase.Add(new Chunk(Environment.NewLine));

                cell1.AddElement(bioPhrase);

                table1.AddCell(cell1);
                Doc.Add(table1);


                PdfPTable table = new PdfPTable(2);

                PdfPCell cell = new PdfPCell(new Phrase("Общее тренировочное время по сборам", f));
                cell.Colspan = 2;

                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.AddCell(cell);
                table.AddCell(new PdfPCell(new Phrase("Вид тренировки",f)));
                table.AddCell(new PdfPCell(new Phrase("Общее время", f)));
              
                foreach (var trainingTotal in getAllTotalTrainingTypesTime)
                {
                    cell = new PdfPCell(new Phrase(trainingTotal.Key.Start.ToShortDateString() + "-" + @trainingTotal.Key.End.ToShortDateString(), f));
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    totalTimeInCampPeriod = new TimeSpan();

                    foreach (var type in trainingTotal.Value)
                    {
                        table.AddCell(new PdfPCell(new Phrase(type.TypeName.ToString(), f)));
                        table.AddCell(new PdfPCell(new Phrase(type.TotalDuration.ToString(), f)));
                         
                        totalTimeInCampPeriod += type.TotalDuration;
                    }

                    table.AddCell(new PdfPCell(new Phrase("Итоговое время", f)));
                    table.AddCell(new PdfPCell(new Phrase(totalTimeInCampPeriod.ToString(), f)));

                    totalTimeForAllCampPeriods += totalTimeInCampPeriod;
                }

                table.AddCell(new PdfPCell(new Phrase("Итоговое общее время", f)));
                table.AddCell(new PdfPCell(new Phrase(totalTimeForAllCampPeriods.ToString(), f)));

                Doc.Add(table);
                Doc.Close();

                return File(fs.ToArray(), "application/pdf", "ExportData.pdf");
            }
        }
    }
}
