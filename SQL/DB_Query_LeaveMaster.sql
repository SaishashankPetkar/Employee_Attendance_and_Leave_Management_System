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


-----------------------------------------------------------------------------------------------------------------------------------

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




 

