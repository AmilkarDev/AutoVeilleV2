//happens when page is loaded
$( document ).ready(function() {
   
    //hide and diplay the table section and change the icon
   $(".btnPLus" ).on('click',function() {
        if($(this).parent().parent().next().hasClass('hidden')){
            $(this).parent().parent().next().removeClass('hidden').addClass("visible");
            $(this).children().attr("src","images/Moins.png");
            $(this).parent().parent().css("marginBottom","0px");
        } else if($(this).parent().parent().next().hasClass('visible')){
    	    $(this).parent().parent().next().removeClass("visible").addClass("hidden");
            $(this).children().attr("src","images/Plus.png");
            $(this).parent().parent().css("marginBottom","5px");
         }
        
        $(".details").removeClass("visible").addClass("hidden");
        $(".clientSelect").removeClass("selected").addClass("unselected");
     } );
    
    //menu display 
    $(".menuIcon" ).on('click',function() {
        if($(this).parent().parent().parent().parent().next().hasClass('hidden')){
            //console.log("hidden");
            $(this).parent().parent().parent().parent().next().removeClass('hidden').addClass("visible");
        } else if($(this).parent().parent().parent().parent().next().hasClass('visible')){
    	    $(this).parent().parent().parent().parent().next().removeClass("visible").addClass("hidden");
            //console.log("visible");
        }
     }
    );
    
    //details display
        
        $(".clientSelect" ).on('click',function(e) {
            if ($(e.target).parent().hasClass("unselected")){
                //console.log($(e.target).parent());
                $(".details").removeClass("hidden").addClass("visible");
                $(e.target).parent().removeClass("unselected").addClass("selected");
            }
            if ($(e.target).parent().hasClass("selected")){
                $(e.target).parent().siblings().removeClass("selected").addClass("unselected");
                $(e.target).parent().siblings().on("click",function(e){
                    $(".details").removeClass("hidden").addClass("visible");
                });
            }
        });
});




 