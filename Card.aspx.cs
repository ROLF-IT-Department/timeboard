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

public partial class Card : System.Web.UI.Page
{
    protected string role = "";
    protected string month_id = "";
    protected string year = "";
    protected string start_date = "";
    protected string end_date = "";
    protected string tab_num = "";
    protected string begda = "";
    protected string endda = "";
    protected int count_days = 0;
    protected Employee employee = null;
    protected Person user = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");

        this.user = (Person)Session["User"];

        bool check = false;

        if (Request.QueryString["card"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["card"]);
            if (QueryString["role"] != null) role = QueryString["role"].ToString();
            if (QueryString["month_id"] != null) month_id = QueryString["month_id"].ToString();
            if (QueryString["year"] != null) year = QueryString["year"].ToString();
            if (QueryString["start_date"] != null) start_date = QueryString["start_date"].ToString();
            if (QueryString["end_date"] != null) end_date = QueryString["end_date"].ToString();
            if (QueryString["emp_id"] != null) tab_num = QueryString["emp_id"].ToString();
            if (QueryString["emp_begda"] != null) begda = QueryString["emp_begda"].ToString();
            if (QueryString["emp_endda"] != null) endda = QueryString["emp_endda"].ToString();
            if (QueryString["count_days"] != null) count_days = Convert.ToInt32(QueryString["count_days"]);
            if (QueryString["check"] != null) check = true;
        }


        setButtons(role);
        
        string month_name = "";
        
        month_name = month_id;

        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        EmployeeList emp_list = new EmployeeList();

        Period period_time = perdb.getPeriod(Convert.ToInt32(month_id), Convert.ToInt32(year));

        this.employee = sql.getEmployee(tab_num, start_date, begda, endda);

        int status = sql.getStatusOverhours(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate);

        string img_status = "";
        string status_msg = "";
        bool is_closed_emp = false;

        switch (status)
        {
            case 0:
                img_status = "green_small.gif";
                status_msg = "открыт";
                is_closed_emp = false;
                break;
            case 1:
                img_status = "yellow_small.gif";
                status_msg = "частично закрыт";
                is_closed_emp = true;
                break;
            case 2:
                img_status = "red_small.gif";
                status_msg = "закрыт";
                is_closed_emp = true;
                break;
            default:
                break;
        }

        

        Status.Text = "<img alt='' src='App_Resources/" + img_status + "' style='position:relative; top:4px;' />&nbsp;" + status_msg;


        List<TimekeeperHours> hours = emp_list.getEmployeesTimekeeperHours(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);
        List<HRHours> overhours = emp_list.getEmployeesHRHours(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);
        List<Schedule> schedule = emp_list.getEmployeesSchedule(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);
        List<ScheduleDeflection> dschedule = emp_list.getEmployeesScheduleDeflection(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);

        Title.Text = "“абель сотрудника за " + period_time.MonthName.ToUpper() + " " + year + " года";
        Employee.Text = employee.FullName;
        Post.Text = employee.Post;
        Department.Text = employee.Department;
        Tab.Text = DeleteZeroFromEmployeeID(employee.EmployeeID);
        WeekSchedule.Text = schedule[0].ScheduleType;

        Label lb = new Label();
        lb.Text = DisplayInfo(count_days, employee, employee.DepartmentID, hours, overhours, schedule, dschedule, role, check, is_closed_emp);
        Panel1.Controls.Add(lb);
        
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

    // функци€ дл€ обработки дес€тичного числа в формате decimal(5,2)
    public string CheckDecimalNumber(string decNumber)
    {
        if (decNumber.Contains(","))
        {
            string integer = decNumber.Substring(0, decNumber.IndexOf(','));     // копируем целую часть числа
            string real = decNumber.Substring(decNumber.IndexOf(',') + 1);      // копируем Ќ≈целую часть числа

            string result = "";

            if ((real[0] == '0') && (real[1] == '0')) return integer;

            if (real[1] == '0')
            {
                result = integer + ',' + real[0];
                return result;
            }

        }

        return decNumber;

    }

