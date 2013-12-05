using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTest1
{
    public partial class InputName : Form
    {
        Form1 _form;

        public string name = "Player";
        public InputName(Form1 form)
        {
            InitializeComponent();
            _form = form;
            textBox1.Text = "Player";
        }

        public string GetName()
        {
            return this.name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.name = textBox1.Text;

            // Close the windows form
            this.Close();
        }
    }
}
