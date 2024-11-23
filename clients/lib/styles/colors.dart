import 'package:flutter/material.dart';

class AppColors {
  static const Color mainColor = Color(0xFF2E8C68);

  static Color withOpacity(Color color, double opacity) {
    return color.withOpacity(opacity);
  }

  static Color get gradientLight => withOpacity(mainColor, 0.2); // 20%
  static Color get gradientMediumLight => withOpacity(mainColor, 0.4); // 40%
  static Color get gradientMediumDark => withOpacity(mainColor, 0.6); // 60%
  static Color get gradientDark => mainColor.withOpacity(0.9); // 90%

  static Color lighten(Color color, double amount) {
    final hsl = HSLColor.fromColor(color);
    final hslLight = hsl.withLightness((hsl.lightness + amount).clamp(0.0, 1.0));
    return hslLight.toColor();
  }

  static Color darken(Color color, double amount) {
    final hsl = HSLColor.fromColor(color);
    final hslDark = hsl.withLightness((hsl.lightness - amount).clamp(0.0, 1.0));
    return hslDark.toColor();
  }

  static Color get lighter => lighten(mainColor, 0.2);
  static Color get darker => darken(mainColor, 0.2);
}