    public string DisplayInfo(int count_days, Employee emp, string depID, List<TimekeeperHours> current, List<HRHours> hr_current, List<Schedule> schedule, List<ScheduleDeflection> dschedule, string role, bool check, bool closed)
    {
        string html = "<table cellpadding='0' cellspacing='0' border='0' width='100%'>";
        string time = "";

        string timetable = "<tr>";
        string hr = "<tr>";
        string hrdoc = "<tr>";

        string hr_day = "<tr>";
        string hr_night = "<tr>";

        string daysInMonth = "<tr>";

        string timeBegin = "<tr>";
        string timeEnd = "<tr>";
        string timeHours = "<tr>";
        string daySchedule = "<tr>";
        string dayScheduleVar = "<tr>";
        string abs_att_type = "<tr>";
        string timeHoursDeflection = "<tr>";

        int day = 1;


        foreach (Schedule sch in schedule)
        {

            // если у нас день не совпадает с днем в графике, заполн€ю пустым квадратом
            
            int d = Convert.ToInt32(sch.DayPeriod);

            while (d != day)
            {
                if (day == count_days) break;
                daysInMonth += "<td class='columns' style='background-color: #CCCCCC;'><div style='width: 20px; height: 20px;'><span class='day_number'><b>" + day.ToString() + "</b></span></td>"; 
                timetable += "<td class='columns'>&nbsp</td>";
                hr += "<td class='columns' style='background-color: #FED7AB'>&nbsp</td>";
                hrdoc += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
                hr_day += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
                hr_night += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
                timeBegin += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                timeEnd += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                timeHours += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                daySchedule += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                dayScheduleVar += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                abs_att_type += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                timeHoursDeflection += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                day++;
            }


            decimal hour_timekeeper = 0;
            decimal hour_schedule = 0;
            decimal hour_hr = 0;
            decimal hour_night = 0;

            daysInMonth += "<td class='columns' style='background-color: #CCCCCC;'><div style='width: 20px; height: 20px;'><span class='day_number'><b>" + day.ToString() + "</b></span></td>"; 


            
            ScheduleDeflection sd = dschedule.Find(delegate(ScheduleDeflection dsch) { return Convert.ToInt32(dsch.DayPeriod) == day; });
            if (sd != null)
            {
                decimal diversity = sch.TimeHours - sd.TimeHours;
                if (diversity == 0)
                {
                    // если отклонение с кодом 2000 или 2001 - ќтзыв из отпуска - то в основной график проставл€ем часы. Ёто присутствие.
                    if (sd.AbsAttType.Equals("2000") || sd.AbsAttType.Equals("2001"))
                    {
                        time = CheckDecimalNumber(sd.TimeHours.ToString());
                        hour_schedule = Convert.ToDecimal(sd.TimeHours);
                    }
                    else
                        time = sd.CodeT13;
                }
                else
                {
                    if (diversity > 0)
                    {
                        time = CheckDecimalNumber(diversity.ToString());
                        hour_schedule = Convert.ToDecimal(diversity);
                    }
                    else
                    {
                        time = CheckDecimalNumber(sd.TimeHours.ToString());
                        hour_schedule = Convert.ToDecimal(sd.TimeHours);
                    }
                }
                hr += "<td class='columns' style='background-color:#FED7AB'><span class='day_number'>" + time + "</span></td>";
                abs_att_type += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + sd.AbsAttType + "</span></td>";
                timeHoursDeflection += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + sd.TimeHours.ToString() + "</span></td>";
            }            
            else
            {
                // если у нас выходной то вставл€ем в квадрат букву ¬
                if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                {
                    time = "¬";
                    hr += "<td class='columns' style='background-color:#DCB589'><span class='day_number'>" + time + "</span></td>";
                }
                else
                {
                    time = (Convert.ToDecimal(sch.TimeHours)).ToString();
                    hour_schedule = Convert.ToDecimal(sch.TimeHours);
                    hr += "<td class='columns' style='background-color:#FED7AB'><span class='day_number'>" + time + "</span></td>";
                }

                abs_att_type += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
                timeHoursDeflection += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            }


            string bgcolor = "#D1E9E9";
            HRHours hrh = hr_current.Find(delegate(HRHours h) { return h.Day == day.ToString(); });
            string hr_value = "&nbsp";
            string hr_day_value = "&nbsp";
            string hr_night_value = "&nbsp";
            if (hrh != null)
            {
                if ((hrh.DayOverHours != 0) && (hrh.NightOverHours != 0))
                {
                    hour_hr = hrh.DayOverHours + hrh.NightOverHours;
                    hr_day_value = CheckDecimalNumber(hrh.DayOverHours.ToString());
                    hr_night_value = CheckDecimalNumber(hrh.NightOverHours.ToString());
                }
                if ((hrh.DayOverHours != 0) && (hrh.NightOverHours == 0))
                {
                    hour_hr = hrh.DayOverHours;
                    hr_day_value = CheckDecimalNumber(hrh.DayOverHours.ToString());
                    hr_night_value = "&nbsp";
                }
                if ((hrh.DayOverHours == 0) && (hrh.NightOverHours != 0))
                {
                    hour_hr = hrh.NightOverHours;
                    hr_day_value = "&nbsp";
                    hr_night_value = CheckDecimalNumber(hrh.NightOverHours.ToString());
                }

                hr_value = CheckDecimalNumber(hour_hr.ToString());
            }

            if (time == "¬") bgcolor = "#A4C8B7";
            if ((hrh != null) && (hrh.NightOverHours != 0)) bgcolor = "#83abdc";
            if (role == "2")
            {
                if (closed)
                {
                    hrdoc += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_value + "</td>";
                    hr_day += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_day_value + "</td>";
                    hr_night += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_night_value + "</td>";
                }
                else
                {
                    hrdoc += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_value + "</td>";

                    if (hr_day_value == "&nbsp") hr_day_value = "";
                    if (hr_night_value == "&nbsp") hr_night_value = "";

                    hr_day += "<td class='columns' style='background-color:" + bgcolor + "'><input type='text' value='" + hr_day_value + "' name='hrday" + day.ToString() + "' maxlength='5' style='width: 20px; height: 20px; border: 1px #818181 solid; text-align: center; font-family: 'Trebuchet MS' , 'Times New Roman'; color: #000000; font-size: 8pt;' onblur='CheckHourType(this)'></td>";
                    hr_night += "<td class='columns' style='background-color:" + bgcolor + "'><input type='text' value='" + hr_night_value + "' name='hrnight" + day.ToString() + "' maxlength='5' style='width: 20px; height: 20px; border: 1px #818181 solid; text-align: center; font-family: 'Trebuchet MS' , 'Times New Roman'; color: #000000; font-size: 8pt;' onblur='CheckHourType(this)'></td>";
                } 
            }
            else
            {
                hrdoc += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_value + "</td>";
                hr_day += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_day_value + "</td>";
                hr_night += "<td class='columns' style='background-color:" + bgcolor + "'>" + hr_night_value + "</td>";
            }

            TimekeeperHours th = current.Find(delegate(TimekeeperHours h) { return h.Day == day.ToString(); });
            string value = "";
            if (th != null)
            {
                if (th.Hours >= 0)
                {
                    value = CheckDecimalNumber(th.Hours.ToString());
                    hour_timekeeper = th.Hours;
                }
                else
                {
                    value = th.Symbols;
                }
            }

            string color_check = "#EEEEEE";
            string color_check_timekeeper = "#FFFFFF";

            if (check)
            {
                decimal result = hour_timekeeper - hour_hr - hour_schedule;
                if (result != 0)
                {
                    color_check = "red";
                    color_check_timekeeper = "red";
                }

                if ((hour_timekeeper <= 0) && (!value.ToUpper().Equals(time)))
                {
                    try
                    {
                        if (Convert.ToDecimal(value) != Convert.ToDecimal(time))
                        {
                            color_check = "red";
                            color_check_timekeeper = "red";
                        }
                    }
                    catch
                    {
                        color_check = "red";
                        color_check_timekeeper = "red";
                    }
                }
            }

            if (role == "1")
            {
                if (closed)
                    if (value != "")
                        timetable += "<td class='columns' style='background-color:" + color_check + "'>" + value + "</td>";
                    else
                        timetable += "<td class='columns' style='background-color:" + color_check + "'>&nbsp</td>";
                else
                    timetable += "<td class='columns'><input type='text' value='" + value + "' name='tb" + day.ToString() + "' maxlength='5' style='height: 20px; width: 20px; border: 1px #818181 solid; text-align: center; vertical-align: middle; background-color:" + color_check_timekeeper + "' onblur='CheckHourType(this)'></td>";
            }
            else
            {
                if (value != "")
                    timetable += "<td class='columns' style='background-color:" + color_check + "'>" + value + "</td>";
                else
                    timetable += "<td class='columns' style='background-color:" + color_check + "'>&nbsp</td>";
            }


            timeBegin += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + getTime(sch.TimeBegin, sch.DayScheduleVar) + "</span></td>";
            timeEnd += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + getTime(sch.TimeEnd, sch.DayScheduleVar) + "</span></td>";
            timeHours += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + sch.TimeHours.ToString() + "</span></td>";
            daySchedule += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + sch.DaySchedule + "</span></td>";
            if (sch.DayScheduleVar != " ")
                dayScheduleVar += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>" + sch.DayScheduleVar + "</span></td>";
            else
                dayScheduleVar += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";

            day++;
        }


        while (day <= count_days)
        {
           
            daysInMonth += "<td class='columns' style='background-color: #CCCCCC;'><div style='width: 20px; height: 20px;'><span class='day_number'><b>" + day.ToString() + "</b></span></td>";
            timetable += "<td class='columns'>&nbsp</td>";
            hr += "<td class='columns' style='background-color: #FED7AB'>&nbsp</td>";
            hrdoc += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
            hr_day += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
            hr_night += "<td class='columns' style='background-color: #D1E9E9'>&nbsp</td>";
            timeBegin += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            timeEnd += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            timeHours += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            daySchedule += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            dayScheduleVar += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            abs_att_type += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            timeHoursDeflection += "<td class='columns' style='background-color: #ECEDD3'><span style='font-size: 10pt'>&nbsp</span></td>";
            day++;
        }

        html += daysInMonth + "</tr>";
        html += timetable + "</tr>";
        html += hr + "</tr>";
        html += hrdoc + "</tr>";
        html += hr_day + "</tr>";
        html += hr_night + "</tr>";
        html += timeBegin + "</tr>";
        html += timeEnd + "</tr>";
        html += timeHours + "</tr>";
        html += daySchedule + "</tr>";
        html += dayScheduleVar + "</tr>";
        html += abs_att_type + "</tr>";
        html += timeHoursDeflection + "</tr>";

        html += "</table>";

        return html;

    }

