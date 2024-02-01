﻿using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.EstadoDao;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Es.Udc.DotNet.TFG.Model.Service.Tarifas
{
    public interface IServiceTarifa
    {

        [Transactional]
        List<TarifaDTO> verTarifasDelDia(DateTime fecha);
        [Transactional]
        double BuscarMejorTarifa(DateTime fecha);
        [Transactional]
        double BuscarpeorTarifa(DateTime fecha);
        [Transactional]
        List<TarifaDTO> OrdenarMejorPrecioTarifasDelDia(DateTime fecha);
        [Transactional]
        List<TarifaDTO> OrdenarPeorPrecioTarifasDelDia(DateTime fecha);
        [Transactional]
        TarifaDTO TarifaActual(DateTime fecha, int hora);
        [Transactional]
        long crearTarifa(double precio, long hora, DateTime fecha);
        [Transactional]
        bool ExistenTarifasDelDia(DateTime fecha);
        [Transactional]
        void scrapyTarifas();

    }
}
