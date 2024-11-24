class LoginResponse {
  final bool flag;
  final String? message;
  final String? token;

  LoginResponse({
    required this.flag,
    this.message,
    this.token,
  });

  factory LoginResponse.fromJson(Map<String, dynamic> json) {
    return LoginResponse(
      flag: json['flag'],
      message: json['message'],
      token: json['token'],
    );
  }
}
