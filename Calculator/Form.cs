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
        private bool maximizeWindow = false;          //bool for maximizing and "demaximizing" calculator window
        private bool dragging = false;                  //bool for dragging
        private Point startPoint = new Point(0, 0);     //starting position of calculator window to make it draggable
        public Calculator()
        {
            InitializeComponent();
        }

        private void Clcltr_Load(object sender, EventArgs e)
        {

        }

        //reading numbers from keyboard
        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad0)
            {
                b0.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad1)
            {
                b1.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad2)
            {
                b2.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad3)
            {
                b3.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                b4.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad5)
            {
                b5.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                b6.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad7)
            {
                b7.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad8)
            {
                b8.PerformClick();
            }
            if (e.KeyCode == Keys.NumPad9)
            {
                b9.PerformClick();
            }
            if (e.KeyCode == Keys.Add)
            {
                //Console.WriteLine("key + was pressed");
                plus.PerformClick();
            }
            if (e.KeyCode == Keys.Subtract)
            {
                //Console.WriteLine("key - was pressed");
                minus.PerformClick();
            }
            if (e.KeyCode == Keys.Multiply)
            {
                //Console.WriteLine("key * was pressed");
                multiply.PerformClick();
            }
            if (e.KeyCode == Keys.Divide)
            {
                //Console.WriteLine("key / was pressed");
                divide.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                //Console.WriteLine("key ENTER was pressed");     //NOT WORKING: vs ukazuje že sa niečo deje ale conzola neukazuje nič
                equals.PerformClick();
            }
            if (e.KeyCode == Keys.Delete)
            {
                //Console.WriteLine("key DELETE was pressed");
                delete.PerformClick();
            }
            // !,=,desatina,
        }

        private void help_Click(object sender, EventArgs e)
        {

        }

        //erase last added number
        private void delete_Click(object sender, EventArgs e)
        {
            textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
        }

        //erase everything in textBox
        private void clear_Click(object sender, EventArgs e)
        {
            textBox.Clear();
        }
        private void factorial_Click(object sender, EventArgs e)
        {
    
        }
        private void root_Click(object sender, EventArgs e)
        {

        }
        private void power_Click(object sender, EventArgs e)
        {

        }
        private void divide_Click(object sender, EventArgs e)
        {

        }
        private void multiply_Click(object sender, EventArgs e)
        {

        }
        private void abs_Click(object sender, EventArgs e)
        {

        }
        private void minus_Click(object sender, EventArgs e)
        {

        }
        private void plus_Click(object sender, EventArgs e)
        {

        }
        private void dot_Click(object sender, EventArgs e)
        {
            //printing dot to text field, will print it only once
            if (!textBox.Text.Contains("."))
            {
                textBox.Text += "."; 
            }
        }
        private void equals_Click(object sender, EventArgs e)
        {

        }

        private void b0_Click(object sender, EventArgs e)
        {   
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "0";
            }
            else
            {
                textBox.Text += "0";
            }
        }
        private void b1_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "1";
            }
            else
            {
                textBox.Text += "1";
            }
        }
       
        private void b2_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "2";
            }
            else
            {
                textBox.Text += "2";
            }
        }
        private void b3_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "3";
            }
            else
            {
                textBox.Text += "3";
            }
        }
        private void b4_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "4";
            }
            else
            {
                textBox.Text += "4";
            }
        }
        private void b5_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "5";
            }
            else
            {
                textBox.Text += "5";
            }
        }
        private void b6_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "6";
            }
            else
            {
                textBox.Text += "6";
            }
        }
        private void b7_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "7";
            }
            else
            {
                textBox.Text += "7";
            }
        }
        private void b8_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "8";
            }
            else
            {
                textBox.Text += "8";
            }
        }
        private void b9_Click(object sender, EventArgs e)
        {
            //printing number to text field
            if (textBox.Text == "0")
            {
                textBox.Text = "9";
            }
            else
            {
                textBox.Text += "9";
            }
        }
        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void maximize_Click(object sender, EventArgs e)
        {
            //maximizing and getting it back to it's original size
            switch (maximizeWindow)
            {
                case false:
                
                    this.WindowState = FormWindowState.Maximized;
                    maximizeWindow = true;
                    break;

                case true:
                
                    this.WindowState = FormWindowState.Normal;
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
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }
        
    }
}
