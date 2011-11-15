using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using System.Data.SqlClient;

/// <summary>
/// Класс для получения отчетного периода
/// </summary>
public class PeriodDB
{
    private string ConnectionString;

	public PeriodDB()
	{
        this.ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;
	}

    // записываем новый период(название месяца + год) в базу
    public void insertNewPeriod(int monthID, string monthName, int year, int is_closed)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_periods (month_id, month, year, is_closed) VALUES (@month_id, @monthName, @year, @is_closed)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@month_id", SqlDbType.Int, 4));
        cmd.Parameters["@month_id"].Value = monthID;
        cmd.Parameters.Add(new SqlParameter("@monthName", SqlDbType.VarChar, 50));
        cmd.Parameters["@monthName"].Value = monthName;
        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar, 8));
        cmd.Parameters["@year"].Value = year;
        cmd.Parameters.Add(new SqlParameter("@is_closed", SqlDbType.VarChar, 8));
        cmd.Parameters["@is_closed"].Value = is_closed;

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new Exception();
        }
        finally
        {
            conn.Close();
        }

    }

    // обновляем статус периода
    public void updatePeriodIsClosed(int id, int status)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "UPDATE rolf_timeboard_periods SET is_closed = @status WHERE (id = @id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Int, 4));
        cmd.Parameters["@status"].Value = status;
        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new Exception();
        }
        finally
        {
            conn.Close();
        }

    }

    // проверяем существует ли период в базе
    public bool isPeriodExists(int month_id, int year)
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_periods WHERE (month_id = @month_id) AND (year = @year)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@month_id", SqlDbType.Int, 4));
        cmd.Parameters["@month_id"].Value = month_id;
        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.Int, 4));
        cmd.Parameters["@year"].Value = year;

        bool hasRow = false;

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
                hasRow = true;
            reader.Close();

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return hasRow;
    }

    // получаем период в формате - название месяца + год
    public Period getPeriod(int month_id, int year)
    {
        SqlConnection conn = new SqlConnection(this.ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_periods WHERE (month_id = @month_id) AND (year = @year)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@month_id", SqlDbType.Int, 4));
        cmd.Parameters["@month_id"].Value = month_id;
        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar, 8));
        cmd.Parameters["@year"].Value = year;

        Period period = null;

        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            period = new Period((int)reader["id"], (int)reader["month_id"], (string)reader["month"], (int)reader["year"], (int)reader["is_closed"]);
            reader.Close();

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }
        return period;
    }

}
