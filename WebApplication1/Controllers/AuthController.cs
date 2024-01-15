using Azure.Core;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Policy;
using WebApplication1.Models;
using WebApplication1.ViewModel.AuthVM;

public class AuthController : Controller
{
    SignInManager<AppUser> _signInManager { get; }
    UserManager<AppUser> _userManager { get; }
    RoleManager<IdentityRole> _roleManager { get; }

    public AuthController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager
     )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var user = new AppUser
        {
            FirstName = vm.Fisrtname,
            LastName = vm.Lastname,
            Email = vm.Email,
            UserName = vm.Username
        };
        var result = await _userManager.CreateAsync(user, vm.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }

        return View();
    }


    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(string? returnUrl, LoginVm vm)
    {
        AppUser user;
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (vm.UsernameOrEmail.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
        }
        else
        {
            user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
        }
        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
       
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


}

