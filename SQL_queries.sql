CREATE DATABASE AttendanceLeaveManagement;
USE AttendanceLeaveManagement;

-- Employees Table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(15),
    HireDate DATE NOT NULL,
    RoleID INT NOT NULL,
    ManagerID INT NULL,
    ProjectID INT NULL,
    LeaveBalance INT DEFAULT 0,
    PasswordHash VARCHAR(256) NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID)
);

select * from Employees
 
-- Roles Table
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(50) NOT NULL
);
select * from Roles
 
-- Projects Table
CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName VARCHAR(100) NOT NULL,
    Description VARCHAR(255)
);
select * from Projects
 
-- Attendance Table
CREATE TABLE Attendance (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    ProjectID INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    Status VARCHAR(50) NOT NULL,
    ApprovalStatus VARCHAR(50) DEFAULT 'Pending',
    ManagerID INT NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID),
    FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
);

select * from Attendance
 
-- Leaves Table
CREATE TABLE Leaves (
    LeaveID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    LeaveType VARCHAR(50) NOT NULL,
    Reason VARCHAR(255),
    ApprovalStatus VARCHAR(50) DEFAULT 'Pending',
    ManagerID INT NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
);

select * from Leaves
 
-- LoginLogs Table
CREATE TABLE LoginLogs (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    LoginDateTime DATETIME NOT NULL,
    LogoutDateTime DATETIME,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);

select * from LoginLogs
 
-- Add a new employee
CREATE or alter PROCEDURE spAddEmployee
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @HireDate DATE,
    @RoleID INT,
    @ManagerID INT = NULL,
    @PasswordHash VARCHAR(256),
    @PasswordSalt VARCHAR(256)
AS
BEGIN
    INSERT INTO Employees (FirstName, LastName, Email, Phone, HireDate, RoleID, ManagerID, PasswordHash)
    VALUES (@FirstName, @LastName, @Email, @Phone, @HireDate, @RoleID, @ManagerID, @PasswordHash);
END


 
-- Update an existing employee
CREATE  or alter PROCEDURE spUpdateEmployee
    @EmployeeID INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @HireDate DATE,
    @RoleID INT,
    @ManagerID INT = NULL,
    @ProjectID INT = NULL,
    @LeaveBalance INT = 3
AS
BEGIN
    UPDATE Employees
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Phone = @Phone,
        HireDate = @HireDate,
        RoleID = @RoleID,
        ManagerID = @ManagerID,
        ProjectID = @ProjectID,
        LeaveBalance = @LeaveBalance
    WHERE EmployeeID = @EmployeeID;
END



 
--soft Delete an employee
CREATE or alter PROCEDURE spDeleteEmployee
    @EmployeeID INT
AS
BEGIN
    DELETE FROM Employees WHERE EmployeeID = @EmployeeID;
END

 
-- Add a new project
CREATE or alter PROCEDURE spAddProject
    @ProjectName VARCHAR(100),
    @Description VARCHAR(255)
AS
BEGIN
    INSERT INTO Projects (ProjectName, Description)
    VALUES (@ProjectName, @Description);
END

 
-- Update an existing project
CREATE or alter PROCEDURE spUpdateProject
    @ProjectID INT,
    @ProjectName VARCHAR(100),
    @Description VARCHAR(255)
AS
BEGIN
    UPDATE Projects
    SET ProjectName = @ProjectName,
        Description = @Description
    WHERE ProjectID = @ProjectID;
END

 
-- soft Delete a project
CREATE or alter PROCEDURE spDeleteProject
    @ProjectID INT
AS
BEGIN
    DELETE FROM Projects WHERE ProjectID = @ProjectID;
END

 
-- Add a new attendance record
CREATE or alter PROCEDURE spAddAttendance
    @EmployeeID INT,
    @ProjectID INT,
    @AttendanceDate DATE,
    @Status VARCHAR(50),
    @ManagerID INT
AS
BEGIN
    DECLARE @ApprovalStatus VARCHAR(50) = 'Pending';
    
    IF @Status = 'Present'
    BEGIN
        SET @ApprovalStatus = 'Approved';
    END
 
    INSERT INTO Attendance (EmployeeID, ProjectID, AttendanceDate, Status, ManagerID, ApprovalStatus)
    VALUES (@EmployeeID, @ProjectID, @AttendanceDate, @Status, @ManagerID, @ApprovalStatus);
END

 
-- Update an existing attendance record
CREATE or alter PROCEDURE spUpdateAttendance
    @AttendanceID INT,
    @Status VARCHAR(50),
    @ApprovalStatus VARCHAR(50)
AS
BEGIN
    UPDATE Attendance
    SET Status = @Status,
        ApprovalStatus = @ApprovalStatus
    WHERE AttendanceID = @AttendanceID;
END

 
-- Add a new leave request
CREATE or alter PROCEDURE spAddLeaveRequest
    @EmployeeID INT,
    @StartDate DATE,
    @EndDate DATE,
    @LeaveType VARCHAR(50),
    @Reason VARCHAR(255),
    @ManagerID INT
AS
BEGIN
    INSERT INTO Leaves (EmployeeID, StartDate, EndDate, LeaveType, Reason, ManagerID, ApprovalStatus)
    VALUES (@EmployeeID, @StartDate, @EndDate, @LeaveType, @Reason, @ManagerID, 'Pending');
END

 
-- Update an existing leave request
CREATE or alter PROCEDURE spUpdateLeaveRequest
    @LeaveID INT,
    @ApprovalStatus VARCHAR(50)
AS
BEGIN
    UPDATE Leaves
    SET ApprovalStatus = @ApprovalStatus
    WHERE LeaveID = @LeaveID;
END

 
-- Get pending leave requests for a specific manager
CREATE or alter PROCEDURE spGetPendingLeaveRequests
    @ManagerID INT
AS
BEGIN
    SELECT * FROM Leaves
    WHERE ManagerID = @ManagerID AND ApprovalStatus = 'Pending';
END

 
-- Approve or reject a leave request
CREATE or alter PROCEDURE spApproveRejectLeaveRequest
    @LeaveID INT,
    @ApprovalStatus VARCHAR(50)
AS
BEGIN
    UPDATE Leaves
    SET ApprovalStatus = @ApprovalStatus
    WHERE LeaveID = @LeaveID;
END

 
-- Get pending attendance approvals for a specific manager
CREATE or alter PROCEDURE spGetPendingAttendanceApprovals
    @ManagerID INT
AS
BEGIN
    SELECT * FROM Attendance
    WHERE ManagerID = @ManagerID AND ApprovalStatus = 'Pending';
END

 
-- Approve or reject attendance
CREATE or alter PROCEDURE spApproveRejectAttendance
    @AttendanceID INT,
    @ApprovalStatus VARCHAR(50)
AS
BEGIN
    UPDATE Attendance
    SET ApprovalStatus = @ApprovalStatus
    WHERE AttendanceID = @AttendanceID;
END

 