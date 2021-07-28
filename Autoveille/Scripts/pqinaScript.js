$(document).ready(function () {
    var opportunities = $('.opportunitiesPercent').text();

    var oppoCanvas = document.getElementById('oppo-circle');
    var ctx = oppoCanvas.getContext('2d');
    var $canvas = $("#oppo-circle");
    var canvasOffset = $canvas.offset();
    var offsetX = canvasOffset.left;
    var offsetY = canvasOffset.top;
    var scrollX = $canvas.scrollLeft();
    var scrollY = $canvas.scrollTop();

    //Create gradient
    var yellowGradienta = ctx.createLinearGradient(0, 500, 0, 0);
    yellowGradienta.addColorStop(0, '#f5df2e');
    yellowGradienta.addColorStop(1, '#f07654');

    //Create gradient
    var yellowGradient = ctx.createLinearGradient(0, 500, 0, 0);
    yellowGradient.addColorStop(0, '#f07654');
    yellowGradient.addColorStop(1, '#f5df2e');

    var oppoCircle = {
        x: 170,
        y: 170,
        radius: 100,
        angleStartArc: 1.5 * Math.PI,
        angleEndArc: 0 * Math.PI,
        wasInside: false,
        finish: parseInt(opportunities), // Finish (in %)
        gradientColor: yellowGradienta,
        gradientaColor: yellowGradient,
        curr: 0, // Current position (in %)
        circum: Math.PI * 2
    }


    drawOppoCircle(oppoCircle, false);
    TickInit(tick);
    function drawOppoCircle(circle, isInside, draw_to) {
        var ctx = oppoCanvas.getContext('2d');
        ctx.clearRect(0, 0, oppoCanvas.width, oppoCanvas.height);
        //Draw circle
        ctx.beginPath();
        ctx.arc(circle.x, circle.x, circle.radius, 0, 2 * Math.PI, false);
        ctx.lineWidth = 30;
        ctx.strokeStyle = 'black';
        ctx.shadowColor = '#a57b0d';
        ctx.shadowBlur = 50;
        ctx.shadowOffsetY = 3;
        ctx.stroke();

        ctx.stroke();

        //Draw arc
        ctx.beginPath();
        ctx.arc(circle.x, circle.x, circle.radius, circle.angleStartArc, draw_to);
        ctx.strokeStyle = isInside ? circle.gradientColor : circle.gradientaColor;
        // ctx.strokeStyle =circle.gradientaColor;
        ctx.lineWidth = 30;
        ctx.lineCap = 'round';
        ctx.stroke();
        circle.wasInside = isInside;

        // Increment percent
        circle.curr++;
        // Animate until end
        if (circle.curr < circle.finish + 1) {
            // Recursive repeat this function until the end is reached
            requestAnimationFrame(function () {
                drawOppoCircle(circle, isInside, circle.circum * circle.curr / 100 + circle.angleStartArc);
            });
        }

    }



});


//sconsole.log(tick);
function TickInit(tick) {
    // set language
    var locale = {
        YEAR_PLURAL: 'ANNEES',
        YEAR_SINGULAR: 'Ans',
        MONTH_PLURAL: 'Mois',
        MONTH_SINGULAR: 'MOIS',
        WEEK_PLURAL: 'Semaines',
        WEEK_SINGULAR: 'Semaine',
        DAY_PLURAL: 'JOURS',
        DAY_SINGULAR: 'Jour',
        HOUR_PLURAL: 'Heures',
        HOUR_SINGULAR: 'Heure',
        MINUTE_PLURAL: 'Minutes',
        MINUTE_SINGULAR: 'Minute',
        SECOND_PLURAL: 'Secondes',
        SECOND_SINGULAR: 'Seconde',
        MILLISECOND_PLURAL: 'Millisecondes',
        MILLISECOND_SINGULAR: 'Milliseconde'
    };

    for (var key in locale) {
        if (!locale.hasOwnProperty(key)) { continue; }
        tick.setConstant(key, locale[key]);
    }



    var dd = Tick.helper.date();
    console.log("here is new dd " + dd);
    tick.value = [dd.getDate(), dd.getMonth() + 1, dd.getFullYear()];
    // console.log(tick.value);

}


