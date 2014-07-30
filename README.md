SharpExpress
==============================

Simple express-inspired mini web-server in C#.

Probably not production ready :P

Features
------------------------------
* Routes
* Template Rendering (Handlebars)
* Static files
* File uploads

Example
------------------------------
```
var app = new WebApplication();

app.Get("/", (req, res) =>
{
	res.Send("Hello, World!");
}

app.Listen(80);
```

Links
------------------------------
* GitHub: https://github.com/exectails/SharpExpress
