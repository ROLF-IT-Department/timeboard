using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Init(object sender, EventArgs e)
    {
        // авторизаци€ Windows        
        Auth auth = new Auth();
        SQLDB db = new SQLDB();
        string netname = "";
        string user = User.Identity.Name.ToUpper();
        if (user.Contains("\\"))
            netname = user.Substring(user.IndexOf('\\') + 1, user.Length - user.IndexOf('\\') - 1);
        string tab_num = auth.Authentication(netname, "", "");
        //string tab_num = auth.Authentication(user, "", "");
        if (tab_num != null)
        {
            List<Role> roles = auth.getRoles(tab_num);
            Person pers = new Person(user, null, null, tab_num, roles);
            Session["User"] = pers;
            if (roles != null)
            {
                db.insertLogon(tab_num, DateTime.Now, Request.UserHostAddress, Request.UserAgent);
                Response.Redirect("TimeTable.aspx?role=" + roles[0].RoleID);
            }
            else MessageBox.Show("¬ам не присвоена роль! ");
        }
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void submit_Click(object sender, EventArgs e)
    {
        // авторизаци€ по логину-паролю
        Auth auth = new Auth();
        SQLDB db = new SQLDB();
        if ((Login.Text == "") && (Password.Text == "")) return;
        if ((Login.Text.Length > 100) || (Password.Text.Length > 100)) return;
        string login = Login.Text.ToUpper();
        string password = Password.Text;
        string tab_num = auth.Authentication("", login, password);
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
            else MessageBox.Show("¬ам не присвоена роль!");
        }
        else MessageBox.Show("Ќеправильный логин или пароль!");
        
    }


}
