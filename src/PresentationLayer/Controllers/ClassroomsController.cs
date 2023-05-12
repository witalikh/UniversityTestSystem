namespace PresentationLayer.Controllers
{
    using BusinessLayer.Services.Interfaces;
    using BusinessLayer.ViewModels;
    using DataAccessLayer;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ClassroomsController : BaseController
    {
        private readonly IClassroomManagementService _service;
        private readonly ILogger<ClassroomsController> _logger;

        public ClassroomsController(
            UserManager<UserEntity> manager,
            IClassroomManagementService service,
            ILogger<ClassroomsController> logger)
            : base(manager)
        {
            this._service = service;
            this._logger = logger;
        }

        // GET: Classrooms
        [Route("Classrooms", Name = "classrooms-index")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = this.GetUserId() !;
            var classrooms = await this._service.GetClassroomListByUserAsync(userId);
            var classroomsViewModels = classrooms.Select(c => new ClassroomViewModel(c)).ToList();

            this.ViewBag.CurrentUserId = userId;
            return this.View(classroomsViewModels);
        }

        // GET: Classrooms/Details/5
        [Route("Classrooms/Details/{id}", Name = "classrooms-details")]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            string userId = this.GetUserId() !;

            if (id == null)
            {
                return this.NotFound();
            }

            var classroom = await this._service.GetClassroomByIdAndUserAsync((int)id, userId);
            if (classroom == null)
            {
                return this.NotFound();
            }

            this.ViewBag.CurrentUserId = userId;
            return this.View(new ClassroomViewModel(classroom));
        }

        // GET: Classrooms/Create
        [Route("Classrooms/Create", Name = "classrooms-create-get")]
        [Authorize]
        public IActionResult Create()
        {
            string userId = this.GetUserId() !;
            var empty = new ClassroomViewModel();

            this.ViewBag.CurrentUserId = userId;
            return this.View(empty);
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("Classrooms/Create", Name = "classrooms-create-post")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ClassroomViewModel classroom)
        {
            string userId = this.GetUserId() !;

            classroom.OwnerId = userId;

            this.ModelState.Remove("Members");

            this.ViewBag.CurrentUserId = userId;

            if (!this.ModelState.IsValid)
            {
                this._logger.LogInformation("Failed to create classroom");
                return this.View(classroom);
            }

            bool created = await this._service.AddClassroomAsync(classroom);
            if (created)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            this.ViewData["Error"] = "Something happened: couldn't create the classroomEntity";
            return this.View(classroom);
        }

        [Route("Classrooms/Edit/{id}", Name = "classrooms-edit-get")]
        // GET: Classrooms/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            string userId = this.GetUserId() !;

            var ownClassroom = await this._service.GetOwnedClassroomByIdAndUserAsync(id, userId);

            this.ViewBag.CurrentUserId = userId;
            if (ownClassroom == null)
            {
                return this.NotFound();
            }

            return this.View(new ClassroomViewModel(ownClassroom));
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Classrooms/Edit/{id}", Name = "classrooms-edit-post")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,OwnerId")] ClassroomViewModel classroomEntity)
        {
            string userId = this.GetUserId() !;
            var ownClassroom = await this._service.GetOwnedClassroomByIdAndUserAsync(id, userId);

            this.ViewBag.CurrentUserId = userId;
            if (ownClassroom == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(classroomEntity);
            }

            bool edited = await this._service.EditClassroomAsync(ownClassroom, classroomEntity);
            if (edited)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(classroomEntity);
        }

        // GET: Classrooms/Delete/5
        [Route("Classrooms/Delete/{id}", Name = "classrooms-delete-get")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            string userId = this.GetUserId() !;

            var ownClassroom = await this._service.GetOwnedClassroomByIdAndUserAsync(id, userId);

            this.ViewBag.CurrentUserId = userId;
            if (ownClassroom == null)
            {
                return this.NotFound();
            }

            return this.View(new ClassroomViewModel(ownClassroom));
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Classrooms/Delete/{id}", Name = "classrooms-delete-post")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userId = this.GetUserId() !;

            var ownClassroom = await this._service.GetOwnedClassroomByIdAndUserAsync(id, userId);

            this.ViewBag.CurrentUserId = userId;
            if (ownClassroom == null)
            {
                return this.NotFound();
            }

            await this._service.DeleteClassroomAsync(ownClassroom);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
