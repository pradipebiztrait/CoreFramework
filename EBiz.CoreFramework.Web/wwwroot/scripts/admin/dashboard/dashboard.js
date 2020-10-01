let selected_duration = 1;
let today = new Date();
let start = new Date();
let end = new Date();
let selected_subscrition = "annual";


$(document).ready(function () {
    $("select.ddl-duration").change(function () {
        selected_duration = $(this).children("option:selected").val();
        Load();
    });

    Load();
});

function Load() {
	$("#dashboard-report-area").html('<br/><div class="row"><div class="col-md-12 text-center">Loading...</div></div></br>');
    $("#dashboard-report-area").load(adminBaseUrl + '/admin/Dashboard/DashboardComponent?duration=' + selected_duration, function (response, status, xhr) { });

}

function ChangesSubscription(val) {
	selected_subscrition = (val.value || val.options[val.selectedIndex].value);
	$("#subscription-tbody").html('<tr><td colspan="3" class="text-center">Loading...</td></tr>');
	$("#subscription-tbody").load(adminBaseUrl + '/admin/Dashboard/DashboardSubscriptionComponent?duration=' + selected_duration + '&subscription=' + selected_subscrition, function (response, status, xhr) { });
}