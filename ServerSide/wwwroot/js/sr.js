$(function () {
    var connection = new signalR.hubConnectionBuilder().writeUrl("/myHub").build();

    connection.start();

    $('message-input-form').submit(() => {
        console.log('sending: ' + $('textarea').val());
        connection.invoke("Changed", $('textarea').val());
    });

    connection.on("ChangedRecived", function (value) {
        console.log('recieved: ' + value);
        $('textarea').val(value);
    });
});