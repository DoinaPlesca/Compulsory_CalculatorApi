namespace CalculatorApi.Models;

public class CalculationRequest
{
    public int Id { get; set; }
    public string Expression { get; set; }
    public double Result { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}