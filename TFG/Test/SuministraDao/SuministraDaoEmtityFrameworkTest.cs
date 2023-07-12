﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.TarifaDao;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;

namespace Es.Udc.DotNet.TFG.Model.Daos.SuministraDao.Tests
{
    [TestClass()]
    public class SuministraDaoEntityFrameworkTests
    {

        private static IKernel kernel;
        private static IBateriaDao bateriaDao;
        private static IUbicacionDao ubicacionDao;
        private static IUsuarioDao usuarioDao;
        private static ISuministraDao suministraDao;
        private static ITarifaDao tarifaDao;


        // Variables used in several tests are initialized here

        private TransactionScope transactionScope;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        private TestContext testContextInstance;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();
            bateriaDao = kernel.Get<IBateriaDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            suministraDao = kernel.Get<ISuministraDao>();
            tarifaDao = kernel.Get<ITarifaDao>();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearNInjectKernel(kernel);
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            transactionScope = new TransactionScope();
        }


        [TestCleanup()]
        public void MyTestCleanup()
        {
            transactionScope.Dispose();
        }

        [TestMethod()]
        public void getInfoSuministraTest()
        {
            // CREAMOS UBICACION
            Ubicacion u = new Ubicacion();
            u.codigoPostal = 15405;
            u.localidad = "Ferrol";
            u.calle = "calle de Ferrol";
            u.portal = 1;
            u.numero = 1;
            ubicacionDao.Create(u);


            //CREAMOS LOS USUARIO
            Usuario user = new Usuario();
            user.nombre = "Dani";
            user.email = "micorreo@gmail.com";
            user.apellido1 = "Díaz";
            user.apellido2 = "González";
            user.contraseña = "unacontraseña";
            user.telefono = "981123456";
            user.pais = "España";
            user.idioma = "es-ES";
            usuarioDao.Create(user);


            //CREAMOS LA BATERIA
            Bateria b = new Bateria();
            b.ubicacionId = u.ubicacionId;
            b.usuarioId = user.usuarioId ;
            b.precioMedio = 111 ;
            b.kwAlmacenados = 1000 ;
            b.almacenajeMaximoKw = 1000 ;
            b.fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            b.marca = "MARCA 1" ;
            b.modelo = "MODELO 1" ;
            b.ratioCarga = 10 ;
            b.ratioCompra = 10 ;
            b.ratioUso = 10;
            bateriaDao.Create(b);

            //CREAMOS LA TARIFA
            Tarifa t = new Tarifa();
            t.precio = 100;
            t.hora = 1;
            t.fecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tarifaDao.Create(t);

            //CREAMOS Suministrar
            Suministra c = new Suministra();
            c.kws = 1000;
            c.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            c.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, DateTime.Now.Second);
            c.ahorro = 100;
            c.tarifaId = t.tarifaId;
            c.bateriaId = b.bateriaId;
            suministraDao.Create(c);

            Suministra c2 = new Suministra();
            c2.kws = 1000;
            c2.horaIni = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, DateTime.Now.Second);
            c2.horaFin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(3).Minute, DateTime.Now.Second);
            c.ahorro = 200;
            c2.tarifaId = t.tarifaId;
            c2.bateriaId = b.bateriaId;
            suministraDao.Create(c2);


            //COMPROBAMOS
            Suministra cargaResult = suministraDao.getInfoSuministra(c.suministraId);
            Suministra cargaResult2 = suministraDao.getInfoSuministra(c2.suministraId);

            Assert.AreEqual(cargaResult, c);
            Assert.AreEqual(cargaResult2, c2);

        }

    }
}