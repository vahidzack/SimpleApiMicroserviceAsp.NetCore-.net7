using BehpouyanApiProject.Models;
using System.Net.Http.Json;

namespace IntegTest
{
    public class UnitTest1: IClassFixture<CustomWebApplicationFactory>
    {
        #region Constractor
        private readonly HttpClient _client;
        public UnitTest1(CustomWebApplicationFactory customWebApplicationFactory)
        {
                _client= customWebApplicationFactory.CreateClient();
        }
        #endregion

        [Fact]
        public async Task GetCustomers_ReturnsOkWithCustomersList()
        {
            // Act
            var response = await _client.GetAsync("/api/Customer/GetCustomers");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.MediaType);

            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();
            Assert.NotNull(customers);
            Assert.NotEmpty(customers);
        }


        [Fact]
        public async Task GetCustomerById_ReturnsOkWithCustomer()
        {
            // Arrange
            var customerId = 2; // Assuming this ID exists in the database

            // Act
            var response = await _client.GetAsync($"/api/Customer/GetbyId/{customerId}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.MediaType);

            var customer = await response.Content.ReadFromJsonAsync<Customer>();
            Assert.NotNull(customer);
            Assert.Equal(customerId, customer.Id);
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

            // Act
            var response = await _client.PostAsJsonAsync("/api/Customer/CreateCustomer", newCustomer);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
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

            // Act
            var response = await _client.PutAsJsonAsync("/api/Customer/UpdateCustomer", existingCustomer);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOk()
        {
            // Arrange
            var customerIdToDelete = 1; // Assuming this ID exists in the database

            // Act
            var response = await _client.DeleteAsync($"/api/Customer/DeleteCustomer/{customerIdToDelete}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200
        }

    }
}