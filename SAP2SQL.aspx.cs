using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class SAP2SQL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SAPDB db = new SAPDB();

        int k = 0; /// db.insertPersonalDataToSQL("20090401", "20090430", "36386", "2");
        if (k > 0)
            Response.Write("Выгрузка в rolf_timeboard_employees_sap завершена!<br>");
        else
            Response.Write("Ошибка с выгрузкой в rolf_timeboard_employees_sap!<br>");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        SAPDB db = new SAPDB();

        int k = 0;//// db.insertSchedulesToSQL("20090401", "20090430", "36386", "2");
        if (k > 0)
            Response.Write("Выгрузка в rolf_timeboard_schedules_sap завершена!<br>");
        else
            Response.Write("Ошибка с выгрузкой в rolf_timeboard_schedules_sap!<br>");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        SAPDB db = new SAPDB();

        int k = 0;//// db.insertVarSchedulesToSQL("20090401", "20090430", "36386", "2");
        if (k > 0)
            Response.Write("Выгрузка в rolf_timeboard_dev_schedules_sap завершена!<br>");
        else
            Response.Write("Ошибка с выгрузкой в rolf_timeboard_dev_schedules_sap!<br>");
    }
}
