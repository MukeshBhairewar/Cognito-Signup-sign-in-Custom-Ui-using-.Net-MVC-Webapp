using System.Threading.Tasks;
using CustomuiCognito.Models;
using CustomuiCognito;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly ICognitoService _cognitoService;

    public AccountController(ICognitoService cognitoService)
    {
        _cognitoService = cognitoService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Implement login logic using _cognitoService
            var signInResponse = await _cognitoService.SignInAsync(model.Username, model.Password);

            if (signInResponse != null)
            {
                // Successful login
                // Redirect to the home page or another protected area
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Login failed
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
        }

        // If we got this far, something failed, redisplay the form
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _cognitoService.SignUpAsync(model.Username, model.Password, model.Activationcode);

            // Redirect to the login page upon successful registration
            return RedirectToAction("Login");
        }

        // If we got this far, something failed, redisplay the form
        return View(model);
    }
}


