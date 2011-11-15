using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Help : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");

        string html = "";
        DevDescriptionDB devDB = new DevDescriptionDB();
        List<DevDescription> devdescription = devDB.getDevDescription();
        html += "<center><table cellpadding='0' cellspacing='0' border='1' width='800px'><tr style='height: 30px;'><td align='center' colspan='2'><b>Обозначения в SAP</b></td></tr>";

        foreach (DevDescription dev in devdescription)
            html += "<tr style='height: 30px;'><td align='center'><b>" + dev.Symbols + "</b></td><td >" + dev.Name + "</td></tr>";

        html += "</table></center>";

        lbHelp.Text = html;

    }
}
