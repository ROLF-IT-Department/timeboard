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
/// Класс для получения строки соединения с SAP
/// </summary>
public class SAPConnection
{
    private SAP.Connector.Destination dest;
    
    public SAPConnection()
	{
        this.dest = new SAP.Connector.Destination();
        dest.Client = Convert.ToInt16(ConfigurationManager.AppSettings["SAPClient"]);
        dest.AppServerHost = ConfigurationManager.AppSettings["SAPAppServerHost"];
        dest.SystemNumber = Convert.ToInt16(ConfigurationManager.AppSettings["SAPSystemNumber"]);
        dest.Username = ConfigurationManager.AppSettings["SAPUsername"];
        dest.Password = ConfigurationManager.AppSettings["SAPPassword"];
        dest.Language = "RU";
	}

    public SAP.Connector.Destination SAPDestination
    {
        get { return dest; }
        set { dest = value; }
    }
}
