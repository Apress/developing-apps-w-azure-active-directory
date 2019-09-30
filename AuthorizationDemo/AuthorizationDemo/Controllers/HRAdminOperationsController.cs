using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationDemo.Models;
using AuthorizationDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationDemo.Controllers
{
    [Authorize(Policy = "HRAdminPolicy")]
    public class HRAdminOperationsController : Controller
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IMockEmployeeService mockEmployeeService;

        public HRAdminOperationsController(IAuthorizationService authorizationService, IMockEmployeeService mockEmployeeService)
        {
            this.authorizationService = authorizationService;
            this.mockEmployeeService = mockEmployeeService;
        }

        // GET: HRAdminOperations
        public ActionResult Index()
        {
            return View();
        }

        // GET: HRAdminOperations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            EmployeeEntity employeeEntity = this.mockEmployeeService.CreateMockEmployee(id);
            var result = await this.authorizationService.AuthorizeAsync(this.User, employeeEntity, "HROperationsReadPolicy");
            if (result.Succeeded)
            {
                return View();
            }

            return Forbid();
        }

        // GET: HRAdminOperations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HRAdminOperations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HRAdminOperations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeEntity employeeEntity = this.mockEmployeeService.CreateMockEmployee(id);
            var result = await this.authorizationService.AuthorizeAsync(this.User, employeeEntity, "HROperationsWritePolicy");
            if (result.Succeeded)
            {
                return View();
            }

            return Forbid();
        }

        // POST: HRAdminOperations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HRAdminOperations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HRAdminOperations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}