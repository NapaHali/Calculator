using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

enum ErrorCode
{
    Success,
    SyntaxError,
    InvalidOperation,
    DivideByZeroError,
    InvalidOutputFormula,
    InvalidRootY,
    InvalidPowerY,
    InvalidFactorial,
    MathError,
    OverflowError,
    InternalError
}
enum CalculatorState
{
    Normal,
    AbsValue,
    Root,
    Power
}
namespace Calculator
{
    public partial class Calculator : Form
    {
        private bool errorDisplay = false;
        private bool pointAllowed = true;
        private bool pointAllowedPrevious = true;
        private CalculatorState calcState = CalculatorState.Normal;
        private FormulaParser parser = new FormulaParser();
        private int funcEndIndex = 0;

        // GUI Variables
        //private bool maximizeWindow = false;          // bool for maximizing and "demaximizing" calculator window
        private bool dragging = false;                // bool for dragging
        private Point startPoint = new Point(0, 0);   // starting position of calculator window to make it draggable
        float textFontChange = 36;                    // variable for changing font size in textBox_Result
        const int maxFontSize = 36;                         // original size of textBox_Result text size
        const int minFontSize = 10;                         // minimal size for text size in textBox_Result
        const int maximalInput = 32;                        // digits of biggest number that can be inputted
        public static bool helpPageOpen= false;                     //bool for opening the help popup


        
        private string ErrorMessage(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.SyntaxError:
                    return "Syntax error.";
                case ErrorCode.MathError:
                    return "Math error.";
                case ErrorCode.OverflowError:
                    return "Overflow error.";
                case ErrorCode.DivideByZeroError:
                    return "Can't divide by zero.";
                case ErrorCode.InvalidRootY:
                    return "Root Y must be whole number.";
                case ErrorCode.InvalidPowerY:
                    return "Power Y must be whole number.";
                case ErrorCode.InvalidFactorial:
                    return "Invalid factorial.";
                default:
                    return "Internal error.";
            }
        }

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            this.textBox_Result.AutoSize = false;
            btnPoint.Text = parser.decimalSeparator.ToString();
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
            if (!helpPageOpen)    //checking if the help window is open
            {
                helpPageOpen = true;
                HelpForm formHelp = new HelpForm();
                formHelp.Show();

            }

        }

        //erase last added number
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length > 1 && !errorDisplay)
            {
                if ((calcState == CalculatorState.Root || calcState == CalculatorState.Power) && textBox_Result.Text.Length - 1 == funcEndIndex)
                {
                    return;
                }
                if (calcState == CalculatorState.AbsValue)
                {
                    textBox_Result.Text = textBox_Result.Text.Remove(0, 1);
                    textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.Text.Length - 1, 1);
                    calcState = CalculatorState.Normal;
                    return;
                }
                if (textBox_Result.Text[textBox_Result.TextLength-1] == parser.decimalSeparator)
                {
                    pointAllowed = true;
                } else if (!StringOperations.isLastNumeric(textBox_Result.Text))
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
            calcState = CalculatorState.Normal;
            pointAllowed = true;
            errorDisplay = false;
        }

        private void btnFactorial_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                if (!StringOperations.isLastNumeric(textBox_Result.Text) && !StringOperations.lastEquals(textBox_Result.Text, parser.decimalSeparator) && (calcState != CalculatorState.Normal && textBox_Result.Text.Length - 1 != funcEndIndex))
                {
                    textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
                }
                textBox_Result.Text += '!';
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
        }

        private void btnRoot_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput - 6 && !errorDisplay) // maximalInput - 6 to make space for root function parentheses
            {
                string text = textBox_Result.Text;
                if (!StringOperations.isLastNumeric(text) || calcState != CalculatorState.Normal)
                {
                    return;
                }
                text = text.Insert(0, "Root(");
                text = text.Insert(text.Length, ")");
                funcEndIndex = text.Length - 1;
                textBox_Result.Text = text;
                calcState = CalculatorState.Root;
            }
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput - 5 && !errorDisplay) // maximalInput - 5 to make space for power function parentheses
            {
                string text = textBox_Result.Text;
                if (!StringOperations.isLastNumeric(text) || calcState != CalculatorState.Normal)
                {
                    return;
                }
                text = text.Insert(0, "Pow(");
                text = text.Insert(text.Length, ")");
                funcEndIndex = text.Length - 1;
                textBox_Result.Text = text;
                calcState = CalculatorState.Power;
            }
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                if (!StringOperations.isLastNumeric(textBox_Result.Text) && !StringOperations.lastEquals(textBox_Result.Text, parser.decimalSeparator) && (calcState != CalculatorState.Normal && textBox_Result.Text.Length - 1 != funcEndIndex))
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                if (!StringOperations.isLastNumeric(textBox_Result.Text) && !StringOperations.lastEquals(textBox_Result.Text, parser.decimalSeparator) && (calcState != CalculatorState.Normal && textBox_Result.Text.Length - 1 != funcEndIndex))
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
            if (textBox_Result.Text.Length < maximalInput - 2 && !errorDisplay) // maximalInput - 2 to make space for the abs function parentheses
            {
                string text = textBox_Result.Text;
                if (!StringOperations.isLastNumeric(text) || calcState != CalculatorState.Normal)
                {
                    return;
                }
                text = text.Insert(0, "|");
                text = text.Insert(text.Length, "|");
                textBox_Result.Text = text;
                calcState = CalculatorState.AbsValue;
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                textBox_Result.Text += "-";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
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
                if (StringOperations.isLastNumeric(textBox_Result.Text) && pointAllowed)
                {
                    textBox_Result.Text += parser.decimalSeparator;
                    pointAllowed = false;
                }
            }
            

        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (errorDisplay) return;
            double result;
            double _resultFuncY;
            int resultFuncY = 0;
            ErrorCode parseError;
            ErrorCode parseErrorFuncY = ErrorCode.Success;
            switch(calcState)
            {
                case CalculatorState.AbsValue:
                    parseError = parser.Parse(textBox_Result.Text.Substring(1, textBox_Result.Text.Length - 2), out result); // ParseFormula using text without abs function parentheses
                    break;
                case CalculatorState.Root:
                    parseError = parser.Parse(textBox_Result.Text.Substring(5, funcEndIndex - 5), out result);
                    parseErrorFuncY = parser.Parse(textBox_Result.Text.Substring(funcEndIndex + 1, textBox_Result.Text.Length - funcEndIndex - 1), out _resultFuncY);
                    if (_resultFuncY % 1 != 0)
                    {
                        parseErrorFuncY = ErrorCode.InvalidRootY;
                        break;
                    }
                    resultFuncY = Convert.ToInt32(_resultFuncY);
                    break;
                case CalculatorState.Power:
                    parseError = parser.Parse(textBox_Result.Text.Substring(4, funcEndIndex - 4), out result);
                    parseErrorFuncY = parser.Parse(textBox_Result.Text.Substring(funcEndIndex + 1, textBox_Result.Text.Length - funcEndIndex - 1), out _resultFuncY);
                    if (_resultFuncY % 1 != 0)
                    {
                        parseErrorFuncY = ErrorCode.InvalidPowerY;
                        break;
                    }
                    resultFuncY = Convert.ToInt32(_resultFuncY);
                    break;
                default:
                    parseError = parser.Parse(textBox_Result.Text, out result);
                    break;
            }

            if (parseError != ErrorCode.Success)
            {
                textBox_History.Text = textBox_Result.Text;
                textBox_Result.Text = ErrorMessage(parseError);
                errorDisplay = true;
                pointAllowed = true;
            } else if (parseErrorFuncY != ErrorCode.Success)
            {
                textBox_History.Text = textBox_Result.Text;
                textBox_Result.Text = ErrorMessage(parseErrorFuncY);
                errorDisplay = true;
                pointAllowed = true;
            }
            else
            { 
                switch (calcState)
                {
                    case CalculatorState.AbsValue:
                        result = MathLib.Abs(result);
                        break;
                    case CalculatorState.Root:
                        result = MathLib.Root(result, resultFuncY);
                        break;
                    case CalculatorState.Power:
                        result = MathLib.Power(result, resultFuncY);
                        break;
                }

                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    textBox_History.Text = textBox_Result.Text;
                    textBox_Result.Text = ErrorMessage(ErrorCode.MathError);
                    errorDisplay = true;
                    pointAllowed = true;
                } else
                {
                    textBox_History.Text = textBox_Result.Text;
                    textBox_Result.Text = result.ToString();
                    if (!textBox_Result.Text.Contains('.'))
                    {
                        pointAllowed = true;
                    }
                }
                calcState = CalculatorState.Normal;
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!'))
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
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if (textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && !StringOperations.lastEquals(textBox_Result.Text, '!') && calcState != CalculatorState.AbsValue)
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

        //when the left mouse button is pressed down on specific places, the object is draggable
        //startPoint- position the calculator window on the monitor 
        private void Calculator_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.X < 484 && e.Y < 41)
            {
                dragging = true;
                startPoint = new Point(e.X, e.Y);
            }
            
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

            if (s.Width >= ((textBox_Result.Width) - maxFontSize) && textFontChange > minFontSize) 
            {
                textFontChange -= 2;        //fluently decreasing font size
            }

            if (s.Width < ((textBox_Result.Width) - maxFontSize)  && textFontChange < maxFontSize)
            {
                textFontChange += 2;        //fluently increasing font size
            }

        }

        private void textBox_History_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
