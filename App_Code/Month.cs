using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
///  ласс дл€ мес€ца
/// </summary>
public class Month
{
	private string monthName;   // Ќазвание мес€ца  
    private int monthID;     // Ќомер мес€ца

    public Month(int monthID, string monthName)
	{
        this.monthID = monthID;
        this.monthName = monthName;
	}

    public string MonthName
    {
        get { return monthName; }
        set { monthName = value; }
    }

    public int MonthID
    {
        get { return monthID; }
        set { monthID = value; }
    }
}
