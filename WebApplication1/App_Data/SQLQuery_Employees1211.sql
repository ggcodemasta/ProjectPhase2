-- DATABASE ----------------------------
USE master
GO 

IF EXISTS (SELECT * FROM sysdatabases WHERE name='Employees')
  DROP DATABASE Employees
GO

CREATE DATABASE Employees
GO

USE Employees
GO

-- DELETE OLD TABLES  ----------------------------
IF OBJECT_ID('Career')
	IS NOT NULL DROP TABLE Career
GO
IF OBJECT_ID('ProfileSkill')
	IS NOT NULL DROP TABLE ProfileSkill
GO 
IF OBJECT_ID('Skill')
	IS NOT NULL DROP TABLE Skill
GO 

IF OBJECT_ID('SvcProfile')
	IS NOT NULL DROP TABLE SvcProfile
GO
IF OBJECT_ID('SvcType')
	IS NOT NULL DROP TABLE SvcType
GO 
IF OBJECT_ID('Admin')
	IS NOT NULL DROP TABLE Admin
IF OBJECT_ID('Profile')
	IS NOT NULL DROP TABLE Profile
GO 

-- CREATE TABLES  ----------------------------
CREATE TABLE Profile
(
	profileID INTEGER IDENTITY(1,1) PRIMARY KEY,
	email VARCHAR(100) NOT NULL,
	password VARCHAR(20) NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	lastName VARCHAR(30) NOT NULL,
	linkedinURL VARCHAR(255),
	pictureURL VARCHAR(255),
	portfolioURL VARCHAR(255),
	highestEducation VARCHAR(30),
	relocationYN VARCHAR(10) default 'yes',
	country VARCHAR(30),
	province VARCHAR(30),
	city VARCHAR(30),
	-- CONSTRAINT chkHighestEducation CHECK (highestEducation IN ('??')),
	CONSTRAINT chkRelocationYN CHECK (relocationYN IN ('yes','no'))
);
GO 


CREATE TABLE Career
(
	careerID INTEGER IDENTITY(1,1) PRIMARY KEY,
	profileID INTEGER NOT NULL,
	industry VARCHAR(50) NOT NULL,
	company VARCHAR(50) NOT NULL,
	jobTitle VARCHAR(50) NOT NULL,
	years INTEGER NOT NULL,
	description VARCHAR(255),
	FOREIGN KEY(profileID) REFERENCES Profile(profileID)
);
GO


CREATE TABLE Skill
(
	skillID	INTEGER IDENTITY(1,1) PRIMARY KEY,
	skillName VARCHAR(50) NOT NULL,
);
GO

CREATE TABLE ProfileSkill
(
		profileID INTEGER NOT NULL,
		skillID INTEGER NOT NULL,
		FOREIGN KEY(profileID) REFERENCES Profile(profileID),
		FOREIGN KEY(skillID) REFERENCES Skill(skillID)
);
GO

CREATE TABLE Platform
(
	platformID	INTEGER IDENTITY(1,1) PRIMARY KEY,
	platformName VARCHAR(50) NOT NULL,
);

CREATE TABLE ProfilePlatform
(
		profileID INTEGER NOT NULL,
		platformID INTEGER NOT NULL,
		FOREIGN KEY(profileID) REFERENCES Profile(profileID),
		FOREIGN KEY(platformID) REFERENCES Platform(platformIDID)
);
GO


CREATE TABLE SvcType
(
	svcTypeID INTEGER IDENTITY(1,1) PRIMARY KEY,
	svcName VARCHAR(50) NOT NULL,
	svcCharge MONEY NOT NULL,
	svcWeeks INTEGER NOT NULL,
	description	VARCHAR(255)
);
GO


