using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testplugin2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //var commandForm = new CommandForm();
            //commandForm.TopLevel = false;
            //commandForm.FormBorderStyle = FormBorderStyle.None;
            //commandForm.Parent = tabPage1;
            //commandForm.ControlBox = false;
            //commandForm.Dock = DockStyle.Fill;
            //commandForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TShockAPI.Commands.HandleCommand(TShockAPI.TSPlayer.Server, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExecuteCommand("/time 4:30");
        }
        private static void ExecuteCommand(string text)
        {
            TShockAPI.Commands.HandleCommand(TShockAPI.TSPlayer.Server, text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExecuteCommand("/time 12:00");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ExecuteCommand("/time night");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExecuteCommand("/time 00:00");
        }
    }
}
