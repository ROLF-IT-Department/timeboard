using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Tabel : System.Web.UI.Page , ICallbackEventHandler
{
    protected string StartDateOfPeriod;
    protected string EndDateOfPeriod;
    protected List<Employee> employees;
    protected List<TimekeeperHours> hours;
    protected List<HRHours> overhours;
    protected int count_days;
    protected string month;
    protected string year;
    protected string role;
    protected bool closed;
    protected bool check;

    private string eventArgument;
    public void RaiseCallbackEvent(string eventArgument)
    {
        this.eventArgument = eventArgument;
    }

    public string GetCallbackResult()
    {
        //SAPDB db = new SAPDB();
        Display disp = new Display();
        //List<Employee> emps = db.getEmployeeListForCurrentDepartment(StartDateOfPeriod, EndDateOfPeriod, "00000050", "1", eventArgument);
        EmployeeList emp_list = new EmployeeList();
        List<Employee> emps = null;// emp_list.getEmployeesOfDepartment(eventArgument, employees);
        EmployeeComparerByPostASC cmpByPostASC = new EmployeeComparerByPostASC();
        emps.Sort(cmpByPostASC);
        string html = "";
        html += eventArgument;
        html += "||";
        //int k = 0;
        //for (int j = 0; j < 100; j++)
        /*foreach (Employee em in emps)
        {
            //k++;
            html += "<table cellpadding='0' cellspacing='0' border='0' class='employee'><tr><td class='employee_post'><div style='width: 105px; height: 80px; overflow: hidden;'>" + em.Post + "</div></td><td class='employee_id'>" + em.EmployeeID + "</td><td class='employee_name' ><img src='App_Resources/person.gif' style='cursor: hand; cursor: pointer;'>&nbsp<span style='cursor: hand; cursor: pointer;'  onclick='window.open(\"" + getCardUrl(em) + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")'>" + em.FullName + CheckRedundantEmployee(em) + "</span></td><td>" + disp.DisplaySchedules(count_days, 18, em, em.PostID, eventArgument, hours, overhours, em.Schedule, em.ScheduleDeflection, role, check, closed, false) + "</td></tr></table>";

        }
        */
        html += "||";

        DevDescriptionDB devDB = new DevDescriptionDB();
        List<DevDescription> devdescription = devDB.getDevDescription();
        html += "<table cellpadding='0' cellspacing='0' border='0' width='100%'><tr><td align='center' colspan='2'><b>ќбозначени€ в SAP</b></td></tr>";

        foreach (DevDescription dev in devdescription)
            html += "<tr><td width='30px' align='center'><b>" + dev.Symbols + "</b></td><td>" + dev.Name + "</td></tr>";

        html += "</table>";


        return html;
    }

    public string CheckRedundantEmployee(Employee emp)
    {
        string msg = " ";
        Date dt = new Date();
        DateTime end_date = dt.getDateFromSAPToSQL(emp.EndDate);
        DateTime last_date_month = dt.getEndPeriodDate(month, year);

        if ((DateTime.Now > end_date) && (end_date.Day < last_date_month.Day)) msg = "<br><span class='redundant'>(уволен с " + end_date.AddDays(1).ToString("d") + ")</span>";

        return msg;
    } 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");


        Display disp = new Display();
        Date dt = new Date();
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();

        Person user = (Person)Session["User"];


        //List<Role> rol = user.Roles;
        //foreach (Role r in rol)
        //    Response.Write(r.RoleID + " = " + r.RoleName);

        if (!IsPostBack)
        {
            fillMonths();
            if (Request.QueryString["data"] == null)
                period.SelectedValue = dt.getMonthToday();
        }

        year = period.SelectedItem.Text.Substring(period.SelectedItem.Text.Length - 4);
        month = period.SelectedItem.Value;

        string fio = "";
        string tab_number = "";
        string post = "";
        check = false;
        closed = false;

        count_days = dt.getCountDays(Convert.ToInt32(month), Convert.ToInt32(year));

        if (Request.QueryString["type"] != null) role = Request.QueryString["type"].ToString();

        Period per = perdb.getPeriod(Convert.ToInt32(month), Convert.ToInt32(year));

        if (per.IsClosed == 1)
        {
            lbStatus.Text = "закрыт";
            Traffic.Text = "<img alt='' src='App_Resources/red.bmp' style='position:relative; top:4px;' />";
            closed = true;
        }
        else Traffic.Text = "<img alt='' src='App_Resources/green.bmp' style='position:relative; top:4px;' />";

         
        if (Request.QueryString["data"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["data"]);
            if (QueryString["fio"] != null) fio = QueryString["fio"];
            if (QueryString["tab"] != null) tab_number = QueryString["tab"];
            if (QueryString["post"] != null) post = QueryString["post"];
            if (QueryString["role"] != null) role = QueryString["role"];
            if (QueryString["check"] != null) check = true;
            if (!QueryString["month"].Equals(period.SelectedItem.Value))
            {
                fio = "";
                tab_number = "";
                post = "";
                check = false;
            }
        }


        //Response.Write(month);

        //Response.Write("fio=" + fio + "tab=" + tab_number + " - " + month + "<br>");
        //Response.Write(post + "<br>");

        string start_date = dt.getSAPStartPeriodDate(month, year);
        string end_date = dt.getSAPEndPeriodDate(month, year);

        StartDateOfPeriod = start_date;
        EndDateOfPeriod = end_date;
        //Response.Write(start_date + end_date);

        Label lb = new Label();
        lb.Text = disp.DisplayDaysTabel(count_days, 18);
        days.Controls.Add(lb);

        //Response.Write(start_date + end_date);
        //Period period_time = perdb.getPeriod(Convert.ToInt32(month), Convert.ToInt32(year));

        setButtons(role);

        //Response.Write(start_date + "-" + end_date + "<br>");

        this.employees = null;/// db.getEmployeeList(start_date, end_date, user.TabNum, this.role);
        EmployeeList emp_list = new EmployeeList();
        List<Post> post_list = null;/// pl.getPosts(db.getPostList(start_date, end_date, user.TabNum, this.role));
        if (post_list != null)
        {
            fillPosts(post_list);
        }
        else
        {
            ListItem li = new ListItem("¬ыберите должность");
            ddl_post.Items.Add(li);
        }


        /*if (post != "")
        {
            if (post != "¬—≈")
            {
                EmployeeList empl = new EmployeeList();
               // this.employees = empl.getEmployeesOnCurrentPost(post, employees);

            }
        }

        if (fio != "")
        {
            EmployeeList emp_fio = new EmployeeList();
            this.employees = emp_fio.getEmployeesByFIO(fio, employees);
        }

        if (tab_number != "")
        {
            EmployeeList emp_tab = new EmployeeList();
            this.employees = emp_tab.getEmployeesByTab(tab_number, employees);
        }*/

        if (check)
        {
            EmployeeList emp_check = new EmployeeList();
            /////this.employees = emp_check.getWrongCheckedEmployees(employees, count_days, hours, overhours);
        }

        if (employees != null)
        {

            DepartmentList department_list = new DepartmentList();
            List<Department> departments = department_list.getDepartments(employees);

            
            foreach (Department dep in departments)
            {
                
                

                Label depart = new Label();
                depart.Text = disp.DisplayDepartmentName(dep.DepartmentName, dep.DepartmentID, StartDateOfPeriod);
                content.Controls.Add(depart);

                Label lbEmps = new Label();
                lbEmps.Text = "<div  id='" + dep.DepartmentID + "' style='display:none'>";

                // загрузка всех графиков при загрузке страницы
                /*EmployeeList emp_list = new EmployeeList();
                List<Employee> emps = emp_list.getEmployeesOfDepartment(dep.DepartmentID, employees);
                EmployeeComparerByPostASC cmpByPostASC = new EmployeeComparerByPostASC();
                emps.Sort(cmpByPostASC);
                //for (int j = 0; j < 100; j++)
                foreach (Employee em in emps)
                {
                    lbEmps.Text += "<table cellpadding='0' cellspacing='0' border='0' class='employee'><tr><td class='main_info_left_no'>&nbsp</td><td class='employee_post'>" + em.Post + "</td><td class='employee_id'>" + em.EmployeeID + "</td><td class='employee_name'><img src='App_Resources/person.gif' style='cursor: hand; cursor: pointer;'>&nbsp<span style='cursor: hand; cursor: pointer;'  onclick='window.open(\"" + getCardUrl(em.EmployeeID) +"\",\"displayWindow\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")'>" + em.FullName + "</span></td><td class='employee_grid_line_center'>&nbsp;</td><td>" + disp.DisplaySchedules(count_days, 18, em.EmployeeID, dep.DepartmentID, hours, overhours, em.Schedule, em.ScheduleDeflection, role, check, closed) + "</td><td class='main_info_right_no'></td></tr></table>";
                        
                }*/
                lbEmps.Text += "</div>";
                content.Controls.Add(lbEmps);

            }

        }


        // ajax на загрузку списка сотрудников определенного подразделени€
        string cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientCallback", "context");
        string func_name = "FillEmployeesCallback";
        string cbScript = "function " + func_name + "(arg, context)" + "{" + cbReference + ";" + "}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), func_name, cbScript, true);
        
        
        
    }

    public string getCardUrl(Employee emp)
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", StartDateOfPeriod);
        QueryString.Add("end_date", EndDateOfPeriod);
        QueryString.Add("role", role);
        QueryString.Add("month_id", period.SelectedItem.Value);
        QueryString.Add("date", period.SelectedItem.Text);
        QueryString.Add("emp_id", emp.EmployeeID);
        QueryString.Add("emp_begda", emp.BeginDate);
        QueryString.Add("emp_endda", emp.EndDate);
        QueryString.Add("count_days", count_days.ToString());
        string url = "Card.aspx?card=" + QueryString.ToString();
        return url;
    }


    private void fillMonths()
    {
        if (period.Items.Count != 0) return;

        Date dt = new Date();
        List<Period> periods = dt.getPeriodsToDropDownList();
        string monthID = "";
        foreach (Period p in periods)
        {
            monthID = p.MonthID.ToString();
            if (monthID.Length == 1) monthID = monthID.Insert(0, "0");
            ListItem li = new ListItem(p.MonthName.ToUpper(), monthID);
            period.Items.Add(li);
        }

        //Response.Write("fillMonths<br>");

        if (Request.QueryString["data"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["data"]);
            foreach (string key in QueryString.Keys)
                if (key == "month") period.SelectedValue = QueryString[key];
        }


    }

    private void fillPosts(List<Post> post_list)
    {

        foreach (Post p in post_list)
        {
            ListItem li = new ListItem(p.Post_Name, p.Post_Name);
            ddl_post.Items.Add(li);
        }

    }

    protected void bt_filter_fio_Click(object sender, EventArgs e)
    {

        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("fio", filter_fio_text.Text);
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        //Server.Transfer("TimeTable.aspx?data=" + QueryString.ToString());
        Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());
    }

    protected void bt_filter_tab_Click(object sender, EventArgs e)
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("tab", filter_tab_text.Text);
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        //Server.Transfer("TimeTable.aspx?data=" + QueryString.ToString(`));
        Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());
    }



    protected void period_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_post.Items.Clear();
        Person user = (Person)Session["User"];
        SAPDB db = new SAPDB();
        EmployeeList emp_list = new EmployeeList();
        List<Post> post_list = null;/// pl.getPosts(db.getPostList(StartDateOfPeriod, EndDateOfPeriod, user.TabNum, this.role));
        ListItem li = new ListItem("¬ыберите должность");
        ddl_post.Items.Add(li);
        if (post_list != null) fillPosts(post_list);

        PeriodDB perdb = new PeriodDB();
        Period per = perdb.getPeriod(Convert.ToInt32(month), Convert.ToInt32(year));

        if (per.IsClosed == 1) lbStatus.Text = "закрыт";
        else lbStatus.Text = "открыт";

    }


    protected void Refresh_Click(object sender, ImageClickEventArgs e)
    {
        //RefreshPage();
    }

    protected void RefreshPage()
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        //Server.Transfer("TimeTable.aspx?data=" + QueryString.ToString());
        Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());
    }


    protected void ddl_post_SelectedIndexChanged(object sender, EventArgs e)
    {

        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("post", ddl_post.SelectedItem.Value);
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        //Server.Transfer("TimeTable.aspx?data=" + QueryString.ToString());

        Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());

    }


    protected void Save_Click(object sender, ImageClickEventArgs e)
    {
        //Response.Write(Request.Form["test"]);
        Person user = (Person)Session["User"];
        PeriodDB perdb = new PeriodDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month), Convert.ToInt32(this.year));


        if (this.employees != null)
        {

            DepartmentList department_list = new DepartmentList();
            List<Department> departments = department_list.getDepartments(employees);

            foreach (Department dep in departments)
            {
                EmployeeList emp_list = new EmployeeList();
                List<Employee> emps = null;//// emp_list.getEmployeesOfDepartment(dep.DepartmentID, employees);
                EmployeeComparerByPostASC cmpByPostASC = new EmployeeComparerByPostASC();
                emps.Sort(cmpByPostASC);
                foreach (Employee em in emps)
                {
                    EmployeeList emplist = new EmployeeList();
                    List<TimekeeperHours> current = null;//emplist.getEmployeesTimekeeperHours(em);
                    List<HRHours> hr_current = null;/// emplist.getEmployeesHRHours(em);

                    for (int i = 1; i <= count_days; i++)
                    {
                        string textbox_id = "tb" + dep.DepartmentID + em.PostID + em.EmployeeID + i.ToString();
                        //string hr_id = "hr" + dep.DepartmentID + em.EmployeeID + i.ToString();
                        
                        Date dt = new Date();
                        SQLDB db = new SQLDB();
                        
                        string hour = Request.Form[textbox_id];
                        //string overhour = Request.Form[hr_id];

                        TimekeeperHours th = current.Find(delegate(TimekeeperHours ht) { return ht.Day == i.ToString(); });

                        
                        decimal h = -1;
                        string sym = "";

                        if ((hour != "") && (hour != null))
                        {
                            if (HasSymbols(hour))
                            {
                                if (th == null)
                                    db.insertTimekeeperHoursAndSymbols(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate, h, hour, user.TabNum, dep.DepartmentID, em.PostID, i.ToString(), month, year, per.PeriodID);
                                else
                                    if (!th.Symbols.Equals(hour))
                                        db.updateTimekeeperHoursAndSymbols(user.TabNum, h, hour, th.ID);
                            }
                            else
                            {
                                h = Convert.ToDecimal(hour);

                                if (th == null)
                                    db.insertTimekeeperHoursAndSymbols(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate, h, sym, user.TabNum, dep.DepartmentID, em.PostID, i.ToString(), month, year, per.PeriodID);
                                else
                                    if (th.Hours != h)
                                        db.updateTimekeeperHoursAndSymbols(user.TabNum, h, sym, th.ID);
                            }
                        }

                        if ((hour == "") && (th != null)) db.deleteTimekeeperHours(th.ID);

                        
                    }
                }

            }
        }


        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        //Server.Transfer("TimeTable.aspx?data=" + QueryString.ToString());
        //Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());

    }

    // провер€ем данные - число это или строка
    bool HasSymbols(string hour)
    {
        string alphabet = "јаЅб¬в√гƒд≈е®Є∆ж«з»и…й кЋлћмЌнќоѕп–р—с“т”у‘ф’х÷ц„чЎшўщЏъџы№ьЁэёюя€";

        for (int j = 0; j < hour.Length; j++)
            if (alphabet.Contains(hour[j].ToString()))
                return true;
        return false;
    }

    protected void setButtons(string roleID)
    {

        switch (roleID)
        {
            case "1":
                Check.Visible = true;
                ClosePeriod.Visible = false;
                break;
            case "2":
                Check.Visible = true;
                ClosePeriod.Visible = false;
                break;
            case "3":
                Check.Visible = true;
                ClosePeriod.Visible = true;
                break;
            default:
                Check.Visible = false;
                ClosePeriod.Visible = false;
                break;
        }

    }


    protected void Check_Click(object sender, ImageClickEventArgs e)
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("role", role);
        QueryString.Add("check", "true");
        Response.Redirect("Tabel.aspx?data=" + QueryString.ToString());
    }


    protected void ClosePeriod_Click(object sender, ImageClickEventArgs e)
    {

        /*
        Person user = (Person)Session["User"];
        PeriodDB perdb = new PeriodDB();
        SAPDB db = new SAPDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month), Convert.ToInt32(this.year));

        if (per.IsClosed == 1)
        {
            MessageBox.Show("ѕериод уже закрыт!");

        }
        else
        {

            MonthDB mondb = new MonthDB();
            string month_name = mondb.getMonthName(Convert.ToInt32(this.month));
            List<Employee> all_employees = db.getEmployeeList(StartDateOfPeriod, EndDateOfPeriod, user.TabNum, this.role);
            if (all_employees == null)
            {
                MessageBox.Show("Ќевозможно закрыть период! —писок сотрудников за период пуст!");
            }
            else
            {
                EmployeeList emp_check = new EmployeeList();
                this.employees = emp_check.getWrongCheckedEmployees(all_employees, count_days, hours, overhours);
                if (this.employees.Count > 0)
                    MessageBox.Show("Ќевозможно закрыть период! ѕроверьте графики сотрудников!");
                else
                {

                    perdb.updatePeriodIsClosed(per.PeriodID, 2);
                    emp_check.getOvertimeHours(all_employees, count_days, hours, overhours, Convert.ToInt32(this.month), Convert.ToInt32(this.year), user.TabNum, per.PeriodID);
                    MessageBox.Show("ѕериод за " + month_name.ToLower() + " " + this.year + " года закрыт!");
                }
            }
        }
        ddl_post.Items.Clear();

        PostList pl = new PostList();
        List<Post> post_list = pl.getPosts(db.getPostList(StartDateOfPeriod, EndDateOfPeriod, user.TabNum, this.role));
        ListItem li = new ListItem("¬ыберите должность");
        ddl_post.Items.Add(li);
        if (post_list != null)
        {
            fillPosts(post_list);
        }

        //RefreshPage();
         */
    }

}
