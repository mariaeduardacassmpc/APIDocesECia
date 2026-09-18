using Application.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using Application.Dtos.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ExpenseController(ExpenseService expenseService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateExpense(InputExpenseDto dto)
    {
        await expenseService.CreateExpense(dto);

        return Ok(ApiResponses.Created<object?>(null, ApiMessages.Created("Despesa")));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var expenses = await expenseService.GetAllExpenses();

        return Ok(ApiResponses.Success(expenses));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await expenseService.GetById(id);

        return Ok(ApiResponses.Success(expense));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, InputExpenseDto dto)
    {
        var expense = await expenseService.UpdateExpense(id, dto);

        return Ok(ApiResponses.Success(expense, ApiMessages.Updated("Despesa")));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        await expenseService.DeleteExpense(id);

        return Ok(ApiResponses.Success(ApiMessages.Deleted("Despesa")));
    }

    [HttpGet("financial-summary")]
    public async Task<IActionResult> GetFinancial(int month, int year)
    {
        var financials = await expenseService.GetFinancialSummary(month, year);

        return Ok(ApiResponses.Success(financials));
    }
}