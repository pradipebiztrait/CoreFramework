-- phpMyAdmin SQL Dump
-- version 4.8.5
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Aug 18, 2020 at 02:06 PM
-- Server version: 10.1.38-MariaDB
-- PHP Version: 7.2.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `coreframework`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `Admin_GetAllUser` (INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT 
		u.UserId, 
        u.DeviceToken as Token, 
        u.DeviceType
	FROM users u
	WHERE u.UserType != 1
	AND u.DeviceToken IS NOT NULL
	ORDER BY u.CreatedOn DESC;
    
    SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `Admin_Login` (IN `uPassword` VARCHAR(500), IN `uEmail` VARCHAR(100), IN `uUserType` INT(11), INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	DECLARE returnId BIGINT(20);
    
	SELECT u.UserId INTO returnId FROM users u WHERE u.EmailAddress = uEmail AND u.Password = uPassword AND u.IsActive=1 AND u.UserType = uUserType;        
	IF returnId IS NOT NULL THEN          
		SELECT 
			u.*
			, c.CountryName
			, s.StateName
			, ct.CityName
            , (SELECT count(*) from planpurchasehistory ph where ph.UserId = u.UserId) AS IsPaid
		FROM users u
		LEFT JOIN countries c
		ON c.CountryId = u.CountryId
		LEFT JOIN states s
		ON s.StateId = u.StateId
		LEFT JOIN cities ct
		ON ct.CityId = u.CityId
		WHERE u.EmailAddress = uEmail AND u.Password = uPassword;
        
        SET SuccessMsg = "User logined has been successfully!";
		SET IsSuccess = 1;
		SELECT SuccessMsg, IsSuccess;
    ELSE
		SET SuccessMsg = "User EmailId and Password Invalid!";
		SET IsSuccess = 0;
		SELECT SuccessMsg, IsSuccess;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_AdminLogin` (IN `uPassword` VARCHAR(500), IN `uUserName` VARCHAR(100), INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	DECLARE returnId INT;
		SELECT COUNT(u.UserId) INTO returnId FROM users u WHERE u.EmailAddress = uUserName AND u.Password = uPassword;
	IF returnId = 1 THEN
		SELECT u.*  FROM users u 
		WHERE u.EmailAddress = uUserName AND u.Password = uPassword;
        SET SuccessMsg = "User logined has been successfully!";
		SET IsSuccess = 1;
		SELECT SuccessMsg, IsSuccess;
    ELSE
		SET SuccessMsg = "User EmailId and Password Invalid!";
		SET IsSuccess = 0;
		SELECT SuccessMsg, IsSuccess;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_DeleteSubAdminUser` (IN `oid` BIGINT(20), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	DELETE FROM users WHERE UserId = oid AND UserType = 2;
    
    SET SuccessMsg = "Staff Member user has been deleted successfully.";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`admin` PROCEDURE `API_GetAllCities` (IN `uStateId` BIGINT(20), INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM cities where StateId = uStateId;
	SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetAllCMS` (INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM pages;
    
    SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetAllCountries` (INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM countries;
    SET SuccessMsg = "Success";
    SET IsSuccess = 1;
    SELECT SuccessMsg,IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetAllStates` (IN `uCountryId` BIGINT(20), INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM states where CountryId = uCountryId;
	SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetAllSubAdmin` (INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT sa.* FROM users sa
	WHERE sa.UserType = 2
    ORDER BY sa.createdon DESC;
    
    SET SuccessMsg = "success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetAllUser` (INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT 
		u.UserId,
        u.ImagePath,
        u.FirstName,
        u.LastName,
        u.EmailAddress,
        u.IsActive
	FROM users u 
    where u.UserType = 3
    order by CreatedOn desc;
    
    SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetCMS` (IN `oPageId` INT(11), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM pages WHERE PageId = oPageId;
    
    SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetCMSByPageUrl` (IN `oPageUrl` VARCHAR(200), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	SELECT * FROM pages WHERE PageUrl = '/' + oPageUrl;
    
    SET SuccessMsg = "Success";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`admin`@`%` PROCEDURE `API_GetSubAdminUserById` (IN `ouser_id` INT(11))  BEGIN
	SELECT u.*  
    FROM users u
	WHERE u.UserId = ouser_id AND u.UserType = 2;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetUserByEmail` (IN `oemail` VARCHAR(100), INOUT `SuccessMsg` VARCHAR(500), INOUT `IsSuccess` INT)  BEGIN
	DECLARE returnId INT;
		SELECT COUNT(u.UserId) INTO returnId FROM users u WHERE u.EmailAddress = oemail;
	IF returnId = 1 THEN
		SELECT u.*  FROM users u 
		WHERE u.EmailAddress = oemail;
        SET SuccessMsg = "Success";
		SET IsSuccess = 1;
		SELECT SuccessMsg, IsSuccess;
    ELSE
		SET SuccessMsg = "Success";
		SET IsSuccess = 0;
		SELECT SuccessMsg, IsSuccess;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetUserById` (IN `oUserId` BIGINT(20))  BEGIN
	SELECT u.*  
    FROM users u
	WHERE u.UserId = oUserId;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_GetUserProfile` (IN `uToken` LONGTEXT, IN `uUserId` BIGINT(20), INOUT `uMessage` VARCHAR(500), INOUT `uStatus` INT)  BEGIN
DECLARE UID BIGINT(20);
SELECT  UserID INTO UID FROM  users WHERE Token=uToken and IsActive=1 and UserType != 1;
	IF UID = uUserId THEN
		SELECT 
			u.*
			, c.CountryName
			, s.StateName
			, ct.CityName
		FROM users u
		LEFT JOIN countries c
		ON c.CountryId = u.CountryId
		LEFT JOIN states s
		ON s.StateId = u.StateId
		LEFT JOIN cities ct
		ON ct.CityId = u.CityId
        WHERE u.UserId = UID;
        
        SET uMessage = "Success";
		SET uStatus = 1;
		SELECT uMessage, uStatus;
	ELSE
		SET uMessage = "Unauthorized User!";
		SET uStatus = 0;
		SELECT uMessage, uStatus;
	END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_IsActiveUser` (IN `oUserId` BIGINT(20), IN `oIsActive` INT(11), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	UPDATE users u SET u.IsActive = oIsActive WHERE u.UserId = oUserId;
    
    IF oIsActive = 1 THEN
		SET SuccessMsg = "User activate has been successfully!";	
    ELSE 
		SET SuccessMsg = "User deactivate has been successfully!";	
    END IF;    			
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_SaveAdminProfile` (IN `oUserId` BIGINT(20), IN `oEmail` VARCHAR(200), IN `oFirstName` VARCHAR(200), IN `oLastName` VARCHAR(200), IN `oImagePath` VARCHAR(200), IN `oMobileNumber` VARCHAR(15), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	UPDATE users u SET
	u.FirstName = oFirstName,
    u.LastName = oLastName,
    u.MobileNumber = oMobileNumber
    WHERE u.UserId = oUserId;
    
    SET SuccessMsg = "User profile has been updated successfully!";
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_SaveCMS` (IN `oPageId` INT(11), IN `oUserId` BIGINT(20), IN `oPageTitle` VARCHAR(200), IN `oPageUrl` LONGTEXT, IN `oPageDescription` LONGTEXT, IN `oIsActive` INT(11), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT)  BEGIN
	DECLARE existId INT;
    
    SELECT COUNT(*) INTO existId FROM pages WHERE PageId = oPageId;
    IF existId = 0 THEN
		INSERT INTO pages
			(PageId,
			PageTitle,
            PageUrl,
			PageDescription,
			IsActive,
			CreatedBy,
            CreatedOn,
            UpdatedBy,
            UpdatedOn)
			VALUES
			(NULL,
			oPageTitle,
			oPageUrl,
            oPageDescription,
			1,
            oUserId,
			CURRENT_TIMESTAMP,
            oUserId,
            CURRENT_TIMESTAMP);

		SET SuccessMsg = "CMS page has been added successfully";
    ELSE
       UPDATE pages SET 
	   PageTitle = oPageTitle,
       PageUrl = oPageUrl,
       IsActive = oIsActive,
       PageDescription = oPageDescription,
       UpdatedBy = oUserId,
       UpdatedOn = CURRENT_TIMESTAMP
	   WHERE PageId = oPageId;
       SET SuccessMsg = "CMS page has been updated successfully";
    END IF;
    
	SET IsSuccess = 1;
	SELECT SuccessMsg, IsSuccess;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_SaveSubAdmin` (IN `ouser_id` INT(11), IN `oid` INT(11), IN `ofirst_name` VARCHAR(100), IN `olast_name` VARCHAR(100), IN `oemail` VARCHAR(100), IN `opassword` VARCHAR(255), IN `ophone` VARCHAR(50), INOUT `SuccessMsg` VARCHAR(200), INOUT `IsSuccess` INT, INOUT `LastReturnId` BIGINT(20))  BEGIN
	DECLARE existId INT;
	DECLARE exist_email INT;
    DECLARE msg LONGTEXT;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
	BEGIN
		GET DIAGNOSTICS CONDITION 1	msg = MESSAGE_TEXT;
        SET LastReturnId = 0;
		SET SuccessMsg = 'SQLEXCEPTION - '+ msg;						
		SET IsSuccess = 0;
		SELECT SuccessMsg, IsSuccess;
	END;
    
    SELECT COUNT(*) INTO existId FROM users WHERE UserId = ouser_id;
	
    IF existId = 0 THEN    
    
		SELECT COUNT(*) INTO exist_email FROM users WHERE EmailAddress = oemail;
        
        IF exist_email = 0 THEN
			INSERT INTO `users`
				(`UserId`,
				`FirstName`,
				`LastName`,
				`DateOfBirth`,
				`MobileNumber`,
				`EmailAddress`,
				`Address`,
				`CountryId`,
				`StateId`,
				`CityId`,
				`PostalCode`,
				`IsActive`,
				`IsDelete`,
				`Token`,
				`OTP`,
				`Password`,
				`ImagePath`,
				`Gender`,
				`CreatedOn`,
				`UpdatedOn`,
				`UserType`,
				`AspNetUserId`,
				`DeviceToken`,
				`DeviceType`,
				`UdId`,
				`UserUniqueId`,
				`CreatedBy`,
				`UpdatedBy`,
				`Description`,
				`Qrcode`,
                `IsNotification`)
				VALUES
				(null,
				ofirst_name,
				olast_name,
				null,
				ophone,
				oemail,
				null,
				0,
				0,
				0,
				null,
				1,
				0,
				null,
				null,
				opassword,
				null,
				1,
				CURRENT_TIMESTAMP,
				CURRENT_TIMESTAMP,
				2,
				0,
				null,
				null,
				null,
				null,
				oid,
				oid,
				null,
				null,
                1);

			SET LastReturnId = LAST_INSERT_ID();
            SET IsSuccess = 1;
			SET SuccessMsg = "Sub admin has been added successfully.";
            SELECT SuccessMsg, IsSuccess, LastReturnId;	
        ELSE
			SET LastReturnId = 0;
			SET IsSuccess = 0;
			SET SuccessMsg = "Email has been already exists.";
            SELECT SuccessMsg, IsSuccess, LastReturnId;	
        END IF;
		
    ELSE
       UPDATE `users`
		SET
			`FirstName` = ofirst_name,
			`LastName` = olast_name,
			`MobileNumber` = ophone,
			`UpdatedBy` = oid,
            `UpdatedOn` = CURRENT_TIMESTAMP
		WHERE `UserId` = ouser_id;
		
		SET LastReturnId = ouser_id;
        SET IsSuccess = 1;
        SET SuccessMsg = "Sub admin has been updated successfully";
        SELECT SuccessMsg, IsSuccess, LastReturnId;		
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_SignUp` (IN `uUserId` BIGINT(20), IN `uEmailAddress` VARCHAR(200), IN `uPassword` VARCHAR(500), IN `uFirstName` VARCHAR(200), IN `uLastName` VARCHAR(200), IN `uToken` LONGTEXT, IN `uDeviceType` INT(11), IN `uDeviceToken` LONGTEXT, IN `uUDID` LONGTEXT, IN `uQrcode` VARCHAR(200), IN `uCityId` BIGINT(20), IN `uStateId` BIGINT(20), IN `uCountryId` BIGINT(20), IN `uUserType` INT(11), INOUT `uMessage` VARCHAR(500), INOUT `uStatus` INT)  BEGIN
	DECLARE returnId BIGINT(20);
    
    SELECT COUNT(*) INTO returnId FROM users u WHERE u.EmailAddress = uEmailAddress;
    
    IF returnId > 0 THEN          
        SET uMessage = "Email already registered.";
		SET uStatus = 0;
		SELECT uMessage, uStatus;
    ELSE
		INSERT INTO `users`
			(`UserId`,
			`FirstName`,
			`LastName`,
			`DateOfBirth`,
			`MobileNumber`,
			`EmailAddress`,
			`Address`,
			`CountryId`,
			`StateId`,
			`CityId`,
			`PostalCode`,
			`IsActive`,
			`IsDelete`,
			`Token`,
			`OTP`,
			`Password`,
			`ImagePath`,
			`Gender`,
			`CreatedOn`,
			`UpdatedOn`,
			`UserType`,
			`AspNetUserId`,
			`DeviceToken`,
			`DeviceType`,
			`UdId`,
			`UserUniqueId`,
			`CreatedBy`,
			`UpdatedBy`,
			`Description`,
			`Qrcode`,
            `IsNotification`)
		VALUES
			(null,
			uFirstName,
			uLastName,
			null,
			null,
			uEmailAddress,
			null,
			uCountryId,
			uStateId,
			uCityId,
			null,
			1,
			0,
			uToken,
			null,
			uPassword,
			null,
			null,
			now() ,
			now() ,
			uUserType,
			0,
			uDeviceToken,
			uDeviceType,
			uUDID,
			null,
			uUserId,
			uUserId,
			null,
			uQrcode,
            1);			
            
		SELECT 
			u.*
			, c.CountryName
			, s.StateName
			, ct.CityName
		FROM users u
		LEFT JOIN countries c
		ON c.CountryId = u.CountryId
		LEFT JOIN states s
		ON s.StateId = u.StateId
		LEFT JOIN cities ct
		ON ct.CityId = u.CityId
        WHERE u.UserId = last_insert_id();

		SET uMessage = "User has been created successfully.";
		SET uStatus = 1;
		SELECT uMessage, uStatus;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_UpdateProfilePicture` (IN `ouser_id` INT(11), IN `oprofile_photo` LONGTEXT)  BEGIN
	UPDATE users SET ImagePath = oprofile_photo WHERE UserId = ouser_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_UpdateUserProfile` (IN `uToken` LONGTEXT, IN `uUserId` BIGINT(20), IN `uFirstName` VARCHAR(200), IN `uLastName` VARCHAR(200), IN `uCountryId` BIGINT(20), IN `uStateId` BIGINT(20), IN `uCityId` BIGINT(20), IN `uDeviceToken` LONGTEXT, IN `uDeviceType` INT(11), IN `uUdId` LONGTEXT, INOUT `uMessage` VARCHAR(500), INOUT `uStatus` INT)  BEGIN
DECLARE UID BIGINT(20);
SELECT  UserId INTO UID FROM  users WHERE Token=uToken and IsActive=1 and UserType != 1;
	IF UID = uUserId THEN
		UPDATE users SET 
			FirstName=uFirstName ,
			LastName=uLastName ,
			CountryId=uCountryId,
			StateId=uStateId,
			CityId=uCityId,
			DeviceToken = uDeviceToken,
            DeviceType = uDeviceType,
            UdId = uUdId,
            UpdatedBy = uUserId,
            UpdatedOn = now()
		WHERE UserId = uUserId;
        
        SELECT 
			u.*
			, c.CountryName
			, s.StateName
			, ct.CityName
            , (SELECT count(*) from planpurchasehistory ph where ph.UserId = u.UserId) AS IsPaid
		FROM users u
		LEFT JOIN countries c
		ON c.CountryId = u.CountryId
		LEFT JOIN states s
		ON s.StateId = u.StateId
		LEFT JOIN cities ct
		ON ct.CityId = u.CityId
        where u.UserId = uUserId;
        
        SET uMessage = "User profile has been updated successfully.";
		SET uStatus = 1;
		SELECT uMessage, uStatus;
	ELSE
		SET uMessage = "Unauthorized User.";
		SET uStatus = 0;
		SELECT uMessage, uStatus;
	END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `API_UpdateUserToken` (IN `UserId` BIGINT(20), IN `uEmail` VARCHAR(100), IN `uToken` LONGTEXT, IN `udid` LONGTEXT, IN `device_token` LONGTEXT, IN `device_type` VARCHAR(100))  BEGIN
	UPDATE users SET Token=uToken ,
		DeviceToken=device_token,
		DeviceType=device_type,
		UdId=udid  
    WHERE EmailAddress=uEmail AND UserId=UserId;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(450) NOT NULL,
  `ConcurrencyStamp` varchar(1000) DEFAULT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(450) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(128) NOT NULL,
  `AccessFailedCount` int(11) DEFAULT NULL,
  `ConcurrencyStamp` longtext,
  `Email` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `EmailConfirmed` int(11) DEFAULT NULL,
  `LockoutEnabled` int(11) DEFAULT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8 DEFAULT NULL,
  `PasswordHash` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` int(11) DEFAULT NULL,
  `SecurityStamp` longtext,
  `TwoFactorEnabled` int(11) DEFAULT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `UserName` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `cities`
--

CREATE TABLE `cities` (
  `CityId` bigint(20) NOT NULL,
  `CityName` varchar(100) NOT NULL,
  `StateId` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `countries`
--

CREATE TABLE `countries` (
  `CountryId` bigint(20) NOT NULL,
  `CountryName` varchar(100) NOT NULL,
  `CountryCode` varchar(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `pages`
--

CREATE TABLE `pages` (
  `PageId` bigint(20) NOT NULL,
  `PageTitle` varchar(200) CHARACTER SET utf8 DEFAULT NULL,
  `PageUrl` longtext,
  `PageDescription` longtext,
  `IsActive` int(11) DEFAULT NULL,
  `CreatedBy` bigint(20) DEFAULT NULL,
  `CreatedOn` datetime DEFAULT NULL,
  `UpdatedBy` bigint(20) DEFAULT NULL,
  `UpdatedOn` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `pages`
--

INSERT INTO `pages` (`PageId`, `PageTitle`, `PageUrl`, `PageDescription`, `IsActive`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES
(4, 'About Us', '/about-us', '<h1>About Us Page</h1>', 1, 50, '2020-07-08 13:35:17', 50, '2020-07-08 13:35:17'),
(5, 'Term and condition', '/term-and-condition', '<h1>Term And Conditions</h1>', 1, 50, '2020-07-08 13:35:17', 50, '2020-07-08 13:35:17'),
(6, 'Privacy Policy', '/privacy-policy', '<h1>Privacy Policy</h1>', 1, 50, '2020-07-08 13:35:17', 50, '2020-07-08 13:35:17'),
(7, 'FAQs', '/faq', 'FAQ&#39;s page content here...edit', 1, 50, '2020-07-14 09:46:18', 50, '2020-07-14 09:49:59');

-- --------------------------------------------------------

--
-- Table structure for table `rolemanagement`
--

CREATE TABLE `rolemanagement` (
  `RoleManagementId` bigint(20) NOT NULL,
  `MenuId` varchar(500) NOT NULL,
  `RoleManagementName` varchar(200) DEFAULT NULL,
  `CreatedBy` bigint(20) DEFAULT NULL,
  `CreatedOn` datetime DEFAULT NULL,
  `UpdatedBy` bigint(20) DEFAULT NULL,
  `UpdatedOn` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `states`
--

CREATE TABLE `states` (
  `StateId` bigint(20) NOT NULL,
  `StateName` varchar(100) NOT NULL,
  `CountryId` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `userrolemanagement`
--

CREATE TABLE `userrolemanagement` (
  `UserRoleManagementId` bigint(20) NOT NULL,
  `UserId` bigint(20) NOT NULL,
  `RoleManagementId` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `UserId` bigint(20) NOT NULL,
  `FirstName` varchar(200) DEFAULT NULL,
  `LastName` varchar(200) DEFAULT NULL,
  `DateOfBirth` datetime DEFAULT NULL,
  `MobileNumber` varchar(15) CHARACTER SET utf8 DEFAULT NULL,
  `EmailAddress` varchar(200) DEFAULT NULL,
  `Address` varchar(200) DEFAULT NULL,
  `CountryId` bigint(20) DEFAULT NULL,
  `StateId` bigint(20) DEFAULT NULL,
  `CityId` bigint(20) DEFAULT NULL,
  `PostalCode` varchar(10) CHARACTER SET utf8 DEFAULT NULL,
  `IsActive` int(11) DEFAULT NULL,
  `IsDelete` int(11) DEFAULT NULL,
  `Token` longtext,
  `OTP` int(11) DEFAULT NULL,
  `Password` varchar(500) DEFAULT NULL,
  `ImagePath` varchar(1000) CHARACTER SET utf8 DEFAULT NULL,
  `Gender` int(11) DEFAULT NULL,
  `CreatedOn` datetime DEFAULT NULL,
  `UpdatedOn` datetime DEFAULT NULL,
  `UserType` int(11) DEFAULT NULL,
  `AspNetUserId` varchar(128) NOT NULL,
  `DeviceToken` longtext,
  `DeviceType` int(11) DEFAULT NULL,
  `UdId` longtext,
  `UserUniqueId` bigint(20) DEFAULT NULL,
  `CreatedBy` bigint(20) DEFAULT NULL,
  `UpdatedBy` bigint(20) DEFAULT NULL,
  `Description` varchar(1000) DEFAULT NULL,
  `Qrcode` varchar(200) DEFAULT NULL,
  `IsNotification` int(11) DEFAULT NULL,
  `Latitude` varchar(200) DEFAULT NULL,
  `Longitude` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`UserId`, `FirstName`, `LastName`, `DateOfBirth`, `MobileNumber`, `EmailAddress`, `Address`, `CountryId`, `StateId`, `CityId`, `PostalCode`, `IsActive`, `IsDelete`, `Token`, `OTP`, `Password`, `ImagePath`, `Gender`, `CreatedOn`, `UpdatedOn`, `UserType`, `AspNetUserId`, `DeviceToken`, `DeviceType`, `UdId`, `UserUniqueId`, `CreatedBy`, `UpdatedBy`, `Description`, `Qrcode`, `IsNotification`, `Latitude`, `Longitude`) VALUES
(1, 'Admin', 'User', NULL, '', 'admin@gmail.com', NULL, 0, 0, 0, '', 1, 0, '', NULL, 'fc0iUkg331qk3V8HY6MWvQ==', '', 1, '2020-07-18 11:18:21', '2020-07-18 11:18:21', 1, '0', '', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2, 'staff', 'User', NULL, '', 'staff@gmail.com', NULL, 0, 0, 0, '', 1, 0, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzdGFmZkBnbWFpbC5jb20iLCJqdGkiOiJjYjRhNGFmZS1kNDg2LTQwNDEtOWVhOC1jY2ExYTRmMWNjYTUiLCJleHAiOjE1OTQ5ODU5OTUsImlzcyI6Imh0dHA6Ly8zLjE3LjE1Ny4xNzgiLCJhdWQiOiJodHRwOi8vMy4xNy4xNTcuMTc4In0.andMIXu07uODJjVWWpevoR2in9WOon4DgnOogdFGUOI', 0, 'fc0iUkg331qk3V8HY6MWvQ==', '', 1, '2020-07-08 12:30:39', '2020-07-08 12:30:39', 2, '0', 'fkxnA4TCROuQD6zu3oAdNT:APA91bFuX7h69uwh5Y1uc3JhkrcMaEdn_tD9E5ScU4D0cnxzvVxgUdVOpLjFHYLqbgDIvTy1ZVJoG3IlUsjt9zFXMyF4XvfEisTrMQ3tRo_EgmErpf0FVz5fZ-hV3ZoRocweZF-RUvJy', 2, 'd03933ae7455090d', NULL, 0, 0, NULL, NULL, 1, '19.0760', '72.8777'),
(3, 'Client', 'User', NULL, '', 'user@gmail.com', NULL, 3, 112, 6090, '', 1, 0, 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGdtYWlsLmNvbSIsImp0aSI6IjA2MWUzNTc4LTYwMWYtNDIzYi05MDUyLWEwY2ZhNGRhNDFiMSIsImV4cCI6MTU5NDczODc2OSwiaXNzIjoiaHR0cDovLzMuMTcuMTU3LjE3OCIsImF1ZCI6Imh0dHA6Ly8zLjE3LjE1Ny4xNzgifQ.1672VIt9DlR8LBOze5nOb1kgWLC-x2R_gRucIwPYvoU', 0, 'fc0iUkg331qk3V8HY6MWvQ==', 'test.png', 1, '2020-07-08 12:30:39', '2020-07-13 09:21:41', 3, '0', 'd5bL7rUHRjW2UHS-VNcAqV:APA91bHtnAz3CGcYKGHagJsDBErWP6dJ_VgGQNmCiVbgm0-uSFetQGzdysyz401dw4hDISIWKyNhs2O7Tqb9ZrGTV3SxDv4J5L6V97ErmuQp1cL-ufCCeQguVgvmNWEc5PL9DeL09cn3', 2, 'd03933ae7455090d', NULL, 0, 52, NULL, 'QR-9A331063-329F-4E08-AFC2-D12EF4C88E98', 1, '25.5941', '85.1376');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Id` (`Id`),
  ADD KEY `UserId` (`UserId`);

--
-- Indexes for table `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `countries`
--
ALTER TABLE `countries`
  ADD PRIMARY KEY (`CountryId`);

--
-- Indexes for table `pages`
--
ALTER TABLE `pages`
  ADD PRIMARY KEY (`PageId`);

--
-- Indexes for table `rolemanagement`
--
ALTER TABLE `rolemanagement`
  ADD PRIMARY KEY (`RoleManagementId`);

--
-- Indexes for table `states`
--
ALTER TABLE `states`
  ADD PRIMARY KEY (`StateId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`UserId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `countries`
--
ALTER TABLE `countries`
  MODIFY `CountryId` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `pages`
--
ALTER TABLE `pages`
  MODIFY `PageId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `rolemanagement`
--
ALTER TABLE `rolemanagement`
  MODIFY `RoleManagementId` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `states`
--
ALTER TABLE `states`
  MODIFY `StateId` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `UserId` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
