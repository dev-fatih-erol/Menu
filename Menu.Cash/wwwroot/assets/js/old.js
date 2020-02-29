let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {
    $.get(host + "/ajax/table/old", function (data) {
        console.log(data)
        var template = $("#template-old").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $(".olders").html(result);
    });

    $(document).on("click", "#logout", function () {
        var url = host + '/logout'
        $.post(url, function (data) {
            location.href = host;
        });
    });


    $(document).on("click", ".btnSave", function () {
        const id = $(this).data("id");
        const cash = $(this).data("cash");
        const val = $("#select-" + id + " :selected").val();
        $.post(host + "/Ajax/Table/ChangeOrderCashStatus",
            {
                id: cash,
                orderCashStatus: val,
            },
            function (data, status) {
                alert("Başarılı bir şekilde değiştirildi.");
            });
    });

    Handlebars.registerHelper('ifEquals', function (arg1, arg2, options) {
        return (arg1 == arg2) ? options.fn(this) : options.inverse(this);
    });
});

function getValueAtIndex(index1) {
    var str = window.location.href;
    return str.split("/")[index1];
}