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
/// Summary description for OverhoursManagement
/// </summary>
public class OverhoursManagement
{
    private int id;
    private string employee_id;
    private string start_period;
    private string begda;
    private string endda;
    private DateTime date_day;
    private decimal hours;
    private bool downloaded;
    private string accountant_id;
    private int period_id;

    public OverhoursManagement(int id, string employee_id, string start_period, string begda, string endda, DateTime date_day, decimal hours, bool downloaded, string accountant_id, int period_id)
    {
        this.id = id;
        this.employee_id = employee_id;
        this.start_period = start_period;
        this.begda = begda;
        this.endda = endda;
        this.date_day = date_day;
        this.hours = hours;
        this.downloaded = downloaded;
        this.accountant_id = accountant_id;
        this.period_id = period_id;
    }

    public int ID
    {
        get { return id; }
        set { id = value; }
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

    public bool Downloaded
    {
        get { return downloaded; }
        set { downloaded = value; }
    }

    public string AccountantID
    {
        get { return accountant_id; }
        set { accountant_id = value; }
    }

    public int PeriodID
    {
        get { return period_id; }
        set { period_id = value; }
    }
}
