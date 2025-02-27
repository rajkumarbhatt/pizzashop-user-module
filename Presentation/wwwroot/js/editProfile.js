$(document).ready(function () {
    console.log('Edit profile script loaded');
    
    // Get states based on selected country
    $('#select-country').change(function () {
        // empty the state and city dropdowns
        $('#select-state').empty();
        $('#select-city').empty();
        $('#select-state').append($('<option>').text('Select State').val(''));
        $('#select-city').append($('<option>').text('Select City').val(''));

        var countryId = $(this).val();
        $.ajax({
            url: '/Profile/GetStates',
            type: 'GET',
            data: { countryId: countryId },
            success: function (data) {
                $.each(data, function (i, state) {
                    $('#select-state').append($('<option>').text(state.name).val(state.id));
                });
            }
        });
    });
        
    // Get cities based on selected state
    $('#select-state').change(function () {
        var stateId = $(this).val();
        $.ajax({
            url: '/Profile/GetCities',
            type: 'GET',
            data: { stateId: stateId },
            success: function (data) {
                $('#select-city').empty();
                $('#select-city').append($('<option>').text('Select City').val(''));
                $.each(data, function (i, city) {
                    $('#select-city').append($('<option>').text(city.name).val(city.id));
                });
            }
        });
    });

    
    
    // submit form
    $('#edit-profile-form').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        var data = form.serialize();
        console.log(data);
        $.ajax({
            url: '/Profile/EditProfile',
            type: 'POST',
            data: data,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    setTimeout(function () {
                        window.location.href = '/Profile';
                    }, 1000);
                } else {
                    toastr.error(data.message);
                }
            },
            error: function (data) {
                toastr.error('An error occurred. Please try again.');
            }
        });
    });

});    
