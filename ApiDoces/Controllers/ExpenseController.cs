using ApiDoces.Services;
using ApiDoces.Dtos.Expense;
using ApiDoces.Helpers;
using ApiDoces.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController(ExpenseService expenseService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
    {
        await expenseService.CreateExpense(dto);

        return Ok(ApiResponse.Success(ApiMessages.ExpenseCreated));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var expenses = await expenseService.GetAllExpenses();

        return Ok(ApiResponse.Success(expenses));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await expenseService.GetById(id);

        if (expense == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.ExpenseNotFound));

        return Ok(ApiResponse.Success(expense));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        var expense = await expenseService.UpdateExpense(id, dto);

        if (expense == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.ExpenseNotFound));

        return Ok(ApiResponse.Success(expense, ApiMessages.ExpenseUpdated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var deleted = await expenseService.DeleteExpense(id);

        if (!deleted)
            return NotFound(ApiResponse.NotFound(ApiMessages.ExpenseNotFound));

        return Ok(ApiResponse.Success(ApiMessages.ExpenseDeleted));
    }

    [HttpGet("financial-summary")]
    public async Task<IActionResult> GetFinancial(int month, int year)
    {
        var financials = await expenseService.GetFinancialSummary(month, year);

        return Ok(ApiResponse.Success(financials));
    }
}