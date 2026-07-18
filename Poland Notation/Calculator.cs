namespace Poland_Notation;

public class Calculator
{
    private readonly ExpressionConverter converter;


    public Calculator()
    {
        converter = new ExpressionConverter();
    }


    public double Calculate(string expression)
    {
        Console.WriteLine("Infix: " + expression);


        string postfix = converter.InfixToPostfix(expression);


        Console.WriteLine("Postfix: " + postfix);


        Expression result = new Expression(postfix);


        return result.Evaluate();
    }
}