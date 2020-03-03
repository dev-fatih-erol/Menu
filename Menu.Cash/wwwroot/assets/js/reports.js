let host = window.location.protocol + '//' + window.location.hostname + ':' + window.location.port;

$(document).ready(function () {

    const date = $("#slct").val();
    const dateNow = $("#hdnNow").val();
    $.get(host + "/ajax/table/reports?date=" + date + "&endDate=" + dateNow, function (data) {
        console.log(data);
        var template = $("#template-reports").html();
        var compiledCode = Handlebars.compile(template);
        var result = compiledCode(data.result);
        $(".reports").html(result);
    });

    $(document).on("change", "#slct", function () {

        const date = $("#slct").val();
        let endDate = "";

        const index = document.getElementById("slct").selectedIndex;

        if (index > 0) {
            endDate = $("#slct option").eq(index - 1).val();
        }
        else {
            endDate = $("#hdnNow").val();
        }

        $.get(host + "/ajax/table/reports?date=" + date + "&endDate=" + endDate, function (data) {
            console.log(data);
            var template = $("#template-reports").html();
            var compiledCode = Handlebars.compile(template);
            var result = compiledCode(data.result);
            $(".reports").html(result);
        });
    });

    $(document).on("click", "#logout", function () {
        var url = host + '/logout';
        $.post(url, function (data) {
            location.href = host;
        });
    });

    $(document).on("click", "#btndayreport", function () {
        var url = host + '/table/dayreports';
        $.post(url, function (data) {
            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Gün Sonu Başarıyla Alındı.Sayfa Birazdan Yenilenecektir.',
                showConfirmButton: false,
                timer: 2550
            });
            setTimeout(window.location.reload.bind(window.location), 2550);
        });
    });

    Handlebars.registerHelper('ifEquals', function (arg1, arg2, options) {
        return (arg1 == arg2) ? options.fn(this) : options.inverse(this);
    });
});
