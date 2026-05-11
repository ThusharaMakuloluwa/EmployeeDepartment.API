using EmployeeDepartment.Application.Interfaces;
using EmployeeDepartment.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeDepartment.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public List<Department> GetAll()
        {
            var departments = new List<Department>();

            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT DepartmentId, DepartmentCode, DepartmentName 
                FROM Departments
                ORDER BY DepartmentId DESC";

            using SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                departments.Add(new Department
                {
                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                    DepartmentCode = reader["DepartmentCode"].ToString() ?? string.Empty,
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
                });
            }

            return departments;
        }

        public Department? GetById(int id)
        {
            Department? department = null;

            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                SELECT DepartmentId, DepartmentCode, DepartmentName 
                FROM Departments 
                WHERE DepartmentId = @DepartmentId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", id);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                department = new Department
                {
                    DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                    DepartmentCode = reader["DepartmentCode"].ToString() ?? string.Empty,
                    DepartmentName = reader["DepartmentName"].ToString() ?? string.Empty
                };
            }

            return department;
        }

        public void Add(Department department)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
                INSERT INTO Departments 
                (DepartmentCode, DepartmentName) 
                VALUES 
                (@DepartmentCode, @DepartmentName)";

            using SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DepartmentCode", department.DepartmentCode);
            command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Update(Department department)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string checkQuery = @"SELECT COUNT(1) FROM Departments WHERE DepartmentId = @DepartmentId";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    connection.Open();
                    int exists = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (exists == 0)
                    {
                        throw new InvalidOperationException($"Department with ID {department.DepartmentId} does not exist.");
                    }
                }
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Departments 
                    SET DepartmentCode = @DepartmentCode,
                        DepartmentName = @DepartmentName
                    WHERE DepartmentId = @DepartmentId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    command.Parameters.AddWithValue("@DepartmentCode", department.DepartmentCode);
                    command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = "DELETE FROM Departments WHERE DepartmentId = @DepartmentId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", id);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
