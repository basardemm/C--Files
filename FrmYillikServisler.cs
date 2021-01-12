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
using TeknikServis.Model;

namespace TeknikServis.Raporlar
{
    public partial class FrmYillikServisler : Kalitim
    {
        string dosya1 = "YillikServisAdetli.frx";
        string dosya2 = "YillikServisToplamli.frx";

        public FrmYillikServisler()
        {
            InitializeComponent();
        }

        private void FrmYillikServisler_Load(object sender, EventArgs e)
        {
            LookUpEditFill();
        }



        #region Getir Butonu
        private void btnGetir_Click(object sender, EventArgs e)
        {
            switch (ceToplamHali.Checked)
            {
                case true: ceToplamHali.CheckState = CheckState.Unchecked; break;
            }

            if (leYillar.EditValue != null || int.Parse(leYillar.EditValue.ToString()) != 0)
            {
                FastReport.Report rep = new FastReport.Report();

                TYillar yillar = new TYillar
                {
                    Yil = int.Parse(leYillar.Text)
                };

                using (SqlKutuphane lib = new SqlKutuphane())
                {
                    DataSet ds1 = lib.SqlBinding_Rapor(string.Format(@"SELECT TM.MusteriUnvani
	,X.Servis
	,Y.Personel
	,Z.Musteri
FROM TMusteriTanimlari TM
CROSS APPLY (
	SELECT count(SI.GidisTarihi) AS Servis
	FROM TServisIslem SI
	WHERE SI.DelDate IS NULL
		AND DATEPART(YEAR, SI.AddDate) = {0}
		AND TM.Id = SI.Musteri
	) X
CROSS APPLY (
	SELECT count(SGC.GelisTarihi) AS Personel
	FROM TServiseGelenCihaz SGC
	WHERE SGC.DelDate IS NULL
		AND DATEPART(YEAR, SGC.AddDate) = {0}
		AND TM.Id = SGC.MusteriId
	) Y
CROSS APPLY (
	SELECT count(SGCM.GelisTarihi) AS Musteri
	FROM TServiseGelenCihaz_M SGCM
	WHERE SGCM.DelDate IS NULL
		AND DATEPART(YEAR, SGCM.AddDate) = {0}
		AND TM.Id = SGCM.MusteriId
	) Z
WHERE TM.DelDate IS NULL
GROUP BY TM.MusteriUnvani
	,X.Servis
	,Y.Personel
	,Z.Musteri
ORDER BY Servis DESC", yillar.Yil));

                    rep.RegisterData(ds1, "YillikServisAdeti");

                    rep.Load(dosya1);

                    rep.Show(true);
                }
            }
            else MessageBoxes.Error("Yanlış Seçim yaptınız");

        }
        #endregion



        #region Toplanmış Hali
        private void ceToplamHali_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                switch (leYillar.EditValue != null)
                {
                    case true:

                        TYillar yillar = new TYillar
            {
                Yil = int.Parse(leYillar.Text)
            };
                        FastReport.Report rep = new FastReport.Report();
                        switch (ceToplamHali.Checked)
                        {
                            case true:
                                using (SqlKutuphane lib = new SqlKutuphane())
                                {
                                    DataSet ds2 = lib.SqlBinding_Rapor(string.Format(@"SELECT TM.MusteriUnvani
	,(SUM(X.Gidis) + SUM(Y.Gelis1) + SUM(Z.Gelis2)) Sonuc
FROM TMusteriTanimlari TM
CROSS APPLY (
	SELECT count(SI.GidisTarihi) AS Gidis
	FROM TServisIslem SI
	WHERE SI.DelDate IS NULL
		AND DATEPART(YEAR, SI.AddDate) = {0}
		AND TM.Id = SI.Musteri
	) X
CROSS APPLY (
	SELECT count(SGC.GelisTarihi) AS Gelis1
	FROM TServiseGelenCihaz SGC
	WHERE SGC.DelDate IS NULL
		AND DATEPART(YEAR, SGC.AddDate) = {0}
		AND TM.Id = SGC.MusteriId
	) Y
CROSS APPLY (
	SELECT count(SGCM.GelisTarihi) AS Gelis2
	FROM TServiseGelenCihaz_M SGCM
	WHERE SGCM.DelDate IS NULL
		AND DATEPART(YEAR, SGCM.AddDate) = {0}
		AND TM.Id = SGCM.MusteriId
	) Z
WHERE TM.DelDate IS NULL
GROUP BY TM.MusteriUnvani
	,X.Gidis
	,Y.Gelis1
	,Z.Gelis2
ORDER BY Sonuc DESC", yillar.Yil));

                                    rep.RegisterData(ds2, "YillikServisToplami");

                                    rep.Load(dosya2);

                                    rep.Show(true);
                                }
                                break;
                        }
                        break;
                    case false: MessageBoxes.Error("Yıl seçiniz"); break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxes.Error(ex.Message);
            }
        }
        #endregion


        public override void LookUpEditFill()
        {
            base.LookUpEditFill();
        }



    }
}