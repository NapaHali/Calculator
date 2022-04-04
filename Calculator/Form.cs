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
        private bool _dragging = false;
        private Point _offset;
        private Point _start_point = new Point(0, 0);
        public Calculator()
        {
            InitializeComponent();
        }

        private void Kalkulačka_Load(object sender, EventArgs e)
        {

        }
        private void help_Click(object sender, EventArgs e)
        {

        }
        private void delete_Click(object sender, EventArgs e)
        {

        }
        private void clear_Click(object sender, EventArgs e)
        {

        }
        private void factorial_Click(object sender, EventArgs e)
        {

        }
        private void square_root_Click(object sender, EventArgs e)
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
        private void absolut_Click(object sender, EventArgs e)
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

        }
        private void equals_Click(object sender, EventArgs e)
        {

        }
        private void b_0_Click(object sender, EventArgs e)
        {

        }
        private void b_1_Click(object sender, EventArgs e)
        {

        }
        private void b_2_Click(object sender, EventArgs e)
        {

        }
        private void b_3_Click(object sender, EventArgs e)
        {

        }
        private void b_4_Click(object sender, EventArgs e)
        {

        }
        private void b_5_Click(object sender, EventArgs e)
        {

        }
        private void b_6_Click(object sender, EventArgs e)
        {

        }
        private void b_7_Click(object sender, EventArgs e)
        {

        }
        private void b_8_Click(object sender, EventArgs e)
        {

        }
        private void b_9_Click(object sender, EventArgs e)
        {

        }
        private void exit_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void maximize_b_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        
        private void Calculator_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _start_point = new Point(e.X, e.Y);
        }

        private void Calculator_MoveUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void Calculator_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this._start_point.X, p.Y - this._start_point.Y);
            }
        }
    }
}
