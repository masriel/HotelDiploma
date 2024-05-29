-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2024-05-29 08:51:27
-- --------------------------------------
-- Server version 8.0.36 MySQL Community Server - GPL


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of birthcertificate
-- 

DROP TABLE IF EXISTS `birthcertificate`;
CREATE TABLE IF NOT EXISTS `birthcertificate` (
  `birthCertificateID` int NOT NULL AUTO_INCREMENT,
  `registrationNumber` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `issueDate` date NOT NULL,
  `issuingAuthority` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`birthCertificateID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table birthcertificate
-- 

/*!40000 ALTER TABLE `birthcertificate` DISABLE KEYS */;

/*!40000 ALTER TABLE `birthcertificate` ENABLE KEYS */;

-- 
-- Definition of clientpassports
-- 

DROP TABLE IF EXISTS `clientpassports`;
CREATE TABLE IF NOT EXISTS `clientpassports` (
  `passportID` int NOT NULL AUTO_INCREMENT,
  `passportSeries` varchar(4) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `passportNumber` varchar(6) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `issueDate` date NOT NULL,
  `issuingAuthority` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`passportID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table clientpassports
-- 

/*!40000 ALTER TABLE `clientpassports` DISABLE KEYS */;
INSERT INTO `clientpassports`(`passportID`,`passportSeries`,`passportNumber`,`issueDate`,`issuingAuthority`) VALUES(1,'4510','123456','2010-05-12 00:00:00','ОВД Пресненского района г. Москвы'),(2,'4511','234567','2008-11-23 00:00:00','ОВД Тверского района г. Москвы'),(3,'4512','345678','2005-07-09 00:00:00','ОВД Замоскворечья г. Москвы'),(4,'4513','456789','2012-03-15 00:00:00','ОВД Хамовников г. Москвы'),(5,'4514','567890','2001-10-30 00:00:00','ОВД Басманного района г. Москвы'),(6,'4515','678901','2003-06-18 00:00:00','ОВД Красносельского района г. Москвы'),(7,'4516','789012','2011-09-27 00:00:00','ОВД Мещанского района г. Москвы'),(8,'4517','890123','2009-04-21 00:00:00','ОВД Таганского района г. Москвы'),(9,'4518','901234','2004-12-05 00:00:00','ОВД Арбатского района г. Москвы'),(10,'4519','12345','2006-01-14 00:00:00','ОВД Савёловского района г. Москвы'),(11,'4520','123456','2007-08-22 00:00:00','ОВД Соколиной горы г. Москвы'),(12,'4521','234567','2002-02-08 00:00:00','ОВД Лефортово г. Москвы'),(13,'4522','345678','2013-11-19 00:00:00','ОВД Марьиной рощи г. Москвы'),(14,'4523','456789','2004-09-16 00:00:00','ОВД Кунцево г. Москвы'),(15,'4524','567890','2000-07-25 00:00:00','ОВД Солнцево г. Москвы'),(16,'4525','678901','2011-05-30 00:00:00','ОВД Выхино-Жулебино г. Москвы'),(17,'4526','789012','2008-03-12 00:00:00','ОВД Бутырского района г. Москвы'),(18,'4527','890123','2004-06-03 00:00:00','ОВД Нагатино-Садовники г. Москвы'),(19,'4528','901234','2009-12-11 00:00:00','ОВД Тропарёво-Никулино г. Москвы'),(20,'4529','12345','2010-02-24 00:00:00','ОВД Черёмушки г. Москвы'),(21,'4530','123456','2002-08-30 00:00:00','ОВД Котловка г. Москвы'),(22,'4531','234567','2004-10-17 00:00:00','ОВД Теплый Стан г. Москвы'),(23,'4532','345678','2000-11-04 00:00:00','ОВД Москворечье-Сабурово г. Москвы'),(24,'4533','456789','2012-09-19 00:00:00','ОВД Братеево г. Москвы'),(25,'4534','567890','2001-05-01 00:00:00','ОВД Орехово-Борисово Южное г. Москвы'),(26,'4535','678901','2006-07-28 00:00:00','ОВД Зябликово г. Москвы'),(27,'4536','789012','2004-04-10 00:00:00','ОВД Бирюлево Восточное г. Москвы'),(28,'4537','890123','2013-11-05 00:00:00','ОВД Даниловский г. Москвы'),(29,'4538','901234','2003-02-14 00:00:00','ОВД Донской г. Москвы'),(30,'4539','12345','2009-03-23 00:00:00','ОВД Нагатинский Затон г. Москвы'),(31,'4540','123456','2007-10-08 00:00:00','ОВД Нагорный г. Москвы'),(32,'4541','234567','2004-11-29 00:00:00','ОВД Чертаново Северное г. Москвы'),(33,'4542','345678','2005-08-02 00:00:00','ОВД Чертаново Центральное г. Москвы'),(34,'4543','456789','2012-12-12 00:00:00','ОВД Чертаново Южное г. Москвы'),(35,'4544','567890','2001-06-16 00:00:00','ОВД Северное Бутово г. Москвы'),(36,'4545','678901','2000-04-07 00:00:00','ОВД Южное Бутово г. Москвы'),(37,'4546','789012','2004-09-11 00:00:00','ОВД Коньково г. Москвы'),(38,'4547','890123','2000-11-03 00:00:00','ОВД Беляево г. Москвы'),(39,'4548','901234','2010-06-22 00:00:00','ОВД Ясенево г. Москвы'),(40,'4549','12345','2002-07-31 00:00:00','ОВД Южное Тушино г. Москвы'),(41,'4550','123456','2005-05-29 00:00:00','ОВД Северное Тушино г. Москвы'),(42,'4551','234567','2013-12-17 00:00:00','ОВД Покровское-Стрешнево г. Москвы'),(43,'4552','345678','2008-03-06 00:00:00','ОВД Хорошёво-Мнёвники г. Москвы'),(44,'4553','456789','2012-10-21 00:00:00','ОВД Щукино г. Москвы'),(45,'4554','567890','2004-08-27 00:00:00','ОВД Строгино г. Москвы'),(46,'4555','678901','2000-01-19 00:00:00','ОВД Митино г. Москвы'),(47,'4556','789012','2011-07-14 00:00:00','ОВД Куркино г. Москвы'),(48,'4557','890123','2006-05-09 00:00:00','ОВД Хорошёвский г. Москвы'),(49,'4558','901234','2009-11-02 00:00:00','ОВД Савёловский г. Москвы'),(50,'4559','12345','2013-03-14 00:00:00','ОВД Беговой г. Москвы');
/*!40000 ALTER TABLE `clientpassports` ENABLE KEYS */;

-- 
-- Definition of clients
-- 

DROP TABLE IF EXISTS `clients`;
CREATE TABLE IF NOT EXISTS `clients` (
  `clientID` int NOT NULL AUTO_INCREMENT,
  `firstName` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `lastName` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `middleName` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  `birthDate` date NOT NULL,
  `phoneNumber` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  `passport` int DEFAULT NULL,
  `birthCertificate` int DEFAULT NULL,
  PRIMARY KEY (`clientID`),
  KEY `passport` (`passport`),
  KEY `birthCertificate` (`birthCertificate`),
  CONSTRAINT `clients_ibfk_1` FOREIGN KEY (`passport`) REFERENCES `clientpassports` (`passportID`),
  CONSTRAINT `clients_ibfk_2` FOREIGN KEY (`birthCertificate`) REFERENCES `birthcertificate` (`birthCertificateID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table clients
-- 

/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients`(`clientID`,`firstName`,`lastName`,`middleName`,`birthDate`,`phoneNumber`,`email`,`passport`,`birthCertificate`) VALUES(1,'Алексей','Иванов','Петрович','1990-05-12 00:00:00','8 903 123 45 67','alexey.ivanov@example.com',1,NULL),(2,'Мария','Петрова','Александровна','1985-11-23 00:00:00','8 905 234 56 78','maria.petrova@example.com',2,NULL),(3,'Дмитрий','Смирнов','Сергеевич','1978-07-09 00:00:00','8 910 345 67 89','dmitry.smirnov@example.com',3,NULL),(4,'Ольга','Кузнецова','Ивановна','1992-03-15 00:00:00','8 915 456 78 90','olga.kuznetsova@example.com',4,NULL),(5,'Иван','Соколов','Андреевич','1980-10-30 00:00:00','8 916 567 89 01','ivan.sokolov@example.com',5,NULL),(6,'Елена','Попова','Дмитриевна','1983-06-18 00:00:00','8 917 678 90 12','elena.popova@example.com',6,NULL),(7,'Сергей','Лебедев','Николаевич','1991-09-27 00:00:00','8 918 789 01 23','sergey.lebedev@example.com',7,NULL),(8,'Анна','Козлова','Владимировна','1988-04-21 00:00:00','8 919 890 12 34','anna.kozlova@example.com',8,NULL),(9,'Андрей','Новиков','Михайлович','1979-12-05 00:00:00','8 920 901 23 45','andrey.novikov@example.com',9,NULL),(10,'Наталья','Морозова','Александровна','1986-01-14 00:00:00','8 925 012 34 56','natalya.morozova@example.com',10,NULL),(11,'Владимир','Федоров','Павлович','1987-08-22 00:00:00','8 926 123 45 67','vladimir.fedorov@example.com',11,NULL),(12,'Светлана','Волкова','Георгиевна','1982-02-08 00:00:00','8 927 234 56 78','svetlana.volkova@example.com',12,NULL),(13,'Михаил','Соловьев','Петрович','1993-11-19 00:00:00','8 928 345 67 89','mikhail.solovev@example.com',13,NULL),(14,'Юлия','Васильева','Андреевна','1984-09-16 00:00:00','8 929 456 78 90','yulia.vasileva@example.com',14,NULL),(15,'Александр','Зайцев','Евгеньевич','1977-07-25 00:00:00','8 930 567 89 01','alexander.zaicev@example.com',15,NULL),(16,'Татьяна','Павлова','Михайловна','1991-05-30 00:00:00','8 931 678 90 12','tatiana.pavlova@example.com',16,NULL),(17,'Евгений','Семенов','Владимирович','1985-03-12 00:00:00','8 932 789 01 23','evgeny.semenov@example.com',17,NULL),(18,'Ирина','Голубева','Сергеевна','1979-06-03 00:00:00','8 933 890 12 34','irina.golubeva@example.com',18,NULL),(19,'Максим','Виноградов','Павлович','1988-12-11 00:00:00','8 934 901 23 45','maxim.vinogradov@example.com',19,NULL),(20,'Екатерина','Миронова','Александровна','1990-02-24 00:00:00','8 935 012 34 56','ekaterina.mironova@example.com',20,NULL),(21,'Николай','Мельников','Юрьевич','1982-08-30 00:00:00','8 936 123 45 67','nikolay.melnikov@example.com',21,NULL),(22,'Галина','Антонова','Леонидовна','1984-10-17 00:00:00','8 937 234 56 78','galina.antonova@example.com',22,NULL),(23,'Павел','Григорьев','Игоревич','1977-11-04 00:00:00','8 938 345 67 89','pavel.grigorev@example.com',23,NULL),(24,'Валентина','Тихонова','Николаевна','1992-09-19 00:00:00','8 939 456 78 90','valentina.tikhonova@example.com',24,NULL),(25,'Роман','Крылов','Андреевич','1981-05-01 00:00:00','8 950 567 89 01','roman.krylov@example.com',25,NULL),(26,'Алина','Макарова','Вячеславовна','1986-07-28 00:00:00','8 951 678 90 12','alina.makarova@example.com',26,NULL),(27,'Константин','Афанасьев','Владимирович','1979-04-10 00:00:00','8 952 789 01 23','konstantin.afanasyev@example.com',27,NULL),(28,'Олеся','Никитина','Сергеевна','1993-11-05 00:00:00','8 953 890 12 34','olesya.nikitina@example.com',28,NULL),(29,'Василий','Гаврилов','Петрович','1983-02-14 00:00:00','8 954 901 23 45','vasily.gavrilov@example.com',29,NULL),(30,'Маргарита','Рыбакова','Ивановна','1989-03-23 00:00:00','8 955 012 34 56','margarita.rybakova@example.com',30,NULL),(31,'Виталий','Куликов','Георгиевич','1987-10-08 00:00:00','8 956 123 45 67','vitaliy.kulikov@example.com',31,NULL),(32,'Вера','Сафонова','Михайловна','1984-11-29 00:00:00','8 957 234 56 78','vera.safonova@example.com',32,NULL),(33,'Виктор','Ершов','Александрович','1978-08-02 00:00:00','8 958 345 67 89','viktor.ershov@example.com',33,NULL),(34,'Людмила','Игнатова','Петровна','1992-12-12 00:00:00','8 959 456 78 90','lyudmila.ignatova@example.com',34,NULL),(35,'Геннадий','Шишкин','Сергеевич','1981-06-16 00:00:00','8 960 567 89 01','gennady.shishkin@example.com',35,NULL),(36,'Жанна','Орлова','Ивановна','1980-04-07 00:00:00','8 961 678 90 12','zhanna.orlova@example.com',36,NULL),(37,'Игорь','Зиновьев','Дмитриевич','1983-09-11 00:00:00','8 962 789 01 23','igor.zinovyev@example.com',37,NULL),(38,'Марина','Шестакова','Георгиевна','1977-11-03 00:00:00','8 963 890 12 34','marina.shestakova@example.com',38,NULL),(39,'Кирилл','Костин','Андреевич','1990-06-22 00:00:00','8 964 901 23 45','kirill.kostin@example.com',39,NULL),(40,'София','Кудрявцева','Павловна','1982-07-31 00:00:00','8 965 012 34 56','sofia.kudryavtseva@example.com',40,NULL),(41,'Борис','Мясников','Николаевич','1978-05-29 00:00:00','8 966 123 45 67','boris.myasnikov@example.com',41,NULL),(42,'Оксана','Зверева','Владимировна','1989-12-17 00:00:00','8 967 234 56 78','oksana.zvereva@example.com',42,NULL),(43,'Артур','Панкратов','Сергеевич','1985-03-06 00:00:00','8 968 345 67 89','artur.pankratov@example.com',43,NULL),(44,'Полина','Щербакова','Ивановна','1993-10-21 00:00:00','8 969 456 78 90','polina.scherbakova@example.com',44,NULL),(45,'Рустам','Логинов','Михайлович','1984-08-27 00:00:00','8 970 567 89 01','rustam.loginov@example.com',45,NULL),(46,'Алиса','Чернова','Александровна','1979-01-19 00:00:00','8 971 678 90 12','alisa.chernova@example.com',46,NULL),(47,'Ярослав','Мещеряков','Павлович','1991-07-14 00:00:00','8 972 789 01 23','yaroslav.mescheryakov@example.com',47,NULL),(48,'Лариса','Рябова','Андреевна','1986-05-09 00:00:00','8 973 890 12 34','larisa.ryabova@example.com',48,NULL),(49,'Григорий','Бобров','Дмитриевич','1988-11-02 00:00:00','8 974 901 23 45','grigoriy.bobrov@example.com',49,NULL),(50,'Кузнецов','Руслан','Ильич','1974-01-02 00:00:00','9 974 901 23 47','ruslan.kuznetsov@example.com',50,NULL);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;

-- 
-- Definition of bookingclients
-- 

DROP TABLE IF EXISTS `bookingclients`;
CREATE TABLE IF NOT EXISTS `bookingclients` (
  `bookingClientsID` int NOT NULL AUTO_INCREMENT,
  `booking` int NOT NULL,
  `client` int NOT NULL,
  PRIMARY KEY (`bookingClientsID`),
  KEY `booking` (`booking`),
  KEY `client` (`client`),
  CONSTRAINT `bookingclients_ibfk_1` FOREIGN KEY (`booking`) REFERENCES `bookings` (`bookingID`),
  CONSTRAINT `bookingclients_ibfk_2` FOREIGN KEY (`client`) REFERENCES `clients` (`clientID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table bookingclients
-- 

/*!40000 ALTER TABLE `bookingclients` DISABLE KEYS */;

/*!40000 ALTER TABLE `bookingclients` ENABLE KEYS */;

-- 
-- Definition of meals
-- 

DROP TABLE IF EXISTS `meals`;
CREATE TABLE IF NOT EXISTS `meals` (
  `mealID` int NOT NULL AUTO_INCREMENT,
  `mealName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `mealCost` double NOT NULL,
  PRIMARY KEY (`mealID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table meals
-- 

/*!40000 ALTER TABLE `meals` DISABLE KEYS */;
INSERT INTO `meals`(`mealID`,`mealName`,`mealCost`) VALUES(1,'Завтрак',700),(2,'Обед',950),(3,'Ужин',800);
/*!40000 ALTER TABLE `meals` ENABLE KEYS */;

-- 
-- Definition of bookingmeals
-- 

DROP TABLE IF EXISTS `bookingmeals`;
CREATE TABLE IF NOT EXISTS `bookingmeals` (
  `bookingMealID` int NOT NULL AUTO_INCREMENT,
  `booking` int NOT NULL,
  `meal` int NOT NULL,
  `quantity` int NOT NULL,
  PRIMARY KEY (`bookingMealID`),
  KEY `booking` (`booking`),
  KEY `meal` (`meal`),
  CONSTRAINT `bookingmeals_ibfk_1` FOREIGN KEY (`booking`) REFERENCES `bookings` (`bookingID`),
  CONSTRAINT `bookingmeals_ibfk_2` FOREIGN KEY (`meal`) REFERENCES `meals` (`mealID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table bookingmeals
-- 

/*!40000 ALTER TABLE `bookingmeals` DISABLE KEYS */;

/*!40000 ALTER TABLE `bookingmeals` ENABLE KEYS */;

-- 
-- Definition of roomtypes
-- 

DROP TABLE IF EXISTS `roomtypes`;
CREATE TABLE IF NOT EXISTS `roomtypes` (
  `roomTypeID` int NOT NULL AUTO_INCREMENT,
  `roomType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `maxOccupancy` int NOT NULL,
  `roomCost` double NOT NULL,
  `roomPhoto` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `roomDescription` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`roomTypeID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table roomtypes
-- 

/*!40000 ALTER TABLE `roomtypes` DISABLE KEYS */;
INSERT INTO `roomtypes`(`roomTypeID`,`roomType`,`maxOccupancy`,`roomCost`,`roomPhoto`,`roomDescription`) VALUES(1,'Одноместный',1,3500,'','Уютный номер с одной односпальной кроватью и видом на город.'),(2,'Двухместный',2,4500,'','Комфортабельный номер с двумя односпальными кроватями и ванной комнатой.'),(3,'Двухместный Люкс',2,5500,'','Просторный номер с большой двуспальной кроватью и отдельной гостиной зоной.'),(4,'Семейный',4,7000,'','Номер  идеально подходящий для семьи, с двумя спальнями и детской игровой зоной.'),(5,'Полулюкс',2,6000,'','Улучшенный номер с двуспальной кроватью и видом на море.'),(6,'Люкс',2,8000,'','Роскошный номер с отдельной гостиной и спальней,  джакузи и панорамным видом.'),(7,'Студия',3,5000,'','Современная студия с мини-кухней и зоной отдыха.'),(8,'Апартаменты',4,9000,'','Полностью оборудованные апартаменты с кухней и двумя спальнями.'),(9,'Королевский Люкс',2,15000,'','Эксклюзивный номер с королевским оформлением, собственной террасой и джакузи.'),(10,'Эконом',1,2500,'','Доступный номер с основными удобствами, идеально подходит для краткосрочного проживания.');
/*!40000 ALTER TABLE `roomtypes` ENABLE KEYS */;

-- 
-- Definition of rooms
-- 

DROP TABLE IF EXISTS `rooms`;
CREATE TABLE IF NOT EXISTS `rooms` (
  `roomID` int NOT NULL AUTO_INCREMENT,
  `roomNumber` int NOT NULL,
  `roomType` int NOT NULL,
  PRIMARY KEY (`roomID`),
  KEY `roomType` (`roomType`),
  CONSTRAINT `rooms_ibfk_1` FOREIGN KEY (`roomType`) REFERENCES `roomtypes` (`roomTypeID`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table rooms
-- 

/*!40000 ALTER TABLE `rooms` DISABLE KEYS */;
INSERT INTO `rooms`(`roomID`,`roomNumber`,`roomType`) VALUES(1,101,1),(2,102,1),(3,103,1),(4,104,1),(5,105,1),(6,106,2),(7,107,2),(8,108,2),(9,109,2),(10,110,2),(11,201,3),(12,202,3),(13,203,3),(14,204,4),(15,205,4),(16,206,4),(17,207,4),(18,208,4),(19,209,5),(20,210,5),(21,301,6),(22,302,6),(23,303,6),(24,304,6),(25,305,7),(26,306,7),(27,307,7),(28,308,7),(29,309,8),(30,310,8),(31,401,8),(32,402,8),(33,403,8),(34,404,8),(35,405,9),(36,406,9),(37,407,9),(38,501,10),(39,502,10),(40,503,10),(41,504,10),(42,505,10),(43,506,10),(44,507,10),(45,508,10),(46,509,10),(47,510,10),(48,601,6),(49,602,6),(50,603,9);
/*!40000 ALTER TABLE `rooms` ENABLE KEYS */;

-- 
-- Definition of bookings
-- 

DROP TABLE IF EXISTS `bookings`;
CREATE TABLE IF NOT EXISTS `bookings` (
  `bookingID` int NOT NULL AUTO_INCREMENT,
  `arrivalDate` date NOT NULL,
  `departureDate` date NOT NULL,
  `room` int NOT NULL,
  PRIMARY KEY (`bookingID`),
  KEY `room` (`room`),
  CONSTRAINT `bookings_ibfk_2` FOREIGN KEY (`room`) REFERENCES `rooms` (`roomID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table bookings
-- 

/*!40000 ALTER TABLE `bookings` DISABLE KEYS */;

/*!40000 ALTER TABLE `bookings` ENABLE KEYS */;

-- 
-- Definition of usertypes
-- 

DROP TABLE IF EXISTS `usertypes`;
CREATE TABLE IF NOT EXISTS `usertypes` (
  `typeID` int NOT NULL,
  `typeName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`typeID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table usertypes
-- 

/*!40000 ALTER TABLE `usertypes` DISABLE KEYS */;
INSERT INTO `usertypes`(`typeID`,`typeName`) VALUES(1,'Администратор'),(2,'Сотрудник ресепшн');
/*!40000 ALTER TABLE `usertypes` ENABLE KEYS */;

-- 
-- Definition of users
-- 

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `userID` int NOT NULL AUTO_INCREMENT,
  `userName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `userEmail` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `userPassword` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `userType` int NOT NULL,
  PRIMARY KEY (`userID`),
  KEY `userType` (`userType`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`userType`) REFERENCES `usertypes` (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table users
-- 

/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users`(`userID`,`userName`,`userEmail`,`userPassword`,`userType`) VALUES(1,'Екатерина Мухина','admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',1),(2,'Екатерина Мухина','user','04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb',2),(3,'Сергей Сидоров','sidorov3@example.com','bb6856b9d7b9775858348f2b11b147ebfa90a8aa53a0b67cc2f063c155351155',1),(4,'Анна Кузнецова','kuznetsova4@example.com','160e80f714d8f60b40f16d994a8244e45c945b70217aaf87d33c1a60c328791',2),(5,'Дмитрий Попов','popov5@example.com','5a5b3b7ab95a3c2ad2b0071b05a12b8261d7431b6d812ccda5c373f6dcf7af27',1),(6,'Елена Смирнова','smirnova6@example.com','2ff663315af44c496fb6b7f9cb7ef919a29d79a9fd9c10c0bc5c168883df0f49',2),(7,'Алексей Волков','volkov7@example.com','a8eb66b95e07ab34d822b6942e7628a4f60ab1745bb4c7d531b5f95eef0a562d',1),(8,'Ольга Морозова','morozova8@example.com','58047a97c32b7dc72069c961b37e80c1de907c7d9c0aa31debf579a42cb5751b',2),(9,'Михаил Зайцев','zaicev9@example.com','d7c08bfe5c9aa477e2c249d467ab6ec48d6d5c0875d6eb828ad8a5f30a7d1669',1),(10,'Татьяна Белоусова','belousova10@example.com','2e35570cb6d7c1e1d6bf46a013334e28e56a34e92d5c2a2c2a22041534d0841d',2),(11,'Павел Михайлов','mikhailov11@example.com','4bb01a61db4bc3b2332521b58aa1201eb0fba1fc4e11a39c2252f5bcf72a90dc',1),(12,'Наталья Воробьева','vorobeva12@example.com','0380e2a21f3cf8cf66e544ad964f7f28f0aa37808548969aa163b4265b4f6e79',2),(13,'Владимир Федоров','fedorov13@example.com','a9f29180b3a5ac9cf1397b99394146c3d125af46b73fe0c2e21b8cd9a9ec453d',1),(14,'Екатерина Александрова','aleksandrova14@example.com','2301542e771911e9a9e64f23883f577300df5824544e92fb14f1a01820db3f82',2),(15,'Роман Николаев','nikolaev15@example.com','35fe5ab54f12c2217a3e6d1e0214932f11c09d9ef74925e973da74005992d0c4',1),(16,'Юлия Макарова','makarova16@example.com','482c811da5d5b4bc6d497ffa98491e38fab005eef42166a73b5f037b4d982a2a',2),(17,'Андрей Лебедев','lebedev17@example.com','75bc4c48af4d2a97b3c2c6f0fe0a53ad56a0d3435c6e2fb6ac7c687a0a2a8b3c',1),(18,'Оксана Киселева','kiseleva18@example.com','d04e7a7b971f37b8530b5ecbe29849d4e8efdbd51d41b657cab35eb9fe8e0a1e',2),(19,'Игорь Павлов','pavlov19@example.com','92d5d9796bdf8713695cf76c587787b3289b7cf53a22b2d3a3e88bc73f5f0f6f',1),(20,'Светлана Григорьева','grigorieva20@example.com','09db7e01b75dcbfe75c49e77b7e148a08d1413beeff93f63e48f3f072ef169da',2),(21,'Виктор Тихонов','tikhonov21@example.com','8621ffdbc56988270d7ff63e689d8289be54f1fbfaecde71d6473e13d9184d36',1),(22,'Людмила Семенова','semenova22@example.com','c1c06fbbf15c3efadfb2c9a3de2317d72914582de07a8a25fb3e93f3e5c8fb4d',2),(23,'Анатолий Крылов','krylov23@example.com','d8583d6b84aedd98d86bb5cc815b0114979c5e8d214e4a3f71e7eb9145790b1b',1),(24,'Марина Орлова','orlova24@example.com','0f37d08491a301b038f8ab16d48f4b20d2670e6eb58a4a02c54b99295b049434',2),(25,'Николай Захаров','zaharov25@example.com','50e3f80527f9a8499b4404872ed5e02f18a0cd1d34d0c717b620a8b950c4a2e1',1),(26,'Алена Миронова','mironova26@example.com','3d81b2711d6efab3afda41e836a12abf7d3d3ebf66358a1618c2085a88881ff8',2),(27,'Григорий Константинов','konstantinov27@example.com','3d34fc6b2c8e003cb8b58a14c64815f6dcd25fb3b6a2af5623e70d6fbd1114bc',1),(28,'Вероника Соколова','sokolova28@example.com','1a53677c197f5e41c9ee05e18a3af7d94c0a40e1a760860ff566bc27c0e9a4a0',2),(29,'Вадим Ермаков','ermakov29@example.com','deb00ec2aa70b0f3c4c832b43fde2453cf5d8ed5a62676b06ab4f750ddaf500f',1),(30,'Дарья Голубева','golubeva30@example.com','7a68f647da587203273ba0f0563f9df27b53e1c61b1d4b7f6fc2f3cd512d47f5',2),(31,'Станислав Мартынов','martynov31@example.com','6a5ef87492e6dab9e759df0ab7bcff25b31d7a8f865f301fd3bdc103be6c31d6',1),(32,'Ксения Фролова','frolova32@example.com','f5d84cfbafbb95b1e883107d6202843147d42b8e3b36fd6c9f10113b571759fd',2),(33,'Юрий Сафонов','safonov33@example.com','cf5b83dd952a370fed2d4d5e4ca62e0a9ec8bbede345200c79c36704b9b4b4ab',1),(34,'Олеся Романова','romanova34@example.com','4e84d1278c31d9dbffbf7e8a7d2b70ab91b9a78d7e4b09e68420058d0c5bfe29',2),(35,'Валерий Васильев','vasiliev35@example.com','bfd999feff8e0d4a770c97f9b6bb29026b9e1a3f1bb11609d5d1f7143db94d44',1),(36,'Тамара Гусева','guseva36@example.com','df47b6cf7ac072bc7a60e43b4b6d78985904c5159c5eeb317f1079a5db582f54',2),(37,'Максим Иванов','ivanov37@example.com','d2de79725c2040f39e43995b1d8ab0c01fb64ad556d08df208245bc1f97d4d19',1),(38,'Лариса Климова','klimova38@example.com','7238a79a2ef3b9585b94634ddc53e9503d0cb4412534fa27f6ff536bb5eb61e4',2),(39,'Евгений Петров','petrov39@example.com','2c4a46390bf328f876d9c6bb460b7b9f93a9ab7e3f43a6c7d80d9dab5adefde5',1),(40,'Алина Егорова','egorova40@example.com','5be24713f5be662d4e9c7ebc240e10e18fe91dd3a2a6f1c9a33f930c120d2f49',2),(41,'Константин Мельников','melnicov41@example.com','1808bb74cc01a5f6ad9a31500efb8e4be2c5c7d168d41703dc0fe130982da21e',1),(42,'Ирина Савельева','saveleva42@example.com','57d331bc8bfef906c23a062a8cf76cd1727a0950136fb6cd9202dc32e4d481db',2),(43,'Глеб Тимофеев','timofeev43@example.com','4a3a73fb1c25ec947f101cde4ff3c82db370d15b46b31411aa3fc6160847cc50',1),(44,'Жанна Королева','koroleva44@example.com','66d565d6a275d5e70e40a95b597b67d6725a4807017a28c6d26e18bb8da5e69a',2),(45,'Федор Захаров','zaharov45@example.com','88501be58ac072ff49a99d9c0ad30af4c5feaf4ee3aa27c6fa74f82f0e62a8a2',1),(46,'Лидия Никитина','nikitina46@example.com','4f89a0e8b1e89a1b6a92e0adccfa0d79d45d8e5f992c7b5307e20c8532a5febf',2),(47,'Семен Соколов','sokolov47@example.com','5d69662c7ff8b9e9f86f1d8b1c6c81c9a2f13bb9e04b71b4c8f3e2ddfb16fbec',1),(48,'Виктория Козлова','kozlova48@example.com','4a66a3f381fa4d5793fc5479c26c4070467ef906b8b003ce0a13b55c1f464e06',2),(49,'Александр Иванов','ivanov49@example.com','d1df9fb416b1a157d1e43ea98a1b36028a055a106899e62db8e946f5ebd6fbbd',1),(50,'Вера Мартынова','martynova50@example.com','3e5f17815a5b5f155479dce8db3083441a8f65b241a4d7e34b8a7b374b286678',2),(52,'Кабанов Михаил','test@test.com','cbad1e9256eb7400bada7f9b64265fd430fb7c1f25de20783c05fcadee4a968c',2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2024-05-29 08:51:28
-- Total time: 0:0:0:0:192 (d:h:m:s:ms)
