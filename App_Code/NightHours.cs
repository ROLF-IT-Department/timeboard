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
/// Summary description for NightHours
/// </summary>
public class NightHours
{
	private string day_schedule;   // Код суточного графика
    private decimal night_hours;     // Количество ночных часов

    public NightHours(string day_schedule, decimal night_hours)
	{
		this.day_schedule = day_schedule;
        this.night_hours = night_hours;
	}

    public string DaySchedule
    {
        get { return day_schedule; }
        set { day_schedule = value; }
    }

    public decimal Night_Hours
    {
        get { return night_hours; }
        set { night_hours = value; }
    }
}
