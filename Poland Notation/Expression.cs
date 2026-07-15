namespace Poland_Notation;

public class Expression : IExpresion
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
            double number;
            if (double.TryParse(token, out number))
            {
                stack.Push(number);
            }
            else
            {
                double b=stack.Pop();
                double a=stack.Pop();
                if(token == "+")stack.Push(a+b);
                else if (token=="-")stack.Push(a-b);
                else if(token=="*")stack.Push(a*b);
                else stack.Push(a/b);
            }
        }
        return stack.Pop();
    }
}