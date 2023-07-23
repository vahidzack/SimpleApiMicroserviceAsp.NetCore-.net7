using BehpouyanApiProject.Context;
using BehpouyanApiProject.Models;
using BehpouyanApiProject.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject1
{
    public class UnitTest1
    {

        private CustomerDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new CustomerDbContext(options);
        }


        private Customer GetSampleCustomer()
        {
            return new Customer { Id = 1, CustomerName = "John Doe", Email = "john@example.com", Address = "shandiz", PhoneNumber = "1232323" };
        }

        [Fact]
        public async Task AddCustomerAsync_ShouldAddCustomerToDbContext()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext);
            var customerToAdd = GetSampleCustomer();

            // Act
            await customerRepository.AddCustomerAsync(customerToAdd);

            // Assert
            var addedCustomer = await dbContext.Customers.FindAsync(customerToAdd.Id);
            Assert.NotNull(addedCustomer);
            Assert.Equal(customerToAdd.CustomerName, addedCustomer.CustomerName);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldRemoveCustomerFromDbContext()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext);
            var customerToRemove = GetSampleCustomer();
            dbContext.Customers.Add(customerToRemove);
            await dbContext.SaveChangesAsync();

            // Act
            await customerRepository.DeleteCustomerAsync(customerToRemove);

            // Assert
            var removedCustomer = await dbContext.Customers.FindAsync(customerToRemove.Id);
            Assert.Null(removedCustomer);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCorrectCustomer()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext);
            var sampleCustomer = GetSampleCustomer();
            dbContext.Customers.Add(sampleCustomer);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedCustomer = await customerRepository.GetCustomerByIdAsync(sampleCustomer.Id);

            // Assert
            Assert.NotNull(retrievedCustomer);
            Assert.Equal(sampleCustomer.CustomerName, retrievedCustomer.CustomerName);
        }

        [Fact]
        public async Task GetCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext);
            var sampleCustomers = new List<Customer>
            {
                GetSampleCustomer(),
                new Customer { Id = 2, CustomerName = "Jane Smith", Email = "jane@example.com", Address = "shandiz", PhoneNumber = "4567890" }
            };
            dbContext.Customers.AddRange(sampleCustomers);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedCustomers = await customerRepository.GetCustomersAsync();

            // Assert
            Assert.NotNull(retrievedCustomers);
            Assert.Equal(sampleCustomers.Count, retrievedCustomers.Count);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomerInDbContext()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var customerRepository = new CustomerRepository(dbContext);
            var customerToUpdate = GetSampleCustomer();
            dbContext.Customers.Add(customerToUpdate);
            await dbContext.SaveChangesAsync();

            // Update the customer's name
            customerToUpdate.CustomerName = "Updated Name";

            // Act
            await customerRepository.UpdateCustomerAsync(customerToUpdate);

            // Assert
            var updatedCustomer = await dbContext.Customers.FindAsync(customerToUpdate.Id);
            Assert.NotNull(updatedCustomer);
            Assert.Equal(customerToUpdate.CustomerName, updatedCustomer.CustomerName);
        }
    }
}