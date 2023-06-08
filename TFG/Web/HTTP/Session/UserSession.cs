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
        private String tipo_usuario;
        private String direccionPostal;

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
    }
}