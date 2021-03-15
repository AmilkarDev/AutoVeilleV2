//happens when page is loaded
$( document ).ready(function() {
   
    //hide and diplay the table section and change the icon
   $(".btnPLus" ).on('click',function() {
     if($(this).parent().parent().next().hasClass('hidden')){
        $(this).parent().parent().next().removeClass('hidden').addClass("visible");
        $(this).children().attr("src","images/Moins.png");
       } else if($(this).parent().parent().next().hasClass('visible')){
    	   $(this).parent().parent().next().removeClass("visible").addClass("hidden");
           $(this).children().attr("src","images/Plus.png");
         }
     } 
    );
    
    $(".menuIcon" ).on('click',function() {
     if($(this).parent().next().hasClass('hidden')){
         console.log("hidden");
        $(this).parent().next().removeClass('hidden').addClass("visible");
       } else if($(this).parent().next().hasClass('visible')){
    	   $(this).parent().next().removeClass("visible").addClass("hidden");
            console.log("visible");
         }
     }
    );
    
})




 