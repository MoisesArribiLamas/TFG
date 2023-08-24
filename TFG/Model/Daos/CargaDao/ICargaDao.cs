using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.CargaDao
{
    public interface ICargaDao : IGenericDao<Carga, Int64>
    {
        Carga getInfoCarga(long cargaId);
        List<Carga> MostrarCargasBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);
        Carga UltimaCargaBareria(long bateriaId);
        bool FinalizarCarga(long cargaID, TimeSpan horaFin, double kws);
    }
}