CREATE TABLE SvcProfile
(
	svcProfileID INTEGER IDENTITY(1,1) PRIMARY KEY,
	profileID INTEGER NOT NULL,
	svcTypeID INTEGER NOT NULL,
	billingDate DATETIME DEFAULT GETDATE() NOT NULL,
	billingCode VARCHAR(100) NOT NULL,	
	billingMethod VARCHAR(50) NOT NULL,
	svcStartDate DATETIME,
	svcEndDate DATETIME,
	FOREIGN KEY(profileID) REFERENCES Profile(profileID),
	FOREIGN KEY(svcTypeID) REFERENCES SvcType(svcTypeID)
);
GO

CREATE TABLE Admin
(
	adminID INTEGER IDENTITY(1,1) PRIMARY KEY,
	profileID INTEGER NOT NULL,
	FOREIGN KEY(profileID) REFERENCES Profile(profileID)
);
GO 


-- INSERT SAMPLE DATA ----------------------------
INSERT INTO Profile( email, password, firstName, lastName, linkedinURL, pictureURL, 
					portfolioURL, highestEducation, relocationYN, country, province, city)
VALUES('dba@ericvanier.com', 'startend', 'Eric', 'Vanier', 
		'https://www.linkedin.com/profile/view?id=24619626&authType=name&authToken=Hyoh&offset=4&trk=prof-sb-pdm-similar-photo',
		'https://media.licdn.com/mpr/mpr/shrink_240_240/p/7/005/00e/2da/2cb0eb1.jpg',
		'', 'BA', 'yes', 'Canada', 'Quebec', 'Montreal');


INSERT INTO Profile( email, password, firstName, lastName, linkedinURL, pictureURL, 
					portfolioURL, highestEducation, relocationYN, country, province, city)
VALUES('Rana_Dhaliwal@globalrelay.com', 'passw0rd', 'Rana', 'Dhaliwal', 
		'https://www.linkedin.com/profile/view?id=278206798',
		'https://media.licdn.com/mpr/mpr/shrink_200_200/p/2/005/03d/2b2/2f8e20e.jpg',
		'', 'BA', 'yes', 'Canada', 'BC', 'Surrey');


INSERT INTO Profile( email, password, firstName, lastName, linkedinURL, pictureURL, 
					portfolioURL, highestEducation, relocationYN, country, province, city)
VALUES('kiyohiko@socilogica.com', 'da7876', 'Kiyohiko Daniel', 'Takeuchi', 
		'https://www.linkedin.com/in/kiyohiko',
		'https://media.licdn.com/mpr/mpr/shrink_200_200/p/7/005/024/2ac/04c9f4c.jpg',
		'', 'certificate', 'yes', 'Canada', 'BC', 'Vancouver');

INSERT INTO Profile( email, password, firstName, lastName )
VALUES ('treefieldsun@gmail.com', 'admin1','Cassie', 'Xu');
INSERT INTO Profile( email, password, firstName, lastName )
VALUES ('grant2381@gmail.com', 'admin2','Grant', 'Yao');
INSERT INTO Profile( email, password, firstName, lastName )
VALUES ('seleong@gmail.com', 'admin3','Steve', 'Leong');
INSERT INTO Profile( email, password, firstName, lastName )
VALUES ('calmwoods@gmail.com', 'admin4','Dana', 'Jin');

INSERT INTO Skill( category, skillName, description )
VALUES ( 'Design', 'CSS', 'Style sheet language used for describing the look and formatting of a document');
INSERT INTO Skill( category, skillName, description )
VALUES ('Programming', 'C#', 'A multi-paradigm programming language skill encompassing strong typing');
INSERT INTO Skill( category, skillName, description )
VALUES ('DB', 'SQL Server', ' SQL Server is a relational database management system developed by Microsoft.');
INSERT INTO Skill( category, skillName, description )
VALUES ('Programming', 'JAVA', 'Java is a general-purpose computer programming language');

INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 1, 1 );
INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 1, 2 );
INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 1, 3 );
INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 2, 1 );
INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 2, 3 );
INSERT INTO ProfileSkill( profileID, skillID )
VALUES( 3, 4 );


INSERT INTO Career( profileID, industry, company, jobTitle, years, description )
VALUES( 1, 'Technology', 'Desjardins', 'Technology Coordonator', 4, 
		'* Technlogies activities Coordonnator and responsible of integration recommendations for CGI and Fédération Desjardins' );
