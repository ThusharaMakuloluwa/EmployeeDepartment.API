using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public List<Employee> GetAll()
        {
            var employees = new List<Employee>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT 
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.EmailAddress,
                    e.DateOfBirth,
                    e.Salary,
                    e.DepartmentId,
                    d.DepartmentName
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                ORDER BY e.EmployeeId DESC";

            using SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DateTime dob = Convert.ToDateTime(reader["DateOfBirth"]);

                employees.Add(new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    EmailAddress = reader["EmailAddress"].ToString() ?? string.Empty,
                    DateOfBirth = dob,
                    Age = CalculateAge(dob),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
                });
            }

            return employees;
        }

        public Employee? GetById(int id)
        {
            Employee? employee = null;

            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT 
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.EmailAddress,
                    e.DateOfBirth,
                    e.Salary,
                    e.DepartmentId,
                    d.DepartmentName
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                WHERE e.EmployeeId = @EmployeeId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                DateTime dob = Convert.ToDateTime(reader["DateOfBirth"]);

                employee = new Employee
                {
                    EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    EmailAddress = reader["EmailAddress"].ToString() ?? string.Empty,
                    DateOfBirth = dob,
                    Age = CalculateAge(dob),
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
                };
            }

            return employee;
        }

        public void Add(Employee employee)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Employees 
                (FirstName, LastName, EmailAddress, DateOfBirth, Salary, DepartmentId)
                VALUES 
                (@FirstName, @LastName, @EmailAddress, @DateOfBirth, @Salary, @DepartmentId)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
            command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string checkQuery = @"SELECT COUNT(1) FROM Employees WHERE EmployeeId = @EmployeeId";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    connection.Open();
                    int exists = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (exists == 0)
                    {
                        throw new InvalidOperationException($"Employee with ID {employee.EmployeeId} does not exist.");
                    }
                }
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Employees
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        EmailAddress = @EmailAddress,
                        DateOfBirth = @DateOfBirth,
                        Salary = @Salary,
                        DepartmentId = @DepartmentId
                    WHERE EmployeeId = @EmployeeId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
                    command.Parameters.AddWithValue("@DateOfBirth", employee.DateOfBirth);
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
