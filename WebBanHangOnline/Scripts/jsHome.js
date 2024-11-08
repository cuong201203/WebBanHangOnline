$(document).ready(function () {
    var startX, endX;
    var threshold = 100;
    var carousel = $('#customCarousel');
    var productItems = $('.product-grid .product-item');

    // Handle touch events for swipe on mobile
    carousel.on('touchstart', function (e) {
        startX = e.originalEvent.touches[0].clientX;
    });

    carousel.on('touchmove', function (e) {
        endX = e.originalEvent.touches[0].clientX;
        e.preventDefault();
    });

    carousel.on('touchend', function () {
        handleSwipe();
    });

    // Handle mouse events for swipe on desktop
    carousel.on('mousedown', function (e) {
        startX = e.clientX;
    });

    carousel.on('mousemove', function (e) {
        if (startX) {
            endX = e.clientX;
        }
    });

    carousel.on('mouseup', function () {
        handleSwipe();
        startX = null;
    });

    $('.best-seller-img').hover(
        function () {
            var hoverImage = $(this).attr('data-hover');
            $(this).stop(true, true).fadeOut(200, function () {
                $(this).attr('src', hoverImage).fadeIn(200);
            });
        },
        function () {
            var defaultImage = $(this).attr('data-default');
            $(this).stop(true, true).fadeOut(200, function () {
                $(this).attr('src', defaultImage).fadeIn(200);
            });
        }
    );

    function handleSwipe() {
        if (startX && endX) {
            if (endX - startX > threshold) {
                carousel.carousel('prev'); // swipe right to go to the previous item
            } else if (startX - endX > threshold) {
                carousel.carousel('next'); // swipe left to go to the next item
            }
        }
    }

    // Pagination with isotope
    var itemSelector = '.product-item';
    var itemsPerPage = 10;
    var currentPage = 1;
    var currentNumberPages = 1;

    var $container = $('.product-grid').isotope({
        itemSelector: itemSelector,
        animationOptions: {
            duration: 750,
            easing: 'linear',
            queue: false
        }
    });

    function setupPagination() {
        var itemsLength = $container.children(itemSelector).length;
        currentNumberPages = Math.ceil(itemsLength / itemsPerPage);

        var item = 1, page = 1;
        $container.children(itemSelector).each(function () {
            if (item > itemsPerPage) {
                page++;
                item = 1;
            }
            $(this).addClass('page' + page);
            item++;
        });
    }

    function changePage() {
        currentPage = 1;
        var selector = itemSelector + '.page' + currentPage;
        $container.isotope({ filter: selector });        
    }

    setupPagination();
    changePage();

    $('.all-sorting-button').on('click', function () {
        changePage();
    })
})
