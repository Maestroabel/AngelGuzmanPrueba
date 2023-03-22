using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaCRUD
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            //Inicializar el formulario LoginForm
            InitializeComponent();
        }

        
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            /*Si no se coloca el nombre "Admin" y la contraseña "123", colocará un mensaje de error.
             En cambio, si las 2 son correctas, cerrará el LoginForm y abrirá el Menuform.
             */
            if (txtNombre.Text != "Admin" || txtContra.Text != "123")
            {
                MessageBox.Show("Usuario o contraseña incorrecta", "Fallo de login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ActiveForm.Hide();
            MenuForm menu = new MenuForm();
            menu.Show();
            
        }
    }
}
