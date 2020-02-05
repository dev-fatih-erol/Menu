let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {
    $.get(host + "/ajax/table/" + getValueAtIndex(4) + "/orders", function (data) {
        console.log(data)
        var template = $("#template-orders-actual").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $(".orders-actual").html(result);
    });

    $.get(host + "/waiters", function (data) {
        console.log(data)
        var template = $("#template-waiters").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $("#waiters > tbody").append(result);
    });

    $(document).on("click", "#logout", function () {
        var url = host + '/logout'
        $.post(url, function (data) {
            location.href = host;
        });
    });

    Handlebars.registerHelper('ifEquals', function (arg1, arg2, options) {
        return (arg1 == arg2) ? options.fn(this) : options.inverse(this);
    });

    function getValueAtIndex(index) {
        var str = window.location.href; 
        return str.split("/")[index];
    }
});