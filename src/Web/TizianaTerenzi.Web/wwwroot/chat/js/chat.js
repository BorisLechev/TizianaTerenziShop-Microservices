"use strict";
var receiver = document.getElementById("receiver").textContent;
var sender = document.getElementById("sender").textContent;

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;

    let receiver = document.getElementById("receiver").textContent; //<h3>
    let sender = document.getElementById("sender").textContent; //<h3>
    let groupId = document.getElementById("groupId").value; //<input>

    updateScroll();

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

connection.on("SendMessage", function (message) {
    let dateTime = new Date();
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    var li = document.createElement("li");

    li.classList.add("message-list-item");

    li.innerHTML = `<div class="media-body ${(message.authorUserName === sender) ? "speech-right" : ""}">
                        <div class="speech">
                            <p>${message.content}</p>
                            <p class="speech-time">
                                <i class="far fa-clock"></i> ${formattedDate}
                            </p>
                        </div>
                    </div>`;

    document.getElementById("messagesList").appendChild(li);

    updateScroll();
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    let group = document.getElementById("groupId").value;
    let message = document.getElementById("messageInput").innerHTML;

    let data = new FormData();

    if (message) {
        if (message.length <= 500) {
            connection.invoke("SendMessage", sender, receiver, message, group).catch(function (err) {
                return console.error(err.toString());
            });

            document.getElementById("messageInput").innerHTML = "";
        } else {
            data.append('toUsername', receiver);
            data.append('fromUsername', sender);
            data.append('group', group);
            data.append('message', message);

            var li = document.createElement("li");

            li.classList.add("message-list-item");

            li.innerHTML = `<div class="media-body ${(message.authorUserName === sender) ? "speech-right" : ""}">
                        <div class="speech bg-danger text-white">
                            <p>Message should be no more than 500 characters long.</p>
                        </div>
                    </div>`;

            document.getElementById("messagesList").appendChild(li);

            updateScroll();
        }
    }

    $('#messageInput').css('padding-left', '10px');
    event.preventDefault();
});

function updateInputScroller() {
    let scroller = document.getElementById("messageInput");
    scroller.scrollTop = scroller.scrollHeight;
}

function updateScroll() {
    let element = document.getElementById("demo-chat-body");
    element.scrollTop = element.scrollHeight;
}

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