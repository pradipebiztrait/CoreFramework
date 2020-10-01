
//Local variable
var $submitForm = $('#submit-form');
var $resetForm = $('#reset-form');
var $closeForm = $('#close-form');

var $dataTable = $('#dataTable');
var $table = $('#table')
var $remove = $('#remove')
var selections = [];
var selectUserId = 0;
var filterDefaults = {
    0: 'Deactive',
    1: 'Active'
};

$(document).ready(function () {
    formValidation();
    $('[data-mask]').inputmask();
    $('.select2').select2();
});

//events
$submitForm.click(function () {
    formValidation();
    $('#form-model').submit(function (ev) { ev.preventDefault(); });
    $('#form-model').data('bootstrapValidator').validate();
    var isValid = $('#form-model').data('bootstrapValidator').isValid();

    if (isValid) {
        var formData = new FormData();
        var model = {
            RoleId: document.getElementById("ddlRoleId").options[document.getElementById("ddlRoleId").selectedIndex].value,
            UserId: $('#hidden_UserId').val(),
            FirstName: $('#FirstName').val(),
            LastName: $('#LastName').val(),
            MobileNumber: $('#MobileNumber').val(),
            EmailAddress: $('#EmailAddress').val(),
            Password: $('#Password').val()
        };

        formData.append("model", JSON.stringify(model));
        formData.append("img", null);

        $.ajax({
            url: adminBaseUrl + "/Admin/staff/SaveAsync",
            type: "POST",
            contentType: false,
            data: formData,
            cache: false,
            processData: false,
            success: function (response) {
                debugger;
                if (response.status) {
                    $table.bootstrapTable('refresh');
                    toastr.success(response.message);
                    $('#modal-manage-staff').modal('hide');
                } else {
                    toastr.error(response.message, "Error");
                }
                console.log(response);
            }, error: function (jqXHR) {
                toastr.error(jqXHR.responseJSON.Message, "Error");
            }
        });
    }
})

$resetForm.click(function () { formValidation(); $('#form-model').bootstrapValidator('resetForm', true); });
$closeForm.click(function () { formValidation(); $('#form-model').bootstrapValidator('resetForm', true); });

function formValidation() {
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
                    },

                }
            },
            MobileNumber: {
                validators: {
                    notEmpty: {
                        message: 'Contact Number is required.'
                    },
                    phone: {
                        country: 'US',
                        message: 'Invalid Contact Number'
                    }
                }
            },
            EmailAddress: {
                validators: {
                    notEmpty: {
                        message: 'Email Address is required.'
                    },
                    emailAddress: {
                        message: 'Invalid Email Address.'
                    },
                    remote: {
                        type: 'GET',
                        url: adminBaseUrl + "/Admin/staff/IsExistEmailAddress",
                        message: 'Email Address already exist.',
                        delay: 1000
                    },
                }
            },
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
            },
            role: {
                validators: {
                    notEmpty: {
                        message: 'Role is required.'
                    }
                }
            }
        }
    });
}

function IsExistEmail(email) {
    $.get(adminBaseUrl + "/Admin/staff/IsExistEmailAddress?email" + email).then(function (res) {
        if (res) {
            return true;
        } else {
            return false;
        }
    }).fail(function (error) {
        return false;
    });
}

function clearFilter() {
    $('#table').bootstrapTable('destroy').bootstrapTable(); 
}

