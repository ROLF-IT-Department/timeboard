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
///  ласс дл€ получени€ мес€цев из базы SQL
/// </summary>
public class MonthDB
{
	private string ConnectionString;

    public MonthDB()
    {
        this.ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;
    }

    // получаем список мес€цев
    public List<Month> getMonths()
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_months";
        SqlCommand cmd = new SqlCommand(sql, conn);
        List<Month> months = new List<Month>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Month m = new Month((int)reader["id"], (string)reader["name"]);
                months.Add(m);
            }
            reader.Close();
            return months;

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

    // получаем название мес€ца по его id 
    public string getMonthName(int id)
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_months WHERE (id = @id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string name = (string)reader["name"];
            reader.Close();
            return name;
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
