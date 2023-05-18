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

namespace Es.Udc.DotNet.PracticaMaD.Model.Service.Tests
{
    [TestClass()]
    public class ServiceCarritoTest
    {

        private static IKernel kernel;
        private static IServiceCarrito servicio;


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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();
            servicio = kernel.Get<IServiceCarrito>();
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
        public void verCarritoTest()
        {

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

            LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio, false);

            carrito.productos.Add(lc);

            List<LineaCarrito> carrito2 = servicio.verCarrito(carrito,1,0);

            Assert.AreEqual(carrito.productos[0].idProducto, carrito2[0].idProducto);

        }

        [TestMethod()]
        public void addToCarritoTest()
        {
     
                Carrito carrito2 = new Carrito();

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
                stock s = new stock();
                s.producto = libro.id_producto;
                s.unidades_Stock = 10;
                stockDao.Create(s);



                LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio, false);

                carrito2.productos.Add(lc);

                servicio.addToCarrito(libro.id_producto, libro.nombre, 1, false, carrito);

                Assert.AreEqual(carrito, carrito2);

            }

            [TestMethod()]
        public void removeFromCarritoTest()
        {
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
            stock s = new stock();
            s.producto = libro.id_producto;
            s.unidades_Stock = 10;
            stockDao.Create(s);


            Carrito carrito = new Carrito();
            LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio, false);
            carrito.productos.Add(lc);

            Carrito carrito2 = new Carrito();


            servicio.removeFromCarrito(lc, carrito);

            Assert.AreEqual(carrito, carrito2);
        }

        [TestMethod()]
        public void modificarLineaCarritoTest()
        {
            Carrito carrito2 = new Carrito();

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
            stock s = new stock();
            s.producto = libro.id_producto;
            s.unidades_Stock = 10;
            stockDao.Create(s);

            LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio, false);

            carrito.productos.Add(lc);

            servicio.modificarLineaCarrito(lc, 2, true, carrito);

            Assert.AreEqual(2,lc.numeroUnidades);
        }

    
    
    }
}