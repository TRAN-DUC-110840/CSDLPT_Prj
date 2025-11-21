using Microsoft.AspNetCore.Mvc;

namespace CSDLPTProject.Controllers;

public class AuthController : Controller
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Login()
    {
        ViewBag.ApiBaseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:5057/api";
        return View();
    }

    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }
}

