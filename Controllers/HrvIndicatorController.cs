using ArtBiathlon.Models;
using ArtBiathlon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ArtBiathlon.Controllers;

public class HrvIndicatorController: Controller
{
    private readonly IHrvIndicatorService _hrvIndicatorService;
    
    public HrvIndicatorController(IHrvIndicatorService hrvIndicatorService)
    {
        _hrvIndicatorService = hrvIndicatorService;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var hrvIndicators = _hrvIndicatorService.GetAll().Result;
        
        if (hrvIndicators.StatusCode != Enums.StatusCode.OK)
        {
            ModelState.AddModelError("", hrvIndicators.Description);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.HrvIndicators = hrvIndicators.Data;

        return View();
    }
}