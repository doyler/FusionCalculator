using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusionCalculator
{
    public partial class Form1 : Form
    {
        //private
        public Form1()
        {
            InitializeComponent();
        }

        private void calculate_Click(object sender, EventArgs e)
        {
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
            else if (armor2element1.Text != "None" && (armor2element1.Text == armor1element1.Text || armor2element1.Text == armor1element2.Text))
            {
                showError("Armor 2 cannot contain " + armor2element1.Text + " as Armor 1 already contains it.");
            }
            else if (armor2element2.Text != "None" && (armor2element2.Text == armor1element1.Text || armor2element2.Text == armor1element2.Text))
            {
                showError("Armor 2 cannot contain " + armor2element2.Text + " as Armor 1 already contains it.");
            }

            outcomes.Text = "";

            outcomes.Text += armor1element1.Text;
            outcomes.Text += "\r\n";
            outcomes.Text += armor2element1.Text;
            outcomes.Text += "\r\n";
            outcomes.Text += armor1element1.Text + "/" + armor2element1.Text;
            outcomes.Text += "\r\n";
            if (armor1element2.Text != "None")
            {
                outcomes.Text += armor1element2.Text;
                outcomes.Text += "\r\n";
                outcomes.Text += armor1element2.Text + "/" + armor2element1.Text;
                outcomes.Text += "\r\n";
                if (armor2element2.Text != "None")
                {
                    outcomes.Text += armor1element2.Text + "/" + armor2element2.Text;
                    outcomes.Text += "\r\n";
                }
            }
            if (armor2element2.Text != "None")
            {
                outcomes.Text += armor2element2.Text;
                outcomes.Text += "\r\n";
                outcomes.Text += armor2element2.Text + "/" + armor1element1.Text;
                outcomes.Text += "\r\n";
            }

            int minStars = Math.Min(Convert.ToInt32(armor1stars.Text), Convert.ToInt32(armor2stars.Text)) - 2;
            if (minStars < 1) { minStars = 1; }
            int maxStars = Math.Max(Convert.ToInt32(armor1stars.Text), Convert.ToInt32(armor2stars.Text)) + 2;
            if (maxStars > 5) { maxStars = 5; }

            outcomes.Text += "\r\nPossible star values: " + minStars;

            for (int i = minStars + 1; i <= maxStars; i++)
            {
                outcomes.Text += ", " + i;
            }

            DataTable myTable = CsvToDataTable("armors.csv");

            foreach (DataRow row in myTable.Rows)
            {
                string name = row["Name"].ToString();
                string rarity = row["Rarity"].ToString();
                int combinedStats = (int)row["Attack"] + (int)row["Defense"];

                outcomes.Text += "\r\n" + name + " : " + rarity + " : " + combinedStats;
            }

        }

        private DataTable CsvToDataTable(string strFileName)
        {
            DataTable dataTable = new DataTable("DataTable Name");

            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + Directory.GetCurrentDirectory() + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\""))
            {
                conn.Open();
                string strQuery = "SELECT * FROM [" + strFileName + "]";
                OleDbDataAdapter adapter =
                    new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        private void showError(string theError)
        {
            MessageBox.Show(theError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }
    }
}
