using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_calculator
{
    internal class Calc
    {
        public static string Calculate(string str)
        {
            str = str.Replace(" ", "");
            var postfix = Postfix(str);

            return EvalPostfix(postfix).ToString();
        }

        private static string Postfix(string infix)
        {
            Stack<char> stack = new Stack<char>();
            string output = "";
            int Priority(char c)
            {
                switch (c)
                {
                    case '+': case '-':
                        return 1;
                    case '*': case '/':
                        return 2;
                    default:
                        return 0;   
                }
            }
            for (int i = 0; i < infix.Length; i++)
            {
                char c = infix[i];
                if (char.IsDigit(c)|| c == '.')
                {
                    output += c;
                }
                else if ("*+-/".Contains(c))
                {
                    output += " ";
                    while (stack.Count > 0 && Priority(stack.Peek()) >= Priority(c))
                    {
                        output += stack.Pop() + " ";
                    }
                    stack.Push(c);
                }
                else if (c == '(')
                {
                    stack.Push(c);
                }
                else if (c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output += " " + stack.Pop().ToString();
                    }
                    if (stack.Count == 0)
                    {

                    }
                    stack.Pop();
                }
                else
                {

                }
            }

            while (stack.Count > 0)
            {
                output += " " + stack.Pop().ToString();
            }
            return output;
        }

        private static double EvalPostfix(string postfix)
        {
            Stack<double> stack = new Stack<double>();
            var mas = postfix.Split();

            foreach (var c in mas)
            {
                if(Double.TryParse(c, out double d))
                {
                    stack.Push(d);
                }
                else if ("*+-/".Contains(c))
                {
                    double a = stack.Pop();
                    double b = stack.Pop();
                    switch (c)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;
                        case "-":
                            stack.Push(a - b);
                            break;
                        case "/":
                            stack.Push(a / b);
                            break;
                        case "*":
                            stack.Push(a * b);
                            break;
                    }  
                }
            }
            return stack.Pop();
        }

    }
}
