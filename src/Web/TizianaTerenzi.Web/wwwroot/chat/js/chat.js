"use strict";

var receiver = document.getElementById("receiver").textContent;
var sender = document.getElementById("sender").textContent;

window.scrollTo(0, document.body.scrollHeight);

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    let receiver = document.getElementById("receiver").textContent; //<h3>
    let sender = document.getElementById("sender").textContent; //<h3>
    let groupId = document.getElementById("groupId").value; //<input>

    connection.invoke("AddToGroup", groupId, receiver, sender).catch(function (err) {
        return console.error(err.toString());
    });

    connection.invoke("GetUserNotificationsCount", sender).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("VisualizeUserType", function () {
    let typeMessageElement = document.getElementById("typeMessage");

    if (!typeMessageElement) {
        document.getElementById("messagesList").innerHTML += `
            <li id="typeMessage" class="chat-other">
                <div>
                    <span class="jumping-dots">
                        <span class="jumping-dot-1"></span>
                        <span class="jumping-dot-2"></span>
                        <span class="jumping-dot-3"></span>
                    </span>
                </div>
            </li>`;
    }

    window.scrollTo({
        top: document.body.scrollHeight,
        behavior: 'smooth'
    });
});

connection.on("VisualizeUserStopType", function () {
    let typeMessageElement = document.getElementById("typeMessage");

    if (typeMessageElement) {
        document.getElementById("messagesList").removeChild(typeMessageElement);
    }
});

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("SendMessage", function (message) {
    let li = document.createElement("li");

    li.classList.add(`chat-${(message.authorUserName === sender) ? "user" : "other"}`);

    li.innerHTML = `<small class="text-muted px-2">
                        ${message.authorUserName},
                        ${moment().utc(message.createdOn).format('lll')}
                    </small>
                    <div>${message.content}</div>`;

    document.getElementById("messagesList").appendChild(li);

    window.scrollTo({
        top: document.body.scrollHeight,
        behavior: 'smooth'
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let group = document.getElementById("groupId").value;
    let message = document.getElementById("messageInput").value;

    if (message) {
        if (message.length <= 500) {
            connection.invoke("SendMessage", sender, receiver, message, group).catch(function (err) {
                return console.error(err.toString());
            });

            document.getElementById("messageInput").value = "";
        } else {
            let li = document.createElement("li");

            li.classList.add(`chat-user`);

            li.innerHTML = `<div class="bg-danger text-white">Message should be no more than 500 characters long.</div>`;

            document.getElementById("messagesList").appendChild(li);

            window.scrollTo({
                top: document.body.scrollHeight,
                behavior: 'smooth'
            });
        }
    }

    event.preventDefault();
});

function userType(receiverUsername) {
    connection.invoke("UserType", receiverUsername).catch(function (err) {
        return console.error(err.toString());
    });
}

function userStopType(receiverUsername) {
    connection.invoke("UserStopType", receiverUsername).catch(function (err) {
        return console.error(err.toString());
    });
}