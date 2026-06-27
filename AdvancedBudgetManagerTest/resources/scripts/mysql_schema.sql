CREATE TABLE users (
  userID int NOT NULL AUTO_INCREMENT,
  username varchar(20) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  salt binary(32) NOT NULL,
  password varchar(100) NOT NULL,
  email varchar(30) NOT NULL,
  PRIMARY KEY (userID),
  UNIQUE KEY username (username),
  UNIQUE KEY username_2 (username)
);

CREATE TABLE income_types (
  typeID int NOT NULL AUTO_INCREMENT,
  typeName varchar(20) NOT NULL,
  PRIMARY KEY (typeID)
);

CREATE TABLE incomes (
  incomeID int NOT NULL AUTO_INCREMENT,
  user_ID int NOT NULL,
  name varchar(50) NOT NULL,
  incomeType int NOT NULL,
  value int NOT NULL,
  date date NOT NULL,
  PRIMARY KEY (incomeID),
  KEY user_ID (user_ID),
  KEY incomeType (incomeType),
  CONSTRAINT incomes_ibfk_1 FOREIGN KEY (user_ID) REFERENCES users (userID),
  CONSTRAINT incomes_ibfk_2 FOREIGN KEY (incomeType) REFERENCES income_types (typeID)
);
