using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Service.Baterias
{
    public class BateriaDTO
    {
        public BateriaDTO(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwHAlmacenados, double almacenajeMaximoKwH, 
            DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso)
        {
            this.bateriaId = bateriaId;
            this.ubicacionId = ubicacionId;
            this.usuarioId = usuarioId;
            this.precioMedio = precioMedio;
            this.kwHAlmacenados = kwHAlmacenados;
            this.almacenajeMaximoKwH = almacenajeMaximoKwH;
            this.fechaDeAdquisicion = fechaDeAdquisicion;
            this.marca = marca;
            this.modelo = modelo;
            this.ratioCarga = ratioCarga;
            this.ratioCompra = ratioCompra;
            this.ratioUso = ratioUso; 
        }

        public long bateriaId { get; set; }
        public long ubicacionId { get; set; }
        public long usuarioId { get; set; }
        public double precioMedio { get; set; }

        public double kwHAlmacenados { get; private set; }

        public double almacenajeMaximoKwH { get; private set; }
        public DateTime fechaDeAdquisicion { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }

        public double ratioCarga { get; private set; }

        public double ratioCompra { get; private set; }
        public double ratioUso { get; private set; }

        public override bool Equals(object obj)
        {
            var details = obj as BateriaDTO;
            return details != null &&
                   bateriaId == details.bateriaId &&
                   ubicacionId == details.ubicacionId &&
                   usuarioId == details.usuarioId &&
                   precioMedio == details.precioMedio
                   && (this.kwHAlmacenados == details.kwHAlmacenados)
                   && (this.almacenajeMaximoKwH == details.almacenajeMaximoKwH)
                   && (this.fechaDeAdquisicion == details.fechaDeAdquisicion)
                   && (this.marca == details.marca)
                   && (this.modelo == details.modelo)
                   && (this.ratioCarga == details.ratioCarga)
                   && (this.ratioCompra == details.ratioCompra)
                   && (this.ratioUso == details.ratioUso);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
