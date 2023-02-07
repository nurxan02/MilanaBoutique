
$(document).ready(function () {
    'use strict';

    owlCarousels();
    quantityInputs();

 

    var $searchWrapper = $('.header-search-wrapper'),
    	$body = $('body'),
        $searchToggle = $('.search-toggle');

	$searchToggle.on('click', function (e) {
		$searchWrapper.toggleClass('show');
		$(this).toggleClass('active');
		$searchWrapper.find('input').focus();
		e.preventDefault();
	});

	$body.on('click', function (e) {
		if ( $searchWrapper.hasClass('show') ) {
			$searchWrapper.removeClass('show');
			$searchToggle.removeClass('active');
			$body.removeClass('is-search-active');
		}
	});

	$('.header-search').on('click', function (e) {
		e.stopPropagation();
	});

	
    var catDropdown = $('.category-dropdown'),
        catInitVal = catDropdown.data('visible');
        
	if ( $('.sticky-header').length && $(window).width() >= 992 ) {
		var sticky = new Waypoint.Sticky({
			element: $('.sticky-header')[0],
			stuckClass: 'fixed',
			offset: -300,
            handler: function ( direction ) {
             
                if ( catInitVal &&  direction == 'up') {
                    catDropdown.addClass('show').find('.dropdown-menu').addClass('show');
                    catDropdown.find('.dropdown-toggle').attr('aria-expanded', 'true');
                    return false;
                }

              
                if ( catDropdown.hasClass('show') ) {
                    catDropdown.removeClass('show').find('.dropdown-menu').removeClass('show');
                    catDropdown.find('.dropdown-toggle').attr('aria-expanded', 'false');
                } 
            }
		});
	}

  
    if ( $.fn.superfish ) {
        $('.menu, .menu-vertical').superfish({
            popUpSelector: 'ul, .megamenu',
            hoverClass: 'show',
            delay: 0,
            speed: 80,
            speedOut: 80 ,
            autoArrows: true
        });
    }


    $('.mobile-menu-toggler').on('click', function (e) {
		$body.toggleClass('mmenu-active');
		$(this).toggleClass('active');
		e.preventDefault();
    });

    $('.mobile-menu-overlay, .mobile-menu-close').on('click', function (e) {
		$body.removeClass('mmenu-active');
		$('.menu-toggler').removeClass('active');
		e.preventDefault();
    });


    $('.mobile-menu').find('li').each(function () {
        var $this = $(this);

        if ( $this.find('ul').length ) {
            $('<span/>', {
                'class': 'mmenu-btn'
            }).appendTo($this.children('a'));
        }
    });

   
    $('.mmenu-btn').on('click', function (e) {
        var $parent = $(this).closest('li'),
            $targetUl = $parent.find('ul').eq(0);

            if ( !$parent.hasClass('open') ) {
                $targetUl.slideDown(300, function () {
                    $parent.addClass('open');
                });
            } else {
                $targetUl.slideUp(300, function () {
                    $parent.removeClass('open');
                });
            }

        e.stopPropagation();
        e.preventDefault();
    });

	
    var $sidebarToggler = $('.sidebar-toggler');
    $sidebarToggler.on('click', function (e) {
		$body.toggleClass('sidebar-filter-active');
		$(this).toggleClass('active');
		e.preventDefault();
    });

    $('.sidebar-filter-overlay').on('click', function (e) {
		$body.removeClass('sidebar-filter-active');
		$sidebarToggler.removeClass('active');
		e.preventDefault();
    });

   
    $('.sidebar-filter-clear').on('click', function (e) {
    	$('.sidebar-shop').find('input').prop('checked', false);

    	e.preventDefault();
    });

  

  
    if ( $.fn.hoverIntent ) {
        $('.product-3').hoverIntent(function () {
            var $this = $(this),
                animDiff = ( $this.outerHeight() - ( $this.find('.product-body').outerHeight() + $this.find('.product-media').outerHeight()) ),
                animDistance = ( $this.find('.product-footer').outerHeight() - animDiff );

            $this.find('.product-footer').css({ 'visibility': 'visible', 'transform': 'translateY(0)' });
            $this.find('.product-body').css('transform', 'translateY('+ -animDistance +'px)');

        }, function () {
            var $this = $(this);

            $this.find('.product-footer').css({ 'visibility': 'hidden', 'transform': 'translateY(100%)' });
            $this.find('.product-body').css('transform', 'translateY(0)');
        });
    }

   
    if ( typeof noUiSlider === 'object' ) {
		var priceSlider  = document.getElementById('price-slider');

		
		if (priceSlider == null) return;

		noUiSlider.create(priceSlider, {
			start: [ 0, 750 ],
			connect: true,
			step: 50,
			margin: 200,
			range: {
				'min': 0,
				'max': 1000
			},
			tooltips: true,
			format: wNumb({
		        decimals: 0,
		        prefix: '$'
		    })
		});

		
		priceSlider.noUiSlider.on('update', function( values, handle ){
			$('#filter-price-range').text(values.join(' - '));
		});
	}

	
	

	
    function quantityInputs() {
        if ( $.fn.inputSpinner ) {
            $("input[type='number']").inputSpinner({
                decrementButton: '<i class="icon-minus"></i>',
                incrementButton: '<i class="icon-plus"></i>',
                groupClass: 'input-spinner',
                buttonsClass: 'btn-spinner',
                buttonsWidth: '26px'
            });
        }
    }

   
    if ( $.fn.stick_in_parent && $(window).width() >= 992 ) {
    	$('.sticky-content').stick_in_parent({
			offset_top: 80,
            inner_scrolling: false
		});
    }

    function owlCarousels($wrap, options) {
        if ( $.fn.owlCarousel ) {
            var owlSettings = {
                items: 1,
                loop: true,
                margin: 0,
                responsiveClass: true,
                nav: true,
                navText: ['<i class="bi bi-chevron-left"></i>', '<i class="bi bi-chevron-right"></i>'],
                dots: true,
                smartSpeed: 400,
                autoplay: true,
                autoplayTimeout: 6000
            };
            if (typeof $wrap == 'undefined') {
                $wrap = $('body');
            }
            if (options) {
                owlSettings = $.extend({}, owlSettings, options);
            }

           
            $wrap.find('[data-toggle="owl"]').each(function () {
                var $this = $(this),
                    newOwlSettings = $.extend({}, owlSettings, $this.data('owl-options'));

                $this.owlCarousel(newOwlSettings);
                
            });   
        }
    }

  
    if ( $.fn.elevateZoom ) {
        $('#product-zoom').elevateZoom({
            gallery:'product-zoom-gallery',
            galleryActiveClass: 'active',
            zoomType: "inner",
            cursor: "crosshair",
            zoomWindowFadeIn: 400,
            zoomWindowFadeOut: 400,
            responsive: true
        });

     
        $('.product-gallery-item').on('click', function (e) {
            $('#product-zoom-gallery').find('a').removeClass('active');
            $(this).addClass('active');

            e.preventDefault();
        });

        var ez = $('#product-zoom').data('elevateZoom');

        
        //$('#btn-product-gallery').on('click', function (e) {
        //    if ( $.fn.magnificPopup ) {
        //        $.magnificPopup.open({
        //            items: ez.getGalleryList(),
        //            type: 'image',
        //            gallery:{
        //                enabled:true
        //            },
        //            fixedContentPos: false,
        //            removalDelay: 600,
        //            closeBtnInside: false
        //        }, 0);

        //        e.preventDefault();
        //    }
        //});
    }

    
    //if ( $.fn.owlCarousel && $.fn.elevateZoom ) {
    //    var owlProductGallery = $('.product-gallery-carousel');

    //    owlProductGallery.on('initialized.owl.carousel', function () {
    //        owlProductGallery.find('.active img').elevateZoom({
    //            zoomType: "inner",
    //            cursor: "crosshair",
    //            zoomWindowFadeIn: 400,
    //            zoomWindowFadeOut: 400,
    //            responsive: true
    //        });
    //    });

    //    owlProductGallery.owlCarousel({
    //        loop: false,
    //        margin: 0,
    //        responsiveClass: true,
    //        nav: true,
    //        navText: ['<i class="icon-angle-left">', '<i class="icon-angle-right">'],
    //        dots: false,
    //        smartSpeed: 400,
    //        autoplay: false,
    //        autoplayTimeout: 15000,
    //        responsive: {
    //            0: {
    //                items: 1
    //            },
    //            560: {
    //                items: 2
    //            },
    //            992: {
    //                items: 3
    //            },
    //            1200: {
    //                items: 3
    //            }
    //        }
    //    });

    //    owlProductGallery.on('change.owl.carousel', function () {
    //        $('.zoomContainer').remove();
    //    });

    //    owlProductGallery.on('translated.owl.carousel', function () {
    //        owlProductGallery.find('.active img').elevateZoom({
    //            zoomType: "inner",
    //            cursor: "crosshair",
    //            zoomWindowFadeIn: 400,
    //            zoomWindowFadeOut: 400,
    //            responsive: true
    //        });
    //    });
    //}

    // // Product Gallery Separeted- product-sticky.html 
    //if ( $.fn.elevateZoom ) {
    //    $('.product-separated-item').find('img').elevateZoom({
    //        zoomType: "inner",
    //        cursor: "crosshair",
    //        zoomWindowFadeIn: 400,
    //        zoomWindowFadeOut: 400,
    //        responsive: true
    //    });

    //    // Create Array for gallery popup
    //    var galleryArr = [];
    //    $('.product-gallery-separated').find('img').each(function () {
    //        var $this = $(this),
    //            imgSrc = $this.attr('src'),
    //            imgTitle= $this.attr('alt'),
    //            obj = {'src': imgSrc, 'title': imgTitle };

    //        galleryArr.push(obj);
    //    })

    //    $('#btn-separated-gallery').on('click', function (e) {
    //        if ( $.fn.magnificPopup ) {
    //            $.magnificPopup.open({
    //                items: galleryArr,
    //                type: 'image',
    //                gallery:{
    //                    enabled:true
    //                },
    //                fixedContentPos: false,
    //                removalDelay: 600,
    //                closeBtnInside: false
    //            }, 0);

    //            e.preventDefault();
    //        }
    //    });
    //}

    
    $('#checkout-discount-input').on('focus', function () {
       
        $(this).parent('form').find('label').css('opacity', 0);
    }).on('blur', function () {
       
        var $this = $(this);

        if( $this.val().length !== 0 ) {
            $this.parent('form').find('label').css('opacity', 0);
        } else {
            $this.parent('form').find('label').css('opacity', 1);
        }
    });

   
    $('.tab-trigger-link').on('click', function (e) {
    	var targetHref = $(this).attr('href');

    	$('.nav-dashboard').find('a[href="'+targetHref+'"]').trigger('click');

    	e.preventDefault();
    });

  
	function layoutInit( container, selector) {
		$(container).each(function () {
            var $this = $(this);

            $this.isotope({
                itemSelector: selector,
                layoutMode: ( $this.data('layout') ? $this.data('layout'): 'masonry' )
            });
        });
	}
 
	function isotopeFilter ( filterNav, container) {
		$(filterNav).find('a').on('click', function(e) {
			var $this = $(this),
				filter = $this.attr('data-filter');

			
			$(filterNav).find('.active').removeClass('active');

			
			$(container).isotope({
				filter: filter,
				transitionDuration: '0.7s'
			});
			
			
			$this.closest('li').addClass('active');
			e.preventDefault();
		});
	}

    /* Masonry / Grid Layout & Isotope Filter for blog/portfolio etc... */
    //if ( typeof imagesLoaded === 'function' && $.fn.isotope) {
    //    // Portfolio
    //    $('.portfolio-container').imagesLoaded(function () {
    //        // Portfolio Grid/Masonry
    //        layoutInit( '.portfolio-container', '.portfolio-item' ); // container - selector
    //        // Portfolio Filter
    //        isotopeFilter( '.portfolio-filter',  '.portfolio-container'); //filterNav - .container
    //    });

    //    // Blog
    //    $('.entry-container').imagesLoaded(function () {
    //        // Blog Grid/Masonry
    //        layoutInit( '.entry-container', '.entry-item' ); // container - selector
    //        // Blog Filter
    //        isotopeFilter( '.entry-filter',  '.entry-container'); //filterNav - .container
    //    });

    //    // Product masonry product-masonry.html
    //    $('.product-gallery-masonry').imagesLoaded(function () {
    //        // Products Grid/Masonry
    //        layoutInit( '.product-gallery-masonry', '.product-gallery-item' ); // container - selector
    //    });

    //    // Products - Demo 11
    //    $('.products-container').imagesLoaded(function () {
    //        // Products Grid/Masonry
    //        layoutInit( '.products-container', '.product-item' ); // container - selector
    //        // Product Filter
    //        isotopeFilter( '.product-filter',  '.products-container'); //filterNav - .container
    //    });
    //}

	
    var $countItem = $('.count');
	if ( $.fn.countTo ) {
        if ($.fn.waypoint) {
            $countItem.waypoint( function () {
                $(this.element).countTo();
            }, {
                offset: '90%',
                triggerOnce: true 
            });
        } else {
            $countItem.countTo();
        }    
    } else { 
       
        $countItem.each(function () {
            var $this = $(this),
                countValue = $this.data('to');
            $this.text(countValue);
        });
    }

    

    
    $('#review-link').on('click', function (e) {
        var target = $(this).attr('href'),
            $target = $(target);

        if ( $('#product-accordion-review').length ) {
            $('#product-accordion-review').collapse('show');
        }

        if ($target.length) {
            
            var scrolloffset = ( $(window).width() >= 992 ) ? ($target.offset().top - 72) : ( $target.offset().top - 10 )
            $('html, body').animate({
                'scrollTop': scrolloffset
            }, 600);

            $target.tab('show');
        }

    	e.preventDefault();
    });

    //this code have bug/ see you later in layout.cshtml
	
    //var $scrollTop = $('#scroll-top-button');
    
    //$(window).on('load scroll', function() {
    //    if ($(window).scrollTop() >= 400) {
    //      
    //        $scrollTop.addClass('show');
    //    } else {
    //        $scrollTop.removeClass('show');
    //    }
    //});


  
    //$scrollTop.on('click', function (e) {
    //    $('html, body').animate({
    //        'scrollTop': 0
    //    }, 800);
    //    e.preventDefault();
    //});

  
   

    var $megamenu = $('.megamenu-container .sf-with-ul');
    $megamenu.hover(function() {
        $('.demo-item.show').addClass('hidden');
        $('.demo-item.show').removeClass('show');
        $viewAll.removeClass('disabled-hidden');
    });

    
});
