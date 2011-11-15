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
/// ����� ��� �����������
/// </summary>
public class HRHours
{
    private int id;                     // id ������ � ����
    private string employee_id;         // id ����������
    private string post_id;             // id ���������
    private string department_id;       // id  �������������
    private string day;                 // ����
    private string month;               // �����
    private string year;                // ���
    private decimal day_overhours;      // ������� ���� ����������� 
    private decimal night_overhours;    // ������ ���� ����������� 
    private string hr_id;               // id ���������� HR
    private int period_id;              // id ��������� �������


    public HRHours(int id, string employee_id, decimal day_overhours, decimal night_overhours, string hr_id, string department_id, string post_id, string day, string month, string year, int period_id)
	{
        this.id = id;
        this.employee_id = employee_id;
        this.day_overhours = day_overhours;
        this.night_overhours = night_overhours;
        this.hr_id = hr_id;
        this.department_id = department_id;
        this.post_id = post_id;
        this.day = day;
        this.month = month;
        this.year = year;
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

    public decimal DayOverHours
    {
        get { return day_overhours; }
        set { day_overhours = value; }
    }

    public decimal NightOverHours
    {
        get { return night_overhours; }
        set { night_overhours = value; }
    }

    public string HrID
    {
        get { return hr_id; }
        set { hr_id = value; }
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

    public int PeriodID
    {
        get { return period_id; }
        set { period_id = value; }
    }

}
