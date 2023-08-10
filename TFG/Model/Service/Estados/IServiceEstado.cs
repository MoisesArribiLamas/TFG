﻿using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Estados
{
    public interface IServiceEstado
    {
        [Transactional]
        long CrearEstadoBateria(TimeSpan horaIni, TimeSpan horaFin, DateTime fecha, long bateriaId, long estadoId);
        [Transactional]
        List<EstadoDTO> verTodosLosEstados();
        [Transactional]
        List<SeEncuentraDTO> MostrarEstadoBateriaPorFecha(long bateriaId, DateTime fecha, DateTime fecha2, int startIndex, int count);
    }   
}
