"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

function updateScroll() {
    //if (document.getElementById("scrollBottomButton").style.visibility != "visible") {
        let element = document.getElementById("demo-chat-body");
        element.scrollTop = element.scrollHeight;
    //}
}

function userType(receiverUsername, senderUsername) {
    connection.invoke("UserType", senderUsername, receiverUsername).catch(function (err) {
        return console.error(err.toString());
    });
}

function userStopType(receiverUsername) {
    connection.invoke("UserStopType", receiverUsername).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("VisualizeUserType", function () {
    let typeMessageElement = document.getElementById("typeMessage");

    if (!typeMessageElement) {
        document.getElementById("messagesList").innerHTML += `
            <li id="typeMessage" class="message-list-item">
                <div class="media-body">
                    <div class="speech">
                        <span class="jumping-dots">
                            <span class="jumping-dot-1"></span>
                            <span class="jumping-dot-2"></span>
                            <span class="jumping-dot-3"></span>
                        </span>
                    </div>
                </div>
            </li>`;

        updateScroll();
    }
});

connection.on("VisualizeUserStopType", function () {
    let typeMessageElement = document.getElementById("typeMessage");

    if (typeMessageElement) {
        document.getElementById("messagesList").removeChild(typeMessageElement);
    }
});

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message; //.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let dateTime = new Date();
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    let li = document.createElement("li");

    li.classList.add("message-list-item");

    li.innerHTML = `<div class="media-body">
                        <div class="speech">
                            <p>${msg}</p>
                            <p class="speech-time">
                                <i class="far fa-clock"></i> ${formattedDate}
                            </p>
                        </div>
                     </div>`;

    document.getElementById("messagesList").appendChild(li);

    updateScroll();
});

connection.on("SendMessage", function (user, message) {
    var msg = message; //.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let dateTime = new Date();
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    var li = document.createElement("li");

    li.classList.add("message-list-item");

    li.innerHTML = `<div class="media-body speech-right">
                        <div class="speech">
                            <p>${msg}</p>
                            <p class="speech-time">
                                <i class="far fa-clock"></i> ${formattedDate}
                            </p>
                        </div>
                    </div>`;

    document.getElementById("messagesList").appendChild(li);

    updateScroll();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    let receiver = document.getElementById("receiver").textContent;
    let sender = document.getElementById("sender").textContent;
    let group = document.getElementById("groupName").value;

    updateScroll();

    connection.invoke("AddToGroup", group, receiver, sender).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let receiver = document.getElementById("receiver").textContent;
    let sender = document.getElementById("sender").textContent;
    let group = document.getElementById("groupName").value;
    let message = document.getElementById("messageInput").innerHTML;

    let data = new FormData();

    if (message) {
        connection.invoke("SendMessage", sender, receiver, message, group).catch(function (err) {
            return console.error(err.toString());
        });

        connection.invoke("ReceiveMessage", sender, message, group).catch(function (err) {
            return console.error(err.toString());
        });
    } else {
        data.append('toUsername', receiver);
        data.append('fromUsername', sender);
        data.append('group', group);
        data.append('message', message);
    }

    document.getElementById("messageInput").innerHTML = "";
    $('#messageInput').css('padding-left', '10px');
    event.preventDefault();
});

function updateInputScroller() {
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
}