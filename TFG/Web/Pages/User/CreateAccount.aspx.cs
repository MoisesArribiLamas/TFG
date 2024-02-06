using Es.Udc.DotNet.ModelUtil.Exceptions;
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
    public partial class CreateAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                /* Get current language and country from browser */
                String defaultLanguage =
                   GetLanguageFromBrowserPreferences();
                String defaultCountry =
                    GetCountryFromBrowserPreferences();

                /* Combo box initialization */
                UpdateListaIdiomas(defaultLanguage);
                UpdateListaPaises(defaultLanguage, defaultCountry);
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

        protected void btRegistrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    UserProfileDetails userProfileDetailsVO = 
                        new UserProfileDetails(BoxUserMailCreateAccount.Text, BoxNombreCreateAccount.Text,
                            BoxApellido1CreateAccount.Text, BoxApellido2CreateAccount.Text, BoxTelefonoCreateAccount.Text, ListaIdiomasCreateAccount.SelectedValue,
                            ListaPaisesCreateAccount.SelectedValue);

                    SessionManager.RegisterUser(Context,
                        BoxPasswordCreateAccount.Text, userProfileDetailsVO);

                    Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/SuccesfulOperation.aspx"));
                }
                catch (DuplicateInstanceException)
                {
                    lblLoginErrorCreateAccount.Visible = true;
                }
            }
        }
        private void UpdateListaIdiomas(String selectedLanguage)
        {
            this.ListaIdiomasCreateAccount.DataSource = Languages.GetLanguages(selectedLanguage);
            this.ListaIdiomasCreateAccount.DataTextField = "text";
            this.ListaIdiomasCreateAccount.DataValueField = "value";
            this.ListaIdiomasCreateAccount.DataBind();
            this.ListaIdiomasCreateAccount.SelectedValue = selectedLanguage;
        }

        private void UpdateListaPaises(String selectedLanguage, String selectedCountry)
        {
            this.ListaPaisesCreateAccount.DataSource = Countries.GetCountries(selectedLanguage);
            this.ListaPaisesCreateAccount.DataTextField = "text";
            this.ListaPaisesCreateAccount.DataValueField = "value";
            this.ListaPaisesCreateAccount.DataBind();
            this.ListaPaisesCreateAccount.SelectedValue = selectedCountry;
        }



        protected void ListaIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language.
             */
            this.UpdateListaPaises(ListaIdiomasCreateAccount.SelectedValue, ListaPaisesCreateAccount.SelectedValue
                );
        }

        protected void ListaPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* After a language change, the countries are printed in the
             * correct language.
             */
            this.UpdateListaPaises(ListaIdiomasCreateAccount.SelectedValue, ListaPaisesCreateAccount.SelectedValue
                );
        }
    }
}