using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CukrProject;
using ClassLibraryGC;



namespace HealthTracker
{
    public partial class FMeni : Form
    {
        public FMeni()
        {
            InitializeComponent();
        }
        
        private void btnCukr_Click(object sender, EventArgs e)
        {
            FCukr window = new FCukr();
            window.Show();             
        }

        private void btnSport_Click(object sender, EventArgs e)
        {
            FSport window = new FSport();
            window.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FSport window = new FSport();
            window.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            FCukr window = new FCukr();
            window.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FSport window = new FSport();
            window.Show();
        }

        private void btnZnanje_Click(object sender, EventArgs e)
        {
            FZnanje window = new FZnanje();
            window.Show();
        }

        #region INFO Dinamično
        Form FInfo = new Form();

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FInfo.StartPosition = FormStartPosition.CenterScreen;
            FInfo.Load += FInfo_Load;
            FInfo.Text = "Info";
            FInfo.Width = 600;
            FInfo.Height = 600;
            FInfo.ControlBox = false;
            FInfo.FormBorderStyle = FormBorderStyle.None;

            FInfo.ShowDialog();
        }

        private void FInfo_Load(object sender, EventArgs e)
        {
            PictureBox slika = new PictureBox();
            slika.Parent = FInfo;
            slika.Width = 560;
            slika.Height = 225;
            slika.Left = 20;
            slika.Top = 20;
            slika.ImageLocation = @"C:\Users\GEP\Desktop\N1\C#\HealthTracker\HealthTracker\slika1.jpg";
            slika.SizeMode = PictureBoxSizeMode.StretchImage;

            ClassLibraryGC.UtripajocaOznaka lblNaslov = new ClassLibraryGC.UtripajocaOznaka();
            lblNaslov.Parent = FInfo;
            lblNaslov.Top = 290;
            lblNaslov.Left = 130;
            lblNaslov.Besedilo = "HEALTH TRACKER";
            lblNaslov.Height = 50;
            lblNaslov.Width = 300;
            FInfo.Controls.Add(lblNaslov);

            Label lblText = new Label();
            lblText.Parent = FInfo;
            lblText.Font = new Font("Verdana", 15);
            lblText.Width = 560;
            lblText.Height = 80;
            lblText.Top = 350;
            lblText.Left = 20;
            lblText.Text = "Porgram je namenjen vsem, ki želijo spremljati svojim trendom kot so zdravje (diabetes) \r\n" +
                            "šport ter znanje";

            Label lblLink = new Label();
            lblLink.Parent = FInfo;
            lblLink.Font = new Font("Verdana", 15);
            lblLink.Width = 400;
            lblLink.Height = 30;
            lblLink.Top = 445;
            lblLink.Left = 20;
            lblLink.Text = "Spletna stran z vsebino:";
            FInfo.Controls.Add(lblLink);

            LinkLabel link = new LinkLabel();
            link.Parent = FInfo;
            link.Font = new Font("Verdana", 10);
            link.Width = 350;
            link.Height = 30;
            link.Top = 475;
            link.Left = 20;
            link.Text = "https://fitnesmotivacija.webs.com/";
            link.Click += Link_Click;

            Button Zapri = new Button();
            Zapri.Parent = FInfo;
            Zapri.Text = "Zapri";
            Zapri.Font = new Font("Verdana", 10);
            Zapri.Height = 30;
            Zapri.Width = 100;
            Zapri.Left = 250;
            Zapri.Top = 550;
            Zapri.Click += Zapri_Click;
        }

        private void Zapri_Click(object sender, EventArgs e)
        {
            FInfo.Close();
        }

        private void Link_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://fitnesmotivacija.webs.com/");
        }
        #endregion

        private void FMeni_Load(object sender, EventArgs e)
        {        
        }

        private void zapriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ali res želite zapreti aplikacijo?", "Zapri?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }          
        }
    }
}
