using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service.Exceptions;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Web.HTTP.Util;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Es.Udc.DotNet.ModelUtil.Exceptions;



namespace Es.Udc.DotNet.TFG.Web.HTTP.Session
{
    public class SessionManager
    {
        public const String LOCALE_SESSION_ATTRIBUTE = "locale";
        public static readonly String USER_SESSION_ATTRIBUTE =
              "userSession";

        private static IServiceUsuario serviceUsuario;

        public IServiceUsuario ServiceUsuario
        {
            set { serviceUsuario = value; }
        }

        static SessionManager()
        {
            IIoCManager iocManager =
                (IIoCManager)HttpContext.Current.Application["managerIoC"];

            serviceUsuario = iocManager.Resolve<IServiceUsuario>();
        }

        public static void RegisterUser(HttpContext context,
           String clearPassword,
           UserProfileDetails userProfileDetails)
        {
            /* Register user. */
            long usrId = serviceUsuario.registrarUsuario( clearPassword,
                userProfileDetails);

            /* Insert necessary objects in the session. */
            UserSession userSession = new UserSession();
            userSession.UserProfileId = usrId;
            userSession.FirstName = userProfileDetails.nombre;

           Locale locale = new Locale(userProfileDetails.Language,
               userProfileDetails.Country);
           

            UpdateSessionForAuthenticatedUser(context, userSession, locale);

            FormsAuthentication.SetAuthCookie(userProfileDetails.email, false);
        }
        /// <summary>
        /// Login method. Authenticates an user in the current context.
        /// </summary>
        /// <param name="context">Http Context includes request, response, etc.</param>
        /// <param name="loginName">Username</param>
        /// <param name="clearPassword">Password in clear text</param>
        /// <param name="rememberMyPassword">Remember password to the next logins</param>
        /// <exception cref="IncorrectPasswordException"/>
        /// <exception cref="InstanceNotFoundException"/>
        public static void Login(HttpContext context, String loginName,
          String clearPassword, Boolean rememberMyPassword)
        {
         
                /* Try to login, and if successful, update session with the necessary
                 * objects for an authenticated user. */
                LoginResult loginResult = DoLogin(context, loginName,
                    clearPassword, false, rememberMyPassword);

                /* Add cookies if requested. */
                if (rememberMyPassword)
                {
                    CookiesManager.LeaveCookies(context, loginName,
                        loginResult.passEncriptada);
                }
        }

        private static LoginResult DoLogin(HttpContext context,
             String loginName, String password, Boolean passwordIsEncrypted,
             Boolean rememberMyPassword)
        {
            
                LoginResult loginResult =
                    serviceUsuario.logearUsuario(loginName, password,
                        passwordIsEncrypted);

                /* Insert necessary objects in the session. */

                UserSession userSession = new UserSession();
                userSession.UserProfileId = loginResult.userId;
                userSession.FirstName = loginResult.nombre;

                Locale locale =
                   new Locale(loginResult.Language, loginResult.Country);

                UpdateSessionForAuthenticatedUser(context, userSession, locale);

                return loginResult;
            }
 

        private static void UpdateSessionForAuthenticatedUser(
            HttpContext context, UserSession userSession, Locale locale)
        {
            /* Insert objects in session. */
            context.Session.Add(USER_SESSION_ATTRIBUTE, userSession);
            context.Session.Add(LOCALE_SESSION_ATTRIBUTE, locale);
        }

        public static Boolean IsUserAuthenticated(HttpContext context)
        {
            if (context.Session == null)
                return false;

            return (context.Session[USER_SESSION_ATTRIBUTE] != null);
        }


        public static void SetLocale(HttpContext context, Locale locale)
        {
            context.Session[LOCALE_SESSION_ATTRIBUTE] = locale;
        }

        public static Locale GetLocale(HttpContext context)
        {
            Locale locale = (Locale)context.Session[LOCALE_SESSION_ATTRIBUTE];

            return (locale);
        }

        public static void UpdateUserProfileDetails(HttpContext context,
           UserProfileDetails userProfileDetails)
        {
            /* Update user's profile details. */

            UserSession userSession =
                (UserSession)context.Session[USER_SESSION_ATTRIBUTE];

            serviceUsuario.modificarUsuario(userSession.UserProfileId,
               userProfileDetails);

            /* Update user's session objects. */

            Locale locale = new Locale(userProfileDetails.Language,
              userProfileDetails.Country);

            userSession.FirstName = userProfileDetails.nombre;

            UpdateSessionForAuthenticatedUser(context, userSession, locale);
        }


        public static UserSession GetUserSession(HttpContext context)
        {
            if (IsUserAuthenticated(context))
                return (UserSession)context.Session[USER_SESSION_ATTRIBUTE];
            else
                return null;
        }

        public static void Logout(HttpContext context)
        {
            /* Remove cookies. */
            CookiesManager.RemoveCookies(context);

            /* Invalidate session. */
            context.Session.Abandon();

            /* Invalidate Authentication Ticket */
            FormsAuthentication.SignOut();
        }


        public static void TouchSession(HttpContext context)
        {
            /* Check if "UserSession" object is in the session. */
            UserSession userSession = null;

            if (context.Session != null)
            {
                userSession =
                    (UserSession)context.Session[USER_SESSION_ATTRIBUTE];
                

                // If userSession object is in the session, nothing should be doing.
                if (userSession != null)
                {
                    return;
                }
            }

            /*
             * The user had not been authenticated or his/her session has
             * expired. We need to check if the user has selected "remember my
             * password" in the last login (login name and password will come
             * as cookies). If so, we reconstruct user's session objects.
             */
            UpdateSessionFromCookies(context);
        }
        private static void UpdateSessionFromCookies(HttpContext context)
        {
            HttpRequest request = context.Request;
            if (request.Cookies == null)
            {
                return;
            }

            /*
             * Check if the login name and the encrypted password come as
             * cookies.
             */
            String loginName = CookiesManager.GetLoginName(context);
            String encryptedPassword = CookiesManager.GetEncryptedPassword(context);

            if ((loginName == null) || (encryptedPassword == null))
            {
                return;
            }

            /* If loginName and encryptedPassword have valid values (the user selected "remember
             * my password" option) try to login, and if successful, update session with the
             * necessary objects for an authenticated user.
             */
            try
            {
                DoLogin(context, loginName, encryptedPassword, true, true);

                /* Authentication Ticket. */
                FormsAuthentication.SetAuthCookie(loginName, true);
            }
            catch (Exception)
            { // Incorrect loginName or encryptedPassword
                return;
            }
        }

        public static void ChangePassword(HttpContext context,
               String oldClearPassword, String newClearPassword)
        {
            UserSession userSession =
                (UserSession)context.Session[USER_SESSION_ATTRIBUTE];

            serviceUsuario.modificarContraseña(userSession.UserProfileId,
                oldClearPassword, newClearPassword);

            /* Remove cookies. */
            CookiesManager.RemoveCookies(context);
        }

        public static Boolean IsLocaleDefined(HttpContext context)
        {
            if (context.Session == null)
                return false;
            else
                return (context.Session[LOCALE_SESSION_ATTRIBUTE] != null);
        }
    }
}