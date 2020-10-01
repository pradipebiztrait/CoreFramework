$("#ImageFile").change(function () {
    readURLEmail(this);
});

function readURLEmail(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imagePreview').css('background-image', 'url(' + e.target.result + ')');
            $('#imagePreview').hide();
            $('#imagePreview').fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}


function FormModelValidator() {
    $('#form-model').bootstrapValidator({
        fields: {
            FirstName: {
                validators: {
                    notEmpty: {
                        message: 'First Name is required.'
                    }
                }
            },
            LastName: {
                validators: {
                    notEmpty: {
                        message: 'Last Name is required.'
                    }
                }
            },
        }
    });
}

function SubmitForm() {
    FormModelValidator();
    $('#form-model').submit(function (ev) { ev.preventDefault(); });
    $('#form-model').data('bootstrapValidator').validate();
    var isValid = $('#form-model').data('bootstrapValidator').isValid();

    if (isValid) {
        var formData = new FormData();
        var model = {
            UserId: $('#hidden_UserId').val(),
            Email: $('#Email').val(),
            FirstName: $('#FirstName').val(),
            LastName: $("#LastName").val(),
            MobileNumber: $("#MobileNumber").val(),
            ImagePath: $("#hidden_ImagePath").val(),
            ImageName: $("#hidden_ImageName").val(),
        };

        formData.append("model", JSON.stringify(model));
        formData.append("img", $("#ImageFile")[0].files[0]);
        console.log(model);
        $.ajax({
            url: adminBaseUrl + "/Admin/Profile/SaveAsync",
            type: "POST",
            contentType: false,
            data: formData,
            cache: false,
            processData: false,
            success: function (response) {
                console.log(response);
                if (response.status) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message, "Error");
                }
                console.log(response);
            }, error: function (jqXHR) {
                toastr.error(jqXHR.responseJSON.Message, "Error");
            }
        });
    }
}