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
using System.Data.Common;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DevDescriptionDB
/// </summary>
public class DevDescriptionDB
{
    private string ConnectionString;

    public DevDescriptionDB()
    {
        this.ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;
    }

    public List<DevDescription> getDevDescription()
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_dev_description";
        SqlCommand cmd = new SqlCommand(sql, conn);
        List<DevDescription> description = new List<DevDescription>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DevDescription d = new DevDescription((int)reader["id"], (string)reader["symbols"], (string)reader["name"]);
                description.Add(d);
            }
            reader.Close();
            return description;

        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return null;
    }

    public List<DevDescription> getBasicDevDescription()
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_dev_description WHERE additional = 0";
        SqlCommand cmd = new SqlCommand(sql, conn);
        List<DevDescription> description = new List<DevDescription>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DevDescription d = new DevDescription((int)reader["id"], (string)reader["symbols"], (string)reader["name"]);
                description.Add(d);
            }
            reader.Close();
            return description;

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return null;
    }
}
