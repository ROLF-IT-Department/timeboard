using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Класс сотрудника
/// </summary>
public class EmployeeAttrib
{
    private string employee_id;         // id сотрудника
    private string start_period;        // начало периода
    private string begin_date;          // дата начала действия в периоде
    private string end_date;            // дата конца действия в периоде

    public EmployeeAttrib(string employee_id, string start_period, string begin_date, string end_date)
	{
        this.employee_id = employee_id;
        this.start_period = start_period;
        this.begin_date = begin_date;
        this.end_date = end_date;
	}

    public string EmployeeID
    {
        get { return employee_id; }
        set { employee_id = value; }
    }

    public string StartPeriod
    {
        get { return start_period; }
        set { start_period = value; }
    }

    public string BeginDate
    {
        get { return begin_date; }
        set { begin_date = value; }
    }

    public string EndDate
    {
        get { return end_date; }
        set { end_date = value; }
    }

}
