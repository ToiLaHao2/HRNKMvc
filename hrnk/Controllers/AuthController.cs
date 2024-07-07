using hrnk.Data;
using hrnk.Models;
using hrnk.Models.AuthModel.Request;
using hrnk.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace hrnk.Controllers
{
    public class AuthController : Controller
    {
        private readonly hrnkContext _context;
        private readonly AuthHelpers _authHelpers;

        public AuthController(hrnkContext context, AuthHelpers authHelpers)
        {
            _authHelpers = authHelpers;
            _context = context;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RequestRegister model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.User.FirstOrDefaultAsync(u => u.UserAccountName.Equals(model.UserAccountName));
                if (user == null)
                {
                    try
                    {
                        user = new User();
                        user.UserAccountName = model.UserAccountName.ToString();
                        byte[] HashPassword = await _authHelpers.MakeHashPassword(model.Password);
                        user.PasswordHash = HashPassword;
                        user.UserRole = "a851a42c-65fe-4c40-96fe-09740de3e899";
                        user.CreatedAt = DateTime.Now;
                        user.UpdatedAt = DateTime.Now;
                        _context.User.Add(user);
                        await _context.SaveChangesAsync();
                        return Json("success");
                    }
                    catch (Exception ex)
                    {
                        return Json(ex);
                    }
                }
                else
                {
                    return Json(model);
                }
            }
            else
            {
                // Dữ liệu không hợp lệ, trả về thông tin lỗi
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(RequestLogin model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.User.FirstOrDefaultAsync(u => u.UserAccountName.Equals(model.UserAccountName));
                if (user != null)
                {
                    try
                    {
                        if (await _authHelpers.ComparePasswords(model.Password, user.PasswordHash))
                        {
                            HttpContext.Session.SetString("UserId", user.UserId.ToString());
                            HttpContext.Session.SetString("UserReady", "ready");
                            HttpContext.Session.SetString("UserData", user.ToJson());
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Register", "Auth");
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Internal server error: {ex.Message}");
                    }
                }
                else
                {
                    return Json("User is not registed");
                }
            }
            else
            {
                return Json("Wrong");
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}
