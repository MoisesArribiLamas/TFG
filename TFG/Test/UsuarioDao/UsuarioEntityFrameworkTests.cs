using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ninject;
using Es.Udc.DotNet.TFG.Test;
using Es.Udc.DotNet.ModelUtil.Exceptions;
namespace Es.Udc.DotNet.TFG.Model.UsuarioDao.Tests
{
    [TestClass()]
    public class UsuarioEntityFrameworkTests
    {
        private static IKernel kernel;
        private static IUsuarioDao usuarioDao;

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
            usuarioDao = kernel.Get<IUsuarioDao>();

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
        public void findUserByNameTest1()
        {
            Usuario user = new Usuario();
            user.nombre = "Dani";
            user.email = "micorreo@gmail.com";
            user.apellido1 = "Díaz";
            user.apellido2 = "Glez";
            user.contraseña = "unacontraseña";
            user.telefono = "981123456";
            user.pais = "España";
            user.idioma = "es-ES";
            usuarioDao.Create(user);

            Assert.AreEqual(user, usuarioDao.findUserByName(user.email));

        }


      


        [TestMethod()]
        public void updateInformacionTest()
        {

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
            

            Usuario user2 = new Usuario();
            user2.usuarioId = user.usuarioId;
            user2.nombre = "María";
            user2.contraseña = "nos olvidamos ups";
            user2.email = "micorreo@gmail.com";
            user2.apellido1 = "Pérez";
            user2.apellido2 = "Fernández";
            user2.telefono = "981123457";
            user2.idioma = "es-ES";
            user2.pais = "España";
            usuarioDao.updateInformacion(user.usuarioId, user2.nombre, user2.apellido1, user2.apellido2, user2.contraseña, user2.telefono, user2.email, user2.idioma, user2.pais );

            Usuario userActualizado = usuarioDao.Find(user.usuarioId);
            Assert.AreEqual(user2.nombre ,userActualizado.nombre );
            Assert.AreEqual(user2.apellido1, userActualizado.apellido1);
            Assert.AreEqual(user2.apellido2, userActualizado.apellido2);
            Assert.AreEqual(user2.contraseña, userActualizado.contraseña);
            Assert.AreEqual(user2.telefono, userActualizado.telefono);
            Assert.AreEqual(user2.email, userActualizado.email);
            Assert.AreEqual(user2.idioma, userActualizado.idioma);
            Assert.AreEqual(user2.pais, userActualizado.pais);



        }
    }
}