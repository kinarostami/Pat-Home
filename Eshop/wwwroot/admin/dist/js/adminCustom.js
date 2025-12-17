function UploadImage(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            //'userAvatar' img
            $('#image_target').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
function sendProduct(id) {
    Question(``, "محصولات را برای خریدار ارسال کرده اید؟", "", "");
    swal({
            text: 'کد رهگیری  سفارش را وارد کنید',
            content: {
                element: "input",
                attributes: {
                    placeholder: "کد رهگیری  مرسوله را وارد کنید",
                    type: "number",
                    required: "true"
                },
            },
            button: {
                text: "ثبت",
                closeModal: false,
            },
        })
        .then(name => {
            if (!name) throw null;

            $.ajax({
                url: `/admin/Orders/Show/${id}/SendProduct?trackingCode=${name}`,
                type: 'get',
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
                        icon: "success",
                        button: "باشه"
                    }).then((isOk) => {
                        location.reload();
                    });
                } else {
                    swal({
                        title: "عملیات ناموفق",
                        text: "مشکلی در عملیات پیش آمده",
                        icon: "error",
                        button: "باشه"
                    });
                }
            });
        })
        .catch(err => {
            if (err) {
                swal("Oh noes!", "The AJAX request failed!", "error");
            } else {
                swal.stopLoading();
                swal.close();
            }
        });
}
function showImage(imageName) {
    $(".show-gallery").fadeIn();
    $(".show-gallery img").attr("src", "/Assets/Images/Teachers/Galleries/" + imageName);
}
function showImageGallery(imageName) {
    $(".show-gallery").fadeIn();
    $(".show-gallery img").attr("src", "/Assets/Images/Groups/Galleries/" + imageName);
}

$(".show-gallery i").click(function () {
    $('.show-gallery').fadeOut();
    $(".show-gallery img").removeAttr("src");
});
$(".color-select").click(function () {
    var value = $(this).attr("color-for");
    $(this).addClass("selected");
    $(`#${value}`).prop("checked", true);
    var otherSelector = "[color-for!=" + value + "]";
    $(otherSelector).removeClass("selected");
});
$(".submit-discount").click(function () {
    var title = $("#DiscountCode_CodeTitle").val();
    $.ajax({
        url: `/Admin/DiscountCodes/IsCodeExist?code=${title}`,
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        if (data == "No") {
            $("#discount_form").submit();
        } else {
            Error("عنوان کد تکراری است", "عنوان دیگری انتخاب کنید");
        }
    });
});

$(document).ready(function () {

    $('[data-toggle="tooltip"]').tooltip();
    if (window.location.href.toLowerCase().includes("/tickets/show")) {
        $('.ticket-Body').animate({
            scrollTop: $('.ticket-Body .chat:last-child').position().top
        }, 'slow');
    }
    $.ajax({
        url: "/Admin/Notifications",
        type: "get"
    }).done(function (data) {
        //item1=messageCount
        //item2=notificationsCount
        //item3=MessageContents
        //item4=NotificationContent
        if (data.item2 > 0) {
            $(".notifications-menu .dropdown-toggle span").html(data.item2);
            $(".notifications-menu .dropdown-menu .header").html(`
            <li class="header text-center">شما ${data.item2} اعلان جدید دارید</li>`);

            $(".notifications-menu .dropdown-menu .menu").append(data.item4);
        }
        if (data.item1 > 0) {
            $(".messages-menu .dropdown-toggle span").html(data.item1);

            $(".messages-menu .dropdown-menu .header").html(`
            <li class="header text-center"> ${data.item1} پیام جدید ثبت شده است</li>`);
            $(".messages-menu .dropdown-menu .menu").append(data.item3);

            $(".notifications-menu .dropdown-menu").append(`
            <li class="footer"><a href="/Admin/ContactUs">نمایش تمام پیام ها</a></li>`);
        }
    });
});
$("#image_selector").change(function () {
    UploadImage(this);
});

class ShowMessage {
    constructor(isSuccess, isError) {
        this.isSuccess = isSuccess;
        this.isError = isError;
    }

