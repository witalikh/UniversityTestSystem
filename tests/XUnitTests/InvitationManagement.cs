using Xunit;
using Moq;
using System.Threading.Tasks;
using BusinessLayer.Services.Implementations;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Enums;
using Microsoft.Extensions.Logging;

namespace XUnitTests
{
    public class InvitationManagerServiceTests
    {
        private readonly Mock<IInvitationRepository> invitationRepositoryMock;
        private readonly Mock<IClassroomRepository> classroomRepositoryMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<ILogger<InvitationManagerService>> loggerMock;
        private readonly InvitationManagerService invitationManagerService;

        public InvitationManagerServiceTests()
        {
            invitationRepositoryMock = new Mock<IInvitationRepository>();
            classroomRepositoryMock = new Mock<IClassroomRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            loggerMock = new Mock<ILogger<InvitationManagerService>>();

            invitationManagerService = new InvitationManagerService(
                invitationRepositoryMock.Object,
                classroomRepositoryMock.Object,
                userRepositoryMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task BindInvitationsAfterSignUpAsync_ShouldBindInvitations_WhenInvitationsExist()
        {
            // Arrange
            var user = new UserEntity { Email = "test@example.com" };
            var invitationEntity = new InvitationEntity { Email = "test@example.com" };
            invitationRepositoryMock.Setup(repo => repo.GetAllInvitationsForEmail(user.Email))
                .ReturnsAsync(new List<InvitationEntity> { invitationEntity });

            // Act
            await invitationManagerService.BindInvitationsAfterSignUpAsync(user);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAllInvitationsForEmail(user.Email), Times.Once);
            invitationEntity.User = user;
            invitationRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task InviteUserIntoClassroom_ShouldAddInvitation_WhenUserExists()
        {
            // Arrange
            int classroomPk = 1;
            string email = "test@example.com";
            var user = new UserEntity { Email = email };
            userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
                .ReturnsAsync(user);
            invitationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<InvitationEntity>()))
                .ReturnsAsync(true);

            // Act
            bool result = await invitationManagerService.InviteUserIntoClassroom(classroomPk, email);

            // Assert
            userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
            invitationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<InvitationEntity>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task RevokeUserInvitesFromClassroom_ShouldDeleteInvitation_WhenInvitationExists()
        {
            // Arrange
            int classroomPk = 1;
            string email = "test@example.com";
            var invitation = new InvitationEntity { Email = email };
            invitationRepositoryMock.Setup(repo => repo.GetInvitationByClassroomAndEmail(classroomPk, email))
                .ReturnsAsync(invitation);
            invitationRepositoryMock.Setup(repo => repo.DeleteAsync(invitation))
                .ReturnsAsync(true);

            // Act
            bool result = await invitationManagerService.RevokeUserInvitesFromClassroom(classroomPk, email);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetInvitationByClassroomAndEmail(classroomPk, email), Times.Once);
            invitationRepositoryMock.Verify(repo => repo.DeleteAsync(invitation), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task GetInvitationsByUser_ShouldReturnInvitations_WhenInvitationsExist()
        {
            // Arrange
            string userPk = "123";
            var invitations = new List<InvitationEntity> { new InvitationEntity(), new InvitationEntity() };
            invitationRepositoryMock.Setup(repo => repo.GetAllInvitationsForUser(userPk))
                .ReturnsAsync(invitations);

            // Act
            List<InvitationEntity> result = await invitationManagerService.GetInvitationsByUser(userPk);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAllInvitationsForUser(userPk), Times.Once);
            Assert.Equal(invitations, result);
        }

        [Fact]
        public async Task GetInvitationsByClassroom_ShouldReturnInvitations_WhenInvitationsExist()
        {
            // Arrange
            int classroomPk = 1;
            var invitations = new List<InvitationEntity> { new InvitationEntity(), new InvitationEntity() };
            invitationRepositoryMock.Setup(repo => repo.GetAllInvitationsForClassroom(classroomPk))
                .ReturnsAsync(invitations);

            // Act
            List<InvitationEntity> result = await invitationManagerService.GetInvitationsByClassroom(classroomPk);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAllInvitationsForClassroom(classroomPk), Times.Once);
            Assert.Equal(invitations, result);
        }

