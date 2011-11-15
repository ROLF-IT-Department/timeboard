using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAP.Connector;
using sap;

/// <summary>
/// ����� ������ �����������
/// </summary>
public class EmployeeList
{

    public EmployeeList()
    {

    }

    public Employee getEmployee(EmployeeAttrib emp_attrib)
    {
        SQLDB db = new SQLDB();

        Employee emp = db.getEmployee(emp_attrib.EmployeeID, emp_attrib.StartPeriod, emp_attrib.BeginDate, emp_attrib.EndDate);

        return emp;
    }

    public List<Post> getPostList(List<EmployeeAttrib> empAttribs)
    {
        SQLDB db = new SQLDB();
        List<Post> all_posts = new List<Post>();

        foreach (EmployeeAttrib empAtr in empAttribs)
        {
            Post post = db.getPost(empAtr.EmployeeID, empAtr.StartPeriod, empAtr.BeginDate, empAtr.EndDate);
            if (post != null)
            {
                if (all_posts.Count > 0)
                {
                    Post p = all_posts.Find(delegate(Post pt) { return pt.Post_Name.Equals(post.Post_Name); });
                    if (p == null) all_posts.Add(post);
                }
                else
                    all_posts.Add(post);
            }
        }

        PostComparerASC pc = new PostComparerASC();
        all_posts.Sort(pc);

        Post all = new Post("0", "���");
        all_posts.Add(all);

        return all_posts;
    }

    public void getDepartmentSector(List<EmployeeAttrib> empAttribs, ref List<Department> all_departments, ref List<Sector> all_sectors)
    {
        SQLDB db = new SQLDB();

        foreach (EmployeeAttrib empAtr in empAttribs)
        {
            Department department = db.getDepartment(empAtr.EmployeeID, empAtr.StartPeriod, empAtr.BeginDate, empAtr.EndDate);
            if (department != null)
            {
                Department d = all_departments.Find(delegate(Department dep) { return dep.DepartmentID.Equals(department.DepartmentID); });
                if (d == null) all_departments.Add(department);
            }
            List<Sector> sectors = db.getSector(empAtr.EmployeeID, empAtr.StartPeriod, empAtr.BeginDate, empAtr.EndDate);
            if (sectors != null)
            {
                foreach (Sector sector in sectors)
                {
                    Sector s = all_sectors.Find(delegate(Sector sec) { return sec.SectorID.Equals(sector.SectorID); });
                    if (s == null) all_sectors.Add(sector);
                }
            }
        }

        DepartmentComparerASC dc = new DepartmentComparerASC();
        all_departments.Sort(dc);

        SectorComparerASC sc = new SectorComparerASC();
        all_sectors.Sort(sc);


    }

    // �������� ������ ����������� � ���������� ������
    public List<Employee> getEmployeesOfDepartment(List<EmployeeAttrib> emp_attribs, string depID, string period)
    {
        SQLDB db = new SQLDB();
        List<Employee> all_employees = db.getEmployeesOfDepartment(depID, period);
        List<Employee> new_employees = new List<Employee>();

        foreach (EmployeeAttrib emp_attr in emp_attribs)
        {
            if (new_employees.Count > 0)
            {
                Employee employee = new_employees.Find(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });
                if (employee == null)
                {
                    List<Employee> employees = all_employees.FindAll(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });

                    foreach (Employee em in employees)
                        new_employees.Add(em);

                }
            }
            else
            {
                List<Employee> employees = all_employees.FindAll(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });

