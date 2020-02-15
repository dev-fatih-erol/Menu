let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {
    loadData();

    setInterval(function () { loadData(); }, 60000);

    $(document).on("click", ".order-action", function () {
        var id = $(this).data('id');
        var orderStatus = $(this).data('status');
        var url = host + '/order/' + id + '?orderStatus=' + orderStatus;
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
}