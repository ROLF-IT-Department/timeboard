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
/// Класс для формирования html кода
/// </summary>
public class Display
{
    public Display()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // Показываем доступные кнопки меню в зависимости от роли пользователя
    public string DisplayMenuBar(List<Role> roles)
    {
        string html = "<table cellpadding='0' cellspacing='0' border='0' width='100%'><tr>";
        if (roles != null)
        {
            foreach (Role r in roles)
            {
                if (r.RoleID == "1") html += "<td class='menu_button' align='center' onmouseover='onButton(this);' onmouseout='offButton(this);' onclick='onChangeRole(\"TimeTable.aspx?role=1\")'>Табель</td>";
                if (r.RoleID == "2") html += "<td class='menu_button' align='center' onmouseover='onButton(this);' onmouseout='offButton(this);' onclick='onChangeRole(\"TimeTable.aspx?role=2\")'>Для HR</td>";
                if (r.RoleID == "3") html += "<td class='menu_button' align='center' onmouseover='onButton(this);' onmouseout='offButton(this);' onclick='onChangeRole(\"TimeTable.aspx?role=3\")'>Для бухгалтера</td>";
            }
        }
        html += "<td class='menu' >&nbsp;</td><td class='menu' width='100px' align='center'><span class='exit' id='exit'><a href='Help.aspx'>Справка</a></span></td>";
        html += "<td class='menu' width='100px' align='center'><span class='exit' id='exit'><a href='#' onclick='window.close()'>Выйти</a></span></td></tr></table>";
        return html;
    }

    // Показываем дни в месяце
    public string DisplayDays(int count_days, int size)
    {
        string html = "<table class='days_table'><tr>";
        for (int i = 1; i <= count_days; i++)
            html += "<td width='22px'><div style='width: " + size + "px; height: " + size + "px; border: 1px #818181 solid; '><span class='day_number'><b>" + i + "</b></span></div></td>";
        html += "<td><div style='width:30px; height: 18px; border: 1px #818181 solid; text-align: center; background-color: #ECEDD3'><span class='day_number'>Sum</span></div></td></tr></table>";
        return html;
    }

    // показываем дни в месяце для табеля в отдельном окне
    public string DisplayDaysTabel(int count_days, int size)
    {
        string html = "<table class='days_table' ><tr>";
        for (int i = 1; i <= count_days; i++)
            html += "<td width='18px' style='padding: 1px'><div style='width: " + size + "px; height: " + size + "px; border: 1px #818181 solid; '><span class='day_number'><b>" + i + "</b></span></div></td>";
        html += "<td style='padding: 1px'><div style='width:22px; height: 18px; border: 1px #818181 solid; text-align: center; background-color: #ECEDD3'><span class='day_number'>Sum</span></div></td></tr></table>";
        //html += "<td align='center' style='width: 30px'><img alt='' src='App_Resources/sum.jpg'></td></tr></table>";
        return html;
    }


