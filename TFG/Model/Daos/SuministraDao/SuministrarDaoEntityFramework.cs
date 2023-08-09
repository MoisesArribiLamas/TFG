using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao
{
    public class SuministraDaoEntitFramework : GenericDaoEntityFramework<Suministra, Int64>, ISuministraDao
    {
        
        public Suministra getInfoSuministra(long suministraId)
        {
            DbSet<Suministra> suministraciones = Context.Set<Suministra>();

            var result =
                (from s in suministraciones
                 where s.suministraId == suministraId
                 select s).FirstOrDefault();

            return result;
        }

        #region suministros en un perriodo de tiempo
        public List<Suministra> MostrarSuministrosBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count)
        {
            DbSet<Suministra> suministros = Context.Set<Suministra>();

            var result =
                (from s in suministros
                 where ((s.Tarifa.fecha >= fecha) && (s.Tarifa.fecha <= fecha2) && (s.bateriaId == bateriaId))
                 select s).OrderBy(s => s.horaIni).Skip(startIndex).Take(count).ToList();

            return result;
        }
        #endregion

        #region ahorro bateria entre fechas
        public double ahorroBareriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2)
        {
            DbSet<Suministra> suministros = Context.Set<Suministra>();
            double MontoInical = 0;

            var result =
                (from s in suministros
                 where ((s.Tarifa.fecha >= fecha) && (s.Tarifa.fecha <= fecha2) && (s.bateriaId == bateriaId))
                 select s).ToList();

            double Montofinal = MontoInical + result.Sum(x => x.ahorro);

            return Montofinal;
        }
        #endregion

        #region ahorro usuario entre fechas
        public double ahorroUsuarioPorFecha(long usuarioId, DateTime fecha, DateTime fecha2)
        {
            DbSet<Suministra> suministros = Context.Set<Suministra>();
            double MontoInical = 0;

            var result =
                (from s in suministros
                 where ((s.Tarifa.fecha >= fecha) && (s.Tarifa.fecha <= fecha2) && (s.Bateria.usuarioId == usuarioId))
                 select s).ToList();

            double Montofinal = MontoInical + result.Sum(x => x.ahorro);

            return Montofinal;
        }
        #endregion
    }
}
