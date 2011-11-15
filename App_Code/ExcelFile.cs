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
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

/// <summary>
/// Summary description for Excel
/// </summary>
public class ExcelFile
{
    private Excel.Application thisApplication = null;
    
    public ExcelFile()
	{
        this.thisApplication = new Microsoft.Office.Interop.Excel.Application();
	}


    public void SaveReportToExcel()
    {
        Office.FileDialog dlg;
        dlg = thisApplication.get_FileDialog(Office.MsoFileDialogType.msoFileDialogSaveAs);
        if (dlg.Show() != 0)  dlg.Execute();
        thisApplication.Quit();
    }

    public void OpenReportExcelFile(string fullname)
    {
        thisApplication.Visible = false;
        thisApplication.Workbooks.Open(fullname, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        
    }

    public void GetReportFactTime(List<Employee> employees, Period per)
    {
        OpenReportExcelFile(@"C:\Inetpub\wwwroot\timeboard\Reports\ReportFactTime.xls");
        Excel.Workbook workbook = (Excel.Workbook)thisApplication.Workbooks["ReportFactTime.xls"];
        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets["����1"];

        // ����� �� ����� ������ �����
        Excel.Range caption = worksheet.get_Range("C4", "H4");
        caption.Value2 = "�� ���������� ������������� ������� �� " + per.MonthName.ToUpper() + " " + per.Year + " ����";

        // ���������� � ����� ������ ������
        int i = 10;
        int last = employees.Count + i - 1;
        
        // ���������� ������� �������
        string coord1 = "A" + i.ToString();
        string coord2 = "X" + last.ToString();

        // ������ ����� �������
        Excel.Range range = worksheet.get_Range(coord1, coord2);
        range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        range.Borders.Weight = Excel.XlBorderWeight.xlThin;

        SQLDB sql = new SQLDB();

        EmployeeComparerByFullnameASC emp_comp = new EmployeeComparerByFullnameASC();
        employees.Sort(emp_comp);

        foreach (Employee emp in employees)
        {
            decimal norma = 0;
            decimal overhours = 0;
            decimal def_sum = 0;

            Deflections d = new Deflections();

            List<HRHours> hr_current = sql.getHRHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<Schedule> schedule = sql.getSchedule(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<ScheduleDeflection> dschedule = sql.getScheduleDeflection(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);


            // ����������   
            /*decimal def_B = 0;   // �
            decimal def_V = 0;   // � - ��������
            decimal def_VP = 0;  // ��
            decimal def_G = 0;   // �
            decimal def_DO = 0;  // ��
            decimal def_K = 0;   // �
            decimal def_NN = 0;  // ��
            decimal def_NP = 0;  // ��
            decimal def_OV = 0;  // ��
            decimal def_OD = 0;  // ��
            decimal def_OJ = 0;  // ��
            decimal def_OZ = 0;  // ��
            decimal def_OT = 0;  // ��
            decimal def_PK = 0;  // ��
            decimal def_PR = 0;  // ��
            decimal def_R = 0;   // �
            decimal def_RP = 0;  // ��
            decimal def_U = 0;   // �
            decimal def_UD = 0;  // ��
            */

            foreach (Schedule sch in schedule)
            {
                // ���� �������� ��� ��������� �� �������� �� ����� � ���������� �� ��������� ���
                if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                    d.def_V += sch.TimeHours;
                else
                    norma += sch.TimeHours;
            }

            foreach (HRHours over in hr_current)
                overhours += (over.DayOverHours + over.NightOverHours);

            foreach (ScheduleDeflection sdf in dschedule)
            {
                def_sum += sdf.TimeHours;

                if (sdf.CodeT13.Equals("��"))
                {
                    def_sum -= sdf.TimeHours;       // ������� �� ���� ����� ���������� �� �����
                    overhours += sdf.TimeHours;          // ���������� � ���� ����� ������������ �����
                }

                switch (sdf.CodeT13)
                { 
                    case "�":
                        d.def_B += sdf.TimeHours; 
                        break;
                    case "��":
                        d.def_VP += sdf.TimeHours;
                        break;
                    case "�":
                        d.def_G += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_DO += sdf.TimeHours;
                        break;
                    case "�":
                        d.def_K += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_NN += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_NP += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_OV += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_OD += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_OJ += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_OZ += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_OT += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_PK += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_PR += sdf.TimeHours;
                        break;
                    case "�":
                        d.def_R += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_RP += sdf.TimeHours;
                        break;
                    case "�":
                        d.def_U += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_UD += sdf.TimeHours;
                        break;
                    case "��":
                        d.def_RV += sdf.TimeHours;
                        break;
                    default:
                        break;
                
                }
            }

            worksheet.Cells[i, 1] = emp.EmployeeID;
            worksheet.Cells[i, 2] = emp.FullName;
            worksheet.Cells[i, 3] = norma;
            worksheet.Cells[i, 4] = d.def_B;
            worksheet.Cells[i, 5] = d.def_V;
            worksheet.Cells[i, 6] = d.def_VP;
            worksheet.Cells[i, 7] = d.def_G;
            worksheet.Cells[i, 8] = d.def_DO;
            worksheet.Cells[i, 9] = d.def_K;
            worksheet.Cells[i, 10] = d.def_NN;
            worksheet.Cells[i, 11] = d.def_NP;
            worksheet.Cells[i, 12] = d.def_OV;
            worksheet.Cells[i, 13] = d.def_OD;
            worksheet.Cells[i, 14] = d.def_OJ;
            worksheet.Cells[i, 15] = d.def_OZ;
            worksheet.Cells[i, 16] = d.def_OT;
            worksheet.Cells[i, 17] = d.def_PK;
            worksheet.Cells[i, 18] = d.def_PR;
            worksheet.Cells[i, 19] = d.def_R;
            worksheet.Cells[i, 20] = d.def_RP;
            worksheet.Cells[i, 21] = d.def_U;
            worksheet.Cells[i, 22] = d.def_UD;
            worksheet.Cells[i, 23] = d.def_RV;
            worksheet.Cells[i, 24] = norma - def_sum;
            worksheet.Cells[i, 25] = overhours;
            worksheet.Cells[i, 26] = getCompany(emp.Department);
            worksheet.Cells[i, 27] = getPath(emp.Department);
            worksheet.Cells[i, 28] = getDepartment(emp.Department);


            i++;
        }

        SaveReportToExcel();
         

    }

    public void GetReportTimeboard(List<Employee> all_employees, Period per)
    {
        SQLDB sql = new SQLDB();

        Date dt = new Date();

        OpenReportExcelFile(@"C:\Inetpub\wwwroot\timeboard\Reports\ReportTimeboard.xls");
        Excel.Workbook workbook = (Excel.Workbook)thisApplication.Workbooks["ReportTimeboard.xls"];
        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets["Sheet1"];

        NightHoursDB nightdb = new NightHoursDB();
        List<NightHours> nighthours = nightdb.getNightHours();

        EmployeeComparerByFullnameASC emp_comp = new EmployeeComparerByFullnameASC();
        all_employees.Sort(emp_comp);

        int count_days = dt.getCountDays(per.MonthID, per.Year);

        worksheet.Cells[1, 1] = "���. �����";
        worksheet.Cells[1, 2] = "���";

        int j = 2;
        int i = 1;

        for (i = 1; i <= count_days; i++)
        {
            worksheet.Cells[1, ++j] = i;
            worksheet.Cells[1, ++j] = i + "a"; 
        }

        i = 2;

        foreach (Employee emp in all_employees)
        {

            worksheet.Cells[i, 1] = DeleteZeroFromEmployeeID(emp.EmployeeID);
            worksheet.Cells[i, 2] = emp.FullName;

            j = 2;

            int day = 1;

            EmployeeList emp_list = new EmployeeList();

            List<HRHours> hr_current = sql.getHRHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<Schedule> schedule = sql.getSchedule(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<ScheduleDeflection> dschedule = sql.getScheduleDeflection(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);


            foreach (Schedule sch in schedule)
            {
                j++;

                decimal all_hours = 0;
                decimal night_hours = 0;

                int d = Convert.ToInt32(sch.DayPeriod);
                // ���� � ��� ���� �� ��������� � ���� � �������, �������� ������ ���������


                HRHours hr = hr_current.Find(delegate(HRHours hrh) { return Convert.ToInt32(hrh.Day) == day; });



                if (hr != null)
                {
                    all_hours += hr.DayOverHours + hr.NightOverHours;
                    night_hours += hr.NightOverHours;
                }

                while (d != day)
                {
                    if (day == count_days) break;

                    if (all_hours != 0) worksheet.Cells[i, j] = all_hours.ToString();


                    if (night_hours != 0) worksheet.Cells[i, ++j] = night_hours.ToString();
                    else j++;

                    j++;
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


                    if (all_hours != 0) worksheet.Cells[i, j] = all_hours.ToString();


                    if (night_hours != 0) worksheet.Cells[i, ++j] = night_hours.ToString();
                    else j++;


                }
                else
                {

                    // ���� �������� ��� ��������� �� �������� �� ����� � ���������� �� ��������� ���
                    if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                    {
                        if (all_hours != 0) worksheet.Cells[i, j] = all_hours.ToString();


                        if (night_hours != 0) worksheet.Cells[i, ++j] = night_hours.ToString();
                        else j++;
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
                        }*/
        
                        all_hours += sch.TimeHours;

                        NightHours nhour = nighthours.Find(delegate(NightHours nh) { return nh.DaySchedule.ToUpper() == sch.DaySchedule.ToUpper(); });

                        if (nhour != null)
                        {
                            night_hours += nhour.Night_Hours;
                        }

                        //day_hours += (night_hours + sch.TimeHours);
                        if (all_hours != 0) worksheet.Cells[i, j] = all_hours.ToString();


                        if (night_hours != 0) worksheet.Cells[i, ++j] = night_hours.ToString();
                        else j++;
                    }
                }

                day++;
            }

            while (day <= count_days)
            {
                decimal all_hours = 0;
                decimal night_hours = 0;

                //HRHours hr = hr_current.Find(delegate(HRHours hrh) { return Convert.ToInt32(hrh.Day) == day; });

                if (all_hours != 0) worksheet.Cells[i, j] = all_hours.ToString();


                if (night_hours != 0) worksheet.Cells[i, ++j] = night_hours.ToString();
                else j++;

                j++;
                day++;
            }

            worksheet.Cells[i, ++j] = getCompany(emp.Department);
            worksheet.Cells[i, ++j] = getPath(emp.Department);
            worksheet.Cells[i, ++j] = getDepartment(emp.Department);


            i++;

        }

        SaveReportToExcel();
       
    }

    public string getCompany(string path)
    {
        string[] names = path.Split('/');
        return names[0];
    }

    public string getDepartment(string path)
    {
        string[] names = path.Split('/');
        return names[names.Length - 1];
    }

    public string getPath(string path)
    {
        return path.Remove(0, path.IndexOf('/') + 1);
    }


    // ������� ��� ��������� ����������� ����� � ������� decimal(5,2)
    public string CheckDecimalNumber(string decNumber)
    {
        if (decNumber.Contains(","))
        {
            string integer = decNumber.Substring(0, decNumber.IndexOf(','));     // �������� ����� ����� �����
            string real = decNumber.Substring(decNumber.IndexOf(',') + 1);      // �������� ������� ����� �����

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

    // ������� ������� ������ ���� � ID ����������
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
