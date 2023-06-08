using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.TFG.Model.Service;
using System.Collections.Generic;

namespace Es.Udc.DotNet.TFG.Web.HTTP.View.ApplicationObjects
{
    public class Categories
    {


        private static readonly ArrayList categories = new ArrayList();

        /* Access modifiers are not allowed on static constructors
         * so if we want to prevent that anybody creates instances
         * of this class we must do the following ...
         */

        private Categories()
        {
          
        }

        static Categories()
        { 
            
        }


    public static ArrayList GetCategories()
    {
        return categories;
    }
}
}