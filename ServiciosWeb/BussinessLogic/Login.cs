using BussinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiciosWeb.Data.Model;

namespace ServicioWeb.BussinessLogic
{
    public class Login : IUsuario
    {
 

        public IList<ServiciosWeb.Data.Model.Login> GetAll()
        {
            BPM_SIEEntities BD = new BPM_SIEEntities();
            var listado = BD.Login.ToList();
            return listado;
        }
    }
}
