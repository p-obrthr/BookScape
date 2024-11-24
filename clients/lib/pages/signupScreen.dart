import 'package:clients/widgets/credentialBox.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../widgets/switchSign.dart';
import '../styles/styles.dart';
import '../widgets/mainBtn.dart';
import 'package:http/http.dart' as http;
import 'dart:convert';

class SignUpScreen extends StatefulWidget {
  @override
  _SignUpScreenState createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {

  final TextEditingController emailController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  final TextEditingController confirmPasswordController = TextEditingController();
  final TextEditingController nameController = TextEditingController();

  Future<void> signUp() async {
    final email = emailController.text;
    final password = passwordController.text;
    final confirmPassword = confirmPasswordController.text;
    final name = nameController.text;

    if (password != confirmPassword) {
      print("Passwords do not match");
      return;
    }

    final url = Uri.parse('http://localhost:5070/register');
    final response = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({
        'name': name,
        'email': email,
        'password': password,
        'confirmPassword': confirmPassword,
      }),
    );

    if (response.statusCode == 201) {
      Navigator.pushReplacementNamed(context, '/login');
    } else {
      print('Registration failed');
    }
  }

  Widget buildLoginRedirect() {
    return SwitchSign(
      mainText: "Already have an account? ",
      actionText: "Log in",
      route: '/login',
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
                  physics: const AlwaysScrollableScrollPhysics(),
                  padding: const EdgeInsets.symmetric(horizontal: 25, vertical: 120),
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: <Widget>[
                      const Text(
                        'SIGN UP',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 40,
                            fontWeight: FontWeight.bold
                        ),
                      ),
                      const SizedBox(height: 50),
                      CredentialBox(
                          text: 'Name',
                          icon: Icons.source,
                          controller: nameController
                      ),
                      const SizedBox(height: 20),
                      CredentialBox(
                          text: 'Email',
                          icon: Icons.email,
                          controller: emailController,
                      ),
                      const SizedBox(height: 20),
                      CredentialBox(
                          text: 'Password',
                          icon: Icons.lock,
                          controller: passwordController,
                      ),
                      const SizedBox(height: 20),
                      CredentialBox(
                          text: 'Confirm Password',
                          icon: Icons.lock,
                          controller: confirmPasswordController,
                      ),
                      const SizedBox(height: 20),
                      MainBtn(
                        text: 'SIGN UP',
                        onPressed: signUp
                      ),
                      buildLoginRedirect(),
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
