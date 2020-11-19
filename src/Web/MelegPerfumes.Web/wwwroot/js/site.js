$(document).ready(function () {
    function open() {
        let id = $(this).attr('data-youtube-id');
        let autoplay = '?autoplay=1';
        let related_no = '&rel=0';
        let src = '//www.youtube.com/embed/' + id + autoplay + related_no;

        $("#youtube").attr('src', src);
        return false;
    }

    $('img.video-thumb').click(function () {

        let id = $(this).attr('data-youtube-id');
        let autoplay = '?autoplay=1';
        let related_no = '&rel=0';
        let src = '//www.youtube.com/embed/' + id + autoplay + related_no;

        $("#youtube").attr('src', src);
        return false;
    });

    function toggle_video_modal() {
        $(".js-trigger-modal").on("click", function (event) {
            event.preventDefault();
            $("article").addClass("show-video-modal");
        });

        $('article').on('click', '.close-video-modal, .video-modal .overlay', function (event) {
            event.preventDefault();

            $("article").removeClass("show-video-modal");
            $("#youtube").attr('src', '');
        });
    }

    toggle_video_modal();

    $(window).bind("scroll", function () {
        if ($(window).scrollTop() > 0) {
            $(".layout-navbar").addClass("white");
            $(".alert-warning").addClass("box-shadow");
            $("#back-to-top-arrow").addClass("visible");
        } else {
            $(".layout-navbar").removeClass("white");
            $(".alert-warning").removeClass("box-shadow");
            $("#back-to-top-arrow").removeClass("visible");
        }
    });

    setTimeout(function () {
        $("#response-message").fadeOut("slow").empty();
    }, 5000);

    $("#navbar-toggler").click(function () {
        $(".layout-navbar").addClass("white");
    });

    $("#back-to-top-arrow").click(function (event) {
        event.preventDefault();

        $("html,body").animate({ scrollTop: 0 }, 1000);
    });

    //arrow-down-home-page
    $("a[href^='#']").click(function (event) {
        event.preventDefault();

        let target = $($(this).attr("href"));

        if (target.length) {
            $("html,body").animate({ scrollTop: target.offset().top }, 1000);
        }
    });

    // momment.js
    $("time").each(function (i, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        const time = moment.utc(dateTimeValue).local();
        $(e).html(time.format("llll"));
        $(e).attr("title", $(e).attr("datetime"));
    });
});