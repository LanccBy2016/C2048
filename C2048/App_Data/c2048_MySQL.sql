

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `UserID` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Pwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`UserID`) USING BTREE
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for userscorelog
-- ----------------------------
DROP TABLE IF EXISTS `userscorelog`;
CREATE TABLE `userscorelog`  (
  `UserID` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Score` int(11) NULL DEFAULT NULL,
  `CreateTime` datetime NULL DEFAULT NULL
) ENGINE = MyISAM CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
