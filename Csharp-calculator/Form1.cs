using System;
using System.Linq;
using System.Windows.Forms;

namespace Csharp_calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (standart_mode.Checked)
            {
                textBox1.Text = Standart.Calculate(textBox1.Text);
            }
            else 
            {
                dataGridView1.Rows.Clear();
                var mas = Logic.Calculat(textBox1.Text);
                var keys = Logic.dictKeys();
                dataGridView1.RowCount = mas.GetLength(0);
                dataGridView1.ColumnCount = mas.GetLength(1);
                
                for (int i = 0; i < mas.GetLength(1); i++)
                {
                    try
                    {
                        dataGridView1.Columns[i].HeaderText = keys[i].ToString();
                    }
                    catch
                    {
                        dataGridView1.Columns[i].HeaderText = "F";
                    }
                    for (int j = 0; j < mas.GetLength(0); j++)
                    {
                        dataGridView1[i, j].Value = Convert.ToInt32(mas[j, i]);
                    }
                }
                
            }
        }

        private void modemenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var clicked = e.ClickedItem as ToolStripMenuItem;
            foreach(ToolStripMenuItem item in clicked.Owner.Items.OfType<ToolStripMenuItem>())
            {
                item.Checked = false;
            }
            clicked.Checked = true;
            modemenu.Text = clicked.Text;
        }

        private void logic_mode_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = logic_mode.Checked;
            dataGridView1.Enabled = isChecked;
            dataGridView1.Visible = isChecked;
            this.Width = isChecked ? 971 : 483;
        }
    }
}
