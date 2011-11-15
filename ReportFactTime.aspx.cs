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

public partial class ReportFactTime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");
        
        Date dt = new Date();
        SAPDB db = new SAPDB();
        SQLDB sql = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        Person user = (Person)Session["User"];

        string role = "";
        string month_id = "";
        string year = "";
        string start_date = "";
        string end_date = "";

        if (Request.QueryString["rft"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["rft"]);
            if (QueryString["role"] != null) role = QueryString["role"].ToString();
            if (QueryString["month_id"] != null) month_id = QueryString["month_id"].ToString();
            if (QueryString["year"] != null) year = QueryString["year"].ToString();
            if (QueryString["start_date"] != null) start_date = QueryString["start_date"].ToString();
            if (QueryString["end_date"] != null) end_date = QueryString["end_date"].ToString();
        }

        Period per = perdb.getPeriod(Convert.ToInt32(month_id), Convert.ToInt32(year));

        EmployeeList emp_list = new EmployeeList();

        List<Employee> all_employees = db.getEmployeeListForReports(start_date, end_date, user.TabNum, role);

        if (all_employees == null)
        {
            MessageBox.Show("���������� ������������ �����! ������ ����������� �� ������ ����!");
            return;
        }

        EmployeeComparerByFullnameASC emp_comp = new EmployeeComparerByFullnameASC();
        all_employees.Sort(emp_comp);

        Response.Clear();
        Response.Charset = "utf-8";
        Response.ContentType = "application/vnd.ms-excel";

        

        string str = @"<table cellspacing='0' cellpadding='0' border='1'>
                            <tr>
                                <td colspan='25' style='border: 0px; height: 30px; font-weight: bold;' align='center' valign='middle'>
                                    �����
                                </td>
            <td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan='25' style='border: 0px; height: 30px;' align='center' valign='middle'>";
        str += "�� ���������� ������������� ������� �� " + per.MonthName.ToUpper() + " " + per.Year + " ����</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";

        str += @"<tr>
                    <td rowspan='2' style='background-color: #99CCFF; font-weight: bold;' align='center'>
                            ���. �����
                    </td>
                     <td rowspan='2' style='background-color: #99CCFF; font-weight: bold;' align='center'>
                            ���
                    </td>
                     <td rowspan='2' style='width:80px; background-color: #99CCFF; font-weight: bold;' align='center'>
                            ���-�� ������� ����� �� ������� (�����)
                    </td>
                    <td colspan='20' style='height: 30px;  background-color: #99CCFF; font-weight: bold;' align='center'>
                          ����������
                    </td>
                    <td rowspan='2' style='width:80px; background-color: #99CCFF; font-weight: bold;' align='center'>
                            ����� ���������� ����� �� ������ (����)
                    </td>
                     <td rowspan='2' style='width:80px; background-color: #99CCFF; font-weight: bold;' align='center'>
                            ����� ���������� ����������� �����
                    </td>
        <td rowspan='2'>&nbsp;</td><td rowspan='2'>&nbsp;</td><td rowspan='2'>&nbsp;</td>
                </tr>
                <tr>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ��������� ������������������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            �������� ��� (������������ ������) � ��������� ����������� ���
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ����� ������� �� ���� ���������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            �������� �� ����� ���������� ��������������� ��� ������������ ������������ �������� ����������������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ ��� ���������� ���������� �����, ��������������� ��������� �� ���������� ������������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ��������� ������������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ �� ������������ �������� (�� ��������� �������������)
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ����� ������� �� ��������, �� ��������� �� ������������ � ���������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            �������������� �������� ��� (������������)
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ��������� �������������� ������������ ������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ �� ����� �� �������� �� ���������� �� �������� ���� ���
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ ��� ���������� ���������� ����� ��� ��������, ��������������� ����������� ����������������� ���������� ���������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ��������� �������� ������������ ������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ��������� ������������ � ������� �� ������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������� (���������� �� ������� ����� ��� ������������ ������ � ������� �������, �������������� �����������������)
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ �� ������������ � ����� (������ � ����� � ������������ �������������� �������)
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ����� ������� �� ���� ������������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            �������������� ������ � ����� � ��������� � ����������� �������� ��������� ����������, ����������� ������ � ���������
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            �������������� ������ � ����� � ��������� ��� ���������� ���������� �����
                    </td>
                    <td style='width:80px; background-color: #FFFF99; font-weight: bold;' align='center'>
                            ������ � �������� � ����������� ���
                    </td>

                </tr>
                ";


        foreach(Employee emp in all_employees)
        {
            decimal norma = 0;
            decimal overhours = 0;
            decimal def_sum = 0;

            Deflections d = new Deflections();

            List<HRHours> hr_current = sql.getHRHours(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<Schedule> schedule = sql.getSchedule(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);
            List<ScheduleDeflection> dschedule = sql.getScheduleDeflection(emp.EmployeeID, emp.StartPeriod, emp.BeginDate, emp.EndDate);



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
            str += "<tr>";
            str += "<td align='center'>" + DeleteZeroFromEmployeeID(emp.EmployeeID) + "</td>";
            str += "<td align='center'>" + emp.FullName + "</td>";

            if (norma != 0) str += "<td align='center'>" + norma.ToString() + "</td>";
            else  str += "<td>&nbsp;</td>";

            if (d.def_B != 0) str += "<td align='center'>" + d.def_B.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_V != 0) str += "<td align='center'>" + d.def_V.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_VP != 0) str += "<td align='center'>" + d.def_VP.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_G != 0) str += "<td align='center'>" + d.def_G.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_DO != 0) str += "<td align='center'>" + d.def_DO.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_K != 0) str += "<td align='center'>" + d.def_K.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_NN != 0) str += "<td align='center'>" + d.def_NN.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_NP != 0) str += "<td align='center'>" + d.def_NP.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_OV != 0) str += "<td align='center'>" + d.def_OV.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_OD != 0) str += "<td align='center'>" + d.def_OD.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_OJ != 0) str += "<td align='center'>" + d.def_OJ.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_OZ != 0) str += "<td align='center'>" + d.def_OZ.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_OT != 0) str += "<td align='center'>" + d.def_OT.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_PK != 0) str += "<td align='center'>" + d.def_PK.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_PR != 0) str += "<td align='center'>" + d.def_PR.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_R != 0) str += "<td align='center'>" + d.def_R.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_RP != 0) str += "<td align='center'>" + d.def_RP.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_U != 0) str += "<td align='center'>" + d.def_U.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_UD != 0) str += "<td align='center'>" + d.def_UD.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (d.def_RV != 0) str += "<td align='center'>" + d.def_RV.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if ((norma - def_sum) != 0) str += "<td align='center'>" + (norma - def_sum).ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            if (overhours != 0) str += "<td align='center'>" + overhours.ToString() + "</td>";
            else str += "<td>&nbsp;</td>";

            str += "<td>" + getCompany(emp.Department) + "</td><td>" + getPath(emp.Department) + "</td><td>" + getDepartment(emp.Department) + "</td>";

            str += "</tr>";

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
        return names[names.Length - 1];
    }

    public string getPath(string path)
    {
        return path.Remove(0, path.IndexOf('/') + 1);
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
