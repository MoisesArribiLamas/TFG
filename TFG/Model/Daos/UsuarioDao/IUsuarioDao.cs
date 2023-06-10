using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao
{
    public interface IUsuarioDao : IGenericDao<Usuario, Int64>
    {

        /// <exception cref="InstanceNotFoundException"/>
        Usuario findUserByName(String username);

        //usuario login(String usuario, String contraseña);

        bool updateInformacion(long userId, string nombre, string apellido1, string apellido2, string contraseña, string tlf, string email, string language, string country);



    }
}
