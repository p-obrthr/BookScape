import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../styles/styles.dart';
import '../widgets/switchSign.dart';
import '../widgets/credentialBox.dart';
import '../widgets/mainBtn.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class LoginScreen extends StatefulWidget {
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool? isRememberMe = false;

  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();

  Future<void> login() async {
    final email = emailController.text;
    final password = passwordController.text;

    // Hier könntest du den API-Aufruf durchführen
    final url = Uri.parse('https://your-api-endpoint.com/api/login');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'email': email, 'password': password}),
    );

    if (response.statusCode == 200) {
      Navigator.pushReplacementNamed(context, '/page');
    } else {
      print('Login fehlgeschlagen');
    }
  }


  Widget buildForgotPasswordBtn() {
    return Container(
      alignment: Alignment.centerRight,
      child: TextButton(
        onPressed: () => print("Forgot Password pressed"),
        style: TextButton.styleFrom(
          padding: const EdgeInsets.only(right: 0),
        ),
        child: const Text(
          'Forgot Password?',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
          ),
        ),
      ),
    );
  }

  Widget buildRememberBtn() {
    return Container(
        height: 20,
        child: Row(
          children: <Widget>[
            Theme(
              data: ThemeData(unselectedWidgetColor: Colors.white),
              child: Checkbox(
                value: isRememberMe,
                checkColor: Colors.green,
                activeColor: Colors.white,
                onChanged: (value) {
                  setState(() {
                    isRememberMe = value;
                  });
                },
              ),
            ),
            const Text(
                'Remember me',
                style: TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.bold
                )
            )
          ],
        )
    );
  }

  Widget buildSwitch() {
    return const SwitchSign(
      mainText: "Don't have an account? ",
      actionText: "Sign up",
      route: '/register',
    );
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AnnotatedRegion<SystemUiOverlayStyle>(
        value: SystemUiOverlayStyle.light,
        child: GestureDetector(
          child: Stack(
            children: <Widget>[
              Container(
                height: double.infinity,
                width: double.infinity,
                decoration: BoxDecoration(
                  gradient: AppStyles.backgroundGradient,
                ),
                child: SingleChildScrollView(
                  physics: AlwaysScrollableScrollPhysics(),
                  padding: EdgeInsets.symmetric(horizontal: 25, vertical: 120),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      Text('LOG IN', style: AppStyles.titleStyle),
                      SizedBox(height: 50),
                      // buildEmail(),
                      CredentialBox(
                        text: 'Email',
                        icon: Icons.email,
                        controller: emailController
                      ),
                      SizedBox(height: 20),
                      CredentialBox(
                        text: 'Password',
                        icon: Icons.lock,
                        controller: passwordController,
                      ),
                      buildForgotPasswordBtn(),
                      buildRememberBtn(),
                      MainBtn(
                        text: 'LOG IN',
                        onPressed: login,
                      ),
                      buildSwitch()
                    ],
                  ),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
