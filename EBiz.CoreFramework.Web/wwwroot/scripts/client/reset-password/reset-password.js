//Init
$(document).ready(function () {
	ResetPasswordValidation();
	$('input').keydown(function (event) {
		if (event.which === 13) {
			SubmitResetPassword();
		}
	});
});

//functions
function ResetPasswordValidation() {
    $('#resetPasswordForm').bootstrapValidator({
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

//function SubmitResetPassword() {
//	ResetPasswordValidation();
//    $('#resetPasswordForm').submit(function (ev) { ev.preventDefault(); });
//	$('#resetPasswordForm').data('bootstrapValidator').validate();
//	var isValid = $('#resetPasswordForm').data('bootstrapValidator').isValid();

//    if (isValid) {
//        var model = {
//            UserName: $('#UserName').val(),
//            Password: $('#Password').val(),
//			ConfirmPassword: $('#ConfirmPassword').val(),
//			token: $('#Token').val()
//		};
//		var settings = {
//			"url": 'https://localhost:44358/ResetPassword',
//			"method": "POST",
//			"timeout": 0,
//			"headers": {
//				"authorization": "Bearer " + model.token,
//				"Content-Type": "application/json"
//			},
//			"data": JSON.stringify(model),
//		};

//		$.ajax(settings).done(function (response) {
//			var respo = JSON.parse(response);
//			if (respo.status == 200) {
//				$('#alert-reset-password').html('<div class="alert alert-success" role="alert">' + respo.message + '</div>');
//				$('#Password').val("");
//				$('#ConfirmPassword').val("");
//				ResetForm()
//			}
			
//		}).fail(function (err) {
//			$('#alert-reset-password').html('<div class="alert alert-danger" role="alert">Sorry! Something bad happened.</div>');
//		});
//    }
//}

function ResetForm() {
	$('#resetPasswordForm').bootstrapValidator('resetForm', true);
}
//events