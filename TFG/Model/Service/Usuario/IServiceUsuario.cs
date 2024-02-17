using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service
{


    public interface IServiceUsuario

    {
        #region daos
        [Inject]
        IUsuarioDao UsuarioDao { set; }
        #endregion
        #region
        [Transactional]
        long registrarUsuario( string clearPassword, UserProfileDetails userProfileDetails);

        [Transactional]
        void modificarUsuario(long usuarioId, UserProfileDetails userProfileDetails  );
        [Transactional]
        void modificarContraseña(long usuarioId, string viejaPass , string nuevaPass);

        [Transactional]
        LoginResult logearUsuario(String email, String pass, bool passwordIsEncrypted);
        #endregion

        [Transactional]
        UserProfileDetails BuscarUsuarioPorID(long usuarioId);
    }
}
