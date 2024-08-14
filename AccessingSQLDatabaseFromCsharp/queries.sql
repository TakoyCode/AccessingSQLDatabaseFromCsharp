﻿-- Read - mange											-- CRUD = Create Read Update Delete
SELECT Id, FirstName, LastName, BirthYear
FROM Person
-- WHERE BirthYear > 1980

-- Read - én
SELECT Id, FirstName, LastName, BirthYear
FROM Person
WHERE Id = 1001

-- Read - én - med parameter
DECLARE @Id int = 1;
SELECT Id, FirstName, LastName, BirthYear
FROM Person
WHERE Id = @Id

-- Create
DECLARE @FirstName varchar(max) = 'Audun';
DECLARE @LastName varchar(max) = 'Nicolaisen';
DECLARE @BirthYear int = 2001;
INSERT INTO Person (FirstName, LastName, BirthYear)
VALUES (@FirstName, @LastName, @BirthYear)

-- Delete
DECLARE @Id int = 1;
DELETE FROM Person WHERE Id = @Id

-- Update
DECLARE @FirstName varchar(max) = 'Petter';
DECLARE @LastName varchar(max) = 'Pettersen';
DECLARE @Id int = 1;
UPDATE Person 
SET LastName = @LastName, FirstName = @FirstName
WHERE Id = @Id
-- WHERE BirthYear > @BirthYear


CREATE TABLE [dbo].[Person](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[BirthYear] [int] NULL
) 