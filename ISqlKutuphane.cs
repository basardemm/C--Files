using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Business_Model
{
    interface ISqlKutuphane
    {
        string DosyaOkuyucu();

        SqlConnection Connector();

        void SqlExecuteNonQuery(string sorgu, SqlParameter[] sdr);

        int SqlExecuteScalar(string sorgu);

        SqlDataReader SqlExecuteReader(string sorgu, SqlParameter[] sdr);

        SqlDataReader SqlExecuteReader2(string sorgu);
        
        DataTable SqlBindind(string sorgu);

        DataSet SqlBinding_Rapor(string sorgu);
    }
}