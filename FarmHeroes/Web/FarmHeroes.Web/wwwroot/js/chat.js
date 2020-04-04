let connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chatter")
        .build();

connection.on("NewMessage",
    function (message) {
        let chatMessageElement = `<div class="row mt-2 notification">
                    <div class="col-9">
                        <div class="row">
                            <small class="align-top pl-3 chat-message"><strong>${message.sender}:</strong></small>
                        </div>
                        <div class="row">
                            <small class="pl-3 align-top">${escapeHtml(message.text)}</small>
                        </div>
                    </div>
                    <div class="col-3">
                        <small class="float-right">${message.createdOn}</small>
                    </div>
                </div>`;

        $('#chat-messages').prepend(chatMessageElement);
    });

connection.on("NewSystemMessage",
    function (message) {
        let chatMessageElement = `<div class="row mt-2 notification">
                    <div class="col-9">
                        <div class="row">
                            <small class="align-top pl-3 chat-message"><strong>${message.sender}:</strong></small>
                        </div>
                        <div class="row">
                            <small class="pl-3 align-top">${escapeHtml(message.text)}</small>
                        </div>
                    </div>
                    <div class="col-3">
                        <small class="float-right">${message.createdOn}</small>
                    </div>
                </div>`;

        $('#chat-messages').prepend(chatMessageElement);
    });

$('#send-message-button').click(function () {
    let message = $('#message-input').val();

    if (message.length < 1) {
        return;
    }

    connection.invoke("Send", message);
    $('#message-input').val('');
});

$('#message-input').on('keypress', function (e) {
    if (e.which == 13) {
        let message = $('#message-input').val();

        if (message.length < 1) {
            return;
        }

        connection.invoke("Send", message);
        $('#message-input').val('');
    }
});

connection.start().catch(function (error) {
    return console.error(error.toString());
});

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
};