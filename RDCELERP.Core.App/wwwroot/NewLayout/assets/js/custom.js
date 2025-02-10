
$("#collapsebtn").click(function () {
    $('main').toggleClass('toggleSidebarTrue');
    $('#sidebarMenu').toggleClass('toggleSidebar');
    $('#collapsebtn').toggleClass('collapsebtnactive');
    $('.logowrapbox').addClass('logowrapboxsm');
    $('.sidebar .nav-link + .multi-level').removeClass('show');
    $('.sidebar .nav-link[data-bs-toggle=collapse][aria-expanded=true] .link-arrow').css('transform', 'rotate(0deg)')
});
// Add click event listener to all nav-link elements

//$("#sidebarMenu").mouseenter(function () {
//    if ($(this).hasClass("toggleSidebar")) {
//        $('main').removeClass('toggleSidebarTrue');
//        $(this).removeClass('toggleSidebar');
//        $('.logowrapbox').removeClass('logowrapboxsm');
//    }
//});

$("#sidebarMenu").mouseleave(function () {
    if ($("#collapsebtn").hasClass("collapsebtnactive")) {
        $('main').addClass('toggleSidebarTrue');
        $(this).addClass('toggleSidebar');
        $('.sidebar .nav-link + .multi-level').removeClass('show');
        $('.sidebar .nav-link[data-bs-toggle=collapse][aria-expanded=true] .link-arrow').css('transform', 'rotate(0deg)');
        $('.logowrapbox').addClass('logowrapboxsm');
    }
});

$("img.img-responsive").each(function () {
    var img = $(this);
    var container = $("<div class='responsive-img-container'></div>");
    img.wrap(container);
});

$(document).on("click", "img.img-responsive", function () {
    $("img.img-responsive").not(this).removeClass("FullWidthIMG");
    $(this).toggleClass("FullWidthIMG");
    $('#IMGBGWrapContainer').toggleClass('IMGBGWrap');
    $('#CloseBTNTop').toggleClass('closebtntop');
});

$('#CloseBTNTop').click(function () {
    $('#IMGBGWrapContainer').toggleClass('IMGBGWrap');
});

$(document).ready(function () {
    var IMGBGWrap = $('<div id="IMGBGWrapContainer"></div>');
    $('body').append(IMGBGWrap);
    var CloseBTNTop = $('<div id="CloseBTNTop"><i class="fa-solid fa-xmark"></i></div>');
    $('body').append(CloseBTNTop);
});

$(document).on('click', '.close-button', function () {
    $(this).parent().removeClass("FullWidthIMG");
    $(this).remove();
});

$(document).on('click', function (event) {
    if (!$(event.target).closest('.img-responsive').length) {
        $("img.img-responsive").removeClass("FullWidthIMG");
        $('#IMGBGWrapContainer').removeClass('IMGBGWrap');
        $('#CloseBTNTop').removeClass('closebtntop');
    }
});

$(document).ready(function () {
    
    $('.autocomplete').wrap('<div class="autocompletewrap"></div>');

    $('.autocomplete').click(function () {
        $('.autocompletewrap').addClass('autocompletewrap-open');
    });

    $('#loader').fadeOut();


    if ($('input[type=checkbox]').val() == 'true') {
        $(this).prop('checked', true);
    }

});

$(document).mouseup(function (e) {
    $('.autocompletewrap').removeClass('autocompletewrap-open');
});

$('.autocomplete').click(function () {
    $('.autocompletewrap').addClass('autocompletewrap-open');
});

$(document).ajaxStart(function () {
    $("#loader").show();
});

$(document).ajaxStop(function () {
    $("#loader").hide();
});

//List Page Common JS By PS 21.03.2030
$('.filterbox').hide();

$('.filterheading input[type=checkbox]').click(function () {
    $(this).parent().next('.filterbox').toggle();
});

$('#closefilters').click(function () {
    $('.filters').hide();
    $('.content').toggleClass('maincontent');
});

$(document).ready(function () {
    var currentUrl = window.location.href;

    $('.nav-link').each(function () {
        if ($(this).attr('href') === currentUrl) {
            $(this).parents('.nav-item').find('span.nav-link').addClass('active');
        }
    });
});