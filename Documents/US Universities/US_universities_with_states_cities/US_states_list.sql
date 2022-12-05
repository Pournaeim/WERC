-- MySQL dump 10.13  Distrib 5.5.28, for Win32 (x86)
--
-- Host: localhost    Database: insurance
-- ------------------------------------------------------
-- Server version	5.5.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `states`
--

DROP TABLE IF EXISTS `states`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `states` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `state_code` char(2) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `states`
--

LOCK TABLES `states` WRITE;
/*!40000 ALTER TABLE `states` DISABLE KEYS */;
INSERT INTO `states` VALUES (1,'Alaska','AK'),(2,'Alabama','AL'),(3,'Arkansas','AR'),(4,'Arizona','AZ'),(5,'California','CA'),(6,'Colorado','CO'),(7,'Connecticut','CT'),(8,'District of Columbia','DC'),(9,'Delaware','DE'),(10,'Florida','FL'),(11,'Georgia','GA'),(12,'Hawaii','HI'),(13,'Iowa','IA'),(14,'Idaho','ID'),(15,'Illinois','IL'),(16,'Indiana','IN'),(17,'Kansas','KS'),(18,'Kentucky','KY'),(19,'Louisiana','LA'),(20,'Massachusetts','MA'),(21,'Maryland','MD'),(22,'Maine','ME'),(23,'Michigan','MI'),(24,'Minnesota','MN'),(25,'Missouri','MO'),(26,'Mississippi','MS'),(27,'Montana','MT'),(28,'North Carolina','NC'),(29,'North Dakota','ND'),(30,'Nebraska','NE'),(31,'New Hampshire','NH'),(32,'New Jersey','NJ'),(33,'New Mexico','NM'),(34,'Nevada','NV'),(35,'New York','NY'),(36,'Ohio','OH'),(37,'Oklahoma','OK'),(38,'Oregon','OR'),(39,'Pennsylvania','PA'),(40,'Rhode Island','RI'),(41,'South Carolina','SC'),(42,'South Dakota','SD'),(43,'Tennessee','TN'),(44,'Texas','TX'),(45,'Utah','UT'),(46,'Virginia','VA'),(47,'Vermont','VT'),(48,'Washington','WA'),(49,'Wisconsin','WI'),(50,'West Virginia','WV'),(51,'Wyoming','WY');
/*!40000 ALTER TABLE `states` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-09-29 22:01:52
