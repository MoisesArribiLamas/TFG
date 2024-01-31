using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Es.Udc.DotNet.CSharpTutorial
{

    /// <summary>
    /// Class HelloWorld
    /// The simplest C# example
    /// </summary>
    class ScrapingTarifa
    {
        

        /// <summary>
        /// Obtener los balores de la tarifa de la pagina web.
        /// </summary>
        public static void Main()
        {
            HtmlWeb oWeb = new HtmlWeb();
            HtmlDocument doc = oWeb.Load("https://tarifaluzhora.es/");
            var nodo = doc.DocumentNode.CssSelect("template - tlh__colors--hours - price template - tlh__color - low").First();
            //ValueTuple Precio = nodo.CssSelect()
        }


    }
}