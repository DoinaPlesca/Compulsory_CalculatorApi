using NCalc;

namespace CalculatorApi.Services;

public class CalculatorService
{
    public double Evaluate(string expression)
    {
        var calc = new Expression(expression);

        if (!calc.HasErrors())
        {
            var result = calc.Evaluate();
            return Convert.ToDouble(result);
        }
        else
        {
            throw new ArgumentException("Invalid expression.");
        }
    }
}