using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAP.Connector;
using sap;
using System.IO;

/// <summary>
/// Класс для извлечения данных из сапа. Сразу формируются списки сотрудников с их информацией.
/// </summary>
public class SAPDB
{
    private BAPIRET2Table ErrorMessage;
    private ZHR_POLIST_RFC_STRTable EmployeeList;

    private SAPProxy sap_proxy;
    private SAPConnection con;


    public SAPDB()	
    {
        this.con = new SAPConnection();
        this.sap_proxy = new SAPProxy(con.SAPDestination.ConnectionString);
        this.ErrorMessage = null;
        this.EmployeeList = null;

    }
    
    // получаем список сотрудников из сапа
    public List<EmployeeAttrib> getEmployeeAttribsList(string periodStart, string periodEnd, string tab_num, string role)
    {
        this.ErrorMessage = new BAPIRET2Table();
        this.EmployeeList = new ZHR_POLIST_RFC_STRTable();

        this.sap_proxy.Zitc_Rfc_Get_Data_From_Portal(periodStart, periodEnd, role, tab_num, ref this.ErrorMessage, ref this.EmployeeList);
        
        if (ErrorMessage.Count != 0)
            return null;

        List<EmployeeAttrib> emp_list = new List<EmployeeAttrib>();

        int count = this.EmployeeList.Count;
        int i;

        for (i = 0; i < count; i++)
        {

            EmployeeAttrib emp = new EmployeeAttrib(this.EmployeeList[i].Employee_Id, this.EmployeeList[i].Start_Period, this.EmployeeList[i].Begda, this.EmployeeList[i].Endda);
            emp_list.Add(emp);
        }

        return emp_list;
    }


    // ДЛЯ ОТЧЕТОВ!!!! получаем список сотрудников из сапа и их графики и отклонения из SAP 
    public List<Employee> getEmployeeListForReports(string periodStart, string periodEnd, string tab_num, string role)
    {
        SQLDB db = new SQLDB();
        this.ErrorMessage = new BAPIRET2Table();
        this.EmployeeList = new ZHR_POLIST_RFC_STRTable();

        this.sap_proxy.Zitc_Rfc_Get_Data_From_Portal(periodStart, periodEnd, role, tab_num, ref this.ErrorMessage, ref this.EmployeeList);

        if (ErrorMessage.Count != 0)
            return null;

        List<Employee> emp_list = new List<Employee>();

        int count = this.EmployeeList.Count;
        int i;

        for (i = 0; i < count; i++)
        {
            Employee emp = db.getEmployee(this.EmployeeList[i].Employee_Id, this.EmployeeList[i].Start_Period, this.EmployeeList[i].Begda, this.EmployeeList[i].Endda);

            if (emp == null) continue;

            Employee em = emp_list.Find(delegate(Employee ep) { return ep.EmployeeID == emp.EmployeeID; });

            if (em == null)
            {
                emp_list.Add(emp);
            }
            else
            {
                if (emp.DepartmentID.Equals(em.DepartmentID))
                {
                    emp.BeginDate = emp.StartPeriod;
                    if (em.EndDate.CompareTo(emp.EndDate) == 1) emp.EndDate = em.EndDate; 
                    emp_list.Remove(em);
                    emp_list.Add(emp);
                }
                else
                    emp_list.Add(emp);
            }
        }

        return emp_list;
    }
  
