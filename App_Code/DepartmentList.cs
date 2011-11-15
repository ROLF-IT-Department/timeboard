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

/// <summary>
/// Список подразделений
/// </summary>
public class DepartmentList
{
	public DepartmentList()
	{

	}


    // формируем список подразделений по списку сотрудников
    public List<Department> getDepartments(List<Employee> emps)
    {
        SAPDB db = new SAPDB();
        string depID = "";
        EmployeeComparerByDepartmentASC dc = new EmployeeComparerByDepartmentASC();
        emps.Sort(dc);
        List<Department> departments = new List<Department>();
        foreach (Employee em in emps)
        {
            if (depID != em.DepartmentID)
            {
                depID = em.DepartmentID;
                Department dep = null;// new Department();
                dep.DepartmentName = em.Department;
                dep.DepartmentID = em.DepartmentID;
                departments.Add(dep);
            }

        }
        return departments;
    }

}
