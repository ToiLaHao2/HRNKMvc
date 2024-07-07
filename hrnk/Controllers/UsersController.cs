using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hrnk.Data;
using hrnk.Models;
using hrnk.Models.AdminModel.Request;
using hrnk.utils;

namespace hrnk.Controllers
{
    public class UsersController : Controller
    {
        private readonly hrnkContext _context;
        private readonly AuthHelpers _authHelpers;

        public UsersController(hrnkContext context, AuthHelpers authHelpers)
        {
            _context = context;
            _authHelpers = authHelpers;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewBag.Roles = _context.Role.ToList();
            ViewBag.Users = _context.User.ToList();
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            ViewBag.Roles = _context.Role.ToList();
            ViewBag.Users = _context.User.ToList();
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Role.ToList();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserRequest createUser)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.User.FirstOrDefaultAsync(u => u.UserAccountName.Equals(createUser.UserAccountName));
                if (user == null)
                {
                    try
                    {
                        user = new User();
                        user.UserAccountName = createUser.UserAccountName.ToString();
                        byte[] HashPassword = await _authHelpers.MakeHashPassword(createUser.Password);
                        user.PasswordHash = HashPassword;
                        user.UserRole = createUser.UserRole;
                        user.CreatedAt = DateTime.Now;
                        user.UpdatedAt = DateTime.Now;
                        _context.User.Add(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        return Json(ex);
                    }
                }
          
            }
            return View(createUser);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Roles = _context.Role.ToList();
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,UserAccountName,UserRole,CreatedAt,UpdatedAt,UpdatedBy")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                User UpdateUser = _context.User.Find(id);
                try
                {
                    UpdateUser.UserAccountName = user.UserAccountName;
                    UpdateUser.UserRole = user.UserRole;
                    UpdateUser.UpdatedAt = DateTime.Now;
                    UpdateUser.UpdatedBy = long.Parse(HttpContext.Session.GetString("UserId"));
                    _context.Update(UpdateUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.User.ToList();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'hrnkContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
          return _context.User.Any(e => e.UserId == id);
        }
    }
}