    // вставляем сформированные таблицы с переработками в SAP
    public string insertOverhoursToSAP(ZTB_OVERHOURS_MAN_TYPE_TAB ManagementMethod, ZTB_OVERHOURS_T13_TYPE_TAB MethodT13, ZTB_OVERHOURS_DIV_TYPE_TAB DiversityTable)
    {
        string result = "Errors<br>";
        try
        {
            
            this.ErrorMessage = new BAPIRET2Table();
            this.sap_proxy.Zitc_Rfc_Data_Trans_From_Hrp(DiversityTable, ManagementMethod, MethodT13, ref this.ErrorMessage);

            for(int i=0; i<this.ErrorMessage.Count; i++)
                result += "Type=" + this.ErrorMessage[i].Type + "<br>" + "Message=" + this.ErrorMessage[i].Message + "<br>" + "MessageV1=" + this.ErrorMessage[i].Message_V1 + "<br>" + "MessageV2=" + this.ErrorMessage[i].Message_V2 + "<br>" + "MessageV3=" + this.ErrorMessage[i].Message_V3 + "<br>" + "MessageV4=" + this.ErrorMessage[i].Message_V4 + "<br>" + "CountRows=" + this.ErrorMessage.Count.ToString() + "<br>" + "ID=" + this.ErrorMessage[i].Id + "<br><br>";

            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        return result;
    }


    // downloaded - определяем, что мы закачиваем впервые или закачиваем блокированные записи
    public void insertDataIntoSAP(string employee_id, string start_period, string begda, string endda, Person accountant, bool downloaded)
    {
        SQLDB sql = new SQLDB();
        Date dt = new Date();
        EmployeeList emps = new EmployeeList();

        List<OverhoursDiversity> od = emps.getEmployeesOverhoursDiversity(employee_id, start_period, begda, endda, downloaded);
        List<OverhoursManagement> om = emps.getEmployeesOverhoursManagement(employee_id, start_period, begda, endda, downloaded);
        List<OverhoursMethodT13> om13 = emps.getEmployeesOverhoursMethodT13(employee_id, start_period, begda, endda, downloaded);

        
        // таблицы для закачки в SAP
        ZTB_OVERHOURS_MAN_TYPE_TAB ManagementMethod = new ZTB_OVERHOURS_MAN_TYPE_TAB();     // метод управленческий
        ZTB_OVERHOURS_T13_TYPE_TAB MethodT13 = new ZTB_OVERHOURS_T13_TYPE_TAB();            // метод Т13
        ZTB_OVERHOURS_DIV_TYPE_TAB DiversityTable = new ZTB_OVERHOURS_DIV_TYPE_TAB();       // разность переработок

        if (om.Count > 0)
        {

            foreach (OverhoursManagement overMan in om)
            {
                ZTB_OVERHOURS_MAN ztbMan = new ZTB_OVERHOURS_MAN();
                ztbMan.Date_Day = dt.getDataToSAP(overMan.DateDay.Day, overMan.DateDay.Month, overMan.DateDay.Year);
                ztbMan.Employee_Id = overMan.EmployeeID;
                ztbMan.Hours = overMan.Hours;

                ManagementMethod.Add(ztbMan);

                if (overMan.Downloaded == false) 
                    sql.updateNotDownloadedRecords(overMan.EmployeeID, overMan.StartPeriod, overMan.BeginDate, overMan.EndDate, accountant.TabNum, "MAN", true);
            }
        }

        if (om13.Count > 0)
        {
            foreach (OverhoursMethodT13 overT13 in om13)
            {
                ZTB_OVERHOURS_T13 ztbT13 = new ZTB_OVERHOURS_T13();
                ztbT13.Date_Day = dt.getDataToSAP(overT13.DateDay.Day, overT13.DateDay.Month, overT13.DateDay.Year);
                ztbT13.Employee_Id = overT13.EmployeeID;
                ztbT13.Hours = overT13.Hours;

                MethodT13.Add(ztbT13);

                if (overT13.Downloaded == false)
                    sql.updateNotDownloadedRecords(overT13.EmployeeID, overT13.StartPeriod, overT13.BeginDate, overT13.EndDate, accountant.TabNum, "T13", true);
            }
        }

        if (od.Count > 0)
        {
            foreach (OverhoursDiversity overDiv in od)
            {
                ZTB_OVERHOURS_DIV ztbDIVM11 = new ZTB_OVERHOURS_DIV();
                ztbDIVM11.Date_Day = dt.getDataToSAP(overDiv.DateDay.Day, overDiv.DateDay.Month, overDiv.DateDay.Year);
                ztbDIVM11.Employee_Id = overDiv.EmployeeID;
                ztbDIVM11.Hours = overDiv.Hours;
                ztbDIVM11.Type_Payment = overDiv.TypePayment;

                DiversityTable.Add(ztbDIVM11);

                if (overDiv.Downloaded == false)
                    sql.updateNotDownloadedRecords(overDiv.EmployeeID, overDiv.StartPeriod, overDiv.BeginDate, overDiv.EndDate, accountant.TabNum, "DIV", true);
            }
        }


        try
        {

            this.ErrorMessage = new BAPIRET2Table();
            this.sap_proxy.Zitc_Rfc_Data_Trans_From_Hrp(DiversityTable, ManagementMethod, MethodT13, ref this.ErrorMessage);
            


            // если возникли ошибки
            if (this.ErrorMessage.Count > 0)
            {
                
                writeErrorsToFile(this.ErrorMessage, accountant);
                
                for (int i = 0; i < this.ErrorMessage.Count; i++)
                {
                    string tabNum = this.ErrorMessage[i].Message_V1;
                    string tableCode = this.ErrorMessage[i].Message_V4;

                    if (tabNum.Length == 8)
                        switch (tableCode)
                        {
                            case "DIV":
                                sql.updateNotDownloadedRecords(employee_id, start_period, begda, endda, accountant.TabNum, "DIV", false);
                                break;
                            case "MAN":
                                sql.updateNotDownloadedRecords(employee_id, start_period, begda, endda, accountant.TabNum, "MAN", false);
                                break;
                            case "T13":
                                sql.updateNotDownloadedRecords(employee_id, start_period, begda, endda, accountant.TabNum, "T13", false);
                                break;
                            default:
                                break;
                        }
                }

            }

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }


    }

    public void writeErrorsToFile(BAPIRET2Table errorTable, Person accountant)
    {

            string filename = @"C:\Inetpub\wwwroot\timeboard\Logs\log_" + DateTime.Now.ToString("dMyyyy") + ".txt";
            
            using (FileStream fs = new FileStream(filename, FileMode.Append))
            {
                StreamWriter sw = new StreamWriter(fs);

                for (int i = 0; i < errorTable.Count; i++)
                {
                    string tabNum = errorTable[i].Message_V1;
                    string message = errorTable[i].Message_V2;
                    string saptable = errorTable[i].Message_V3;
                    string tableCode = errorTable[i].Message_V4;

                    sw.WriteLine(DateTime.Now + " - Error");
                    sw.WriteLine("From: " + accountant.TabNum + " " + accountant.Netname + " " + accountant.Login);
                    sw.WriteLine("Message: " + tabNum + "  " + message + "  " + "  " + saptable + "  " + tableCode );
                    sw.WriteLine();

                }

                sw.Flush();
                sw.Close();
            }
    
    }

}
