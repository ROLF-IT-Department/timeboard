using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ×àñû ïî êàæäîìó îòêëîíåíèş
/// </summary>
public class Deflections
{
    public decimal def_B;   // Á
    public decimal def_V;   // Â - âûõîäíîé
    public decimal def_VP;  // ÂÏ
    public decimal def_G;   // Ã
    public decimal def_DO;  // ÄÎ
    public decimal def_K;   // Ê
    public decimal def_NN;  // ÍÍ
    public decimal def_NP;  // ÍÏ
    public decimal def_OV;  // ÎÂ
    public decimal def_OD;  // ÎÄ
    public decimal def_OJ;  // ÎÆ
    public decimal def_OZ;  // ÎÇ
    public decimal def_OT;  // ÎÒ
    public decimal def_PK;  // ÏÊ
    public decimal def_PR;  // ÏĞ
    public decimal def_R;   // Ğ
    public decimal def_RP;  // ĞÏ
    public decimal def_U;   // Ó
    public decimal def_UD;  // ÓÄ
    public decimal def_RV;  // ĞÂ
    
    public Deflections()
	{
        this.def_B = 0;   // Á
        this.def_V = 0;   // Â - âûõîäíîé
        this.def_VP = 0;  // ÂÏ
        this.def_G = 0;   // Ã
        this.def_DO = 0;  // ÄÎ
        this.def_K = 0;   // Ê
        this.def_NN = 0;  // ÍÍ
        this.def_NP = 0;  // ÍÏ
        this.def_OV = 0;  // ÎÂ
        this.def_OD = 0;  // ÎÄ
        this.def_OJ = 0;  // ÎÆ
        this.def_OZ = 0;  // ÎÇ
        this.def_OT = 0;  // ÎÒ
        this.def_PK = 0;  // ÏÊ
        this.def_PR = 0;  // ÏĞ
        this.def_R = 0;   // Ğ
        this.def_RP = 0;  // ĞÏ
        this.def_U = 0;   // Ó
        this.def_UD = 0;  // ÓÄ
        this.def_RV = 0;  // ĞÂ
	}
}