function datepickerTemplate() {
    return [
        '<a class="like" href="javascript:void(0)" title="Like">',
        '<i class="fa fa-heart"></i>',
        '</a>  ',
        '<a class="remove" href="javascript:void(0)" title="Remove">',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
}

function ajaxRequest(params) {
    var url = "/Admin/Staff/GetAllAsync";
    $.post(url + '?' + $.param(params.data)).then(function (res) {
        params.success(res);
    }).fail(function (error) {
        toastr.error(error.responseJSON.Message);
    });
}

function imageFormatter(value, row, index) {
    return '<img id="blah" style="border-radius: 50%;" src="' + row.imagePath + '?=' + Math.random() + '" alt="' + row.firstName + '" width="45" height="45" onError="this.onerror=null;this.src=\'' + defaultImageUrl + '\'"/>'
}

function dateOfBirthFormatter(value, row, index) {
    return moment(row.dateOfBirth).format('DD-MMM-YYYY');
}

function activeDeactiveFormatter(value, row, index) {
    var is_checked = row.isActive ? 'checked' : '';

    return '<div class="form-group">'
        + '<div class="custom-control custom-switch custom-switch-off-danger custom-switch-on-success">'
        + '<input type="checkbox" class="custom-control-input" id="user_active_' + row.userId + '" ' + is_checked + '>'
        + '<label class="custom-control-label" for="user_active_' + row.userId + '" onclick="ChangeUserStatus(' + row.userId + ')">' + (row.isActive ? 'Active' : 'Deactive') + '</label>'
        + '</div>'
        + '</div>';
}

function operateFormatter(value, row, index) {
    return [
        '<a class="btn-action-success" href="javascript:void(0)" title="Edit" onclick=manageModal(' + row.userId + ')>',
        '<i class="fas fa-edit"></i>',
        '</a>',
        '<a class="btn-action-danger" href="javascript:void(0)" title="Remove" onclick=OpenDeleteModal("'+row.userId+'")>',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
}

function manageModal(id) {
    $('#btn-reset').css('display', 'block');
    $('#manage-modal-title').text('Add Staff');
    if (Number(id) > 0) {
        $('#btn-reset').css('display', 'none');
        $('#manage-modal-title').text('Edit Staff');
    }
    $('#page-form-content').load(adminBaseUrl + "/Admin/Staff/ManageComponent?id=" + id)
    $('#modal-manage-staff').modal('show');
}

function OpenDeleteModal(userId) {
    selectUserId = userId;
    $('#confirm-delete-modal').modal('show');
}

function DeleteSingle() {    
    var url = adminBaseUrl + "/Admin/User/DeleteAsync";
    $.post(url + '?id=' + selectUserId).then(function (res) {
        if (res.status) {
            $table.bootstrapTable('refresh');
            toastr.success(res.message);
            $remove.prop('disabled', true);
            $('#confirm-delete-modal').modal('hide');
        } else {
            toastr.error(res.message);
        }
    }).fail(function (error) {
        toastr.error(error.responseJSON.Message);
    });
}

function DeleteMultiple() {
    var ids = getIdSelections()
    var url = adminBaseUrl + "/Admin/User/DeleteMultipleAsync";
    $.post(url + '?ids=' + ids).then(function (res) {
        if (res.status) {
            $table.bootstrapTable('refresh');
            toastr.success(res.message);
            $remove.prop('disabled', true)
            $('#confirm-multidelete-modal').modal('hide');
        } else {
            toastr.error(res.message);
        }
    }).fail(function (error) {
        toastr.error(error.responseJSON.Message);
    });
}

function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) {
        return row.userId
    })
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.userId, selections) !== -1
    })
    return res
}

$table.on('check.bs.table uncheck.bs.table ' +
    'check-all.bs.table uncheck-all.bs.table',
    function () {
        $remove.prop('disabled', !$table.bootstrapTable('getSelections').length)
        selections = getIdSelections()
    })

$remove.click(function () {
    $('#confirm-multidelete-modal').modal('show');
})

function ChangeUserStatus(id) {
	$.ajax({
		url: adminBaseUrl + '/Admin/User/IsActiveUser',
		type: "POST",
		data: {
			userId: id,
			isActive: document.getElementById("user_active_" + id).checked ? 0 : 1
		},
		success: function (response) {
			if (response.status) {
				//Load();
				//$(".modal").modal('hide');
                $dataTable.api().ajax.reload();
				toastr.success(response.message, "Success");
			} else {
				toastr.error("Something went to wrong!", "Error");
			}
		}, error: function (jqXHR) {
			console.log(jqXHR);
		}
	});
}
