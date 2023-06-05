using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.PracticaMaD.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.PracticaMaD.Test;
using Es.Udc.DotNet.PracticaMaD.Model.PedidoDao;
using Es.Udc.DotNet.PracticaMaD.Model.ProductoDao;
using Es.Udc.DotNet.PracticaMaD.Model.StockDao;
using Es.Udc.DotNet.PracticaMaD.Model.CategoriaDao;
using Es.Udc.DotNet.PracticaMaD.Model.LibroDao;
using Es.Udc.DotNet.PracticaMaD.Model.Tarjeta_creditoDao;
using Es.Udc.DotNet.PracticaMaD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMaD.Model.Service.Util;
using Es.Udc.DotNet.PracticaMaD.Model.Linea_pedidoDao;
using Es.Udc.DotNet.PracticaMaD.Model.ComentariosDao;
using Es.Udc.DotNet.PracticaMaD.Model.EtiquetasDao;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.PracticaMaD.Model.Service.Tests
{
    [TestClass()]
    public class ServiceComentariosTest
    {
        private static IKernel kernel;
        private static IServiceComentarios servicio;


        private static IPedidoDao pedidoDao;
        private static IProductoDao productoDao;
        private static IStockDao stockDao;
        private static ICategoriaDao categoriaDao;
        private static ILibroDao libroDao;
        private static ITarjeta_creditoDao tarjeta_CreditoDao;
        private static IUsuarioDao usuarioDao;
        private static ILinea_pedidoDao linea_PedidoDao;
        private static IComentarioDao comentarioDao;
        private static IEtiquetaDao etiquetaDao;


        public const string clearPassword = "password";
        public const string firstName = "name";
        private const string lastName = "lastName";
        private const string email = "user@udc.es";
        private const int codigoPostal = 15007;

        private const long NON_EXISTENT_USER_ID = -1;

        // Variables used in several tests are initialized here
        private const long idLibro = 1;

        private const long NON_EXISTENT_LIBRO_ID = -2;

        private TransactionScope transactionScope;


        private TestContext testContextInstance;

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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();
            servicio = kernel.Get<IServiceComentarios>();
            pedidoDao = kernel.Get<IPedidoDao>();
            stockDao = kernel.Get<IStockDao>();
            productoDao = kernel.Get<IProductoDao>();
            categoriaDao = kernel.Get<ICategoriaDao>();
            libroDao = kernel.Get<ILibroDao>();
            usuarioDao = kernel.Get<IUsuarioDao>();
            tarjeta_CreditoDao = kernel.Get<ITarjeta_creditoDao>();
            linea_PedidoDao = kernel.Get<ILinea_pedidoDao>();
            comentarioDao = kernel.Get<IComentarioDao>();
            etiquetaDao = kernel.Get<IEtiquetaDao>();

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
        public void anhadirComentarioAProductoTest()
        {
            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1234568";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            Carrito carrito = new Carrito();

            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);

            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO
            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            libroDao.Create(libro);
            List<string> etqs = new List<string>();
            etqs.Add("ganga");

            servicio.anhadirComentarioAProducto(libro.id_producto, user.id_usuario, "prueba", etqs);

            List<comentarios> comentarioInsertado = comentarioDao.findComentariosByProduct(libro.id_producto, 0, 1);


            comentarios comentario = new comentarios();
            comentario.fecha = DateTime.Now;
            comentario.id_comentario = comentarioInsertado[0].id_comentario;
            comentario.producto = libro.id_producto;
            comentario.usuario = user.id_usuario;
            comentario.contenido = "prueba";

            Assert.AreEqual(comentario, comentarioInsertado[0]);
        }


        [TestMethod()]
        public void etiquetarComentarioTest()
        {

            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo2@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1234541";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            Carrito carrito = new Carrito();

            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);

            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO
            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            libroDao.Create(libro);
            List<string> etqs = new List<string>();
            etqs.Add("ganga2");

            comentarios comentario = new comentarios();
            comentario.producto = libro.id_producto;
            comentario.usuario = user.id_usuario;
            comentario.fecha = DateTime.Now;
            comentario.contenido = "ok";
            comentario.etiquetas = new List<etiquetas>();
            comentarioDao.Create(comentario);
            servicio.etiquetarComentario(etqs, comentario.id_comentario);

            comentarios comentarioInsertado = comentarioDao.Find(comentario.id_comentario);
            Assert.AreEqual(comentario, comentarioInsertado);
        }


        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void borrarComentarioTest()
        {

            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo3@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1233541";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            Carrito carrito = new Carrito();

            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);

            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO
            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            libroDao.Create(libro);
            List<string> etqs = new List<string>();
            etqs.Add("ganga3");

            comentarios comentario = new comentarios();
            comentario.producto = libro.id_producto;
            comentario.usuario = user.id_usuario;
            comentario.fecha = DateTime.Now;
            comentario.contenido = "ok";
            comentario.etiquetas = new List<etiquetas>();
            comentarioDao.Create(comentario);

            servicio.borrarComentario(comentario.id_comentario);
            comentarioDao.Find(comentario.id_comentario);


        }


        [TestMethod()]
        public void contarComentarios()
        {

            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo5@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1233641";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            Carrito carrito = new Carrito();

            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);

            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO
            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            libroDao.Create(libro);
            List<string> etqs = new List<string>();
            etqs.Add("ganga4");

            comentarios comentario = new comentarios();
            comentario.producto = libro.id_producto;
            comentario.usuario = user.id_usuario;
            comentario.fecha = DateTime.Now;
            comentario.contenido = "ok";
            comentario.etiquetas = new List<etiquetas>();
            comentarioDao.Create(comentario);

            Assert.AreEqual(1, servicio.contarComentarios(libro.id_producto));
                       
        }

        [TestMethod()]
        public void modificarComentarioTest()
        {

            usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo6@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1233611";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            Carrito carrito = new Carrito();

            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);

            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO
            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            libroDao.Create(libro);
            List<string> etqs = new List<string>();
            etqs.Add("ganga5");

            comentarios comentario = new comentarios();
            comentario.producto = libro.id_producto;
            comentario.usuario = user.id_usuario;
            comentario.fecha = DateTime.Now;
            comentario.contenido = "ok";
            comentario.etiquetas = new List<etiquetas>();
            comentarioDao.Create(comentario);

            comentarios comentario2 = new comentarios();
            comentario2.producto = libro.id_producto;
            comentario2.usuario = user.id_usuario;
            comentario2.fecha = DateTime.Now;
            comentario2.contenido = "No ok";
            comentario2.etiquetas = new List<etiquetas>();
            comentarioDao.Create(comentario);
            List<string> etiqu = new List<string>();
            etiqu.Add("ganga5");


            comentarios coment = comentarioDao.Find(comentario.id_comentario);

            servicio.modificarComentario(coment.id_comentario, "No ok", etiqu);

            Assert.AreEqual(comentario2.contenido, comentario.contenido);

        }

    }
}

