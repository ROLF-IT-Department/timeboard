using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ����� ��� ������������ ����.
/// ������ ���� � SAP "��������"
/// </summary>
public class Date
{
    
    
    public Date()
	{

	}

    // �������� ��������� ���� �������� ������ � ������� SAP
    public string getSAPStartPeriodDateNow()
    {
        string d = DateTime.Now.ToString();
        string day = d.Substring(0, 2);
        d = d.Remove(0, 3);
        string month = d.Substring(0, 2);
        d = d.Remove(0, 3);
        string year = d.Substring(0, 4);
        string sap_date = year + month + "01";
        return sap_date;
    }

    // �������� �������� ���� �������� ������ � ������� SAP
    public string getSAPEndPeriodDateNow()
    {
        string d = DateTime.Now.ToString();
        string day = d.Substring(0, 2);
        d = d.Remove(0, 3);
        string month = d.Substring(0, 2);
        d = d.Remove(0, 3);
        string year = d.Substring(0, 4);
        day = (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToString();
        string sap_date = year + month + day;
        return sap_date;
    }

    // ��������� ��������� ���� ������ � ������� SAP
    public string getSAPStartPeriodDate(string curMonth, string curYear)
    {
        string sap_date = curYear + curMonth + "01";
        return sap_date;
    }

    // ��������� �������� ���� ������ � ������� SAP
    public string getSAPEndPeriodDate(string curMonth, string curYear)
    {
        string day = (DateTime.DaysInMonth(Convert.ToInt32(curYear), Convert.ToInt32(curMonth))).ToString();
        string sap_date = curYear + curMonth + day;
        return sap_date;
    }

    // ��������� �������� ���� � ������ � ������� Windows
    public DateTime getEndPeriodDate(string curMonth, string curYear)
    {      
        string day = (DateTime.DaysInMonth(Convert.ToInt32(curYear), Convert.ToInt32(curMonth))).ToString();
        return new DateTime(Convert.ToInt32(curYear), Convert.ToInt32(curMonth), Convert.ToInt32(day)); 
    }


    // ������� ����������� ���������� ���� � ������
    public int getCountDays(int month, int year)
    {
        return DateTime.DaysInMonth(year, month);
    }

    // ��������� �������� ������� � ������� - �������� ������ + ��� (������������ � ���������� ������)
    public List<Period> getPeriodsToDropDownList()
    {
        MonthDB monthDB = new MonthDB();
        PeriodDB periodDB = new PeriodDB();
        List<Period> periods = new List<Period>();

        int year = DateTime.Now.Year;
        int curMonth = DateTime.Now.Month;
        int furtherMonth = curMonth;
        int furtherYear = year;
        int day = DateTime.Now.Day;           

        if (furtherMonth == 12)
        {
             furtherMonth = 1;
             furtherYear++;
        }
        else
             furtherMonth++;

        // ���� ������� ������ ����� ������, �� ��������� ���� ������ � ����� ��������. ���� ���, �� ���������� ����� ������.
        if (!periodDB.isPeriodExists(furtherMonth, furtherYear))
             periodDB.insertNewPeriod(furtherMonth, monthDB.getMonthName(furtherMonth), furtherYear, 0);

        Period p3 = periodDB.getPeriod(furtherMonth, furtherYear);
        p3.MonthName = p3.MonthName + " " + p3.Year.ToString();
        periods.Add(p3);


        Period p1 = periodDB.getPeriod(curMonth, year);
        p1.MonthName = p1.MonthName + " " + p1.Year.ToString();
        periods.Add(p1);
        

        // ���� ������ ������, �� �������� ������ �� ������� ����������� ����
        if (curMonth == 1)
        {
            year--;
            curMonth = 12;
        }
        else curMonth--;

        Period p2 = periodDB.getPeriod(curMonth, year);
        p2.MonthName = p2.MonthName + " " + p2.Year.ToString();
        periods.Add(p2);


        /*
        Period p4 = periodDB.getPeriod(5, 2009);
        p4.MonthName = p4MonthName + " " + p4.Year.ToString();
        periods.Add(p4);
        */


        return periods;
    }

    // ��������� ���� ��� ������ � MS SQL Server
    public DateTime getDateToSQL(int day, int month, int year)
    {
        DateTime dt = new DateTime(year, month, day);
        return dt;
    }

    // ��������� ����� �������� ������
    public string getMonthToday()
    {
        string month = DateTime.Now.Month.ToString();
        if (month.Length == 1) month = month.Insert(0, "0");
        return month;
    }

    // ������������ ���� �� ������� SAP � ������ SQL
    public DateTime getDateFromSAPToSQL(string dt)
    {
        int year = Convert.ToInt32(dt.Substring(0, 4));
        dt = dt.Remove(0, 4);
        int month = Convert.ToInt32(dt.Substring(0, 2));
        dt = dt.Remove(0, 2);
        int day = Convert.ToInt32(dt);
        DateTime dt_sql = getDateToSQL(day, month, year);
        return dt_sql;
    }

    // ��������� ���� � ������� SAP
    public string getDataToSAP(int day, int month, int year)
    {
        string sap_year = year.ToString();
        string sap_month = month.ToString();
        if (sap_month.Length == 1) sap_month = sap_month.Insert(0, "0");
        string sap_day = day.ToString();
        if (sap_day.Length == 1) sap_day = sap_day.Insert(0, "0");
        string sap_date = sap_year + sap_month + sap_day;
        return sap_date;
    }
}
