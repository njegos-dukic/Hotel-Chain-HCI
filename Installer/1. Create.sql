-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema LanacHotela
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `LanacHotela` ;

-- -----------------------------------------------------
-- Schema LanacHotela
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `LanacHotela` DEFAULT CHARACTER SET utf8 ;
USE `LanacHotela` ;

-- -----------------------------------------------------
-- Table `LanacHotela`.`HOTEL`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`HOTEL` (
  `hotelID` INT NOT NULL AUTO_INCREMENT,
  `ime` VARCHAR(50) NOT NULL,
  `brojZvjezdica` INT NOT NULL,
  `ulica` VARCHAR(50) NOT NULL,
  `broj` VARCHAR(50) NOT NULL,
  `grad` VARCHAR(50) NOT NULL,
  `postanskiBroj` INT NOT NULL,
  `drzava` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`hotelID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`GOST`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`GOST` (
  `gostID` INT NOT NULL AUTO_INCREMENT,
  `JMBG` VARCHAR(13) NOT NULL,
  `ime` VARCHAR(50) NOT NULL,
  `prezime` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`gostID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`KONTAKT`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`KONTAKT` (
  `kontaktID` INT NOT NULL AUTO_INCREMENT,
  `tip` VARCHAR(10) NOT NULL,
  `info` VARCHAR(50) NOT NULL,
  `hotelID` INT NULL,
  `gostID` INT NULL,
  PRIMARY KEY (`kontaktID`),
  INDEX `fk_KONTAKT_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  INDEX `fk_KONTAKT_GOST1_idx` (`gostID` ASC) VISIBLE,
  CONSTRAINT `fk_KONTAKT_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_KONTAKT_GOST1`
    FOREIGN KEY (`gostID`)
    REFERENCES `LanacHotela`.`GOST` (`gostID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`SOBA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`SOBA` (
  `sobaID` INT NOT NULL AUTO_INCREMENT,
  `brojSprata` INT NOT NULL,
  `brojSobe` INT NOT NULL,
  `brojKreveta` INT NOT NULL,
  `imaTV` TINYINT NOT NULL,
  `imaKlimu` TINYINT NOT NULL,
  `cijenaNocenja` DECIMAL(5,2) NOT NULL,
  `hotelID` INT NOT NULL,
  PRIMARY KEY (`sobaID`),
  INDEX `fk_SOBA_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  CONSTRAINT `fk_SOBA_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`ARANZMAN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`ARANZMAN` (
  `aranzmanID` INT NOT NULL AUTO_INCREMENT,
  `pocetak` DATETIME NOT NULL,
  `kraj` DATETIME NOT NULL,
  `jeOtkazan` TINYINT NOT NULL,
  `jeZavrsen` TINYINT NOT NULL,
  `hotelID` INT NOT NULL,
  `gostID` INT NOT NULL,
  `sobaID` INT NOT NULL,
  PRIMARY KEY (`aranzmanID`),
  INDEX `fk_ARANŽMAN_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  INDEX `fk_ARANŽMAN_GOST1_idx` (`gostID` ASC) VISIBLE,
  INDEX `fk_ARANŽMAN_SOBA1_idx` (`sobaID` ASC) VISIBLE,
  CONSTRAINT `fk_ARANŽMAN_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ARANŽMAN_GOST1`
    FOREIGN KEY (`gostID`)
    REFERENCES `LanacHotela`.`GOST` (`gostID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ARANŽMAN_SOBA1`
    FOREIGN KEY (`sobaID`)
    REFERENCES `LanacHotela`.`SOBA` (`sobaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`ZAPOSLENI`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`ZAPOSLENI` (
  `zaposleniID` INT NOT NULL AUTO_INCREMENT,
  `korisnickoIme` VARCHAR(50) NOT NULL,
  `lozinka` VARCHAR(512) NOT NULL,
  `jeMenadzer` TINYINT NOT NULL,
  `hotelID` INT NOT NULL,
  PRIMARY KEY (`zaposleniID`),
  INDEX `fk_HOTEL_has_ZAPOSLENI_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  CONSTRAINT `fk_HOTEL_has_ZAPOSLENI_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RACUN_ZA_ARANZMAN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RACUN_ZA_ARANZMAN` (
  `racunAranzmanID` INT NOT NULL AUTO_INCREMENT,
  `aranzmanID` INT NOT NULL,
  `cijena` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`racunAranzmanID`),
  INDEX `fk_RACUN_ZA_ARANZMAN_ARANŽMAN1_idx` (`aranzmanID` ASC) VISIBLE,
  CONSTRAINT `fk_RACUN_ZA_ARANZMAN_ARANŽMAN1`
    FOREIGN KEY (`aranzmanID`)
    REFERENCES `LanacHotela`.`ARANZMAN` (`aranzmanID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
