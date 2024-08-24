using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class AdminController : Controller
{
    // GET: Admin/Login
    public ActionResult AdminLogin()
    {
        return View();
    }

    // POST: Admin/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AdminLogin(string username, string password)
    {
        // Implement login logic here
        if (username == "admin" && password == "password") // Example validation
        {
            return RedirectToAction("AdminDashboard");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View();
    }

    // GET: Admin/Dashboard
    public ActionResult AdminDashboard()
    {
        return View();
    }
}
