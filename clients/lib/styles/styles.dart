import 'package:flutter/material.dart';
import 'colors.dart';

class AppStyles {

  static const TextStyle titleStyle = TextStyle(
    color: Colors.white,
    fontSize: 40,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle labelStyle = TextStyle(
    color: Colors.white,
    fontSize: 16,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle hintStyle = TextStyle(
    color: Colors.black38,
  );

  static const TextStyle buttonTextStyle = TextStyle(
    color: AppColors.mainColor,
    fontSize: 18,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle linkTextStyle = TextStyle(
    color: Colors.white,
    fontSize: 18,
    fontWeight: FontWeight.bold,
  );

  static const TextStyle linkRegularTextStyle = TextStyle(
    color: Colors.white,
    fontSize: 18,
    fontWeight: FontWeight.w500,
  );

  static BoxDecoration inputBoxDecoration = BoxDecoration(
    color: Colors.white,
    borderRadius: BorderRadius.circular(10),
    boxShadow: [
      BoxShadow(
        color: Colors.black26,
        blurRadius: 6,
        offset: Offset(0, 2),
      ),
    ],
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
