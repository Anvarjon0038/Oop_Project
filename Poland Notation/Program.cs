using Poland_Notation;


string expression = "( 10 + 3 ) / 4";


Calculator calculator = new Calculator();


double result = calculator.Calculate(expression);


Console.WriteLine("Result: " + result);