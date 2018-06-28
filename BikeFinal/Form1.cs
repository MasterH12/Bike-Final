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
            //La siguiente linea de código tiene que ser modificada según la dirección del repositorio en el que estén (en cada computadora es diferente)
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\MasterH\source\repos\BikeControlFinal\BikeFinal\bin\Debug\DB.accdb;
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
            //this.arriendosTableAdapter.Fill(this.dBDataSet.Arriendos);
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
                REPARACION.Visible = false;
            }
            else
            {
                panel1.Visible = false;
            }

            mARCATextBox.Text = "";
            rODADOTextBox.Text = "";
            tALLATextBox.Text = "";
            vALORTextBox.Text = "";
            iDTextBox.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                rODADOTextBox.ForeColor = Color.Black;
                tALLATextBox.ForeColor = Color.Black;
                vALORTextBox.ForeColor = Color.Black;
                iDTextBox.ForeColor = Color.Black;
                if (mARCATextBox.Text == "")
                {
                    throw new ArgumentException("No se ha ingresado MARCA");
                }
                else { 
                    int a = Int32.Parse(iDTextBox.Text);
                    int x;
                    //x no sirve de nada
                    //Agregar arriendos está clausurado por ahora para evitar problemas de base de datos
                    //this.arriendosTableAdapter.AGREGAR_ARR(a);

                    this.bicicletasTableAdapter.AGREGAR(mARCATextBox.Text, Int32.Parse(rODADOTextBox.Text), Int32.Parse(tALLATextBox.Text), Int32.Parse(vALORTextBox.Text), false, false, a);
                    this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);

                }
            }
            catch(Exception ex)
            {
                if (ex.Message == "La cadena de entrada no tiene el formato correcto." || ex.Message == "No se ha ingresado MARCA")
                {
                    //Se procede a buscar la información que se ingresó erroneamente
                    string[] frases = new string[5];
                    string frase = "";
                    string mensaje = "Se han ingresado erróneamente los siguientes datos:";
                    frases[0] = iDTextBox.Text;
                    frases[1] = mARCATextBox.Text;
                    frases[2] = rODADOTextBox.Text;
                    frases[3] = tALLATextBox.Text;
                    frases[4] = vALORTextBox.Text;
                    Boolean error = false;
                    for (int j = 0; j < frases.Length; j++)
                    {
                        error = false;
                        frase = frases[j];
                        for (int i = 0; i < frase.Length; i++)
                        {
                            //Se verifica si se no se ingresó algo en MARCA, y si los datos de ID, rodad, talla y valor hayan sido correctos (números)
                            //Si se encuentra error, se saldrá del for con un break
                            if (frase[i] < 48 || frase[i] > 57 && j!=1)
                            {
                                error = true;
                                break;
                            }
                        }
                        if (j == 1 && mARCATextBox.Text == "")
                            error = true;
                        if (error == true)
                        {
                            mensaje = string.Concat(mensaje, "\n");
                            switch (j)
                            {
                                case 0:
                                    iDTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-ID (Debe ser un número)");
                                    break;
                                case 1:
                                    mensaje = string.Concat(mensaje, "\t-MARCA (No ha ingresado nada)");
                                    break;
                                case 2:
                                    rODADOTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-RODADO (Debe ser un número)");
                                    break;
                                case 3:
                                    tALLATextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-TALLA (Debe ser un número)");
                                    break;
                                case 4:
                                    vALORTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-VALOR (Debe ser un número)");
                                    break;
                            }
                        }
                    }
                    MessageBox.Show(mensaje, "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (ex.Message == "No se ha ingresado MARCA")
                    {
                        MessageBox.Show(ex.Message, "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (ex.GetType().ToString() == "System.Data.OleDb.OleDbException")
                        {
                            MessageBox.Show("Ya existe un elemento con la ID ingresada, por favor introduzca una ID diferente", "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            iDTextBox.ForeColor = Color.Red;
                        }
                    }
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
                REPARACION.Visible = false;
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
            try
            {
                rODADOTextBox1.ForeColor = Color.Black;
                tALLATextBox1.ForeColor = Color.Black;
                vALORTextBox1.ForeColor = Color.Black;
                iDTextBox1.ForeColor = Color.Black;
                if (mARCATextBox1.Text == "")
                {
                    throw new ArgumentException("No se ha ingresado MARCA");
                }
                else
                {
                    this.bicicletasTableAdapter.MODIFICAR(mARCATextBox1.Text, Int32.Parse(rODADOTextBox1.Text), Int32.Parse(tALLATextBox1.Text), Int32.Parse(vALORTextBox1.Text), Int32.Parse(comboBox1.Text));
                    this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
                }
            }
            catch(Exception ex)
            {
                if (ex.Message == "La cadena de entrada no tiene el formato correcto." || ex.Message == "No se ha ingresado MARCA")
                {
                    //Se procede a buscar la información que se ingresó erroneamente
                    string[] frases = new string[4];
                    string frase = "";
                    string mensaje = "Se han ingresado erróneamente los siguientes datos:";
                    frases[0] = mARCATextBox1.Text;
                    frases[1] = rODADOTextBox1.Text;
                    frases[2] = tALLATextBox1.Text;
                    frases[3] = vALORTextBox1.Text;
                    Boolean error = false;
                    for (int j = 0; j < frases.Length; j++)
                    {
                        error = false;
                        frase = frases[j];
                        for (int i = 0; i < frase.Length; i++)
                        {
                            //Se verifica si se no se ingresó algo en MARCA, y si los datos de ID, rodad, talla y valor hayan sido correctos (números)
                            //Si se encuentra error, se saldrá del for con un break
                            if (frase[i] < 48 || frase[i] > 57 && j!=0)
                            {
                                error = true;
                                break;
                            }
                        }
                        if (j == 0 && mARCATextBox1.Text == "")
                            error = true;
                        if (error == true)
                        {
                            mensaje = string.Concat(mensaje, "\n");
                            switch (j)
                            {
                                case 0:
                                    mARCATextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-MARCA (No se ha ingresado Marca)");
                                    break;
                                case 1:
                                    rODADOTextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-RODADO (Debe ser un número)");
                                    break;
                                case 2:
                                    tALLATextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-TALLA (Debe ser un número)");
                                    break;
                                case 3:
                                    vALORTextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-VALOR (Debe ser un número)");
                                    break;
                            }
                        }
                    }
                    MessageBox.Show(mensaje, "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
            DialogResult respuesta=MessageBox.Show("Está seguro de que desea eliminar", "Problema!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (respuesta == DialogResult.Yes)
            {
                this.bicicletasTableAdapter.ELIMINAR(Int32.Parse(comboBox1.Text));
                this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
            }
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
                REPARACION.Visible = false;
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
        //El evento siguiente controla lo que aparece en en el panel REPARACION
        //Todavía no se actualiza el listado de bicicletas en la listbox
        private void button8_Click(object sender, EventArgs e)
        {
            if (REPARACION.Visible == false)
            {
                listBox2.Items.Clear();
                REPARACION.Visible = true;
                panel3.Visible = false;
                panel2.Visible = false;
                panel1.Visible = false;
            }
            else
            {
                REPARACION.Visible = false;
            }

        }
    }

       
    }

