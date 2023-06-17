using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Models;
using BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Controllers
{
    public class AttemptsController : BaseController
    {
        private readonly ITestAttemptManagementService _testAttemptManagementService;
        private readonly IClassroomManagementService _classroomManagementService;
        private readonly ITestManagementService _testManagementService;
        private readonly ILogger<ClassroomInvitationsController> _logger;


        public AttemptsController(
            UserManager<UserEntity> manager,
            IClassroomManagementService classroomManagementService,
            ITestManagementService testManagementService,
            ITestAttemptManagementService testAttemptManagementService,
            ILogger<ClassroomInvitationsController> logger)
            : base(manager)
        {
            this._classroomManagementService = classroomManagementService;
            this._testManagementService = testManagementService;
            this._testAttemptManagementService = testAttemptManagementService;
            this._logger = logger;
        }

        // GET: Attempts
        [Route("{classroomPk}/Tests/{testPk}/Attempts", Name = "attempts-index-by-test")]
        public async Task<IActionResult> Index(int classroomPk, int testPk)
        {

            var attempts = await this._testAttemptManagementService.GetAttemptsByTest(testPk);
            return this.View(await attempts.ToListAsync());
        }

        // GET: Attempts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var attemptEntity = await this._context.Attempts
                .Include(a => a.TestEntity)
                .FirstOrDefaultAsync(m => m.uuid == id);
            if (attemptEntity == null)
            {
                return this.NotFound();
            }

            return this.View(attemptEntity);
        }

        // GET: Attempts/Create
        public IActionResult Create()
        {
            this.ViewData["TestId"] = new SelectList(this._context.Tests, "Id", "Id");
            return this.View();
        }

        // POST: Attempts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("uuid,MemberId,TestId,AttemptNumber,Grade,DateTimeStarted,DateTimeEnded")] AttemptEntity attemptEntity)
        {
            if (this.ModelState.IsValid)
            {
                attemptEntity.uuid = Guid.NewGuid();
                this._context.Add(attemptEntity);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["TestId"] = new SelectList(this._context.Tests, "Id", "Id", attemptEntity.TestId);
            return this.View(attemptEntity);
        }

        // GET: Attempts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var attemptEntity = await this._context.Attempts.FindAsync(id);
            if (attemptEntity == null)
            {
                return this.NotFound();
            }

            this.ViewData["TestId"] = new SelectList(this._context.Tests, "Id", "Id", attemptEntity.TestId);
            return this.View(attemptEntity);
        }

        // POST: Attempts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("uuid,MemberId,TestId,AttemptNumber,Grade,DateTimeStarted,DateTimeEnded")] AttemptEntity attemptEntity)
        {
            if (id != attemptEntity.uuid)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(attemptEntity);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.AttemptEntityExists(attemptEntity.uuid))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["TestId"] = new SelectList(this._context.Tests, "Id", "Id", attemptEntity.TestId);
            return this.View(attemptEntity);
        }

        // GET: Attempts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var attemptEntity = await this._context.Attempts
                .Include(a => a.TestEntity)
                .FirstOrDefaultAsync(m => m.uuid == id);
            if (attemptEntity == null)
            {
                return this.NotFound();
            }

            return this.View(attemptEntity);
        }

        // POST: Attempts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attemptEntity = await this._context.Attempts.FindAsync(id);
            if (attemptEntity != null)
            {
                this._context.Attempts.Remove(attemptEntity);
            }
            
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        private bool AttemptEntityExists(Guid id)
        {
          return (this._context.Attempts?.Any(e => e.uuid == id)).GetValueOrDefault();
        }
    }
}
