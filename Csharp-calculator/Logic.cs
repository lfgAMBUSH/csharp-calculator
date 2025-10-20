using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Csharp_calculator
{
    internal class Logic
    {
        private static Dictionary<char, bool> dict = new Dictionary<char, bool>();


        public static char[] dictKeys()
        {
            return dict.Keys.ToArray();
        }
        public static bool[,] Calculat(string c)
        {
            dict.Clear();          
            
            c = Postfix(c);
            tabl(c, out bool[,] mas);
            return mas;
        }

        public static string Postfix(string infix)
        {
            infix = infix.Replace(" ", "");
            Stack<char> stack = new Stack<char>();
            string output = "";
            int Priority(char a)
            {
                switch (a)
                {
                    case '=':
                        return 1;
                    case '>':
                        return 2;
                    case '|':
                        return 3;
                    case '+':
                        return 4;
                    case '^':
                        return 4;
                    case '/':
                        return 4;
                    case '*':
                        return 4;
                    case '!':
                        return 5;
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
                
                if (char.IsLetter(c))
                {
                    output += c;
                    if (!dict.ContainsKey(c))
                    {
                        dict.Add(c, default);
                    }
                }
                else if ("!".Contains(c))
                {
                    
                    while (stack.Count > 0 && Priority(stack.Peek()) > Priority(c))
                    {
                        output += stack.Pop() + " ";
                    }
                    stack.Push(c);
                }
                else if ("*/^+|>=".Contains(c))
                {
                    output += " ";
                    while (stack.Count > 0 && Priority(stack.Peek()) >= Priority(c))
                    {
                        output += stack.Pop() + " ";
                    }
                    stack.Push(c);
                }
                else if(c == '(')
                {
                    stack.Push(c);
                }
                else if(c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                    {
                        output += " " + stack.Pop();
                    }
                    if(stack.Count == 0)
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
                if (stack.Peek() == '(')
                {
                    throw new FormatException("Нет закрывающей скобки");
                }
                output += " " + stack.Pop();
            }
            return output;
        }
        
        public static bool EvalPostfix(string postfix)
        {
            var stack = new Stack<bool>();
            var chars = postfix.Split();
            foreach(string c in chars)
            {
                if(dict.ContainsKey(Convert.ToChar(c)))
                {
                    stack.Push(dict[Convert.ToChar(c)]);
                }
                else if (c == "!")
                {
                    bool d = stack.Pop();
                    stack.Push(!d);
                }
                else if ("*/^+|>=".Contains(c))
                {
                    bool a = stack.Pop();
                    bool b = stack.Pop();
                    
                    switch (c)
                    {
                        case "=":
                            stack.Push(bool.Equals(a, b));
                            break;
                        case ">":
                            stack.Push(a || !b);
                            break;
                        case "|":
                            stack.Push(a || b);
                            break;
                        case "+":
                            stack.Push((!a && b) || (a && !b));
                            break;
                        case "^":
                            stack.Push(!(a || b));
                            break;
                        case "/":
                            stack.Push(!(a && b));
                            break;
                        case "*":
                            stack.Push(a && b);
                            break;
                    }
                }                  
            }
            return stack.Pop();
            
        }
        public static void tabl(string c, out bool[,] mas)
        {
            int j;
            mas = new bool[(int)Math.Pow(2, dict.Count), dict.Count + 1];
            var keys = dict.Keys.ToArray();
            for(int i = 0; i < Math.Pow(2, dict.Count); i++)
            {
                j = 0;

                foreach(var key in keys)
                {
                    dict[key] = Convert.ToBoolean(i >> j & 1);
                    mas[i, j] = dict[key];
                    j++;

                }
                mas[i, j] = EvalPostfix(c);
            }
            
        }
    }
}