    handlerMessage() {
        if (this.isSuccess == true) {
            Success();
        }
        if (this.isError == true) {
            Error();
        }
    }
}
function deleteShortCourse(id) {
    deleteItem("/Admin/Courses/ShortCourses/DeleteCourse?id=" + id);
}
function deleteLink(id) {
    deleteItem("/Admin/FooterLinks/DeleteLink?id=" + id);
}
function deleteFaq(id) {
    deleteItem("/Admin/Faq/DeleteFaq?faqId=" + id);
}
function deleteDetail(id) {
    deleteItem("/Admin/Orders/CreateOrder/DeleteDetail?id=" + id);
}

function DeleteEntity(id) {
    deleteItem("/Admin/Courses/Groups/BestTeachers/2/DeleteEntity?Id=" + id);
}
function deleteFaqDetail(id) {
    deleteItem("/Admin/Faq/Details/2/DeleteDetail?detailId=" + id);
}
function deleteImageGallery(id) {
    deleteItem("/Admin/Courses/Groups/Galleries/63/Delete?galleryId=" + id);
}

function deleteRoleCloth(id) {
    deleteItem("/Admin/RoleCloth/DeleteProductRole?id=" + id, "امکان حذف این رول وجود ندارد", "این رول دارای زیر مجموعه است برای حذف باید اول محصولات  تعریف شده برای این رول را حذف کنید");
}
function DeleteBrand(id) {
    deleteItem("/Admin/Brands/DeleteBrand?brandId=" + id, "امکان حذف این برند وجود ندارد", "این برند دارای زیر مجموعه است برای حذف باید اول رول های تعریف شده  برای این برند را حذف کنید");

}
function DeleteRole(id) {
    deleteItem("/Admin/Users/Roles/DeleteRole?roleId=" + id, null, "امکان حذف این نقش وجود ندارد، در صورت نیاز آن را ویرایش کنید");
}

function DeleteArticleGroup(id) {
    deleteItem("/Admin/Articles/Group/DeleteGroup?groupId=" + id, null, "امکان حذف این گروه وجود ندارد");
}
function DeleteCourseGroup(id) {
    deleteItem("/Admin/Courses/Groups/DeleteGroup?groupId=" + id, null, "امکان حذف این گروه وجود ندارد");
}
function deleteArticle(id) {
    Question("/Admin/Articles/DeleteArticle?articleId=" + id, "آیا از انجام عملیات اطمینان دارید؟", "امکان حذف کامل وجود ندارد، فقط وضعیت مقاله به غیرفعال تغییر میکند.");
}
function deleteEpisode(id) {
    deleteItem("/Admin/Courses/Episodes/2/DeleteEpisode?episodeId=" + id);
}
function DeleteProductGroup(id) {
    deleteItem("/Admin/Products/Groups/DeleteGroup?groupId=" + id, null, "امکان حذف این گروه وجود ندارد");
}
function deleteSlider(id) {
    deleteItem("/Admin/Sliders/DeleteSlider?sliderId=" + id);
}

function deleteType(id) {
    deleteItem("/Admin/SendTypes/DeleteSendType?id=" + id);
}
function deleteBanner(id) {
    deleteItem("/Admin/Banners/DeleteBanner?bannerId=" + id);

}
function deleteCode(id) {
    deleteItem("/Admin/DiscountCodes/DeleteCode?codeId=" + id);
}
function deleteAmazing(id) {
    deleteItem("/Admin/Products/Amazings/DeleteAmazing?amazingId=" + id);
}
function deleteCard(id) {
    deleteItem("/Admin/Cards/Show/" + id + "/deleteCard");
}
function deleteImage(id) {
    deleteItem("/Admin/Teachers/Show/" + id + "/DeleteGallery?id=" + id);
}
function toggleGalleryStatus(id) {
    Question("/Admin/Teachers/Show/" + id + "/ToggleGalleryStatus?id=" + id);
}
function changeSellerStatus(id, status) {
    Question(`/Admin/Sellers/Show/${id}/ChangeStatus?status=${status}`);
}
function ToggleVerifyStatus(id) {
    Question(`/Admin/Sellers/Show/${id}/ChangeVerifyStatus`);
}
function ToggleVerifyStatusForTeacher(id) {
    Question(`/Admin/Teachers/Show/${id}/ChangeVerifyStatus`);
}
function deleteColor(id) {
    deleteItem("/Admin/Colors/DeleteColor?colorId=" + id);
}
function changeStatus(id, status) {
    Question(`/Admin/Teachers/Show/${id}/ChangeStatus?status=${status}`);
}
function RejectRequest(id) {
    var text = $("#delete_desc").val();
    if (!text || text.length < 5) {
        alert("دلیل حذف خیلی کوتاه است");
    } else {
        swal({
            title: "هشدار",
            text: "آیا از حذف اطمینان دارید ؟",
            icon: "warning",
            buttons: ["لغو", "بله"]
        }).then((isOk) => {
            if (isOk) {
                $.ajax({
                    url: `/Admin/Products/Edit/${id}/DeleteProduct?rejectText=${text}`,
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
                            location.replace("/Admin/Products");

                        });
                    } else {
                        swal({
                            title: "عملیات ناموفق",
                            icon: "error",
                            button: "باشه"
                        });
                    }
                });
            }
        });
    }
}
function AcceptEpisode(id) {
    Question("/Admin/courses/Episodes/2/AcceptEpisode?episodeId=" + id, "آیا از انجام عملیات اطمینان دارید؟");

}

