CREATE DATABASE LeaveMaster;
USE LeaveMaster;

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
ALTER TABLE Projects
ADD Prj_status varchar(10) DEFAULT 'Active';

-- Employees Table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(13) NOT NULL UNIQUE,
    HireDate DATE NOT NULL,
    RoleID INT NOT NULL,
    ManagerID INT NULL,
    ProjectID INT NOT NULL,
    LeaveBalance INT DEFAULT 3,
	No_of_leave INT DEFAULT 0,
	Emp_status varchar(10)  ,
    Password VARCHAR(256) NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID)
);
drop  table Employees 
ALTER TABLE Employees
ADD CONSTRAINT DF_Employees_emp_status
DEFAULT 'Active' FOR emp_status;
select * from Employees

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

ALTER TABLE Attendance
ADD Atd_Status VARCHAR(50) NOT NULL Default 'Active'
select * from Attendance
Alter table Attendance 
Drop Column Leave_Status 
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
 ALTER TABLE Leaves
ADD Leave_Status VARCHAR(50) NOT NULL Default 'Active'
select * from Leaves

-- Add a new employee
CREATE or alter PROCEDURE spAddEmployee
    @EmployeeName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(15),
    @HireDate DATE,
    @RoleID INT,
    @ManagerID INT = NULL,
    @Password VARCHAR(256),
	@LeaveBalance INT = 3,
    @No_of_leave INT = 0,
    @Emp_status varchar(10) 
AS
BEGIN
    INSERT INTO Employees (EmployeeName , Email, Phone, HireDate, RoleID, ManagerID, Password,LeaveBalance , No_of_leave,Emp_status)
    VALUES (@EmployeeName, @Email, @Phone, @HireDate, @RoleID, @ManagerID, @Password,@LeaveBalance, @No_of_leave,@Emp_status );
END
 
 -- Update an existing employee
CREATE  or alter PROCEDURE spUpdateEmployee
    @EmployeeID INT,
    @EmployeeName VARCHAR(50),
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
    SET EmployeeName = @EmployeeName,
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
	UPDATE Employees SET Emp_status = 'Inactive' Where EmployeeID = @EmployeeID;
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
    @ProjectID INT,
	@Prj_status Varchar(10)
AS
BEGIN
    UPDATE Projects SET Prj_status = 'Inactive'  WHERE ProjectID = @ProjectID;
END


---------------------------------------------------------------------------------------------------
--1. Add Attendance

CREATE OR ALTER PROCEDURE spAddAttendance
    @EmployeeID INT,
    @ProjectID INT,
    @AttendanceDate DATE,
    @Status VARCHAR(50),
    @ManagerID INT
AS
BEGIN
    -- Declare variable for leave request check
    DECLARE @LeaveExists INT;

    -- Check if there is any leave request on the same date for the employee
    SELECT @LeaveExists = COUNT(*)
    FROM Leaves
    WHERE EmployeeID = @EmployeeID
      AND StartDate <= @AttendanceDate
      AND EndDate >= @AttendanceDate;

    -- Determine the approval status
    DECLARE @ApprovalStatus VARCHAR(50) = 'Pending';
    IF @LeaveExists = 0
    BEGIN
        SET @ApprovalStatus = 'Approved';
    END

    -- Insert the attendance record
    INSERT INTO Attendance (EmployeeID, ProjectID, AttendanceDate, Status, ApprovalStatus, ManagerID)
    VALUES (@EmployeeID, @ProjectID, @AttendanceDate, @Status, @ApprovalStatus, @ManagerID);
END;

-- Update an existing attendance record
CREATE OR ALTER PROCEDURE spUpdateAttendance
    @AttendanceID INT,
    @Status VARCHAR(50),
    @ManagerID INT
