using BehpouyanApiProject.Models;
using Moq;
using System.Net.Http.Json;
using Xunit;

namespace BehpouyanApiProject.IntegrationTest.ControllersTest
{
    public class CustomerControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public CustomerControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Fact]
        public async Task GetCustomers_ReturnsOkWithCustomersList()
        {
            // Arrange
            var customers = new List<Customer>
    {
        new Customer { Id = 1, CustomerName = "John Doe", Email = "john@example.com", Address = "New York", PhoneNumber = "1234567890" },
        new Customer { Id = 2, CustomerName = "Jane Doe", Email = "jane@example.com", Address = "Los Angeles", PhoneNumber = "9876543210" }
        // Add more test data if needed
    };

            var customerRepositoryMock = _factory.CustomerRepositoryMock;
            customerRepositoryMock.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);

            // Act
            var response = await _client.GetAsync("/api/Customer/GetCustomers");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            // Check if the response content type is as expected
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var responseCustomers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            Assert.NotNull(responseCustomers);
            Assert.NotEmpty(responseCustomers);
            Assert.Equal(customers.Count, responseCustomers.Count);
            Assert.Equal(customers[0].CustomerName, responseCustomers[0].CustomerName);
            Assert.Equal(customers[1].CustomerName, responseCustomers[1].CustomerName);
            
        }


        [Fact]
        public async Task GetbyId_ReturnsOkWithCustomer()
        {
            // Arrange
            var customerId = 2; // Assuming this ID exists in the database
            var customer = new Customer { Id = customerId, CustomerName = "Jane Doe", Email = "jane@example.com", Address = "Los Angeles", PhoneNumber = "9876543210" };

            var customerRepositoryMock = _factory.CustomerRepositoryMock;
            customerRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(customerId)).ReturnsAsync(customer);

            // Act
            var response = await _client.GetAsync($"/api/Customer/GetbyId/{customerId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            // Check if the response content type is as expected
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var responseCustomer = await response.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(responseCustomer);
            Assert.Equal(customer.Id, responseCustomer.Id);
            Assert.Equal(customer.CustomerName, responseCustomer.CustomerName);
            Assert.Equal(customer.Email, responseCustomer.Email);
            Assert.Equal(customer.Address, responseCustomer.Address);
            Assert.Equal(customer.PhoneNumber, responseCustomer.PhoneNumber);
           
        }


        [Fact]
        public async Task CreateCustomer_ReturnsOk()
        {
            // Arrange
            var newCustomer = new Customer
            {
                CustomerName = "Jane Doe",
                Email = "jane@example.com",
                Address = "New York",
                PhoneNumber = "9876543210"
            };

            var customerRepositoryMock = _factory.CustomerRepositoryMock;

            // Act
            var response = await _client.PostAsJsonAsync("/api/Customer/CreateCustomer", newCustomer);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            customerRepositoryMock.Verify(repo => repo.AddCustomerAsync(It.IsAny<Customer>()), Times.Once);
            // You can also verify the customer data that was passed to the repository method, if needed
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsOk()
        {
            // Arrange
            var existingCustomer = new Customer
            {
                Id = 2, // Assuming this ID exists in the database
                CustomerName = "John Doe",
                Email = "john@example.com",
                Address = "New York",
                PhoneNumber = "1234567890"
            };

            var customerRepositoryMock = _factory.CustomerRepositoryMock;
            customerRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(existingCustomer.Id)).ReturnsAsync(existingCustomer);

            // Act
            var response = await _client.PutAsJsonAsync("/api/Customer/UpdateCustomer", existingCustomer);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            customerRepositoryMock.Verify(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>()), Times.Once);
            // You can also verify the customer data that was passed to the repository method, if needed
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOk()
        {
            // Arrange
            var customerIdToDelete = 1; // Assuming this ID exists in the database

            var customerRepositoryMock = _factory.CustomerRepositoryMock;
            customerRepositoryMock.Setup(repo => repo.GetCustomerByIdAsync(customerIdToDelete)).ReturnsAsync(new Customer());

            // Act
            var response = await _client.DeleteAsync($"/api/Customer/DeleteCustomer/{customerIdToDelete}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            customerRepositoryMock.Verify(repo => repo.DeleteCustomerAsync(It.IsAny<Customer>()), Times.Once);
        }
    }
}
