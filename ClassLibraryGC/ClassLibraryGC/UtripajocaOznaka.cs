using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibraryGC
{
    public partial class UtripajocaOznaka : UserControl
    {
        public UtripajocaOznaka()
        {
            InitializeComponent();
        }
        bool spremeni = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (spremeni)
                label1.ForeColor = Color.Blue;
            else
                label1.ForeColor = Color.ForestGreen;
            spremeni = !spremeni;
        }

        public int Interval
        {
            get { return timer1.Interval; }
            set { timer1.Interval = value; }
        }

        public string Besedilo  //lastnost/property, ki bo omogočala pridobivanje in spreminjanje lastnosti Text oznake Label1, ki je na kontroli
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

    }
}
