import 'package:flutter/material.dart';
import 'colors.dart';

class AppStyles {

  static const TextStyle titleStyle = TextStyle(
    color: Colors.white,
    fontSize: 40,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle buttonTextStyle = TextStyle(
    color: AppColors.mainColor,
    fontSize: 18,
    fontWeight: FontWeight.bold,
  );

  static ButtonStyle elevatedButtonStyle = ElevatedButton.styleFrom(
    elevation: 5,
    padding: EdgeInsets.all(15),
    shape: RoundedRectangleBorder(
      borderRadius: BorderRadius.circular(15),
    ),
    backgroundColor: Colors.white,
  );

  static LinearGradient backgroundGradient = LinearGradient(
    begin: Alignment.topCenter,
    end: Alignment.bottomCenter,
    colors: [
      AppColors.gradientLight,
      AppColors.gradientMediumLight,
      AppColors.gradientMediumDark,
      AppColors.gradientDark
    ],
  );
}