        [Fact]
        public async Task GetInvitationEntityByUserAndId_ShouldReturnInvitation_WhenInvitationExists()
        {
            // Arrange
            string userPk = "123";
            Guid uuid = Guid.NewGuid();
            var invitation = new InvitationEntity();
            invitationRepositoryMock.Setup(repo => repo.GetAllInvitationsByIdAndUser(userPk, uuid))
                .ReturnsAsync(invitation);

            // Act
            InvitationEntity? result = await invitationManagerService.GetInvitationEntityByUserAndId(userPk, uuid);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAllInvitationsByIdAndUser(userPk, uuid), Times.Once);
            Assert.Equal(invitation, result);
        }

        [Fact]
        public async Task GetInvitationEntityByClassroomAndId_ShouldReturnInvitation_WhenInvitationExists()
        {
            // Arrange
            int classroomPk = 1;
            Guid uuid = Guid.NewGuid();
            var invitation = new InvitationEntity();
            invitationRepositoryMock.Setup(repo => repo.GetAllInvitationsByIdAndClassroom(classroomPk, uuid))
                .ReturnsAsync(invitation);

            // Act
            InvitationEntity? result = await invitationManagerService.GetInvitationEntityByClassroomAndId(classroomPk, uuid);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAllInvitationsByIdAndClassroom(classroomPk, uuid), Times.Once);
            Assert.Equal(invitation, result);
        }

        [Fact]
        public async Task AcceptInvitation_ShouldAcceptInvitationAndAddUserToClassroom_WhenInvitationIsValid()
        {
            // Arrange
            string userPk = "123";
            Guid id = Guid.NewGuid();
            var invitation = new InvitationEntity
            {
                UserId = userPk,
                InvitationStatus = InvitationStatus.Pending,
                ClassroomId = 1
            };
            var classroom = new ClassroomEntity();
            invitationRepositoryMock.Setup(repo => repo.GetAsync(id))
                .ReturnsAsync(invitation);
            classroomRepositoryMock.Setup(repo => repo.GetAsync(invitation.ClassroomId))
                .ReturnsAsync(classroom);
            invitationRepositoryMock.Setup(repo => repo.DeleteAsync(invitation))
                .ReturnsAsync(true);
            classroomRepositoryMock.Setup(repo => repo.AddUserIntoClassroom(invitation.ClassroomId, userPk))
                .ReturnsAsync(true);

            // Act
            bool result = await invitationManagerService.AcceptInvitation(userPk, id);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAsync(id), Times.Once);
            classroomRepositoryMock.Verify(repo => repo.GetAsync(invitation.ClassroomId), Times.Once);
            invitationRepositoryMock.Verify(repo => repo.DeleteAsync(invitation), Times.Once);
            classroomRepositoryMock.Verify(repo => repo.AddUserIntoClassroom(invitation.ClassroomId, userPk), Times.Once);
            Assert.True(result);
            Assert.Equal(InvitationStatus.Accepted, invitation.InvitationStatus);
        }

        [Fact]
        public async Task DeclineInvitation_ShouldDeclineInvitation_WhenInvitationIsValid()
        {
            // Arrange
            string userPk = "123";
            Guid id = Guid.NewGuid();
            var invitation = new InvitationEntity
            {
                UserId = userPk,
                InvitationStatus = InvitationStatus.Pending
            };
            invitationRepositoryMock.Setup(repo => repo.GetAsync(id))
                .ReturnsAsync(invitation);
            invitationRepositoryMock.Setup(repo => repo.DeleteAsync(invitation))
                .ReturnsAsync(true);

            // Act
            bool result = await invitationManagerService.DeclineInvitation(userPk, id);

            // Assert
            invitationRepositoryMock.Verify(repo => repo.GetAsync(id), Times.Once);
            invitationRepositoryMock.Verify(repo => repo.DeleteAsync(invitation), Times.Once);
            Assert.True(result);
            Assert.Equal(InvitationStatus.Declined, invitation.InvitationStatus);
        }
    }
}
