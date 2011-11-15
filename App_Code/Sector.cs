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
///  ласс участка табел€
/// </summary>
public class Sector
{
    private string sector_id;  // id участка
    private string sector_txt; // название участка

    public Sector(string sector_id, string sector_txt)
	{
        this.sector_id = sector_id;
        this.sector_txt = sector_txt;
	}

    public string SectorID
    {
        get { return sector_id; }
        set { sector_id = value; }
    }

    public string SectorName
    {
        get { return sector_txt; }
        set { sector_txt = value; }
    }
}
