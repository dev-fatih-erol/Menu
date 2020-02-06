let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

let apiUrl = GetApiUrl();

$(document).ready(function () {
  
    var groupName = document.getElementById("group-name").value;

    const orderHubUrl = apiUrl + "/orderHub?groupName=" + groupName;

    var connection = new signalR.HubConnectionBuilder().withUrl(orderHubUrl, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    }).build();

    connection.start().then(function () {
        console.log("connected");
    });

    connection.on("ReceiveVenueOrders", function (data) {
        console.log(data)
        var templateApproved = $("#template-orders-approved").html();
        var compiledCodeApproved = Handlebars.compile(templateApproved);
        var resultApproved = compiledCodeApproved(data.approvedOrders);
        $("#orders-approved").html(resultApproved);

        var templatePreparing = $("#template-orders-preparing").html();
        var compiledCodePreparing = Handlebars.compile(templatePreparing);
        var resultPreparing = compiledCodePreparing(data.preparingOrders);
        $("#orders-preparing").html(resultPreparing);

        var templatePrepared = $("#template-orders-prepared").html();
        var compiledCodePrepared = Handlebars.compile(templatePrepared);
        var resultPrepared = compiledCodePrepared(data.preparedOrders);
        $("#orders-prepared").html(resultPrepared);
    });

    $.get(host + "/orders?orderStatus=Approved", function (data) {
        console.log(data)
        var template = $("#template-orders-approved").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $("#orders-approved").html(result);
    });

    $.get(host + "/orders?orderStatus=Preparing", function (data) {
        console.log(data)
        var template = $("#template-orders-preparing").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $("#orders-preparing").html(result);
    });

    $.get(host + "/orders?orderStatus=Prepared", function (data) {
        console.log(data)
        var template = $("#template-orders-prepared").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $("#orders-prepared").html(result);
    });

    $(document).on("click", ".order-action", function () {
        var venueId = document.getElementById("venueId").value;
        var groupName = document.getElementById("group-name").value;
        var id = $(this).data('id');
        var orderStatus = $(this).data('status');
        var url = host + '/order/' + id + '?orderStatus=' + orderStatus;
        $.post(url, function () {
            connection.invoke("ReloadVenueOrders", venueId, groupName).catch(function (err) {
                return console.error(err.toString());
            });
        });
    });

    $(document).on("click", "#logout", function () {
        var url = host + '/logout'
        $.post(url, function (data) {
            location.href = host;
        });
    });
});

function GetApiUrl() {
    if (location.hostname === "localhost") {
        return "https://localhost:5001";
    }
    return "http://mottomobil.net";
}