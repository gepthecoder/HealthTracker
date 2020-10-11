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

namespace HealthTracker
{
    public partial class FPrijava : Form
    {
        public FPrijava()
        {
            InitializeComponent();         
           
        }
        public string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30";
        public static string _username = "";

        #region Gumb Prijava
        //gumb: Prijava
        private void button2_Click(object sender, EventArgs e)
        {
            FSport fs = new FSport();
            SqlConnection dataConnection = new SqlConnection(ConnectionString);
            dataConnection.Open(); //odpremo povezavo
            string poizvedba = "Select Count(*) from tbl_login where Username='" + txtUser.Text + "' and Password='" + txtPass.Text + "'";
            using (SqlCommand dataCommand = new SqlCommand(poizvedba, dataConnection))
            {
                if (System.Convert.ToInt32(dataCommand.ExecuteScalar()) == 1) //preverimo, če je bila poizvedba uspešna
                {
                    _username = txtUser.Text;
                    this.Hide();//skrijemo prijavni obrazec
                    FMeni meni = new FMeni();  //nov objekt za glavni obrazec
                    meni.Show();
                   
                }
                else
                {
                    MessageBox.Show("Napačno uporabniško ime ali geslo!");
                    Ponastavi();
                }
            }
        }
        #endregion

        private string Encryptdata(string password)  //šifrirni algoritem za kodiranje niza
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string VrniTrenutnoImeUporabnika(string username)
        {
           
            Uporabniki trenutni = new Uporabniki();
            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                string sql = "Select Ime From MetaUser where Username='"+username+"'";
                SqlCommand oCmd = new SqlCommand(sql, myConnection);             
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        trenutni.Ime = oReader[0].ToString();                       
                    }                 
                    myConnection.Close();

                    return trenutni.Ime;
                }
            }
        }
        public string VrniTrenutniPriimekUporabnika(string username)
        {

            Uporabniki trenutni = new Uporabniki();
            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                string sql = "Select Priimek From MetaUser where Username='" + username + "'";
                SqlCommand oCmd = new SqlCommand(sql, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        trenutni.Priimek = oReader[0].ToString();
                    }
                    myConnection.Close();

                    return trenutni.Priimek;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {           
            this.Hide();
            FStart window = new FStart();
            window.Show();
        }

        private void registracijaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            FRegistracija window = new FRegistracija();
            window.Show();
        }

        public void Ponastavi()
        {
            txtUser.Text = "";
            txtPass.Text = "";
        }
    }
}
