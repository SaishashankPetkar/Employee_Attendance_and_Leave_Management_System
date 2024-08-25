CREATE TABLE Leaves (
    LeaveID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    LeaveType VARCHAR(50) NOT NULL,
    Reason VARCHAR(255),
    ApprovalStatus VARCHAR(50) DEFAULT 'Pending',
    ManagerID INT NOT NULL,
    --FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    --FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
);
