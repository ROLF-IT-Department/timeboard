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
using System.IO;
using System.Security.Principal;



public partial class _Default : System.Web.UI.Page, ICallbackEventHandler 
{
    protected string StartDateOfPeriod;         // начало периода
    protected string EndDateOfPeriod;           // конец периода
    protected List<TimekeeperHours> hours;      // часы табельщика
    protected List<HRHours> overhours;          // часы переработок
    protected List<EmployeeAttrib> employeeAttribs; // атрибуты сотрудников
    protected Person user;
    protected int count_days;                   // количество дней в мес€це
    protected string month;                     // номер мес€ца
    protected string year;                      // год
    protected string role;                      // id роли
    protected bool check;                       // признак проверки соответстви€ графиков

    protected List<Post> posts;
    protected List<Department> departments;
    protected List<Sector> sectors;

    // входной параметр в ajax функции
    private string eventArgument;
    public void RaiseCallbackEvent(string eventArgument)
    {
        this.eventArgument = eventArgument;
    }

    // процедура обработки ajax вызова - формирование списка сотрудников
    public string GetCallbackResult()
    {
        
        Display disp = new Display();
        EmployeeList emp_list = new EmployeeList();
        string argum = eventArgument;
        string msg = "";

        if (argum.Contains("navigate"))
        {
            string[] url = argum.Split('|');
            return url[1];
        }

        if (argum.Contains("save"))
        {
            string data = argum.Substring(argum.IndexOf('|') + 1);
            SaveTimeboard(data);
        }

        if (argum.Contains("close_period"))
        {
            string data = argum.Substring(0, argum.IndexOf('|'));
            if (data.Contains("all"))
                data = "all";
            else
                data = argum.Substring(argum.IndexOf('|') + 1);
            msg = ClosePeriod(data);
        }

        if (argum.Contains("refresh"))
        {
            string result_html = "";

            if (argum.Contains("department"))
            {
                result_html = "<div id='departments'>";
                foreach (Department dep in departments)
                {
                    result_html += disp.DisplayDepartmentName(dep.DepartmentName, dep.DepartmentID, StartDateOfPeriod);
                    result_html += "<div  id='" + dep.DepartmentID + "' style='display:none'></div>";
                }
                result_html += "</div>";
            }
            else
            {
                result_html = "<div id='departments'>";
                foreach (Sector sec in sectors)
                {
                    result_html += "<div id='sectors'>" + disp.DisplaySectorName(sec.SectorName, sec.SectorID, StartDateOfPeriod) + "</div>";
                    result_html += "<div  id='" + sec.SectorID + "' style='display:none'></div>";
                }
                result_html += "</div>";
            }
            result_html += "||" + msg;
            return result_html;
        }

        List<Employee> employeesByDepartment = null; 

        string arg = eventArgument;
        string[] id = arg.Split('|');

        if (id[0].Equals("dep"))
        {
            employeesByDepartment = emp_list.getEmployeesOfDepartment(this.employeeAttribs, id[1], StartDateOfPeriod);
        }
        else
        {
            employeesByDepartment = emp_list.getEmployeesOfSector(this.employeeAttribs, id[1], StartDateOfPeriod);
        }

        string html = id[1];
        html += "||";

        string img_status = "green_small.gif";
        string alt_status = "открыт";
        bool is_closed_emp = false;

        SQLDB sql = new SQLDB();

        foreach (Employee em in employeesByDepartment)
        {
            
            int status = sql.getStatusOverhours(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate);

            switch (status)
            {
                case 0:
                    img_status = "green_small.gif";         // открыт
                    alt_status = "открыт";
                    is_closed_emp = false;
                    break;
                case 1:
                    img_status = "yellow_small.gif";        // частично закрыт
                    alt_status = "блокирован";
                    is_closed_emp = true;
                    break;
                case 2:
                    img_status = "red_small.gif";           // закрыт
                    alt_status = "закрыт";
                    is_closed_emp = true;
                    break;
                default:
                    break;
            }

            string checkbox = "";
            
            if (role.Equals("3"))
                checkbox = "<input type='checkbox' class='checkbox_close' name='" + em.EmployeeID + "+" + em.StartPeriod + "+" + em.BeginDate + "+" + em.EndDate + "' value=''>";

            int count = sql.getCountNotes(Convert.ToInt32(em.EmployeeID));
            string scount = count.ToString();
            if (count == 0) scount = "&nbsp;";

            html += "<table class='employee'><tr><td class='employee_status'><img alt='" + alt_status + "' src='App_Resources/" + img_status + "'><div class='bookmark' onclick='window.open(\"Bookmark.aspx?eid=" + em.EmployeeID + "&rid=" + this.role + "\",\"_blank\",\"menubar=no,width=400,height=510,resizable=no,scrollbars=no\")'><span class='comment_count'>" + scount + "</span></div>" + checkbox + "</td><td class='employee_post'><div class='employee_post_div' title='" + em.Post + "'>" + em.Post + "</div></td><td class='employee_id'>" + DeleteZeroFromEmployeeID(em.EmployeeID) + "</td><td class='employee_name'><div class='employee_name_div' title='" + em.FullName + "'><img src='App_Resources/person.gif'>&nbsp;<span onclick='window.open(\"" + getCardUrl(em) + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")'>" + em.FullName + CheckRedundantEmployee(em) + "</span></div></td><td>" + disp.DisplaySchedules(count_days, 18, em, role, this.check, is_closed_emp, month, year) + "</td></tr></table>";
           
        }

        html += "||";

        DevDescriptionDB devDB = new DevDescriptionDB();
        List<DevDescription> devdescription = devDB.getBasicDevDescription();
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

    // функци€ удал€ет первые нули в ID сотрудника
    public string DeleteZeroFromEmployeeID(string EmployeeID)
    {
        string newEmployeeID = null;
        int i = 0;
        while (EmployeeID[i].Equals('0'))
            i++;
        newEmployeeID = EmployeeID.Substring(i);
        
        return newEmployeeID;
    }
    


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User"] == null) Response.Redirect("Default.aspx");

        Display disp = new Display();
        Date dt = new Date();
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        
        this.user = (Person)Session["User"];

        this.year = DateTime.Now.Year.ToString();
        this.month = dt.getMonthToday();


        ////getReportExcelFactTime();
        //return;


        string fio = "";
        string tab_number = "";
        string post = "";
        string status = "";
        this.check = false;
        
        if (Request.QueryString["role"] != null) this.role = Request.QueryString["role"].ToString();
        if (Request.QueryString["month"] != null) this.month = Request.QueryString["month"].ToString();
        if (Request.QueryString["year"] != null) this.year = Request.QueryString["year"].ToString();
        if (Request.QueryString["check"] != null) this.check = true;
        if (Request.QueryString["post"] != null) post = Request.QueryString["post"].ToString();
        if (Request.QueryString["fio"] != null) fio = Request.QueryString["fio"].ToString();
        if (Request.QueryString["tab"] != null) tab_number = Request.QueryString["tab"].ToString();
        if (Request.QueryString["status"] != null) status = Request.QueryString["status"].ToString();

        Period per = perdb.getPeriod(Convert.ToInt32(month), Convert.ToInt32(year));
        this.count_days = dt.getCountDays(Convert.ToInt32(this.month), Convert.ToInt32(this.year));

        lbPeriod.Text = fillMonths();

        string start_date = dt.getSAPStartPeriodDate(month, year);
        string end_date = dt.getSAPEndPeriodDate(month, year);

        StartDateOfPeriod = start_date;
        EndDateOfPeriod = end_date;


        Label lb = new Label();
        lb.Text = disp.DisplayDays(count_days, 18);
        days.Controls.Add(lb);

        setButtons(role);

        EmployeeList emp_list = new EmployeeList();
        posts = new List<Post>();

        this.employeeAttribs = db.getEmployeeAttribsList(start_date, end_date, user.TabNum, this.role);

        posts = emp_list.getPostList(this.employeeAttribs);
        lbPost.Text = fillPosts(posts);

        if ((post != "") && (post != "¬—≈"))
            this.employeeAttribs =  emp_list.getEmployeesOnCurrentPost(post, this.employeeAttribs);

        if (fio != "")
            this.employeeAttribs = emp_list.getEmployeesByFIO(fio, this.employeeAttribs);
        
        if (tab_number != "")
            this.employeeAttribs = emp_list.getEmployeesByTab(tab_number, this.employeeAttribs);

        if (status != "")
            this.employeeAttribs = emp_list.getEmployeesByStatus(status, this.employeeAttribs);

        if (check)
        {
            List<EmployeeAttrib> emp_attr_list = new List<EmployeeAttrib>();
            foreach(EmployeeAttrib emp_attr in this.employeeAttribs)
                if (!emp_list.checkEmployee(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate, count_days))
                    emp_attr_list.Add(emp_attr);
            this.employeeAttribs = emp_attr_list;
        }

       
        departments = new List<Department>();
        sectors = new List<Sector>();

        emp_list.getDepartmentSector(this.employeeAttribs, ref departments, ref sectors);

        if (this.employeeAttribs != null)
        {
                Label depart = new Label();
                depart.Text = "<div id='departments' style='display:block'>";
                foreach (Department dep in departments)
                {
                    depart.Text += disp.DisplayDepartmentName(dep.DepartmentName, dep.DepartmentID, StartDateOfPeriod);
                    depart.Text += "<div  id='" + dep.DepartmentID + "' style='display:none'></div>";

                }
                depart.Text += "</div>";
                content.Controls.Add(depart);
                
        }


        // ссылка на табель в отдельном окне
        //lbTabel.Text = "<img src='App_Resources/doc.gif' alt='' style='cursor:pointer; cursor: hand;' /><span style='cursor:pointer; cursor: hand; font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold;'  onclick='window.open(\"" + getTabelUrl() + "\",\"displayWindow\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")'>&nbsp;ќткрыть в новом окне</span>";
        lbTabel.Text = "<img src='App_Resources/doc.gif' alt='' style='cursor:pointer; cursor: hand;' /><span style='cursor:pointer; cursor: hand; font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold;' >&nbsp;ќткрыть в новом окне</span>";

        // отчет по отклонени€м
        lbReportFactTime.Text = "<img alt='' src='App_Resources/excel.bmp' onclick='window.open(\"" + getReportFactTimeUrl() + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")' style='cursor: hand; cursor: pointer; padding-left: 20px; top: 2px; position:relative;' />&nbsp;<span onclick='window.open(\"" + getReportFactTimeUrl() + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")' style='cursor: hand; cursor: pointer; font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold;'>ќтчет по отклонени€м</span>";

        // отчет по рабочему времени
        lbReportTimeboard.Text = "<img alt='' src='App_Resources/excel.bmp' onclick='window.open(\"" + getReportTimeboardUrl() + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")' style='cursor: hand; cursor: pointer; padding-left: 20px; top: 2px; position:relative;' />&nbsp;<span onclick='window.open(\"" + getReportTimeboardUrl() + "\",\"_blank\",\"menubar=no,width=800,height=600,resizable=yes,scrollbars=yes\")' style='cursor: hand; cursor: pointer; font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold;'>ќтчет по рабочему времени</span>";

        // ajax на загрузку списка сотрудников определенного подразделени€      
        string cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientCallback", "context");
        string func_name = "FillEmployeesCallback";
        string cbScript = "function " + func_name + "(arg, context)" + "{" + cbReference + ";" + "}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), func_name, cbScript, true);


        string cbReference2 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientRefreshCallback", "context");
        string func_name2 = "RefreshCallback";
        string cbScript2 = "function " + func_name2 + "(arg, context)" + "{" + cbReference2 + ";" + "}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), func_name2, cbScript2, true);

        string cbReference3 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientNavigateCallback", "context");
        string func_name3 = "NavigateCallback";
        string cbScript3 = "function " + func_name3 + "(arg, context)" + "{" + cbReference3 + ";" + "}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), func_name3, cbScript3, true);
        
        
    }


