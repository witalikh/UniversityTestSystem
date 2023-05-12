namespace PresentationLayer.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Services.Interfaces;
    using BusinessLayer.ViewModels;
    using DataAccessLayer;
    using DataAccessLayer.Enums;
    using DataAccessLayer.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QuestionsController : BaseController
    {
        private readonly IClassroomManagementService _classroomManagementService;
        private readonly ITestManagementService _testManagementService;
        private readonly ILogger<QuestionsController> _logger;

        public QuestionsController(
            UserManager<UserEntity> manager,
            IClassroomManagementService classroomManagementService,
            ITestManagementService testManagementService,
            ILogger<QuestionsController> logger)
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

        // GET: QuestionEntities
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions", Name = "tests-questions-index-by-classroom")]
        public async Task<IActionResult> Index(int classroomPk, int testPk)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomMember(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            var applicationDbContext = await this._testManagementService.GetQuestionsByTestAsync(testPk);
            return this.View(applicationDbContext.Select(t => new QuestionViewModel(t)).ToList());
        }

        // GET: QuestionEntities/Details/5
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/{id}", Name = "tests-questions-details-by-classroom")]
        public async Task<IActionResult> Details(int classroomPk, int testPk, int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomMember(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            var questionEntity = await this._testManagementService.GetQuestionByTestAndIdAsync(testPk, id);
            return this.View(new QuestionViewModel(questionEntity));
        }

        // GET: QuestionEntities/Create
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Create", Name = "tests-questions-create-by-classroom-get")]
        public async Task<IActionResult> Create(int classroomPk, int testPk)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            var emptyQuestion = new QuestionViewModel()
            {
                TestId = testPk,
            };
            return this.View(emptyQuestion);
        }

        // POST: QuestionEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Create", Name = "tests-questions-create-by-classroom-post")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int classroomPk,
            int testPk,
            [Bind("Id,TestId,InnerOrder,Type,Marks,QuestionText,CorrectNumber,CorrectShortText")]
            QuestionViewModel questionData)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            bool created = false;
            if (this.ModelState.IsValid)
            {
                created = await this._testManagementService.AddQuestionAsync(testPk, questionData);
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            if (created) {
                return this.RedirectToAction(nameof(this.Index), new
                {
                    classroomPk,
                    testPk
                });
            }

            return this.View(questionData);
        }

        // GET: QuestionEntities/Edit/5
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Edit/{id}", Name = "tests-questions-edit-by-classroom-get")]
        public async Task<IActionResult> Edit(int classroomPk, int testPk, int? id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            var questionEntity = await this._testManagementService.GetQuestionByTestAndIdAsync(testPk, id);
            if (questionEntity == null)
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            return this.View(new QuestionViewModel(questionEntity));
        }

        // POST: QuestionEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Edit/{id}", Name = "tests-questions-edit-by-classroom-post")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int classroomPk,
            int testPk,
            int id, 
            [Bind("Id,TestId,InnerOrder,Type,Marks,QuestionText,CorrectNumber,CorrectShortText")] 
            QuestionViewModel questionData)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            var questionEntity = await this._testManagementService.GetQuestionByTestAndIdAsync(testPk, id);
            if (questionEntity == null)
            {
                return this.NotFound();
            }

            bool edited = false;
            if (this.ModelState.IsValid)
            {
                edited = await this._testManagementService.EditQuestionAsync(testPk, questionEntity, questionData);
            }

            if (edited)
            {
                return this.RedirectToAction(nameof(this.Index), new { classroomPk, testPk });
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            return this.View(questionData);
        }

        // GET: QuestionEntities/Delete/5
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Delete/{id}", Name = "tests-questions-delete-by-classroom-get")]
        public async Task<IActionResult> Delete(
            int classroomPk,
            int testPk,
            int? id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            var questionEntity = await this._testManagementService.GetQuestionByTestAndIdAsync(testPk, id);
            if (questionEntity == null)
            {
                return this.NotFound();
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            return this.View(new QuestionViewModel(questionEntity));
        }

        // POST: QuestionEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [Route("{classroomPk}/Tests/{testPk}/Questions/Delete/{id}", Name = "tests-questions-delete-by-classroom-post")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int classroomPk,
            int testPk,
            int id)
        {
            string userPk = this.GetUserId() !;
            if (!await this.IsClassroomOwner(userPk, classroomPk))
            {
                return this.Forbid();
            }

            if (!await this._classroomManagementService.IsTestInClassroomAsync(testPk, classroomPk))
            {
                return this.NotFound();
            }

            var questionEntity = await this._testManagementService.GetQuestionByTestAndIdAsync(testPk, id);
            if (questionEntity != null)
            {
                await this._testManagementService.DeleteQuestionAsync(testPk, questionEntity);
            }

            this.ViewBag.ClassroomId = classroomPk;
            this.ViewBag.TestId = testPk;
            return this.RedirectToAction(nameof(this.Index), new { classroomPk, testPk });
        }
    }
}
