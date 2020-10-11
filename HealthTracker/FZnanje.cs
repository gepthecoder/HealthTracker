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
    public partial class FZnanje : Form
    {
        public string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30";
        public FZnanje()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            PokaziPodatke();
            NastaviGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FDodajKnjigo window = new FDodajKnjigo();
            window.StartPosition = FormStartPosition.CenterScreen;
            window.Show();
            
        }

        public void NastaviGridView()
        {
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 30;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            column1.Width = 80;
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            column2.Width = 80;
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            column3.Width = 80;
        
        }
        public void PokaziPodatke()
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionString);

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select * from Znanje";
                SqlDataAdapter adpt = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PokaziPodatke();
        }

        private void button3_Click(object sender, EventArgs e)
        {       
            this.Close();
        }
    }
}
