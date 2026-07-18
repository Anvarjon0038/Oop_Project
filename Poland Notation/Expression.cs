namespace Poland_Notation;

public class Expression : IExpression
{
    private string expression;

    public Expression(string expression)
    {
        this.expression = expression;
    }

    public double Evaluate()
    {
        Stack<double> stack = new Stack<double>();
        string[] tokens = expression.Split(' ');
        
        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else
            {
                double b = stack.Pop();
                double a = stack.Pop();
                switch(token)
                {
                    case "+":
                        stack.Push(a + b);
                        break;
                    
                    case "-":
                        stack.Push(a - b);
                        break;

                    case "*":
                        stack.Push(a * b);
                        break;

                    case "/":
                        stack.Push(a / b);
                        break;
                }
            }
        }
        return stack.Pop();
    }
}