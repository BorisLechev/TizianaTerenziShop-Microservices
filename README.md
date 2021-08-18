# TizianaTerenziShop-MVC
## Create initial administrator
1. Make an account in the application.
2. Connect to the SQL database using a tool such as SSMS.
3. From the *AspNetRoles* table, note the ID of the Administrator role.
4. From the *AspNetUsers* table, note the ID of your newly created user account.
5. Create a new row in the *AspNetUserRoles* table and insert the user and role ID.

## Set up GitHub login functionality
1. Create a [GitHub OAuth application](https://github.com/settings/developers).
2. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the GitHub Client ID and Client Secret.

Example:
```
"GitHub": {
  "ClientId": "[[GitHub Client Id]]",
  "ClientSecret": "[[GitHub Client Secret]]"
}
```

## Set up Facebook login functionality
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Facebook Client ID and Client Secret.

Example:
```
"FacebookSettings": {
  "AppId": "[[Facebook App Id]]",
  "AppSecret": "[[Facebook App Secret]]"
}
```

## Set up Google login functionality
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Google Client ID and Client Secret.

Example:
```
"Google": {
  "ClientId": "[[Google Client Id]]",
  "ClientSecret": "[[Google Client Secret]]"
}
```

## Set up Google ReCaptcha v3
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Site key and Secret key.

Example:
```
"GoogleReCaptcha": {
  "Key": "[[GoogleReCaptcha Key]]",
  "Secret": "[[GoogleReCaptcha Secret Key]]"
},
```

## Set up Cloudinary
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Cloud name, Api Key and Api Secret key.

Example:
```
"Cloudinary": {
  "CloudName": "[[Cloudinary Cloud Name]]",
  "ApiKey": "[[Cloudinary Api Key]]",
  "ApiSecret": "[[Cloudinary Api Secret]]"
},
```

## Set up SendGrid
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Api Key.

Example:
```
"SendGrid": {
  "ApiKey": "[[SendGrid Api Key]]"
},
```

## Set up IpInfo
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Api Key.

Example:
```
"IpInfo": {
  "ApiKey": "[[IpInfo Api Key]]"
},
```

## Set up Stripe
1. In the *TizianaTerenzi.Web/appsettings.json* configuration file insert the Publishable Key and Secret Key.

Example:
```
"Stripe": {
    "PublishableKey": "[[Stripe Publishable Key]]",
    "SecretKey": "[[Stripe Secret Key]]"
  },
```

## 🛠 Built with:
  * C#
  * Repository pattern
  * [ASP.NET Core 5.0 MVC](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0)
  * [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr) - deliver up-to-date information without hitting a refresh button
  * [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/)
  * [Entity Framework Plus](https://entityframework-plus.net/)
  * [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  * [Automapper](https://automapper.org/)
  * [Google ReCaptcha v3](https://www.google.com/recaptcha/about/)
  * [Cloudinary](https://cloudinary.com/)
  * [SendGrid](https://sendgrid.com/) - for mailing
  * jQuery
  * jQuery Unobtrusive Validation library
  * AJAX
  * HTML
  * CSS
  * [Bootstrap 4](https://getbootstrap.com/docs/4.0/getting-started/introduction/)
  * [Bootstrap-Select](https://developer.snapappointments.com/bootstrap-select/) - for select dropdowns
  * [Semantic-UI](https://semantic-ui.com/) - for the multiple select dropdowns in Admin Dashboard
  * [Swiper](https://swiperjs.com/) - Modern mobile touch slider
  * [SB Admin 2](https://startbootstrap.com/theme/sb-admin-2) - Bootstrap 4 based admin theme for Admin dashboard
  * [AOS](https://michalsnik.github.io/aos/) && [Animate.css](https://animate.style/) - Animate On Scroll library
  * [FontAwesome](https://fontawesome.com/)
  * [Moment.js](https://momentjs.com/) - to parse, validate and display dates and times in JavaScript
  * [TinyMCE](https://www.tiny.cloud/) - text editor
  * [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer) - library for cleaning HTML fragments and documents from constructs that can lead to XSS attacks.
  * [AngleSharp](https://anglesharp.github.io/) - scraping emojis for the chat from unicode.org/emojis.
  * [DataTables](https://datatables.net/) - tool that adds pagination, instant search, multi-column ordering etc. to any HTML table
  * [OpenStreetMap](https://www.openstreetmap.org/) && [Leaflet](https://leafletjs.com/)
  * [Chart.js](https://www.chartjs.org/) - create interactive charts for web and mobile apps
  * [Chartjs-plugin-colorschemas](https://nagix.github.io/chartjs-plugin-colorschemes/) - pick the perfect colour combination for pie/doughnut charts
  * [IpInfo](https://ipinfo.io/) - API for getting access to reliable IP data
  * [Stripe](https://stripe.com/) - online payment processing platform
  * [xUnit](https://xunit.net/) - unit testing tool for .NET Framework
  * [Moq](https://github.com/Moq/moq4/wiki/Quickstart) && [MockQueryable.Moq](https://github.com/romantitov/MockQueryable)
  * [MyTested.AspNetCore.Mvc](https://github.com/ivaylokenov/MyTested.AspNetCore.Mvc/tree/development) - strongly-typed unit testing library providing an easy fluent interface to test the ASP.NET Core framework, perfectly suitable for both MVC and API scenarios.
  * [PhantomJS v2.1.1](https://phantomjs.org/) - to generate PDF reports
  * English language
  * Bulgarian language
  * ASP .NET Core Template by: Nikolay Kostov
  
## Author

- [Boris Lechev](https://github.com/BorisLechev)