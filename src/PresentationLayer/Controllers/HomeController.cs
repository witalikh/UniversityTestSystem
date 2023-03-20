// <copyright file="HomeController.cs" company="SegmentationFault">
// Copyright (c) SegmentationFault. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this._logger = logger;
    }

    public IActionResult Index()
    {
        this._logger.LogInformation("Index from HomeController called.");
        return this.RedirectToRoute("classrooms-index");
    }

    public IActionResult Privacy()
    {
        return this.View();
    }
}
