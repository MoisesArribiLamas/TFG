using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Ubicaciones
{
    public interface IServiceUbicacion
    {


        #region crear Ubicación
        [Transactional]
        long crearUbicacion(long codigoPostal, string localidad, string calle, string portal, long numero, string etiqueta);

        #endregion

        [Transactional]
        void modificarUbicacion(long ubicacionId, long? codigoPostal, string localidad, string calle, string portal, long? numero, string etiqueta);

        [Transactional]
        void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora);

        [Transactional]
        List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count);

        [Transactional]
        Ubicacion buscarUbicacionById(long ubicacionId);

        [Transactional]
        long crearConsumo(long ubicacionId, double consumoActual);

        [Transactional]
        double calcularConsumo(double consumoActual, TimeSpan fechaIni, TimeSpan fechaFin);

        [Transactional]
        void finalizarConsumo(long ubicacionId, double consumoActual);

        //modificar consumo
        //finalizar consumo
        //

        /*
        [Transactional]
        void eliminarUbicacion(long ubicacionId, UbicacionProfileDetails ubicacionProfileDetails)
        */

    }
}
