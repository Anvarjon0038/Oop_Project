// See https://aka.ms/new-console-template for more information

using Poland_Notation;

string infinix="( 10 + 3 ) / 4)";
ExpressionConverter converter = new ExpressionConverter();
string expression = converter.InfinixToPostfix(infinix);

Console.WriteLine("Infix " + infinix);




