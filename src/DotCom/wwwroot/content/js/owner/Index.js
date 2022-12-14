$(function () {
    $container = $("#oa-owner-overview-container");

    $container.isotope({
        itemSelector: '.mdl-card',
        masonry: {
            isFitWidth: true
        }
    });

    setTimeout(function () {
        $container.isotope('layout');
    }, 100);

    $("#resetFilter").click(function () {
        $container.isotope({ filter: '*' });
    });

    $("#filterByGreen").click(function () {
        $container.isotope({ filter: '.oa-owner-overview-container__status-indicator--green, .oa-owner-overview-container__status-indicator--lightgrey' });
    });

    $("#filterByRed").click(function () {
        $container.isotope({ filter: '.oa-owner-overview-container__status-indicator--red, .oa-owner-overview-container__status-indicator--yellow' });
    });
});

var masonryUpdate = function () {
    setTimeout(function () {
        $('#oa-owner-overview-container').isotope('layout')
    }, 0);
}

$(document).on('click', masonryUpdate);
$(document).ajaxComplete(masonryUpdate);