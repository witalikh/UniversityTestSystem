using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Services.Implementations;
using BusinessLayer.ViewModels;
using DataAccessLayer.Enums;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests
{
    public class ClassroomManagement
    {
        // Repository mock
        private readonly Mock<IClassroomRepository> _classroomRepositoryMock = new();

        // Logger mock
        private readonly Mock<ILogger<ClassroomManagementService>> _loggerMock = new();

        // Service
        private readonly ClassroomManagementService _classroomManagementService;

        // Mock collections
        private readonly List<ClassroomEntity> _classroomEntities = new();

        private void configClassroomRepositoryMock()
        {
            this._classroomRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(true);

            this._classroomRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<ClassroomEntity>()))
                .ReturnsAsync((ClassroomEntity entity) =>
                {
                    this._classroomEntities.Add(entity);
                    return true;
                });

            this._classroomRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<ClassroomEntity>()))
                .ReturnsAsync((ClassroomEntity entity) =>
                {
                    this._classroomEntities.Remove(entity);
                    return true;
                });

            this._classroomRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return this._classroomEntities.Find(c => c.Id == id);
                });

            this._classroomRepositoryMock
                .Setup(x => x.GetClassroomByIdAndUserAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((int id, string userPk) =>
                {
                    return this._classroomEntities.Find(c => c.Id == id && c.Members.Any(m => m.UserEntityId == userPk));
                });

            this._classroomRepositoryMock
                .Setup(x => x.GetClassroomByIdAndUserAndRoleAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<MembershipRole>()))
                .ReturnsAsync((int id, string userPk, MembershipRole role) =>
                {
                    return this._classroomEntities.Find(c => c.Id == id && c.Members.Any(m => m.UserEntityId == userPk));
                });

            this._classroomRepositoryMock
                .Setup(x => x.GetAllClassroomsByUserAsync(It.IsAny<string>()))
                .ReturnsAsync((string userPk) =>
                {
                    return this._classroomEntities.FindAll(c => c.Members.Any(m => m.UserEntityId == userPk));
                });
        }

        public ClassroomManagement()
        {
            this.configClassroomRepositoryMock();

            this._classroomManagementService = new ClassroomManagementService(
                this._classroomRepositoryMock.Object,
                this._loggerMock.Object);
        }

        [Fact]
        public async Task AddClassroomAsync_ReturnsFalse_WhenClassroomDataIsNull()
        {
            // Arrange
            ClassroomViewModel classroomData = null;

            // Act
            bool result = await this._classroomManagementService.AddClassroomAsync(classroomData);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddClassroomAsync_ReturnsFalse_WhenClassroomIsNotAdded()
        {
            // Arrange
            ClassroomViewModel classroomData = new ClassroomViewModel();

            // Act
            bool result = await this._classroomManagementService.AddClassroomAsync(classroomData);

            // Assert
            Assert.False(result);
            this._classroomRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ClassroomEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddClassroomAsync_ReturnsTrue_WhenClassroomIsAdded()
        {
            // Arrange
            ClassroomViewModel classroomData = new ClassroomViewModel()
            {
                Name = "Test",
                Description = "Test description"
            };

            // Act
            bool result = await this._classroomManagementService.AddClassroomAsync(classroomData);

            // Assert
            Assert.True(result);
            this._classroomRepositoryMock.Verify(x => x.AddAsync(It.IsAny<ClassroomEntity>()), Times.Once);
        }

        [Fact]
        public async Task EditClassroomAsync_ReturnsFalse_WhenOldIsNull()
        {
            // Arrange
            ClassroomEntity old = null;
            ClassroomViewModel data = new ClassroomViewModel();

            // Act
            bool result = await this._classroomManagementService.EditClassroomAsync(old, data);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditClassroomAsync_ReturnsFalse_WhenDataIsNull()
        {
            // Arrange
            ClassroomEntity old = new ClassroomEntity();
            ClassroomViewModel data = null;

            // Act
            bool result = await this._classroomManagementService.EditClassroomAsync(old, data);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditClassroomAsync_ReturnsFalse_WhenClassroomIsNotEdited()
        {
            // Arrange
            ClassroomEntity old = new ClassroomEntity();
            ClassroomViewModel data = new ClassroomViewModel();

            this._classroomRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(false);

            // Act
            bool result = await this._classroomManagementService.EditClassroomAsync(old, data);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EditClassroomAsync_ReturnsTrue_WhenClassroomIsEdited()
        {
            // Arrange
            ClassroomEntity old = new ClassroomEntity()
            {
                Name = "Old",
                Description = "Old description"
            };
            ClassroomViewModel data = new ClassroomViewModel()
            {
                Name = "New",
                Description = "New description"
            };

            // Act
            bool result = await this._classroomManagementService.EditClassroomAsync(old, data);

            // Assert
            Assert.True(result);
            Assert.Equal("New", old.Name);
            Assert.Equal("New description", old.Description);
        }

        [Fact]
        public async Task DeleteClassroomAsync_ReturnsFalse_WhenClassroomEntityIsNull()
        {
            // Arrange
            ClassroomEntity classroomEntity = null;

            // Act
            bool result = await this._classroomManagementService.DeleteClassroomAsync(classroomEntity);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteClassroomAsync_ReturnsFalse_WhenClassroomIsNotDeleted()
        {
            // Arrange
            ClassroomEntity classroomEntity = new ClassroomEntity();

            this._classroomRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<ClassroomEntity>()))
                .ReturnsAsync(false);

            // Act
            bool result = await this._classroomManagementService.DeleteClassroomAsync(classroomEntity);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteClassroomAsync_ReturnsTrue_WhenClassroomIsDeleted()
        {
            // Arrange
            ClassroomEntity classroomEntity = new ClassroomEntity();

            this._classroomRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<ClassroomEntity>()))
                .ReturnsAsync(true);

            // Act
            bool result = await this._classroomManagementService.DeleteClassroomAsync(classroomEntity);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetClassroomByIdAsync_ReturnsNull_WhenIdIsInvalid()
        {
            // Arrange
            int id = -1;

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetClassroomByIdAsync_ReturnsNull_WhenIdIsNotFound()
        {
            // Arrange
            int id = 1;

            this._classroomRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return null;
                });

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetClassroomByIdAsync_ReturnsClassroom_WhenIdIsValid()
        {
            // Arrange
            int id = 1;

            var classroomEntity = new ClassroomEntity()
            {
                Id = id,
                Name = "Test",
                Description = "Test description"
            };

            this._classroomEntities.Add(classroomEntity);

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("Test description", result.Description);
        }

        [Fact]
        public async Task GetClassroomByIdAndUserAsync_ReturnsNull_WhenIdIsInvalid()
        {
            // Arrange
            int id = -1;
            string userPk = "test";

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAndUserAsync(id, userPk);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetClassroomByIdAndUserAsync_ReturnsNull_WhenIdAndUserPkAreNotFound()
        { // Arrange
            int id = 1; string userPk = "test";
            this._classroomRepositoryMock
                .Setup(x => x.GetClassroomByIdAndUserAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((int id, string userPk) =>
                {
                    return null;
                });

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAndUserAsync(id, userPk);

            // Assert
            Assert.Null(result);

        }
        [Fact]
        public async Task GetClassroomByIdAndUserAsync_ReturnsClassroom_WhenIdAndUserPkAreValid()
        {
            // Arrange
            int id = 1; string userPk = "test";
            var classroomEntity = new ClassroomEntity()
            {
                Id = id,
                Name = "Test",
                Description = "Test description",
            };

            this._classroomEntities.Add(classroomEntity);

            // Act
            var result = await this._classroomManagementService.GetClassroomByIdAndUserAsync(id, userPk);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test", result.Name);
            Assert.Equal("Test description", result.Description);
        }
    }
}
