import toastr from 'toastr';
window.toastr = toastr

var dir = $('body').attr('dir');

toastr.options = {
    maxOpened: 0,
    newestOnTop: true,
    preventDuplicates: false,
    closeButton: true,
    preventOpenDuplicates: false,
    target: 'body',
    escapeHtml: true,
    progressBar: true,
    positionClass: dir === 'ltr' ? 'toast-bottom-right' : 'toast-bottom-left',
    rtl: dir === 'ltr' ? false : true
};

var $alert = $('.alert-data'),
    alertType = '',
    alertTitle = '',
    alertMessage = '';

if ($alert.length > 0) {
    alertType = $alert.data('alert-type');
    alertTitle = $alert.data('alert-title');
    alertMessage = $alert.data('alert-message');
    switch (alertType) {
        case 'Success':
            toastr.success(alertMessage, alertTitle);
            break;
        case 'Warning':
            toastr.warning(alertMessage, alertTitle);
            break;
        case 'Error':
            toastr.error(alertMessage, alertTitle);
            break;
        default:
            break;
    }
}