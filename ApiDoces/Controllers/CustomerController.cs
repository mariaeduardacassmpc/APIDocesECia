using Application.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using ApiDoces.Services.Report;
using Application.Dtos.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CustomerController(CustomerService customerService, CustomerReportService customerReportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(InputCustomerDto dto)
    {
        var customer = await customerService.CreateCustomer(dto);
        return Ok(ApiResponses.Created(customer, ApiMessages.Created("Cliente")));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await customerService.GetAllCustomers();

        return Ok(ApiResponses.Success(customers));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await customerService.GetById(id);

        return Ok(ApiResponses.Success(customer));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, InputCustomerDto dto)
    {
        var customer = await customerService.UpdateCustomer(id, dto);

        return Ok(ApiResponses.Success(customer, ApiMessages.Updated("Cliente")));
    }

    [HttpGet("for-sale")]
    public async Task<ActionResult<IEnumerable<CustomerForSaleDto>>> GetCustomersForSale()
    {
        var customers = await customerService.GetCustomersForSale();

        return Ok(ApiResponses.Success(customers));
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var customer = await customerService.ToggleActive(id);

        return Ok(ApiResponses.Success(customer));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetCustomerReport()
    {
        var file = await customerReportService.GenerateCustomerReport();

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"customer-report-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }
}