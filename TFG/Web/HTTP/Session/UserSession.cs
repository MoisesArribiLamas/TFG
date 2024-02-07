using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Es.Udc.DotNet.TFG.Web.HTTP.Session
{
    public class UserSession
    {
        private long userProfileId;
        private String firstName;
        private String apellido1;
        private String apellido2;
        private String email;
        private String telefono;
        private String idioma;
        private String pais;


        public long UserProfileId
        {
            get { return userProfileId; }
            set { userProfileId = value; }
        }

        public String FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String Apellido1
        {
            get { return apellido1; }
            set { apellido1 = value; }
        }
        public String Apellido2
        {
            get { return apellido2; }
            set { apellido2 = value; }
        }
        public String Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        public String Idioma
        {
            get { return idioma; }
            set { idioma = value; }
        }
        public String Pais
        {
            get { return pais; }
            set { pais = value; }
        }
    }
}