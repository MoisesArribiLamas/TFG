using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.ModelUtil.Log;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages
{
    public partial class ModifyUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {

                if (!SessionManager.IsUserAuthenticated(Context))
                {
                    Response.Redirect(
                   Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
                }


                /* Get current language and country from browser */
                String defaultLanguage =
                   GetLanguageFromBrowserPreferences();
                String defaultCountry =
                    GetCountryFromBrowserPreferences();

                /* Combo box initialization */
                UpdateListaIdiomas(defaultLanguage);
                UpdateListaPaises(defaultLanguage, defaultCountry);




            }
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }
        }


        private String GetLanguageFromBrowserPreferences()
        {
            String language;
            CultureInfo cultureInfo =
                CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);
            language = cultureInfo.TwoLetterISOLanguageName;
            LogManager.RecordMessage("Preferred language of user" +
                                     " (based on browser preferences): " + language);
            return language;
        }

        private String GetCountryFromBrowserPreferences()
        {
            String country;
            CultureInfo cultureInfo =
                CultureInfo.CreateSpecificCulture(Request.UserLanguages[0]);

            if (cultureInfo.IsNeutralCulture)
            {
                country = "";
            }
            else
            {
                // cultureInfoName is something like en-US
                String cultureInfoName = cultureInfo.Name;
                // Gets the last two caracters of cultureInfoname
                country = cultureInfoName.Substring(cultureInfoName.Length - 2);

                LogManager.RecordMessage("Preferred region/country of user " +
                                         "(based on browser preferences): " + country);
            }

            return country;
        }

        private void UpdateListaIdiomas(String selectedLanguage)
        {
            this.ListaIdiomasModModifyUser.DataSource = Languages.GetLanguages(selectedLanguage);
            this.ListaIdiomasModModifyUser.DataTextField = "text";
            this.ListaIdiomasModModifyUser.DataValueField = "value";
            this.ListaIdiomasModModifyUser.DataBind();
            this.ListaIdiomasModModifyUser.SelectedValue = selectedLanguage;
        }

        private void UpdateListaPaises(String selectedLanguage, String selectedCountry)
        {
            this.ListaPaisesModModifyUser.DataSource = Countries.GetCountries(selectedLanguage);
            this.ListaPaisesModModifyUser.DataTextField = "text";
            this.ListaPaisesModModifyUser.DataValueField = "value";
            this.ListaPaisesModModifyUser.DataBind();
            this.ListaPaisesModModifyUser.SelectedValue = selectedCountry;
        }



        protected void ListaIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language.
             */
            this.UpdateListaPaises(ListaIdiomasModModifyUser.SelectedValue, ListaPaisesModModifyUser.SelectedValue);
        }

        protected void ListaPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language.
             */
            this.UpdateListaPaises(ListaIdiomasModModifyUser.SelectedValue, ListaPaisesModModifyUser.SelectedValue
                );
        }

        protected void btModificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                UserSession sesion = SessionManager.GetUserSession(Context);

                UserProfileDetails userProfileDetails =
               new UserProfileDetails(BoxUserMailModifyUser.Text, BoxNombreModModifyUser.Text, BoxApellidosModModifyUser.Text,
                     BoxApellidosModModifyUser.Text, BoxCPModModifyUser.Text, ListaIdiomasModModifyUser.SelectedValue,
                            ListaPaisesModModifyUser.SelectedValue);

                SessionManager.UpdateUserProfileDetails(Context,
              userProfileDetails);

                Response.Redirect(
                    Response.ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));

            }
        }
    }
}