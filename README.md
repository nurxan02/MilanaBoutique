# MilanaBoutique
<a href="http://nurxan02-001-site1.atempurl.com/"><img src="https://i.ibb.co/YdjHrMX/84b5a87f-3d5d-41f0-bc36-99fa5ff85883logo.png" height="80"></a>

#### :handbag: A simple ASP.Net MVC Project for Purchases and Products

## Deploy

<a href="https://smarterasp.net/"><img src="https://www.smarterasp.net/images/logo1.png" height="32"></a>

## Features

<b>Products Features</b>

| Feature  |  Coded?       | Description  |
|----------|:-------------:|:-------------|
| Add a Product | &#10004; | Ability of Add a Product on the System |
| List Products | &#10004; | Ability of List Products |
| Edit a Product | &#10004; | Ability of Edit a Product |
| Delete a Product | &#10004; | Ability of Delete a Product |
| Stock | &#10004; | Ability of Update the Stock |
| Stock History | &#10004; | Ability to see the Stock History |

<b>Purchase Features</b>

| Feature  |  Coded?       | Description  |
|----------|:-------------:|:-------------|
| Create a Cart | &#10004; | Ability of Create a new Cart |
| See Cart | &#10004; | Ability to see the Cart and it items |
| Remove a Cart | &#10004; | Ability of Remove a Cart |
| Add Item | &#10004; | Ability of add a new Item on the Cart |
| Remove a Item | &#10004; | Ability of Remove a Item from the Cart |
| Checkout | &#10004; | Ability to Checkout |


<b>Other Features</b>

| Feature  |  Coded?       | Description  |
|----------|:-------------:|:-------------|
| Users Registration | &#10004; | User register, login, edit all user infos |
| Status Track | &#10004; | User track order status |
| Wish List | &#10004; | Adding to wishlist |
| Admin Panel | &#10004; | Good and easy administration |
| Personalization | &#10004; | Personalize your eCommerce app features easily|

## Documentation

**Milana Boutique** store i deployed web eCommerce app. At that time all configs is same as Publishing features. ``appsettings.json`` includes all hosting connection string datas. For example ``Server name, ID and Password``. At the same time ``appsettigs.json`` has local **SA** server connection string. You can unComment Hosting Connection String and use this local server's connection string.

## Note.: Remember importing each SQL files, if using MSSQL for Production.

* **Username:** SA
* **Password:** query
* **Database:** Milnana
* **Port:** 44388
* **Server:** ``{your local server name}``

![](https://user-images.githubusercontent.com/90649844/217049546-433ffde2-7c90-4ec0-9814-8f07ac6f057d.png)

You can change all for your using.

### **Please read here!!!** 

```
//string prodlink = $"https://localhost:44388/{prodVM.Product.Gender.Name}/Details/" + $"{productSizeColor.Id}";
string prodlink = $"http://nurxan02-001-site1.atempurl.com/{prodVM.Product.Gender.Name}/Details/" + $"{productSizeColor.Id}";
```
You must change this ``productlink`` in the ``./Areas/Admin/Controllers/ProductController.cs`` on the line of 189-191. 
This link provide all members, **deal of the day** with email. This feature only using `` localhost:44388 ``.

![](https://user-images.githubusercontent.com/90649844/217045572-1f045301-07f3-4401-854e-2d425f1955bd.png)

## Additional 
`` MilanaBoutique/responsive-viewer-chrome-extension.json `` You can use this extension for see all devices adaptations and responsive design of this project. 
Extension link: <a href="https://chrome.google.com/webstore/detail/responsive-viewer/inmopeiepgfljkpkidclfgbgbmfcennb">Click for Responsive Viewer Chrome extension</a>

## Credits

If you want to contact with me: [**Click Here**](https://bio.link/nurxanmasimzade/)
