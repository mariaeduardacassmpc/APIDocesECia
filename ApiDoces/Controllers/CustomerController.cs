using ApiDoces.Services;
using ApiDoces.Dtos.Customer;
using ApiDoces.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(CustomerService customerService, CustomerReportService customerReportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
    {
        await customerService.CreateCustomer(dto);

        return Ok(new { message = "Customer created successfully!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await customerService.GetAllCustomers();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await customerService.GetById(id);

        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        return Ok(customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto dto)
    {
        var customer = await customerService.UpdateCustomer(id, dto);

        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        return Ok(customer);
    }

    [HttpGet("for-sale")]
    public async Task<ActionResult<IEnumerable<CustomerForSaleDto>>> GetCustomersForSale()
    {
        var customers = await customerService.GetCustomersForSale();
        
        return Ok(customers);
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var customer = await customerService.ToggleActive(id);

        if (customer == null)
            return NotFound(new { message = "Customer not found" });

        return Ok(customer);
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetCustomerReport()
    {
        var file = await customerReportService.GenerateCustomerReport();

        return File(file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"customer-report-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
        );
    }
}