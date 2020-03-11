$.validator.setDefaults({
    ignore: [],
    highlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).addClass(errorClass).removeClass(validClass);
        } else {
            $(element).addClass('md-input-danger').removeClass('md-input-success');
            $(element).closest('.md-input-wrapper').addClass('md-input-wrapper-danger').removeClass('md-input-wrapper-success');
        }
    },
    unhighlight: function (element, errorClass, validClass) {
        if (element.type === 'radio') {
            this.findByName(element.name).removeClass(errorClass).addClass(validClass);
        } else {
            $(element).removeClass('md-input-danger').addClass('md-input-success');
            $(element).closest('.md-input-wrapper').removeClass('md-input-wrapper-danger').addClass('md-input-wrapper-success');
        }
    }
});

$.validator.methods.range = function (value, element, param) {
    var newValue = value.replace(",", ".");
    return this.optional(element) || (newValue >= param[0] && newValue <= param[1]);
}
 
$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}