using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.TFG.Model.Dao.UsuarioDao;
using Es.Udc.DotNet.TFG.Model.Daos.ConsumoDao;
using Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao;
using Es.Udc.DotNet.TFG.Model.Service.Baterias;
using Es.Udc.DotNet.TFG.Model.Service.Estados;
using Es.Udc.DotNet.TFG.Model.Service.Ubicaciones;
using Ninject;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Consumos
{
    public class ServiceConsumo : IServiceConsumo
    {

        [Inject]
        public IUbicacionDao ubicacionDao { private get; set; }
        [Inject]
        public IConsumoDao consumoDao { private get; set; }

        [Inject]
        public IServiceUbicacion ServicioUbicacion { private get; set; } 


       



        #region modificar Consumo
        [Transactional]
        public void scrapyTarifas()
        {

            HtmlWeb oWeb = new HtmlWeb();
            HtmlDocument doc = oWeb.Load("https://tarifaluzhora.es/");
            //var nodo = doc.DocumentNode.CssSelect(".template-tlh__colors--hours-price").First();
            foreach (var nodo in doc.DocumentNode.CssSelect(".template-tlh__colors--hours-price"))
            {
                var elemento = nodo.CssSelect("span").First();
                string span = elemento.InnerHtml;
                string p = span.Substring(0, span.IndexOf(" "));
                long precio = long.Parse(p);
            }
        }

        #endregion modificar Consumo

     

    }

}
