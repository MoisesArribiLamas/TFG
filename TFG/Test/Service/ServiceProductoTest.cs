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
    public class ServiceProductoTest
    {
        private static IKernel kernel;
        private static IServiceProducto servicio;
        

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
            servicio = kernel.Get<IServiceProducto>();
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
        public void actualizarProductoTest()
        {
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);



            libro libro = new libro();
            libro.nombre = "LibroTest";

            libro.isbn = "123123";
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;
            productoDao.Create(libro);

            int count = 1;
            int startOfIndex = 0;
            List<producto> prod2 = productoDao.findProductoByName("Libro", libro.categoria, startOfIndex, count);


            servicio.actualizarProducto(libro.id_producto,
                libro.nombre, 13.00);


            prod2[0].precio = 13.00;

            Assert.AreEqual(prod2[0], productoDao.findProductoByName("Libro", libro.categoria, startOfIndex, count)[0]);
        }

        [TestMethod()]
        public void ObtenerCategorias()
        {

            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);


            categoria cat2 = new categoria();
            cat2.nombre = "Peli";
            categoriaDao.Create(cat2);

            List<CategoriaDTO> obtained = servicio.getAllCategories();
            List<CategoriaDTO> expected = new List<CategoriaDTO>();
            expected.Add(new CategoriaDTO(cat.nombre));
            expected.Add(new CategoriaDTO(cat2.nombre));

            Assert.AreEqual(expected[0], obtained[0]);
            Assert.AreEqual(expected[1], obtained[1]);
        }



            [TestMethod()]
        public void buscarProductosTest()
        {
            //CREAMOS LA CATEGORIA
            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);



            libro libro = new libro();
            libro.nombre = "LibroTest";
            libro.isbn = "123";

            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;

            productoDao.Create(libro);

            int count = 1;
            int startOfIndex = 0;

            ProductoDTO libroDTO = new ProductoDTO(libro.id_producto, libro.nombre, libro.precio, libro.categoria1.nombre, libro.fecha_alta);


            List<ProductoDTO> prod3 = servicio.buscarProductos("LibroTest", libro.categoria1.nombre, count, startOfIndex);
            List<ProductoDTO> prod2 = servicio.buscarProductos("LibroTest", "Cualquiera", count, startOfIndex);


            Assert.AreEqual(prod3[0], prod2[0]);
        }

        
      [TestMethod()]
      public void getInfoDetalladaProductoTest()
      {
          //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO

          categoria cat = new categoria();
          cat.nombre = "Libro";
          categoriaDao.Create(cat);



          libro libro = new libro();
          libro.nombre = "LibroTest";
          libro.isbn = "123";
          libro.fecha_alta = DateTime.Now;
          libro.categoria = cat.id_categoria;
          libro.precio = 12.00;

          productoDao.Create(libro);


            ProductoDetails esperado = new ProductoDetails();
            esperado.nombre = libro.nombre;
            esperado.fecha_alta = libro.fecha_alta;
            esperado.categoria = libro.categoria1.nombre;
            esperado.precio = libro.precio;
            esperado.informacion.Add(new Tuple<string, string>("isbn", "123"));


          ProductoDetails libro2 = servicio.getInfoDetalladaProducto(libro.id_producto);

          Assert.AreEqual(esperado, libro2);
      }


        [TestMethod()]
        public void contarProductosTest()
        {
            //CREAMOS UN LIBRO, QUE AÑADIRÁ DATOS EN LA TABLA DE PRODUCTO Y EN LA DE LIBRO

            categoria cat = new categoria();
            cat.nombre = "Libro";
            categoriaDao.Create(cat);



            libro libro = new libro();
            libro.nombre = "LibroTest";
            libro.isbn = "123";
            libro.fecha_alta = DateTime.Now;
            libro.categoria = cat.id_categoria;
            libro.precio = 12.00;

            productoDao.Create(libro);


                       

            Assert.AreEqual(1, servicio.GetNumberOfProducts(libro.nombre, "Libro"));
        }


    }
}