                foreach (Employee em in employees)
                    new_employees.Add(em);
            }

        }

        EmployeeComparerByFullnameASC cp = new EmployeeComparerByFullnameASC();
        new_employees.Sort(cp);

        return new_employees;
    }

    // �������� ������ ����������� �� ���������� �������
    public List<Employee> getEmployeesOfSector(List<EmployeeAttrib> emp_attribs, string secID, string period)
    {
        SQLDB db = new SQLDB();
        List<Employee> all_employees = db.getEmployeesOfSector(secID, period);
        List<Employee> new_employees = new List<Employee>();

        foreach (EmployeeAttrib emp_attr in emp_attribs)
        {
            if (new_employees.Count > 0)
            {
                Employee employee = new_employees.Find(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });
                if (employee == null)
                {
                    List<Employee> employees = all_employees.FindAll(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });

                    foreach (Employee em in employees)
                        new_employees.Add(em);
                }
            }
            else
            {
                List<Employee> employees = all_employees.FindAll(delegate(Employee emp) { return emp.EmployeeID.Equals(emp_attr.EmployeeID); });

                foreach (Employee em in employees)
                    new_employees.Add(em);
            }

        }

        EmployeeComparerByFullnameASC cp = new EmployeeComparerByFullnameASC();
        new_employees.Sort(cp);

        return new_employees;
    }


    // �������� ������ ����������� �� ���������� ���������
    public List<EmployeeAttrib> getEmployeesOnCurrentPost(string postName, List<EmployeeAttrib> emp_attribs)
    {
        if (emp_attribs == null) return null;
        SQLDB db = new SQLDB();

        List<EmployeeAttrib> emps = new List<EmployeeAttrib>();
        foreach (EmployeeAttrib em in emp_attribs)
        {
            Employee employee = db.getEmployee(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate);
            if (employee.Post.Equals(postName)) emps.Add(em);
        }

        return emps;
    }

    // �������� ������ ����������� �� ������ ������ �������
    public List<EmployeeAttrib> getEmployeesByFIO(string fio, List<EmployeeAttrib> emp_attribs)
    {
        if (emp_attribs == null) return null;

        SQLDB db = new SQLDB();
        List<EmployeeAttrib> emps = new List<EmployeeAttrib>();

        foreach (EmployeeAttrib em in emp_attribs)
        {
            Employee employee = db.getEmployee(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate);
            string fam = employee.FullName.Substring(0, employee.FullName.IndexOf(' '));
            if (fam.ToLower().StartsWith(fio.ToLower())) emps.Add(em);
        }
        return emps;
    }

    // �������� ������ ����������� �� ���������� ������
    public List<EmployeeAttrib> getEmployeesByTab(string tab, List<EmployeeAttrib> emp_attribs)
    {
        if (emp_attribs == null) return null;

        List<EmployeeAttrib> emps = new List<EmployeeAttrib>();

        foreach (EmployeeAttrib em in emp_attribs)
        {
            string empID = DeleteZeroFromEmployeeID(em.EmployeeID);
            if (empID.Equals(tab)) emps.Add(em);
        }
        return emps;
    }

    public List<EmployeeAttrib> getEmployeesByStatus(string status, List<EmployeeAttrib> emp_attribs)
    {
        if (emp_attribs == null) return null;

        SQLDB sql = new SQLDB();
        List<EmployeeAttrib> emps = new List<EmployeeAttrib>();

        int status_1 = 0;
        int status_2 = Convert.ToInt32(status);


        foreach (EmployeeAttrib em in emp_attribs)
        {
            status_1 = sql.getStatusOverhours(em.EmployeeID, em.StartPeriod, em.BeginDate, em.EndDate);

            if (status_1 == status_2) emps.Add(em);
        }

        return emps;
    }


    // ������� ������� ������ ���� � ID ����������
    public string DeleteZeroFromEmployeeID(string EmployeeID)
    {
        string newEmployeeID = null;
        int i = 0;
        while (EmployeeID[i].Equals('0'))
            i++;
        newEmployeeID = EmployeeID.Substring(i);

        return newEmployeeID;
    }

    // ������� ��� ��������� ����������� ����� � ������� decimal(5,2)
    public string CheckDecimalNumber(string decNumber)
    {
        if (decNumber.Contains(","))
        {
            string integer = decNumber.Substring(0, decNumber.IndexOf(','));     // �������� ����� ����� �����
            string real = decNumber.Substring(decNumber.IndexOf(',') + 1);      // �������� ������� ����� �����

            string result = "";

            if ((real[0] == '0') && (real[1] == '0')) return integer;

            if (real[1] == '0')
            {
                result = integer + ',' + real[0];
                return result;
            }

        }

        return decNumber;

    }

    // �������� ������ ����������� � ������� ������� �� ���������
    public bool checkEmployee(string employee_id, string start_period, string begda, string endda, int count_days)
    {

        bool is_right = true;
        int day = 1;

        EmployeeList emp_list = new EmployeeList();

        List<TimekeeperHours> current = emp_list.getEmployeesTimekeeperHours(employee_id, start_period, begda, endda);
        List<HRHours> hr_current = emp_list.getEmployeesHRHours(employee_id, start_period, begda, endda);
        List<Schedule> schedule = emp_list.getEmployeesSchedule(employee_id, start_period, begda, endda);
        List<ScheduleDeflection> dschedule = emp_list.getEmployeesScheduleDeflection(employee_id, start_period, begda, endda);

        foreach (Schedule sch in schedule)
        {

            // ���� � ��� ���� �� ��������� � ���� � �������, �������� ������ ���������
            int d = Convert.ToInt32(sch.DayPeriod);

            while (d != day)
            {
                if (day == count_days) break;
                day++;
            }

            is_right = true;
            string time = "";
            decimal hour_timekeeper = 0;
            decimal hour_schedule = 0;
            decimal hour_hr = 0;
            string symbols = "";

            TimekeeperHours th = current.Find(delegate(TimekeeperHours h) { return h.Day == day.ToString(); });

            if (th != null)
            {
                hour_timekeeper = th.Hours;
                symbols = th.Symbols.ToUpper();
            }


            ScheduleDeflection sd = dschedule.Find(delegate(ScheduleDeflection dsch) { return Convert.ToInt32(dsch.DayPeriod) == day; });
            if (sd != null)
            {
                time = sd.CodeT13;
                decimal diversity = sch.TimeHours - sd.TimeHours;
                if (diversity == 0)
                {
                    // ���� ���������� � ����� 2000 ��� 2001 - ����� �� �������, �� � �������� ������ ����������� ����. ��� �����������.
                    if (sd.AbsAttType.Equals("2000") || sd.AbsAttType.Equals("2001"))
                        hour_schedule = Convert.ToDecimal(sd.TimeHours);
                    else
                        time = sd.CodeT13;
                }
                else
                {
                    if (diversity > 0)
                    {
                        time = CheckDecimalNumber(diversity.ToString());
                        hour_schedule = Convert.ToDecimal(diversity);
                    }
                    else
                    {
                        time = CheckDecimalNumber(sd.TimeHours.ToString());
                        hour_schedule = Convert.ToDecimal(sd.TimeHours);
                    }
                }

            }
            else
            {
                // ���� � ��� �������� �� ��������� � ������� ����� �
                if (((sch.DaySchedule == "FREE") && (Convert.ToDecimal(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                {
                    time = "�";  // � - � ������� ���������!!!!!!!!
                }
                else
                {
                    time = CheckDecimalNumber(sch.TimeHours.ToString());
                    hour_schedule = Convert.ToDecimal(sch.TimeHours);

                }

            }

            HRHours hrh = hr_current.Find(delegate(HRHours h) { return h.Day == day.ToString(); });
            if (hrh != null)
            {
                if ((hrh.DayOverHours != null) && (hrh.NightOverHours != null)) hour_hr = hrh.DayOverHours + hrh.NightOverHours;
                if ((hrh.DayOverHours != null) && (hrh.NightOverHours == null)) hour_hr = hrh.DayOverHours;
                if ((hrh.DayOverHours == null) && (hrh.NightOverHours != null)) hour_hr = hrh.NightOverHours;
            }

            if (hour_timekeeper <= 0)
            {
                // ���� � ����� ���������� ����� � ��� ��������� � �������� ��������, ���� ������
                try
                {
                    if ((symbols.Equals(time) == true) || ((symbols == "") && (Convert.ToDecimal(time) == 0)))
                        continue;
                }
                catch
                {
                }

                // ���� � ����� ���������� ����������� ����� � ��� �� ��������� � �������� ��������, �� ���������� ����������
                if ((symbols.Equals(time) == false) || (hour_hr > 0))
                {
                    is_right = false;
                    break;
                }

            }

            // ��������� ��������� = ���� ���������� - ���� ����������� - ���� �� ��������� �������
            decimal result = hour_timekeeper - hour_hr - hour_schedule;

            if (result != 0)
            {
                is_right = false;
                break;
            }



            day++;
        }

        return is_right;

    }

    // �������� ������ ���������� ��� ����������� ����������
    public List<TimekeeperHours> getEmployeesTimekeeperHours(string employee_id, string start_period, string begda, string endda)
    {
        SQLDB db = new SQLDB();

        List<TimekeeperHours> current = db.getTimekeeperHours(employee_id, start_period, begda, endda);

        return current;
    }

    // �������� ���� ����������� ��� ����������� ����������
    public List<HRHours> getEmployeesHRHours(string employee_id, string start_period, string begda, string endda)
    {
        SQLDB db = new SQLDB();

        List<HRHours> current = db.getHRHours(employee_id, start_period, begda, endda);

        return current;
    }

    public List<Schedule> getEmployeesSchedule(string employee_id, string start_period, string begda, string endda)
    {
        SQLDB db = new SQLDB();

        List<Schedule> current = db.getSchedule(employee_id, start_period, begda, endda);

        return current;
    }

    public List<ScheduleDeflection> getEmployeesScheduleDeflection(string employee_id, string start_period, string begda, string endda)
    {
        SQLDB db = new SQLDB();

        List<ScheduleDeflection> current = db.getScheduleDeflection(employee_id, start_period, begda, endda);

        return current;
    }

    // �������� ����������� ����������� �� ������� OverhoursDiversity
    public List<OverhoursDiversity> getEmployeesOverhoursDiversity(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {

        SQLDB sql = new SQLDB();

        List<OverhoursDiversity> current = sql.getOverhoursDiversity(employee_id, start_period, begda, endda, downloaded);

        return current;
    }

    // �������� ����������� ����������� �� ������� OverhoursManagement
    public List<OverhoursManagement> getEmployeesOverhoursManagement(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {

        SQLDB sql = new SQLDB();

        List<OverhoursManagement> current = sql.getOverhoursManagement(employee_id, start_period, begda, endda, downloaded);

        return current;
    }

    // �������� ����������� ����������� �� ������� OverhoursMethodT13
    public List<OverhoursMethodT13> getEmployeesOverhoursMethodT13(string employee_id, string start_period, string begda, string endda, bool downloaded)
    {

        SQLDB sql = new SQLDB();

        List<OverhoursMethodT13> current = sql.getOverhoursMethodT13(employee_id, start_period, begda, endda, downloaded);

        return current;
    }


    // �������� ���� ����������� - ������������ 0 - ���� � �������� 0 ����� �����������. 1 - ���� � �������� ���� ��������� ���������� �����������, -1 - ��� ������ ����������
    public int insertOvertimeHoursToDB(string employee_id, string start_period, string begda, string endda, int count_days, int month, int year, string accountant_id, int period_id)
    {

        SQLDB db = new SQLDB();
        Date dt = new Date();

        int day = 1;

        EmployeeList emp_list = new EmployeeList();

        List<TimekeeperHours> current = emp_list.getEmployeesTimekeeperHours(employee_id, start_period, begda, endda);
        List<HRHours> hr_current = emp_list.getEmployeesHRHours(employee_id, start_period, begda, endda);
        List<Schedule> schedule = emp_list.getEmployeesSchedule(employee_id, start_period, begda, endda);
        List<ScheduleDeflection> dschedule = emp_list.getEmployeesScheduleDeflection(employee_id, start_period, begda, endda);

        decimal overhours_count = db.getOverhoursSum(employee_id, start_period, begda, endda);

        bool t13 = false;
        bool diversity = false;

        if (overhours_count == 0)
        {
            db.insertOverhoursToManagement(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), 0, accountant_id, period_id);
            return 0;
        }

        if (overhours_count <= 8) t13 = true;
        else diversity = true;

        // ���� ���-�� ����������� ������ 8 �����, �� ���������� ��� ����������
        decimal m111 = 0; // ��� ����������� M111 - ����� ������ 2 ����� �����������, ����� ��������
        decimal m122 = 0; // ��� ����������� M122 - ����� ���������� ����� � ����������� � �������� � ����������� ���
        decimal m210 = 0; // ��� ����������� M210 - ����� ������ ������ �����������

        foreach (Schedule sch in schedule)
        {

            // ���� � ��� ���� �� ��������� � ���� � �������, �������� ������ ���������
            int d = Convert.ToInt32(sch.DayPeriod);

            while (d != day)
            {
                if (day == count_days) break;
                day++;
            }

            HRHours hrh = hr_current.Find(delegate(HRHours h) { return h.Day == day.ToString(); });
            decimal hr_sum = 0;
            if (hrh != null)
                hr_sum = hrh.DayOverHours + hrh.NightOverHours;


            ScheduleDeflection sd = dschedule.Find(delegate(ScheduleDeflection dsch) { return Convert.ToInt32(dsch.DayPeriod) == day; });
            if (sd == null)
            {
                // ���� � ��� �������� �� ������� ��� ����������� �� ���� ����
                if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                {
                    if (hrh != null)
                    {

                        db.insertOverhoursToManagement(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                        if (t13) db.insertOverhoursToT13(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                    }
                    if ((diversity) && (hrh != null)) m122 += hr_sum;
                }
                else        // ���� ������� ����
                {
                    if (hrh != null)
                    {
                        db.insertOverhoursToManagement(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                        if (t13) db.insertOverhoursToT13(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                    }
                    if ((diversity) && (hrh != null))
                    {
                        if (hr_sum <= 2) m111 += hr_sum;
                        else
                        {
                            m111 += 2;
                            m122 += (hr_sum - 2);
                        }
                        if (hrh.NightOverHours != 0) m210 += hrh.NightOverHours;
                    }

                }
            }
            else // !!!!!!!!!!!! ���������� �����������, ����� ����� ����������� ���� ������� ����������� � SAP !!!!!!!!!
            {
                // ������� ���������� ����� �� �������. ���� � ���� ���� ���� ����������� - ����� ��
                if (sd.AbsAttType.Equals("2000") || sd.AbsAttType.Equals("2001"))
                {
                    // ���� � ��� �������� �� ������� ��� ����������� �� ���� ����
                    if (((sch.DaySchedule == "FREE") && (Convert.ToInt32(sch.TimeHours) == 0)) || (sch.DayScheduleVar == "F"))
                    {
                        if (hrh != null)
                        {

                            db.insertOverhoursToManagement(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                            if (t13) db.insertOverhoursToT13(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                        }
                        if ((diversity) && (hrh != null)) m122 += hr_sum;
                    }
                    else        // ���� ������� ����
                    {
                        if (hrh != null)
                        {
                            db.insertOverhoursToManagement(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                            if (t13) db.insertOverhoursToT13(employee_id, start_period, begda, endda, dt.getDateToSQL(day, month, year), hr_sum, accountant_id, period_id);
                        }
                        if ((diversity) && (hrh != null))
                        {
                            if (hr_sum <= 2) m111 += hr_sum;
                            else
                            {
                                m111 += 2;
                                m122 += (hr_sum - 2);
                            }
                            if (hrh.NightOverHours != 0) m210 += hrh.NightOverHours;
                        }

                    }
                }
            }

            day++;
        }


        if (m111 > 0) db.insertOverhoursDiversity(employee_id, start_period, begda, endda, dt.getDateFromSAPToSQL(endda), m111, "M111", accountant_id, period_id);
        if (m122 > 0) db.insertOverhoursDiversity(employee_id, start_period, begda, endda, dt.getDateFromSAPToSQL(endda), m122, "M122", accountant_id, period_id);
        if (m210 > 0) db.insertOverhoursDiversity(employee_id, start_period, begda, endda, dt.getDateFromSAPToSQL(endda), m210, "M210", accountant_id, period_id);

        return 1;
    }



}
