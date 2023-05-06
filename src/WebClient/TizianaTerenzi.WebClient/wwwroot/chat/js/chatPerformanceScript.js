$(document).ready(function () {
    $("#demo-chat-body").scroll(function () {
        let scrollHeight = document.getElementById("demo-chat-body").scrollHeight;
        let scrollDistanceToTop = document.getElementById("demo-chat-body").scrollTop;
        let chatBodyHeight = document.getElementById("demo-chat-body").offsetHeight;
        let distanceToBottom = scrollHeight - scrollDistanceToTop - chatBodyHeight;

        if (distanceToBottom > 400) {
            document.getElementById("scrollBottomButton").style.visibility = "visible";
        } else if (distanceToBottom >= 0 && distanceToBottom <= 100) {
            document.getElementById("scrollBottomButton").style.visibility = "hidden";
        }
    });
});

function scrollChatToBottom() {
    $("#demo-chat-body").animate({
        scrollTop: document.getElementById("demo-chat-body").scrollHeight
    }, 800);
}