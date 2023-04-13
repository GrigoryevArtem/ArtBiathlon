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
    private static List<MovingAveragesViewModel> GetMovingAverages(IReadOnlyList<double> values)
    {
        var movingAverages = new List<MovingAveragesViewModel>();
        
        for (var i = 0; i < values.Count; ++i)
        {
            double? l3, l5, l7;

            switch (i)
            {
                case 0:
                    l3 = (5 * values[0] + 2 * values[1]  - values[2]) / 6;
                    l5 = (31 * values[0] + 9 * values[1] - 3 * values[2] - 5 * values[3] + 3 * values[4]) / 35;
                    l7 = (39 * values[0] + 8 * values[1] - 4 * values[2] - 4 * values[3] + 1 * values[4] + 4 * values[5] -
                          2 * values[6]) / 42;
                    break;
                case 1:
                    l3 = (values[i - 1] + values[i] + values[i + 1]) / 3;
                    l5 = (9 * values[0] + 13 * values[1] + 12 * values[2] + 6 * values[3] + -5 * values[4]) / 35;
                    l7 = (8 * values[0] + 19 * values[1] + 16 * values[2] + 6 * values[3] - 4 * values[4] - 7 * values[5] +
                          4 * values[6]) / 42;
                    break;
                case 2:
                    l3 = (values[i - 1] + values[i] + values[i + 1]) / 3;
                    l5 = (-3 * values[i - 2] + 12 * values[i - 1] + 17 * values[i] + 12 * values[i + 1] -
                          3 * values[i + 2]) / 35;
                    l7 = (-4 * values[0] + 16 * values[1] + 19 * values[2] + 12 * values[3] + 2 * values[4] -
                        4 * values[5] + values[6]) / 42;
                    break;
                default:
                {
                    if (i == values.Count - 3)
                    {
                        l3 = (values[i - 1] + values[i] + values[i + 1]) / 3;
                        l5 = (-3 * values[i - 2] + 12 * values[i - 1] + 17 * values[i] + 12 * values[i + 1] -
                              3 * values[i + 2]) / 35;

                        l7 = (values[^7] - 4 * values[^6] + 2 * values[^5] + 12 * values[^4] + 19 * values[^3] +
                            16 * values[^2] - 4 * values[^1]) / 42;
                    }
                    else if (i == values.Count - 2)
                    {
                        l3 = (values[i - 1] + values[i] + values[i + 1]) / 3;
                        l5 = (-5 * values[^5] + 6 * values[^4] + 12 * values[^3] + 13 * values[^2] - 9 * values[^1]) /
                             35;

                        l7 = (4 * values[^7] - 7 * values[^6] - 4 * values[^5] + 6 * values[^4] + 16 * values[^3] +
                              19 * values[^2] + 8 * values[^1]) / 42;

                    }
                    else if (i == values.Count - 1)
                    {
                        l3 = (-values[^3] + 2 * values[^2] + 5 * values[^1]) / 6;
                
                        l5 = (3 * values[^5] - 5 * values[^4] - 3 * values[^3] + 9 * values[^2] + 31 * values[^1]) / 35;

                        l7 = (2 * values[^7] + 4 * values[^6] + values[^5] - 4 * values[^4] - 4 * values[^3] +
                              4 * values[^2] + 39 * values[^1]) / 42;
                    }
                    else
                    {
                        l3 = (values[i - 1] + values[i] + values[i + 1]) / 3;
                        l5 = (-3 * values[i - 2] + 12 * values[i - 1] + 17 * values[i] + 12 * values[i + 1] -
                              3 * values[i + 2]) / 35;
                
                        l7 = (values[i - 3] + values[i - 2] + values[i - 1] + values[i] + values[i + 1] + values[i + 2] +
                              values[i + 3]) / 7;
                    }

                    break;
                }
            }

            movingAverages.Add(new MovingAveragesViewModel
            {
                Id = (uint)(i + 1),
                Value = values[i],
                IntervalLengthThree = l3,
                IntervalLengthSeven = l7,
                IntervalLengthFive = l5
            });
        }

        return movingAverages;
    } 

    [NonAction]
    private MovingAveragesViewModel GetPredictiveValueByMovingAverages(List<MovingAveragesViewModel> movingAverages)
    {

        var predictiveValueByMovingAverages = new MovingAveragesViewModel
        {
            Id = (uint)(movingAverages.Count + 1),
            Value = null,
            IntervalLengthThree = movingAverages[^2].IntervalLengthThree +
                                  (movingAverages[^1].Value - movingAverages[^2].Value) / 3,
            IntervalLengthFive = movingAverages[^2].IntervalLengthFive +
                                 (movingAverages[^1].Value - movingAverages[^2].Value) / 5,
            IntervalLengthSeven = movingAverages[^2].IntervalLengthSeven +
                                  (movingAverages[^1].Value - movingAverages[^2].Value) / 7
        };
        
        return predictiveValueByMovingAverages;
    }
    
    public IActionResult StatisticalTotalTrainingTimeByCampsDynamicsIndicators()
    {
        ViewBag.TotalTrainingTimeForTrainingCamps = TotalTrainingCampsDuration();
        ViewBag.TotalDayTrainingDuration = TotalDayTrainingDuration();
        ViewBag.TotalDayTrainingCount = TotalDayTrainingCount();
        ViewBag.TotalDayMailingCount = TotalDayMailingCount();

        var totalTrainingCampsDurationGroupByYearAndMonth = TotalTrainingCampsDurationGroupByYearAndMonth();
        var movingAveragesForTotalTrainingCampsDuration = GetMovingAverages(totalTrainingCampsDurationGroupByYearAndMonth);
        ViewBag.MovingAveragesForTotalTrainingCampsDuration = movingAveragesForTotalTrainingCampsDuration;
        ViewBag.PredictiveValueForTotalTrainingDuration = GetPredictiveValueByMovingAverages(movingAveragesForTotalTrainingCampsDuration);
        
        var totalTrainingCampsCountGroupByYearAndMonth = TotalTrainingCampsCountGroupByYearAndMonth();
        var movingAveragesForTotalTrainingCampsCount = GetMovingAverages(totalTrainingCampsCountGroupByYearAndMonth);
        ViewBag.MovingAveragesForTotalTrainingCampsCount = movingAveragesForTotalTrainingCampsCount;
        ViewBag.PredictiveValueForTotalTrainingCount = GetPredictiveValueByMovingAverages(movingAveragesForTotalTrainingCampsCount);
        
        var totalDayMailingCountGroupByYearAndMonth = TotalDayMailingCountGroupByYearAndMonth();
        var movingAveragesForTotalTotalDayMailingCount = GetMovingAverages(totalDayMailingCountGroupByYearAndMonth);
        ViewBag.MovingAveragesForTotalDayMailingCount = movingAveragesForTotalTotalDayMailingCount;
        ViewBag.PredictiveValueForTotalMailingCount = GetPredictiveValueByMovingAverages(movingAveragesForTotalTotalDayMailingCount);
        
        return View();
    }
}