-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: sbccashier
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
-- Current Database: `sbccashier`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `sbccashier` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `sbccashier`;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StudentKey` int NOT NULL,
  `PaymentType` varchar(60) NOT NULL,
  `Description` varchar(240) DEFAULT NULL,
  `Amount` decimal(12,2) NOT NULL,
  `PaymentMethod` varchar(30) NOT NULL,
  `ReferenceNumber` varchar(100) DEFAULT NULL,
  `PaymentDate` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `StudentKey` (`StudentKey`),
  CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`StudentKey`) REFERENCES `students` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registrarrequests`
--

DROP TABLE IF EXISTS `registrarrequests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `registrarrequests` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StudentKey` int NOT NULL,
  `DocumentType` varchar(60) NOT NULL,
  `Purpose` varchar(240) DEFAULT NULL,
  `Fee` decimal(12,2) NOT NULL,
  `Status` varchar(30) NOT NULL,
  `RequestedDate` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `StudentKey` (`StudentKey`),
  CONSTRAINT `registrarrequests_ibfk_1` FOREIGN KEY (`StudentKey`) REFERENCES `students` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registrarrequests`
--

LOCK TABLES `registrarrequests` WRITE;
/*!40000 ALTER TABLE `registrarrequests` DISABLE KEYS */;
/*!40000 ALTER TABLE `registrarrequests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StudentId` varchar(60) NOT NULL,
  `FullName` varchar(180) NOT NULL,
  `Course` varchar(160) DEFAULT NULL,
  `YearLevel` varchar(40) DEFAULT NULL,
  `Status` varchar(30) NOT NULL DEFAULT 'Active',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `StudentId` (`StudentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vehiclepasses`
--

DROP TABLE IF EXISTS `vehiclepasses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vehiclepasses` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StudentKey` int NOT NULL,
  `PlateNumber` varchar(60) NOT NULL,
  `VehicleType` varchar(60) NOT NULL,
  `Amount` decimal(12,2) NOT NULL,
  `PaymentMethod` varchar(30) NOT NULL,
  `Status` varchar(30) NOT NULL,
  `IssuedDate` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `StudentKey` (`StudentKey`),
  CONSTRAINT `vehiclepasses_ibfk_1` FOREIGN KEY (`StudentKey`) REFERENCES `students` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vehiclepasses`
--

LOCK TABLES `vehiclepasses` WRITE;
/*!40000 ALTER TABLE `vehiclepasses` DISABLE KEYS */;
/*!40000 ALTER TABLE `vehiclepasses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'sbccashier'
--

--
-- Dumping routines for database 'sbccashier'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-25  3:06:05
