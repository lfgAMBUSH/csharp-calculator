using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Csharp_calculator
{
    public class Standart
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
                    case '+':
                    case '-':
                        return 1;
                    case '*':
                    case '/':
                        return 2;
                    case '~':
                        return 3;
                    default:
                        return 0;
                }
            }
            
            if(infix.Length == 0)
            {
                throw new FormatException("Поле не может быть пустым");
            }
            for (int i = 0; i < infix.Length; i++)
            { 
                char c = infix[i];
                if (char.IsDigit(c) || c == '.')
                {
                    output += c;
                }
                else if ("~".Contains(c))
                {
                    while (stack.Count > 0 && Priority(stack.Peek()) >= Priority(c))
                    {
                        output += stack.Pop() + " ";
                    }
                    stack.Push(c);
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
                        throw new FormatException("Нет открывающей скобки");
                    }
                    stack.Pop();
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            

            while (stack.Count > 0)
            {
                if(stack.Peek() == '(')
                {
                    throw new FormatException("Нет закрывающей скобки");
                }
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
                if (Double.TryParse(c, out double d))
                {
                    stack.Push(d);
                }
                else if ("~".Contains(c))
                {
                    double a = stack.Pop();
                    stack.Push(a * -1);
                }
                else if ("*+-/".Contains(c))
                {
                    double a = stack.Pop();
                    double b = stack.Pop();
                    switch (c)
                    {
                        case "+":
                            stack.Push(b + a);
                            break;
                        case "-":
                            stack.Push(b - a);
                            break;
                        case "/":
                            stack.Push(b / a);
                            break;
                        case "*":
                            stack.Push(b * a);
                            break;
                    }
                }
            }
            return stack.Pop();
        }

    }
}
