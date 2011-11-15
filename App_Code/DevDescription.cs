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
/// Summary description for DevDescription
/// </summary>
public class DevDescription
{
    private int id;         
    private string symbols;   // Код отсутсвия
    private string name;     // Описание отсутствия

	public DevDescription(int id, string symbols, string name)
	{
		this.id = id;
        this.symbols = symbols;
        this.name = name;
	}

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public string Symbols
    {
        get { return symbols; }
        set { symbols = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}
