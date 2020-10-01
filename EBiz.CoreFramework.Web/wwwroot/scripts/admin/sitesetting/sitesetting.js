//Initialization
let IsEmailActive = 


$(document).ready(function () {    
    $("input[data-bootstrap-switch]").each(function () {
        $(this).bootstrapSwitch('state', $(this).prop('checked'));
    });
    $('#custom-tabs-one-mail').load(adminBaseUrl + "/Admin/Site/EmailSettingComponent");
});


function changeTab(tab) {
    if (tab == 1) {
        $('#custom-tabs-one-mail').load(adminBaseUrl + "/Admin/Site/EmailSettingComponent");
    }
    if (tab == 2) {
        $('#custom-tabs-one-notification').load(adminBaseUrl + "/Admin/Site/PushNotificationSettingComponent");
    }
    if (tab == 3) {
        $('#custom-tabs-one-awsproperty').load(adminBaseUrl + "/Admin/Site/AWSPropertySettingComponent");
    }
}


function IsActiveEmail(e) {
    $.post(adminBaseUrl + "/Admin/Site/IsActiveEmailSetting?status=" + (e.checked ? 1 : 0));
}

function IsActiveNotification(e) {
    $.post(adminBaseUrl + "/Admin/Site/IsActiveNotification?status=" + (e.checked ? 1 : 0));
}