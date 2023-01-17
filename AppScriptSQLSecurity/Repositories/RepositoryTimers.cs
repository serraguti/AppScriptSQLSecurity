using AppScriptSQLSecurity.Models;
using Microsoft.Extensions.Primitives;
using System.Data.Common;
using System.Data.SqlClient;

namespace AppScriptSQLSecurity.Repositories
{
    public class RepositoryTimers
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        string connectionString;

        public RepositoryTimers(string connectionString)
        {
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.com.CommandType = System.Data.CommandType.Text;
            this.connectionString = connectionString; 
        }

        public void ExecuteScript(string script)
        {
            using (DbConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (DbCommand command = new SqlCommand(script))
                {
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TiemposEventos> GetTimers()
        {
            this.com.CommandType = System.Data.CommandType.Text;
            string sql = "select * from TIEMPOS_EVENTOS order by inicio";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<TiemposEventos> lista = new List<TiemposEventos>();
            while (this.reader.Read())
            {
                TiemposEventos tiempo = new TiemposEventos();
                tiempo.Id = int.Parse(this.reader["UNIQUEID"].ToString());
                tiempo.Empresa = this.reader["EMPRESA"].ToString();
                tiempo.Sala = this.reader["SALA"].ToString();
                tiempo.Duracion = int.Parse(this.reader["DURACION"].ToString());
                tiempo.Inicio = DateTime.Parse(this.reader["INICIO"].ToString());
                tiempo.Categoria = this.reader["CATEGORIA"].ToString();
                lista.Add(tiempo);
            }
            this.reader.Close();
            this.cn.Close();
            return lista;
        }

        public void IncreaseMinutesTimers(int minutes)
        {
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "SP_INCREASETIMERS";
            SqlParameter pamminutes = new SqlParameter("@INCREASE", minutes);
            this.com.Parameters.Add(pamminutes);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
            this.cn.Close();
        }
    }
}