    // формируем url с параметрами
    public string getCardUrl(Employee emp)
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", StartDateOfPeriod);
        QueryString.Add("end_date", EndDateOfPeriod);
        QueryString.Add("role", role);
        QueryString.Add("month_id", this.month);
        QueryString.Add("year", this.year);
        QueryString.Add("emp_id", emp.EmployeeID);
        QueryString.Add("emp_begda", emp.BeginDate);
        QueryString.Add("emp_endda", emp.EndDate);
        QueryString.Add("count_days", count_days.ToString());
        string url = "Card.aspx?card=" + QueryString.ToString();
        return url;
    }

    // формируем url с параметрами
    public string getReportFactTimeUrl()
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", StartDateOfPeriod);
        QueryString.Add("end_date", EndDateOfPeriod);
        QueryString.Add("role", role);
        QueryString.Add("month_id", this.month);
        QueryString.Add("year", this.year);
        string url = "ReportFactTime.aspx?rft=" + QueryString.ToString();
        return url;
    }


    // формируем url с параметрами
    public string getReportTimeboardUrl()
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", StartDateOfPeriod);
        QueryString.Add("end_date", EndDateOfPeriod);
        QueryString.Add("role", role);
        QueryString.Add("month_id", this.month);
        QueryString.Add("year", this.year);
        string url = "ReportTimeboard.aspx?rtb=" + QueryString.ToString();
        return url;
    }

    // формируем url с параметрами
    public string getTabelUrl()
    {
        /*EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("role", role);
        QueryString.Add("month", period.SelectedItem.Value);
        QueryString.Add("count_days", count_days.ToString());
        string url = "Tabel.aspx?data=" + QueryString.ToString();*/
        return "";
    }


    private string fillMonths()
    {
        Date dt = new Date();
        List<Period> periods = dt.getPeriodsToDropDownList();
        string monthID = "";

        string html = "<select id='period' class='ddl_period' onchange='onChangePeriod()'>";

        string selected = "";

        foreach (Period p in periods)
        {
            if (p.MonthID == Convert.ToInt32(this.month)) selected = "selected";
            monthID = p.MonthID.ToString();
            if (monthID.Length == 1) monthID = monthID.Insert(0, "0");
            string value = monthID + "|" + p.Year.ToString() + "|" + this.role;
            html += "<option value='" + value + "' " + selected + ">" + p.MonthName.ToUpper() + "</option>";
            selected = "";
        }

        html += "</select>";

        return html;
    }
    
    // заполн€ем список должностей
    private string fillPosts(List<Post> post_list)
    {
        string html = "<select id='post' style='width:180px' onchange='onChangePost()'>";

        html += "<option value=''>¬ыберите должность</option>";

        foreach (Post p in post_list)
        {
            html += "<option value='" + p.Post_Name + "'>" + p.Post_Name + "</option>";
        }

        html += "</select>";

        return html;  
    }


    protected void SaveTimeboard(string data)
    {
        SQLDB db = new SQLDB();
        Date dt = new Date();
        PeriodDB perdb = new PeriodDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month), Convert.ToInt32(this.year));
        EmployeeList emp_list = new EmployeeList();
        EmployeeAttrib emp_attr = null;
        Employee em = null;
        List<TimekeeperHours> current = null;
        int count_days = dt.getCountDays(Convert.ToInt32(month), Convert.ToInt32(year));
        string[] inputs = data.Split(';');
        int i = 0;
        int len = inputs.Length-1;
        for (i = 0; i < len; i++)
        {
            string[] input = inputs[i].Split('=');
            string[] input_id = input[0].Split('+');

            //                  0           1           2               3           4        5
            // ѕор€док ID = employee_id + post_id + department_id + begin_date + end_date + day
            

            string day = input_id[5];

            if (day.Equals("1"))
            {
                emp_attr = new EmployeeAttrib(input_id[0], StartDateOfPeriod, input_id[3], input_id[4]);
                em = emp_list.getEmployee(emp_attr);
                current = emp_list.getEmployeesTimekeeperHours(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate);
            }

            TimekeeperHours th = current.Find(delegate(TimekeeperHours ht) { return ht.Day == day; });
            
            string hour = input[1];
            decimal h = -1;
            string sym = "";

            if ((hour != "") && (hour != null))
            {
                if (HasSymbols(hour))
                {
                    if (th == null)
                        db.insertTimekeeperHoursAndSymbols(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate, h, hour, user.TabNum, em.DepartmentID, em.PostID, day, month, year, per.PeriodID);
                    else
                        if (!th.Symbols.Equals(hour))
                            db.updateTimekeeperHoursAndSymbols(user.TabNum, h, hour, th.ID);
                }
                else
                {
                    h = Convert.ToDecimal(hour);

                    if (th == null)
                        db.insertTimekeeperHoursAndSymbols(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate, h, sym, user.TabNum, em.DepartmentID, em.PostID, day, month, year, per.PeriodID);
                    else
                        if (th.Hours != h)
                            db.updateTimekeeperHoursAndSymbols(user.TabNum, h, sym, th.ID);
                }

            }

            if ((hour == "") && (th != null)) db.deleteTimekeeperHours(th.ID);

            
        }
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


    // установка прав на кнопки
    protected void setButtons(string roleID)
    {
        
        switch (roleID)
        {
            case "1":
                lbRole.Text = "“абельщик";
                break;
            case "2":
                lbRole.Text = "HR";
                break;
            case "3":
                lbCloseButton.Text = "<img alt='' id='btClose' src='App_Resources/button_close.bmp' onclick='onClosePeriodClick()' onmouseover='src=\"App_Resources/button_close_yellow.bmp\"' onmouseout='src=\"App_Resources/button_close.bmp\"' style='cursor: hand; cursor: pointer; display:block;' />";
                lbRole.Text = "Ѕухгалтер";
                break;
            default:
                lbRole.Text = "нет";
                break;
        }
    
    }


    // обработчик кнопки закрыти€ периода
    protected string ClosePeriod(string data)
    {
        string msg = "";
        
        //Person user = (Person)Session["User"];
        PeriodDB perdb = new PeriodDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month), Convert.ToInt32(this.year));
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        //MonthDB mondb = new MonthDB();
        //string month_name = mondb.getMonthName(Convert.ToInt32(this.month_id));
        EmployeeList emp_list = new EmployeeList();

        if (data.Equals("all"))
        {
            List<EmployeeAttrib> all_emp_attr = db.getEmployeeAttribsList(StartDateOfPeriod, EndDateOfPeriod, this.user.TabNum, this.role);
            foreach (EmployeeAttrib emp_attr in all_emp_attr)
            {
                if (emp_list.checkEmployee(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate, this.count_days))
                {
                    int status = sql.getStatusOverhours(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate);

                    switch (status)
                    {
                        case 0:     // открыт
                            int res = emp_list.insertOvertimeHoursToDB(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate, this.count_days, Convert.ToInt32(this.month), Convert.ToInt32(this.year), this.user.TabNum, per.PeriodID);
                            // if (res > 0)
                            //    db.insertDataIntoSAP(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate, this.user, true);
                            break;
                        case 1:     // частично закрыт / блокирован
                            // db.insertDataIntoSAP(emp_attr.EmployeeID, emp_attr.StartPeriod, emp_attr.BeginDate, emp_attr.EndDate, this.user, false);
                            break;
                        case 2:     // закрыт
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    msg += "¬нимание! ” сотрудника " + DeleteZeroFromEmployeeID(emp_attr.EmployeeID) + " не совпадают графики!\r\n";
                }
            }
        }
        else
        {
            string employee_id = "";
            string start_period = "";
            string begda = "";
            string endda = "";
            string[] inputs = data.Split(';');
            int i = 0;
            int len = inputs.Length - 1;
            for (i = 0; i < len; i++)
            {
                string[] input_emp = inputs[i].Split('+');

                //                  0              1           2       3
                // ѕор€док ID = employee_id + start_period + begda + endda 

                employee_id = input_emp[0];
                start_period = input_emp[1];
                begda = input_emp[2];
                endda = input_emp[3];

                if (emp_list.checkEmployee(employee_id, start_period, begda, endda, this.count_days))
                {
                    int status = sql.getStatusOverhours(employee_id, start_period, begda, endda);

                    switch (status)
                    {
                        case 0:     // открыт
                            int res = emp_list.insertOvertimeHoursToDB(employee_id, start_period, begda, endda, this.count_days, Convert.ToInt32(this.month), Convert.ToInt32(this.year), this.user.TabNum, per.PeriodID);
                            // if (res > 0)
                            //    db.insertDataIntoSAP(employee_id, start_period, begda, endda, this.user, true);
                            break;
                        case 1:     // частично закрыт
                            // db.insertDataIntoSAP(employee_id, start_period, begda, endda, this.user, false);
                            break;
                        case 2:     // закрыт
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    msg += "¬нимание! ” сотрудника " + DeleteZeroFromEmployeeID(employee_id) + " не совпадают графики!\r\n";
                }


            }
        }
        
        return msg;
    }


    // ‘ормируем отчет по плану-факту
    protected void getReportExcelFactTime()
    {

        Person user = (Person)Session["User"];
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        //Period per = perdb.getPeriod(Convert.ToInt32(month), Convert.ToInt32(year));

        Period per = perdb.getPeriod(6, 2009);
        //List<Employee> all_employees = db.getEmployeeList(StartDateOfPeriod, EndDateOfPeriod, user.TabNum, this.role);

        List<Employee> all_employees = db.getEmployeeListForReports("20090601", "20090630", "37735", "2");


        if (all_employees == null)
        {
            MessageBox.Show("Ќевозможно сформировать отчет! —писок сотрудников за период пуст!");
            return;
        }

        ExcelFile exc = new ExcelFile();
        
        exc.GetReportFactTime(all_employees, per);
       
        //exc.GetReportTimeboard(all_employees,per);

    }



}
