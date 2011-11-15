using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

public partial class ReportFactTimeALL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["User"] == null) Response.Redirect("Default.aspx");
        
        Date dt = new Date();
        SAPDB db = new SAPDB();
        SQLDB sqldb = new SQLDB();
        PeriodDB perdb = new PeriodDB();
        Person user = (Person)Session["User"];

        string role = "";
        string month_id = "";
        string year = "";
        string start_date = "";
        string end_date = "";

        /*if (Request.QueryString["rft"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["rft"]);
            if (QueryString["role"] != null) role = QueryString["role"].ToString();
            if (QueryString["month_id"] != null) month_id = QueryString["month_id"].ToString();
            if (QueryString["year"] != null) year = QueryString["year"].ToString();
            if (QueryString["start_date"] != null) start_date = QueryString["start_date"].ToString();
            if (QueryString["end_date"] != null) end_date = QueryString["end_date"].ToString();
        }*/

        role = "2";
        month_id = "07";
        year = "2009";
        start_date = "20090701";
        end_date = "20090731";


        Period per = perdb.getPeriod(Convert.ToInt32(month_id), Convert.ToInt32(year));
        /*
        EmployeeList emp_list = new EmployeeList();

        List<Employee> all_employees = db.getEmployeeListForReports(start_date, end_date, user.TabNum, role);

        if (all_employees == null)
        {
            MessageBox.Show("���������� ������������ �����! ������ ����������� �� ������ ����!");
            return;
        }

        EmployeeComparerByFullnameASC emp_comp = new EmployeeComparerByFullnameASC();
        all_employees.Sort(emp_comp);
        */

        //Response.Clear();
        //Response.Charset = "utf-8";
        //Response.ContentType = "application/vnd.ms-excel";

        

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

        writeToFile(str);

        string ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_report_deviations WHERE (start_period = @start_period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = "20090701";

        conn.Open();
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
        
            str += "<tr>";
            str += "<td align='center'>" + (int)reader["employee_id"] + "</td>";
            str += "<td align='center'>" + (string)reader["full_name"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["time_norm"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["�"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["��"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["Total_actual"] + "</td>";

            str += "<td align='center'>" + (decimal)reader["hr_overhours"] + "</td>";

            //str += "<td>" + getCompany(emp.Department) + "</td><td>" + getPath(emp.Department) + "</td><td>" + getDepartment(emp.Department) + "</td>";

            str += "</tr>";

            writeToFile(str);
        }

        reader.Close();
        str += "</table>";

        writeToFile(str);
        ////Response.Write(str);
        //Response.End();

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

    public void writeToFile(string str)
    {

        string filename = @"C:\Inetpub\wwwroot\timeboard\Logs\report_" + DateTime.Now.ToString("dMyyyy") + ".txt";

        using (FileStream fs = new FileStream(filename, FileMode.Append))
        {
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(str);

            sw.Flush();
            sw.Close();
        }

    }

}
