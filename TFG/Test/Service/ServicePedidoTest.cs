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
using Es.Udc.DotNet.PracticaMaD.Model.Service.Pedidos;

namespace Es.Udc.DotNet.PracticaMaD.Model.Service.Tests
{
    [TestClass()]
    public class ServicePedidoTest
    {
        private static IKernel kernel;
        private static IServicePedido servicio;


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
            servicio = kernel.Get<IServicePedido>();
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


        
        [TestMethod()]
        public void crearPedidoTest()
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

            String direccion = "Estrada Castela 761-763";
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

            LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio,false);

            carrito.productos.Add(lc);

            pedido pedCreado = servicio.crearPedido("para mi",direccion, carrito, tarjeta.numero);

            pedido ped = new pedido();
            ped.id_pedido = pedCreado.id_pedido;
            ped.fecha = pedCreado.fecha;
            ped.precio = pedCreado.precio;
            ped.direccion = pedCreado.direccion;
            ped.tarjeta = pedCreado.tarjeta;
            ped.descripcion = pedCreado.descripcion;


            Assert.AreEqual(ped, pedCreado);
        }

        
        [TestMethod()]
        
        public void verPedidosTest()
        {
          usuario user = new usuario();
            user.nombre = "Dani";
            user.tipo_usuario = "admin";
            user.email = "micorreo11@gmail.com";
            user.apellidos = "Díaz Glez";
            user.contraseña = "unacontraseña";
            user.codigo_postal = "direccion";
            usuarioDao.Create(user);
            //Creamos tarjeta
            tarjeta_credito tarjeta = new tarjeta_credito();
            tarjeta.numero = "1211568";
            tarjeta.fecha = DateTime.Now;
            tarjeta.por_defecto = true;
            tarjeta.pertenece_a = user.id_usuario;
            tarjeta.cvv = "123";
            tarjeta.tipo = "C";
            tarjeta_CreditoDao.Create(tarjeta);

            String direccion = "Estrada Castela 761-763";
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

            LineaCarrito lc = new LineaCarrito(libro.id_producto, libro.nombre, 1, libro.precio,false);

            carrito.productos.Add(lc);

            pedido pedCreado = servicio.crearPedido("para mi",direccion, carrito, tarjeta.numero);

            PedidosDTO ped = new PedidosDTO(pedCreado.id_pedido, pedCreado.descripcion, pedCreado.fecha, pedCreado.precio, pedCreado.direccion);
         
            List<PedidosDTO> pedidosRef = new List<PedidosDTO>();

            pedidosRef.Add(ped);

            List<PedidosDTO> pedidosObtenidos = servicio.verPedidos(user.id_usuario, 0, 2);

            Assert.AreEqual(pedidosRef[0].idPedido, pedidosObtenidos[0].idPedido);


        }

    

        [TestCleanup()]
        public void MyTestCleanup()
        {
            transactionScope.Dispose();
        }
    }
}

