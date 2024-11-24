import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:http/http.dart' as http;

class NewPage extends StatefulWidget {
  @override
  _NewPageState createState() => _NewPageState();
}

class _NewPageState extends State<NewPage> {
  List<dynamic> books = [];
  TextEditingController _titleController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _loadBooks();
  }

  Future<String?> _getAuthToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('auth_token');
  }

  Future<void> _loadBooks() async {
    String? token = await _getAuthToken();
    if (token == null) {
      print('token not found');
      return;
    }

    final response = await http.get(
      Uri.parse('http://localhost:4000/api/books'),
      headers: {
        'Authorization': 'Bearer $token',
      },
    );

    if (response.statusCode == 200) {
      setState(() {
        books = jsonDecode(response.body);
      });
    } else {
      print('error loading books: ${response.statusCode}');
    }
  }

  Future<void> _createBook(String title) async {
    String? token = await _getAuthToken();
    if (token == null) {
      print('Token fehlt!');
      return;
    }

    final response = await http.post(
      Uri.parse('http://localhost:4000/api/books'),
      headers: {
        'Authorization': 'Bearer $token',
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'title': title,
      }),
    );

    if (response.statusCode == 201) {
      print('book created successfully');
      _loadBooks();
    } else {
      print('error while creating bok: ${response.statusCode}');
    }
  }

  void _showCreateBookDialog() {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text('create new book'),
          content: TextField(
            controller: _titleController,
            decoration: InputDecoration(labelText: 'title'),
          ),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
              },
              child: Text('cancel'),
            ),
            TextButton(
              onPressed: () {
                String title = _titleController.text;
                if (title.isNotEmpty) {
                  _createBook(title);
                  _titleController.clear();
                  Navigator.of(context).pop();
                }
              },
              child: Text('create'),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Books'),
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: _showCreateBookDialog,
          ),
        ],
      ),
      body: books.isEmpty
          ? Center(child: CircularProgressIndicator())
          : ListView.builder(
        itemCount: books.length,
        itemBuilder: (context, index) {
          return ListTile(
            title: Text(books[index]['title']),
          );
        },
      ),
    );
  }
}
