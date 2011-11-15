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
/// Класс для часов табельщика
/// </summary>
public class TimekeeperHours
{
    private int id;                     // id записи
    private string employee_id;         // id сотрудника
    private decimal hours;              // проставленные часы
    private string symbols;             // буквенное обозначение отсутствия
    private string timekeeper_id;       // id табельщика
    private string department_id;       // id подразделения
    private string post_id;             // id должности
    private string day;                 // день
    private string month;               // месяц
    private string year;                // год
    
    public TimekeeperHours(int id, string employee_id, decimal hours, string symbols, string timekeeper_id, string department_id, string post_id, string day, string month, string year)
	{
        this.id = id;
        this.employee_id = employee_id;
        this.hours = hours;
        this.symbols = symbols;
        this.timekeeper_id = timekeeper_id;
        this.department_id = department_id;
        this.post_id = post_id;
        this.day = day;
        this.month = month;
        this.year = year;
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

    public decimal Hours
    {
        get { return hours; }
        set { hours = value; }
    }

    public string Symbols
    {
        get { return symbols; }
        set { symbols = value; }
    }

    public string TimekeeperID
    {
        get { return timekeeper_id; }
        set { timekeeper_id = value; }
    }

    public string DepartmentID
    {
        get { return department_id; }
        set { department_id = value; }
    }

    public string PostID
    {
        get { return post_id; }
        set { post_id = value; }
    }

    public string Day
    {
        get { return day; }
        set { day = value; }
    }

    public string Month
    {
        get { return month; }
        set { month = value; }
    }

    public string Year
    {
        get { return year; }
        set { year = value; }
    }

}
