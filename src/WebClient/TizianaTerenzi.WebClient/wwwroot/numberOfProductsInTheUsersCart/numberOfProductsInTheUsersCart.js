"use strict";

var jwtToken = document.getElementById("jwtToken").value;
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5011/numberOfProductsInTheUsersCartHub", { accessTokenFactory: () => jwtToken }).build();

connection.start().then(function () {
    connection.invoke("GetNumberOfProductsInTheUsersCart").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("NumberOfProductsInTheUsersCart", function (numberOfProductsInTheUsersCart) {
    let userCartBadgeCount = document.getElementById(`cart-badge-count`);

    if (userCartBadgeCount) {
        userCartBadgeCount.innerText = numberOfProductsInTheUsersCart; // span
    }
});

connection.on("AddProductInTheUsersCart", function () {
    connection.invoke("GetNumberOfProductsInTheUsersCart").catch(function (err) {
        return console.error(err.toString());
    });
});