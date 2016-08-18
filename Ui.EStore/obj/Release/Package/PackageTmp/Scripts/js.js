$(document).ready(function () {

    


    $('#drop').click(function () {
        $('#nav').slideToggle(); 
         
        
    });


    $(function () {
        $(".first-menu").hover(function () {
            $(this).find("#first-menu").stop().slideDown();
           
        }, function() {
            $(this).find("#first-menu").stop().slideUp();
        });
        
        $(".socend-menu").hover(function () {
            $(this).find("#socend-menu").stop().slideToggle(0);
            return false;
        });
     
        $(".socend-menu:last-child").mouseleave(function () {
            $("#nav ul li ul").stop().slideUp();
            return false;
        });
      
        
    });
    
    $(function () {
        $(".socend-menu").hover(function () {
            $(this).find("ul").stop().slideToggle();

            if ($(this).find("ul").children().length<=0) {
                $(this).find("ul").remove();
            }
           
            
        
     
            return false;
        });

        $(".socend-menu").hover(function () {
            $(this).find("#socend-menu").stop().slideToggle(0);
            return false;
        });
        
     

    });
    

    $(function () {
        
  
    });

    $(function () {
        $("#more-menu h3").click(function () {
            $("#more-menu-slide").stop().slideToggle();
            return false;
        });

        $(".more-menu").hover(function () {
            $(this).find("#sub-more-menu").stop().slideToggle(0);
            return false;
        });

    });


});

