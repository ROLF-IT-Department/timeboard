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
/// Summary description for Note
/// </summary>
public class Note
{
    private int id;
    private int employee_id;
    private string text;
    private int author_id;
    private int role_id;
    private DateTime date_expire;
    private DateTime date_note;

    public Note(int id, int employee_id, string text, int author_id, int role_id, DateTime date_expire, DateTime date_note)
	{
        this.id = id;
        this.employee_id = employee_id;
        this.text = text;
        this.author_id = author_id;
        this.role_id = role_id;
        this.date_expire = date_expire;
        this.date_note = date_note;
	}

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int EmployeeID
    {
        get { return employee_id; }
        set { employee_id = value; }
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public int AuthorID
    {
        get { return author_id; }
        set { author_id = value; }
    }
    
    public int RoleID
    {
        get { return role_id; }
        set { role_id = value; }
    }

    public DateTime DateExpire
    {
        get { return date_expire; }
        set { date_expire = value; }
    }

    public DateTime DateNote
    {
        get { return date_note; }
        set { date_note = value; }
    }
}
