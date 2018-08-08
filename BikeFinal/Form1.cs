using System;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;


namespace BikeFinal
{
    public partial class Form1 : Form
    {
        OleDbConnection connection = new OleDbConnection();
        public Form1()
        {
            //La siguiente linea de código tiene que ser modificada según la dirección del repositorio en el que estén (en cada computadora es diferente)
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Nicolas Windows 7\source\repos\Bike-Final\BikeFinal\bin\Debug\DB.accdb;
Persist Security Info=False;";
            InitializeComponent();

            //algo trucho
            //iDTextBox1.Clear();
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
                panel4.Visible = false;
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
                else
                {
                    int a = Int32.Parse(iDTextBox.Text);
                    //Agregar arriendos está clausurado por ahora para evitar problemas de base de datos
                    this.arriendosTableAdapter.AGREGAR_ARR(a);

                    this.bicicletasTableAdapter.AGREGAR(mARCATextBox.Text, Int32.Parse(rODADOTextBox.Text), Int32.Parse(tALLATextBox.Text), Int32.Parse(vALORTextBox.Text), false, false, a);
                    this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);

                }
            }
            catch (Exception ex)
            {
                //PROBLEMA, AL INGRESAR ID 12, DICE QUE YA EXISTE LA ID
                if (ex.GetType().ToString() == "System.Data.OleDb.OleDbException")
                {
                    MessageBox.Show("Ya existe un elemento con la ID ingresada, por favor introduzca una ID diferente", "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    iDTextBox.ForeColor = Color.Red;
                }
                else
                {
                    //Se procede a buscar la información que se ingresó erroneamente
                    string[] frases = new string[5];
                    string frase = "";
                    string mensaje = "Se han ingresado erróneamente los siguientes datos:";
                    string causa = "";
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
                        if (frase == "")
                        {
                            causa = "nada";
                            error = true;
                        }
                        else
                        {
                            for (int i = 0; i < frase.Length; i++)
                            {
                                //Se verifica si se no se ingresó algo en MARCA, y si los datos de ID, rodad, talla y valor hayan sido correctos (números)
                                //Si se encuentra error, se saldrá del for con un break
                                if (frase[i] < 48 || frase[i] > 57 && j != 1)
                                {
                                    error = true;
                                    break;
                                }
                            }
                        }
                        if (error == true)
                        {
                            mensaje = string.Concat(mensaje, "\n");
                            switch (j)
                            {
                                case 0:
                                    iDTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-ID ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                                case 1:
                                    mensaje = string.Concat(mensaje, "\t-MARCA (No ha ingresado nada)");
                                    break;
                                case 2:
                                    rODADOTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-RODADO ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                                case 3:
                                    tALLATextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-TALLA ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                                case 4:
                                    vALORTextBox.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-VALOR ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                            }
                        }
                    }
                    MessageBox.Show(mensaje, "Problema!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                panel4.Visible = false;
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
                    iDTextBox1.Text = "";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "La cadena de entrada no tiene el formato correcto." || ex.Message == "No se ha ingresado MARCA")
                {
                    //Se procede a buscar la información que se ingresó erroneamente
                    string[] frases = new string[4];
                    string frase = "";
                    string mensaje = "Se han ingresado erróneamente los siguientes datos:";
                    string causa = "";
                    frases[0] = mARCATextBox1.Text;
                    frases[1] = rODADOTextBox1.Text;
                    frases[2] = tALLATextBox1.Text;
                    frases[3] = vALORTextBox1.Text;
                    Boolean error = false;
                    for (int j = 0; j < frases.Length; j++)
                    {
                        error = false;
                        frase = frases[j];
                        if (frase == "")
                        {
                            causa = "nada";
                            error = true;
                        }
                        for (int i = 0; i < frase.Length; i++)
                        {
                            //Se verifica si se no se ingresó algo en MARCA, y si los datos de ID, rodad, talla y valor hayan sido correctos (números)
                            //Si se encuentra error, se saldrá del for con un break
                            if (frase[i] < 48 || frase[i] > 57 && j != 1)
                            {
                                error = true;
                                break;
                            }
                        }
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
                                    mensaje = string.Concat(mensaje, "\t-RODADO ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                                case 2:
                                    tALLATextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-TALLA ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
                                    break;
                                case 3:
                                    vALORTextBox1.ForeColor = Color.Red;
                                    mensaje = string.Concat(mensaje, "\t-VALOR ");
                                    if (causa == "nada")
                                    {
                                        mensaje = string.Concat(mensaje, "(No ha ingresado nada)");
                                    }
                                    else
                                    {
                                        mensaje = string.Concat(mensaje, "(Debe ser un número)");
                                    }
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
            DialogResult respuesta = MessageBox.Show("Está seguro de que desea eliminar", "Problema!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (respuesta == DialogResult.Yes)
            {
                this.bicicletasTableAdapter.ELIMINAR(Int32.Parse(comboBox1.Text));
                this.arriendosTableAdapter.ELIMINAR(Int32.Parse(comboBox1.Text));
                this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
                mARCATextBox1.Text = "";
                rODADOTextBox1.Text = "";
                tALLATextBox1.Text = "";
                vALORTextBox1.Text = "";
                comboBox1.Text = "";
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
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            iDTextBox1.Text = "";
            if (panel3.Visible == false)
            {
                
                listBox1.Items.Clear();
                iDTextBox1.Clear();

                try
                {

                    connection.Open();

                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string query = "select DISTINCT * from Bicicletas WHERE EN_ARRIENDO = false AND EN_REPARACION = false";
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
                panel4.Visible = false;
                REPARACION.Visible = false;
            }
            else

            {
               // iDTextBox1.Text = "";
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
                if (listBox1.Text != "") { 
                    string query = "select * from Bicicletas where ID =" + listBox1.Text;
                    command.CommandText = query;

                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                     iDTextBox1.Text = reader["ID"].ToString();
                    }
                   
                }
                connection.Close();

            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show("Error " + ex);
            }

        }
        DateTime hoy = DateTime.Now;
        private void button7_Click(object sender, EventArgs e)
        {
            int b = Int32.Parse(iDTextBox1.Text);
            this.bicicletasTableAdapter.INICIAR_ARRIENDO(true, b);
            this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
            panel3.Visible = false;
            string HORAs = hoy.ToShortTimeString();
            string hora;
            string minutos;
            if (HORAs[1]==':'){
                hora = (HORAs[0].ToString());
                minutos = (HORAs[2].ToString()) + (HORAs[3].ToString());
            }
            else
            {
                hora = (HORAs[0].ToString()) + (HORAs[1].ToString());
                minutos = (HORAs[3].ToString()) + (HORAs[4].ToString());

            }
            char[] HORAc = new char[HORAs.Length];
            int h = Int32.Parse(hora);
            int m = Int32.Parse(minutos);
            this.arriendosTableAdapter.INICIAR_ARR(h, m, b);

        }
        //El evento siguiente controla lo que aparece en en el panel REPARACION
        //Todavía no se actualiza el listado de bicicletas en la listbox (ahora si deberia (nicolas))
        private void button8_Click(object sender, EventArgs e)
            //BOTON REPARACION en modo visible= false en propiedades(en la otra pestaña ->>>>>>)
        {
            if (REPARACION.Visible == false)
            {
                listBox2.Items.Clear();
                REPARACION.Visible = true;
                panel4.Visible = false;
                panel3.Visible = false;
                panel2.Visible = false;
                panel1.Visible = false;
            }
            else
            {
                REPARACION.Visible = false;
            }

        }
        //Cambios de Estado de reparación para las bicicletas
        private void bicicletasDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            if (bicicletasDataGridView.Columns[e.ColumnIndex].Name == "dataGridViewCheckBoxColumn2")
            {
                DataGridViewRow row = bicicletasDataGridView.Rows[e.RowIndex];
                DataGridViewCheckBoxCell cell = row.Cells["dataGridViewCheckBoxColumn2"] as DataGridViewCheckBoxCell;
                string id = row.Cells[0].Value.ToString();
                int ID = Int32.Parse(id);
                if (Convert.ToBoolean(cell.Value) == false)

                {
                    

                    DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea marcar en Reparación?, del ID = "+ID, "Problema!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (respuesta == DialogResult.Yes)
                    {
                        this.bicicletasTableAdapter.MODIFICAR_REPARACION(true,ID);
                        cell.Value = true;
                    }
                    else
                    {
                        this.bicicletasTableAdapter.MODIFICAR_REPARACION(false,ID);
                        cell.Value = false;
                    }
                }
                else
                {
                    DialogResult respuesta = MessageBox.Show("¿Ha sido reparada esta bicicleta?", "Problema!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (respuesta == DialogResult.Yes)
                    {
                        this.bicicletasTableAdapter.MODIFICAR_REPARACION(false, ID);
                        cell.Value = false;
                    }
                    else
                    {
                        cell.Value = true;
                    }
                }

            }

        }
        //variables para guardar la hora y minutos del arriendo de tal ID
        double minutosM;
        double horasH;
        double diferenciaM;
        double diferenciaH;
        double total;
        //variable para guardar el valor de tal cleta
        double valor;


        //BOTON TERMINAR ARRIENDO (boton que esta dento del PANEL TERMINAR ARRIENDO)
        private void button11_Click(object sender, EventArgs e)
        {
            try//rescata la hora de inicio
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string query = "select * from Arriendos Where ID =" + textBox2.Text;
                command.CommandText = query;
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    minutosM = Int32.Parse(reader["INICIO_MINUTO"].ToString());
                    horasH = Int32.Parse(reader["INICIO_HORA"].ToString());
                }
                connection.Close();
                ///////////////////////////////
                //rescatamos el valor
                connection.Open();
                command = new OleDbCommand();
                command.Connection = connection;
                query = "select * from Bicicletas Where ID =" + textBox2.Text;
                command.CommandText = query;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    valor = Int32.Parse(reader["VALOR"].ToString());
                   
                }
                connection.Close();

                panel4.Visible = false;
                string HORAs = hoy.ToShortTimeString();
                string hora;
                string minutos;
                if(HORAs[1] == ':')
                {
                    hora = HORAs[0].ToString();
                    minutos = HORAs[2].ToString() + HORAs[3].ToString();
                }
                else
                {
                    hora = HORAs[0].ToString()+HORAs[1].ToString();
                    minutos = HORAs[3].ToString() + HORAs[4].ToString();

                }
                double h = Int32.Parse(hora);
                double m = Int32.Parse(minutos);
                diferenciaM = (m+(h*60)-(horasH*60)-minutosM);
                total = valor * (diferenciaM / 60);
                double horasTotales = diferenciaM / 60;
                this.bicicletasTableAdapter.TERMINAR_ARRIENDO(Int32.Parse(textBox2.Text));
                this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);

                // aqui deberia actualizar el estado de arrendar de true a false
                //int b = Int32.Parse(textBox2.Text);
                //this.bicicletasTableAdapter.INICIAR_ARRIENDO(false, b);
                //this.bicicletasTableAdapter.Fill(this.dBDataSet.Bicicletas);
                MessageBox.Show(
                    "Formula = (valor*minuto)*(minutosActuales-minutosArriendo)"+
                    "\n"+valor/60+" * ("+(int)(h*60+m)+" - "+(int)(horasH*60+(minutosM))+" )"+
                    "\n Minutos totales = "+horasTotales*60+"      Valor Total = "+total+" pesos"
                    );



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

        }

        //BOTON TERMINAR  ARRIENDO (FORM1 osea es el boton que inicia el panel para terminar el arriendo )
        private void button10_Click(object sender, EventArgs e)
        {

            if (panel4.Visible == false)
            {
                textBox2.Clear();
                listBox3.Items.Clear();
                try
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    string query = "select DISTINCT * from Bicicletas where EN_ARRIENDO = true";
                    command.CommandText = query;

                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        listBox3.Items.Add(reader["ID"].ToString());
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }

                panel4.Visible = true;
                panel3.Visible = false;
                panel2.Visible = false;
                panel1.Visible = false;
                REPARACION.Visible = false;
            }
            else
            {
                panel4.Visible = false;
            }
        }

        //listbox3 que esta en el panel TERMINAR ARRIENDO
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                if (listBox3.Text !="")
                {
                    string query = "Select * from Bicicletas where ID =" + listBox3.Text;
                    command.CommandText = query;
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        textBox2.Text = reader["Id"].ToString();

                    }

                }
               
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show("Error" + ex);

            }
        }

        private void iDTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
    }
}

