using System;

namespace StringCalculator
{
    internal static class Program
    {
        // словарь функций для каждого оператора
        // приоритет операторов зависит от их расположения в словаре
        static Dictionary<string, Func<string, string, string>> operators = new()
        {
            {"^", (a, b) => Math.Pow(double.Parse(a),double.Parse(b)).ToString() },
            {"*", (a, b) => (double.Parse(a) * double.Parse(b)).ToString() },
            {"/", (a, b) => (double.Parse(a) / double.Parse(b)).ToString() },
            {"+", (a, b) => (double.Parse(a) + double.Parse(b)).ToString() },
            {"-", (a, b) => (double.Parse(a) - double.Parse(b)).ToString() }
        };

        static void Main(string[] args) 
        {
            if (args.Length != 0)
            {
                Console.WriteLine(Calculator(string.Join(string.Empty, args)));
            }
            else
            {
                string input;
                while (true)
                {
                    Console.Write(">");
                    input = Console.ReadLine();
                    if (input == "exit" || string.IsNullOrEmpty(input)) break;
                    else
                    {
                        string example = string.Join(string.Empty, input.Split(' '));
              
                        try
                        {
                            int left = 0, rigth = 0;
                            foreach (char item in example)              // проверка на всякое
                            {
                                if (char.IsLetter(item)) throw new ArithmeticException();
                                if (item == '(') left++;
                                if (item == ')') rigth++;
                            }
                            if (left != rigth) throw new ArithmeticException();
                            Console.WriteLine(Calculator(example));
                        }
                        catch
                        {
                            Console.WriteLine("Error!");
                        }
                    }
                }
            }
        }

        static string Calculator(string exp)
        {
            int first, last;
            string result;
            while ((first = exp.LastIndexOf("(")) != -1)            // достает выражения из скобок 
            {                                                       // и вызывает для них Calculator
                last = exp.IndexOf(")", first);
                result = Calculator(exp.Substring(first + 1, last - first - 1));
                exp = exp.Remove(first, last - first + 1);          // затем вставляет результат 
                exp = exp.Insert(first, result);                    // на место выражения в скобках
            }                                       

            List<string> tokens = new();

            last = 0;
            for (int i = 1; i < exp.Length; i++)                    // разбивает строку на элементы
            {
                if (char.IsDigit(exp[i]) || exp[i] == ',') continue;
                else
                {
                    if (exp[i] == '-' && !char.IsDigit(exp[i - 1])) continue;
                    tokens.Add(exp.Substring(last, i - last));
                    tokens.Add(exp[i].ToString());
                    last = i + 1;
                }
            }
            tokens.Add(exp.Substring(last));

            foreach (string oper in operators.Keys)                 // перебирает строку на наличие
            {                                                       // операторов, каждый оператор по очереди
                for (int i = 1; i < tokens.Count; i++)
                {
                    if (tokens[i] == oper)                          
                    {
                        tokens.Insert(i - 1, operators[oper](tokens[i - 1], tokens[i + 1]));
                        tokens.RemoveRange(i, 3);
                        i = 0;                                      // если нашел применяет оператор
                    }                                               // к двум соседним элемнтам и
                }                                                   // помещяет результат на место левого
            }
            return tokens[0];
        }
    }  
}