function deleteCourse(id) {
    var desc = $("#delete_desc").val();
    if (!desc && desc.length < 10) {
        Error("لطفا متن بزرگ تری وارد کنید");
    } else {
        deleteItem(`/Admin/Courses/DeleteCourse?courseId=${id}&description=${desc}`, "امکان حذف این دوره وجود ندارد");
    }
}

function closeTicket(id) {
    Question(`/Admin/Tickets/show/${id}/CloseTicket`,
        "آیا از بستن تیکت اطمینان دارید؟",
        "درصورت بسته شدن تیکت دیگر امکان ارسال پیام در تیکت وجود ندارد.");
}


$("#ArticleModel_GroupId").select2();
$("#ArticleModel_GroupId").change(function () {
    $.ajax({
        url: "/Admin/Articles/Add/GroupsByParent?parentId=" + $(this).val(),
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $("#select_").html(data);
    });
});
$("#meta_des").keydown(function () {
    var length = $(this).val().length;
    $(".counter").html(length);
});
$("#meta_des").change(function () {
    var length = $(this).val().length;
    $(".counter").html(length);
});
$("#meta_des1").keydown(function () {
    var length = $(this).val().length;
    $(".counter1").html(length);
});
$("#meta_des1").change(function () {
    var length = $(this).val().length;
    $(".counter1").html(length);
});
$("#meta_des2").keydown(function () {
    var length = $(this).val().length;
    $(".counter2").html(length);
});
$("#meta_des2").change(function () {
    var length = $(this).val().length;
    $(".counter2").html(length);
});
$("#ProductModel_Product_SubParentGroupId").select2();
$("#ProductModel_Product_ParentGroupId").select2();
$("#ProductModel_Product_GroupId").select2();


