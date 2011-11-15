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

public partial class Bookmark : System.Web.UI.Page, ICallbackEventHandler 
{
    protected Person user = null;
    protected int employee_id;
    protected int role_id;

    // входной параметр в ajax функции
    private string eventArgument;
    public void RaiseCallbackEvent(string eventArgument)
    {
        this.eventArgument = eventArgument;
    }

    // процедура обработки ajax вызова - формирование списка сотрудников
    public string GetCallbackResult()
    {
        string argum = eventArgument;

        string date = argum.Substring(0, 10);
        string text = argum.Substring(10);
        DateTime dt = DateTime.Parse(date);
        
        SQLDB sql = new SQLDB();
        sql.insertNotes(employee_id, text, Convert.ToInt32(user.TabNum), role_id, dt);

        string url = "Bookmark.aspx?eid=" + employee_id.ToString() + "&rid=" + role_id.ToString();

        return url;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null) Response.Redirect("Default.aspx");
        this.user = (Person)Session["User"];
        SQLDB sql = new SQLDB();

        if (Request.QueryString["eid"] != null) employee_id = Convert.ToInt32(Request.QueryString["eid"].ToString());
        if (Request.QueryString["rid"] != null) role_id = Convert.ToInt32(Request.QueryString["rid"]);

        string employee_fio = sql.getFIO(employee_id);
        lbTitle.Text = employee_fio + "&nbsp;&nbsp;" + employee_id;
        
        string html = "<table class='notes-table'>";

        List<Note> notes = sql.getNotes(employee_id);
        int k = 0;

        foreach (Note note in notes)
        { 
            k++;
            string role = "";
            switch (note.RoleID)
            {
                case 1:
                    role = "Табельщик";
                    break;
                case 2:
                    role = "HR";
                    break;
                case 3:
                    role = "Бухгалтер";
                    break;
                default:
                    role = "нет";
                    break;
            }
            html += "<tr><td class='notes-field-num'>" + k.ToString() + "</td><td class='notes-field-fio'><div style='width: 200px; overflow: hidden;'>" + sql.getFIO(Convert.ToInt32(user.TabNum)) + "</div></td><td class='notes-field-date-record'>" + note.DateNote.ToString() + "</td></tr><tr><td class='notes-field-num'>&nbsp;</td><td class='notes-field-fio'>Роль:&nbsp;" + role + "</td><td class='notes-field-date-record'>Срок:&nbsp;" + note.DateExpire.ToString("dd.MM.yyyy") + "</td></tr><tr><td class='notes-field-text' colspan='3'><div>" + note.Text + "</div></td></tr>";
        }

        html += "</table>";

        lbNotes.Text = html;

        string cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ClientBookmarkCallback", "context");
        string func_name = "BookmarkCallback";
        string cbScript = "function " + func_name + "(arg, context)" + "{" + cbReference + ";" + "}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), func_name, cbScript, true);
        
        
    }

    // функция удаляет первые нули в ID сотрудника
    public int DeleteZeroFromEmployeeID(string EmployeeID)
    {
        string newEmployeeID = null;
        int i = 0;
        while (EmployeeID[i].Equals('0'))
            i++;
        newEmployeeID = EmployeeID.Substring(i);

        return Convert.ToInt32(newEmployeeID);
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        SQLDB sql = new SQLDB();
       // string text = TextBox1.Text;
        //sql.insertNotes(employee_id, text, Convert.ToInt32(user.TabNum), role_id, DateTime.Now);
        //Response.Redirect("Bookmark.aspx?eid=" + employee_id + "&rid=" + role_id.ToString());
        string txt = Request.Form["TextBox1"];
        MessageBox.Show(txt);
        //Response.Write(txt);
    }
}
