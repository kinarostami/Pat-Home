$("form").submit(
    function () {
        var isOk = $(".input-validation-error");
        if (isOk.length == 0) {
            $(".loading").fadeIn();
            $("form button[type=submit]").attr("disabled", "true");
            setTimeout(function () {
                var s = $(".input-validation-error");
                console.log(s.length);
                if (s.length > 0) {
                    $(".loading").fadeOut();
                    $("form button[type=submit]").removeAttr("disabled");
                }
            }, 100, 1);
        } else {
        }
    });
function deleteItem(url, errorTitle, errorText) {
    if (errorTitle == null || errorTitle == "undefined") {
        errorTitle = "عملیات ناموفق";
    }
    if (errorText == null || errorText == "undefined") {
        errorText = "";
    }
    swal({
        title: "هشدار !!",
        text: "آیا از حذف اطمینان دارید ؟",
        icon: "warning",
        buttons: ["لغو", "بله"]
    }).then((isOk) => {
        if (isOk) {
            $.ajax({
                url: url,
                type: "get",
                beforeSend: function () {
                    $(".loading").show();
                },
                complete: function () {
                    $(".loading").hide();
                }
            }).done(function (data) {
                if (data === "Deleted") {
                    swal({
                        title: "عملیات با موفقیت انجام شد",
                        icon: "success",
                        button: "باشه"
                    }).then((isOk) => {
                        location.reload();
                    });
                } else {
                    swal({
                        title: errorTitle,
                        text: errorText,
                        icon: "error",
                        button: "باشه"
                    });
                }
            });
        }
    });
}
function Question(url, QuestionTitle, QuestionText, successText) {
    if (QuestionTitle == null || QuestionTitle == "undefined") {
        QuestionTitle = "آیا از انجام عملیات اطمینان دارید؟";
    }
    if (QuestionText == null || QuestionText == "undefined") {
        QuestionText = "";
    }
    if (successText == null || successText == "undefined") {
        successText = "";
    }
    swal({
        title: QuestionTitle,
        text: QuestionText,
        icon: "info",
        buttons: ["لغو", "بله"]
    }).then((isOk) => {
        if (isOk) {
            $.ajax({
                url: url,
                type: "get",
                beforeSend: function () {
                    $(".loading").show();
                },
                complete: function () {
                    $(".loading").hide();
                }
            }).done(function (data) {
                if (data === "Success") {
                    swal({
                        title: "عملیات با موفقیت انجام شد",
                        text: successText,
                        icon: "success",
                        button: "باشه"
                    }).then((isOk) => {
                        location.reload();
                    });
                } else {
                    swal({
                        title: "مشکلی پیش آمده !",
                        text: "لطفا در زمان دیگری امتحان کنید",
                        icon: "error",
                        button: "باشه"
                    });
                }
            });
        }
    });
}
function Success(Title, description) {
    if (Title == null || Title == "undefined") {
        Title = "عملیات با موفقیت انجام شد";
    }
    if (description == null || description == "undefined") {
        description = "";
    }
    swal({
        title: Title,
        text: description,
        icon: "success",
        button: "باشه"
    });
}
function Info(Title, description) {
    if (Title == null || Title == "undefined") {
        Title = "توجه";
    }
    if (description == null || description == "undefined") {
        description = "";
    }
    swal({
        title: Title,
        text: description,
        icon: "info",
        button: "باشه"
    });
}
function Error(Title, description) {
    if (Title == null || Title == "undefined") {
        Title = "مشکلی در عملیات رخ داده است";
    }
    if (description == null || description == "undefined") {
        description = "";
    }
    swal({
        title: Title,
        text: description,
        icon: "error",
        button: "باشه"
    });
}
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        textbox.addEventListener(event, function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    });
};

$(document).ready(function () {
    if (document.getElementById("select_")) {
        $("#select_").select2();
    }
    if (document.getElementById("select_1")) {
        $("#select_1").select2();
    }
    if (document.getElementById("select_2")) {
        $("#select_2").select2();
    }
    if (document.getElementById("select_custom")) {
        $("#select_custom").select2();
    }
    if (document.getElementById("number_input")) {
        setInputFilter(document.getElementById("number_input"),
            function (value) {
                return /^\d*\.?\d*$/.test(value);
            });
    }
    if (document.getElementById("number_input2")) {
        setInputFilter(document.getElementById("number_input2"),
            function (value) {
                return /^\d*\.?\d*$/.test(value);
            });
    }
    if (document.getElementById("select_Product")) {
        $("#select_Product").select2();
    }

});
$("#select_Product").change(function () {
    var id = $(this).val();
    console.log(id);
    if (id >= 1) {
        $.ajax({
            url: "/Seller/Products/ProductRoleInfo?productId=" + id,
            type: "get",
            beforeSend: function () {
                $(".loading").show();
            },
            complete: function () {
                $(".loading").hide();
            }
        }).done(function (data) {
            $("#info").prepend(data);
            $("#info").fadeIn();
        });

    } else {
        $("#info").fadeOut();
    }
});
function changePage(id) {
    $("#pageId").val(id);
    $("#filter").submit();
}
function AddFooterRow() {
    var count = $("#rowCount").val();

    for (var i = 0; i < count; i++) {
        $("#table-body").append(
            "<tr>" +
            "<td><input type='text' autocomplete='off' name='title' class='form-control'/></td>" +
            "<td><input type='text' autocomplete='off' name='url' class='form-control'/></td></tr>"
        );
    }
}
function AddRow() {
    var count = $("#rowCount").val();

    for (var i = 0; i < count; i++) {
        $("#table-body").append(
            "<tr>" +
            "<td><input type='text' autocomplete='off' name='ProductModel.Keys' class='form-control'/></td>" +
            "<td><input type='text' autocomplete='off' name='ProductModel.Values' class='form-control'/></td></tr>"
        );
    }
} 