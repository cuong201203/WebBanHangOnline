$(document).ready(function () {
    var startX, endX;
    var carousel = $('#customCarousel');

    // Handle touch events for swipe on mobile
    carousel.on('touchstart', function (e) {
        startX = e.originalEvent.touches[0].clientX;
    });

    carousel.on('touchmove', function (e) {
        endX = e.originalEvent.touches[0].clientX;
        e.preventDefault(); // Prevent page from scrolling while swiping
    });

    carousel.on('touchend', function () {
        handleSwipe();
    });

    // Handle mouse events for swipe on desktop
    carousel.on('mousedown', function (e) {
        startX = e.clientX;
    });

    carousel.on('mousemove', function (e) {
        if (startX !== undefined) {
            endX = e.clientX;
        }
    });

    carousel.on('mouseup', function () {
        handleSwipe();
    });

    function handleSwipe() {
        if (startX !== undefined && endX !== undefined) {
            if (endX - startX > threshold) {
                carousel.carousel('prev'); // swipe right to go to the previous item
            } else if (startX - endX > threshold) {
                carousel.carousel('next'); // swipe left to go to the next item
            }
        }
    }
})
