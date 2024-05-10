DROP DATABASE IF EXISTS CECSproject;
CREATE DATABASE CECSproject;
USE CECSproject;
CREATE USER 'cecs535' IDENTIFIED BY 'taforever';
GRANT ALL PRIVILEGES ON CECSproject TO 'cecs535';

CREATE TABLE HOTEL (
hotelid VARCHAR(30),
address VARCHAR(50),
`manager-name` VARCHAR(50),
`number-rooms` INT(5),
amenities VARCHAR(255),
PRIMARY KEY(hotelid)
);

CREATE TABLE ROOM (
number VARCHAR(4),
type VARCHAR(8),
occupancy INT(1),
`number-beds` INT(1),
`type-beds` VARCHAR(5),
price DECIMAL(7,2),
`hotel-id` VARCHAR(30),
FOREIGN KEY(`hotel-id`) REFERENCES HOTEL (hotelid),
PRIMARY KEY(number,`hotel-id`)
);

CREATE TABLE CUSTOMER (
`cust-id` VARCHAR(30),
name VARCHAR(50),
street VARCHAR(40),
city VARCHAR(30),
zip VARCHAR(10),
status VARCHAR(8),
PRIMARY KEY(`cust-id`)
);

CREATE TABLE RESERVATION (
hotelid VARCHAR(30),
`cust-id` VARCHAR(30),
`room-number` VARCHAR(8),
`begin-date` DATE,
`end-date` DATE,
`credit-card-number` VARCHAR(20),
`exp-date` DATE,
FOREIGN KEY(hotelid) REFERENCES ROOM (`hotel-id`),
FOREIGN KEY(`room-number`) REFERENCES ROOM (number),
PRIMARY KEY(hotelid,`cust-id`, `room-number`,`begin-date`)
);

DELIMITER //
CREATE TRIGGER roomInsertionCheck
BEFORE INSERT ON ROOM 
FOR EACH ROW 
BEGIN
IF NEW.type != 'regular' AND NEW.type != 'extra' AND NEW.type != 'suite' AND NEW.type != 'business' AND NEW.type != 'luxury' AND NEW.type != 'family' THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'room type is invalid';
    END IF;
IF NEW.occupancy > 5 OR NEW.occupancy < 1 THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'occupancy is invalid (greater than 5 or less than 1)';
    END IF;
IF NEW.`number-beds` > 3 OR NEW.`number-beds` < 1 THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'number-beds is invalid (greater than 3 or less than 1)';
    END IF;
IF NEW.`type-beds` != 'queen' AND NEW.`type-beds` != 'king' AND NEW.`type-beds` != 'full' THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'type-beds is invalid';
    END IF;
IF NEW.price < 0 THEN
	SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'price is invalid (less than 0 or NaN)';
    END IF;
END; //

DELIMITER ;

DELIMITER //
CREATE PROCEDURE Occupancy (IN `number-id` VARCHAR(30), IN date DATE, OUT `num-of-reserved-rooms` INT(5))
BEGIN 
SELECT COUNT(*) INTO `num-of-reserved-rooms` FROM RESERVATION AS r
WHERE r.`begin-date` <= date AND r.`end-date` >= date AND r.hotelid = `number-id`;
END; //

DELIMITER ;

CREATE TABLE REVENUE (
hotelid VARCHAR(30),
total DECIMAL(20,2),
FOREIGN KEY(hotelid) REFERENCES HOTEL (hotelid),
PRIMARY KEY(hotelid)
);

DELIMITER //
CREATE TRIGGER updateTotal 
AFTER INSERT ON RESERVATION
FOR EACH ROW
BEGIN 
DECLARE oldTotal DECIMAL(20,2);
DECLARE roomDailyCost DECIMAL(20,2);
DECLARE newTotal DECIMAL(20,2);

SELECT rev.total INTO oldTotal 
FROM REVENUE AS rev 
WHERE rev.hotelid = NEW.hotelid;

SELECT r.price INTO roomDailyCost
FROM ROOM AS r
WHERE r.number = NEW.`room-number` AND r.`hotel-id` = NEW.hotelid;

SET newTotal = oldTotal + (DATEDIFF(NEW.`end-date`, NEW.`begin-date`) * roomDailyCost);
UPDATE REVENUE 
SET total = newTotal
WHERE hotelid = NEW.hotelid;
END; //

