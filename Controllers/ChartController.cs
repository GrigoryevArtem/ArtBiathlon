using System.Text.Json.Serialization;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArtBiathlon.Controllers;

public class ChartController : Controller
{
    private readonly ApplicationDbContext _context;

    public ChartController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public IActionResult TrainingHistogram()
    {
        var trainingsTypes = _context.TrainingTypes.ToList();
        
        ViewBag.TrainingTypes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(trainingsTypes, "Id", "NameType");
        return View();
    }
    
    [HttpPost]
    public IActionResult TrainingHistogram(int id)
    {
        var trainingsTypes = _context.TrainingTypes.ToList();
        
        ViewBag.TrainingTypes = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(trainingsTypes, "Id", "NameType");;
        
        
        var trainingDictionary =
            _context.TrainingsSchedules
                .Join(_context.Trainings.Where(x => x.IdType == id), x => x.IdTraining, x => x.Id, (x, y) =>
                    new
                    {
                        y.NameTraining
                    }).GroupBy(x => x.NameTraining)
                .ToDictionary(x => x.Key, y => y.Count());

        ViewBag.Trainings = trainingDictionary;

        var dataChart = new object[trainingDictionary.Count()];
        int j = 0;

        foreach (var i in trainingDictionary)
        {
            dataChart[j] = new object[]
            {
                i.Key.ToString(),
                i.Value
            };
            j++;
        }

        string dataStr = JsonConvert.SerializeObject(dataChart, Formatting.None);

        ViewBag.dataj = new HtmlString(dataStr);

        return View();
    }
    
    [HttpGet]
    public IActionResult TrainingPie()
    {
        var trainingTypes = _context.TrainingTypes
            .Join(_context.Trainings, x => x.Id, y => y.IdType, (x, y) => new { x, y })
            .Join(_context.TrainingsSchedules, z => z.y.Id, p => p.IdTraining, (z, p) => new
                {
                    z.x.NameType
                })
                .GroupBy(x => x.NameType)
                .ToDictionary(x => x.Key, y => y.Count());

        ViewBag.TrainingTypes = trainingTypes;
        
        var dataChart = new object[trainingTypes.Count()];
        int j = 0;

        foreach (var i in trainingTypes)
        {
            dataChart[j] = new object[]
            {
                i.Key.ToString(),
                i.Value
            };
            j++;
        }

        string dataStr = JsonConvert.SerializeObject(dataChart, Formatting.None);

        ViewBag.dataj = new HtmlString(dataStr);
        
        return View();
    }
    
    [HttpGet]
    public IActionResult ScheduleOfTotalTrainingTimeForTrainingCamps()
    {

        var totalTrainingTimeForTrainingCamps = _context.TrainingsSchedules
            .Join(_context.CampsPeriods, x => x.IdCamp, y => y.Id, (x, y) =>
                new
                {
                    x, y
                })
            .GroupBy(x => x.y)
            .Select(x => new TotalTrainingCampsDurationView
            {
                TimeInterval = new TimeInterval
                {
                    EndIntervalTime = x.Key.End,
                    StartIntervalTime = x.Key.Start
                },
                TotalDuration = new TimeSpan(0, (int)x.Sum(y => y.x.Duration), 0)
            });

        ViewBag.TotalTrainingTimeForTrainingCamps = totalTrainingTimeForTrainingCamps;
        
        var dataChart = new object[totalTrainingTimeForTrainingCamps.Count()];
        int j = 0;

        foreach (var i in totalTrainingTimeForTrainingCamps)
        {
            dataChart[j] = new object[]
            {
                i.TimeInterval.StartIntervalTime.ToShortDateString() + "-" + i.TimeInterval.EndIntervalTime.ToShortDateString(),
                i.TotalDuration.TotalMinutes
            };
            j++;
        }

        string dataStr = JsonConvert.SerializeObject(dataChart, Formatting.None);

        ViewBag.dataj = new HtmlString(dataStr);
        
        return View();
    }
    
}