﻿function validateForm(btnSubmit) {
    var $form = $(btnSubmit).parents('form');
    var validator = $form.validate();

    var anyError = false;

    $form.find("input").each(function () {
            if (!validator.element(this)) { // validate every input element inside this step
                anyError = true;
            }
    });

    if (anyError)
        return false;
    else
        return true;

}


function submitContact(btnSubmit) {

    if (!validateForm(btnSubmit))
        return false;

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    btnSubmitvar.attr("disabled", "disabled");

    var errorDiv = $("#errorD");
    errorDiv.show();
    errorDiv.text("wait...");

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            errorDiv.text(error);
            btnSubmitvar.removeAttr("disabled");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            errorDiv.text(data);
            errorDiv.show();
        }
    });
}

function submitUniversal(btnSubmit, target) {

    if (!validateForm(btnSubmit))
        return false;

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    btnSubmitvar.attr("disabled", "disabled");

    var errorDiv = $("#errorD");
    errorDiv.show();
    errorDiv.text("wait...");

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            errorDiv.text(error);
            btnSubmitvar.removeAttr("disabled");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            if (data == "") {
                location.href = target;
            }
            else {
                errorDiv.text(data);
                errorDiv.show();
            }
        }
    });
}

function GetMaxNumber(urlAction) {
    var errorDiv = $("#errorD");
    $.ajax({
        type: "GET",
        url: urlAction,
        data: { topicId: $("input#TopicId").val() },
        error: function (xhr, status, error) {
            errorDiv.text(error);
        },
        success: function (data) {
            errorDiv.text(data);
            $("input#OrderId").val(data);
        }
    })
}

function CreateProfileData(btnSubmit) {
    tinyMCE.triggerSave();

    $("#Body").html($("#Body_ifr").html());

    if (!validateForm(btnSubmit))
        return false;

    console.log(btnSubmit);

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    $(btnSubmit).attr("disabled", "disabled");

    var errorDiv = $("#errorD");
    errorDiv.show();
    errorDiv.text("wait...");

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            errorDiv.text(error);
            btnSubmitvar.removeAttr("disabled");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            if (data == "") {
                location.href = '/Profile/PIndex';
            }
            else {
                errorDiv.text(data);
                errorDiv.show();
            }
        }
    });
    return false;
}

function CreateTdatas(btnSubmit, target) {

    tinyMCE.triggerSave();

    $("#Body").html($("#Body_ifr").html());

    if (!validateForm(btnSubmit))
        return false;

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    $(btnSubmit).attr("disabled", "disabled");

    var errorDiv = $("#errorD");
    errorDiv.show();
    errorDiv.text("wait...");

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            errorDiv.text(error);
            btnSubmitvar.removeAttr("disabled");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            if (data == "") {
                location.href = target;
            }
            else {
                errorDiv.text(data);
                errorDiv.show();
            }
        }
    });
    return false;
}