using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Main namespace of Calculator
/// </summary>
namespace Calculator
{
    /// <summary>
    /// HelpForm class that inherits Form class
    /// </summary>
    public partial class HelpForm : Form
    {
        private bool dragging = false;                // bool for dragging
        private Point startPoint = new Point(0, 0);

        /// <summary>
        /// Constructor used to initialize all GUI elements, events and properties
        /// </summary>
        public HelpForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button element used to exit the form
        /// </summary>
        /// <param name="sender">Object that caused the click function</param>
        /// <param name="e">Event arguments data passed to this function</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Calculator.helpPageOpen = false;       //bool from Calculator form
            Dispose();
            Close();
        }

        /// <summary>
        /// OnMouseDown event used to determine when the mouse is being pressed down
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Mouse event argument data passed to this function</param>
        private void Help_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < 779 && e.Y < 46)
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
        private void Help_MoveUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// OnMouseMove event used to determine where should the form move based on the mouse pointer location
        /// </summary>
        /// <param name="sender">Object that caused this event to fire</param>
        /// <param name="e">Mouse event argument data passed to this function</param>
        private void Help_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }
    }
}
