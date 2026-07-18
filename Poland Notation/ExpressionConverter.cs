namespace Poland_Notation;

public class ExpressionConverter
{
    private int Priority(string op)
    {
        if(op == "+" || op == "-")
            return 1;

        if(op == "*" || op == "/")
            return 2;

        return 0;
    }


    public string InfixToPostfix(string expression)
    {
        Stack<string> stack = new Stack<string>();
        List<string> output = new List<string>();

        string[] tokens = expression.Split(' ');


        foreach(string token in tokens)
        {
            if(double.TryParse(token, out _))
            {
                output.Add(token);
            }

            else if(token == "(")
            {
                stack.Push(token);
            }

            else if(token == ")")
            {
                while(stack.Peek() != "(")
                {
                    output.Add(stack.Pop());
                }

                stack.Pop();
            }

            else
            {
                while(stack.Count > 0 &&
                      Priority(stack.Peek()) >= Priority(token))
                {
                    output.Add(stack.Pop());
                }

                stack.Push(token);
            }
        }


        while(stack.Count > 0)
        {
            output.Add(stack.Pop());
        }
        
        return string.Join(" ", output);
    }
}