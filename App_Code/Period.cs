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
/// ����� ��� ��������� �������
/// </summary>
public class Period
{
    private int periodID;       // ������������� �������
    private int monthID;        // ����� ������
    private string monthName;   // �������� ������
    private int year;           // ���
    private int is_closed;      // ������� �������� ������ �� ������
    
    public Period(int periodID, int monthID, string monthName, int year, int is_closed)
	{
        this.periodID = periodID;
        this.monthID = monthID;
        this.monthName = monthName;
        this.year = year;
        this.is_closed = is_closed;
	}

    public int PeriodID
    {
        get { return periodID; }
        set { periodID = value; }
    }

    public int MonthID
    {
        get { return monthID; }
        set { monthID = value; }
    }

    public string MonthName
    {
        get { return monthName; }
        set { monthName = value; }
    }

    public int Year
    {
        get { return year; }
        set { year = value; }
    }

    public int IsClosed
    {
        get { return is_closed; }
        set { is_closed = value; }
    }
   
}
