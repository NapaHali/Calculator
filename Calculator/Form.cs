using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        private bool maximizeWindow = false;          // bool for maximizing and "demaximizing" calculator window
        private bool dragging = false;                // bool for dragging
        private Point startPoint = new Point(0, 0);   // starting position of calculator window to make it draggable
        private char[] priorityOrder = new char[] { '÷', '*', '+', '-' };

        private bool isNumeric(char ch)
        {
            return int.TryParse(ch.ToString(), out _);
        }

        private bool isLastNumeric(string text)
        {
            if (text.Length == 0) return false;
            return int.TryParse(text[text.Length - 1].ToString(), out _);
        }

        private double PerformOperation(char opchar, double x, double y = 0)
        {
            switch (opchar)
            {
                case '÷':
                    return MathLib.Divide(x, y);
                case '*':
                    return MathLib.Multiply(x, y);
                case '+':
                    return MathLib.Add(x, y);
                case '-':
                    return MathLib.Substract(x, y);
            }
            throw new Exception("Invalid operator character specified.");
        }

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {

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
            if (e.KeyCode == Keys.Return)
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

        }

        //erase last added number
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (textBox_Result.Text.Length > 1)
            {
                textBox_Result.Text = textBox_Result.Text.Substring(0, textBox_Result.Text.Length - 1);
            }
            else
            {
                textBox_Result.Text = "0";
            }
        }

        //erase everything in textBox
        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_Result.Clear();
            textBox_Result.Text = "0";
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
            if (!isLastNumeric(textBox_Result.Text))
            {
                textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
            }
            textBox_Result.Text += "÷";
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            if (!isLastNumeric(textBox_Result.Text))
            {
                textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
            }
            textBox_Result.Text += "*";
        }

        private void btnAbs_Click(object sender, EventArgs e)
        {

        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (!isLastNumeric(textBox_Result.Text))
            {
                textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
            }
            textBox_Result.Text += "-";
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (!isLastNumeric(textBox_Result.Text))
            {
                textBox_Result.Text = textBox_Result.Text.Remove(textBox_Result.TextLength - 1);
            }
            textBox_Result.Text += "+";
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            //printing dot to text field, will print it only once
            if (!textBox_Result.Text.Contains("."))
            {
                textBox_Result.Text += ".";
            }
        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            string text = textBox_Result.Text;
            if (!isLastNumeric(text))
                return;

            int operatorCount = 0;

            foreach (char ch in text)
            {
                if (!isNumeric(ch) && ch != '.')
                {
                    operatorCount++;
                }
            }

            for (int u = 0; u < operatorCount; u++)
            {
                int priorityOperationIndex = -1;

                for (int i = 0; i < text.Length; i++)
                {
                    if (!isNumeric(text[i]) && text[i] != '.')
                    {
                        if (priorityOperationIndex == -1)
                        {
                            priorityOperationIndex = i;
                        }
                        else
                        {
                            if (Array.IndexOf(priorityOrder, text[i]) < Array.IndexOf(priorityOrder, text[priorityOperationIndex]))
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

                string left = "", right = "";
                int expressionIndexLeft = priorityOperationIndex, expressionIndexRight = priorityOperationIndex;

                for (int i = priorityOperationIndex - 1; i >= 0 && (isNumeric(text[i]) || text[i] == '.'); i--)
                {
                    left = left.Insert(0, text[i].ToString());
                    expressionIndexLeft = i;
                }
                for (int i = priorityOperationIndex + 1; i < text.Length && (isNumeric(text[i]) || text[i] == '.'); i++)
                {
                    right += text[i].ToString();
                    expressionIndexRight = i;
                }

                double x = double.Parse(left);
                double y = double.Parse(right);

                double result = PerformOperation(text[priorityOperationIndex], x, y);
                text = text.Replace(text.Substring(expressionIndexLeft, expressionIndexRight - expressionIndexLeft + 1), result.ToString());
            }
            textBox_History.Text = textBox_Result.Text;
            textBox_Result.Text = text;
        }

        private void btn0_Click(object sender, EventArgs e)
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
        private void btn1_Click(object sender, EventArgs e)
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

        private void btn2_Click(object sender, EventArgs e)
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
        private void btn3_Click(object sender, EventArgs e)
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
        private void btn4_Click(object sender, EventArgs e)
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
        private void btn5_Click(object sender, EventArgs e)
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
        private void btn6_Click(object sender, EventArgs e)
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
        private void btn7_Click(object sender, EventArgs e)
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
        private void btn8_Click(object sender, EventArgs e)
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
        private void btn9_Click(object sender, EventArgs e)
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
            switch (maximizeWindow)
            {
                case false:
                    WindowState = FormWindowState.Maximized;
                    maximizeWindow = true;
                    break;

                case true:
                    WindowState = FormWindowState.Normal;
                    maximizeWindow = false;
                    break;

            }
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

        }

        private void textBox_History_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
