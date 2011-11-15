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
/// Класс для получения графиков табельщика и переработок из SQL
/// </summary>
public class SQLDB
{
	private string ConnectionString;


    public SQLDB()
	{
        this.ConnectionString = WebConfigurationManager.ConnectionStrings["Timeboard"].ConnectionString;

	}

    public Employee getEmployee(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_employees_sap WHERE (employee_id = @employee_id) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 50));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 50));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 50));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 50));
        cmd.Parameters["@endda"].Value = endda;
        Employee employee = null;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                employee = new Employee((string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (string)reader["full_name"], (string)reader["plans"], (string)reader["plans_txt"], (string)reader["orgeh"], (string)reader["orgeh_txt"]);
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

        return employee;

    }

    // получаем информацию по сотрудникам 
    public Post getPost(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT plans, plans_txt FROM rolf_timeboard_employees_sap WHERE (employee_id = @employee_id) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 50));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 50));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 50));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 50));
        cmd.Parameters["@endda"].Value = endda;
        Post post = null;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                post = new Post((string)reader["plans"], (string)reader["plans_txt"]);
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

        return post;

    }

    // получаем информацию по сотрудникам 
    public Department getDepartment(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT orgeh, orgeh_txt FROM rolf_timeboard_employees_sap WHERE (employee_id = @employee_id) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 50));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 50));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 50));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 50));
        cmd.Parameters["@endda"].Value = endda;
        Department department = null;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                department = new Department((string)reader["orgeh"], (string)reader["orgeh_txt"]);
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

        return department;

    }

    // получаем информацию по сотрудникам 
    public List<Sector> getSector(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT sector, sector_txt FROM  rolf_timeboard_sectors_sap WHERE (employee_id = @employee_id) AND (start_period = @start_period)  AND (begda = @begda) AND (endda = @endda)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 50));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 50));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 50));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 50));
        cmd.Parameters["@endda"].Value = endda;
        List<Sector> sectors = new List<Sector>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Sector sector = new Sector((string)reader["sector"], (string)reader["sector_txt"]);
                sectors.Add(sector);
            }
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

        return sectors;

    }

    public List<Employee> getEmployeesOfDepartment(string orgeh, string start_period)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT * FROM rolf_timeboard_employees_sap WHERE (orgeh = @orgeh) AND (start_period = @start_period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@orgeh", SqlDbType.VarChar, 8));
        cmd.Parameters["@orgeh"].Value = orgeh;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        List<Employee> employees = new List<Employee>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Employee employee = new Employee((string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (string)reader["full_name"], (string)reader["plans"], (string)reader["plans_txt"], (string)reader["orgeh"], (string)reader["orgeh_txt"]);
                employees.Add(employee);
            }
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

        return employees;

    }

    public List<Employee> getEmployeesOfSector(string sector, string start_period)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT * FROM rolf_timeboard_get_employees_by_sector WHERE (sector = @sector) AND (start_period = @start_period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@sector", SqlDbType.VarChar, 8));
        cmd.Parameters["@sector"].Value = sector;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        List<Employee> employees = new List<Employee>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Employee employee = new Employee((string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (string)reader["full_name"], (string)reader["plans"], (string)reader["plans_txt"], (string)reader["orgeh"], (string)reader["orgeh_txt"]);
                employees.Add(employee);
            }
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

        return employees;

    }

    public List<Employee> getEmployeesOnCurrentPost(string plans_txt, string start_period)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT DISTINCT * FROM rolf_timeboard_employees_sap WHERE (plans_txt = @plans_txt) AND (start_period = @start_period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@plans_txt", SqlDbType.VarChar, 255));
        cmd.Parameters["@plans_txt"].Value = plans_txt;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        List<Employee> employees = new List<Employee>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Employee employee = new Employee((string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (string)reader["full_name"], (string)reader["plans"], (string)reader["plans_txt"], (string)reader["orgeh"], (string)reader["orgeh_txt"]);
                employees.Add(employee);
            }
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

        return employees;

    }


    // записываем часы и символы табельщика в SQL  - по умолчанию часы -1 , буквы  ' '
    public void insertTimekeeperHoursAndSymbols(string employee_id, string start_period, string begda, string endda, decimal hours, string symbols, string user, string department_id, string post_id, string day, string month, string year, int periodID)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_timekeeper (employee_id, start_period, begda, endda, hours, symbols, timekeeper_id, department_id, post_id, day, month, year, record_date, period_id) VALUES (@eid, @start_period, @begda, @endda, @hours, @sym, @tid, @did, @pid, @day, @month, @year, @rdate, @period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@hours"].Value = hours;
        cmd.Parameters.Add(new SqlParameter("@sym", SqlDbType.VarChar, 5));
        cmd.Parameters["@sym"].Value = symbols;
        cmd.Parameters.Add(new SqlParameter("@tid", SqlDbType.VarChar, 8));
        cmd.Parameters["@tid"].Value = user;
        cmd.Parameters.Add(new SqlParameter("@did", SqlDbType.VarChar, 8));
        cmd.Parameters["@did"].Value = department_id;
        cmd.Parameters.Add(new SqlParameter("@pid", SqlDbType.VarChar, 8));
        cmd.Parameters["@pid"].Value = post_id;
        cmd.Parameters.Add(new SqlParameter("@day", SqlDbType.VarChar, 2));
        cmd.Parameters["@day"].Value = day;
        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar, 2));
        cmd.Parameters["@month"].Value = month;
        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar, 4));
        cmd.Parameters["@year"].Value = year;
        cmd.Parameters.Add(new SqlParameter("@rdate", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@rdate"].Value = DateTime.Now;
        cmd.Parameters.Add(new SqlParameter("@period", SqlDbType.Int, 4));
        cmd.Parameters["@period"].Value = periodID;
        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }

    }

    // записываем часы переработок
    public void insertHRHours(string employee_id, string start_period, string begda, string endda, decimal day_overhours, decimal night_overhours, string user, string department_id, string post_id, string day, string month, string year, int periodID)
    { 
        if ((day_overhours == 0) && (night_overhours == 0)) return;

        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_hr (employee_id, start_period, begda, endda, day_overhours, night_overhours, hr_id, department_id, post_id, day, month, year, record_date, period_id) VALUES (@eid, @start_period, @begda, @endda, @day_hours, @night_hours, @tid, @did, @pid, @day, @month, @year, @rdate, @period)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@day_hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@day_hours"].Value = day_overhours;
        cmd.Parameters.Add(new SqlParameter("@night_hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@night_hours"].Value = night_overhours;
        cmd.Parameters.Add(new SqlParameter("@tid", SqlDbType.VarChar, 8));
        cmd.Parameters["@tid"].Value = user;
        cmd.Parameters.Add(new SqlParameter("@did", SqlDbType.VarChar, 8));
        cmd.Parameters["@did"].Value = department_id;
        cmd.Parameters.Add(new SqlParameter("@pid", SqlDbType.VarChar, 8));
        cmd.Parameters["@pid"].Value = post_id;
        cmd.Parameters.Add(new SqlParameter("@day", SqlDbType.VarChar, 2));
        cmd.Parameters["@day"].Value = day;
        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar, 2));
        cmd.Parameters["@month"].Value = month;
        cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar, 4));
        cmd.Parameters["@year"].Value = year;
        cmd.Parameters.Add(new SqlParameter("@rdate", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@rdate"].Value = DateTime.Now;
        cmd.Parameters.Add(new SqlParameter("@period", SqlDbType.Int, 4));
        cmd.Parameters["@period"].Value = periodID;
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

    // обновляем часы и буквы табельщика  -  по умолчанию часы -1 , буквы ' ' 
    public void updateTimekeeperHoursAndSymbols(string timekeeper_id, decimal hours, string symbols, int id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "UPDATE rolf_timeboard_timekeeper SET hours = @hours , symbols = @sym, timekeeper_id = @tid, record_date = @rdate WHERE (id = @id) ";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
        cmd.Parameters.Add(new SqlParameter("@hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@hours"].Value = hours;
        cmd.Parameters.Add(new SqlParameter("@sym", SqlDbType.VarChar, 5));
        cmd.Parameters["@sym"].Value = symbols;
        cmd.Parameters.Add(new SqlParameter("@tid", SqlDbType.VarChar, 8));
        cmd.Parameters["@tid"].Value = timekeeper_id;
        cmd.Parameters.Add(new SqlParameter("@rdate", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@rdate"].Value = DateTime.Now;
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


    // обновляем часы переработок
    public void updateHRHours(string hr_id, decimal day_overhours, decimal night_overhours, int id)
    {
        if ((day_overhours == 0) && (night_overhours == 0)) return;
        
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "UPDATE rolf_timeboard_hr SET day_overhours = @day_hours, night_overhours = @night_hours, hr_id = @tid, record_date = @rdate WHERE (id = @id) ";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
        cmd.Parameters.Add(new SqlParameter("@day_hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@day_hours"].Value = day_overhours;
        cmd.Parameters.Add(new SqlParameter("@night_hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@night_hours"].Value = night_overhours;
        cmd.Parameters.Add(new SqlParameter("@tid", SqlDbType.VarChar, 8));
        cmd.Parameters["@tid"].Value = hr_id;
        cmd.Parameters.Add(new SqlParameter("@rdate", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@rdate"].Value = DateTime.Now;
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

    // удаляем проставленные часы табельщика
    public void deleteTimekeeperHours(int id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "DELETE FROM rolf_timeboard_timekeeper WHERE (id = @id) ";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
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

    // удаляем часы переработок
    public void deleteHRHours(int id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "DELETE FROM rolf_timeboard_hr WHERE (id = @id) ";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 4));
        cmd.Parameters["@id"].Value = id;
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

    
    public List<TimekeeperHours> getTimekeeperHours(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_get_timekeeper_hours";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        List<TimekeeperHours> hours = new List<TimekeeperHours>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                TimekeeperHours time = new TimekeeperHours((int)reader["id"], (string)reader["employee_id"], (decimal)reader["hours"], (string)reader["symbols"], (string)reader["timekeeper_id"], (string)reader["department_id"], (string)reader["post_id"], (string)reader["day"], (string)reader["month"], (string)reader["year"]);
                hours.Add(time);
            }
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

        return hours;

    }

    // получаем ВСЕ переработки за определенный период 
    public List<HRHours> getHRHours(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_get_hr_hours";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        List<HRHours> overhours = new List<HRHours>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                HRHours time = new HRHours((int)reader["id"], (string)reader["employee_id"], (decimal)reader["day_overhours"], (decimal)reader["night_overhours"], (string)reader["hr_id"], (string)reader["department_id"], (string)reader["post_id"], (string)reader["day"], (string)reader["month"], (string)reader["year"], (int)reader["period_id"]);
                overhours.Add(time);
            }
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

        return overhours;

    }

    public List<Schedule> getSchedule(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_get_schedule";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        List<Schedule> schedules = new List<Schedule>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Schedule schedule = new Schedule((string)reader["employee_id"], (string)reader["day_period"], (string)reader["start_period"], (string)reader["schedule_wt"], (string)reader["od_schedule"], (string)reader["od_schedule_var"], (string)reader["time_begin"], (string)reader["time_end"], (decimal)reader["time_hours"]);
                schedules.Add(schedule);
            }
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

        return schedules;

    }

    public List<ScheduleDeflection> getScheduleDeflection(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_get_dev_schedule";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        List<ScheduleDeflection> dschedules = new List<ScheduleDeflection>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ScheduleDeflection dschedule = new ScheduleDeflection((string)reader["employee_id"], (string)reader["day_period"], (string)reader["start_period"], (string)reader["abs_att_type"], (string)reader["code_t13"], (decimal)reader["time_hours"]);
                dschedules.Add(dschedule);
            }
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

        return dschedules;

    }

    public int getStatusOverhours(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_check_overhours";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        int status = 0;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                status = (int)reader["status"];
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

        return status;

    }

    public decimal getOverhoursSum(string employee_id, string start_period, string begda, string endda)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "rolf_timeboard_get_overhours_sum";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.VarChar, 8));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        decimal summa = 0;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                summa = (decimal)reader["summa"];
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

        return summa;

    }

    // вставляем полученные переработки в таблицу Метод Управленческий
    public void insertOverhoursToManagement(string employee_id, string start_period, string begda, string endda, DateTime date_day, decimal hours, string accountant_id, int period_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_overhours_management (employee_id, start_period, begda, endda, date_day, hours, downloaded, accountant_id, period_id) VALUES (@eid, @start_period, @begda, @endda, @date,@hours, @down, @acc, @period_id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@date"].Value = date_day;
        cmd.Parameters.Add(new SqlParameter("@hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@hours"].Value = hours;
        cmd.Parameters.Add(new SqlParameter("@down", SqlDbType.Bit, 1));
        cmd.Parameters["@down"].Value = true;
        cmd.Parameters.Add(new SqlParameter("@acc", SqlDbType.VarChar, 8));
        cmd.Parameters["@acc"].Value = accountant_id;
        cmd.Parameters.Add(new SqlParameter("@period_id", SqlDbType.Int, 4));
        cmd.Parameters["@period_id"].Value = period_id;
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

    // получаем ВСЕ записи из таблицы OverHoursManagement за определенный период
    public List<OverhoursManagement> getOverhoursManagement(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_overhours_management WHERE (employee_id = @eid) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda) AND (downloaded = @downloaded) AND (hours > 0)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@downloaded", SqlDbType.Bit, 1));
        cmd.Parameters["@downloaded"].Value = downloaded;
        List<OverhoursManagement> overhours = new List<OverhoursManagement>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OverhoursManagement record = new OverhoursManagement((int)reader["id"], (string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (DateTime)reader["date_day"], (decimal)reader["hours"], (bool)reader["downloaded"], (string)reader["accountant_id"], (int)reader["period_id"]);
                overhours.Add(record);
            }
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

        return overhours;

    }

    // вставляем полученные часы переработки в таблицу Метод Т-13
    public void insertOverhoursToT13(string employee_id, string start_period, string begda, string endda, DateTime date_day, decimal hours, string accountant_id, int period_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_overhours_method_t13 (employee_id, start_period, begda, endda, date_day, hours, downloaded, accountant_id, period_id) VALUES (@eid, @start_period, @begda, @endda, @date,@hours, @down, @acc, @period_id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@date"].Value = date_day;
        cmd.Parameters.Add(new SqlParameter("@hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@hours"].Value = hours;
        cmd.Parameters.Add(new SqlParameter("@down", SqlDbType.Bit, 1));
        cmd.Parameters["@down"].Value = true;
        cmd.Parameters.Add(new SqlParameter("@acc", SqlDbType.VarChar, 8));
        cmd.Parameters["@acc"].Value = accountant_id;
        cmd.Parameters.Add(new SqlParameter("@period_id", SqlDbType.Int, 4));
        cmd.Parameters["@period_id"].Value = period_id;
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

    // получаем записи из таблицы MethodT13
    public List<OverhoursMethodT13> getOverhoursMethodT13(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_overhours_method_t13 WHERE (employee_id = @eid) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda) AND (downloaded = @downloaded) AND (hours > 0)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@downloaded", SqlDbType.Bit, 1));
        cmd.Parameters["@downloaded"].Value = downloaded;
        List<OverhoursMethodT13> overhours = new List<OverhoursMethodT13>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OverhoursMethodT13 record = new OverhoursMethodT13((int)reader["id"], (string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (DateTime)reader["date_day"], (decimal)reader["hours"], (bool)reader["downloaded"], (string)reader["accountant_id"], (int)reader["period_id"]);
                overhours.Add(record);
            }
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

        return overhours;

    }


    // вставляем разницу часов переработок в таблицу
    public void insertOverhoursDiversity(string employee_id, string start_period, string begda, string endda, DateTime date_day, decimal hours, string type, string accountant_id, int period_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_overhours_diversity (employee_id, start_period, begda, endda, date_day, hours, type_payment, downloaded, accountant_id, period_id) VALUES (@eid, @start_period, @begda, @endda, @date,@hours, @type, @down, @acc, @period_id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@date"].Value = date_day;
        cmd.Parameters.Add(new SqlParameter("@hours", SqlDbType.Decimal, 5));
        cmd.Parameters["@hours"].Value = hours;
        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar, 4));
        cmd.Parameters["@type"].Value = type;
        cmd.Parameters.Add(new SqlParameter("@down", SqlDbType.Bit, 1));
        cmd.Parameters["@down"].Value = true;
        cmd.Parameters.Add(new SqlParameter("@acc", SqlDbType.VarChar, 8));
        cmd.Parameters["@acc"].Value = accountant_id;
        cmd.Parameters.Add(new SqlParameter("@period_id", SqlDbType.Int, 4));
        cmd.Parameters["@period_id"].Value = period_id;
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
   
    // получаем записи из таблицы Diversity 
    public List<OverhoursDiversity> getOverhoursDiversity(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_overhours_diversity WHERE (employee_id = @eid) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda) AND (downloaded = @downloaded) AND (hours > 0)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@downloaded", SqlDbType.Bit, 1));
        cmd.Parameters["@downloaded"].Value = downloaded;
        List<OverhoursDiversity> overhours = new List<OverhoursDiversity>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OverhoursDiversity record = new OverhoursDiversity((int)reader["id"], (string)reader["employee_id"], (string)reader["start_period"], (string)reader["begda"], (string)reader["endda"], (DateTime)reader["date_day"], (decimal)reader["hours"], (string)reader["type_payment"], (bool)reader["downloaded"], (string)reader["accountant_id"], (int)reader["period_id"]);
                overhours.Add(record);
            }
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

        return overhours;

    }

    // помечаем на закачанные табельные номера признаком false
    public void updateNotDownloadedRecords(string employee_id, string start_period, string begda, string endda, string accountant_id, string tableCode, bool downloaded)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);

        string tableName = "";
        
        switch (tableCode)
        {
            case "DIV":
                tableName = "rolf_timeboard_overhours_diversity";
                break;
            case "MAN":
                tableName = "rolf_timeboard_overhours_management";
                break;
            case "T13":
                tableName = "rolf_timeboard_overhours_method_t13";
                break;
            default:
                break;
        }

        if (tableName == "") return;

        string sql = "UPDATE " + tableName + " SET downloaded = @down WHERE (employee_id = @eid) AND (start_period = @start_period) AND (begda = @begda) AND (endda = @endda)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@eid", SqlDbType.VarChar, 8));
        cmd.Parameters["@eid"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@start_period", SqlDbType.VarChar, 8));
        cmd.Parameters["@start_period"].Value = start_period;
        cmd.Parameters.Add(new SqlParameter("@begda", SqlDbType.VarChar, 8));
        cmd.Parameters["@begda"].Value = begda;
        cmd.Parameters.Add(new SqlParameter("@endda", SqlDbType.VarChar, 8));
        cmd.Parameters["@endda"].Value = endda;
        cmd.Parameters.Add(new SqlParameter("@down", SqlDbType.Bit, 1));
        cmd.Parameters["@down"].Value = downloaded;
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



    // записываем лог посещений в таблицу
    public void insertLogon(string employee_id, DateTime date_logon, string ip, string browser)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_logon (employee_id, date_logon, ip, browser) VALUES (@emp, @date, @ip, @browser)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@emp", SqlDbType.VarChar, 8));
        cmd.Parameters["@emp"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime, 8));
        cmd.Parameters["@date"].Value = date_logon;
        cmd.Parameters.Add(new SqlParameter("@ip", SqlDbType.VarChar, 255));
        cmd.Parameters["@ip"].Value = ip;
        cmd.Parameters.Add(new SqlParameter("@browser", SqlDbType.VarChar, 255));
        cmd.Parameters["@browser"].Value = browser;
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

    // получаем по табельному номеру ФИО сотрудника
    public string getFIO(int employee_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT fio FROM it_timeboard_persons WHERE (person_id = @employee_id)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.Int, 4));
        cmd.Parameters["@employee_id"].Value = employee_id;
        string fio = "";
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                fio = (string)reader["fio"];
            reader.Close();
            return fio;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }

    }

    public void insertNotes(int employee_id, string text, int author_id, int role_id, DateTime date_expire)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "INSERT INTO rolf_timeboard_notes (employee_id, text, author_id, role_id, date_expire, date_note) VALUES (@employee_id, @text, @author_id, @role_id, @date_expire, @date_note)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.Int, 4));
        cmd.Parameters["@employee_id"].Value = employee_id;
        cmd.Parameters.Add(new SqlParameter("@text", SqlDbType.Text));
        cmd.Parameters["@text"].Value = text;
        cmd.Parameters.Add(new SqlParameter("@author_id", SqlDbType.Int, 4));
        cmd.Parameters["@author_id"].Value = author_id;
        cmd.Parameters.Add(new SqlParameter("@role_id", SqlDbType.Int, 4));
        cmd.Parameters["@role_id"].Value = role_id;
        cmd.Parameters.Add(new SqlParameter("@date_expire", SqlDbType.SmallDateTime, 4));
        cmd.Parameters["@date_expire"].Value = date_expire;
        cmd.Parameters.Add(new SqlParameter("@date_note", SqlDbType.DateTime, 8));
        cmd.Parameters["@date_note"].Value = DateTime.Now;
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

    public List<Note> getNotes(int employee_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT * FROM rolf_timeboard_notes WHERE (employee_id = @employee_id) AND (disabled IS NULL) ORDER BY date_note";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.Int, 4));
        cmd.Parameters["@employee_id"].Value = employee_id;
        List<Note> notes = new List<Note>();
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Note note = new Note((int)reader["id"], (int)reader["employee_id"], (string)reader["text"], (int)reader["author_id"], (int)reader["role_id"], (DateTime)reader["date_expire"], (DateTime)reader["date_note"]);
                notes.Add(note);
            }
            reader.Close();
            return notes;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }

    }


    public int getCountNotes(int employee_id)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);
        string sql = "SELECT COUNT(*) as Expr FROM rolf_timeboard_notes WHERE (employee_id = @employee_id) AND (disabled IS NULL)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add(new SqlParameter("@employee_id", SqlDbType.Int, 4));
        cmd.Parameters["@employee_id"].Value = employee_id;
        int count = 0;
        try
        {
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                count = (int)reader["Expr"];
            }
            reader.Close();
            return count;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close();
        }

    }

}
