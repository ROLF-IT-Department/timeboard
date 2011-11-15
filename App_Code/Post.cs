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
///  ласс должности
/// </summary>
public class Post
{
    private string post_id;     // id должности
    private string post;        // название должности

	public Post(string post_id, string post)
	{
        this.post_id = post_id;
        this.post = post;
	}

    public string PostID
    {
        get { return post_id; }
        set { post_id = value; }
    }

    public string Post_Name
    {
        get { return post; }
        set { post = value; }
    }
}
