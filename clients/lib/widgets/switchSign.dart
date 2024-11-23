import 'package:flutter/material.dart';

class SwitchSign extends StatelessWidget {
  final String mainText;
  final String actionText;
  final String route;

  const SwitchSign({
    Key? key,
    required this.mainText,
    required this.actionText,
    required this.route,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          mainText,
          style: const TextStyle(
            color: Colors.white,
            fontSize: 18,
            fontWeight: FontWeight.w500,
          ),
        ),
        TextButton(
          onPressed: () => Navigator.pushNamed(context, route),
          style: TextButton.styleFrom(
            padding: EdgeInsets.zero,
          ),
          child: Text(
            actionText,
            style: const TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 18,
            ),
          ),
        ),
      ],
    );
  }
}
