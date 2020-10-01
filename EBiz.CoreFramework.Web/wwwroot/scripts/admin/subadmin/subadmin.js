
//Local variable
var $dataTable = $('#dataTable');

$(document).ready(function () {
    //Init methods
	Load();
	$('#add_btn_area').html('<button type="button" class="btn btn-info-custom" onclick="ManageModule(0)"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp;Add Staff Member</button>');
	$("#searchString").keypress(function (event) { if (event.which == '13') { event.preventDefault(); } });
	FormModelValidator();
});

function Load() {
	$dataTable.dataTable({
        "ajax": {
			"url": adminBaseUrl + "/Admin/SubAdmin/GetAll",
            "type": "POST",
            "cache": false,
            "datatype": "json"
        },
        "bLengthChange": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "bLengthChange": true,
        "lengthMenu": [
            [10, 25, 50, -1],
            [10, 25, 50, "All"]
        ],
        "columnDefs": [
            { "width": "5%", "targets": 0 },
            { "width": "30%", "targets": 1 },
            { "width": "25%", "targets": 2 },
            { "width": "10%", "targets": 3 },
			{ "width": "15%", "targets": 4 },
			{ "width": "15%", "targets": 5 },
        ],
        "columns": [
            {
                "title": "Image",
                "sClass": "centerc",
                "ordering": false,
                "render": function (data, type, row) {
					return '<img id="blah" src="' + row.imagePath + '?=' + Math.random() + '" alt="' + row.firstName + '" width="55" height="55" onError="this.onerror=null;this.src=\'' + defaultImageUrl + '\'"/>'
                }
            },
			{
				"title": "Name",
				"autoWidth": true,
				"bSortable": true,
				"render": function (data, type, row) {
					return row.firstName + " " + row.lastName;
				}
			},
			{
				"data": "emailAddress",
				"title": "Email",
				"autoWidth": true,
				"bSortable": true,
			},
            {
				"data": "mobileNumber",
                "title": "Phone Number",
                "autoWidth": true,
                "bSortable": true,
            },
            {
                "title": "Active/Deactive",
                "bSortable": false,
                "sClass": "centerc",
				"render": function (data, type, row) {

					var is_checked = row.isActive ? 'checked' : '';

					return '<div class="active_profile_check d-flex align-items-center justify-content-center">'
						+ '<input type="checkbox" name="IsActive" id="user_active_'+row.userId+'" ' + is_checked + '>'
						+ '<label for="user_active_' + row.userId + '" onclick="ChangeUserStatus(' + row.userId + ')"></label></div>';

                }
			},
			{
				"title": "Action",
				"bSortable": false,
				"sClass": "centerc btn-action",
				"render": function (data, type, row) {
					return '<a onclick = "return ManageModule(' + row.userId + ');" title = "Edit" ><i class="fa fa-pencil-square-o fa-lg text-success" aria-hidden="true"></i></a>' +
						'<a onclick = "return DeleteModule(' + row.userId + ');" title = "Delete" ><i class="fa fa-trash fa-lg text-danger" aria-hidden="true"></i></a>';
				}
			}
        ],
        "language": {
            "emptyTable": "<div class='row'><div class='col-md-12'><div class='alert alert-info'><p><strong>No record found.</strong></p> </div></div>"
        },
        "destroy": true
    });
}

function IsActive(isActive, id) {
    var tl = isActive == 1 ? "DEACTIVE" : "ACTIVE";
    var msg = isActive == 1 ? "deactive" : "active";
    BootstrapDialog.confirm({
        title: '<b>' + tl + ' USER </b>',
        closable: true,
        message: 'Are you sure want to ' + msg + ' this user ?',
        type: BootstrapDialog.TYPE_DANGER,
        btnCancelLabel: 'CANCEL',
        btnCancelClass: 'btn-default-custom',
        btnOKLabel: tl,
        btnOKClass: isActive == 1 ? 'btn-danger-custom' : 'btn-success-custom',
        callback: function (result) {
            if (result) {
                $.ajax({
					url: adminBaseUrl + '/Admin/SubAdmin/IsActiveUser',
                    type: "POST",
                    data: {
                        userId: id,
                        isActive: isActive == 1 ? 0 : 1
                    },
                    success: function (response) {
                        if (response.status) {
                            Load();
                            $(".modal").modal('hide');
                            toastr.success(response.message, "Success");
                        } else {
                            toastr.error("Something went to wrong!", "Error");
                        }
                    }, error: function (jqXHR) {
                        console.log(jqXHR);
                    }
                });
            }
        }
    });
}

