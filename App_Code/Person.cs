using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// ����� ������������ User
/// </summary>
public class Person
{
    private string netname;     // ������� ��� Windows
    private string login;       // �����
    private string password;    // ������
    private string tab_num;     // ��������� �����
    private List<Role> roles;   // ������ ����� ������������

    public Person(string netname, string login, string password, string tab_num, List<Role> roles)
    {
        this.netname = netname;
        this.login = login;
        this.password = password;
        this.tab_num = tab_num;
        this.roles = roles;
    }

    public string Netname
    {
        get { return netname; }
        set { netname = value; }
    }

    public string Login
    {
        get { return login; }
        set { login = value; }
    }

    public string Password
    {
        get { return password; }
        set { password = value; }
    }

    public string TabNum
    {
        get { return tab_num; }
        set { tab_num = value; }
    }

    public List<Role> Roles
    {
        get { return roles; }
        set { roles = value; }
    }
}
