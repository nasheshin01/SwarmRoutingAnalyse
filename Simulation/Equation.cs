namespace Simulation;

public enum Equation
{
    Equal = 0,
    NotEqual = 1,
    More = 2,
    Less = 3
}

public static class EquationExtensions
{
    public static string ToStringEquation(this Equation me)
    {
        return me switch
        {
            Equation.Equal => "=",
            Equation.NotEqual => "!=",
            Equation.More => ">",
            Equation.Less => "<",
            _ => throw new ArgumentOutOfRangeException(nameof(me), me, null)
        };
    }
}