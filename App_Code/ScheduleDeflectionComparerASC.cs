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
/// Класс для сортировки графика отклонений по дням по возрастанию
/// </summary>
public class ScheduleDeflectionComparerASC: IComparer<ScheduleDeflection>
{
    public int Compare(ScheduleDeflection sch1, ScheduleDeflection sch2)
    {
        if (sch1 == null)
        {
            if (sch2 == null)
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
            if (sch2 == null)
            // ...and emp2 is null, emp1 is greater.
            {
                return 1;
            }
            else
            {
                // ...and emp2 is not null, compare  
                int schedule1 = Convert.ToInt32(sch1.DayPeriod);
                int schedule2 = Convert.ToInt32(sch2.DayPeriod);

                if (schedule1 > schedule2) return 1;
                if (schedule1 < schedule2) return -1;
                if (schedule1 == schedule2) return 0;
                
            }
        }
        return 0;
    }
}
