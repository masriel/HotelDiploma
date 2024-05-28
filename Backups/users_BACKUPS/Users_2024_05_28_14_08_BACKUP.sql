-- MySqlBackup.NET 2.3.8.0
-- Dump Time: 2024-05-28 14:08:42
-- --------------------------------------
-- Server version 5.6.51 MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of Users
-- 

DROP TABLE IF EXISTS `Users`;
CREATE TABLE IF NOT EXISTS `Users` (
  `userID` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `userEmail` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `userPassword` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `userType` int(11) NOT NULL,
  PRIMARY KEY (`userID`),
  KEY `userType` (`userType`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`userType`) REFERENCES `UserTypes` (`typeID`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- 
-- Dumping data for table Users
-- 

/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
INSERT INTO `Users`(`userID`,`userName`,`userEmail`,`userPassword`,`userType`) VALUES(1,'Екатерина Мухина','admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',1),(2,'Екатерина Мухина','user','04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb',2),(3,'Сергей Сидоров','sidorov3@example.com','bb6856b9d7b9775858348f2b11b147ebfa90a8aa53a0b67cc2f063c155351155',1),(4,'Анна Кузнецова','kuznetsova4@example.com','160e80f714d8f60b40f16d994a8244e45c945b70217aaf87d33c1a60c328791',2),(5,'Дмитрий Попов','popov5@example.com','5a5b3b7ab95a3c2ad2b0071b05a12b8261d7431b6d812ccda5c373f6dcf7af27',1),(6,'Елена Смирнова','smirnova6@example.com','2ff663315af44c496fb6b7f9cb7ef919a29d79a9fd9c10c0bc5c168883df0f49',2),(7,'Алексей Волков','volkov7@example.com','a8eb66b95e07ab34d822b6942e7628a4f60ab1745bb4c7d531b5f95eef0a562d',1),(8,'Ольга Морозова','morozova8@example.com','58047a97c32b7dc72069c961b37e80c1de907c7d9c0aa31debf579a42cb5751b',2),(9,'Михаил Зайцев','zaicev9@example.com','d7c08bfe5c9aa477e2c249d467ab6ec48d6d5c0875d6eb828ad8a5f30a7d1669',1),(10,'Татьяна Белоусова','belousova10@example.com','2e35570cb6d7c1e1d6bf46a013334e28e56a34e92d5c2a2c2a22041534d0841d',2),(11,'Павел Михайлов','mikhailov11@example.com','4bb01a61db4bc3b2332521b58aa1201eb0fba1fc4e11a39c2252f5bcf72a90dc',1),(12,'Наталья Воробьева','vorobeva12@example.com','0380e2a21f3cf8cf66e544ad964f7f28f0aa37808548969aa163b4265b4f6e79',2),(13,'Владимир Федоров','fedorov13@example.com','a9f29180b3a5ac9cf1397b99394146c3d125af46b73fe0c2e21b8cd9a9ec453d',1),(14,'Екатерина Александрова','aleksandrova14@example.com','2301542e771911e9a9e64f23883f577300df5824544e92fb14f1a01820db3f82',2),(15,'Роман Николаев','nikolaev15@example.com','35fe5ab54f12c2217a3e6d1e0214932f11c09d9ef74925e973da74005992d0c4',1),(16,'Юлия Макарова','makarova16@example.com','482c811da5d5b4bc6d497ffa98491e38fab005eef42166a73b5f037b4d982a2a',2),(17,'Андрей Лебедев','lebedev17@example.com','75bc4c48af4d2a97b3c2c6f0fe0a53ad56a0d3435c6e2fb6ac7c687a0a2a8b3c',1),(18,'Оксана Киселева','kiseleva18@example.com','d04e7a7b971f37b8530b5ecbe29849d4e8efdbd51d41b657cab35eb9fe8e0a1e',2),(19,'Игорь Павлов','pavlov19@example.com','92d5d9796bdf8713695cf76c587787b3289b7cf53a22b2d3a3e88bc73f5f0f6f',1),(20,'Светлана Григорьева','grigorieva20@example.com','09db7e01b75dcbfe75c49e77b7e148a08d1413beeff93f63e48f3f072ef169da',2),(21,'Виктор Тихонов','tikhonov21@example.com','8621ffdbc56988270d7ff63e689d8289be54f1fbfaecde71d6473e13d9184d36',1),(22,'Людмила Семенова','semenova22@example.com','c1c06fbbf15c3efadfb2c9a3de2317d72914582de07a8a25fb3e93f3e5c8fb4d',2),(23,'Анатолий Крылов','krylov23@example.com','d8583d6b84aedd98d86bb5cc815b0114979c5e8d214e4a3f71e7eb9145790b1b',1),(24,'Марина Орлова','orlova24@example.com','0f37d08491a301b038f8ab16d48f4b20d2670e6eb58a4a02c54b99295b049434',2),(25,'Николай Захаров','zaharov25@example.com','50e3f80527f9a8499b4404872ed5e02f18a0cd1d34d0c717b620a8b950c4a2e1',1),(26,'Алена Миронова','mironova26@example.com','3d81b2711d6efab3afda41e836a12abf7d3d3ebf66358a1618c2085a88881ff8',2),(27,'Григорий Константинов','konstantinov27@example.com','3d34fc6b2c8e003cb8b58a14c64815f6dcd25fb3b6a2af5623e70d6fbd1114bc',1),(28,'Вероника Соколова','sokolova28@example.com','1a53677c197f5e41c9ee05e18a3af7d94c0a40e1a760860ff566bc27c0e9a4a0',2),(29,'Вадим Ермаков','ermakov29@example.com','deb00ec2aa70b0f3c4c832b43fde2453cf5d8ed5a62676b06ab4f750ddaf500f',1),(30,'Дарья Голубева','golubeva30@example.com','7a68f647da587203273ba0f0563f9df27b53e1c61b1d4b7f6fc2f3cd512d47f5',2),(31,'Станислав Мартынов','martynov31@example.com','6a5ef87492e6dab9e759df0ab7bcff25b31d7a8f865f301fd3bdc103be6c31d6',1),(32,'Ксения Фролова','frolova32@example.com','f5d84cfbafbb95b1e883107d6202843147d42b8e3b36fd6c9f10113b571759fd',2),(33,'Юрий Сафонов','safonov33@example.com','cf5b83dd952a370fed2d4d5e4ca62e0a9ec8bbede345200c79c36704b9b4b4ab',1),(34,'Олеся Романова','romanova34@example.com','4e84d1278c31d9dbffbf7e8a7d2b70ab91b9a78d7e4b09e68420058d0c5bfe29',2),(35,'Валерий Васильев','vasiliev35@example.com','bfd999feff8e0d4a770c97f9b6bb29026b9e1a3f1bb11609d5d1f7143db94d44',1),(36,'Тамара Гусева','guseva36@example.com','df47b6cf7ac072bc7a60e43b4b6d78985904c5159c5eeb317f1079a5db582f54',2),(37,'Максим Иванов','ivanov37@example.com','d2de79725c2040f39e43995b1d8ab0c01fb64ad556d08df208245bc1f97d4d19',1),(38,'Лариса Климова','klimova38@example.com','7238a79a2ef3b9585b94634ddc53e9503d0cb4412534fa27f6ff536bb5eb61e4',2),(39,'Евгений Петров','petrov39@example.com','2c4a46390bf328f876d9c6bb460b7b9f93a9ab7e3f43a6c7d80d9dab5adefde5',1),(40,'Алина Егорова','egorova40@example.com','5be24713f5be662d4e9c7ebc240e10e18fe91dd3a2a6f1c9a33f930c120d2f49',2),(41,'Константин Мельников','melnicov41@example.com','1808bb74cc01a5f6ad9a31500efb8e4be2c5c7d168d41703dc0fe130982da21e',1),(42,'Ирина Савельева','saveleva42@example.com','57d331bc8bfef906c23a062a8cf76cd1727a0950136fb6cd9202dc32e4d481db',2),(43,'Глеб Тимофеев','timofeev43@example.com','4a3a73fb1c25ec947f101cde4ff3c82db370d15b46b31411aa3fc6160847cc50',1),(44,'Жанна Королева','koroleva44@example.com','66d565d6a275d5e70e40a95b597b67d6725a4807017a28c6d26e18bb8da5e69a',2),(45,'Федор Захаров','zaharov45@example.com','88501be58ac072ff49a99d9c0ad30af4c5feaf4ee3aa27c6fa74f82f0e62a8a2',1),(46,'Лидия Никитина','nikitina46@example.com','4f89a0e8b1e89a1b6a92e0adccfa0d79d45d8e5f992c7b5307e20c8532a5febf',2),(47,'Семен Соколов','sokolov47@example.com','5d69662c7ff8b9e9f86f1d8b1c6c81c9a2f13bb9e04b71b4c8f3e2ddfb16fbec',1),(48,'Виктория Козлова','kozlova48@example.com','4a66a3f381fa4d5793fc5479c26c4070467ef906b8b003ce0a13b55c1f464e06',2),(49,'Александр Иванов','ivanov49@example.com','d1df9fb416b1a157d1e43ea98a1b36028a055a106899e62db8e946f5ebd6fbbd',1),(50,'Вера Мартынова','martynova50@example.com','3e5f17815a5b5f155479dce8db3083441a8f65b241a4d7e34b8a7b374b286678',2),(52,'Кабанов Михаил','test@test.com','cbad1e9256eb7400bada7f9b64265fd430fb7c1f25de20783c05fcadee4a968c',2);
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2024-05-28 14:08:42
-- Total time: 0:0:0:0:114 (d:h:m:s:ms)
