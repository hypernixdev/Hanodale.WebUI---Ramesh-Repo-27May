$(document).ready(function () {
    jQuery.validator.addMethod("date", function (value, element, params) {
        var result = false;
        var isTime = true;
        if ($(element).hasClass("datepicker")) {
            isTime = true;
            if (this.optional(element)) {
                return true;
            };
            var name = $(element).attr('name');


            //alert($("input[id='" + name + "'][class~='timepicker']").length)
            if ($("input[name='" + name + ".Time'][class~='timepicker']").length > 0) {
                try {
                    var timeRegex = new RegExp('([0-9]{1,2}):([0-9]{1,2}) (AM|PM)');
                    if ($("input[name='" + name + ".Time'][class~='timepicker24Format']").length > 0)
                        timeRegex = new RegExp('([0-9]{1,2}):([0-9]{1,2})');

                    isTime = timeRegex.test($("input[name='" + name + ".Time'][class~='timepicker']").val());
                } catch (err) { return false; };
            }

            try {
                $.datepicker.parseDate("dd/mm/yy", value);
                //alert(true + "  ---" + isTime);
                return (true && isTime);
            } catch (err) { return false; };
        }
        if ($(element).hasClass("timepicker")) {
            var result = false;
            try {
                var timeRegex = new RegExp('([0-9]{1,2}):([0-9]{1,2}) (AM|PM)');
                if ($(element).hasClass("timepicker24Format"))
                    timeRegex = new RegExp('([0-9]{1,2}):([0-9]{1,2})');

                var isTime = timeRegex.test(value);
                return isTime;
            } catch (err) { return false; };
        }
    },
    "");
})
