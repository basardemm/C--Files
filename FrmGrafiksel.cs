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
using System.Data.SqlClient;
using DevExpress.XtraCharts;
using DevExpress.Utils;

namespace TeknikServis.Raporlar
{
    public partial class FrmGrafiksel : Kalitim
    {
        public FrmGrafiksel()
        {
            InitializeComponent();
        }

        private void FrmGrafiksel_Load(object sender, EventArgs e)
        {
            LookUpEditFill();
        }

        public override void LookUpEditFill()
        {
            base.LookUpEditFill();
        }


        #region Yıl Seçicisi
        private void leYillar_EditValueChanged(object sender, EventArgs e)
        {
            chartControl1.Series.Clear();

            Series seri1 = new Series("Servis", ViewType.Bar3D);

            Series seri2 = new Series("Müşteri", ViewType.Bar3D);

            Series seri3 = new Series("Personel", ViewType.Bar3D);

            TYillar yillar = new TYillar
            {
                Yil = int.Parse(leYillar.Text)
            };

            using (SqlKutuphane lib = new SqlKutuphane())
            {
                var x = lib.SqlExecuteReader(@"SELECT t1.Servis
	,t2.Personel
	,t3.Musteri
FROM (
	SELECT count(SI.GelisTarihi) Servis
	FROM TServisIslem SI
	WHERE SI.DelDate IS NULL
		AND DATEPART(YEAR, SI.GelisTarihi) = @Yil
	) t1
	,(
		SELECT count(SGC.GelisTarihi) Personel
		FROM TServiseGelenCihaz SGC
		WHERE SGC.DelDate IS NULL
			AND DATEPART(YEAR, SGC.GelisTarihi) = @Yil
		) t2
	,(
		SELECT count(SGCM.GelisTarihi) Musteri
		FROM TServiseGelenCihaz_M SGCM
		WHERE SGCM.DelDate IS NULL
			AND DATEPART(YEAR, SGCM.GelisTarihi) = @Yil
		) t3", new[]{                               
                 new SqlParameter("@Yil",yillar.Yil)
                                                 });

                while (x.Read())
                {
                    seri1.Points.Add(new SeriesPoint("Servis", x[0].ToString()));
                    seri2.Points.Add(new SeriesPoint("Personel", x[1].ToString()));
                    seri3.Points.Add(new SeriesPoint("Müşteri", x[2].ToString()));

                }
                
                chartControl1.Series.AddRange(new Series[] { seri1, seri2, seri3 });

                seri1.LabelsVisibility = DefaultBoolean.True;
                seri2.LabelsVisibility = DefaultBoolean.True;
                seri3.LabelsVisibility = DefaultBoolean.True;

                ((XYDiagram3D)chartControl1.Diagram).ZoomPercent = 170;
                ((XYDiagram3D)chartControl1.Diagram).RuntimeRotation = true;                
            }
        } 
        #endregion


    }
}