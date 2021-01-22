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

## Build with:
  * C#
  * ASP.NET Core MVC
  * Entity Framework Core
  * Automapper
  * Cloudinary
  * SendGrid
  * jQuery
  * AJAX
  * HTML
  * CSS
  * Bootstrap
  * Moment.js
  * TinyMCE
  * DataTables
  * OpenStreetMap && Leaflet
  * Highcharts
  * IpInfo
  * English language
  * Bulgarian language