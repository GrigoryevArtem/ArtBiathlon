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
    public List<TotalTrainingCampsDurationView> TotalTrainingCampsDuration()
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
            }).ToList();

        //ViewBag.TotalTrainingTimeForTrainingCamps = totalTrainingTimeForTrainingCamps;
        return totalTrainingTimeForTrainingCamps;
    }

    public List<TotalDayTrainingDurationView> TotalDayTrainingDuration()
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

    public List<TotalDayTrainingCountView> TotalDayTrainingCount()
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
    
    public List<TotalDayMailingCountView> TotalDayMailingCount()
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
    
    public IActionResult StatisticalTotalTrainingTimeByCampsDynamicsIndicators()
    {
        ViewBag.TotalTrainingTimeForTrainingCamps = TotalTrainingCampsDuration();
        ViewBag.TotalDayTrainingDuration = TotalDayTrainingDuration();
        ViewBag.TotalDayTrainingCount = TotalDayTrainingCount();
        ViewBag.TotalDayMailingCount = TotalDayMailingCount();
        
        return View();
    }
}