INSERT INTO Career( profileID, industry, company, jobTitle, years, description )
VALUES( 1, 'DB Technology', 'Axper inc.', 'MSSQL Senior DBA', 1, 
		'* MSSQL database administration (replication clusters, scalability techniques, optimization, backup)' );

INSERT INTO Career( profileID, industry, company, jobTitle, years, description )
VALUES( 2, 'Software', 'Global Relay', 'Information Systems Developer (Co-op)', 2, 
	'Worked with our customer facing teams to improve business processes by creating custom internal features' );

INSERT INTO Career( profileID, industry, company, jobTitle, years, description )
VALUES( 3, 'Web', 'socilogica', 'Web Developer', 3, 
    'My main duties include Front End and Back-end web and mobile development using C#, AngularJS, Ionic, BackboneJS, and HTML.' );


INSERT INTO SvcType( svcName, svcCharge, svcWeeks, description )
VALUES( 'Basic Service', 99.99, 4, 'The most common and primary service.');
INSERT INTO SvcType( svcName, svcCharge, svcWeeks, description )
VALUES( 'Short-Term Service', 60.99, 2, 'The service period is only two weeks.');
INSERT INTO SvcType( svcName, svcCharge, svcWeeks, description )
VALUES( 'Long-Term Service', 110.99, 6, 'It is for the user who needs to be showed for six weeks.');



INSERT INTO SvcProfile( profileID, svcTypeID, billingCode, billingMethod, svcStartDate, svcEndDate )
VALUES( 1, 1, 'A293847203801', 'VISA Card',  '2014-05-10 00:00:00', '2014-06-06 00:00:00') ; 

INSERT INTO SvcProfile( profileID, svcTypeID, billingCode, billingMethod, svcStartDate, svcEndDate )
VALUES( 2, 1, 'A293847203803', 'VISA Card',  '2014-12-10 00:00:00', '2015-01-06 00:00:00') ; 

INSERT INTO SvcProfile( profileID, svcTypeID, billingCode, billingMethod, svcStartDate, svcEndDate )
VALUES( 3, 1, 'A293847203835', 'VISA Card',  '2014-09-15 00:00:00', '2014-10-12 00:00:00') ; 

INSERT INTO Admin( profileID ) VALUES( 4 );
INSERT INTO Admin( profileID ) VALUES( 5 );
INSERT INTO Admin( profileID ) VALUES( 6 );
INSERT INTO Admin( profileID ) VALUES( 7 );

CREATE TABLE Industry
(
	industryName VARCHAR(50) PRIMARY KEY,
);
GO 
CREATE TABLE Jobtitle
(
	jobTitle VARCHAR(50) PRIMARY KEY,
);
GO 
-------------------------------------------------------------------- updated Tues 3/17  -----------------------------------------------------------
ALTER TABLE Career
ADD FOREIGN KEY (jobTitle)
REFERENCES Jobtitle (jobTitle)

ALTER TABLE Career
ADD FOREIGN KEY (industry)
REFERENCES Industry (industryName)

INSERT INTO Jobtitle VALUES ('Analyst')
INSERT INTO Jobtitle VALUES ('Project Manager')
INSERT INTO Jobtitle VALUES ('Tester')
INSERT INTO Jobtitle VALUES ('Supervisor')
INSERT INTO Jobtitle VALUES ('Front end Developer')
INSERT INTO Jobtitle VALUES ('Back end Developer')
INSERT INTO Jobtitle VALUES ('Full Stack Developer')


INSERT INTO Industry VALUES ('Video Game','')
INSERT INTO Industry VALUES ('Telecommunications','')
INSERT INTO Industry VALUES ('Networking','')
INSERT INTO Industry VALUES ('Computer Hardware','')
INSERT INTO Industry VALUES ('Banking','')
INSERT INTO Industry VALUES ('Educational services','')
INSERT INTO Industry VALUES ('Health Care and Social Services','')
INSERT INTO Industry VALUES ('Retail','')
INSERT INTO Industry VALUES ('Manufacturing','')
INSERT INTO Industry VALUES ('Finance','')
INSERT INTO Industry VALUES ('Transportation','')

