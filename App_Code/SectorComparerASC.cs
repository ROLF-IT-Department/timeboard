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
public class SectorComparerASC : IComparer<Sector>
{
    public int Compare(Sector d1, Sector d2)
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


                return d1.SectorName.CompareTo(d2.SectorName);

                
            }
        }
        return 0;
    }
}
