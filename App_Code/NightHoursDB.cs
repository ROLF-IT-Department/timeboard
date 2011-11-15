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
/// Summary description for NightHoursDB
/// </summary>
public class NightHoursDB
{
    private string ConnectionString;

    public NightHoursDB()
    {
        this.ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;
    }

    public List<NightHours> getNightHours()
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_night_hours";
        SqlCommand cmd = new SqlCommand(sql, conn);
        List<NightHours> nighthours = new List<NightHours>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                NightHours n = new NightHours((string)reader["day_schedule"], (decimal)reader["night_hours"]);
                nighthours.Add(n);
            }
            reader.Close();
            return nighthours;

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


}
