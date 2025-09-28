using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace app.Banco.Utilidades
{
    [DataContract]

    public class ParametrosConexion
    {
        [DataMember(Order = 0)]
        public string servidor { get; set; }

        [DataMember(Order = 1)]
        public string baseDatos { get; set; }

        public string cadenaConexion =>
            $"Data Source={servidor}; Initial Catalog={baseDatos}; Integrated Security=True;Encrypt=False";
    }

    public static class AdminstrarConexion
    {
        private static readonly string carpeta =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "app.Banco");

        private static readonly string archivo = Path.Combine(carpeta, "conexion.json");

        private static string rutaArchivo => archivo;

        public static void Guardar(ParametrosConexion parametros)
        {
            if(!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var serializar = new DataContractJsonSerializer(typeof(ParametrosConexion));
            using(var ms = new MemoryStream())
            {
                serializar.WriteObject(ms, parametros);
                File.WriteAllText(archivo, Encoding.UTF8.GetString(ms.ToArray()), Encoding.UTF8);
            }
        }

        public static ParametrosConexion Cargar()
        {
            if(!File.Exists(archivo))
                return new ParametrosConexion();

            var json = File.ReadAllText(archivo, Encoding.UTF8);
            var bytes = Encoding.UTF8.GetBytes(json);
            using(var ms = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(ParametrosConexion));
                return(ParametrosConexion)serializer.ReadObject(ms);
            }
        }

        public static bool ProbarConexion(ParametrosConexion parametros, out string error)
        {
            try
            {
                using (var cn = new SqlConnection(parametros.cadenaConexion))
                {
                    cn.Open();
                    error = null;
                    return true;
                }
            }
            catch(Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
