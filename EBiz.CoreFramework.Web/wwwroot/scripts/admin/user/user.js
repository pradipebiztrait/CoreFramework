
//Local variable
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
    
});

//function queryParam(params) {
//    debugger;
//    var q = {
//        "limit": params.pageSize,
//        "offset": params.pageSize * (params.pageNumber - 1),
//        "search": params.searchText,
//        "sort": params.sortName
//    };
//    return q;
//}

function clearFilter() {
    //$('#table').bootstrapTable('clearFilterControl');
    //$('#table').bootstrapTable("resetSearch", "");
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
    var url = "/Admin/User/GetAllAsync";
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
        '<a class="btn-action-danger" href="javascript:void(0)" title="Remove" onclick=OpenDeleteModal("'+row.userId+'")>',
        '<i class="fa fa-trash"></i>',
        '</a>'
    ].join('')
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