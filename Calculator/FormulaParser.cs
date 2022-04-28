using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Calculator
{
    internal class FormulaParser
    {
        public char decimalSeparator { get; private set; }
        private char[] priorityOrder = new char[] { '÷', '*', '-', '+' };

        public FormulaParser()
        {
            this.decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        }

        private ErrorCode PerformOperation(char opchar, out double result, double x, double y = 0)
        {
            result = 0;
            switch (opchar)
            {
                case '÷':
                    try
                    {
                        result = MathLib.Divide(x, y);
                    }
                    catch (DivideByZeroException)
                    {
                        return ErrorCode.DivideByZeroError;
                    }
                    return ErrorCode.Success;
                case '*':
                    result = MathLib.Multiply(x, y);
                    return ErrorCode.Success;
                case '+':
                    result = MathLib.Add(x, y);
                    return ErrorCode.Success;
                case '-':
                    result = MathLib.Substract(x, y);
                    return ErrorCode.Success;
                case '!':
                    try
                    {
                        result = (double)MathLib.Factorial(Convert.ToInt32(x));
                        if (double.IsInfinity(result)) return ErrorCode.OverflowError;
                    }
                    catch
                    {
                        return ErrorCode.InternalError;
                    }
                    return ErrorCode.Success;
            }
            return ErrorCode.InvalidOperation;
        }

        public ErrorCode Parse(string formula, out double result)
        {
            result = 0;
            int negativeSwitch = 1;

            if (!StringOperations.isLastNumeric(formula) && !StringOperations.lastEquals(formula, '!'))
                return ErrorCode.SyntaxError;

            while (formula.Contains('+') || formula.Contains('-') || formula.Contains('*') || formula.Contains('÷') || formula.Contains('!'))
            {
                if (formula[0] == '-')
                {
                    negativeSwitch *= -1;
                    for (int i = 0; i < formula.Length; i++)
                    {
                        if (formula[i] == '+')
                        {
                            if (i - 1 >= 0 && (formula[i - 1] == 'E' || formula[i-1] == '*' || formula[i-1] == '÷'))
                            {
                                continue;
                            }
                            formula = formula.Remove(i, 1);
                            formula = formula.Insert(i, "-");
                        }
                        else if (formula[i] == '-')
                        {
                            if (i - 1 >= 0 && (formula[i - 1] == 'E' || formula[i - 1] == '*' || formula[i - 1] == '÷'))
                            {
                                continue;
                            }
                            formula = formula.Remove(i, 1);
                            formula = formula.Insert(i, "+");
                        }
                    }
                }

                if (formula[0] == '+')
                {
                    formula = formula.Remove(0, 1);
                }

                if (!(formula.Contains('+') || formula.Contains('-') || formula.Contains('*') || formula.Contains('÷') || formula.Contains('!')))
                {
                    break;
                }

                int priorityOperationIndex = -1;

                for (int i = 0; i < formula.Length; i++)
                {
                    if (formula[i] == '!')
                    {
                        priorityOperationIndex = i;
                        break;
                    }
                    if (!StringOperations.isNumeric(formula[i]) && formula[i] != decimalSeparator && formula[i] != 'E')
                    {
                        if (i - 1 >= 0 && formula[i - 1] == 'E')
                        {
                            continue;
                        }
                        if (priorityOperationIndex == -1)
                        {
                            priorityOperationIndex = i;
                        }
                        else
                        {
                            if (Array.IndexOf(priorityOrder, formula[i]) < Array.IndexOf(priorityOrder, formula[priorityOperationIndex]))
                            {
                                priorityOperationIndex = i;
                            }
                        }
                    }
                }

                if (priorityOperationIndex == -1)
                {
                    break;
                }

                string left = "", right = "";
                int expressionIndexLeft = priorityOperationIndex, expressionIndexRight = priorityOperationIndex;

                for (int i = priorityOperationIndex - 1; i >= 0 && (StringOperations.isNumeric(formula[i]) || formula[i] == decimalSeparator || formula[i] == 'E') || (i - 1 >= 0 && formula[i - 1] == 'E'); i--)
                {
                    left = left.Insert(0, formula[i].ToString());
                    expressionIndexLeft = i;
                }
                for (int i = priorityOperationIndex + 1; i < formula.Length && ((StringOperations.isNumeric(formula[i]) || formula[i] == decimalSeparator || formula[i] == 'E') || (i == priorityOperationIndex + 1 && !StringOperations.isNumeric(formula[i]) && formula[i] != '*' && formula[i] != '÷')); i++)
                {
                    right += formula[i].ToString();
                    expressionIndexRight = i;
                }

                // TODO: Globalized decimal point
                double x = 0; double y = 0;
                if (!double.TryParse(left, out x)) return ErrorCode.SyntaxError;
                if (formula[priorityOperationIndex] != '!')
                {
                    if (!double.TryParse(right, out y)) return ErrorCode.SyntaxError;
                } else
                {
                    if (x % 1 != 0 || x == 0) return ErrorCode.InvalidFactorial; // Check if 'x' is whole number
                }

                x = Math.Round(x, 8);
                y = Math.Round(y, 8);

                double operationResult = 0;
                ErrorCode opError = PerformOperation(formula[priorityOperationIndex], out operationResult, x, y);
                if (opError != ErrorCode.Success)
                {
                    return opError;
                }
                if (formula[priorityOperationIndex] == '!')
                {
                    formula = formula.Replace(formula.Substring(expressionIndexLeft, priorityOperationIndex - expressionIndexLeft + 1), operationResult.ToString());
                } else
                {
                    formula = formula.Replace(formula.Substring(expressionIndexLeft, expressionIndexRight - expressionIndexLeft + 1), operationResult.ToString());
                }
                formula = formula.Replace("+-", "-");
                formula = formula.Replace("--", "+");
            }

            if (!double.TryParse(formula, out result))
            {
                return ErrorCode.InvalidOutputFormula;
            }

            result *= negativeSwitch;
            if (result.ToString().Contains("E"))
            {
                result = Math.Round(result, 3);
            }
            else
            {
                result = Math.Round(result, 10);
            }

            return ErrorCode.Success;
        }
    }
}
