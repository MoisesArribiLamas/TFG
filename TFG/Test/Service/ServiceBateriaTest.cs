using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Daos.BateriaDao;
using Es.Udc.DotNet.TFG.Model.Service.Util;
using Es.Udc.DotNet.TFG.Model;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.TFG.Model.Service.Tests
{
    [TestClass()]
    public class ServiceBateriaTest
    {
        private static IKernel kernel;
        private static IServiceBateria servicio;

        private static IBateriaDao bateriaDao;
        private static IUsuarioDao usuarioDao;
        private static IUbicacionDao ubicacionDao;


        //USUARIO

        public const string contraseña = "password";
        public const string nombre = "name";
        private const string apellido1 = "lastName";
        private const string apellido2 = "lastName";
        private const string email = "user@udc.es";
        private const string telefono = "123456789";
        private const string idioma = "es-ES";
        private const string pais = "Spain";


        public long crearUsuario(string nombre, string email, string apellido1, string apellido2, string contraseña
            , string telefono, string pais, string idioma)
        {
            Usuario user = new Usuario();
            user.nombre = nombre;
            user.email = email;
            user.apellido1 = apellido1;
            user.apellido2 = apellido2;
            user.contraseña = contraseña;
            user.telefono = telefono;
            user.pais = pais;
            user.idioma = idioma;

            usuarioDao.Create(user);

            return user.usuarioId;
        }
        /*
         
             */

        //UBICACION

        private const long codigoPostal = 15000;
        private const string localidad = "localidad";
        private const string calle = "calle";
        private const string portal = "portal";
        private const long numero = 1;

        // constructor de Ubicaciones
        public long crearUbicacion(long codigoPostal, string localidad, string calle, string portal, long numero)
        {
            Ubicacion t = new Ubicacion();
            t.codigoPostal = codigoPostal;
            t.localidad = localidad;
            t.calle = calle;
            t.portal = portal;
            t.numero = numero;

            ubicacionDao.Create(t);
            return t.ubicacionId;
        }

        // BATERIA

        private const double precioMedio = 100;
        private const double kwAlmacenados = 1000;
        private const double almacenajeMaximoKw = 2000;
        private DateTime fechaDeAdquisicion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private const string marca = "marca";
        private const string modelo = "modelo" ;
        private const double ratioCarga = 40;
        private const double ratioCompra = 50;
        private const double ratioUso = 45;




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
            servicio = kernel.Get<IServiceBateria>();
            bateriaDao = kernel.Get<IBateriaDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            ubicacionDao = kernel.Get<IUbicacionDao>();

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
        public void CrearBateriaTest()
        {
            using (var scope = new TransactionScope())
            {

                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion( codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria( ubicacionId,  usuarioId,  precioMedio,  kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion,  marca,  modelo,  ratioCarga,  ratioCompra,  ratioUso);


                var bateriaProfile = bateriaDao.Find(bateriaId);


                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(ubicacionId, bateriaProfile.ubicacionId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwAlmacenados, bateriaProfile.kwAlmacenados);
                Assert.AreEqual(almacenajeMaximoKw, bateriaProfile.almacenajeMaximoKw);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);

            }
        }

        [TestMethod()]
        public void ModificarBateriaTest()
        {
            using (var scope = new TransactionScope())
            {

                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //Modificamos datos

                long ubicacionId2 = crearUbicacion(codigoPostal, localidad, calle, portal, numero);
                long usuarioId2 = crearUsuario("nombre2", email, "apellido1", apellido2, contraseña, telefono, pais, idioma);
                double precioMedio2 = 2;
                double kwAlmacenados2 = 2;
                double almacenajeMaximoKw2 = 2;
                DateTime fechaDeAdquisicion2 = fechaDeAdquisicion.AddDays(1);

                string marca2 = "Marca2";
                string modelo2 = "Modelo2";
                double ratioCarga2 = 2;
                double ratioCompra2 = 2;
                double ratioUso2 = 2;

                servicio.ModificarBateria(bateriaId, ubicacionId2, usuarioId2, precioMedio2, kwAlmacenados2, almacenajeMaximoKw2,
             fechaDeAdquisicion2, marca2, modelo2, ratioCarga2, ratioCompra2, ratioUso2);

                //Comprobamos los cambios

                var bateriaProfile = bateriaDao.Find(bateriaId);


                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId2, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio2, bateriaProfile.precioMedio);
                Assert.AreEqual(kwAlmacenados2, bateriaProfile.kwAlmacenados);
                Assert.AreEqual(almacenajeMaximoKw2, bateriaProfile.almacenajeMaximoKw);
                Assert.AreEqual(fechaDeAdquisicion2, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca2, bateriaProfile.marca);
                Assert.AreEqual(modelo2, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga2, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra2, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso2, bateriaProfile.ratioUso);

            }
        }


      

        [TestMethod()]
        public void BuscarBateriaPorIdTest()
        {
            using (var scope = new TransactionScope())
            {

                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //Buscamos
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);


                //Comprobamos

                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwAlmacenados, bateriaProfile.kwAlmacenados);
                Assert.AreEqual(almacenajeMaximoKw, bateriaProfile.almacenajeMaximoKw);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);

            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void EliminarBateriaTest()
        {
            using (var scope = new TransactionScope())
            {

                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);

                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //Buscamos
                var bateriaProfile = servicio.BuscarBateriaById(bateriaId);

                //Comprobamos

                Assert.AreEqual(bateriaId, bateriaProfile.bateriaId);
                Assert.AreEqual(usuarioId, bateriaProfile.usuarioId);
                Assert.AreEqual(precioMedio, bateriaProfile.precioMedio);
                Assert.AreEqual(kwAlmacenados, bateriaProfile.kwAlmacenados);
                Assert.AreEqual(almacenajeMaximoKw, bateriaProfile.almacenajeMaximoKw);
                Assert.AreEqual(fechaDeAdquisicion, bateriaProfile.fechaDeAdquisicion);
                Assert.AreEqual(marca, bateriaProfile.marca);
                Assert.AreEqual(modelo, bateriaProfile.modelo);
                Assert.AreEqual(ratioCarga, bateriaProfile.ratioCarga);
                Assert.AreEqual(ratioCompra, bateriaProfile.ratioCompra);
                Assert.AreEqual(ratioUso, bateriaProfile.ratioUso);

                //Eliminamos
                servicio.EliminarBateria(bateriaId);

                //Buscamos //Comprobamos la exception
                var b = servicio.BuscarBateriaById(bateriaId);


            }
        }

        [TestMethod()]
        public void MostrarBateriasUsuarioTest()
        {
            using (var scope = new TransactionScope())
            {

                long usuarioId = crearUsuario(nombre, email, apellido1, apellido2, contraseña, telefono, pais, idioma);
                long usuarioId2 = crearUsuario("nombre2", "email2", apellido1, apellido2, "contraseña2", telefono, pais, idioma);


                // Creamos las ubicaciones
                long ubicacionId = crearUbicacion(codigoPostal, localidad, calle, portal, numero);
                long ubicacionId2 = crearUbicacion(codigoPostal, localidad, calle, "10B", numero);
                long ubicacionId3 = crearUbicacion(codigoPostal, localidad, calle, "11B", numero);



                // creamos las baterias
                long bateriaId = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId2 = servicio.CrearBateria(ubicacionId, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId3 = servicio.CrearBateria(ubicacionId2, usuarioId, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                long bateriaId4 = servicio.CrearBateria(ubicacionId3, usuarioId2, precioMedio, kwAlmacenados, almacenajeMaximoKw,
             fechaDeAdquisicion, marca, modelo, ratioCarga, ratioCompra, ratioUso);

                //Buscamos los baterias que pertenecen al usuario (dos en la misma residencia y una en otra)

                List<BateriaDTO> bateriasUsuario = servicio.VerBaterias(usuarioId, 0, 3);
                List<BateriaDTO> bateriasUsuario2 = servicio.VerBaterias(usuarioId2, 0, 3);


                //Comprobamos los cambios


                Assert.AreEqual(bateriaId, bateriasUsuario[0].bateriaId);
                Assert.AreEqual(bateriaId2, bateriasUsuario[1].bateriaId);
                Assert.AreEqual(bateriaId3, bateriasUsuario[2].bateriaId);

                Assert.AreEqual(bateriaId4, bateriasUsuario2[0].bateriaId);

            }
        }
    }
}
