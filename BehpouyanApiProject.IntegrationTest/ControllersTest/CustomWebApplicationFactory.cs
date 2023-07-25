using BehpouyanApiProject.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BehpouyanApiProject.IntegrationTest.ControllersTest
{
    public class CustomWebApplicationFactory:WebApplicationFactory<Program>
    {

        public Mock<ICustomerRepository> CustomerRepositoryMock { get; }

        public CustomWebApplicationFactory()
        {
            CustomerRepositoryMock = new Mock<ICustomerRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(CustomerRepositoryMock.Object);
            });
        }
    }
}
