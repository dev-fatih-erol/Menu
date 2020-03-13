let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {
    loadData();

    setInterval(function () { loadData(); }, 30000);

    $(document).on("click", ".order-action", function () {
        var id = $(this).data('id');
        var orderStatus = $(this).data('status');
        var table = $(this).data('table');
        var orderTableId = $(this).data('ordertable');
        var userId = $(this).data('userid');
        var url = host + '/order/' + id + '?orderStatus=' + orderStatus + '&tableId=' + table + '&orderTableId=' + orderTableId + '&userId=' + userId;
        $.post(url, function () {
            loadData(); 
        });
    });

    $(document).on("click", "#logout", function () {
        var url = host + '/logout'
        $.post(url, function (data) {
            location.href = host;
        });
    });
});

function loadData() {
    $.get(host + "/orders?orderStatus=Approved", function (data) {
        console.log(data.orders)
        var template = $("#template-orders-approved").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data.orders);
        $("#orders-approved").html(result);
        $("#approved-count").text(data.count);

    });

    $.get(host + "/orders?orderStatus=Preparing", function (data) {
        console.log(data.orders)
        var template = $("#template-orders-preparing").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data.orders);
        $("#orders-preparing").html(result);
        $("#preparing-count").text(data.count);
    });

    $.get(host + "/orders?orderStatus=Prepared", function (data) {
        console.log(data.orders)
        var template = $("#template-orders-prepared").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data.orders);
        $("#orders-prepared").html(result);
        $("#prepared-count").text(data.count);
    });
}