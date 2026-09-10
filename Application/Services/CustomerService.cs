using ApiDoces.Dtos.Customer;
using Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDoces.Services;

public class CustomerService(ApplicationDbContext context)
{
    public async Task CreateCustomer(CreateCustomerDto dto)
    {
        var customer = dto.ToEntity();

        context.Customer.Add(customer);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
    {
        var customers = await context.Customer.ToListAsync();
        return customers.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CustomerForSaleDto>> GetCustomersForSale()
    {
        return await context.Customer
            .Where(c => c.Active)
            .Select(c => new CustomerForSaleDto
            {
                Id = c.CustomerId,
                Name = c.Name,
                City = c.City
            })
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetById(int id)
    {
        var customer = await context.Customer
         .FirstOrDefaultAsync(c => c.CustomerId == id);

        return customer?.ToDto();
    }

    public async Task<CustomerDto?> UpdateCustomer(int id, UpdateCustomerDto dto)
    {
        var customer = await context.Customer.FindAsync(id);

        if (customer == null)
            return null;

        customer.UpdateFromDto(dto);
        await context.SaveChangesAsync();

        return customer.ToDto();
    }

    public async Task<CustomerDto?> ToggleActive(int id)
    {
        var customer = await context.Customer.FindAsync(id);

        if (customer == null)
            return null;

        customer.Active = !customer.Active;

        await context.SaveChangesAsync();

        return customer.ToDto();
    }
}