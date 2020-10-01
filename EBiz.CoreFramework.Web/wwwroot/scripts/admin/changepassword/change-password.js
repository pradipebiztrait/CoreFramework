//Init
$(document).ready(function () {
    ChangePasswordValidation();
});

//functions
function ChangePasswordValidation() {
    $('#changePasswordForm').bootstrapValidator({
        fields: {
            Password: {
                validators: {
                    notEmpty: {
                        message: 'Password is required.'
                    },
                    identical: {
                        field: 'ConfirmPassword',
                        message: 'The password and its confirm are not the same'
                    }
                }
            },
            ConfirmPassword: {
                validators: {
                    notEmpty: {
                        message: 'Confirm password is required.'
                    },
                    identical: {
                        field: 'Password',
                        message: 'The password and its confirm are not the same'
                    }
                }
            }
        }
    });
}

function SubmitChangePassword() {
    ChangePasswordValidation();
    $('#changePasswordForm').submit(function (ev) { ev.preventDefault(); });
    $('#changePasswordForm').data('bootstrapValidator').validate();
    var isValid = $('#changePasswordForm').data('bootstrapValidator').isValid();

    if (isValid) {
        var model = {
            UserName: $('#UserName').val(),
            Password: $('#Password').val(),
            ConfirmPassword: $('#ConfirmPassword').val()
        };

        $.ajax({
			url: adminBaseUrl + "/Admin/SubmitChangesPassword",
            type: "POST",
            data: { model: JSON.stringify(model) },
            cache: false,
            success: function (response) {
                if (response.status) {
					toastr.success(response.message, "Success");
					setTimeout(function () { window.location.href = adminBaseUrl + '/Admin/ChangePassword'; }, 1500);
                } else {
                    toastr.error(response.message, "error");
                }
            }, error: function (jqXHR) {
                console.log(jqXHR);
            }
        });
    }
}

function ResetForm() {
	$('#changePasswordForm').bootstrapValidator('resetForm', true);
}
//events