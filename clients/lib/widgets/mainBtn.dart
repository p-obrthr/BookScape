import 'package:flutter/material.dart';
import '../styles/styles.dart';

class MainBtn extends StatelessWidget {
  final String text;
  final VoidCallback onPressed;

  const MainBtn({
    Key? key,
    required this.text,
    required this.onPressed,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 25),
      width: double.infinity,
      child: ElevatedButton(
        onPressed: onPressed,
        style: AppStyles.elevatedButtonStyle,
        child: Text(text, style: AppStyles.buttonTextStyle),
      ),
    );
  }
}