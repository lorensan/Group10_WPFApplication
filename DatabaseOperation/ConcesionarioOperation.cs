using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace PruebaGrupo10_WPF.DatabaseOperation
{
    class ConcesionarioOperation
    {



        public List<string> getTipos()
        {
            //Inicializamos la lista de string a devolver
            List<string> tipos = new List<string>();

            using (ConcesionarioDB svcContext = new ConcesionarioDB())
            {
                var tipQuery = (from tp in svcContext.Tipos
                                   select tp.Descripcion).Distinct();

                foreach (var a in tipQuery)
                {
                    tipos.Add(a.ToString());
                }
            }

            return tipos;
        }

        public List<string> getModelosPorTipo(string tipo)
        {
            //Inicializamos la lista de string a devolver
            List<string> modelos = new List<string>();

            using (ConcesionarioDB concesDB = new ConcesionarioDB())
            {
                var tipQuery = from tp in concesDB.Tipos
                                join md in concesDB.Modelos on tp.CodTipo equals md.CodTipo
                                where tp.Descripcion==tipo
                                select md.Marca;

                foreach (var a in tipQuery)
                {
                    modelos.Add(a.ToString());
                }
            }

            return modelos;
        }

        public void insertarRegistro(string marca, string descricpion, string ruta)
        {
            int cod = 0;
            using (ConcesionarioDB concesDB = new ConcesionarioDB())
            {
                var tipQuery = from md in concesDB.Modelos
                               orderby md.CodTipo ascending
                               select md.CodTipo; 

                foreach (var a in tipQuery)
                {
                    cod = Convert.ToInt16(a)+1;
                }

                Modelos t = new Modelos
                {
                    CodTipo = cod++,
                    Marca = marca,
                    Descripcion = descricpion,
                    Imagen = Encoding.ASCII.GetBytes(ruta)
                };

                concesDB.Modelos.AddObject(t);
                concesDB.SaveChanges();
            }

           
            
        }

        /// <summary>
        /// Modificar registro en base de datos
        /// </summary>
        /// <param name="cod"></param>
        /// <param name="marca"></param>
        /// <param name="descricpion"></param>
        /// <param name="ruta"></param>
        public void modificarRegistro(int cod,string marca, string descricpion, string ruta)
        {

            throw new Exception("Funcion no implementada");
        }
    }
}
