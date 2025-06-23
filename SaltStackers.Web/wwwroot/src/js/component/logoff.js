$(document).on('click', '#logoff', function () {
    $.ajax({
        url: '/Home/SignOut',
        type: 'POST',
        cache: false,
        data: { __RequestVerificationToken: $('#logoff').find('input[name="__RequestVerificationToken"]').val() }
    }).done(function (result) {
        window.location.href = result.url;
    });
});