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
using System.IO;
using System.Drawing.Imaging;

namespace HealthTracker
{
    public partial class FRegistracija : Form
    {

        public FRegistracija()
        {
            InitializeComponent();
        }

        public string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30";
        public static string naslovSlike = "";


        #region Gumb Registracija
        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                //spremenljivka ki nam pove, če je uporabnik vsa polja izpolnil
                bool vsaPolja = true;
                //preverimo, če je vnesel vsa polja
                if (txtIme.Text == "" || txtPriimek.Text == "" || txtUser.Text == "" || txtGeslo.Text == "" || txtMail.Text == "")
                {
                    MessageBox.Show("Izpolniti morate vsa polja!");
                    DialogResult = DialogResult.Retry;
                    vsaPolja = false;
                }
                else if (vsaPolja)
                {
                    PreveriValidnostUsername();
                    ZabeležiMetaUser();
                    ZabeležiLogin();                   
                    MessageBox.Show("Hvala za vašo registracijo " + txtIme.Text + " " + txtPriimek.Text + "\nSedaj lahko nadaljujete s prijavo");
                }
            }catch(Exception ex) { MessageBox.Show(ex.Message); }
                                 
        }
        #endregion


        public void PreveriValidnostUsername()
        {
            SqlConnection povezava = new SqlConnection(_connectionString);
            povezava.Open();
            bool obstaja = false;
            //če uporabnik obstaja vrnemo obstaja = true
            using (SqlCommand sql = new SqlCommand())
            {
                sql.Connection = povezava;
                sql.Parameters.Add(new SqlParameter("@upIme", txtUser.Text));
                sql.CommandText = "Select Count(*) from tbl_login where Username=@upIme";
                if (System.Convert.ToInt32(sql.ExecuteScalar()) == 1) //preverimo ali uporabnik s tem imenom že obstaja
                    obstaja = true;
            }
            //Če uporabnik že obstaja to povemo uporabniku, in dialogResult nastavimo na Retry
            if (obstaja)
            {
                MessageBox.Show("Uporabnik s tem imenom že obstaja!");
                DialogResult = DialogResult.Retry;
                txtUser.Clear();
                txtGeslo.Clear();               
            }
        }

        public void NastaviUporabnika()
        {
            Uporabniki trenutni = new Uporabniki();
            trenutni.Ime = txtIme.Text;
            trenutni.Priimek = txtPriimek.Text;

        }

        //funkcija ki vrača prevajlniku hash od slike
        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }



        public void ZabeležiMetaUser()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                byte[] slika = null;
                FileStream fs = new FileStream(naslovSlike, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                slika = br.ReadBytes((int)fs.Length); //branje binarnih podatkob iz file streama, kjer se nahaja kompleksen niz
                String query = "INSERT INTO MetaUser (Ime,Priimek,Enaslov,Username,Password,Slika) VALUES (@ime,@priimek,@enaslov,@user,@pass,@slika)";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        command.Parameters.AddWithValue("@ime", txtIme.Text);
                        command.Parameters.AddWithValue("@priimek", txtPriimek.Text);
                        command.Parameters.AddWithValue("@enaslov", txtMail.Text);
                        command.Parameters.AddWithValue("@user", txtUser.Text);
                        command.Parameters.AddWithValue("@pass", txtGeslo.Text);
                        command.Parameters.Add(new SqlParameter("@slika", slika));
                        int result = command.ExecuteNonQuery();
                        connection.Close();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Prišlo je do napake zabeleževanja v SQL bazo!");
                    }
                }
                catch (Exception x) { MessageBox.Show(x.Message); }

            }
        }
     

        public void ZabeležiLogin()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                String query1 = "INSERT INTO tbl_login (Username,Password) VALUES (@ime,@geslo)";

                try
                {
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {

                        command.Parameters.AddWithValue("@ime", txtUser.Text);
                        command.Parameters.AddWithValue("@geslo", txtGeslo.Text);
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Prišlo je do napake zabeleževanja v SQL bazo !");
                    }
                }
                catch (Exception x) { MessageBox.Show(x.Message); }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FPrijava window = new FPrijava();
            window.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Odpri profilni sliko";
                dlg.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    naslovSlike = dlg.FileName.ToString();
                    pictureBox1.ImageLocation = naslovSlike;
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        public void zapisVdatoteko()
        {
            using (StreamWriter writer =
                    new StreamWriter("Uporabniki.txt"))
            {
                writer.WriteLine(txtUser.Text + ", " + txtGeslo.Text);

            }
        }
    }
}