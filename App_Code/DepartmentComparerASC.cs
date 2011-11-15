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
/// Класс сортировки отделов по возрастанию
/// </summary>
public class DepartmentComparerASC : IComparer<Department>
{
    public int Compare(Department d1, Department d2)
    {
        if (d1 == null)
        {
            if (d2 == null)
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
            if (d2 == null)
            // ...and emp2 is null, emp1 is greater.
            {
                return 1;
            }
            else
            {
                // ...and emp2 is not null, compare  

                
                string[] dep1 = d1.DepartmentName.Split('/');
                int k1 = dep1.Length - 1;
                string dp1 = dep1[k1]; 

                string[] dep2 = d2.DepartmentName.Split('/');
                int k2 = dep2.Length - 1;
                string dp2 = dep2[k2]; 


                return dp1.CompareTo(dp2);

                
            }
        }
        return 0;
    }
}
