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
/// Класс для сортировки сотрудников по ФИО по возрастанию
/// </summary>
public class EmployeeComparerByFullnameASC: IComparer<Employee>
{
    public int Compare(Employee emp1, Employee emp2)
    {
        if (emp1 == null)
        {
            if (emp2 == null)
            {
                // If emp1 is null and emp2 is null, they're
                // equal. 
                return 0;
            }
            else
            {
                // If emp1 is null and emp2 is not null, emp2
                // is greater. 
                return -1;
            }
        }
        else
        {
            // If emp1 is not null...
            //
            if (emp2 == null)
            // ...and emp2 is null, emp1 is greater.
            {
                return 1;
            }
            else
            {
                // ...and emp2 is not null, compare  
                
                return emp1.FullName.CompareTo(emp2.FullName);
                
                
            }
        }
        return 0;
    }
}
