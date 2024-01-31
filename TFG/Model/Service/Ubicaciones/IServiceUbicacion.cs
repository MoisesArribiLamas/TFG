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
        double obtenerCapacidadCargadorBateriaSuministradora(long ubicacionId);

        [Transactional]
        void CambiarBateriaSuministradora(long ubicacionId, long? bateriaSuministradora);

        [Transactional]
        List<UbicacionProfileDetails> verUbicaciones(long idUsuario, int startIndex, int count);

        [Transactional]
        Ubicacion buscarUbicacionById(long ubicacionId);

        [Transactional]
        long crearConsumo(long ubicacionId, double consumoActual, TimeSpan horaActual);

        [Transactional]
        double calcularConsumo(double consumoActual, TimeSpan fechaIni, TimeSpan fechaFin);

        [Transactional]
        bool finalizarConsumo(long ubicacionId, double consumoActual, TimeSpan horaActual, string estado, long bateriaSuministradora);

        [Transactional]
        long modificarConsumoActual(long ubicacionId, double consumoActual);

        [Transactional]
        long? UltimoConsumoEnUbicacion(long ubicacionId);

        [Transactional]
        Consumo buscarConsumoById(long consumoId);

        [Transactional]
        void eliminarUbicacion(long ubicacionId);
       

    }
}
