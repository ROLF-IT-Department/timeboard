using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class CheckEmployees : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void submit_Click(object sender, EventArgs e)
    {
        Auth auth = new Auth();
        SQLDB db = new SQLDB();
        string netname = Netname.Text.ToUpper();
        string login = Login.Text.ToUpper();
        string password = Password.Text;
        string tab_num = auth.Authentication(netname, login, password);
        if (tab_num != null)
        {
            List<Role> roles = auth.getRoles(tab_num);
            Person pers = new Person(null, login, password, tab_num, auth.getRoles(tab_num));
            Session["User"] = pers;
            if (roles != null)
            {
                db.insertLogon(tab_num, DateTime.Now, Request.UserHostAddress, Request.UserAgent);
                Response.Redirect("TimeTable.aspx?role=" + roles[0].RoleID);
            }
            else MessageBox.Show("Вам не присвоена роль!");
        }
        else MessageBox.Show("Неправильный логин или пароль!");
    }
}
