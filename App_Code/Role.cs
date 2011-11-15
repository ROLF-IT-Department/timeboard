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
/// Класс роли пользователя
/// </summary>
public class Role
{
    private string role_id;       // id роли
    private string role;          // название роли

	public Role(string role_id, string role)
	{
        this.role_id = role_id;
        this.role = role;
	}

    public string RoleID
    {
        get { return role_id; }
        set { role_id = value; }
    }

    public string RoleName
    {
        get { return role; }
        set { role = value; }
    }
}
