-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: library
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `library`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `library` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `library`;

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `books` (
  `BookId` int NOT NULL AUTO_INCREMENT,
  `Isbn` varchar(60) DEFAULT NULL,
  `Title` varchar(200) NOT NULL,
  `Author` varchar(160) DEFAULT NULL,
  `Category` varchar(100) DEFAULT NULL,
  `TotalCopies` int NOT NULL,
  `AvailableCopies` int NOT NULL,
  `AccessionNumber` varchar(80) DEFAULT NULL,
  `BookType` varchar(80) NOT NULL DEFAULT 'General',
  PRIMARY KEY (`BookId`)
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `books`
--

LOCK TABLES `books` WRITE;
/*!40000 ALTER TABLE `books` DISABLE KEYS */;
INSERT INTO `books` VALUES (1,'','Introduction to Algorithms','Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, and Clifford Stein','Algorithms',1,1,'STARTER-001','Textbook'),(2,'','Algorithms','Robert Sedgewick and Kevin Wayne','Algorithms',1,1,'STARTER-002','Textbook'),(3,'','The Algorithm Design Manual','Steven S. Skiena','Algorithms',1,1,'STARTER-003','Textbook'),(4,'','A Common-Sense Guide to Data Structures and Algorithms','Jay Wengrow','Algorithms',1,1,'STARTER-004','Textbook'),(5,'','Data Structures and Algorithms in Java','Michael T. Goodrich, Roberto Tamassia, and Michael H. Goldwasser','Algorithms',1,1,'STARTER-005','Textbook'),(6,'','Clean Code','Robert C. Martin','Software Engineering',1,1,'STARTER-006','Textbook'),(7,'','The Pragmatic Programmer','David Thomas and Andrew Hunt','Software Engineering',1,1,'STARTER-007','Textbook'),(8,'','Code Complete','Steve McConnell','Software Engineering',1,1,'STARTER-008','Textbook'),(9,'','Refactoring','Martin Fowler','Software Engineering',1,1,'STARTER-009','Textbook'),(10,'','Design Patterns','Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides','Software Engineering',1,1,'STARTER-010','Textbook'),(11,'','Head First Design Patterns','Eric Freeman and Elisabeth Robson','Software Engineering',1,1,'STARTER-011','Textbook'),(12,'','Working Effectively with Legacy Code','Michael Feathers','Software Engineering',1,1,'STARTER-012','Textbook'),(13,'','Domain-Driven Design','Eric Evans','Software Engineering',1,1,'STARTER-013','Textbook'),(14,'','Patterns of Enterprise Application Architecture','Martin Fowler','Software Engineering',1,1,'STARTER-014','Textbook'),(15,'','Software Engineering','Ian Sommerville','Software Engineering',1,1,'STARTER-015','Textbook'),(16,'','Software Engineering: A Practitioner\'s Approach','Roger S. Pressman and Bruce R. Maxim','Software Engineering',1,1,'STARTER-016','Textbook'),(17,'','Computer Networking: A Top-Down Approach','James F. Kurose and Keith W. Ross','Networking',1,1,'STARTER-017','Textbook'),(18,'','Computer Networks','Andrew S. Tanenbaum and David J. Wetherall','Networking',1,1,'STARTER-018','Textbook'),(19,'','TCP/IP Illustrated, Volume 1','W. Richard Stevens and Kevin R. Fall','Networking',1,1,'STARTER-019','Textbook'),(20,'','Operating System Concepts','Abraham Silberschatz, Peter B. Galvin, and Greg Gagne','Operating Systems',1,1,'STARTER-020','Textbook'),(21,'','Modern Operating Systems','Andrew S. Tanenbaum and Herbert Bos','Operating Systems',1,1,'STARTER-021','Textbook'),(22,'','Operating Systems: Three Easy Pieces','Remzi H. Arpaci-Dusseau and Andrea C. Arpaci-Dusseau','Operating Systems',1,1,'STARTER-022','Textbook'),(23,'','Computer Organization and Design','David A. Patterson and John L. Hennessy','Computer Architecture',1,1,'STARTER-023','Textbook'),(24,'','Computer Architecture: A Quantitative Approach','John L. Hennessy and David A. Patterson','Computer Architecture',1,1,'STARTER-024','Textbook'),(25,'','Database System Concepts','Abraham Silberschatz, Henry F. Korth, and S. Sudarshan','Databases',1,1,'STARTER-025','Textbook'),(26,'','Fundamentals of Database Systems','Ramez Elmasri and Shamkant B. Navathe','Databases',1,1,'STARTER-026','Textbook'),(27,'','Database Systems: The Complete Book','Hector Garcia-Molina, Jeffrey D. Ullman, and Jennifer Widom','Databases',1,1,'STARTER-027','Textbook'),(28,'','Learning SQL','Alan Beaulieu','Databases',1,1,'STARTER-028','Textbook'),(29,'','SQL in 10 Minutes, Sams Teach Yourself','Ben Forta','Databases',1,1,'STARTER-029','Textbook'),(30,'','Artificial Intelligence: A Modern Approach','Stuart Russell and Peter Norvig','Artificial Intelligence',1,1,'STARTER-030','Textbook'),(31,'','Deep Learning','Ian Goodfellow, Yoshua Bengio, and Aaron Courville','Artificial Intelligence',1,1,'STARTER-031','Textbook'),(32,'','Pattern Recognition and Machine Learning','Christopher M. Bishop','Artificial Intelligence',1,1,'STARTER-032','Textbook'),(33,'','Hands-On Machine Learning with Scikit-Learn, Keras, and TensorFlow','Aurelien Geron','Artificial Intelligence',1,1,'STARTER-033','Textbook'),(34,'','Python Crash Course','Eric Matthes','Programming',1,1,'STARTER-034','Textbook'),(35,'','Automate the Boring Stuff with Python','Al Sweigart','Programming',1,1,'STARTER-035','Textbook'),(36,'','Fluent Python','Luciano Ramalho','Programming',1,1,'STARTER-036','Textbook'),(37,'','Effective Python','Brett Slatkin','Programming',1,1,'STARTER-037','Textbook'),(38,'','The C Programming Language','Brian W. Kernighan and Dennis M. Ritchie','Programming',1,1,'STARTER-038','Textbook'),(39,'','The C++ Programming Language','Bjarne Stroustrup','Programming',1,1,'STARTER-039','Textbook'),(40,'','Effective Modern C++','Scott Meyers','Programming',1,1,'STARTER-040','Textbook'),(41,'','Programming Language Pragmatics','Michael L. Scott','Programming Languages',1,1,'STARTER-041','Textbook'),(42,'','Compilers: Principles, Techniques, and Tools','Alfred V. Aho, Monica S. Lam, Ravi Sethi, and Jeffrey D. Ullman','Programming Languages',1,1,'STARTER-042','Textbook'),(43,'','Structure and Interpretation of Computer Programs','Harold Abelson and Gerald Jay Sussman','Programming Languages',1,1,'STARTER-043','Textbook'),(44,'','Computer Security: Principles and Practice','William Stallings and Lawrie Brown','Cybersecurity',1,1,'STARTER-044','Textbook'),(45,'','Security Engineering','Ross Anderson','Cybersecurity',1,1,'STARTER-045','Textbook'),(46,'','Web Development with Node and Express','Ethan Brown','Web Development',1,1,'STARTER-046','Textbook'),(47,'','Learning Web Design','Jennifer Niederst Robbins','Web Development',1,1,'STARTER-047','Textbook'),(48,'','JavaScript: The Definitive Guide','David Flanagan','Web Development',1,1,'STARTER-048','Textbook'),(49,'','Eloquent JavaScript','Marijn Haverbeke','Web Development',1,1,'STARTER-049','Textbook'),(50,'','You Don\'t Know JS Yet: Get Started','Kyle Simpson','Web Development',1,1,'STARTER-050','Textbook'),(51,'','HTML and CSS: Design and Build Websites','Jon Duckett','Web Development',1,1,'STARTER-051','Textbook');
/*!40000 ALTER TABLE `books` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loanreferences`
--

DROP TABLE IF EXISTS `loanreferences`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loanreferences` (
  `LoanType` varchar(60) NOT NULL,
  `DaysAllowed` int NOT NULL,
  PRIMARY KEY (`LoanType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loanreferences`
--

LOCK TABLES `loanreferences` WRITE;
/*!40000 ALTER TABLE `loanreferences` DISABLE KEYS */;
INSERT INTO `loanreferences` VALUES ('Extended',30),('Overnight',1),('Regular',14),('Short-term',7);
/*!40000 ALTER TABLE `loanreferences` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loans`
--

DROP TABLE IF EXISTS `loans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loans` (
  `LoanId` int NOT NULL AUTO_INCREMENT,
  `BookId` int NOT NULL,
  `MemberId` int NOT NULL,
  `BorrowedDate` date NOT NULL,
  `DueDate` date NOT NULL,
  `ReturnedDate` date DEFAULT NULL,
  `LoanType` varchar(60) NOT NULL DEFAULT 'Regular',
  `AccessionNumber` varchar(80) DEFAULT NULL,
  `PenaltyDays` int NOT NULL DEFAULT '0',
  `PenaltyAmount` decimal(10,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`LoanId`),
  KEY `BookId` (`BookId`),
  KEY `MemberId` (`MemberId`),
  CONSTRAINT `loans_ibfk_1` FOREIGN KEY (`BookId`) REFERENCES `books` (`BookId`),
  CONSTRAINT `loans_ibfk_2` FOREIGN KEY (`MemberId`) REFERENCES `students` (`MemberId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loans`
--

LOCK TABLES `loans` WRITE;
/*!40000 ALTER TABLE `loans` DISABLE KEYS */;
/*!40000 ALTER TABLE `loans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `penaltyrates`
--

DROP TABLE IF EXISTS `penaltyrates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `penaltyrates` (
  `BookType` varchar(80) NOT NULL,
  `RatePerDay` decimal(10,2) NOT NULL DEFAULT '10.00',
  PRIMARY KEY (`BookType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `penaltyrates`
--

LOCK TABLES `penaltyrates` WRITE;
/*!40000 ALTER TABLE `penaltyrates` DISABLE KEYS */;
INSERT INTO `penaltyrates` VALUES ('General',10.00);
/*!40000 ALTER TABLE `penaltyrates` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `MemberId` int NOT NULL AUTO_INCREMENT,
  `StudentId` varchar(60) NOT NULL,
  `FullName` varchar(180) NOT NULL,
  `Course` varchar(160) DEFAULT NULL,
  `Status` varchar(30) NOT NULL DEFAULT 'Active',
  `YearLevel` varchar(30) NOT NULL DEFAULT '',
  PRIMARY KEY (`MemberId`),
  UNIQUE KEY `StudentId` (`StudentId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,'LIB-BSCS4-001','Denver Amoson','BSCS','Active','4th Year'),(2,'LIB-BSCS4-002','Kobe Salida','BSCS','Active','4th Year'),(3,'LIB-BSCS4-003','Kier Lanayon','BSCS','Active','4th Year');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'library'
--

--
-- Dumping routines for database 'library'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-28 22:01:28
