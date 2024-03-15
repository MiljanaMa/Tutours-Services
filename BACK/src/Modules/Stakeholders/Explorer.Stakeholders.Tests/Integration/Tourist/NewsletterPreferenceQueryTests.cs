using Explorer.API.Controllers.Tourist;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Tests.Integration.Tourist
{
    public class NewsletterPreferenceQueryTests : BaseStakeholdersIntegrationTest
    {
        public NewsletterPreferenceQueryTests(StakeholdersTestFactory factory) : base(factory) { }
        
        [Fact]
        public void Retrieves_all()
        {
            //Arrange
            using var scope = Factory.Services.CreateScope();
            var controller = CreateController(scope);

            //Act
            var result = ((ObjectResult)controller.GetAll(0, 0).Result)?.Value as PagedResult<NewsletterPreferenceDto>;

            //Assert
            result.ShouldNotBeNull();
            result.Results.Count.ShouldBe(3);
            result.TotalCount.ShouldBe(3);
        }

        private static NewsletterPreferenceController CreateController(IServiceScope scope)
        {
            return new NewsletterPreferenceController(scope.ServiceProvider.GetRequiredService<INewsletterPreferenceService>())
            {
                ControllerContext = BuildContext("-1")
            };
        }
    }
}
