
//declair variable
let $formObject = $('#form-model');
let $submitForm = $('#submit-form');
let $resetForm = $('#reset-form');
let $imageInput = $("#ImageFile");
let $imagePreview = $("#image-preview");


//Initialization
$(document).ready(function () {
    formValidation();
    bsCustomFileInput.init();
    //Money Euro
    $('[data-mask]').inputmask()
});


//events
$submitForm.click(function () {
    formValidation()
    $formObject.submit(function (ev) { ev.preventDefault(); });
    $formObject.data('bootstrapValidator').validate();
    var isValid = $formObject.data('bootstrapValidator').isValid();

    if (isValid) {
        var formData = new FormData();
        var model = {
            company_id: $('#hidden_company_id').val(),
            company_name: $('#company_name').val(),
            email_address: $('#email_address').val(),
            contact_number: $('#contact_number').val(),
            whatsapp_number: $('#whatsapp_number').val(),
            address: $('#address').val(),
            country: $('#country').val(),
            state: $('#state').val(),
            city: $('#city').val(),
            pincode: $('#pincode').val(),
            website_url: $('#website_url').val(),
            facebook_url: $('#facebook_url').val(),
            google_url: $('#google_url').val(),
            twitter_url: $('#twitter_url').val(),
            instagram_url: $('#instagram_url').val(),
            youtube_url: $('#youtube_url').val(),
            company_logo: $('#company_logo').val(),
        };

        formData.append("model", JSON.stringify(model));
        formData.append("img", $("#ImageFile")[0].files[0]);

        console.log(model);
        $.ajax({
            url: adminBaseUrl + "/Admin/Company/SaveAsync",
            type: "POST",
            contentType: false,
            data: formData,
            cache: false,
            processData: false,
            success: function (response) {
                if (response.status) {
                    toastr.success(response.message);
                    setTimeout(function () { window.location.href = adminBaseUrl + "/Admin/Company"; }, 2000);
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

$resetForm.click(function () { $formObject.bootstrapValidator('resetForm', true); });

function formValidation() {
    $formObject.bootstrapValidator({
        fields: {
            company_name: {
                validators: {
                    notEmpty: {
                        message: 'Company Name is required.'
                    }
                }
            },
            email_address: {
                validators: {
                    notEmpty: {
                        message: 'Email Address is required.'
                    },
                    emailAddress: {
                        message: 'Invalid Email Address.'
                    }
                }
            },
            contact_number: {
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
            whatsapp_number: {
                validators: {
                    phone: {
                        country: 'US',
                        message: 'Invalid Whatsapp Number'
                    }
                }
            },
            address: {
                validators: {
                    notEmpty: {
                        message: 'Address is required.'
                    },
                }
            },
            country: {
                validators: {
                    notEmpty: {
                        message: 'Country is required.'
                    }
                }
            },
            state: {
                validators: {
                    notEmpty: {
                        message: 'State is required.'
                    }
                }
            },
            city: {
                validators: {
                    notEmpty: {
                        message: 'City is required.'
                    }
                }
            },
            pincode: {
                validators: {
                    notEmpty: {
                        message: 'Pincode is required.'
                    }
                }
            },
            website_url: {
                validators: {
                    uri: {
                        message: 'Invalid Website link'
                    }
                }
            },
            facebook_url: {
                validators: {
                    uri: {
                        message: 'Invalid facebook link'
                    }
                }
            },
            google_url: {
                validators: {
                    uri: {
                        message: 'Invalid google plus link'
                    }
                }
            },
            twitter_url: {
                validators: {
                    uri: {
                        message: 'Invalid twitter link'
                    }
                }
            },
            instagram_url: {
                validators: {
                    uri: {
                        message: 'Invalid instagram link'
                    }
                }
            },
            youtube_url: {
                validators: {
                    uri: {
                        message: 'Invalid youtube link'
                    }
                }
            },
        }
    });
}

$imageInput.change(function () { readURLEmail(this); });

function readURLEmail(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $imagePreview.css('background-image', 'url(' + e.target.result + ')');
            $imagePreview.hide();
            $imagePreview.fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}