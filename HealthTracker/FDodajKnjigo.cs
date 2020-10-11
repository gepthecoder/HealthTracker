using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HealthTracker
{
    public partial class FDodajKnjigo : Form
    {

        public class Knjiga
        {
            private string naslov;

            public string Naslov
            {
                get { return naslov; }
                set { naslov = value; }
            }

            private string avtor;

            public string Avtor
            {
                get { return avtor; }
                set { avtor = value; }
            }

            private string zanr;

            public string Zanr
            {
                get { return zanr; }
                set { zanr = value; }
            }

           
        }

        public string _connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30";
        public FDodajKnjigo()
        {
            InitializeComponent();
            NastaviComboBox();

        }

        public void NastaviVrednosti()
        {
            Knjiga nova = new Knjiga();
            nova.Naslov = txtNaslov.Text;
            nova.Avtor = txtAvtor.Text;
            nova.Zanr = comboBox1.GetItemText(comboBox1.SelectedItem);
        }

        public void NastaviComboBox()
        {
            comboBox1.Items.Add("Komedija");
            comboBox1.Items.Add("Grozljivka");
            comboBox1.Items.Add("Finance in denar");
            comboBox1.Items.Add("Tehnologija");
            comboBox1.Items.Add("Kriminalka");
            comboBox1.Items.Add("Samopodoba");
        }

        public void ZapisiVBazo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connection))
                {
                    String query = "INSERT INTO Znanje (Naslov,Avtor,Žanr) VALUES (@param1,@param2,@param3)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@param1", txtNaslov.Text);
                        command.Parameters.AddWithValue("@param2", txtAvtor.Text);
                        command.Parameters.AddWithValue("@param3", comboBox1.GetItemText(comboBox1.SelectedItem));                       
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Prišlo je do napake zabeleževanja v SQL bazo!");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        //metoda, ki preveri, če sta polji prazni, in če sta vrne error provider 
        bool PreveriCeSoPraznaPolja()
        {
            string niz = "";
            string niz1 = "";
            try
            {
                niz = txtNaslov.ToString();
                niz1 = txtAvtor.ToString();
            }
            catch
            {
                errorProvider1.SetError(txtNaslov, "Vnesili ste prazno polje!");
                return false;
            }
            if (niz1.Length < 0)
            {
                errorProvider1.SetError(txtAvtor, "Vnesili ste prazno polje!");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtAvtor, "");
                return true;
            }
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            ZapisiVBazo();
            NastaviVrednosti();           
            MessageBox.Show("Čestitke, kmalu boste MODER!!");
        }

        private void btnNazaj_Click(object sender, EventArgs e)
        {
            FZnanje window = new FZnanje();
            window.Show();
            this.Close();
        }

        private void txtAvtor_Validating(object sender, CancelEventArgs e)
        {
            PreveriCeSoPraznaPolja();
        }
    }
}
