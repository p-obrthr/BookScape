import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'styles.dart';
import 'colors.dart';
import 'widgets/switchSign.dart';
import 'widgets/credentialBox.dart';

class LoginScreen extends StatefulWidget {
  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  bool? isRememberMe = false;

  Widget buildEmail() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        const Text('Email', style: AppStyles.labelStyle),
        const SizedBox(height: 10),
        Container(
          alignment: Alignment.centerLeft,
          decoration: AppStyles.inputBoxDecoration,
          height: 60,
          child: const TextField(
            keyboardType: TextInputType.emailAddress,
            style: TextStyle(color: Colors.black87),
            decoration: InputDecoration(
              border: InputBorder.none,
              contentPadding: EdgeInsets.only(top: 14),
              prefixIcon: Icon(Icons.email, color: AppColors.mainColor),
              hintText: 'Email',
              hintStyle: AppStyles.hintStyle,
            ),
          ),
        ),
      ],
    );
  }

  Widget buildPassword() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: <Widget>[
        const Text('Password', style: AppStyles.labelStyle),
        const SizedBox(height: 10),
        Container(
          alignment: Alignment.centerLeft,
          decoration: AppStyles.inputBoxDecoration,
          height: 60,
          child: const TextField(
            obscureText: true,
            style: TextStyle(color: Colors.black87),
            decoration: InputDecoration(
              border: InputBorder.none,
              contentPadding: EdgeInsets.only(top: 14),
              prefixIcon: Icon(Icons.lock, color: Color(0xff5ac18e)),
              hintText: 'Password',
              hintStyle: AppStyles.hintStyle,
            ),
          ),
        ),
      ],
    );
  }

  Widget buildLoginBtn() {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 25),
      width: double.infinity,
      child: ElevatedButton(
        onPressed: () => print('Login pressed'),
        style: AppStyles.elevatedButtonStyle, // Verwende Styles
        child: const Text('LOGIN', style: AppStyles.buttonTextStyle),
      ),
    );
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

  Widget buildSignUpBtn() {
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
                      ),
                      SizedBox(height: 20),
                      CredentialBox(
                        text: 'Password',
                        icon: Icons.lock,
                      ),
                      buildForgotPasswordBtn(),
                      buildRememberBtn(),
                      buildLoginBtn(),
                      buildSignUpBtn()
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
