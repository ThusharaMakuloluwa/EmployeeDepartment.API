# EmployeeDepartment.API

Backend Web API project for the Employee & Department Management System built using **ASP.NET Core Web API** and **SQL Server**.

This API provides CRUD operations for managing employees and departments.

---

## Technologies Used

- ASP.NET Core Web API
- ADO.net
- SQL Server
- Swagger
- C#
- REST API
- Clean Architecture

---

## Features

### Department Management
- Get all departments
- Get department by ID
- Create departments
- Update departments
- Delete departments

### Employee Management
- Get all employees
- Get employee by ID
- Create employees
- Update employees
- Delete employees

---

## Frontend Application

Frontend Repository:

[EmployeeDepartment.UI Frontend](https://github.com/ThusharaMakuloluwa/EmployeeDepartment.UI.git?utm_source=chatgpt.com)

---

## Database Script

Create the **CompanyDB** database and run the following SQL script in SQL Server Management Studio (SSMS).

```sql
USE CompanyDB;
GO

CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,

    DepartmentCode VARCHAR(20) NOT NULL UNIQUE,

    DepartmentName VARCHAR(100) NOT NULL
);

GO

CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,

    FirstName VARCHAR(50) NOT NULL,

    LastName VARCHAR(50) NOT NULL,

    EmailAddress VARCHAR(100) NOT NULL UNIQUE,

    DateOfBirth DATE NOT NULL,

    Salary DECIMAL(18,2) NOT NULL,

    DepartmentId INT NOT NULL,

    CONSTRAINT FK_Employees_Departments
    FOREIGN KEY (DepartmentId)
    REFERENCES Departments(DepartmentId)
);
GO