AS
BEGIN
    -- Declare variable for leave request check
    DECLARE @LeaveExists INT;
    DECLARE @AttendanceDate DATE;
    DECLARE @EmployeeID INT;

    -- Retrieve the current attendance date and employee ID
    SELECT @AttendanceDate = AttendanceDate, @EmployeeID = EmployeeID
    FROM Attendance
    WHERE AttendanceID = @AttendanceID;

    -- Check if there is any leave request on the same date for the employee
    SELECT @LeaveExists = COUNT(*)
    FROM Leaves
    WHERE EmployeeID = @EmployeeID
      AND StartDate <= @AttendanceDate
      AND EndDate >= @AttendanceDate;

    -- Determine the new approval status
    DECLARE @ApprovalStatus VARCHAR(50);
    IF @LeaveExists = 0
    BEGIN
        SET @ApprovalStatus = 'Approved';
    END
    ELSE
    BEGIN
        SET @ApprovalStatus = 'Pending';
    END

    -- Update the attendance record
    UPDATE Attendance
    SET Status = @Status,
        ApprovalStatus = @ApprovalStatus,
        ManagerID = @ManagerID
    WHERE AttendanceID = @AttendanceID;
END;
--Delete
CREATE OR ALTER PROCEDURE spDeleteAttendance
    @AttendanceID INT
AS
BEGIN
    -- Delete the attendance record
   UPDATE Attendance
    SET Atd_Status = 'Inactive'
	    WHERE AttendanceID = @AttendanceID;

END;

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

CREATE OR ALTER PROCEDURE spDeleteLeaveRequest
    @LeaveID INT
AS
BEGIN
    -- Delete the leave request
   UPDATE Leaves
    SET Leave_Status = 'Inactive'
    WHERE LeaveID = @LeaveID;
END;

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

-- Insert sample data into Roles
INSERT INTO Roles (RoleName) VALUES ('Developer');
INSERT INTO Roles (RoleName) VALUES ('Manager');
INSERT INTO Roles (RoleName) VALUES ('HR');
GO

-- Insert sample data into Projects
INSERT INTO Projects (ProjectName, Description) VALUES ('Project Alpha', 'A project focused on developing new features for the product.');
INSERT INTO Projects (ProjectName, Description) VALUES ('Project Beta', 'A project aimed at improving the existing features and fixing bugs.');
GO
-- Insert data into Employees table
INSERT INTO Employees (EmployeeName, Email, Phone, HireDate, RoleID, ManagerID, ProjectID, LeaveBalance, No_of_leave, Emp_status, Password)
VALUES 
('John Doe', 'john.doe@example.com', '555-0101', '2023-01-15', 2, NULL, 1, 10, 0, 'Active', 'passwordHash1'),

('Jane Smith', 'jane.smith@example.com', '555-0102', '2023-02-20', 1, 1, 1, 15, 0, 'Active', 'passwordHash2'),

('Emily Jones', 'emily.jones@example.com', '555-0103', '2023-03-10', 3, 1, 2, 12, 0, 'Active', 'passwordHash3')
;

GO
-- Insert sample data into Attendance
INSERT INTO Attendance (EmployeeID, ProjectID, AttendanceDate, Status, ManagerID, ApprovalStatus)
VALUES (2, 1, '2024-08-01', 'Present', 1, 'Approved');

INSERT INTO Attendance (EmployeeID, ProjectID, AttendanceDate, Status, ManagerID, ApprovalStatus)
VALUES (2, 1, '2024-08-02', 'Absent', 1, 'Pending');
GO
-- Insert sample data into Leaves
INSERT INTO Leaves (EmployeeID, StartDate, EndDate, LeaveType, Reason, ManagerID, ApprovalStatus)
VALUES (2, '2024-08-10', '2024-08-15', 'Vacation', 'Family trip', 1, 'Pending');

INSERT INTO Leaves (EmployeeID, StartDate, EndDate, LeaveType, Reason, ManagerID, ApprovalStatus)
VALUES (3, '2024-08-20', '2024-08-22', 'Sick Leave', 'Medical reasons', 1, 'Approved');
GO




 

