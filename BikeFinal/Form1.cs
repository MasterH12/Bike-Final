using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace BikeFinal
{
    public partial class Form1 : Form
    {
        OleDbConnection connection = new OleDbConnection();
        public Form1()
        {
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\felip\source\repos\Bike-Final\BikeFinal\bin\Debug\DB.accdb;
Persist Security Info=False;";
            InitializeComponent();
        }

        private void bicicletasBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bicicletasBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dBDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dBDataSet.Arriendos' Puede moverla o quitarla según sea necesario.
            this.arriendosTableAdapter.Fill(this.dBDataSet.Arriendos);
            // TODO: esta línea de código carga datos en la tabla 'dBDataSet.Bicicletas' Puede moverla o quitarla según sea necesario.
            this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == false)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
            }
            else
            {
                panel1.Visible = false;
            }
            mARCATextBox.Text = null;
            rODADOTextBox.Text = null;
            tALLATextBox.Text = null;
            vALORTextBox.Text = null;
            iDTextBox.Text = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mARCATextBox.Text == "")
                {
                    throw new ArgumentException("No se ha ingresado MARCA");
                }
                int a = Int32.Parse(iDTextBox.Text);
                this.arriendosTableAdapter.AGREGAR_ARR(a);
                this.bicicletasTableAdapter.AGREGAR(mARCATextBox.Text, Int32.Parse(rODADOTextBox.Text), Int32.Parse(tALLATextBox.Text), Int32.Parse(vALORTextBox.Text), false, false, a);
                this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
            }
            catch(Exception ex)
            {
                if (ex.Message=="La cadena de entrada no tiene el formato correcto.")
                {
                    MessageBox.Show("No se han ingresado correctamente los datos", "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Se procede a buscar la información que se ingresó erroneamente
                    string[] frases = new string[4];
                    string frase="";
                    frases[0] = rODADOTextBox.Text;
                    frases[1] = tALLATextBox.Text;
                    frases[2] = vALORTextBox.Text;
                    frases[3] = iDTextBox.Text;
                    for (int j = 0; j < 4; j++)
                    {
                        frase = frases[j];
                        for(int i = 0; i < frase.Length; i++)
                        {
                            if(frase[i]<48 || frase[i] > 57)
                            {
                                switch (j)
                                {
                                    case 0:
                                        rODADOTextBox.Text = string.Concat("(*)", rODADOTextBox.Text);
                                        break;
                                    case 1:
                                        tALLATextBox.Text = string.Concat("(*)", tALLATextBox.Text);
                                        break;
                                    case 2:
                                        vALORTextBox.Text = string.Concat("(*)", vALORTextBox.Text);
                                        break;
                                    case 3:
                                        iDTextBox.Text = string.Concat("(*)", iDTextBox.Text);
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
                if(ex.Message== "No se ha ingresado MARCA")
                {
                    MessageBox.Show(ex.Message, "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (ex.GetType().ToString() == "System.Data.OleDb.OleDbException")
                {
                    MessageBox.Show("Ya existe un elemento con la ID ingresada, por favor introduzca una ID diferente", "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (panel2.Visible == false)
            {
                comboBox1.Items.Clear();
                try
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string query = "select DISTINCT * from Bicicletas";
                    command.CommandText = query;

                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["Id"].ToString());
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                panel2.Visible = true;
                panel1.Visible = false;
                panel3.Visible = false;
            }
            else
            {
                panel2.Visible = false;
            }
            mARCATextBox1.Text = null;
            rODADOTextBox1.Text = null;
            tALLATextBox1.Text = null;
            vALORTextBox1.Text = null;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.bicicletasTableAdapter.MODIFICAR(mARCATextBox1.Text, Int32.Parse(rODADOTextBox1.Text), Int32.Parse(tALLATextBox1.Text), Int32.Parse(vALORTextBox1.Text), Int32.Parse(comboBox1.Text));
            this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                connection.Open();

                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string query = "select * from Bicicletas where ID =" + comboBox1.Text;
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mARCATextBox1.Text = reader["MARCA"].ToString();
                    rODADOTextBox1.Text = reader["RODADO"].ToString();
                    tALLATextBox1.Text = reader["TALLA"].ToString();
                    vALORTextBox1.Text = reader["VALOR"].ToString();

                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.bicicletasTableAdapter.ELIMINAR(Int32.Parse(comboBox1.Text));
            this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (panel3.Visible == false)
            {
                listBox1.Items.Clear();
                try
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string query = "select DISTINCT * from Bicicletas WHERE EN_ARRIENDO = false";
                    command.CommandText = query;

                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        listBox1.Items.Add(reader["Id"].ToString());
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                panel3.Visible = true;
                panel2.Visible = false;
                panel1.Visible = false;
            }
            else
            {
                panel3.Visible = false;
            }
            }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                connection.Open();

                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string query = "select * from Bicicletas where ID =" + listBox1.Text;
                command.CommandText = query;

                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    iDTextBox1.Text = reader["ID"].ToString();
                    

                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

        }
        DateTime hoy = DateTime.Now;
        private void button7_Click(object sender, EventArgs e)
        {
            int b = Int32.Parse(iDTextBox1.Text);
            this.bicicletasTableAdapter.INICIAR_ARRIENDO(true, b);
            this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
            panel3.Visible=false;
            string HORAs = hoy.ToShortTimeString();
            char[] HORAc= new char[HORAs.Length];
            string hora = (HORAc[0].ToString()) + (HORAc[1].ToString());
            string minutos= (HORAc[3].ToString()) + (HORAc[4].ToString());
            int h = Int32.Parse(hora);
            int m = Int32.Parse(minutos);
            this.arriendosTableAdapter.INICIAR_ARR(h,m,b);



        }

        private void rODADOTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

       
    }

