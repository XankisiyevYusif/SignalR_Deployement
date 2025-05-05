"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

connection.start().then(function () {
    GetAllUsers();
})


connection.on("Connect", function (info) {
    GetAllUsers();
    const element = document.querySelector("#alert");
    element.style.display = "block";
    element.innerHTML = info;

    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})


connection.on("Disconnect", function (info) {
    GetAllUsers();
    const element = document.querySelector("#alert");
    element.style.display = "block";
    element.innerHTML = info;

    setTimeout(() => {
        element.innerHTML = "";
        element.style.display = "none";
    }, 5000);
})

async function SendFollowCall(id) {
    await connection.invoke("SendFollow", id);
}

connection.on("ReceiveNotification", function () {
    GetMyRequests();
    GetAllUsers();
})