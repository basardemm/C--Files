using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Kutuphaneler
{
    public class SqlKutuphane : Business_Model.ISqlKutuphane,IDisposable
    {
        public string DosyaOkuyucu()
        {
            using (StreamReader sr = new StreamReader("kartel.ltd"))
            {
                return sr.ReadToEnd();
            }
        }

        public SqlConnection Connector()
        {
            SqlConnection conn = new SqlConnection(DosyaOkuyucu());
            conn.Open();
            return conn;
        }

        public void SqlExecuteNonQuery(string sorgu, SqlParameter[] param)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sorgu, Connector()))
                {                    
                    cmd.Parameters.AddRange(param);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                Connector().Close();
            }
        }

        public int SqlExecuteScalar(string sorgu)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(sorgu, Connector()))
                {                    
                    return (int)cmd.ExecuteScalar();
                }
            }
            finally
            {
                Connector().Close();
            }
        }

        public DataTable SqlBindind(string sorgu)
        {
            using (DataTable dt=new DataTable())
            {
                using (SqlDataAdapter sda=new SqlDataAdapter(sorgu, Connector()))
                {
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        public SqlDataReader SqlExecuteReader(string sorgu, SqlParameter[] param)
        {
            using (SqlCommand cmd = new SqlCommand(sorgu, Connector()))
            {
                cmd.Parameters.AddRange(param);
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sdr;
            }
        }


        public DataSet SqlBinding_Rapor(string sorgu)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(sorgu, Connector()))
            {
                sda.Fill(ds);
                return ds;
            }
        }


        public SqlDataReader SqlExecuteReader2(string sorgu)
        {
            using (SqlCommand cmd = new SqlCommand(sorgu, Connector()))
            {
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return sdr;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

       
    }
}