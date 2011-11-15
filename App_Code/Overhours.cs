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
/// Класс для часов переработок
/// </summary>
public class Overhours
{
    private string employee_id;     // id сотрудника
    private string start_period;
    private string begda;
    private string endda;
    private DateTime date_day;      // дата
    private decimal hours;          // часы переработки
    
    public Overhours(string employee_id, string start_period, string begda, string endda, DateTime date_day, decimal hours)
	{
        this.employee_id = employee_id;
        this.start_period = start_period;
        this.begda = begda;
        this.endda = endda;
        this.date_day = date_day;
        this.hours = hours;
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
        get { return begda; }
        set { begda = value; }
    }

    public string EndDate
    {
        get { return endda; }
        set { endda = value; }
    }

    public DateTime DateDay
    {
        get { return date_day; }
        set { date_day = value; }
    }

    public decimal Hours
    {
        get { return hours; }
        set { hours = value; }
    }
}
