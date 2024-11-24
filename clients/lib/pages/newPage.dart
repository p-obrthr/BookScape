import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../styles/styles.dart';

class NewPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Page'),
      ),
      body: Center(
        child: Text('Welcome to the Page!'),
      ),
    );
  }
}