INSERT INTO skill VALUES('Ajax')
INSERT INTO skill VALUES('AngularJS')
INSERT INTO skill VALUES('AspectC++')
INSERT INTO skill VALUES('Assembly')
INSERT INTO skill VALUES('ASP.NET')
INSERT INTO skill VALUES('BASIC')
INSERT INTO skill VALUES('C')
INSERT INTO skill VALUES('C++')
INSERT INTO skill VALUES('C#')
INSERT INTO skill VALUES('C-RIMM')
INSERT INTO skill VALUES('CSS')
INSERT INTO skill VALUES('Fortran')
INSERT INTO skill VALUES('Java')
INSERT INTO skill VALUES('Javascript')
INSERT INTO skill VALUES('MySQL')
INSERT INTO skill VALUES('NodeJS')
INSERT INTO skill VALUES('Objective C')
INSERT INTO skill VALUES('Perl')
INSERT INTO skill VALUES('PHP')
INSERT INTO skill VALUES('Python')
INSERT INTO skill VALUES('Ruby')
INSERT INTO skill VALUES('SQL/Variations')
INSERT INTO skill VALUES('Visual Basic')
INSERT INTO skill VALUES('XML')
INSERT INTO skill VALUES('Laravel')
INSERT INTO skill VALUES('Adobe Suite')
INSERT INTO skill VALUES('JQuery')
INSERT INTO skill VALUES('Json')
INSERT INTO skill VALUES('Twitter BootStrap')


INSERT INTO platform VALUES ('Windows')
INSERT INTO platform VALUES ('Mac')
INSERT INTO platform VALUES ('Linux')
INSERT INTO platform VALUES ('Android')
INSERT INTO platform VALUES ('iOS')
INSERT INTO platform VALUES ('Windows Phone')
INSERT INTO platform VALUES ('Blackberry')



INSERT INTO profileskill VALUES ('1','1')
INSERT INTO profileskill VALUES ('1','2')
INSERT INTO profileskill VALUES ('1','3')
INSERT INTO profileskill VALUES ('1','4')
INSERT INTO profileskill VALUES ('1','5')
INSERT INTO profileplatform VALUES ('1','1')
INSERT INTO profileplatform VALUES ('1','2')
INSERT INTO profileplatform VALUES ('1','3')
INSERT INTO profileplatform VALUES ('1','4')
INSERT INTO profileplatform VALUES ('1','5')


INSERT INTO profileskill VALUES ('2','1')
INSERT INTO profileskill VALUES ('2','2')
INSERT INTO profileskill VALUES ('2','3')
INSERT INTO profileskill VALUES ('2','4')
INSERT INTO profileskill VALUES ('2','5')
INSERT INTO profileplatform VALUES ('2','1')
INSERT INTO profileplatform VALUES ('2','2')
INSERT INTO profileplatform VALUES ('2','3')
INSERT INTO profileplatform VALUES ('2','4')
INSERT INTO profileplatform VALUES ('2','5')

INSERT INTO profileskill VALUES ('3','1')
INSERT INTO profileskill VALUES ('3','2')
INSERT INTO profileskill VALUES ('3','3')
INSERT INTO profileskill VALUES ('3','4')
INSERT INTO profileskill VALUES ('3','5')
INSERT INTO profileplatform VALUES ('3','1')
INSERT INTO profileplatform VALUES ('3','2')
INSERT INTO profileplatform VALUES ('3','3')
INSERT INTO profileplatform VALUES ('3','4')
INSERT INTO profileplatform VALUES ('3','5')


-- SHOW DATA  ----------------------------
SELECT * FROM Profile;
SELECT * FROM Career;
SELECT * FROM Skill;
SELECT * FROM ProfileSkill
SELECT * FROM SvcType;
SELECT * FROM SvcProfile;
SELECT * FROM Admin; 