    public string getTime(string time, string var)
    {
        if (var == "F") return "0:00";
        int hour = Convert.ToInt32(time.Substring(0, 2));
        string right_time = hour.ToString() + ":" + "00";
        return right_time;
    }



    protected void Save_Click(object sender, ImageClickEventArgs e)
    {
        Person user = (Person)Session["User"];
        PeriodDB perdb = new PeriodDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month_id), Convert.ToInt32(this.year));
        Date dt = new Date();
        SQLDB db = new SQLDB();
        EmployeeList emp_list = new EmployeeList();
        List<TimekeeperHours> current = emp_list.getEmployeesTimekeeperHours(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);
        List<HRHours> hr_current = emp_list.getEmployeesHRHours(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate);
        
        for (int i = 1; i <= count_days; i++)      
        {
            string textbox_id = "tb" + i.ToString();
            string hr_day = "hrday" + i.ToString();
            string hr_night = "hrnight" + i.ToString();

            string hour = Request.Form[textbox_id];
            string day_overhour = Request.Form[hr_day];
            string night_overhour = Request.Form[hr_night];

            decimal hour_day = 0;
            decimal hour_night = 0;

            TimekeeperHours th = current.Find(delegate(TimekeeperHours ht) { return ht.Day == i.ToString(); });
            HRHours hrh = hr_current.Find(delegate(HRHours ht) { return ht.Day == i.ToString(); });

            decimal h = -1;
            string sym = "";

            if ((hour != "") && (hour != null))
            {
                if (HasSymbols(hour))
                {
                    if (th == null)
                        db.insertTimekeeperHoursAndSymbols(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate, h, hour, user.TabNum, employee.DepartmentID, employee.PostID, i.ToString(), month_id, year, per.PeriodID);
                    else
                        if (!th.Symbols.Equals(hour))
                            db.updateTimekeeperHoursAndSymbols(user.TabNum, h, hour, th.ID);
                }
                else
                {
                    h = Convert.ToDecimal(hour);

                    if (th == null)
                        db.insertTimekeeperHoursAndSymbols(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate, h, sym, user.TabNum, employee.DepartmentID, employee.PostID, i.ToString(), month_id, year, per.PeriodID);
                    else
                        if (th.Hours != h)
                            db.updateTimekeeperHoursAndSymbols(user.TabNum, h, sym, th.ID);
                }

            }

            if ((day_overhour != "") && (day_overhour != null)) hour_day = Convert.ToDecimal(day_overhour);
            if ((night_overhour != "") && (night_overhour != null)) hour_night = Convert.ToDecimal(night_overhour);

            if (hrh == null)
                db.insertHRHours(employee.EmployeeID, employee.StartPeriod, employee.BeginDate, employee.EndDate, hour_day, hour_night, user.TabNum, employee.DepartmentID, employee.PostID, i.ToString(), month_id, year, per.PeriodID);
            else
            {
                if ((hrh.DayOverHours != hour_day) || (hrh.NightOverHours != hour_night)) db.updateHRHours(user.TabNum, hour_day, hour_night, hrh.ID);
            }
           
            if ((hour == "") && (th != null)) db.deleteTimekeeperHours(th.ID);
            if ((day_overhour == "") && (night_overhour == "") && (hrh != null)) db.deleteHRHours(hrh.ID);

        }

        RefreshPage();

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


    protected void Refresh_Click(object sender, ImageClickEventArgs e)
    {
        RefreshPage();
    }

    protected void RefreshPage()
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", start_date);
        QueryString.Add("end_date", end_date);
        QueryString.Add("role", role);
        QueryString.Add("month_id", month_id);
        QueryString.Add("year", year);
        QueryString.Add("emp_id", tab_num);
        QueryString.Add("emp_begda", begda);
        QueryString.Add("emp_endda", endda);
        QueryString.Add("count_days", count_days.ToString());
        Response.Redirect("Card.aspx?card=" + QueryString.ToString());
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
        CheckProcess();
    }

    private void CheckProcess()
    {
        EncryptedQueryString QueryString = new EncryptedQueryString();
        QueryString.Add("start_date", start_date);
        QueryString.Add("end_date", end_date);
        QueryString.Add("role", role);
        QueryString.Add("month_id", month_id);
        QueryString.Add("year", year);
        QueryString.Add("emp_id", tab_num);
        QueryString.Add("emp_begda", begda);
        QueryString.Add("emp_endda", endda);
        QueryString.Add("count_days", count_days.ToString());
        QueryString.Add("check", "true");
        Response.Redirect("Card.aspx?card=" + QueryString.ToString());
    }


    // обработчик кнопки закрыти€ периода
    protected void ClosePeriod_Click(object sender, ImageClickEventArgs e)
    {
        Person user = (Person)Session["User"];
        PeriodDB perdb = new PeriodDB();
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        Period per = perdb.getPeriod(Convert.ToInt32(this.month_id), Convert.ToInt32(this.year));
        MonthDB mondb = new MonthDB();
        string month_name = mondb.getMonthName(Convert.ToInt32(this.month_id));
        EmployeeList emp_list = new EmployeeList();

        if (emp_list.checkEmployee(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate, count_days))
        {
            //MessageBox.Show("ћожно закрыть период!");
            int status = sql.getStatusOverhours(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate);

            switch (status)
            {
                case 0:     // открыт
                    int res = emp_list.insertOvertimeHoursToDB(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate, this.count_days, Convert.ToInt32(this.month_id), Convert.ToInt32(this.year), this.user.TabNum, per.PeriodID);
                    if (res > 0)
                        db.insertDataIntoSAP(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate, this.user, true);
                    RefreshPage();
                    break;
                case 1:     // частично закрыт
                    db.insertDataIntoSAP(this.employee.EmployeeID, this.employee.StartPeriod, this.employee.BeginDate, this.employee.EndDate, this.user, false);
                    RefreshPage();
                    break;
                case 2:     // закрыт
                    MessageBox.Show("ѕериод по данному сотруднику уже закрыт!");
                    break;
                default:
                    break;
            }

        }
        else
        {
            MessageBox.Show("Ќевозможно закрыть период! √рафики не совпадают!");
            return;
        }

    }

  
}
