"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.start().then(function () {
    if (!sessionStorage.getItem("isFirstNotificationSound")) {
        sessionStorage.setItem("isFirstNotificationSound", true);
    } else {
        sessionStorage.setItem("isFirstNotificationSound", false);
    }

    let isFirstNotificationSound = sessionStorage.getItem("isFirstNotificationSound") == "true" ? true : false;

    connection.invoke("GetUserNotificationsCount", isFirstNotificationSound).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveNotification", function (count, isFirstNotificaitonSound) {
    document.getElementById("notificationCount").innerText = count; // span

    let title = document.querySelector("head title"); // head
    let bracketIndex = title.innerText.indexOf(")");
    let newTitle = "";

    if (count > 0) {
        if (isFirstNotificaitonSound) {
            document.querySelector("audio").load();
            document.querySelector("audio").play();
        }

        document.getElementById("notificationCount").classList.add("notificationCircle", "notificationPulse");
        newTitle = `(${count}) ${title.innerText.substring(bracketIndex + 1, title.innerText.length)}`;
    } else {
        document.getElementById("notificationCount").classList.remove("notificationCircle", "notificationPulse");
        newTitle = `${title.innerText.substring(bracketIndex + 1, title.innerText.length)}`;
    }

    title.innerText = newTitle;
});

connection.on("VisualizeNotification", function (notification) {
    let section = document.getElementById("allUserNotifications");

    if (section) {
        let newNotification = createNotification(notification);

        if (section.children.length == 0) {
            section.appendChild(newNotification);
        } else {
            section.insertBefore(newNotification, section.childNodes[0]);
        }
    }
});

function createNotification(notification) {
    let newNotification = document.createElement("article");
    let dateTime = new Date();
    let formattedDate =
        `${dateTime.getDate()}-${(dateTime.getMonth() + 1)}-${dateTime.getFullYear()} ${dateTime.getHours()}:${dateTime.getMinutes()}:${dateTime.getSeconds()}`;

    newNotification.id = notification.id;

    newNotification.innerHTML =
        `<section class="notification-container-content">
                    <img src="https://res.cloudinary.com/dxfq3iotg/image/upload/v1574583246/AAA/2.jpg" alt="avatar" />
                    <header>
                        <h4 class="text-primary">
                            <span class="heading-span">
                                <a class="delete-notification-button" onclick="deleteNotification('${notification.id}')">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                                <span>${notification.heading}</span>
                            </span>
                        </h4>
                    </header>
                    <main>
                        <p class="notification-text">${notification.text}</p>
                    </main>
                    <footer>
                        <span class="time-span">
                            <i class="far fa-clock"></i>
                            <span>${formattedDate}</span>
                        </span>
                    </footer>
                </section>`;

    return newNotification;
}