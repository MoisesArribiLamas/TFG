using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service
{
    public class EstadoDTO
    {
        public EstadoDTO(string nombre)
        {
            this.nombre = nombre;
            
        }

        
        public string nombre { get; set; }
        

        public override bool Equals(object obj)
        {
            var details = obj as EstadoDTO;
            return details != null 
                   && (this.nombre == details.nombre);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
