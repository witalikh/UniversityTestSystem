namespace PresentationLayer.Controllers
{
    using BusinessLayer.Services.Interfaces;
    using BusinessLayer.ViewModels;
    using DataAccessLayer;
    using DataAccessLayer.Enums;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class TestsController : BaseController
    {
        private readonly IClassroomManagementService _classroomManagementService;
        private readonly ITestManagementService _testManagementService;
        private readonly ILogger<TestsController> _logger;

        public TestsController(
            UserManager<UserEntity> manager,
            IClassroomManagementService classroomManagementService,
            ITestManagementService testManagementService,
            ILogger<TestsController> logger)
            : base(manager)
        {
            this._classroomManagementService = classroomManagementService;
            this._testManagementService = testManagementService;
            this._logger = logger;
        }

        private async Task<bool> IsClassroomOwner(string userPk, int classroomPk)
        {
            return await this._classroomManagementService.GetMemberRoleAsync(classroomPk, userPk) ==
                   MembershipRole.Creator;
        }

        private async Task<bool> IsClassroomMember(string userPk, int classroomPk)
        {
            return await this._classroomManagementService.GetMemberRoleAsync(classroomPk, userPk) != null;
        }

        // GET: Tests
        [Route("{classroomId}/Tests", Name="tests-index-by-classroom")]
        [Authorize]
        public async Task<IActionResult> Index(int classroomId)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomMember(userPk, classroomId))
            {
                return this.Forbid();
            }

            this.ViewBag.ClassroomId = classroomId;
            var applicationDbContext = await this._testManagementService.GetTestListByClassroomAsync(classroomId);
            return this.View(applicationDbContext.Select(t => new TestViewModel(t)).ToList());
        }

        // GET: Tests/Details/5
        [Route("{classroomId}/Tests/Details/{id}", Name = "tests-details-by-classroom")]
        [Authorize]
        public async Task<IActionResult> Details(int classroomId, int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomMember(userPk, classroomId))
            {
                return this.Forbid();
            }

            var testEntity = await this._testManagementService.GetTestByClassroomAndIdAsync(classroomId, id);
            if (testEntity == null)
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomId;
            var tvm = new TestViewModel(testEntity);

            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                tvm.Questions = new List<QuestionViewModel>();
            }

            return this.View(tvm);
        }

        // GET: Tests/Create
        [Route("{classroomId}/Tests/Create", Name = "tests-create-by-classroom-get")]
        [Authorize]
        public async Task<IActionResult> Create(int classroomId)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            this.ViewBag.ClassroomId = classroomId;
            var emptyTest = new TestViewModel
            {
                ClassroomId = classroomId
            };
            return this.View(emptyTest);
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{classroomId}/Tests/Create", Name = "tests-create-by-classroom-post")]
        [Authorize]
        public async Task<IActionResult> Create(
            int classroomId,
            [Bind("Id,ClassroomEntityId,CreatedAt,StartDateTime,EndDateTime,DurationSeconds,AttemptsAllowed,TotalGrade,Questions")]
            TestViewModel testData)
        {

            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            bool created = false;
            if (this.ModelState.IsValid)
            {
                created = await this._testManagementService.AddTestAsync(classroomId, testData);
            }

            if (created)
            {
                return this.RedirectToAction(nameof(this.Index), new {classroomId = classroomId});
            }

            this.ViewBag.ClassroomId = classroomId;
            return this.View(testData);
        }

        // GET: Tests/Edit/5
        [Route("{classroomId}/Tests/Edit/{id}", Name = "tests-edit-by-classroom-get")]
        [Authorize]
        public async Task<IActionResult> Edit(
            int classroomId,
            int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            var testEntity = await this._testManagementService.GetTestByClassroomAndIdAsync(classroomId, id);
            if (testEntity == null)
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomId;
            return this.View(new TestViewModel(testEntity));
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("{classroomId}/Tests/Edit/{id}", Name = "tests-edit-by-classroom-post")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(
            int classroomId,
            int id,
            [Bind("Id,ClassroomEntityId,CreatedAt,StartDateTime,EndDateTime,DurationSeconds,AttemptsAllowed,TotalGrade,Questions")]
            TestViewModel testData)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            if (id != testData.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var testEntity = await this._testManagementService.GetTestByClassroomAndIdAsync(classroomId, id);
                if (testEntity == null)
                {
                    return this.NotFound();
                }

                bool edited = await this._testManagementService.EditTestAsync(classroomId, testEntity, testData);
                if (!edited)
                {
                    this._logger.LogError($"Couldn't edit Test in /{classroomId}/Tests/Edit/{id}");
                }

                return this.RedirectToAction(nameof(this.Index), new { classroomId });
            }

            this.ViewBag.ClassroomId = classroomId;
            return this.View(testData);
        }

        [Route("{classroomId}/Tests/Delete/{id}", Name = "tests-delete-by-classroom-get")]
        [Authorize]
        public async Task<IActionResult> Delete(int classroomId, int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            var testEntity = await this._testManagementService.GetTestByClassroomAndIdAsync(classroomId, id);
            if (testEntity == null)
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomId;
            return this.View(new TestViewModel(testEntity));
        }

        [HttpPost, ActionName("Delete")]
        [Route("{classroomId}/Tests/Delete/{id}", Name = "tests-delete-by-classroom-post")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int classroomId, int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomId))
            {
                return this.Forbid();
            }

            var testEntity = await this._testManagementService.GetTestByClassroomAndIdAsync(classroomId, id);
            if (testEntity != null)
            {
                await this._testManagementService.DeleteTestAsync(classroomId, testEntity);
            }

            this.ViewBag.ClassroomId = classroomId;
            return this.RedirectToAction(nameof(this.Index), new { classroomId });
        }

        [HttpGet]
        public IActionResult GetQuestionEditor()
        {
            var viewModel = new QuestionViewModel(); // create an empty question view model
            return this.PartialView("_QuestionEditor", viewModel); // return the partial view with the empty view model
        }

        [HttpGet]
        public IActionResult QuestionChoiceEditor()
        {
            var viewModel = new QuestionChoiceOptionViewModel(); // create an empty question view model
            return this.PartialView("_QuestionChoiceEditor", viewModel); // return the partial view with the empty view model
        }
    }
}
