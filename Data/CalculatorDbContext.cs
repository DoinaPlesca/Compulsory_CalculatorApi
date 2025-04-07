using CalculatorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatorApi.Data;

public class CalculatorDbContext : DbContext
{
    public CalculatorDbContext (DbContextOptions<CalculatorDbContext> options) 
        : base(options) {}
    
    public DbSet<CalculationRequest> Calculations { get; set; }
}