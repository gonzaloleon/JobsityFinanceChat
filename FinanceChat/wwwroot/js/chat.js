"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/Chat")
    .withAutomaticReconnect()
    .build();

connection.keepAliveIntervalInMilliseconds = 1000 * 60 * 3; // Three minutes
connection.serverTimeoutInMilliseconds = 1000 * 60 * 6; // Six minutes
//Disable send button until connection is established  
connection.on("ReceiveMessage", function (user, message) {
    var dt = new Date();
    var dateStr =
        ("00" + (dt.getMonth() + 1)).slice(-2) + "/" +
        ("00" + dt.getDate()).slice(-2) + "/" +
        dt.getFullYear() + " " +
        ("00" + dt.getHours()).slice(-2) + ":" +
        ("00" + dt.getMinutes()).slice(-2) + ":" +
        ("00" + dt.getSeconds()).slice(-2);
    var msg = message.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");
    var encodedMsg = "[" + dateStr + "] <strong>" + user + "</strong> says " + msg;
    var li = document.createElement("li");
    li.innerHTML = encodedMsg;
    document.getElementById("ulmessages").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendBtn").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
