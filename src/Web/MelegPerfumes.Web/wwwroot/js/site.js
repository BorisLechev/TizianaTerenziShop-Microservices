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
        } else {
            $(".layout-navbar").removeClass("white");
            $(".alert-warning").removeClass("box-shadow");
        }
    });

    setTimeout(function () {
        $(".alert").fadeOut("slow").empty();
    }, 5000);
});