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

public partial class ReportTimeboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");
        
        Date dt = new Date();
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        NightHoursDB nightdb = new NightHoursDB();
        Person user = (Person)Session["User"];

        string role = "";
        string month_id = "";
        string year = "";
        string start_date = "";
        string end_date = "";

        if (Request.QueryString["rtb"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["rtb"]);
            if (QueryString["role"] != null) role = QueryString["role"].ToString();
            if (QueryString["month_id"] != null) month_id = QueryString["month_id"].ToString();
            if (QueryString["year"] != null) year = QueryString["year"].ToString();
            if (QueryString["start_date"] != null) start_date = QueryString["start_date"].ToString();
            if (QueryString["end_date"] != null) end_date = QueryString["end_date"].ToString();
        }

        List<NightHours> nighthours = nightdb.getNightHours();

        Period per = perdb.getPeriod(Convert.ToInt32(month_id), Convert.ToInt32(year));


        EmployeeList emp_list = new EmployeeList();

        List<Employee> all_employees = db.getEmployeeListForReports(start_date, end_date, user.TabNum, role);

        if (all_employees == null)
        {
            MessageBox.Show("Невозможно сформировать отчет! Список сотрудников за период пуст!");
            return;
        }

        EmployeeComparerByFullnameASC emp_comp = new EmployeeComparerByFullnameASC();
        all_employees.Sort(emp_comp);

        Response.Clear();
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.ms-excel";

        

        string str = @"<table cellspacing='0' cellpadding='0' border='1'>
                            <tr>
                                <td width='30px' style='font-weight: bold;' align='center' valign='middle'>
                                    Таб. номер
                                </td>
                                <td width='100px' style='font-weight: bold;' align='center' valign='middle'>
                                    ФИО
                                </td>";

        int count_days = dt.getCountDays(per.MonthID, per.Year);

        for (int i = 1; i <= count_days; i++)
            str += "<td width='20px' style='font-weight: bold;' align='center'>" + i.ToString() + "</td><td width='20px' style='background-color: #99CCFF; font-weight: bold;' align='center' >" + i.ToString() + "a</td>";

        str += "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
        
        foreach (Employee emp in all_employees)
        {

            str += "<tr>";
            str += "<td width='30px' align='center'>" + DeleteZeroFromEmployeeID(emp.EmployeeID) + "</td>";
            str += "<td width='100px' align='center'>" + emp.FullName + "</td>";



            int day = 1;


            List<HRHours> hr_current = sql.getHRHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<Schedule> schedule = sql.getSchedule(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<ScheduleDeflection> dschedule = sql.getScheduleDeflection(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);


           foreach (Schedule sch in schedule)
           {

               decimal all_hours = 0;
               decimal night_hours = 0;

               int d = Convert.ToInt32(sch.DayPeriod);
               // если у нас день не совпадает с днем в графике, заполняю пустым квадратом


               //TimekeeperHours tkh = tkh_current.Find(delegate(TimekeeperHours h) { return Convert.ToInt32(h.Day) == day; });
               HRHours hr = hr_current.Find(delegate(HRHours hrh) { return Convert.ToInt32(hrh.Day) == day; });



               if (hr != null)
               {
                   all_hours += hr.DayOverHours + hr.NightOverHours;
                   night_hours += hr.NightOverHours;
               }

               while (d != day)
               {
                   if (day == count_days) break;

                   if (all_hours != 0) str += "<td width='20px' align='center'>" + all_hours.ToString() + "</td>";
                   else str += "<td width='20px' align='center'>&nbsp;</td>";

                   if (night_hours != 0) str += "<td width='20px' style='background-color: #99CCFF;' align='center'>" + night_hours.ToString() + "</td>"; 
                   else str += "<td width='20px' style='background-color: #99CCFF;' align='center'>&nbsp;</td>"; 
                   
                   day++;
               }

               ScheduleDeflection sd = dschedule.Find(delegate(ScheduleDeflection dsch) { return Convert.ToInt32(dsch.DayPeriod) == day; });
               if (sd != null)
               {
                   //time = CheckDecimalNumber(sd.CodeT13);

                   decimal diversity = sch.TimeHours - sd.TimeHours;
                   if (diversity != 0)
                   {
                       if (diversity > 0)
                       {
                          
                           all_hours = diversity;
                       }
                       else
                       {

                           all_hours = sd.TimeHours;
                       }
                   }

                   if (all_hours != 0) str += "<td width='20px' align='center'>" + all_hours.ToString() + "</td>";
                   else str += "<td width='20px' align='center'>&nbsp;</td>";

                   if (night_hours != 0) str += "<td width='20px' style='background-color: #99CCFF;' align='center'>" + night_hours.ToString() + "</td>";
                   else str += "<td width='20px' style='background-color: #99CCFF;' align='center'>&nbsp;</td>"; 
               }
               else
               {

                   // если выходной или изменение на выходной то пишем в отклонения по выходному дню
                   if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                   {
                       if (all_hours != 0) str += "<td width='20px' align='center'>" + all_hours.ToString() + "</td>";
                       else str += "<td width='20px' align='center'>&nbsp;</td>";

                       if (night_hours != 0) str += "<td width='20px' style='background-color: #99CCFF;' align='center'>" + night_hours.ToString() + "</td>";
                       else str += "<td width='20px' style='background-color: #99CCFF;' align='center'>&nbsp;</td>"; 
                   }
                   else
                   {
                       //decimal hour_start = Convert.ToInt32(sch.TimeBegin.Substring(0, 2));
                       //decimal hour_end = Convert.ToInt32(sch.TimeEnd.Substring(0, 2));
                       //decimal minute_start = Convert.ToDecimal(sch.TimeBegin.Substring(2, 2));
                       //decimal minute_end = Convert.ToDecimal(sch.TimeEnd.Substring(2, 2));

                       //if (hour_start == 0) hour_start = 24;
                       //if (hour_end == 0) hour_end = 24;\
                       /*
                       hour_start += minute_start / 60;
                       hour_end += minute_end / 60;

                       if (sch.TimeHours != 0)
                       {
                           if ((hour_start > 6) && (hour_end < 22))
                               all_hours += sch.TimeHours;
                           if (hour_start <= 6)
                           {
                               night_hours += 6 - hour_start - 1;
                               all_hours += sch.TimeHours;
                           }
                           if (hour_end >= 22)
                           {
                               night_hours += hour_end - 22;
                               all_hours += sch.TimeHours;
                           }
                       }
                       */

          
                       all_hours += sch.TimeHours;

                       NightHours nhour = nighthours.Find(delegate(NightHours nh) { return nh.DaySchedule.ToUpper() == sch.DaySchedule.ToUpper(); });

                       if (nhour != null)
                       {
                           night_hours += nhour.Night_Hours;
                       }

                       //day_hours += (night_hours + sch.TimeHours);
                       if (all_hours != 0) str += "<td width='20px' align='center'>" + all_hours.ToString() + "</td>";
                       else str += "<td width='20px' align='center'>&nbsp;</td>";

                       if (night_hours != 0) str += "<td width='20px' style='background-color: #99CCFF;' align='center'>" + night_hours.ToString() + "</td>";
                       else str += "<td width='20px' style='background-color: #99CCFF;' align='center'>&nbsp;</td>";
                   }
               }

               day++;
           }

           while (day <= count_days)
           {
               decimal all_hours = 0;
               decimal night_hours = 0;

               //HRHours hr = hr_current.Find(delegate(HRHours hrh) { return Convert.ToInt32(hrh.Day) == day; });

               if (all_hours != 0) str += "<td width='20px' align='center'>" + all_hours.ToString() + "</td>";
               else str += "<td width='20px' align='center'>&nbsp;</td>";

               if (night_hours != 0) str += "<td width='20px' style='background-color: #99CCFF;' align='center'>" + night_hours.ToString() + "</td>";
               else str += "<td width='20px' style='background-color: #99CCFF;' align='center'>&nbsp;</td>"; 
               
               day++;
           }

           str += "<td>" + getCompany(emp.Department) + "</td><td>" + getPath(emp.Department) + "</td><td>" + getDepartment(emp.Department) + "</td>";

           str += "</tr>";

        }

        str += "</table>";

        Response.Write(str);
        Response.End();
        
    }

    public string getCompany(string path)
    {
        string[] names = path.Split('/');
        return names[0];
    }

    public string getDepartment(string path)
    {
        string[] names = path.Split('/');
        return names[names.Length-1];
    }

    public string getPath(string path)
    {
        return path.Remove(0, path.IndexOf('/')+1);
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

    // функция удаляет первые нули в ID сотрудника
    public string DeleteZeroFromEmployeeID(string EmployeeID)
    {
        string newEmployeeID = null;
        int i = 0;
        while (EmployeeID[i].Equals('0'))
            i++;
        newEmployeeID = EmployeeID.Substring(i);

        return newEmployeeID;
    }

}
