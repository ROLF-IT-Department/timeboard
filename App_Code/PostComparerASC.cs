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
///  ласс сортировки должностей по возрастанию
/// </summary>
public class PostComparerASC: IComparer<Post>
{
    public int Compare(Post p1, Post p2)
    {
        if (p1 == null)
        {
            if (p2 == null)
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
            if (p2 == null)
            // ...and emp2 is null, emp1 is greater.
            {
                return 1;
            }
            else
            {
                // ...and emp2 is not null, compare  

                return p1.Post_Name.CompareTo(p2.Post_Name);

                
            }
        }
        return 0;
    }
}
