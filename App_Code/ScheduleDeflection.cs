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
/// Класс таблицы отклонений от графика
/// </summary>
public class ScheduleDeflection
{
	private string employee_id;         // табельный номер сотрудника
    private string day_period;          // порядковый номер дня в месяце
    private string start_period;        // начало периода
    private string abs_att_type;        // тип отклонения в графике
    private string code_t13;            // кодовое обозначение отклонения
    private Decimal time_hours;         // время отклонения
    
    public ScheduleDeflection(string employee_id, string day_period, string start_period, string abs_att_type, string code_t13, Decimal time_hours)
	{
        this.employee_id = employee_id;
        this.day_period = day_period;
        this.start_period = start_period;
        this.abs_att_type = abs_att_type;
        this.code_t13 = code_t13;
        this.time_hours = time_hours;
	}

    public string EmployeeID
    {
        get { return employee_id; }
        set { employee_id = value; }
    }

    public string DayPeriod
    {
        get { return day_period; }
        set { day_period = value; }
    }

    public string StartPeriod
    {
        get { return start_period; }
        set { start_period = value; }
    }

    public string AbsAttType
    {
        get { return abs_att_type; }
        set { abs_att_type = value; }
    }

    public string CodeT13
    {
        get { return code_t13; }
        set { code_t13 = value; }
    }

    public Decimal TimeHours
    {
        get { return time_hours; }
        set { time_hours = value; }
    }
}