function ChangeUserStatus(id) {
	$.ajax({
		url: adminBaseUrl + '/Admin/SubAdmin/IsActiveUser',
		type: "POST",
		data: {
			userId: id,
			isActive: document.getElementById("user_active_" + id).checked ? 0 : 1
		},
		success: function (response) {
			if (response.status) {
				//Load();
				//$(".modal").modal('hide');
				toastr.success(response.message, "Success");
			} else {
				toastr.error("Something went to wrong!", "Error");
			}
		}, error: function (jqXHR) {
			console.log(jqXHR);
		}
	});
}

function SearchString() {
	$('#dataTable').DataTable().search($("#searchString").val()).draw();
}

function ManageModule(id) {
	window.location.href = adminBaseUrl + '/Admin/SubAdmin/Manage/' + id;
}

function DeleteModule(id) {
	BootstrapDialog.confirm({
		title: '<b>Delete Staff Member</b>',
		message: 'Are you sure want to delete this Staff Member User?',
		closable: true,
		type: BootstrapDialog.TYPE_DANGER,
		btnCancelLabel: 'CANCEL',
		btnCancelClass: 'btn-default-custom',
		btnOKLabel: 'DELETE',
		btnOKClass: 'btn-danger-custom',
		callback: function (result) {
			if (result) {
				$.ajax({
					url: adminBaseUrl + '/Admin/SubAdmin/DeleteById?id=' + id,
					type: "GET",
					success: function (response) {
						debugger;
						if (response.status) {
							$dataTable.api().ajax.reload();
							$(".modal").modal('hide');
							toastr.success(response.message, "Success");
						} else {
							toastr.error("Something went to wrong!", "Error");
						}
					}, error: function (jqXHR) {
						console.log(jqXHR);
					}
				});
			}
		}
	});
}

function FormModelValidator() {
	$('#form-model').bootstrapValidator({
		fields: {
			EmailAddress: {
				validators: {
					notEmpty: {
						message: 'Email Address is required.'
					},
					emailAddress: {
						message: 'Invalid Email Address.'
					}
				}
			},
			FirstName: {
				validators: {
					notEmpty: {
						message: 'First Name is required.'
					},
				}
			},
			LastName: {
				validators: {
					notEmpty: {
						message: 'Last Name is required.'
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
						message: 'The password and its confirm are not the same.'
					}
				}
			},
			ConfirmPassword: {
				validators: {
					notEmpty: {
						message: 'Confirm Password is required.'
					},
					identical: {
						field: 'Password',
						message: 'The password and its confirm are not the same.'
					}
				}
			}
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
			UserId: $('#hidden_user_id').val(),
			EmailAddress: $('#EmailAddress').val(),
			FirstName: $('#FirstName').val(),
			LastName: $('#LastName').val(),
			MobileNumber: $('#MobileNumber').val(),
			ConfirmPassword: $('#ConfirmPassword').val(),
			Password: $('#Password').val()
		};

		formData.append("model", JSON.stringify(model));
		formData.append("img", null);

		console.log(model);
		$.ajax({
			url: adminBaseUrl + "/Admin/SubAdmin/SaveAsync",
			type: "POST",
			contentType: false,
			data: formData,
			cache: false,
			processData: false,
			success: function (response) {
				if (response.status) {
					toastr.success(response.message, "Success");
					window.location.href = adminBaseUrl + '/Admin/SubAdmin';
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

function ResetForm() {
	if (Number($('#hidden_user_id').val()) == 0) {
		$('#imagePreview').css('background-image', 'url(/img/disable_logo.svg)');
	}
	
	$('#form-model').bootstrapValidator('resetForm', true);
}

function Back() {
	window.location.href = adminBaseUrl + '/Admin/SubAdmin';
}

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