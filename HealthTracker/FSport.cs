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
using System.Drawing.Imaging;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace HealthTracker
{
    public partial class FSport : Form
    {

        public string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
        public string user = "";

        SqlConnection conne = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TrackerDataBase.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        public FSport()
        {
            InitializeComponent();                   
            NapolniGrid();                             

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZapisiPodatke(); //zapis podatkov v bazo
            NapolniGrid();
            Ponastavi();
        }

        public void PridobiSliko(string user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Slika FROM MetaUser WHERE Username = '"+user+"'", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] slika = reader["Slika"] as byte[] ?? null;

                        if (slika != null)
                        {
                            MemoryStream ms = new MemoryStream(slika);                         
                            pictureBox1.Image = Image.FromStream(ms);
                            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            
                        }
                        else { pictureBox1.Image = null; }
                    }
                }
            }
        }

        public void NastavitveGridView()
        {
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 30;
            DataGridViewColumn column1 = dataGridView1.Columns[1];
            column1.Width = 40;
            DataGridViewColumn column2 = dataGridView1.Columns[2];
            column2.Width = 40;
            DataGridViewColumn column3 = dataGridView1.Columns[3];
            column3.Width = 40;
            DataGridViewColumn column4 = dataGridView1.Columns[4];
            column4.Width = 50;
        }

        public void ZapisiPodatke()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    String query = "INSERT INTO Sport (sklece,pullups,dips,trebuh,datum) VALUES (@param1,@param2,@param3,@param4,@param5)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@param1", Convert.ToInt32(numSklece.Value));
                        command.Parameters.AddWithValue("@param2", Convert.ToInt32(numPull.Value));
                        command.Parameters.AddWithValue("@param3", Convert.ToInt32(numDips.Value));
                        command.Parameters.AddWithValue("@param4", Convert.ToInt32(numTrebuh.Value));
                        command.Parameters.AddWithValue("@param5", Convert.ToDateTime(dateTimePicker1.Value));
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

        public void NapolniGrid()
        {
            try
            {
                SqlConnection con = new SqlConnection(_connectionString);

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.Connection = con;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select * from Sport";
                SqlDataAdapter adpt = new SqlDataAdapter(sqlCmd);

                DataTable dt = new DataTable();
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
           
        }

        private void btnNazaj_Click(object sender, EventArgs e)
        {
            this.Close();         
        }

        public void Ponastavi()
        {
            numSklece.Value = 0;
            numPull.Value = 0;
            numTrebuh.Value = 0;
            numDips.Value = 0;
        }

        private void FSport_Load(object sender, EventArgs e)
        {
            FPrijava fp = new FPrijava();
            user = FPrijava._username;

            lblime.Text += "  " + fp.VrniTrenutnoImeUporabnika(user);
            lblpriimek.Text += "  " + fp.VrniTrenutniPriimekUporabnika(user);

            PridobiSliko(user);

        }

        private void sportBindingSource_CurrentChanged(object sender, EventArgs e)
        {
       
                              
        }

        public void NapolniTextBoxe()
        {        
            numSklece.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            numPull.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            numDips.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            numTrebuh.Value  = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[3].Value.ToString());
        }

        //izvoz v excel
        //Metoda za zapis vsebine gradnika DataGridView v "EXCEL-ovo" datoteko.
        public static void VExcel(DataGridView dgv)
        {
            try
            {
                //Preverimo ali je sploh kaj podatkov v dataGridView-u
                if (dgv.Rows.Count != 0)
                {
                    /*dialog za shranjevanje: lokacijo Excel datoteke bo določil
                        uporabnik*/
                    SaveFileDialog sFD = new SaveFileDialog();
                    //napis na pogovornem oknu
                    sFD.Title = "Shranjevanje podatkov v \"EXCEL\" datoteko!";
                    sFD.Filter = "Excel datoteke (*.xls)|*.xls|Excel datoteke (*.xlsx)|*.xlsx"; //filter za dialog.
                    string imeExcelDat;
                    if (sFD.ShowDialog() == DialogResult.OK)
                    {
                        imeExcelDat = sFD.FileName;
                        //ustvarimo nov objekt tipa Excel.Application
                        Excel.Application xlApp = new Excel.Application();
                        /*s pomočjo metode Add ustvarimo nov delovni zvezek.
                        Metodi Add lahko v oklepaju navedemo ime obstoječega Excel zvezka in nova
                        stran se bo vanj dodala*/
                        Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
                        /*ustvarimo novo stran: število strani določimo s pomočjo
                            metode get_Item (številka 1 pomeni eno stran)*/
                        Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                        //Shranimo zapise v glavi vsakega stolpca v excel.
                        string nizGlava = "";
                        int stCelice = 0; /*zaporedna številka celice v vrstici*/
                                          /*v preglednici ustvarimo toliko vrstic in celic, kolikor
                                              jih je v dGVDrzave*/
                        for (int vrstica = 0; vrstica < dgv.Columns.Count; vrstica++)
                        {
                            //Shranimo vrednost iz celice v glavi v niz.
                            nizGlava = dgv.Columns[vrstica].HeaderText.ToString();
                            //Font nastavimo na krepko.
                            xlWorkSheet.Cells[1, stCelice + 1].Font.Bold = true;
                            //določimo barvo pisave v glavah stolpcev.
                            xlWorkSheet.Cells[1, stCelice + 1].Font.Color = ColorTranslator.ToOle(System.Drawing.Color.Black);
                            xlWorkSheet.Cells[1, stCelice + 1].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(204, 255, 255));
                            xlWorkSheet.Cells[1, stCelice + 1] = nizGlava;
                            stCelice++; /*povečamo zaporedno številko celice v
                                tekoči vrstici*/
                            nizGlava = "";  /*resetiramo niz za shranjevanje
                                    vrednosti celice*/
                        }
                        /*z dvojno zanko se sprehodimo skozi podatke in
                            shranimo vrednosti celic v excel*/
                        for (int i = 0; i <= dgv.RowCount - 1; i++)
                        {
                            for (int j = 0; j <= dgv.ColumnCount - 1; j++)
                            {
                                /*vsebino tekoče celice shranimo v objekt
                                    ustreznega tipa*/
                                DataGridViewCell cell = dgv[j, i];
                                //zapis v preglednico
                                xlWorkSheet.Cells[i + 2, j + 1] = cell.Value;
                                /*ali pa hitreje kar takole:  
                                xlWorkSheet.Cells[i + 2, j + 1]=dgv[j,i].Value;*/
                            }
                        }
                        //Preverimo končnico excel-ove datoteke.
                        string koncnicaExcel = Path.GetExtension(imeExcelDat);
                        /*Če je excel datoteka s končnico ".xlsx", potem jo
                            shranimo v tem formatu*/
                        if (koncnicaExcel == ".xlsx")
                        {
                            /*preglednico shranimo z metodo SaveAs: večino parametrov te metode ni potrebno navajati (npr. Password, ReadOnlyrecommended, CreateBackup, ...)*/
                            xlWorkBook.SaveAs(imeExcelDat, Excel.XlFileFormat.xlOpenXMLWorkbook, null, null, null, null, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlUserResolution, null, null, null, null);
                        }
                        else //sicer pa bo shranjena v formatu ".xls"
                            xlWorkBook.SaveAs(imeExcelDat, Excel.XlFileFormat.xlWorkbookNormal, null, null, null, null, Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
                        xlWorkBook.Close();//zapremo delovni zvezek
                        xlApp.Quit();//zaključek dela z Excel-om
                                     /*ker objektov za delo z Excel-om ne potrebujemo več,
                                         lahko sami poskrbimo za sproščanje ustvarjenih objektov
                                         ReleaseObject(xlWorkSheet);//ReleaseObject(xlWorkBook);
                                         ReleaseObject(xlApp);*/
                        MessageBox.Show("Excel datoteka je ustvarjena in shranjena v: " + imeExcelDat, "Podatki o ustvarjeni \"EXCEL\" datoteki!");
                    }
                    else
                    {
                        MessageBox.Show("Pretvorba v \"Excel\" zavrnjena, ker je tabela s podatki PRAZNA!");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void izvoziVExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VExcel(dataGridView1);
        }

        private void natisniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pD = new PrintDialog(); //dialog za izbiro tiskalnika brez predogleda, njegovih lastnosti, števila in obseg strani
            PrintPreviewDialog pPD = new PrintPreviewDialog();  //dialog za izbiro tiskalnika s predogledom, njegovih lastnosti, števila in obseg strani
                                                                //pDocje objekt tipa PrintDocument, ki ga na obrazec potegnemo iz orodjarne in ga poimenujemo
            System.Drawing.Printing.PrintDocument pDoc = new System.Drawing.Printing.PrintDocument();
            pDoc.PrintPage += pDoc_PrintPage;
            if (pD.ShowDialog() == DialogResult.OK)
            {
                pDoc.PrinterSettings = pD.PrinterSettings;// privzamemo nastavitve tiskalnika za naš dokument
                pPD.Document = pDoc;   //pred tiskanjem določimo kako bo izgledala stran, ki jo bomo tiskali
                pPD.ShowDialog();
            }
        }

        void pDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(VsebinaTiskanja(), this.Font, Brushes.Blue, new PointF(30, 30));//stiskali bomo napis na obrazcu
        }

        public string VsebinaTiskanja()
        {
            string elementi = "Datum                        Sklece  Pullups  Dips  Trebuh";
            string data = "";

            try
            {
                SqlConnection con = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand("SELECT * FROM Sport", con);
                SqlDataReader myreader;
                con.Open();

                myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    data += myreader[5].ToString() + "      " + myreader[1].ToString() + "            " + myreader[2].ToString() + "            " + myreader[3].ToString() + "       " + myreader[4].ToString() + "\n";

                }
                con.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return elementi + "\n" + data;
        }       

        bool PreveriPolja()
        {
            int skl = 0;
            int dip = 0;
            int treb = 0;
            int pull = 0;
            try
            {
                skl = Convert.ToInt32(numSklece.Value);
                dip = Convert.ToInt32(numDips.Value);
                treb = Convert.ToInt32(numTrebuh.Value);
                pull = Convert.ToInt32(numPull.Value);

            }
            catch
            {
                errorProvider1.SetError(numPull, "Napačen vnos ponovitev.");
                errorProvider1.SetError(numSklece, "Napačen vnos ponovitev.");
                errorProvider1.SetError(numDips, "Napačen vnos ponovitev.");
                errorProvider1.SetError(numTrebuh, "Napačen vnos ponovitev.");
                return false;
            }
            if (skl < 0 || dip < 0 || treb < 0 || pull < 0)
            {
                errorProvider1.SetError(numSklece, "Podatek nemore biti manjši od 0!");
                errorProvider1.SetError(numPull, "Podatek nemore biti manjši od 0!");
                errorProvider1.SetError(numTrebuh, "Podatek nemore biti manjši od 0!");
                errorProvider1.SetError(numDips, "Podatek nemore biti manjši od 0!");
                return false;
            }

            else
            {
                errorProvider1.SetError(numSklece, "");
                return true;
            }
        }

        private void numSklece_Validating_1(object sender, CancelEventArgs e)
        {
            PreveriPolja();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            NapolniTextBoxe();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Uredi();
        }

        public int GetIDfromSelectedColumn()
        {
            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            return id;
        }

        public void Uredi()
        {
            try
            {
                using (var connection1 = new SqlConnection(_connectionString))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter();
                    using (var insertCommand = new SqlCommand("UPDATE Sport SET sklece = '" + numSklece.Value + "', pullups = '" + numPull.Value + "', dips = '" + numDips.Value + "', trebuh = '" + numTrebuh.Value + "' WHERE id = " + GetIDfromSelectedColumn())) 
                    {
                        insertCommand.Connection = connection1;
                        cmd.InsertCommand = insertCommand;                       
                        connection1.Open();
                        insertCommand.ExecuteNonQuery();
                        NapolniGrid();
                        Ponastavi();                       
                    }
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Prišlo je do napake!");
            }
            finally { ZapriPovezavo(); }

        }

        public void ZapriPovezavo()
        {
            if (conne.State == ConnectionState.Open)
            {
                conne.Close();
            }
        }
    }
}
