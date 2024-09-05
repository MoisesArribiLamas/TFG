using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

            LBUsuarios.Visible = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tbConsumo.Text = trackBar1.Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (tbConsumo.Text != "")
            {
                trackBar1.Value = Convert.ToInt32(tbConsumo.Text);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double consumoNuevo = Convert.ToDouble(tbConsumo.Text);

            
            WSConsumo.ServicioWebSoapClient cliente = new WSConsumo.ServicioWebSoapClient();

            //Modificamos el consumo
            cliente.ModificarConsumo(consumoNuevo,LBUbicaciones.SelectedItem.ToString());
           
        }

        private void LBUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            WSConsumo.ServicioWebSoapClient cliente = new WSConsumo.ServicioWebSoapClient();

            LBUbicaciones.Items.Clear();
            // obtenemos las ubicaciones
            List<string> ubicaciones = cliente.MostrarUbicacionesDelUsuario(LBUsuarios.SelectedItem.ToString());

            // Usuarios 
            foreach (string u in ubicaciones)
            {
                LBUbicaciones.Items.Add(u);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WSConsumo.ServicioWebSoapClient cliente = new WSConsumo.ServicioWebSoapClient();

            // obtenemos los usuarios
            List<string> usuarios = cliente.MostrarUsuarios();

            // Usuarios 
            foreach (string u in usuarios)
            {
                this.LBUsuarios.Items.Add(u);
            }
        }

        private void LBUbicaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            WSConsumo.ServicioWebSoapClient cliente = new WSConsumo.ServicioWebSoapClient();

            //Ponemos el consumo de la ubicación seleccionada

            tbConsumo.Text = cliente.ConsumoEnEsteInstante(LBUbicaciones.SelectedItem.ToString()).ToString();

            trackBar1.Value = Convert.ToInt32(tbConsumo.Text);
            
        }
    }
}
