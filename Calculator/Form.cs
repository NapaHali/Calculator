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

/// <para>ErrorCode enumerator</para>
/// <list type="=bullet">
/// <item>Success</item>
/// <item>SyntaxError</item>
/// <item>InvalidOperation</item>
/// <item>DivideByZeroError</item>
/// <item>InvalidOutputFormula</item>
/// <item>InvalidRootY</item>
/// <item>InvalidPowerY</item>
/// <item>InvalidFactorial</item>
/// <item>MathError</item>
/// <item>OverflowError</item>
/// <item>InternalError</item>
/// </list>
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

/// <para>CalculatorState enumerator</para>
/// <list type="=bullet">
/// <item>Normal</item>
/// <item>AbsValue</item>
/// <item>Root</item>
/// <item>Power</item>
/// </list>
enum CalculatorState
{
    Normal,
    AbsValue,
    Root,
    Power
}

/// <summary>
/// Main namespace of Calculator
/// </summary>
namespace Calculator
{

    /// <summary>
    /// Calculator class that inherits from Form class
    /// </summary>
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

        /// <summary>
        /// Handles error messages and prints them to input field
        /// </summary>
        /// <param name="code">ErrorCode value</param>
        /// <returns>Error translated to human language</returns>
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

        /// <summary>
        /// Constructor that initializes all GUI components
        /// </summary>
        public Calculator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event called when all components have been successfully loaded
        /// </summary>
        /// <param name="sender">Object that causes this event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void Calculator_Load(object sender, EventArgs e)
        {
            this.textBox_Result.AutoSize = false;
            btnPoint.Text = parser.decimalSeparator.ToString();
        }

        /// <summary>
        /// OnKeyDown event that occurs when a key is pressed
        /// </summary>
        /// <param name="sender">Object that causes this event to fire</param>
        /// <param name="e">Key event argument data passed to this function</param>
        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("KeyValue: " + e.KeyValue.ToString() + " KeyCode: " + e.KeyCode.ToString() + " KeyData: " + e.KeyData.ToString());
            // Numpad buttons
            if (e.KeyCode == Keys.NumPad0 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D0))
            {
                btn0.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad1 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D1))
            {
                btn1.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad2 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D2))
            {
                btn2.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad3 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D3))
            {
                btn3.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad4 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D4))
            {
                btn4.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad5 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D5))
            {
                btn5.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad6 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D6))
            {
                btn6.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad7 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D7))
            {
                btn7.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad8 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D8))
            {
                btn8.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad9 || (ModifierKeys == Keys.Shift && e.KeyCode == Keys.D9))
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
            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.Decimal)
            {
                btnPoint.PerformClick();
            }

            // Alphabetical keystrokes
            if(e.KeyCode == Keys.R)
            {
                btnRoot.PerformClick();
            }
            if(e.KeyCode == Keys.P)
            {
                btnPower.PerformClick();
            }
            if(e.KeyCode == Keys.A)
            {
                btnAbs.PerformClick();
            }
            if(e.KeyCode == Keys.C)
            {
                btnClear.PerformClick();
            }
            if(e.KeyCode == Keys.H)
            {
                btnHelp.PerformClick();
            }

            // Unspecified buttons
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                btnEquals.PerformClick();
            }
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                btnDelete.PerformClick();
            }
        }

        /// <summary>
        /// Button element used to open HelpForm
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (!helpPageOpen)    //checking if the help window is open
            {
                helpPageOpen = true;
                HelpForm formHelp = new HelpForm();
                formHelp.Show();

            }
        }

        /// <summary>
        /// Button element that erases last inputted character
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element which clears the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_Result.Clear();
            textBox_Result.Text = "0";
            calcState = CalculatorState.Normal;
            pointAllowed = true;
            errorDisplay = false;
        }

        /// <summary>
        /// Button element that adds the factorial operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the root operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the power operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the division operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the multiplication operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the absolute value operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds the difference operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnMinus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                textBox_Result.Text += "-";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        /// <summary>
        /// Button element that adds the sum operator into the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnPlus_Click(object sender, EventArgs e)
        {
            if(textBox_Result.Text.Length < maximalInput && !errorDisplay && calcState != CalculatorState.AbsValue)
            {
                textBox_Result.Text += "+";
                pointAllowedPrevious = pointAllowed;
                pointAllowed = true;
            }
            
        }

        /// <summary>
        /// Button element that adds the floating point to the number
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that sends the formula into the formulaparser class which evaluates the equation <see cref="FormulaParser"/>
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 0 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 1 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 2 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 3 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 4 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 5 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 6 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 7 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 8 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that adds 9 to the input field
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// Button element that exits the application
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Button element that minimizes the application
        /// </summary>
        /// <param name="sender">Object that caused the click event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// OnMouseDown event used to determine when the mouse is being pressed down
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Mouse event argument data passed to this function</param>
        private void Calculator_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.X < 484 && e.Y < 41)
            {
                dragging = true;
                startPoint = new Point(e.X, e.Y);
            }
            
        }

        /// <summary>
        /// OnMouseUp event used to determine when the mouse is not being pressed down
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Mouse event argument data passed to this function</param>
        private void Calculator_MoveUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// OnMouseMove event used to determine where should the form move based on the mouse pointer location
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Mouse event argument data passed to this function</param>
        private void Calculator_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        /// <summary>
        /// Event that is fired everytime the textBox_Result is changed
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
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

        /// <summary>
        /// IGNORE - not used
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Event argument data passed to this function</param>
        private void textBox_History_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
