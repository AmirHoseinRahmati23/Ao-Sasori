// start card slider script
var swiper = new Swiper('.swiper-container', {
    slidesPerView: 1,
    spaceBetween: 40,
        pagination: {
          el: '.swiper-pagination',
          clickable: true,
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
          },
        breakpoints: {
          640: {
            slidesPerView: 2,
            spaceBetween: 30,
          },
          768: {
            slidesPerView: 3,
            spaceBetween: 30,
          },
          1024: {
            slidesPerView: 4,
            spaceBetween: 30,
          },
          1200: {
            slidesPerView: 5,
            spaceBetween: 30,
          },
        }
      });
// end card slider script
$(document).ready(function(){
  // start signup btn
  $("#btnSignUp").click(function(){
    $("#signUp").css("display", "block");
  });
  $("#btnlogin").click(function(){
    $("#signUp").css("display", "none");
  });
  // end signup
    $('.aDBC1080p').click(function(){
      $('.aDBC1080p').toggleClass('hgfh');
      $('.aDBC1080p > *').toggleClass('yjiy')
    })
  // start eye password
  $(".eyeBtn").click(function(){
    var input =$(".password");
    if (input.attr("type")==="password") {
      $(".iiconpass").toggleClass("fas fa-eye-slash");
      input.attr("type","text");
    }else {
      input.attr("type","password");
      $(".iiconpass").removeClass("fas fa-eye-slash");
    }
  });
  //end eye password
});
// end jQuery
// end script login


(function(){

const cclick = document.querySelectorAll('.cclick');
cclick.forEach( itame => {
  itame.addEventListener('click' , event => {
    const parent = event.target.parentNode.parentNode.parentNode;
    parent.classList.toggle('activv');
  });
});




})();


