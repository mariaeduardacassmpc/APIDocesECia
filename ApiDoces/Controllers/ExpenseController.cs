using ApiDoces.Services;
using ApiDoces.Dtos.Expense;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController(ExpenseService ExpenseService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateExpense(CreateExpenseDto dto)
    {
        await ExpenseService.CreateExpense(dto);
        return Ok(new { message = "Expense created successfully." });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var expenses = await ExpenseService.GetAllExpenses();

        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var expense = await ExpenseService.GetById(id);

        if (expense == null)
            return NotFound(new { mensagem = "Expense not found" });

        return Ok(expense);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        var expense = await ExpenseService.UpdateExpense(id, dto);

        if (expense == null)
            return NotFound(new { mensagem = "Expense not found" });

        return Ok(new { message = "Expense updated successfully", dto = expense });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)  
    {
        var deleted = await ExpenseService.DeleteExpense(id);

        if (!deleted)
            return NotFound(new { mensagem = "Expense not found" });

        return Ok(new { message = "Product deleted successfully!" });
    }

    [HttpGet("financial-summary")]
    public async Task<IActionResult> GetFinancial(int month, int year)
    {
        var financials = await ExpenseService
           .GetFinancialSummary(month, year);

        return Ok(financials);
    }
}