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
///  ласс сотрудника
/// </summary>
public class Employee
{
    private string employee_id;         // id сотрудника
    private string start_period;        // начало периода
    private string begin_date;          // дата начала действи€ в периоде
    private string end_date;            // дата конца действи€ в периоде
    private string fullname;            // ‘»ќ сотрудника
    private string post_id;             // id должности
    private string post;                // название должности
    private string department_id;       // id подразделени€
    private string department;          // название подразделени€
    
    public Employee(string employee_id, string start_period, string begin_date, string end_date, string fullname, string post_id, string post, string department_id, string department)
	{
        this.employee_id = employee_id;
        this.start_period = start_period;
        this.begin_date = begin_date;
        this.end_date = end_date;
        this.fullname = fullname;
        this.post_id = post_id;
        this.post = post;
        this.department_id = department_id;
        this.department = department;
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

    public string FullName
    {
        get { return fullname; }
        set { fullname = value; }
    }

    public string PostID
    {
        get { return post_id; }
        set { post_id = value; }
    }

    public string Post
    {
        get { return post; }
        set { post = value; }
    }

    public string DepartmentID
    {
        get { return department_id; }
        set { department_id = value; }
    }

    public string Department
    {
        get { return department; }
        set { department = value; }
    }

}
