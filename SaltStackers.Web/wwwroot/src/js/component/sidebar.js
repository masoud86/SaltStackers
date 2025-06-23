$(document).on('click', '.sidebar-dropdown > a', function () {
    const $sidebarSubmenu = $(this).next(".sidebar-submenu");
    const $parentDropdown = $(this).parent();

    $(".sidebar-submenu").not($sidebarSubmenu).slideUp(200);

    if ($parentDropdown.hasClass("active")) {
        $(".sidebar-dropdown").removeClass("active");
        $parentDropdown.removeClass("active");
    } else {
        $(".sidebar-dropdown").removeClass("active");
        $sidebarSubmenu.slideDown(200);
        $parentDropdown.addClass("active");
    }
});

$(document).on('click', '.toggle-menu', function (e) {
    $(this).toggleClass("is-active");
    $('body').toggleClass('has-sidebar');

    const sidebarState = $('body').hasClass('has-sidebar') ? 'show' : 'hide';
    window.setCookie('sidebar', sidebarState, 9999);
});

function hamburgerMenuToggle() {
    var currentWidth = $(window).width();
    var $toggleMenu = $('.toggle-menu');
    if (currentWidth < 992 && $toggleMenu.hasClass('is-active')) {
        $toggleMenu.trigger('click');
    }
}

hamburgerMenuToggle();

$(window).on('resize', hamburgerMenuToggle);