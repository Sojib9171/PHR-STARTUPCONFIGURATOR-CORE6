import $ from 'jquery';

$(window).on('resize', function () {
    var win = $(this);
    if (win.width() < 768) {

        $('body').addClass('sidebar-hidden');
        $('body').removeClass("sidebar-open");

    } else {
        $('body').removeClass('sidebar-hidden');
        $('body').addClass("sidebar-open");
    }

});

$(document).ready(function () {
    var win = $(this);
    if (win.width() < 768) {

        $('body').addClass('sidebar-hidden');

    } else {
        $('body').removeClass('sidebar-hidden');
    }
    // minimize menu
    $('.menu-toggle, .sidebar-close').on('click', function (e) {
        $(this).toggleClass("sidebar-hidden");
        $(this.parentNode).toggleClass("sidebar-hidden");
        $('body').toggleClass("sidebar-hidden");
        e.stopPropagation()
    });

});