    // функция для обработки десятичного числа в формате decimal(5,2)
    public string CheckDecimalNumber(string decNumber)
    {
        if (decNumber.Contains(","))
        {
            string integer = decNumber.Substring(0, decNumber.IndexOf(','));     // копируем целую часть числа
            string real = decNumber.Substring(decNumber.IndexOf(',') + 1);      // копируем НЕцелую часть числа

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

    // формируем html для графиков сотрудников
    public string DisplaySchedules(int count_days, int size, Employee emp, string role, bool check, bool is_closed_emp, string month, string year)
    {
        string html = "<table class='days_table_schedules' >";
        string time = "";
        string day_message = "";

        string timetable = "";
        string hr = "";
        string hrdoc = "";

        decimal sum_hour_timekeeper = 0;
        decimal sum_hour_schedule = 0;
        decimal sum_hour_hr = 0;

        string readonle = "";

        if (is_closed_emp == true) readonle = "readonly";

        int day = 1;

        EmployeeList emp_list = new EmployeeList();

        List<TimekeeperHours> current = emp_list.getEmployeesTimekeeperHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
        List<HRHours> hr_current = emp_list.getEmployeesHRHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
        List<Schedule> schedule = emp_list.getEmployeesSchedule(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
        List<ScheduleDeflection> dschedule = emp_list.getEmployeesScheduleDeflection(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);


        // крутимся в цикле с основным графиком
        foreach (Schedule sch in schedule)
        {


            int d = Convert.ToInt32(sch.DayPeriod);
            // если у нас день не совпадает с днем в графике, заполняю пустым квадратом
            while (d != day)
            {
                if (day == count_days) break;
                time = "";
                timetable += "<td><input type='text' class='timekeeper dim-" + (size - 2) + "' readonly name='" + emp.EmployeeID + "+" + emp.PostID + "+" + emp.DepartmentID + "+" + emp.BeginDate + "+" + emp.EndDate + "+" + day.ToString() + "' value=''></td>";
                hr += "<td ><div title='" + day.ToString() + "' class='dim-" + size + "'><span class='day_number'>" + time + "</span></div></td>";
                hrdoc += "<td><input type='text' value='' readonly maxlength='5' class='dim-" + (size - 2) + "'></td>";
                day++;
            }


            decimal hour_timekeeper = 0;
            decimal hour_schedule = 0;
            decimal hour_hr = 0;

            // если есть отклонение, пишем его, иначе выводим основной график
            ScheduleDeflection sd = dschedule.Find(delegate(ScheduleDeflection dsch) { return Convert.ToInt32(dsch.DayPeriod) == day; });
            if (sd != null)
            {

                decimal diversity = sch.TimeHours - sd.TimeHours;
                if (diversity == 0)
                {
                    // если отклонение с кодом 2000 или 2001 - Отзыв из отпуска, то в основной график проставляем часы. Это присутствие.
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
                hr += "<td><div class='dim-" + size + "'><span class='day_number'>" + time + "</span></div></td>";

            }
            else
            {
                // если у нас выходной то вставляем в квадрат букву В - в русской раскладке!!!!!!!!
                if (((sch.DaySchedule == "FREE") && (Convert.ToDecimal(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                {
                    time = "В";  // В - в русской раскладке!!!!!!!!
                    hr += "<td ><div class='dim-" + size + "' style='background-color:#DCB589;'><span class='day_number'>" + time + "</span></div></td>";
                }
                else
                {
                    time = CheckDecimalNumber(sch.TimeHours.ToString());
                    hour_schedule = Convert.ToDecimal(sch.TimeHours);
                    hr += "<td ><div class='dim-" + size + "'><span class='day_number'>" + time + "</span></div></td>";
                }
            }


            // выводим график переработок
            string bgcolor = "#D1E9E9";
            HRHours hrh = hr_current.Find(delegate(HRHours h) { return h.Day == day.ToString(); });
            string hr_value = "";
            if (hrh != null)
            {
                hour_hr = hrh.DayOverHours + hrh.NightOverHours;
                hr_value = CheckDecimalNumber(hour_hr.ToString());
            }
            if (time == "В") bgcolor = "#A4C8B7";

            if ((hrh != null) && (hrh.NightOverHours != 0)) bgcolor = "#83abdc";
            hrdoc += "<td><input type='text' value='" + hr_value + "' name='' readonly maxlength='5' class='dim-" + (size - 2) + "' style='background-color:" + bgcolor + ";'></td>";

            // выводим график табельщика
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
                    color_check = "red";
                    color_check_timekeeper = "red";
                }
            }
            if ((role == "1") && (!is_closed_emp)) timetable += "<td><span class='day_number'><input type='text' class='timekeeper dim-" + (size - 2) + "' " + readonle + " value='" + value + "' name='" + emp.EmployeeID + "+" + emp.PostID + "+" + emp.DepartmentID + "+" + emp.BeginDate + "+" + emp.EndDate + "+" + day.ToString() + "' maxlength='5' style='background-color:" + color_check_timekeeper + "' onblur='CheckHourType(this)'></span></td>";
            else timetable += "<td><input type='text' readonly value='" + value + "' name='' class='dim-" + (size - 2) + "'></td>";


            sum_hour_timekeeper += hour_timekeeper;
            sum_hour_schedule += hour_schedule;
            sum_hour_hr += hour_hr;

            day++;
        }

        while (day <= count_days)
        {
            time = "";
            timetable += "<td><input type='text' class='timekeeper dim-" + (size - 2) + "' readonly name='" + emp.EmployeeID + "+" + emp.PostID + "+" + emp.DepartmentID + "+" + emp.BeginDate + "+" + emp.EndDate + "+" + day.ToString() + "' value=''></td>";
            hr += "<td><div title='" + day.ToString() + "' style='width: " + size + "px; height: " + size + "px;'><span class='day_number'>" + time + "</span></div></td>";
            hrdoc += "<td><input type='text' value='' readonly maxlength='5' class='dim-" + (size - 2) + "' style='background-color:#D1E9E9;'></td>";
            day++;
        }

        html += "<tr class='sch_timetable'>" + timetable + "<td class='pb-2'><input type='text' value='" + CheckDecimalNumber(sum_hour_timekeeper.ToString()) + "' name='' readonly class='sum_timetable' ></td></tr>";
        html += "<tr class='sch_hr'>" + hr + "<td class='pb-2'><input type='text' value='" + CheckDecimalNumber(sum_hour_schedule.ToString()) + "' name='' readonly class='sum_timetable' ></td></tr>";
        html += "<tr class='sch_hrdoc'>" + hrdoc + "<td class='pb-2'><input type='text' value='" + CheckDecimalNumber(sum_hour_hr.ToString()) + "' name='' readonly class='sum_timetable' ></td></tr>";

        html += "</table>";

        return html;

    }


    // Выводим название отдела
    public string DisplayDepartmentName(string department_name, string departmentID, string period)
    {
        string dept = getShortDisplayDepartment(department_name);
        string id = "dep|" + departmentID + "|" + period;
        string html = "<table class='department_list'><tr><td width='30px'><img id='square" + departmentID + "' src='App_Resources/plus.bmp'  alt='Развернуть'></td><td align='left' width='750px'><div title='" + department_name + "'><span class='department_name'   onclick='HideEmployeeList(\"" + id + "\")'>" + dept + "</span></div></td><td>&nbsp;</td><td width='200px'><div><span id='ProgressMeterText" + departmentID + "' ></span><span id='ProgressMeter" + departmentID + "' ></span></div></td><td>&nbsp;</td></tr></table>";
        return html;
    }

    public string DisplaySectorName(string sector_name, string sector_id, string period)
    {
        string id = "sec|" + sector_id + "|" + period;
        string html = "<table class='department_list'><tr><td width='30px'><img id='square" + sector_id + "' src='App_Resources/plus.bmp'  alt='Развернуть'></td><td align='left' width='750px'><div title='" + sector_name + "'><span class='department_name'   onclick='HideEmployeeList(\"" + id + "\")'>" + sector_name + "</span></div></td><td>&nbsp;</td><td width='200px'><div><span id='ProgressMeterText" + sector_id + "' ></span><span id='ProgressMeter" + sector_id + "' ></span></div></td><td>&nbsp;</td></tr></table>";
        return html;
    }


    public string getShortDisplayDepartment(string department)
    {
        try
        {
            string dep = "";
            string[] d = department.Split('/');
            int k = d.Length - 1;
            dep = d[k] + " / " + d[k - 1];

            return dep;

        }
        catch
        {
            return department;
        }
    }

    /*
    // Выводим название отдела - для табеля в отдельном окне
    public string DisplayDepartmentNameTabel(string department_name, string departmentID, string emp_count)
    {
        string dept = getShortDisplayDepartment(department_name);
        
        string html = "<table cellpadding='0' cellspacing='0' border='0' class='department_list'><tr><td width='30px'><img id='square" + departmentID + "' src='App_Resources/plus.bmp'  alt='Развернуть' style='cursor: hand; cursor: pointer;' onclick='HideEmployeeList(\"" + departmentID + "\")'></td><td align='left' width='750px'><div title='" + department_name + "'><span class='department_name'   onclick='HideEmployeeList(\"" + departmentID + "\")'>" + dept + "(" + emp_count + ")</span></div></td><td>&nbsp;</td><td width='200px'><div><span id='ProgressMeterText" + departmentID + "' ></span><span id='ProgressMeter" + departmentID + "' ></span></div></td><td>&nbsp;</td></tr></table>";

        //string html = "<table cellpadding='0' cellspacing='0' border='0' class='department_list'><tr><td align='left' >&nbsp&nbsp<img id='square" + departmentID + "' src='App_Resources/plus.bmp'  alt='Развернуть' style='cursor: hand; cursor: pointer;' onclick='HideEmployeeList(\"" + departmentID + "\")'>&nbsp&nbsp<span class='department_name'   onclick='HideEmployeeList(\"" + departmentID + "\")'>" + dept + "(" + emp_count + ")</span><div style='width: 200px; position: absolute; right: 150px; '><span id='ProgressMeterText" + departmentID + "' ></span><span id='ProgressMeter" + departmentID + "' ></span></div></td></tr></table>";
        return html;
    }
    */


}
