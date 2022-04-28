using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

enum ErrorCode
{
    Success,
    SyntaxError,
    InvalidOperation,
    DivideByZeroError,
    InvalidOutputFormula
}
namespace Calculator
{
    public partial class Calculator : Form
    {
        private bool errorDisplay = false;
        private bool pointAllowed = true;
        private bool pointAllowedPrevious = true;
        //private bool maximizeWindow = false;          // bool for maximizing and "demaximizing" calculator window
        private bool dragging = false;                // bool for dragging
        private Point startPoint = new Point(0, 0);   // starting position of calculator window to make it draggable
        private char[] priorityOrder = new char[] { '÷', '*', '-', '+' };
        float textFontChange = 36;                    // variable for changing font size in textBox_Result
        const int maxFontSize = 36;                         // original size of textBox_Result text size
        const int minFontSize = 10;                         // minimal size for text size in textBox_Result
        const int maximalInput = 32;                        // digits of biggest number that can be inputted
        private bool isNumeric(char ch)
        {
            return int.TryParse(ch.ToString(), out _);
        }

        private bool isLastNumeric(string text)
        {
            if (text.Length == 0) return false;
            return int.TryParse(text[text.Length - 1].ToString(), out _);
        }

        private bool lastEquals(string text, char ch)
        {
            return text[text.Length -1] == ch;
        }

        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
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
            }
            return ErrorCode.InvalidOperation;
        }

        private string ErrorMessage(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.SyntaxError:
                    return "Syntax error.";
                case ErrorCode.DivideByZeroError:
                    return "Can't divide by zero.";
                default:
                    return "Internal error.";
            }
        }

        private ErrorCode ParseFormula(string formula, out double result)
        {
            result = 0;
            int negativeSwitch = 1;

            //Console.WriteLine($"Before: {formula}");
            if (!isLastNumeric(formula))
                return ErrorCode.SyntaxError;

            while (formula.Contains('+') || formula.Contains('-') || formula.Contains('*') || formula.Contains('÷'))
            {
                if (formula[0] == '-')
                {
                    negativeSwitch *= -1;
                    for (int i = 0; i < formula.Length; i++)
                    {
                        if (formula[i] == '+')
                        {
                            if (i - 1 >= 0 && formula[i - 1] == 'E')
                            {
                                continue;
                            }
                            formula = formula.Remove(i, 1);
                            formula = formula.Insert(i, "-");
                        }
                        else if (formula[i] == '-')
                        {
                            if (i - 1 >= 0 && formula[i - 1] == 'E')
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

                if (!(formula.Contains('+') || formula.Contains('-') || formula.Contains('*') || formula.Contains('÷')))
                {
                    break;
                }

                int priorityOperationIndex = -1;

                for (int i = 0; i < formula.Length; i++)
                {
                    if (!isNumeric(formula[i]) && formula[i] != '.' && formula[i] != 'E')
                    {
                        if (i-1 >= 0 && formula[i-1] == 'E')
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
                    else
                    {
                        continue;
                    }
                }

                if (priorityOperationIndex == -1)
                {
                    break;
                }

                string left = "", right = "";
                int expressionIndexLeft = priorityOperationIndex, expressionIndexRight = priorityOperationIndex;

                for (int i = priorityOperationIndex - 1; i >= 0 && (isNumeric(formula[i]) || formula[i] == '.' || formula[i] == 'E'); i--)
                {
                    left = left.Insert(0, formula[i].ToString());
                    expressionIndexLeft = i;
                }
                for (int i = priorityOperationIndex + 1; i < formula.Length && ((isNumeric(formula[i]) || formula[i] == '.' || formula[i] == 'E') || (i == priorityOperationIndex + 1 && !isNumeric(formula[i]) && formula[i] != '*' && formula[i] != '÷')); i++)
                {
                    right += formula[i].ToString();
                    expressionIndexRight = i;
                }

                Console.WriteLine($"Operation: {formula[priorityOperationIndex]}");
                Console.WriteLine($"Text: {formula}");
                Console.WriteLine($"Left: {left}");
                Console.WriteLine($"Right: {right}");

                // TODO: Globalized decimal point
                double x; double y;
                if (!double.TryParse(left, out x)) return ErrorCode.SyntaxError;
                if (!double.TryParse(right, out y)) return ErrorCode.SyntaxError;

                x = Math.Round(x, 8);
                y = Math.Round(y, 8);

                double operationResult = 0;
                ErrorCode opError = PerformOperation(formula[priorityOperationIndex], out operationResult, x, y);
                if (opError != ErrorCode.Success)
                {
                    return opError;
                }
                formula = formula.Replace(formula.Substring(expressionIndexLeft, expressionIndexRight - expressionIndexLeft + 1), operationResult.ToString());
                formula = formula.Replace("+-", "-");
                formula = formula.Replace("--", "+");
            }

            Console.WriteLine($"FinalText: {formula}");


            if (!double.TryParse(formula, out result))
            {
                return ErrorCode.InvalidOutputFormula;
            }

            result *= negativeSwitch;
            if (result.ToString().Contains("E"))
            {
                result = Math.Round(result, 2);
            } else
            {
                result = Math.Round(result, 10);
            }

            return ErrorCode.Success;
        }

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            this.textBox_Result.AutoSize = false;
        }

        //reading numbers from keyboard
        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyValue: " + e.KeyValue.ToString() + " KeyCode: " + e.KeyCode.ToString() + " KeyData: " + e.KeyData.ToString());
            // Numpad buttons
            if (e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.D0)
            {
                btn0.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1)
            {
                btn1.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2)
            {
                btn2.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3)
            {
                btn3.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4)
            {
                btn4.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5)
            {
                btn5.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6)
            {
                btn6.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.D7)
            {
                btn7.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.D8)
            {
                btn8.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad9 || e.KeyCode == Keys.D9)
            {
                btn9.PerformClick();
            }

            // Operations
            if (e.KeyCode == Keys.Add)
            {
                btnPlus.PerformClick();
            }
            if (e.KeyCode == Keys.Subtract)
            {
                btnMinus.PerformClick();
            }
            if (e.KeyCode == Keys.Multiply)
            {
                btnMultiply.PerformClick();
            }
            if (e.KeyCode == Keys.Divide)
            {
                btnDivide.PerformClick();
            }
            if (ModifierKeys == Keys.Shift && e.KeyCode == Keys.OemQuotes)
            {
                btnFactorial.PerformClick();
            }
            if (e.KeyCode == Keys.OemQuestion)
            {
                btnEquals.PerformClick();
            }
            if (e.KeyCode == Keys.OemPeriod)
            {
                btnPoint.PerformClick();
            }

            // Unspecified buttons
            if (e.KeyCode == Keys.Enter)
            {
                btnEquals.PerformClick();
            }
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                btnDelete.PerformClick();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Form formHelp = new HelpForm();
            formHelp.Show();


        }

        //erase last added number
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length > 1 && !errorDisplay)
            {
                if (textBox_Result.Text[textBox_Result.TextLength-1] == '.')
                {
                    pointAllowed = true;
                } else if (!isLastNumeric(textBox_Result.Text))
                {
                    pointAllowed = pointAllowedPrevious;
                }
                textBox_Result.Text = textBox_Result.Text.Substring(0, textBox_Result.Text.Length - 1);
            }
            else
            {
                textBox_Result.Text = "0";
            }
            errorDisplay = false;
        }

        //erase everything in textBox
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_Result.Clear();
            textBox_Result.Text = "0";
            errorDisplay = false;
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {

        }

        private void btnRoot_Click(object sender, EventArgs e)
        {

        }

        private void btnPower_Click(object sender, EventArgs e)
        {

        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                if (!isLastNumeric(textBox_Result.Text) && !lastEquals(textBox_Result.Text, '.'))
                {
                    textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
                }
                textBox_Result.Text += '÷';
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                if (!isLastNumeric(textBox_Result.Text) && !lastEquals(textBox_Result.Text, '.'))
                {
                    textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
                }
                textBox_Result.Text += "*";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        private void btnAbs_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                string text = textBox_Result.Text;
                if (!isLastNumeric(text))
                {
                    return;
                }
                int expressionLeft = text.Length - 1;
                for (int i = text.Length-1; i >= 0 && (isNumeric(text[i]) || text[i] == '.'); i--)
                {
                    expressionLeft = i;
                }

                text = text.Insert(expressionLeft, "|");
                text = text.Insert(text.Length, "|");
                textBox_Result.Text = text;
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                textBox_Result.Text += "-";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                textBox_Result.Text += "+";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing dot to text field, will print it only once
                if (isLastNumeric(textBox_Result.Text) && pointAllowed)
                {
                    textBox_Result.Text += ".";
                    pointAllowed = false;
                }
            }
            

        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (errorDisplay) return;
            pointAllowed = true;
            double result;
            ErrorCode parseError = ParseFormula(textBox_Result.Text, out result);
            if (parseError != ErrorCode.Success)
            {
                textBox_Result.Text = ErrorMessage(parseError);
                errorDisplay = true;
            } else
            { 
                textBox_History.Text = textBox_Result.Text;
                textBox_Result.Text = result.ToString();
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "0";
                }
                else
                {
                    textBox_Result.Text += "0";
                }
            }
            
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "1";
                }
                else
                {
                    textBox_Result.Text += "1";
                }
            }
                
            
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "2";
                }
                else
                {
                    textBox_Result.Text += "2";
                }
            }
            
        }
        private void btn3_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "3";
                }
                else
                {
                    textBox_Result.Text += "3";
                }
            }
            
        }
        private void btn4_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "4";
                }
                else
                {
                    textBox_Result.Text += "4";
                }
            }
            
        }
        private void btn5_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "5";
                }
                else
                {
                    textBox_Result.Text += "5";
                }
            }
            
        }
        private void btn6_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "6";
                }
                else
                {
                    textBox_Result.Text += "6";
                }
            }
            
        }
        private void btn7_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "7";
                }
                else
                {
                    textBox_Result.Text += "7";
                }
            }
            
        }
        private void btn8_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "8";
                }
                else
                {
                    textBox_Result.Text += "8";
                }
            }
            
        }
        private void btn9_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay)
            {
                //printing number to text field
                if (textBox_Result.Text == "0")
                {
                    textBox_Result.Text = "9";
                }
                else
                {
                    textBox_Result.Text += "9";
                }

            }
            
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            //maximizing and getting it back to it's original size
        //    switch (maximizeWindow)
        //    {
        //        case false:
        //            WindowState = FormWindowState.Maximized;
        //            maximizeWindow = true;
        //           break;
        //
        //        case true:
        //            WindowState = FormWindowState.Normal;
        //            maximizeWindow = false;
        //            break;
        //        
        //    }
        }

        //when the left mouse button is pressed down on specific places, the object is draggable
        //startPoint- position the calculator window on the monitor 
        private void Calculator_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        //releasing the left mouse button mean end of dragging
        private void Calculator_MoveUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        //Location make the window follow the mouse on monitor
        private void Calculator_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        
        private void textBox_Result_TextChanged(object sender, EventArgs e)
        {
            //changing font size automaticly, based on text width
            textBox_Result.Font = new Font(textBox_Result.Font.Name, textFontChange);

            Size s = TextRenderer.MeasureText(textBox_Result.Text, textBox_Result.Font);
            if (s.Width >= ((textBox_Result.Width) -textFontChange) && textFontChange > minFontSize) 
            {
                textFontChange -= 1;        //fluently decrasing font size
            }

            if (s.Width < ((textBox_Result.Width) - textFontChange)  && textFontChange< maxFontSize)
            {
                textFontChange += 1;        //fluently increasing font size
            }

        }

        private void textBox_History_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
