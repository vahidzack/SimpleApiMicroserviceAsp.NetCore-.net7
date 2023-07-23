using BehpouyanApiProject.Controllers;
using BehpouyanApiProject.Models;
using BehpouyanApiProject.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BehpouyanApiProject.Tests
{
    public class CustomerControllerTests
    {
        // Helper method to create the controller instance with a mocked repository
        private CustomerController CreateController(ICustomerRepository customerRepository)
        {
            return new CustomerController(customerRepository);
        }

        // Helper method to generate a list of sample customers for testing
        private List<Customer> GetSampleCustomers()
        {
            return new List<Customer>
            {
                new Customer { Id = 0, CustomerName = "John Doe", Email = "john@example.com" ,Address="shandiz",PhoneNumber="1232323" },
                new Customer { Id = 0, CustomerName = "vahid zaker", Email = "john@example.com" ,Address="shandiz",PhoneNumber="1232323" },
            };
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnOkWithCustomersList()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var sampleCustomers = GetSampleCustomers();
            mockRepository.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(sampleCustomers);

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var customers = Xunit.Assert.IsAssignableFrom<List<Customer>>(okResult.Value);
            Xunit.Assert.Equal(sampleCustomers.Count, customers.Count);
        }

        [Fact]
        public async Task GetbyId_ValidCustomerId_ShouldReturnOkWithCustomer()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var sampleCustomers = GetSampleCustomers();
            var customerId = 1;
            mockRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync(sampleCustomers[0]);

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.GetbyId(customerId);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var customer = Xunit.Assert.IsType<Customer>(okResult.Value);
            Xunit.Assert.Equal(customerId, customer.Id);
        }

        [Fact]
        public async Task GetbyId_InvalidCustomerId_ShouldReturnNotFound()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var customerId = 100; // Assuming this ID does not exist in the repository
            mockRepository.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync((Customer)null);

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.GetbyId(customerId);

            // Assert
            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_ValidCustomer_ShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var customerToCreate = new Customer { Id = 0, CustomerName = "John Doe", Email = "john@example.com", Address = "shandiz", PhoneNumber = "1232323" };

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.CreateCustomer(customerToCreate);

            // Assert
            Xunit.Assert.IsType<OkResult>(result);
            mockRepository.Verify(repo => repo.AddCustomerAsync(customerToCreate), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomer_ValidCustomer_ShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var customerToUpdate = new Customer { Id = 0, CustomerName = "John Doe", Email = "john@example.com", Address = "shandiz", PhoneNumber = "1232323" };

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.UpdateCustomer(customerToUpdate);

            // Assert
            Xunit.Assert.IsType<OkResult>(result);
            mockRepository.Verify(repo => repo.UpdateCustomerAsync(customerToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ValidCustomerId_ShouldReturnOk()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var sampleCustomers = GetSampleCustomers();
            var customerIdToDelete = 1;
            mockRepository.Setup(repo => repo.GetCustomerByIdAsync(customerIdToDelete)).ReturnsAsync(sampleCustomers[0]);

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.DeleteCustomer(customerIdToDelete);

            // Assert
            Xunit.Assert.IsType<OkResult>(result);
            mockRepository.Verify(repo => repo.DeleteCustomerAsync(sampleCustomers[0]), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_InvalidCustomerId_ShouldReturnNotFound()
        {
            // Arrange
            var mockRepository = new Mock<ICustomerRepository>();
            var customerIdToDelete = 100; // Assuming this ID does not exist in the repository
            mockRepository.Setup(repo => repo.GetCustomerByIdAsync(customerIdToDelete)).ReturnsAsync((Customer)null);

            var controller = CreateController(mockRepository.Object);

            // Act
            var result = await controller.DeleteCustomer(customerIdToDelete);

            // Assert
            Xunit.Assert.IsType<NotFoundResult>(result);
            mockRepository.Verify(repo => repo.DeleteCustomerAsync(It.IsAny<Customer>()), Times.Never);
        }
    }
}
