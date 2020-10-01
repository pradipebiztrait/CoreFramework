
//declair variable
let $table = $('#table');
let $remove = $('#remove');
let $submitForm = $('#submit-form');
let $submitRolePermissionForm = $('#submit-rolepermission-form');
let $resetForm = $('#reset-form');
let $clearFilter = $('#clear-filter');
let $modalRolePermission = $('#modal-role-permission');
let rolePermissionObject = {};
let selections = [];
let selectedDeleteId = 0;


//Initialization
$(document).ready(function () {
    $table.bootstrapTable('updateFormatText', 'formatSearch', 'Enter Search keywords...')
    FormModelValidator();
});


//events
$table.on('check.bs.table uncheck.bs.table ' + 'check-all.bs.table uncheck-all.bs.table', function () {
    $remove.prop('disabled', !$table.bootstrapTable('getSelections').length)
    selections = getIdSelections()
})

$remove.click(function () {
    $('#confirm-multidelete-modal').modal('show');
})

$submitForm.click(function () {
    FormModelValidator();
    $('#form-model').submit(function (ev) { ev.preventDefault(); });
    $('#form-model').data('bootstrapValidator').validate();
    var isValid = $('#form-model').data('bootstrapValidator').isValid();

    if (isValid) {
        var formData = new FormData();
        var model = {
            role_id: $('#hidden_role_id').val(),
            role_name: $('#role_name').val().trim()
        };

        formData.append("model", JSON.stringify(model));
        formData.append("img", null);
        $.ajax({
            url: adminBaseUrl + "/Admin/Role/SaveAsync",
            type: "POST",
            contentType: false,
            data: formData,
            cache: false,
            processData: false,
            success: function (response) {
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
})

$resetForm.click(function () {
    FormModelValidator();
    $('#form-model').bootstrapValidator('resetForm', true);
});

$clearFilter.click(function () {
    $table.bootstrapTable('destroy').bootstrapTable();
});

$submitRolePermissionForm.click(function () {
    for (var i = 0; i < rolePermissionObject.length; i++) {
        rolePermissionObject[i].is_view = Number($('#' + rolePermissionObject[i].menu_id + '_is_view')[0].checked == true ? 1 : 0)
        rolePermissionObject[i].is_add = Number($('#' + rolePermissionObject[i].menu_id + '_is_add')[0].checked == true ? 1 : 0)
        rolePermissionObject[i].is_edit = Number($('#' + rolePermissionObject[i].menu_id + '_is_edit')[0].checked == true ? 1 : 0)
        rolePermissionObject[i].is_delete = Number($('#' + rolePermissionObject[i].menu_id + '_is_delete')[0].checked == true ? 1 : 0)
    }
    console.log(rolePermissionObject);

    var formData = new FormData();
    var model = rolePermissionObject;

    formData.append("model", JSON.stringify(model));

    $.ajax({
        url: adminBaseUrl + "/Admin/Role/SaveRolePermissionAsync",
        type: "POST",
        contentType: false,
        data: formData,
        cache: false,
        processData: false,
        success: function (response) {
            if (response.status) {
                toastr.success(response.message);
                $modalRolePermission.modal('hide');
            } else {
                toastr.error(response.message, "Error");
            }
            console.log(response);
        }, error: function (jqXHR) {
            toastr.error(jqXHR.responseJSON.Message, "Error");
        }
    });
});


//functions
function ajaxRequest(params) {
    $.post('/Admin/Role/GetAllAsync?' + $.param(params.data)).then(function (res) {
        params.success(res);
    }).fail(function (error) {
        toastr.error(error.responseJSON.Message);
    });
}

function stateFormatter(value, row, index) {
    if (row.role_id === 1 || row.role_id === 2) {
        return {
            disabled: true
        }
    }
    //if (index === 5) {
    //    return {
    //        disabled: true,
    //        checked: true
    //    }
    //}
    return value
}

function operateFormatter(value, row, index) {
    var disbaledClass = "";
    if (row.role_id === 1 || row.role_id === 2) {
        disbaledClass += 'disabled';
    }

    return [
        '<a class="btn-action-primary" href="javascript:void(0)" title="Role Permissions" data-role_id="' + row.role_id + '" data-role_name="' + row.role_name +'" onclick=manageRolePermissions(this)>',
        '<i class="fas fa-user-cog"></i>',
        '</a>',
        '<a class="btn-action-success ' + disbaledClass +'" href="javascript:void(0)" title="Edit" onclick=manageModal(' + row.role_id + ')>',
        '<i class="fas fa-edit"></i>',
        '</a>',
        '<a class="btn-action-danger ' + disbaledClass +'" href="javascript:void(0)" title="Delete" onclick=openDeleteModal("' + row.role_id + '")>',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
}

function openDeleteModal(id) {
    selectedDeleteId = id;
    $('#confirm-delete-modal').modal('show');
}

function DeleteSingle() {
    var url = adminBaseUrl + "/Admin/Role/DeleteAsync";
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
    var url = adminBaseUrl + "/Admin/Role/DeleteMultipleAsync";
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
    return $.map($table.bootstrapTable('getSelections'), function (row) { return row.role_id })
}

function responseHandler(res) {
    $.each(res.rows, function (i, row) { row.state = $.inArray(row.role_id, selections) !== -1 })
    return res
}

function manageModal(id) {
    $('#btn-reset').css('display', 'block');
    $('#manage-modal-title').text('Add Role');
    if (Number(id) > 0) {
        $('#btn-reset').css('display', 'none');
        $('#manage-modal-title').text('Edit Role');
    }
    $('#page-form-content').load(adminBaseUrl + "/Admin/Role/ManageComponent?id=" + id)
    $('#modal-manage-page').modal('show');
}

function manageRolePermissions(e) {
    $.post('/Admin/Role/GetUserPermissionAsync?id=' + e.dataset.role_id).then(function (res) {
        if (res.status) {
            rolePermissionObject = res.data;
            console.log(rolePermissionObject);
            $('#modal-rolepermission-content').load(adminBaseUrl + "/Admin/Role/ManageRolePermissionComponent?id=" + e.dataset.role_id);
            $('#modal-rolepermission-title').text(e.dataset.role_name + ' Role Permission');
            $modalRolePermission.modal('show');
            $tableRolePermission = $('#table-role-permission');1
        } else {
            toastr.error(res.message);
        }        
    })  
    
}

function FormModelValidator() {
    $('#form-model').bootstrapValidator({
        fields: {
            role_name: {
                validators: {
                    notEmpty: {
                        message: 'Role name is required.'
                    }
                }
            }
        }
    });
}

