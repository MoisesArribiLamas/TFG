using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class UserProfileDetails
    {
        public UserProfileDetails(string email, string nombre, string apellido1, string apellido2,string telefono, string language, string country)
        {
            this.Email = email;
            this.Nombre = nombre;
            this.Apellido1 = apellido1;
            this.Apellido2 = apellido2;
            this.Telefono = telefono;
            this.Language = language;
            this.Country = country;
           
        }

        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }

        public string Language { get; private set; }

        public string Telefono { get; private set; }

        public string Country { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as UserProfileDetails;
            return details != null &&
                   Nombre == details.Nombre &&
                   Apellido1 == details.Apellido1 &&
                   Apellido2 == details.Apellido2
                   && (this.Language == details.Language)
                 && (this.Country == details.Country);
        }

        public override int GetHashCode()
        {
            var hashCode = -801872377;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Nombre);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Apellido1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Apellido2);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }

      
    }
}
