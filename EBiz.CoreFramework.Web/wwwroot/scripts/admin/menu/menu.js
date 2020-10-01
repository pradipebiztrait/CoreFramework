
//Local variable
var $table = $('#table')
var $remove = $('#remove')
var selections = [];
var selectedDeleteId = 0;

$(document).ready(function () {
    $table.bootstrapTable('updateFormatText', 'formatSearch', 'Enter Search keywords...')
    FormModelValidator();
});

function clearFilter() {
    $('#table').bootstrapTable('destroy').bootstrapTable();
}

//new code
function ajaxRequest(params) {
    $.post('/Admin/Menu/GetAllAsync?' + $.param(params.data)).then(function (res) {
        params.success(res);
    }).fail(function (error) {
        toastr.error(error.responseJSON.Message);
    });
}

function operateFormatter(value, row, index) {
    return [
        '<a class="btn-action-success" href="javascript:void(0)" title="Edit" onclick=manageModal(' + row.menu_id + ')>',
        '<i class="fas fa-edit"></i>',
        '</a>',
        '<a class="btn-action-danger" href="javascript:void(0)" title="Remove" onclick=openDeleteModal("' + row.menu_id + '")>',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
}

function openDeleteModal(userId) {
    selectedDeleteId = userId;
    $('#confirm-delete-modal').modal('show');
}

function DeleteSingle() {
    var url = adminBaseUrl + "/Admin/Menu/DeleteAsync";
    $.post(url + '?id=' + selectedDeleteId).then(function (res) {
        if (res.status) {
            $table.bootstrapTable('refresh');
            toastr.success(res.message);
            $remove.prop('disabled', true);
            $('#confirm-delete-modal').modal('hide');
        } else {
            toastr.error(res.message);
        }
    })
}

function DeleteMultiple() {
    var ids = getIdSelections()
    var url = adminBaseUrl + "/Admin/Menu/DeleteMultipleAsync";
    $.post(url + '?ids=' + ids).then(function (res) {
        if (res.status) {
            $table.bootstrapTable('refresh');
            toastr.success(res.message);
            $remove.prop('disabled', true)
            $('#confirm-multidelete-modal').modal('hide');
        } else {
            toastr.error(res.message);
        }
    })
}

function getIdSelections() {
    return $.map($table.bootstrapTable('getSelections'), function (row) { return row.menu_id })
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) { row.state = $.inArray(row.menu_id, selections) !== -1 })
    return res
}

$table.on('check.bs.table uncheck.bs.table ' +
    'check-all.bs.table uncheck-all.bs.table',
    function () {
        $remove.prop('disabled', !$table.bootstrapTable('getSelections').length)
        selections = getIdSelections()
    })

$remove.click(function () { $('#confirm-multidelete-modal').modal('show'); })

function manageModal(id) {
    $('#btn-reset').css('display', 'block');
    $('#manage-modal-title').text('Add Menu');
    if (Number(id) > 0) {
        $('#btn-reset').css('display', 'none');
        $('#manage-modal-title').text('Edit Menu');
    }
    $('#page-form-content').load(adminBaseUrl + "/Admin/Menu/ManageComponent?id=" + id)
    $('#modal-manage-page').modal('show');
}

//old code

function FormModelValidator(){
	$('#form-model').bootstrapValidator({
		fields: {
			menu_title: {
				validators: {
					notEmpty: {
						message: 'Menu Title is required.'
					}
				}
			},
			menu_url: {
				validators: {
					notEmpty: {
						message: 'Menu Url is required.'
					},
					regexp: {
						regexp: /^\/[a-zA-Z0-9_-]+$/i,
						message: 'Invalid Page Url'
					}
				}
            },
            menu_icon: {
                validators: {
                    notEmpty: {
                        message: 'Menu Icon is required.'
                    }
                }
            },
            sort_order: {
                validators: {
                    notEmpty: {
                        message: 'Sort Order is required.'
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
			menu_id: $('#hidden_menu_id').val(),
			menu_title: $('#menu_title').val(),
			menu_url: $('#menu_url').val(),
            menu_icon: $('#menu_icon').val(),
            sort_order: $('#sort_order').val(),
            parent_menu_id: document.getElementById("ddlParentMenuId").options[document.getElementById("ddlParentMenuId").selectedIndex].value,
            is_active: $("#is_status")[0].checked ? 1 : 0
		};

		formData.append("model", JSON.stringify(model));
		formData.append("img", null);
		console.log(model);
		$.ajax({
			url: adminBaseUrl + "/Admin/Menu/SaveAsync",
			type: "POST",
			contentType: false,
			data: formData,
			cache: false,
			processData: false,
            success: function (response) {
                debugger;
                console.log(response);
				if (response.status) {
                    $table.bootstrapTable('refresh');
                    toastr.success(response.message);
                    $remove.prop('disabled', true)
                    $('#modal-manage-page').modal('hide');
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

function ResetForm() { $('#form-model').bootstrapValidator('resetForm', true); }