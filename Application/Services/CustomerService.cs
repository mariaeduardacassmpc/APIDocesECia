using Application.Helpers;
using Application.Dtos.Customer;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class CustomerService(ApplicationDbContext context, ILogger<CustomerService> logger)
{
    public async Task<CustomerDto> CreateCustomer(InputCustomerDto dto)
    {
        logger.LogInformation("Criando cliente: {CustomerName}", dto.Name);

        var customer = dto.ToEntity();
        context.Customer.Add(customer);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Customer_Email") == true)
        {
            logger.LogWarning("Tentativa de criar cliente duplicado. Email: {Email}", dto.Email);
            throw new InvalidOperationException(ApiMessages.Email.AlreadyExists);
        }

        logger.LogInformation("Cliente criado com sucesso. Id: {CustomerId}", customer.CustomerId);

        return customer.ToDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
    {
        logger.LogInformation("Buscando todos os clientes");

        var customers = await context.Customer.ToListAsync();

        logger.LogInformation("Foram encontrados {Count} clientes", customers.Count);

        return customers.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CustomerForSaleDto>> GetCustomersForSale()
    {
        logger.LogInformation("Buscando clientes ativos para venda");

        var customers = await context.Customer
            .Where(c => c.Active)
            .Select(c => new CustomerForSaleDto
            {
                Id = c.CustomerId,
                Name = c.Name,
                City = c.City
            })
            .ToListAsync();

        logger.LogInformation("Foram encontrados {Count} clientes ativos", customers.Count);

        return customers;
    }

    public async Task<CustomerDto> GetById(int id)
    {
        logger.LogInformation("Buscando cliente por Id: {CustomerId}", id);

        var customer = await context.Customer.FirstOrDefaultAsync(c => c.CustomerId == id);

        if (customer == null)
        {
            logger.LogWarning("Cliente não encontrado. Id: {CustomerId}", id);
            throw new InvalidOperationException($"Cliente com Id {id} não encontrado.");
        }

        return customer.ToDto();
    }

    public async Task<CustomerDto> UpdateCustomer(int id, InputCustomerDto dto)
    {
        logger.LogInformation("Atualizando cliente. Id: {CustomerId}", id);

        var customer = await context.Customer.FindAsync(id);

        if (customer == null)
        {
            logger.LogWarning("Cliente não encontrado para atualização. Id: {CustomerId}", id);
            throw new InvalidOperationException($"Cliente com Id {id} não encontrado.");
        }

        var emailExists = await context.Customer
            .AnyAsync(c => c.Email == dto.Email && c.CustomerId != id);

        if (emailExists)
        {
            logger.LogWarning(
                "Tentativa de atualizar cliente com e-mail já cadastrado. Email: {Email}",
                dto.Email);

            throw new InvalidOperationException(ApiMessages.Email.AlreadyExists);
        }

        dto.UpdateEntity(customer);

        await context.SaveChangesAsync();

        logger.LogInformation("Cliente atualizado com sucesso. Id: {CustomerId}", id);

        return customer.ToDto();
    }

    public async Task<CustomerDto> ToggleActive(int id)
    {
        logger.LogInformation("Alterando status do cliente. Id: {CustomerId}", id);

        var customer = await context.Customer.FindAsync(id);

        if (customer == null)
        {
            logger.LogWarning("Cliente não encontrado para alteração de status. Id: {CustomerId}", id);
            throw new InvalidOperationException($"Cliente com Id {id} não encontrado.");
        }

        customer.Active = !customer.Active;

        await context.SaveChangesAsync();

        logger.LogInformation("Status do cliente alterado. Id: {CustomerId}, Ativo: {Active}", id, customer.Active);

        return customer.ToDto();
    }
}