using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusionCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void calculate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("Calculate");

            if (armor1element1.Text == "")
            {
                showError("You must select an element 1 for armor 1");
            }
            else if (armor1element2.Text == "")
            {
                showError("You must select an element 2 for armor 1 (select none for mono-element)");
            }
            else if (armor2element1.Text == "")
            {
                showError("You must select an element 1 for armor 2");
            }
            else if (armor2element2.Text == "")
            {
                showError("You must select an element 2 for armor 2 (select none for mono-element)");
            }
            else if (armor1element1.Text == armor1element2.Text)
            {
                showError("Armor 1 cannot have 2 of the same element.");
            }
            else if (armor2element1.Text == armor2element2.Text)
            {
                showError("Armor 2 cannot have 2 of the same element.");
            }
        }

        private void showError(string theError)
        {
            MessageBox.Show(theError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
    }
}
