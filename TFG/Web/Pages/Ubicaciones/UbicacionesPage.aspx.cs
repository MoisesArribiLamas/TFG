using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Tarifas;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Es.Udc.DotNet.TFG.Web.HTTP.Session;
using Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects;
using Es.Udc.DotNet.TFG.Web.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.TFG.Web.Pages.Ubicaciones
{
    public partial class UbicacionesPage : SpecificCulturePage
    {
        private ObjectDataSource pbpDataSource = new ObjectDataSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(
               Response.ApplyAppPathModifier("~/Pages/User/LogUser.aspx"));
            }

            pbpDataSource.ObjectCreating += this.PbpDataSource_ObjectCreating;

            pbpDataSource.TypeName =
                 Settings.Default.ObjectDS_ShowUbicaciones_IServiceUbicacion;

            pbpDataSource.EnablePaging = true;

            pbpDataSource.SelectMethod =
                Settings.Default.ObjectDS_ShowUbicaciones_SelectMethod;

            long idUser = SessionManager.GetUserSession(Context).UserProfileId;

            pbpDataSource.SelectParameters.Add("idUsuario", DbType.Int64, idUser.ToString());


            pbpDataSource.SelectCountMethod =
                  Settings.Default.ObjectDS_ShowUbicaciones_CountMethod;

            pbpDataSource.StartRowIndexParameterName =
                    Settings.Default.ObjectDS_ShowUbicaciones_StartIndexParameter;

            pbpDataSource.MaximumRowsParameterName =
                    Settings.Default.ObjectDS_ShowUbicaciones_CountParameter;

            gvUbicaciones.AllowPaging = true;
            gvUbicaciones.PageSize = Settings.Default.TFG_defaultCount;

            gvUbicaciones.DataSource = pbpDataSource;
            gvUbicaciones.DataBind();



            foreach (GridViewRow row in gvUbicaciones.Rows)
            {

                HyperLink link = row.Cells[1].Controls[0] as HyperLink;

                link.NavigateUrl = "~/Pages/Ubicaciones/ModificarUbicacion.aspx?idUbicacion=" + row.Cells[0].Text;
                
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect(Response.
                        ApplyAppPathModifier("~/Pages/Ubicaciones/CrearUbicacion.aspx"));
        }

        protected void PbpDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            /* Get the Service */
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IServiceUbicacion pedidoUbicacion = iocManager.Resolve<IServiceUbicacion>();

            e.ObjectInstance = pedidoUbicacion;

        }


        protected void gvUbicaciones_RowCommand(Object sender, GridViewCommandEventArgs e)
        {



        }
        protected void gvUbicacionesPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUbicaciones.PageIndex = e.NewPageIndex;
            gvUbicaciones.DataBind();
            foreach (GridViewRow row in gvUbicaciones.Rows)
            {

                HyperLink link = row.Cells[1].Controls[0] as HyperLink;

                link.NavigateUrl = "~/Pages/Ubicaciones/ModificarUbicacion.aspx?idUbicacion=" + row.Cells[0].Text;
                
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}