(function($) {
    "use strict"; // Start of use strict

  // Initialization for the theme
    $(document).ready(function () {
        let darkTheme = localStorage.getItem("theme");

        if (darkTheme == undefined) {
            localStorage.setItem("theme", "light");
        } else if (darkTheme == "light") {
            $("#btn-dark-theme").removeClass("fa-moon")
            $("#btn-dark-theme").addClass("fa-sun")

            ChangeTheme();
        } else if (darkTheme == "dark") {
            $("#btn-dark-theme").removeClass("fa-sun")
            $("#btn-dark-theme").addClass("fa-moon")

            ChangeTheme();
        }


        fetch("/Home/GetNotifications").then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        }).then(res => {
            let data = res.data;
            console.log(data)

            data.forEach((notif) => {
                let noti = ` <a class="dropdown-item text-wrap" href="#">${notif.message}  <small>${notif.createdAt}</small></a>`
                $("#notificationList").append(noti);
            })

            let d = `<div class="dropdown-divider"></div>`;
            let seeAll = `<a class="dropdown-item text-wrap" href="#">Ver todas las notificaciones</a>`

            $("#notificationList").append(d).append(seeAll);
                            
            
        })

   })

  // Toggle the side navigation
  $("#sidebarToggle, #sidebarToggleTop").on('click', function(e) {
    $("body").toggleClass("sidebar-toggled");
    $(".sidebar").toggleClass("toggled");
    if ($(".sidebar").hasClass("toggled")) {
      $('.sidebar .collapse').collapse('hide');
    };
  });

  // Close any open menu accordions when window is resized below 768px
  $(window).resize(function() {
    if ($(window).width() < 768) {
      $('.sidebar .collapse').collapse('hide');
    };
    
    // Toggle the side navigation when window is resized below 480px
    if ($(window).width() < 480 && !$(".sidebar").hasClass("toggled")) {
      $("body").addClass("sidebar-toggled");
      $(".sidebar").addClass("toggled");
      $('.sidebar .collapse').collapse('hide');
    };
  });

  // Prevent the content wrapper from scrolling when the fixed side navigation hovered over
  $('body.fixed-nav .sidebar').on('mousewheel DOMMouseScroll wheel', function(e) {
    if ($(window).width() > 768) {
      var e0 = e.originalEvent,
        delta = e0.wheelDelta || -e0.detail;
      this.scrollTop += (delta < 0 ? 1 : -1) * 30;
      e.preventDefault();
    }
  });

  // Scroll to top button appear
  $(document).on('scroll', function() {
    var scrollDistance = $(this).scrollTop();
    if (scrollDistance > 100) {
      $('.scroll-to-top').fadeIn();
    } else {
      $('.scroll-to-top').fadeOut();
    }
  });

  // Smooth scrolling using jQuery easing
  $(document).on('click', 'a.scroll-to-top', function(e) {
    var $anchor = $(this);
    $('html, body').stop().animate({
      scrollTop: ($($anchor.attr('href')).offset().top)
    }, 1000, 'easeInOutExpo');
    e.preventDefault();
  });

    // Dark Theme
    $("#btn-dark-theme").click(function () {
        ChangeTheme();

    });

    function ChangeTheme() {

        if ($("#btn-dark-theme").hasClass("fa-moon")) {
            let bodyCards = document.querySelectorAll(".card-body");

            localStorage.setItem("theme", "dark");

            $("#btn-dark-theme").removeClass("fa-moon");
            $("#btn-dark-theme").addClass("fa-sun");

            $("#content").addClass("dark-content");
            $("#accordionSidebar").addClass("dark-menu");
            $("#lay-footer").addClass("dark-footer");
            $("#navbar").addClass("dark-nav");
            $(".card").addClass("border-0").addClass("dark-cards-body");
            $(".card-header").addClass("border-0");
            $(".table").addClass("border-0").addClass("dark-cards-body");
            $(".card-body").addClass("dark-cards-body");
            $(".card-header").addClass("bg-gradient-dark");
            $(".modal-content").addClass("dark-cards-body")
            $(".dt-button").addClass("text-white");

        } else {
            localStorage.setItem("theme", "light");

            $("#btn-dark-theme").removeClass("fa-sun");
            $("#btn-dark-theme").addClass("fa-moon");

            $("#content").removeClass("dark-content");
            $("#accordionSidebar").removeClass("dark-menu");
            $("#lay-footer").removeClass("dark-footer");
            $("#navbar").removeClass("dark-nav");
            $(".card").removeClass("border-0").removeClass("dark-cards-body");
            $(".card-header").removeClass("border-0");
            $(".table").removeClass("border-0").removeClass("bg-gradient-dark");
            $(".card-body").removeClass("dark-cards-body");
            $(".card-header").removeClass("bg-gradient-dark");
            $(".modal-content").removeClass("dark-cards-body")
            $(".dt-button").removeClass("text-white");

        }

    }


})(jQuery); // End of use strict
