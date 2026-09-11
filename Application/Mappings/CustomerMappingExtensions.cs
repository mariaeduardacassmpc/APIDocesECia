using Application.Dtos.Customer;
using Data.Entities;

public static class CustomerMappingExtensions
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.CustomerId,
            Name = customer.Name,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            Email = customer.Email,
            Obs = customer.Obs,
            Active = customer.Active
        };
    }

    public static Customer ToEntity(this InputCustomerDto dto)
    {
        return new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Email = dto.Email,
            Obs = dto.Obs,
            Active = true
        };
    }
}