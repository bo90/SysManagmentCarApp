create table NewEmployee
(id INTEGER NOT NULL PRIMARY KEY ,
 idEmp INTEGER NOT NULL,
 nameEmp VARCHAR(255),
 sName VARCHAR(255),
 enducation VARCHAR(255),
 enduPlace Varchar(255),
 originCity VARCHAR(255),
 age DATE,
 profession VARCHAR(255),
 telephone VARCHAR(50),
 email VARCHAR(255),
 FOREIGN KEY (idEmp) REFERENCES Employess(id));

