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
        private DataTable allArmors = new DataTable();
        private DataTable confirmedArmors = new DataTable();

        private int total = 0;
        private int epics = 0;
        private int monoLegends = 0;
        private int legends = 0;
        private int ultraRares = 0;
        private int superRares = 0;
        private int rares = 0;
        private int betterStats = 0;

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

            List<string> elements = new List<string>();
            elements.Add("Earth");
            elements.Add("Water");
            elements.Add("Fire");
            elements.Add("Air");
            elements.Add("Spirit");

            outcomes.Text = "";

            foreach (DataRow row in confirmedArmors.Rows)
            {
                string name = row["Name"].ToString();
                string combination = row["Combination"].ToString();
                int rarity = (int)row["Rarity"];
                int combinedStats = (int)row["Attack"] + (int)row["Defense"];

                int minStars = Math.Min(Convert.ToInt32(armor1stars.Text), Convert.ToInt32(armor2stars.Text)) - 2;
                if (minStars < 1) { minStars = 1; }
                int maxStars = Math.Max(Convert.ToInt32(armor1stars.Text), Convert.ToInt32(armor2stars.Text)) + 2;
                if (maxStars > 5) { maxStars = 5; }

                if (rarity < minStars || rarity > maxStars)
                {
                    continue;
                }

                //mono Armor 1 Element 1
                elements.Remove(armor1element1.Text);
                if (row[armor1element1.Text].ToString() == "Yes" && row[elements[0]].ToString() == "No" &&
                    row[elements[1]].ToString() == "No" && row[elements[2]].ToString() == "No"
                    && row[elements[3]].ToString() == "No")
                {
                    addArmor(name, combination, true, rarity, combinedStats);
                }
                elements.Add(armor1element1.Text);

                //mono Armor 2 Element 1
                elements.Remove(armor2element1.Text);
                if (row[armor2element1.Text].ToString() == "Yes" && row[elements[0]].ToString() == "No" &&
                    row[elements[1]].ToString() == "No" && row[elements[2]].ToString() == "No"
                    && row[elements[3]].ToString() == "No")
                {
                    addArmor(name, combination, true, rarity, combinedStats);
                }
                elements.Add(armor2element1.Text);

                //Armor 1 Element 1 + Armor 2 Element 1
                elements.Remove(armor1element1.Text);
                elements.Remove(armor2element1.Text);
                if (row[armor1element1.Text].ToString() == "Yes" && row[armor2element1.Text].ToString() == "Yes" &&
                    row[elements[0]].ToString() == "No" && row[elements[1]].ToString() == "No" &&
                    row[elements[2]].ToString() == "No")
                {
                    addArmor(name, combination, false, rarity, combinedStats);
                }
                elements.Add(armor1element1.Text);
                elements.Add(armor2element1.Text);

                if (armor1element2.Text != "None")
                {
                    //mono Armor 1 Element 2
                    elements.Remove(armor1element2.Text);
                    if (row[armor1element2.Text].ToString() == "Yes" && row[elements[0]].ToString() == "No" &&
                        row[elements[1]].ToString() == "No" && row[elements[2]].ToString() == "No"
                        && row[elements[3]].ToString() == "No")
                    {
                        addArmor(name, combination, true, rarity, combinedStats);
                    }
                    elements.Add(armor1element2.Text);

                    //Armor 1 Element 2 + Armor 2 Element 1
                    elements.Remove(armor1element2.Text);
                    elements.Remove(armor2element1.Text);
                    if (row[armor1element2.Text].ToString() == "Yes" && row[armor2element1.Text].ToString() == "Yes" &&
                        row[elements[0]].ToString() == "No" && row[elements[1]].ToString() == "No" &&
                        row[elements[2]].ToString() == "No")
                    {
                        addArmor(name, combination, false, rarity, combinedStats);
                    }
                    elements.Add(armor1element2.Text);
                    elements.Add(armor2element1.Text);

                    if (armor2element2.Text != "None")
                    {
                        //Armor 1 Element 2 + Armor 2 Element 2
                        elements.Remove(armor1element2.Text);
                        elements.Remove(armor2element2.Text);
                        if (row[armor1element2.Text].ToString() == "Yes" && row[armor2element2.Text].ToString() == "Yes" &&
                            row[elements[0]].ToString() == "No" && row[elements[1]].ToString() == "No" &&
                            row[elements[2]].ToString() == "No")
                        {
                            addArmor(name, combination, false, rarity, combinedStats);
                        }
                        elements.Add(armor1element2.Text);
                        elements.Add(armor2element2.Text);
                    }
                }
                if (armor2element2.Text != "None")
                {
                    //mono Armor 2 Element 2
                    elements.Remove(armor2element2.Text);
                    if (row[armor2element2.Text].ToString() == "Yes" && row[elements[0]].ToString() == "No" &&
                        row[elements[1]].ToString() == "No" && row[elements[2]].ToString() == "No"
                        && row[elements[3]].ToString() == "No")
                    {
                        addArmor(name, combination, true, rarity, combinedStats);
                    }
                    elements.Add(armor2element2.Text);

                    //Armor 1 Element 1 + Armor 2 Element 2
                    elements.Remove(armor1element1.Text);
                    elements.Remove(armor2element2.Text);
                    if (row[armor1element1.Text].ToString() == "Yes" && row[armor2element2.Text].ToString() == "Yes" &&
                        row[elements[0]].ToString() == "No" && row[elements[1]].ToString() == "No" &&
                        row[elements[2]].ToString() == "No")
                    {
                        addArmor(name, combination, false, rarity, combinedStats);
                    }
                    elements.Add(armor1element1.Text);
                    elements.Add(armor2element2.Text);
                }
            }

            outcomes.Text += "\r\n";
            outcomes.Text += "\r\nEpic Chance: " + (epics / (total + 0.0)) * 100 + "%";
            outcomes.Text += "\r\nLegendary Chance: " + (legends / (total + 0.0)) * 100 + "%";
            outcomes.Text += "\r\n     (Mono Legendary Chance: " + (monoLegends / (total + 0.0)) * 100 + "%)";
            outcomes.Text += "\r\nUltra Rare Chance: " + (ultraRares / (total + 0.0)) * 100 + "%";
            outcomes.Text += "\r\nSuper Rare Chance: " + (superRares / (total + 0.0)) * 100 + "%";
            outcomes.Text += "\r\nRare Chance: " + (rares / (total + 0.0)) * 100 + "%";

            outcomes.Text += "\r\n";
            outcomes.Text += "\r\nChance that new armor will be better than either input: " + (betterStats / (total + 0.0)) * 100 + "%";
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

        private void addArmor(string name, string combination, bool isMono, int rarity, int combinedStats)
        {
            total++;
            if (rarity == 1)
            {
                rares++;
            }
            else if (rarity == 2)
            {
                superRares++;
            }
            else if (rarity == 3)
            {
                ultraRares++;
            }
            else if (rarity == 4)
            {
                legends++;
                if (isMono)
                {
                    monoLegends++;
                }
            }
            else if (rarity == 5)
            {
                epics++;
            }

            if (combinedStats > Math.Max(Convert.ToInt32(armor1stats.Text), Convert.ToInt32(armor2stats.Text)))
            {
                betterStats++;
            }

            outcomes.Text += name + " : " + combination + " : " + rarity + " : " + combinedStats + "\r\n";
        }

        private void showError(string theError)
        {
            MessageBox.Show(theError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        private void AllCheckBoxes_CheckedChanged(Object sender, EventArgs e)
        {
            if (armor1manual.Checked)
            {
                armor1combo.Enabled = false;

                armor1element1.Enabled = true;
                armor1element2.Enabled = true;
                armor1stars.Enabled = true;
                armor1stats.Enabled = true;
            }
            else if (armor1auto.Checked)
            {
                armor1combo.Enabled = true;

                armor1element1.Enabled = false;
                armor1element2.Enabled = false;
                armor1stars.Enabled = false;
                armor1stats.Enabled = false;
            }
            if (armor2manual.Checked)
            {
                armor2combo.Enabled = false;

                armor2element1.Enabled = true;
                armor2element2.Enabled = true;
                armor2stars.Enabled = true;
                armor2stats.Enabled = true;
            }
            else if (armor2auto.Checked)
            {
                armor2combo.Enabled = true;

                armor2element1.Enabled = false;
                armor2element2.Enabled = false;
                armor2stars.Enabled = false;
                armor2stats.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            armor1manual.Checked = true;
            armor2manual.Checked = true;

            allArmors = CsvToDataTable("armors.csv");
            confirmedArmors = CsvToDataTable("confirmed.csv");

            foreach (DataRow row in allArmors.Rows)
            {
                string name = row["Name"].ToString();

                armor1combo.Items.Add(name);
                armor2combo.Items.Add(name);
            }
        }

        private void armor1combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string armorName = armor1combo.Text.Replace("\'", "\'\'");

                DataRow[] theArmor;
                theArmor = allArmors.Select("Name=\'" + armorName + "\'");

                armor1element1.Text = theArmor[0]["Element1"].ToString();
                if (theArmor[0]["Element2"].ToString() == "0")
                    armor1element2.Text = "None";
                else
                    armor1element2.Text = theArmor[0]["Element2"].ToString();
                armor1stars.Text = theArmor[0]["Rarity"].ToString();
                armor1stats.Text = ((int)theArmor[0]["Attack"] + (int)theArmor[0]["Defense"]).ToString();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }

        private void armor2combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string armorName = armor2combo.Text.Replace("\'", "\'\'");

                DataRow[] theArmor;
                theArmor = allArmors.Select("Name=\'" + armorName + "\'");

                armor2element1.Text = theArmor[0]["Element1"].ToString();
                if (theArmor[0]["Element2"].ToString() == "0")
                    armor2element2.Text = "None";
                else
                    armor2element2.Text = theArmor[0]["Element2"].ToString();
                armor2stars.Text = theArmor[0]["Rarity"].ToString();
                armor2stats.Text = ((int)theArmor[0]["Attack"] + (int)theArmor[0]["Defense"]).ToString();
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
        }
    }
}
