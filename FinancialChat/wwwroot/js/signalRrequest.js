"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.start()
    .catch(err => alert(err.toString()));

connection.on('receiveMessage', addMessageToChat);

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}