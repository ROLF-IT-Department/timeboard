using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ����� �������������
/// </summary>
public class Department
{
    private string depID;   // id �������������
    private string depName; // �������� ������������

    public Department(string depID, string depName)
	{
        this.depID = depID;
        this.depName = depName;
	}

    public string DepartmentID
    {
        get { return depID; }
        set { depID = value; }
    }

    public string DepartmentName
    {
        get { return depName; }
        set { depName = value; }
    }
}
