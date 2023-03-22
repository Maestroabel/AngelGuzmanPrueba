using PruebaCRUD.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
            
            /*
            Admin Dummy para entrar en el login. (En un proyecto real, se le
            agregaría un register, pero por corto tiempo, se creó de esta forma.) 
             
            using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
            {
                Usuario Admin = new Usuario();
                Admin.Nombre = "Admin";
                Admin.Password = HashContra("1234");

                db.Usuario.Add(Admin);
                db.SaveChanges();
            }
            */
        }
        /// <summary>
        /// Sirve para realizar un Hashcode a un string. Como parámetro,
        /// necesita un string que servirá como contraseña.
        /// </summary>
        /// <param name="contra"></param>
        /// <returns></returns>
        private static string HashContra(string contra)
        {
            var sha = SHA256.Create();
            var ByteArreglo = Encoding.Default.GetBytes(contra);
            var contraHash = sha.ComputeHash(ByteArreglo);
            return Convert.ToBase64String(contraHash);
        }
        /// <summary>
        /// Verifica si el formulario está correcto para poder ingresar. Como parámetros,
        /// requiere 2 strings, que servirán como el nombre y la contraseña.
        /// </summary>
        /// <param name="nombre_"></param>
        /// <param name="contra_"></param>
        /// <returns></returns>
        private bool UsuarioEsValido(string nombre_, string contra_)
        {
            string contra = HashContra(contra_);

            using (AngelPruebaDBEntities db = new AngelPruebaDBEntities())
            {
                if(string.IsNullOrEmpty(nombre_) || string.IsNullOrEmpty(contra))
                    return false;

                return db.Usuario.Any(x => ((x.Nombre == nombre_) && (x.Password == contra)));
            }
        }
        /// <summary>
        /// Evento clickeable: Al verificar el formulario, ingresa al "MenuForm".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            /*Si no se coloca el nombre "Admin" y la contraseña "123", colocará un mensaje de error.
             En cambio, si las 2 son correctas, cerrará el LoginForm y abrirá el Menuform.
             */
            if (!UsuarioEsValido(txtNombre.Text,txtContra.Text))
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
