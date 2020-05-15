using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.Name,
                    Description = model.Description
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new ApplicationRole { Id = role.Id, Name = role.Name, Description = role.Description });
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Description = model.Description;
                    role.Name = model.Name;
                    IdentityResult result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Users(string Id)
        {
            if(string.IsNullOrEmpty(Id))
                return RedirectToAction("Index");
            ViewBag.RoleName = (await RoleManager.FindByIdAsync(Id) as ApplicationRole).Name;
            //получаем пользователей для данной роли
            /*
            var users = from p in UserManager.Users
                        where p.Roles.Any(r => r.RoleId == Id)
                        select p;
            */
            //или LINQ методом:
            IEnumerable<ApplicationUser> users = UserManager.Users.Where(p => p.Roles.Any(r => r.RoleId == Id)).ToArray();
            return View(users);
        }

        [HttpPost]
        public async Task<ActionResult> DelUser(string UserId, string RoleName)
        {
            await UserManager.RemoveFromRoleAsync(UserId, RoleName);
            return RedirectToAction("Users");
        }

        public async Task<ActionResult> AddUsers(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return RedirectToAction("Index");
            var role = await RoleManager.FindByNameAsync(Id) as ApplicationRole;
            ViewBag.Users = UserManager.Users.Where(p => p.Roles.All(r => r.RoleId != role.Id)).ToArray();
            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> AddUsers(ApplicationRole role, string[] InRole)
        {
            foreach(string UserId in InRole)
            {
                if (UserId == "false")
                    continue;
                var rez = await UserManager.AddToRoleAsync(UserId, role.Id);
                if (rez.Succeeded)
                    continue;
                return new HttpNotFoundResult();
            }
            return RedirectToAction("Users");
        }
    }
}