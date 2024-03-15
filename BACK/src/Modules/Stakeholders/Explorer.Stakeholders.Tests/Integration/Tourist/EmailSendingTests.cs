using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Database.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.Tests.Integration.Tourist
{
    public class EmailSendingTests
    {
        [Fact]
        public void Sends_email_when_user_registers()
        {
            var mockNotify = new Mock<IEmailVerificationService>();
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(r => r.Create(It.IsAny<User>()))
            .Returns((User user) => user);

            var mockPersonRepository = new Mock <ICrudRepository<Person>>();

            var mockTokenGenerator = new Mock<ITokenGenerator>();
            AuthenticationService service = new AuthenticationService(mockUserRepository.Object, mockPersonRepository.Object,
            mockTokenGenerator.Object, mockNotify.Object);


            var account = new AccountRegistrationDto
            {
                Name = "Mika",
                Username="mikamikic",
                Password="mika",
                Email="mikamikic@gmail.com",
                Surname = "Mikic",
                ProfileImage = "im",
                Biography = "biography",
                Quote = "quote",
                XP = 0,
                Level = 0
            };
            service.RegisterTourist(account);

            mockNotify.Verify(n => n.SendEmail(account.Email, It.IsAny<string>(), It.IsAny<string>()));

        }
        
        
    }
}
