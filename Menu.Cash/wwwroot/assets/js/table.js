let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {
    $.get(host + "/ajax/table/" + getValueAtIndex(4) + "/orders", function (data) {
        console.log(data)
        var template = $("#template-orders-actual").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data);
        $(".orders-actual").html(result);
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

    $(document).on('click', '.show-modal-btn', function (e) {

        const userId = $(this).data("userid");
        $('#save-payment').data("userid", userId);

        $.get(host + "/ajax/user/" + userId + "/orders", function (data) {
            console.log(data)
            $("#total-price").val(data.result.totalPrice)
            $("#point").val(data.result.tlPoint)
            $("#used-point").val(data.result.usedPoint)
            $("#tip").val(data.result.tip)
            $("#real-price").text(data.result.realPrice)
            $('#payment-method').children().remove();
            $.each(data.result.paymentMethods, function (i, item) {

                $('#payment-method')
                    .append($('<option>', {
                        value: item.id,
                        text: item.text
                    }));
            });

            $('#payment-method option[value="' + data.result.orderPaymentMethod + '"]').prop("selected", true);
        });
    });

    $(document).on('click', '#save-payment', function (e) {
        let paymentMethod = $('#payment-method').val();
        let cashStatus = $('#cash-status').val();
        const userId = $(this).data("userid");
        const cashId = $("#cashIdHdn").val();
        $.post(host + "/ajax/user/" + userId + "/orders",
            {
                paymentMethod: paymentMethod,
                cashStatus: cashStatus,
                cashId : cashId
            },
            function (data, status) {
        });
    });
});

function getValueAtIndex(index) {
    var str = window.location.href;
    return str.split("/")[index];
}