using System.Runtime.InteropServices.JavaScript;
using ArtBiathlon.DataEntity;
using ArtBiathlon.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ArtBiathlon.Controllers;

public class StatisticalForecastingController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatisticalForecastingController(ApplicationDbContext context)
    {
        _context = context;
    }
    [NonAction]
    private List<TotalTrainingCampsDurationView> TotalTrainingCampsDuration()
    {
    var totalTrainingDurationForTrainingCamps = _context.TrainingsSchedules
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
            }).ToList();

        //ViewBag.TotalTrainingTimeForTrainingCamps = totalTrainingTimeForTrainingCamps;
        return totalTrainingDurationForTrainingCamps;
    }
    [NonAction]
    private List<TotalDayTrainingDurationView> TotalDayTrainingDuration()
    {

        var totalDayTrainingDuration = _context.TrainingsSchedules
            .GroupBy(x => x.Date)
            .Select(x => new TotalDayTrainingDurationView
            {
                Date = x.Key,
                TotalDuration = new TimeSpan(0, (int)x.Sum(y => y.Duration), 0 )
            }).ToList();

        return totalDayTrainingDuration;
    }
    [NonAction]
    private List<TotalDayTrainingCountView> TotalDayTrainingCount()
    {
       
        var totalDayTrainingCount = _context.TrainingsSchedules
            .GroupBy(x => x.Date)
            .Select(x => new TotalDayTrainingCountView
            {
                Date = x.Key,
                TrainingCount = (uint)x.Count() 
            }).ToList();
        
        return totalDayTrainingCount;
    }
    [NonAction]
    private List<TotalDayMailingCountView> TotalDayMailingCount()
    {
       
        var totalDayMailingCount = _context.Mailings
            .GroupBy(x => x.Date)
            .Select(x => new TotalDayMailingCountView
            {
                Date = x.Key,
                MailingCount = (uint)x.Count() 
            }).ToList();
        
        return totalDayMailingCount;
    }
    
    [NonAction]
    private List<double> TotalTrainingCampsDurationGroupByYearAndMonth()
    {
        
        var totalTrainingDurationForTrainingCamps = _context.TrainingsSchedules
            .GroupBy(x => new { x.Date.Month, x.Date.Year })
            .Select(x => new TimeSpan(0, (int)x.Sum(y => y.Duration), 0).TotalMinutes)
            .ToList(); ;
        return totalTrainingDurationForTrainingCamps;
    }
    
    [NonAction]
    private List<double> TotalTrainingCampsCountGroupByYearAndMonth()
    {
        var totalTrainingCountForTrainingCamps = _context.TrainingsSchedules
            .GroupBy(x => new { x.Date.Month, x.Date.Year })
            .Select(x => (double)x.Count())
            .ToList();
       
        return totalTrainingCountForTrainingCamps;
    }
    
    [NonAction]
    private List<double> TotalDayMailingCountGroupByYearAndMonth()
    {
       
        var totalDayMailingCount = _context.Mailings
            .GroupBy(x => new { x.Date.Month, x.Date.Year })
            .Select(x => (double)x.Count())
            .ToList();
        
        return totalDayMailingCount;
    }

    [NonAction]
    private List<MovingAveragesViewModel> MovingAverages(List<double> values)
    {
        var movingAverages = new List<MovingAveragesViewModel>();
        
        for (var i = 0; i < values.Count; ++i)
        {
            movingAverages.Add(new MovingAveragesViewModel
                {
                    Id = (uint)(i + 1),
                    Value = values[i],
                    IntervalLengthThree = (i == 0 || i == values.Count - 1) ? null : Math.Round((values[i - 1] + values[i] + values[i + 1]) / 3,2),
                    IntervalLengthSeven = (i > 2 && i < values.Count - 3) ? Math.Round((values[i - 3] + values[i - 2] + values[i - 1] + values[i] + values[i + 1] + values[i + 2] + values[i + 3]) / 7, 2) : null,
                    IntervalLengthFive = (i > 1 && i < values.Count - 2) ? Math.Round((-3 * values[i - 2] + 12 * values[i - 1] + 17 * values[i] + 12 * values[i + 1] - 3 *values[i + 2]) / 35, 2) : null
                }
                );
        }

        return movingAverages;
    }

    public IActionResult StatisticalTotalTrainingTimeByCampsDynamicsIndicators()
    {
        ViewBag.TotalTrainingTimeForTrainingCamps = TotalTrainingCampsDuration();
        ViewBag.TotalDayTrainingDuration = TotalDayTrainingDuration();
        ViewBag.TotalDayTrainingCount = TotalDayTrainingCount();
        ViewBag.TotalDayMailingCount = TotalDayMailingCount();

        var totalTrainingCampsDurationGroupByYearAndMonth = TotalTrainingCampsDurationGroupByYearAndMonth();
        ViewBag.MovingAveragesForTotalTrainingCampsDuration = MovingAverages(totalTrainingCampsDurationGroupByYearAndMonth);
        
        var totalTrainingCampsCountGroupByYearAndMonth = TotalTrainingCampsCountGroupByYearAndMonth();
        ViewBag.MovingAveragesForTotalTrainingCampsCount = MovingAverages(totalTrainingCampsCountGroupByYearAndMonth);
        
        var totalDayMailingCountGroupByYearAndMonth = TotalDayMailingCountGroupByYearAndMonth();
        ViewBag.MovingAveragesForTotalDayMailingCount = MovingAverages(totalDayMailingCountGroupByYearAndMonth);
        
        return View();
    }
}