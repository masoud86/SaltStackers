$(document).on('click', '.expand-card', function () {
    var $card = $(this).closest('.card');
    $card.find('.slide-card').toggle();
    $card.toggleClass('card-fullscreen');
});

$(document).on('click', '.slide-card', function () {
    var $card = $(this).closest('.card');
    $card.find('.expand-card').toggle();
    $card.find('.card-body').slideToggle();
});