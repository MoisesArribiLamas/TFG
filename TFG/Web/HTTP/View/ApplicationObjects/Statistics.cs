using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;

namespace Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects
{
    public class Statistics
    {

        /*
         * In a more realistic application, these values could be read from a
         * database in the "static" constructor.
         */
        private static readonly ArrayList statistics_es = new ArrayList();
        private static readonly ArrayList statistics_en = new ArrayList();
        private static readonly ArrayList statistics_gl = new ArrayList();
        private static readonly Hashtable statistics = new Hashtable();

        /* Access modifiers are not allowed on static constructors
         * so if we want to prevent that anybody creates instances
         * of this class we must do the following ...
         */

        private Statistics()
        {
        }

        static Statistics()
        {
            #region set the statistics

            statistics_es.Add(new ListItem("Ahorro"));
            statistics_es.Add(new ListItem("Suministrado por la Red"));
            statistics_es.Add(new ListItem("Cargados"));
            statistics_es.Add(new ListItem("Suministrado"));
            statistics_es.Add(new ListItem("Red vs Suministrado Cargados"));

            statistics_en.Add(new ListItem("Saving money"));
            statistics_en.Add(new ListItem("Supplied by the Network"));
            statistics_en.Add(new ListItem("Loaded"));
            statistics_en.Add(new ListItem("Supplied"));
            statistics_en.Add(new ListItem("Network vs Loaded vs Supplied"));

            statistics_gl.Add(new ListItem("Aforro"));
            statistics_gl.Add(new ListItem("Suministrado pola Red"));
            statistics_gl.Add(new ListItem("Cargou"));
            statistics_gl.Add(new ListItem("Suministrou"));
            statistics_gl.Add(new ListItem("Red vs Suministrou Cargou"));


            statistics.Add("es", statistics_es);
            statistics.Add("en", statistics_en);
            statistics.Add("gl", statistics_gl);

            #endregion set the statistics
        }



        public static ArrayList GetStatistics(String languageCode)
        {
            ArrayList lang = (ArrayList)statistics[languageCode];

            if (lang != null)
            {
                return lang;
            }
            else
            {
                return (ArrayList)statistics["en"];
            }
        }
    }
}