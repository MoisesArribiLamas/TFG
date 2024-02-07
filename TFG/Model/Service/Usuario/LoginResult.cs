using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    [Serializable()]
    public class LoginResult
    {


        public LoginResult(long userId, String nombre, String apellido1, String apellido2, String passEncriptada, String email, String telefono, string language, string country)
        {
            this.userId = userId;
            this.nombre = nombre;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
            this.passEncriptada = passEncriptada;
            this.email = email;
            this.telefono = telefono;
            this.Language = language;
            this.Country = country;

        }

        #region Properties Region


        public long userId { get; private set; }

        public String nombre { get; private set; }

        public String apellido1 { get; private set; }

        public String apellido2 { get; private set; }

        public String passEncriptada { get; private set; }

        public String email { get; private set; }

        public String telefono { get; private set; }

        public string Language { get; private set; }

        public string Country { get; private set; }



        #endregion Properties Region


        public override bool Equals(object obj)
        {
            LoginResult target = (LoginResult)obj;

            return (this.userId == target.userId)
                   && (this.nombre == target.nombre)
                   && (this.apellido1 == target.apellido1)
                   && (this.apellido2 == target.apellido2)
                   && (this.passEncriptada == target.passEncriptada)
                   && (this.email == target.email)
                   && (this.telefono == target.telefono);

        }

        // The GetHashCode method is used in hashing algorithms and data
        // structures such as a hash table. In order to ensure that it works
        // properly, it is based on a field that does not change.
        public override int GetHashCode()
        {
            return this.userId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the
        /// current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current
        /// <see cref="T:System.Object"></see>.
        /// </returns>
        public override String ToString()
        {
            String strLoginResult;

            strLoginResult =
                "[ userId = " + userId + " | " +
                "nombre = " + nombre + " | " +
                "apellidos = " + apellido1 + " " + apellido2 + " | " +
                "passEncriptada = " + passEncriptada + " | " +
                "email = " + email + " | " +
                "telefono = " + telefono + "]";

            return strLoginResult;
        }




    }
}