$("#ProductModel_Product_ParentGroupId").change(function () {
    var id = $(this).val();
    $.ajax({
        url: "/Admin/Products/Groups/ProductGroupOptions?parentId=" + id,
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $("#ProductModel_Product_SubParentGroupId").html(data);
    });
});
$("#ProductModel_Product_GroupId").change(function () {
    var id = $(this).val();
    $.ajax({
        url: "/Admin/Products/Groups/ProductGroupOptions?parentId=" + id,
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $("#ProductModel_Product_ParentGroupId").html(data);
    });
});
function setColor(currency) {
    if (!currency.id) { return currency.text; }
    var $currency = $('<span style=display:inline-block;color:black>' + currency.text + '</span>    <span style="background:' + currency.title + ';width:15px;height:15px;display:inline-block;box-shadow:0 0 5px 0' + currency.title + ';position:relative;top:3px;"></span>');
    return $currency;
};
$("#select_color").select2({
    templateResult: setColor,
    templateSelection: setColor
});
function deleteProducts(id) {
    swal({
        title: "هشدار !!",
        text: "آیا از حذف اطمینان دارید ؟",
        icon: "warning",
        buttons: ["لغو", "بله"]
    }).then((isOk) => {
        if (isOk) {
            $.ajax({
                url: `/Admin/Products/DeleteProduct?productId=${id}`,
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
                } else if (data === "SoftDeleted") {
                    swal({
                        title: "امکان حذف کامل محصول وجود ندارد",
                        text: "اما وضعیت محصول در فروشگاه به 'ناموجود' تغییر یافت",
                        icon: "success",
                        button: "باشه"
                    }).then((isOk) => {
                        location.reload();
                    });
                }
                else {
                    swal({
                        title: "عملیات ناموفق",
                        icon: "error",
                        button: "باشه"
                    });
                }
            });
        }
    });
}
if (document.getElementById('salesChart')) {
    var registerChartRoot = document.getElementById('salesChart').getContext('2d');
    var registerLabels = JSON.parse($("#sales-chart-label").val());
    var registerValues = JSON.parse($("#sales-chart-value").val());
    var registerChart = new Chart(registerChartRoot, {
        type: 'line',
        data: {
            labels: registerLabels.reverse(),
            datasets: [
                {
                    label: 'آمار فروش هفت روز گذشته',
                    data: registerValues.reverse(),

                }
            ]
        },
        options: {
            showScale: false,
            responsive: true,
            legend: {
                display: false,

            },

            tooltips: {
                enabled: false,
                custom: function (tooltipModel) {
                    // Tooltip Element
                    var tooltipEl = document.getElementById('chartjs-tooltip');

                    // Create element on first render
                    if (!tooltipEl) {
                        tooltipEl = document.createElement('div');
                        tooltipEl.id = 'chartjs-tooltip';
                        tooltipEl.innerHTML = '<table></table>';
                        document.body.appendChild(tooltipEl);
                    }

                    // Hide if no tooltip
                    if (tooltipModel.opacity === 0) {
                        tooltipEl.style.opacity = 0;
                        return;
                    }

                    // Set caret Position
                    tooltipEl.classList.remove('above', 'below', 'no-transform');
                    if (tooltipModel.yAlign) {
                        tooltipEl.classList.add(tooltipModel.yAlign);
                    } else {
                        tooltipEl.classList.add('no-transform');
                    }

                    function getBody(bodyItem) {
                        return bodyItem.lines;
                    }

                    // Set Text
                    if (tooltipModel.body) {
                        var titleLines = tooltipModel.title || [];
                        var bodyLines = tooltipModel.body.map(getBody);

                        var innerHtml = '<thead style="background:transparent">';

                        titleLines.forEach(function (title) {
                            innerHtml += '<tr><th style="text-align:center">' + title + '</th></tr>';
                        });
                        innerHtml += '</thead><tbody>';

                        bodyLines.forEach(function (body, i) {
                            var visit = body[0].split(":")[1];
                            var colors = tooltipModel.labelColors[i];
                            var style = 'background:' + colors.backgroundColor;
                            style += '; border-color:' + colors.borderColor;
                            style += '; border-width: 2px';
                            var span = '<span style="' + style + '"></span>';
                            innerHtml += '<tr><td>' + span + "تعداد فروش  : " + visit + '</td></tr>';
                        });
                        innerHtml += '</tbody>';

                        var tableRoot = tooltipEl.querySelector('table');
                        tableRoot.innerHTML = innerHtml;
                    }

                    // `this` will be the overall tooltip
                    var position = this._chart.canvas.getBoundingClientRect();

                    // Display, position, and set styles for font
                    tooltipEl.style.opacity = .8;
                    tooltipEl.style.position = 'absolute';
                    tooltipEl.style.background = 'black';
                    tooltipEl.style.color = 'white';
                    tooltipEl.style.borderRadius = '10px';
                    tooltipEl.style.left = position.left + window.pageXOffset + tooltipModel.caretX + 'px';
                    tooltipEl.style.top = position.top + window.pageYOffset + tooltipModel.caretY + 'px';
                    tooltipEl.style.fontFamily = 'iranyekan';
                    tooltipEl.style.fontSize = tooltipModel.bodyFontSize + 'px';
                    tooltipEl.style.fontStyle = tooltipModel._bodyFontStyle;
                    tooltipEl.style.padding = tooltipModel.yPadding + 'px ' + tooltipModel.xPadding + 'px';
                    tooltipEl.style.pointerEvents = 'none';
                }
            }
        }
    });
}
if (document.getElementById('registerChart')) {
    var registerChartRoot = document.getElementById('registerChart').getContext('2d');
    var registerLabels = JSON.parse($("#sales-chart-label").val());
    var registerValues = JSON.parse($("#register-chart-value").val());
    var registerChart = new Chart(registerChartRoot, {
        type: 'line',
        data: {
            labels: registerLabels.reverse(),
            datasets: [
                {
                    label: 'آمار فروش هفت روز گذشته',
                    data: registerValues.reverse(),

                }
            ]
        },
        options: {
            showScale: false,
            responsive: true,
            legend: {
                display: false,

            },

            tooltips: {
                enabled: false,
                custom: function (tooltipModel) {
                    // Tooltip Element
                    var tooltipEl = document.getElementById('chartjs-tooltip');

                    // Create element on first render
                    if (!tooltipEl) {
                        tooltipEl = document.createElement('div');
                        tooltipEl.id = 'chartjs-tooltip';
                        tooltipEl.innerHTML = '<table></table>';
                        document.body.appendChild(tooltipEl);
                    }

                    // Hide if no tooltip
                    if (tooltipModel.opacity === 0) {
                        tooltipEl.style.opacity = 0;
                        return;
                    }

                    // Set caret Position
                    tooltipEl.classList.remove('above', 'below', 'no-transform');
                    if (tooltipModel.yAlign) {
                        tooltipEl.classList.add(tooltipModel.yAlign);
                    } else {
                        tooltipEl.classList.add('no-transform');
                    }

                    function getBody(bodyItem) {
                        return bodyItem.lines;
                    }

                    // Set Text
                    if (tooltipModel.body) {
                        var titleLines = tooltipModel.title || [];
                        var bodyLines = tooltipModel.body.map(getBody);

                        var innerHtml = '<thead style="background:transparent">';

                        titleLines.forEach(function (title) {
                            innerHtml += '<tr><th style="text-align:center">' + title + '</th></tr>';
                        });
                        innerHtml += '</thead><tbody>';

                        bodyLines.forEach(function (body, i) {
                            var visit = body[0].split(":")[1];
                            var colors = tooltipModel.labelColors[i];
                            var style = 'background:' + colors.backgroundColor;
                            style += '; border-color:' + colors.borderColor;
                            style += '; border-width: 2px';
                            var span = '<span style="' + style + '"></span>';
                            innerHtml += '<tr><td>' + span + "تعداد ثبت نام  : " + visit + '</td></tr>';
                        });
                        innerHtml += '</tbody>';

                        var tableRoot = tooltipEl.querySelector('table');
                        tableRoot.innerHTML = innerHtml;
                    }

                    // `this` will be the overall tooltip
                    var position = this._chart.canvas.getBoundingClientRect();

                    // Display, position, and set styles for font
                    tooltipEl.style.opacity = .8;
                    tooltipEl.style.position = 'absolute';
                    tooltipEl.style.background = 'black';
                    tooltipEl.style.color = 'white';
                    tooltipEl.style.borderRadius = '10px';
                    tooltipEl.style.left = position.left + window.pageXOffset + tooltipModel.caretX + 'px';
                    tooltipEl.style.top = position.top + window.pageYOffset + tooltipModel.caretY + 'px';
                    tooltipEl.style.fontFamily = 'iranyekan';
                    tooltipEl.style.fontSize = tooltipModel.bodyFontSize + 'px';
                    tooltipEl.style.fontStyle = tooltipModel._bodyFontStyle;
                    tooltipEl.style.padding = tooltipModel.yPadding + 'px ' + tooltipModel.xPadding + 'px';
                    tooltipEl.style.pointerEvents = 'none';
                }
            }
        }
    });
}
function showColorPage(id) {
    $.ajax({
        url: "/Admin/Colors/ShowPage?colorId=" + id,
        type: "get"
    }).done(function (data) {
        $(".modal-content #content").html(data);
    });
}
function showTeacherPage() {
    $.ajax({
        url: "/Admin/Courses/Groups/BestTeachers/2/ShowPage",
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $(".modal-content #content").html(data);
    });
}
function showNewslettersPage(id) {
    $.ajax({
        url: "/Admin/NewsLetters/ShowPage?Id=" + id,
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $(".modal-content #content").html(data);
    });
}
function showWithdrawalPage(id) {
    $.ajax({
        url: "/Admin/Withdrawals/ShowPage?Id=" + id,
        type: "get",
        beforeSend: function () {
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
    }).done(function (data) {
        $(".modal-content #content").html(data);
    });
}

function DeleteWithdrawal() {
    var rejectText = $("#description").val();

    var id = $("#id").val();
    if (!rejectText) {
        Error("لطفا دلیل حذف را بیان کنید");
    } else {
        deleteItem(`/Admin/Withdrawals/RejectRequest?id=${id}&text=${rejectText}`);
    }
}
