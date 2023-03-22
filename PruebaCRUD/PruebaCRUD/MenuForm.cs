using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PruebaCRUD.Models;

namespace PruebaCRUD
{
    public partial class MenuForm : Form
    {
        //Objeto persona para mayor control
        private Persona persona = null;
        //Variable booleana para saber si se esta agregando una persona o se esta modificando.
        public bool Agregando { get; set; }
        public MenuForm()
        {
            /*Se inicializa el formulario MenuForm, se obtienen todos los registros y se le asigna el valor "false" a la 
             variable booleana "Agregando"
             */
            InitializeComponent();

            ObtenerData();
            Agregando = false;
        }
        
        /// <summary>
        /// Se limpian todos los datos y se deja todo como estaba al iniciar el formulario.
        /// </summary>
        private void LimpiarDatos()
        {
            txtBuscar.Text = string.Empty;
            txtID.Text = string.Empty;
            txtNombre.Text = string.Empty;
            dtpFechaDeNacimiento.Value = DateTime.Now;
            
            txtNombre.ReadOnly = true;
            dtpFechaDeNacimiento.Enabled = false;
            
            btnCancelar.Enabled = true;
            btnGuardar.Enabled = false;
            btnNuevo.Enabled = true;
            btnEliminar.Enabled = false;
            dataGridView.Enabled = true;
        }

        /// <summary>
        /// Encuentra todos los registros que existen en la tabla Persona. Si se le añade
        /// un string, busca en la tabla persona los nombres que se asemejen al string. 
        /// </summary>
        /// <param name="buscarNombre"></param>
        private void ObtenerData(string buscarNombre = "")
        {        
            try
            {
                using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
                {
                    var lista = db.Persona.ToList();

                    if (formEsValido(buscarNombre))
                    {
                        lista = lista.Where(x => x.Nombre.Contains(buscarNombre)).ToList();
                    }

                    dataGridView.DataSource = lista;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return;
            }
        }

        /// <summary>
        /// Muestra el registro con el parámetro colocado.
        /// </summary>
        /// <param name="ID"></param>
        private void ObtenerDataPorID(string ID)
        {
            try
            {
                int personaID = int.Parse(ID);
                using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
                {
                    var persona = db.Persona.FirstOrDefault(x => x.ID == personaID);

                    if (persona == null) { return; }

                    txtID.Text = persona.ID.ToString();
                    txtNombre.Text = persona.Nombre.ToString();
                    dtpFechaDeNacimiento.Value = persona.FechaDeNacimiento;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return;
            }        
        }
        /// <summary>
        /// Revisa si el formulario es válido. Para que el formulario sea válido, se requiere que
        /// contenga, por lo menos, un carácter (sin contar espacios) y que el nombre escrito no esté registrado.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        private bool formEsValido(string nombre = "")
        {         
            string nombreSinEspacios = nombre.Trim();
            return !string.IsNullOrEmpty(nombreSinEspacios);
        }
        /// <summary>
        /// Agrega o modifica un registro.
        /// </summary>
        public void Guardar()
        {
            if (formEsValido(txtNombre.Text))
            {
                AgregarYmodificar();
                return;
            }

            MessageBox.Show("El campo nombre está vacío.");
        }
        /// <summary>
        /// Verifica si se quiere agregar o modificar un registro y lo realiza.
        /// </summary>
        private void AgregarYmodificar()
        {
            try
            {
                persona = new Persona();

                using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
                {
                    if (Agregando)
                    {
                        db.Entry<Persona>(persona).State = EntityState.Added;
                    }
                    else
                    {
                        int personaID = int.Parse(dataGridView.Rows[dataGridView.CurrentRow.Index].Cells[0].Value.ToString());
                        persona = db.Persona.FirstOrDefault(x => x.ID == personaID);
                    }

                    persona.Nombre = txtNombre.Text.Trim();
                    persona.FechaDeNacimiento = dtpFechaDeNacimiento.Value;

                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Type type = ex.GetType();
                MessageBox.Show($"{ex.Message}. El nombre ya existe dentro de los registros");
                return;
            }
            

            ObtenerData();
            LimpiarDatos();

            string mensajeExito = Agregando ? "agregado" : "actualizado";
            MessageBox.Show($"El registro ha sido {mensajeExito} correctamente.", mensajeExito, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        /// <summary>
        /// Evento clickeable: Habilita el formulario para que se pueda crear un nuevo registro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            dtpFechaDeNacimiento.Value = DateTime.Now;
            
            txtNombre.ReadOnly = false;
            dtpFechaDeNacimiento.Enabled = true;
            
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnNuevo.Enabled = false;
            dataGridView.Enabled = false;
            
            Agregando = true;
        }
        /// <summary>
        /// Evento clickeable: realiza el método de guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        /// <summary>
        /// Evento clickeable: regresa todo a como estaba en el principio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Agregando = false;

            ObtenerData();
            LimpiarDatos();
        }
        /// <summary>
        /// Evento Clickeable: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Estas seguro que deseas eliminar este registro?", "Eliminar archivo", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            
            try
            {
                using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
                {
                    if (dr == DialogResult.No) { return; }

                    int personaID = int.Parse(txtID.Text);
                    var persona = db.Persona.FirstOrDefault(x => x.ID == personaID);

                    if (persona == null) { return; }

                    db.Persona.Remove(persona);
                    db.SaveChanges();

                    ObtenerData();
                    LimpiarDatos();
                    Agregando = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return;
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ObtenerData(txtBuscar.Text);
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) { return; }

            txtNombre.ReadOnly = false;
            dtpFechaDeNacimiento.Enabled = true;

            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = true;
            btnNuevo.Enabled = false;

            Agregando = false;
            
            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            var ID = row.Cells[0].Value.ToString();
            ObtenerDataPorID(ID);
        }
    }
}
