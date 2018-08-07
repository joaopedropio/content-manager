var uri = "ws://" + window.location.host + "/ws";
function connect() {
    var list = document.getElementById("messages");
    socket = new WebSocket(uri);
    socket.onopen = function (event) {
        console.log("opened connection to " + uri);
    };
    socket.onclose = function (event) {
        console.log("closed connection from " + uri);
    };
    socket.onmessage = function (event) {
        appendItem(list, event.data);
        console.log(event.data);
    };
    socket.onerror = function (event) {
        console.log("error: " + event.data);
    };
}

function appendItem(list, message) {
    var item = document.createElement("p");
    item.appendChild(document.createTextNode(message));
    list.appendChild(item);
}

connect();