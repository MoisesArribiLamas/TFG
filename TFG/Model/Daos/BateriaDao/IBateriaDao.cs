using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.BateriaDao
{
    public interface IBateriaDao : IGenericDao<Bateria, Int64>
    {
        bool updateInformacion(long bateriaId, long ubicacionId, long usuarioId, double precioMedio, double kwAlmacenados, double almacenajeMaximoKw, System.DateTime fechaDeAdquisicion, string marca, string modelo, double ratioCarga, double ratioCompra, double ratioUso);
        List<Bateria> findBateriaByUser(long usuarioID, int startIndex, int count);
        List<Bateria> findBateriaByUbicacion(long ubicacionID, int startIndex, int count);
    }
}
