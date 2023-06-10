﻿using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.TFG.Model.Daos.UbicacionDao
{
    public interface IUbicacionDao : IGenericDao<Ubicacion, Int64>
    {
        List<Ubicacion> FindAllUbicaciones();
    }
}
