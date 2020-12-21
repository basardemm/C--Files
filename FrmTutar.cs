using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using TeknikServis.Kutuphaneler;

namespace TeknikServis.Raporlar
{
    public partial class FrmTutar : Kalitim
    {
        string dosya = "TutarCizelgesi.frx";
        public FrmTutar()
        {
            InitializeComponent();
        }


        private void FrmTutar_Load(object sender, EventArgs e)
        {

        }


        #region Getir Butonu
        private void btnGetir_Click(object sender, EventArgs e)
        {
            Tutar tutar = new Tutar()
            {
                BaslangicTarihi = (deBaslangic.EditValue == null) ? (DateTime?)null : DateTime.Parse(deBaslangic.EditValue.ToString()),
                BitisTarihi = (deBitis.EditValue == null) ? (DateTime?)null : DateTime.Parse(deBitis.EditValue.ToString()),
            };
            try
            {
                if (tutar.BaslangicTarihi == null || tutar.BitisTarihi == null)
                {
                    MessageBoxes.Error("Boş alan");
                }
                else
                {
                    FastReport.Report rep = new FastReport.Report();
                    using (SqlKutuphane lib = new SqlKutuphane())
                    {
                        DataSet ds1 = lib.SqlBinding_Rapor(string.Format(@"SELECT SUM(Ucret)
FROM TServisIslem
WHERE DelDate IS NULL
	AND Ucret != 0
	AND GidisTarihi BETWEEN '{0}'
		AND '{1}'", tutar.BaslangicTarihi.Value.ToString("yyyy-MM-dd"), tutar.BitisTarihi.Value.ToString("yyyy-MM-dd")));

                        rep.RegisterData(ds1, "ServisIslem");


                        DataSet ds2 = lib.SqlBinding_Rapor(string.Format(@"SELECT SUM(Toplam)
FROM TServiseGelenCihaz
WHERE DelDate IS NULL
	AND Toplam != 0
	AND GidisTarihi BETWEEN '{0}'
		AND '{1}'", tutar.BaslangicTarihi.Value.ToString("yyyy-MM-dd"), tutar.BitisTarihi.Value.ToString("yyyy-MM-dd")));


                        rep.RegisterData(ds2, "ServiseGelen");


                        DataSet ds3 = lib.SqlBinding_Rapor(string.Format(@"SELECT SUM(Toplam)
FROM TServiseGelenCihaz_M
WHERE DelDate IS NULL
	AND Toplam != 0
	AND GidisTarihi BETWEEN '{0}'
		AND '{1}'", tutar.BaslangicTarihi.Value.ToString("yyyy-MM-dd"), tutar.BitisTarihi.Value.ToString("yyyy-MM-dd")));

                        rep.RegisterData(ds3, "ServiseGelen_M");
                        
                        rep.Load(dosya);

                        rep.SetParameterValue("Baslangic", tutar.BaslangicTarihi);

                        rep.SetParameterValue("Bitis", tutar.BitisTarihi);

                        rep.Show(true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxes.Error(ex.Message);
            }
        } 
        #endregion


    }
}

public class Tutar
{
    public DateTime? BaslangicTarihi { get; set; }
    public DateTime? BitisTarihi { get; set; }

  
}