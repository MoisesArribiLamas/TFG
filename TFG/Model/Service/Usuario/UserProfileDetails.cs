using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class UserProfileDetails
    {
        public UserProfileDetails(string email, string nombre, string apellido1, string apellido2, string language, string country)
        {
            this.email = email;
            this.nombre = nombre;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
            this.Language = language;
            this.Country = country;
           
        }

        public string email { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string tipo_usuario { get; set; }

        public string Language { get; private set; }

        public string Country { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as UserProfileDetails;
            return details != null &&
                   nombre == details.nombre &&
                   apellido1 == details.apellido1 &&
                   apellido2 == details.apellido2
                   && (this.Language == details.Language)
                 && (this.Country == details.Country);
        }

        public override int GetHashCode()
        {
            var hashCode = -801872377;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(nombre);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(apellido1);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(apellido2);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }

      
    }
}
