using CalculatorApi.Data;
using CalculatorApi.FeatureToggles;
using CalculatorApi.Models;
using CalculatorApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace CalculatorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly CalculatorService _calculatorService;
    private readonly CalculatorDbContext _context;
    private IFeatureManager _featureManager;

    public CalculatorController(CalculatorService calculatorService, CalculatorDbContext context,
        IFeatureManager featureManager)
    {
        _calculatorService = calculatorService;
        _context = context;
        _featureManager = featureManager;
    }

    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] CalculationRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Expression))
            return BadRequest("Expression is required");
        
        request.Result = _calculatorService.Evaluate(request.Expression);

        if (await _featureManager.IsEnabledAsync(FeatureFlags.SaveCalculationHistory))
        {
            _context.Calculations.Add(request);
            await _context.SaveChangesAsync();
        }
        return Ok(new {result = request.Result});
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.SaveCalculationHistory))
        {
            return Ok(new List<CalculationRequest>());
        }

        var history = await _context.Calculations
            .OrderByDescending(c => c.Timestamp)
            .Take(10)
            .ToListAsync();

        return Ok(history);
    }
}