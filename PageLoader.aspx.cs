using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// —траница дл€ отображени€ прогресса загрузки целевой страницы
/// </summary>

public partial class PageLoader : System.Web.UI.Page
{
    protected string PageToLoad;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");

        // выводим доступные кнопки
        Person user = (Person)Session["User"];
        Display disp = new Display();
        //if (user.Roles == null) ContentPlaceHolder1.Visible = false;
        MenuBar.Text = disp.DisplayMenuBar(user.Roles);

        if (Request.QueryString["url"] != null)
        {
            EncryptedQueryString QueryString = new EncryptedQueryString(Request.QueryString["url"]);
            PageToLoad = QueryString["page"];
        }
        
    }
}
