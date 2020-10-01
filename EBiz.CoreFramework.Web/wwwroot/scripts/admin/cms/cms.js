
//Permissions
let is_edit = $("#hidden_is_edit").val() == "1" ? true : false;
let is_delete = $("#hidden_is_delete").val() == "1" ? true : false;

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
    $.post('/Admin/Page/GetAllAsync?' + $.param(params.data)).then(function (res) {
        params.success(res);
    }).fail(function (error) {

        toastr.error(error.responseJSON.Message);
    });
}

function operateFormatter(value, row, index) {
    var actionFormat = '';
    if(is_edit) {
        actionFormat += '<a class="btn-action-success" href="javascript:void(0)" title="Edit" onclick=ManagePageModal(' + row.pageId + ')><i class="fas fa-edit"></i></a>';
    }

    if (is_delete) {
        actionFormat += '<a class="btn-action-danger" href="javascript:void(0)" title="Remove" onclick=OpenDeleteModal("' + row.pageId + '")><i class="fa fa-trash"></i></a>';
    }

    return [
        actionFormat
    ].join('')
}

function OpenDeleteModal(userId) {
    selectedDeleteId = userId;
    $('#confirm-delete-modal').modal('show');
}

function DeleteSingle() {
    var url = adminBaseUrl + "/Admin/Page/DeleteAsync";
    $.post(url + '?id=' + selectedDeleteId).then(function (res) {
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
    var url = adminBaseUrl + "/Admin/Page/DeleteMultipleAsync";
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
        return row.pageId
    })
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.pageId, selections) !== -1
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


function ManagePageModal(id) {
    $('#btn-reset').css('display', 'block');
    $('#manage-modal-title').text('Add Page');
    if (Number(id) > 0) {
        $('#btn-reset').css('display', 'none');
        $('#manage-modal-title').text('Edit Page');
    }
    $('#page-form-content').load(adminBaseUrl + "/Admin/Page/ManagePageComponent?id=" + id)
    $('#modal-manage-page').modal('show');
}

function FormModelValidator(){
	$('#form-model').bootstrapValidator({
		fields: {
			PageTitle: {
				validators: {
					notEmpty: {
						message: 'Page Title is required.'
					}
				}
			},
			PageUrl: {
				validators: {
					notEmpty: {
						message: 'Page Url is required.'
					},
					regexp: {
						regexp: /^\/[a-zA-Z0-9_-]+$/i,
						message: 'Invalid Page Url'
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
			PageId: $('#hidden_PageId').val(),
			PageTitle: $('#PageTitle').val(),
			PageUrl: $('#PageUrl').val(),
            PageDescription: $("#PageDescription").val(),
			IsActive: $("#is_status")[0].checked ? 1 : 0
		};

		formData.append("model", JSON.stringify(model));
		formData.append("img", null);
		console.log(model);
		$.ajax({
			url: adminBaseUrl + "/Admin/Page/SaveAsync",
			type: "POST",
			contentType: false,
			data: formData,
			cache: false,
			processData: false,
            success: function (response) {
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

function ResetForm() {
	$('#form-model').bootstrapValidator('resetForm', true);
}