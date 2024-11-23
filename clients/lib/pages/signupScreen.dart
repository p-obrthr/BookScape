import 'package:clients/widgets/credentialBox.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import '../widgets/switchSign.dart';
import '../styles/styles.dart';
import '../widgets/mainBtn.dart';

class SignUpScreen extends StatefulWidget {
  @override
  _SignUpScreenState createState() => _SignUpScreenState();
}

class _SignUpScreenState extends State<SignUpScreen> {

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
                          text: 'Email',
                          icon: Icons.email
                      ),
                      const SizedBox(height: 20),
                      CredentialBox(
                          text: 'Password',
                          icon: Icons.lock
                      ),
                      const SizedBox(height: 20),
                      CredentialBox(
                          text: 'Confirm Password',
                          icon: Icons.lock
                      ),
                      const SizedBox(height: 20),
                      MainBtn(
                        text: 'SIGN UP'
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
