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
///  ласс таблицы √рафики работы
/// </summary>
public class Schedule
{
	private string employee_id;         // табельный номер сотрудника
    private string day_period;          // номер дн€ в мес€це
    private string start_period;        // начало периода
    private string schedule_type;       // недельный график работы
    private string day_schedule;        // дневной график работф
    private string day_schedule_var;    // изменение в дневном графике работы
    private string time_begin;          // врем€ начала работы
    private string time_end;            // врем€ конца работы
    private Decimal time_hours;         // часы работы по графику
    
    public Schedule(string employee_id, string day_period, string start_period, string schedule_type, string day_schedule, string day_schedule_var, string time_begin, string time_end, Decimal time_hours)
	{
        this.employee_id = employee_id;
        this.day_period = day_period;
        this.start_period = start_period;
        this.schedule_type = schedule_type;
        this.day_schedule = day_schedule;
        this.day_schedule_var = day_schedule_var;
        this.time_begin = time_begin;
        this.time_end = time_end;
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

    public string ScheduleType
    {
        get { return schedule_type; }
        set { schedule_type = value; }
    }

    public string DaySchedule
    {
        get { return day_schedule; }
        set { day_schedule = value; }
    }

    public string DayScheduleVar
    {
        get { return day_schedule_var; }
        set { day_schedule_var = value; }
    }

    public string TimeBegin
    {
        get { return time_begin; }
        set { time_begin = value; }
    }

    public string TimeEnd
    {
        get { return time_end; }
        set { time_end = value; }
    }

    public Decimal TimeHours
    {
        get { return time_hours; }
        set { time_hours = value; }
    }
}
