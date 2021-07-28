//var chartColors = {
//    red: 'rgb(255, 99, 132)',
//    orange: 'rgb(255, 159, 64)',
//    yellow: 'rgb(255, 205, 86)',
//    green: 'rgb(75, 192, 192)',
//    blue: 'rgb(54, 162, 235)',
//    purple: 'rgb(153, 102, 255)',
//    grey: 'rgb(231,233,237)'
//};

//var setUpCanvas = (c) => {
//    c.width = c.clientWidth;
//    c.height = c.clientHeight;
//};

var randomScalingFactor = function () {
    return (Math.random() > 0.5 ? 1.0 : 1.0) * Math.round(Math.random() * 100);
};
Chart.helpers.merge(Chart.defaults.global, {
    datasets: {
        roundedBar: {
            categoryPercentage: 0.8,
            barPercentage: 0.9
        }
    }
});
// draws a rectangle with a rounded top
Chart.helpers.drawRoundedTopRectangle = function (ctx, x, y, width, height, radius) {
    ctx.beginPath();
    ctx.moveTo(x + radius, y);
    // top right corner
    ctx.lineTo(x + width - radius, y);
    ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
    // bottom right	corner
    ctx.lineTo(x + width, y + height);
    // bottom left corner
    ctx.lineTo(x, y + height);
    // top left
    ctx.lineTo(x, y + radius);
    ctx.quadraticCurveTo(x, y, x + radius, y);
    ctx.shadowColor = '#ec9f05';
    ctx.shadowBlur = 80;
    ctx.shadowOffsetY = 3;
    ctx.stroke();
    ctx.closePath();
};

Chart.elements.RoundedTopRectangle = Chart.elements.Rectangle.extend({
    draw: function () {
        var ctx = this._chart.ctx;
        var vm = this._view;
        var left, right, top, bottom, signX, signY, borderSkipped;
        var borderWidth = vm.borderWidth;

        if (!vm.horizontal) {
            // bar
            left = vm.x - vm.width / 2;
            right = vm.x + vm.width / 2;
            top = vm.y;
            bottom = vm.base;
            signX = 1;
            signY = bottom > top ? 1 : -1;
            borderSkipped = vm.borderSkipped || 'bottom';
        } else {
            // horizontal bar
            left = vm.base;
            right = vm.x;
            top = vm.y - vm.height / 2;
            bottom = vm.y + vm.height / 2;
            signX = right > left ? 1 : -1;
            signY = 1;
            borderSkipped = vm.borderSkipped || 'left';
        }

        // Canvas doesn't allow us to stroke inside the width so we can
        // adjust the sizes to fit if we're setting a stroke on the line
        if (borderWidth) {
            // borderWidth shold be less than bar width and bar height.
            var barSize = Math.min(Math.abs(left - right), Math.abs(top - bottom));
            borderWidth = borderWidth > barSize ? barSize : borderWidth;
            var halfStroke = borderWidth / 2;
            // Adjust borderWidth when bar top position is near vm.base(zero).
            var borderLeft = left + (borderSkipped !== 'left' ? halfStroke * signX : 0);
            var borderRight = right + (borderSkipped !== 'right' ? -halfStroke * signX : 0);
            var borderTop = top + (borderSkipped !== 'top' ? halfStroke * signY : 0);
            var borderBottom = bottom + (borderSkipped !== 'bottom' ? -halfStroke * signY : 0);
            // not become a vertical line?
            if (borderLeft !== borderRight) {
                top = borderTop;
                bottom = borderBottom;
            }
            // not become a horizontal line?
            if (borderTop !== borderBottom) {
                left = borderLeft;
                right = borderRight;
            }
        }

        // calculate the bar width and roundess
        var barWidth = Math.abs(left - right);
        var roundness = this._chart.config.options.barRoundness || 0.5;
        var radius = barWidth * roundness * 0.5;

        // keep track of the original top of the bar
        var prevTop = top;

        // move the top down so there is room to draw the rounded top
        top = prevTop + radius;
        var barRadius = top - prevTop;

        ctx.beginPath();
        ctx.fillStyle = vm.backgroundColor;
        ctx.strokeStyle = vm.borderColor;
        ctx.lineWidth = borderWidth;

        // draw the rounded top rectangle
        Chart.helpers.drawRoundedTopRectangle(ctx, left, (top - barRadius + 1), barWidth, bottom - prevTop, barRadius);

        ctx.fill();
        if (borderWidth) {
            ctx.stroke();
        }

        // restore the original top value so tooltips and scales still work
        top = prevTop;
    },
});

Chart.defaults.roundedBar = Chart.helpers.clone(Chart.defaults.bar);

Chart.controllers.roundedBar = Chart.controllers.bar.extend({
    dataElementType: Chart.elements.RoundedTopRectangle
});


var ctx = document.getElementById("barsCanvas").getContext("2d");

var green_blue_gradient = ctx.createLinearGradient(0, 0, 0, 600);
green_blue_gradient.addColorStop(1, '#f07654');
green_blue_gradient.addColorStop(0, '#f5df2e');



var purple_orange_gradient = ctx.createLinearGradient(0, 0, 0, 600);
purple_orange_gradient.addColorStop(0, '#fe5858');
purple_orange_gradient.addColorStop(1, '#ee9617');

var orange_purple_gradient = ctx.createLinearGradient(0, 0, 0, 600);
orange_purple_gradient.addColorStop(0, '#ee9617');
orange_purple_gradient.addColorStop(1, '#fe5858');


var orange_red_gradient = ctx.createLinearGradient(0, 0, 0, 600);
orange_red_gradient.addColorStop(0, '#fc9842');
orange_red_gradient.addColorStop(1, '#f5d020');



//Create gradient
var purpleGradienta = ctx.createLinearGradient(0, 500, 0, 0);
purpleGradienta.addColorStop(0, '#861657');
purpleGradienta.addColorStop(1, '#ffa69e');

//Create gradient
var purpleGradient = ctx.createLinearGradient(0, 500, 0, 0);
purpleGradient.addColorStop(0, '#ffa69e');
purpleGradient.addColorStop(1, '#861657');

var verticalBars = new Chart(ctx, {
    type: 'roundedBar',
    data: {
        labels: ["Appels", "Rejoints", "Rendez-Vous"],
        datasets: [{
            label: 'Consultants de ventes privées',
            barPercentage: 0.6,
            backgroundColor: green_blue_gradient,
            hoverBackgroundColor: 'yellow',
            hoverBorderWidth: 2,
            hoverBorderColor: 'white',
            barPercentage: 0.4,
            data: [
                85,
                22,
                74,
            ]
        }, {
            label: 'Centres d\'appels',
            backgroundColor: orange_red_gradient,
            hoverBackgroundColor: 'orange',
            hoverBorderWidth: 2,
            hoverBorderColor: 'white',
            barPercentage: 0.4,
            data: [62, 75, 60]
        }, {
            label: 'Totaux',
            backgroundColor: purple_orange_gradient,
            hoverBackgroundColor: '#ff4e00',
            hoverBorderWidth: 2,
            hoverBorderColor: 'white',
            barPercentage: 0.4,
            data: [
                34,
                45,
                55,
            ]
        }]
    },
    options: {
        //cornerRadius: 8,
        plugins: {
            datalabels: {
                align: 'end',
                anchor: 'end',
                color: 'white',
                font: {
                    weight: 'bold',
                    size: 15
                },

                formatter: function (value, context) {
                    return value;
                }
            }
        },

        legend: {
            display: false,
            position: 'bottom'
        },
        legendCallback: function (chart) {
            var text = [];

            text.push('<ul class="' + chart.id + '-legend">');
            for (var i = 0; i < chart.data.datasets.length; i++) {
                text.push('<li style="list-style-type:none;"><div class="legendValue"><span style="border-radius :15px;margin-right:15px;background-color:' + chart.data.datasets[i].hoverBackgroundColor + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>');

                if (chart.data.datasets[i].label) {
                    text.push('<span class="label" style="color:white;">' + chart.data.datasets[i].label + '</span><br> <br>');
                }

                text.push('</div></li><div class="clear"></div>');
            }

            text.push('</ul>');

            return text.join('');
        },

        responsive: true,
        barRoundness: 1,
        //legend: {
        //    position: 'bottom',
        //    fullWidth: true,
        //    labels: {
        //        boxWidth: 10,
        //        fontSize: 15,
        //        padding: 50,
        //        maxRotation: 90,
        //        minRotation: 90,
        //        align: 'left',
        //        fontColor: 'white',
        //        fontStyle: 'bold',
        //    }
        //},
        title: {
            display: false,
            text: "Chart.js - Bar Chart with Rounded Tops and customized grid"
        },
        scales: {
            xAxes: [{
                ticks: {
                    fontSize: 20,
                    fontStyle: 'bold',
                    fontColor: 'white',
                },
                gridLines: {
                    display: false,
                    drawBorder: true,
                    color: 'white'
                },

            }],
            yAxes: [{
                display: true,
                gridLines: {
                    drawBorder: false,
                    color: 'grey'
                },
                ticks: {
                    userCallback: function (item, index) {
                        if (index % 2) return item;
                        return '';
                    },

                    min: 0,
                    stepSize: 10,
                    padding: 10,
                    fontSize: 15,
                    fontStyle: 'Bold',
                }
            }],
            animation: {
                duration: 1,
                easing: 'linear'
            },
        }
    }
});
$("#legend-container").html(verticalBars.generateLegend());







/********* Horizontal Bar Chart display and animation ******/

Chart.elements.Rectangle.prototype.draw = function () {

    var ctx = this._chart.ctx;
    var vm = this._view;
    var left, right, top, bottom, signX, signY, borderSkipped, radius;
    var borderWidth = vm.borderWidth;

    // Set Radius Here
    // If radius is large enough to cause drawing errors a max radius is imposed
    var cornerRadius = 75;

    left = vm.base;
    right = vm.x;
    top = vm.y - vm.height / 2;
    bottom = vm.y + vm.height / 2;
    signX = right > left ? 1 : -1;
    signY = 1;
    borderSkipped = vm.borderSkipped || 'left';

    // Canvas doesn't allow us to stroke inside the width so we can
    // adjust the sizes to fit if we're setting a stroke on the line
    if (borderWidth) {
        // borderWidth shold be less than bar width and bar height.
        var barSize = Math.min(Math.abs(left - right), Math.abs(top - bottom));
        borderWidth = borderWidth > barSize ? barSize : borderWidth;
        var halfStroke = borderWidth / 2;
        // Adjust borderWidth when bar top position is near vm.base(zero).
        var borderLeft = left + (borderSkipped !== 'left' ? halfStroke * signX : 0);
        var borderRight = right + (borderSkipped !== 'right' ? -halfStroke * signX : 0);
        var borderTop = top + (borderSkipped !== 'top' ? halfStroke * signY : 0);
        var borderBottom = bottom + (borderSkipped !== 'bottom' ? -halfStroke * signY : 0);
        // not become a vertical line?
        if (borderLeft !== borderRight) {
            top = borderTop;
            bottom = borderBottom;
        }
        // not become a horizontal line?
        if (borderTop !== borderBottom) {
            left = borderLeft;
            right = borderRight;
        }
    }

    ctx.beginPath();
    ctx.shadowColor = 'purple';
    ctx.shadowBlur = 80;
    // ctx.shadowOffsetY=15;
    // ctx.shadowOffsetX=15;
    ctx.fillStyle = vm.backgroundColor;
    ctx.strokeStyle = vm.borderColor;
    ctx.lineWidth = borderWidth;

    // Corner points, from bottom-left to bottom-right clockwise
    // | 1 2 |
    // | 0 3 |
    var corners = [
        [left, bottom],
        [left, top],
        [right, top],
        [right, bottom]
    ];

    // Find first (starting) corner with fallback to 'bottom'
    var borders = ['bottom', 'left', 'top', 'right'];
    var startCorner = borders.indexOf(borderSkipped, 0);
    if (startCorner === -1) {
        startCorner = 0;
    }

    function cornerAt(index) {
        return corners[(startCorner + index) % 4];
    }

    // Draw rectangle from 'startCorner'
    var corner = cornerAt(0);
    var width, height, x, y, nextCorner, nextCornerId
    var x_tl, x_tr, y_tl, y_tr;
    var x_bl, x_br, y_bl, y_br;
    ctx.moveTo(corner[0], corner[1]);

    for (var i = 1; i < 4; i++) {
        corner = cornerAt(i);
        nextCornerId = i + 1;
        if (nextCornerId == 4) {
            nextCornerId = 0
        }

        nextCorner = cornerAt(nextCornerId);

        width = corners[2][0] - corners[1][0];
        height = corners[0][1] - corners[1][1];
        x = corners[1][0];
        y = corners[1][1];
        radius = cornerRadius;

        // Fix radius being too large
        if (radius > Math.abs(height) / 2) {
            radius = Math.floor(Math.abs(height) / 2);
        }
        if (radius > Math.abs(width) / 2) {
            radius = Math.floor(Math.abs(width) / 2);
        }
        ctx.moveTo(x, y);
        ctx.lineTo(x + width - radius, y);
        ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
        ctx.lineTo(x + width, y + height - radius);
        ctx.quadraticCurveTo(x + width, y + height, x + width - radius, y + height);
        ctx.lineTo(x, y + height);
    }
    ctx.fill();
    if (borderWidth) {
        ctx.stroke();
    }
};



var ctx = document.getElementById("horizontalBarsCanvas").getContext("2d");

var green_blue_gradient = ctx.createLinearGradient(0, 0, 250, 0);
green_blue_gradient.addColorStop(0, '#4f0457');
green_blue_gradient.addColorStop(1, '#940f66');


var purple_orange_gradient = ctx.createLinearGradient(0, 0, 250, 0);
purple_orange_gradient.addColorStop(0, '#ba4e97');
purple_orange_gradient.addColorStop(1, '#dc3c85');


var orange_red_gradient = ctx.createLinearGradient(0, 0, 70, 0);
orange_red_gradient.addColorStop(1, '#cc3677');
orange_red_gradient.addColorStop(0, '#dc2567');

var myBar = new Chart(ctx, {
    type: 'horizontalBar',

    data: {
        labels: ["Neuf", "Occasion", "Location"],
        datasets: [{
            axis: 'y',
            //label: 'My First Dataset',
            data: [47, 52, 31],
            barPercentage: 0.4,

            fill: false,
            hoverBackgroundColor:
                ['#c83a79', '#bf4e9a', '#55015c'],
            backgroundColor: [orange_red_gradient, purple_orange_gradient, green_blue_gradient, 'green'],
        }]
    },
    options: {
        //cornerRadius: 8,
        //barPercentage : 0.5,
        barThickness: 10,
        indexAxis: 'y',
        plugins: {
            datalabels: {
                align: 'end',
                anchor: 'end',
                color: 'white',
                //   padding : {
                //   right : 150
                // },
                font: {
                    weight: 'bold',
                    size: 15
                },

                formatter: function (value, context) {
                    return value;
                }
            },
        },
        legend: {
            display: false,
            position: 'bottom'
        },
        legendCallback: function (chart) {
            var text = [];

            text.push('<ul class="' + chart.id + '-legend">');
            for (var i = 0; i < chart.data.labels.length; i++) {
                text.push('<li style="list-style-type:none;"><div class="legendValue"><span style="border-radius :15px;margin-right:15px;background-color:' + chart.data.datasets[0].hoverBackgroundColor[i] + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>');

                if (chart.data.labels[i]) {
                    text.push('<span class="label" style="color:white;">' + chart.data.labels[i] + '</span><br> <br>');
                }

                text.push('</div></li><div class="clear"></div>');
            }

            text.push('</ul>');

            return text.join('');
        },

        responsive: true,
        //barRoundness: 10,

        title: {
            display: false,
            text: "Chart.js - Bar Chart with Rounded Tops and customized grid"
        },
        scales: {
            xAxes: [{
                display: true,
                gridLines: {
                    display: true,
                    drawBorder: false,
                    color: 'white'
                },

                ticks: {
                    min: 0,
                    stepSize: 5,
                    padding: 10,
                    fontSize: 15,
                    fontStyle: 'Bold',
                },
                beginAtZero: true,


            }],
            yAxes: [{
                gridLines: {
                    display: true,
                    drawOnChartArea: false,
                    drawBorder: true,
                    borderColor: 'white',
                    color: 'white',
                    width: 15,
                    tickMarkLength: 100,
                    drawTicks: false,
                    min: 0,
                },
                ticks: {
                    display: false,

                    // min :0,
                    stepSize: 50,
                    padding: 10,
                    fontSize: 15,
                    fontStyle: 'Bold',
                }
            }],
            animation: {
                duration: 1,
                easing: 'linear'
            },
        },

        // Core options
        aspectRatio: 5 / 3,
        layout: {
            padding: {
                top: 24,
                right: 55,
                bottom: 0,
                left: 10
            }
        },
    }
});


// Create our legend
$("#horizontalBarsLegendContainer").html(myBar.generateLegend());









//Create gradient
var gradienta = ctx.createLinearGradient(0, 500, 0, 0);
gradienta.addColorStop(0, '#31B8A5');
gradienta.addColorStop(1, '#24B24A');

//Create gradient
var gradient = ctx.createLinearGradient(0, 500, 0, 0);
gradient.addColorStop(0, '#24B24A');
gradient.addColorStop(1, '#31B8A5');

var circleRdvJour1 = {
    x: 110,
    y: 110,
    radius: 200,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: gradienta,
    gradientColor: gradient,
    shadowColor: 'green',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}
var circleRdvJour2 = {
    x: 110,
    y: 110,
    radius: 200,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: gradienta,
    gradientColor: gradient,
    shadowColor: 'green',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}
var circleRdvJour3 = {
    x: 110,
    y: 110,
    radius: 200,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: gradienta,
    gradientColor: gradient,
    shadowColor: 'green',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}
var circleRdvHorsEvt = {
    x: 110,
    y: 110,
    radius: 200,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: gradienta,
    gradientColor: gradient,
    shadowColor: 'green',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}
var circleVenteJour1 = {
    x: 110,
    y: 110,
    radius: 80,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: purpleGradient,
    gradientColor: purpleGradient,
    shadowColor: '#a4508b',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}

var circleVenteJour2 = {
    x: 110,
    y: 110,
    radius: 80,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: purpleGradient,
    gradientColor: purpleGradient,
    shadowColor: '#a4508b',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}

var circleVenteJour3 = {
    x: 110,
    y: 110,
    radius: 80,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: purpleGradient,
    gradientColor: purpleGradient,
    shadowColor: '#a4508b',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}

var circleVenteHorsEvt = {
    x: 110,
    y: 110,
    radius: 80,
    angleStartArc: 1.5 * Math.PI,
    angleEndArc: 0 * Math.PI,
    wasInside: false,
    lineWidth: 30,
    shadowBlur: 20,
    finish: 25, // Finish (in %)
    gradientaColor: purpleGradient,
    gradientColor: purpleGradient,
    shadowColor: '#a4508b',
    curr: 0, // Current position (in %)
    circum: Math.PI * 2
}
//Create gradient
var gradienta = ctx.createLinearGradient(0, 500, 0, 0);
gradienta.addColorStop(0, '#5aff15');
gradienta.addColorStop(1, '#01baef');

//Create gradient
var gradient = ctx.createLinearGradient(0, 500, 0, 0);
gradient.addColorStop(0, '#01baef');
gradient.addColorStop(1, '#20bf55');

// Enables browser-decided smooth animation (60fps)
var raf = window.requestAnimationFrame || window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame || window.msRequestAnimationFrame;
window.requestAnimationFrame = raf;





this.drawCircle = function (circle, id, isInside, draw_to) {

    var canvas = document.getElementById(id);

    ctx = canvas.getContext('2d');

    setUpCanvas(canvas);

    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.beginPath();
    ctx.arc(canvas.width / 2, canvas.height / 2, canvas.width / 3, 0, 2 * Math.PI, false);
    ctx.lineWidth = circle.lineWidth;//30;
    ctx.strokeStyle = 'black';
    ctx.shadowColor = circle.shadowColor;
    ctx.shadowBlur = circle.shadowBlur//20;
    ctx.shadowOffsetY = 3;
    ctx.stroke();

    ctx.stroke();

    //Draw arc
    ctx.beginPath();
    ctx.arc(canvas.width / 2, canvas.height / 2, canvas.width / 3, circle.angleStartArc, draw_to);
    ctx.strokeStyle = isInside ? circle.gradientColor : circle.gradientaColor;
    //ctx.strokeStyle =circle.gradientaColor;
    ctx.lineWidth = circle.lineWidth;
    ctx.lineCap = 'round';
    ctx.stroke();
    circle.wasInside = isInside;

    // Increment percent
    circle.curr++;
    // Animate until end
    if (circle.curr < circle.finish + 1) {
        // Recursive repeat this function until the end is reached
        requestAnimationFrame(function () {
            drawCircle(circle, id, isInside, circle.circum * circle.curr / 100 + circle.angleStartArc);
        });
    }
    else {
        ctx.closePath();
    }

}




drawCircle(circleRdvJour1, 'circleRdvJour1', false);
drawCircle(circleRdvJour2, 'circleRdvJour2', false);
drawCircle(circleRdvJour3, 'circleRdvJour3', false);
drawCircle(circleRdvHorsEvt, 'circleRdvHorsEvt', false);

drawCircle(circleVenteJour1, 'circleVenteJour1', false);
drawCircle(circleVenteJour2, 'circleVenteJour2', false);
drawCircle(circleVenteJour3, 'circleVenteJour3', false);
drawCircle(circleVenteHorsEvt, 'circleVenteHorsEvt', false);



var rtime;
var timeout = false;
var delta = 200;

window.addEventListener('resize', () => {
    rtime = new Date();
    if (timeout === false) {
        timeout = true;
        setTimeout(resizeend, delta);
    }
});



function resizeend() {
    if (new Date() - rtime < delta) {
        setTimeout(resizeend, delta);
    } else {
        //console.log($(window).width());
        if ($(window).width() < 1000) {
            circleRdvJour1.lineWidth = 20;
            circleRdvJour2.lineWidth = 20;
            circleRdvJour3.lineWidth = 20;
            circleRdvHorsEvt.lineWidth = 20;


            circleVenteJour1.lineWidth = 20;
            circleVenteJour2.lineWidth = 20;
            circleVenteJour3.lineWidth = 20;
            circleVenteHorsEvt.lineWidth = 20;


            circleRdvJour1.shadowBlur = 20;
            circleRdvJour2.shadowBlur = 20;
            circleRdvJour3.shadowBlur = 20;
            circleRdvHorsEvt.shadowBlur = 20;

            circleVenteJour1.shadowBlur = 20;
            circleVenteJour2.shadowBlur = 20;
            circleVenteJour3.shadowBlur = 20;
            circleVenteHorsEvt.shadowBlur = 20;
        }
        else {
            circleRdvJour1.lineWidth = 30;
            circleRdvJour2.lineWidth = 30;
            circleRdvJour3.lineWidth = 30;
            circleRdvHorsEvt.lineWidth = 30;


            circleVenteJour1.lineWidth = 30;
            circleVenteJour2.lineWidth = 30;
            circleVenteJour3.lineWidth = 30;
            circleVenteHorsEvt.lineWidth = 30;


            circleRdvJour1.shadowBlur = 20;
            circleRdvJour2.shadowBlur = 20;
            circleRdvJour3.shadowBlur = 20;
            circleRdvHorsEvt.shadowBlur = 20;

            circleVenteJour1.shadowBlur = 20;
            circleVenteJour2.shadowBlur = 20;
            circleVenteJour3.shadowBlur = 20;
            circleVenteHorsEvt.shadowBlur = 20;
        }



        timeout = false;
        circleRdvJour1.curr = 0;
        circleRdvJour2.curr = 0;
        circleRdvJour3.curr = 0;
        circleRdvHorsEvt.curr = 0;

        circleVenteJour1.curr = 0;
        circleVenteJour2.curr = 0;
        circleVenteJour3.curr = 0;
        circleVenteHorsEvt.curr = 0;


        drawCircle(circleRdvJour1, 'circleRdvJour1', false);
        drawCircle(circleRdvJour2, 'circleRdvJour2', false);
        drawCircle(circleRdvJour3, 'circleRdvJour3', false);
        drawCircle(circleRdvHorsEvt, 'circleRdvHorsEvt', false);

        drawCircle(circleVenteJour1, 'circleVenteJour1', false);
        drawCircle(circleVenteJour2, 'circleVenteJour2', false);
        drawCircle(circleVenteJour3, 'circleVenteJour3', false);
        drawCircle(circleVenteHorsEvt, 'circleVenteHorsEvt', false);

    }
}

/********* Line charts display and configuration  ************/

Chart.defaults.global.plugins.datalabels.display = false;


var dataPack1 = [50, 75, 80, 100, 75];
var dataPack2 = [30, 50, 130, 140, 90];

var ctx = document.getElementById("lineChart");

var colors = {
    green: {
        fill: '#e0eadf',
        stroke: '#5eb84d',
    },
    lightBlue: {
        stroke: '#6fccdd',
    },
    darkBlue: {
        fill: '#92bed2',
        stroke: '#3282bf',
    },
    purple: {
        fill: '#8fa8c8',
        stroke: '#75539e',
    },
};

var data = {
    labels: ["", "Jour 2", "Jour 3", "Jour 4", ""],
    datasets: [
        {
            borderCapStyle: 'round',
            label: "Blue",
            fill: true,
            backgroundColor: 'rgba(46, 166, 222, 0.2)',
            pointBackgroundColor: colors.lightBlue.stroke,
            borderColor: 'rgba(46, 166, 222, 0.8)',
            borderWidth: 10,
            pointHighlightStroke: colors.lightBlue.stroke,
            pointRadius: 0,
            hoverBorderWidth: 5,
            hoverBorderColor: colors.lightBlue.stroke,
            data: dataPack2
        },
        {
            borderCapStyle: 'round',
            label: "Red",
            fill: true,
            backgroundColor: 'rgba(50, 185, 169,0.2)',
            pointBackgroundColor: 'rgba(50, 185, 169,0.8)',
            borderColor: 'rgba(50, 185, 169,0.8)',
            borderWidth: 10,
            pointHighlightStroke: colors.lightBlue.strokev,
            pointRadius: 0,
            hoverBorderWidth: 5,
            hoverBorderColor: colors.lightBlue.strokev,
            data: dataPack1,
        }
    ]
};

var lineOptions = {
    layout: {
        padding: {
            left: 15,
            right: 30,
            top: 15,
            bottom: 15
        }
    },
    scales: {
        xAxes: [{
            stacked: false,
            min: 0,
            ticks: {
                fontSize: 20,
                fontStyle: 'bold',
                fontColor: 'white',
            },
            gridLines: {
                display: false,
                drawBorder: true,
                color: 'white'
            },
        }],
        yAxes: [{
            stacked: false,
            display: true,
            min: 0,
            ticks: {
                min: 0,
                stepSize: 15,
                padding: 10,
                fontSize: 20,
                fontStyle: 'bold',
            },

            gridLines: {
                drawBorder: false,
                color: 'grey',
                lineWidth: 0.5,
            },
        }],
        animation: {
            duration: 750,
            easing: 'linear'
        },
    },
    legend: {
        display: false,
    },
    stepped: false,
    plugins: {
        //dataLabels: {
        //    display : false
        //}
    }
}

var myLineChart = new Chart(ctx, {
    type: 'line',
    data: data,
    options: lineOptions,
});






function explodePieceOnSelect() {
    $('#ExplodedPieChart').on('click', function (event) {
        var activePoints = myChart.getElementsAtEvent(event);

        if (activePoints.length > 0) {
            //get the internal index of slice in pie chart
            var clickedElementindex = activePoints[0]["_index"];

            //get specific label by index
            var clickedLabel = myChart.data.labels[clickedElementindex];

            if (currentSelectedPieceLabel.toUpperCase() == "") {
                // no piece selected yet, save piece label
                currentSelectedPieceLabel = clickedLabel.toUpperCase();

                // clear whole pie
                myChart.outerRadius = defaultRadiusMyChart;
                myChart.update();

                // update selected pie
                activePoints[0]["_model"].outerRadius = defaultRadiusMyChart + addRadiusMargin;
            }
            else {
                if (clickedLabel.toUpperCase() == currentSelectedPieceLabel.toUpperCase()) {
                    // already selected piece clicked, clear the chart
                    currentSelectedPieceLabel = "";

                    // clear whole pie
                    myChart.outerRadius = defaultRadiusMyChart;
                    myChart.update();

                    // update selected pie
                    activePoints[0]["_model"].outerRadius = defaultRadiusMyChart;
                }
                else {
                    // other piece clicked
                    currentSelectedPieceLabel = clickedLabel.toUpperCase();

                    // clear whole pie
                    myChart.outerRadius = defaultRadiusMyChart;
                    myChart.update();

                    // update the newly selected piece
                    activePoints[0]["_model"].outerRadius = defaultRadiusMyChart + addRadiusMargin;
                }
            }
            myChart.render(200, false);
        }
    });
}





var defaultRadiusMyChart;
var addRadiusMargin = 10;
var currentSelectedPieceLabel = "";
var myPie;
$(document).ready(function () {

    /****************** Exploded pie chart display and animation ************/

    var ctx = document.getElementById("ExplodedPieChart");

    var draw = Chart.controllers.pie.prototype.draw;
    Chart.controllers.pie = Chart.controllers.doughnut.extend({
        draw: function () {
            draw.apply(this, arguments);
            let ctx = this.chart.chart.ctx;
            let _fill = ctx.fill;
            ctx.fill = function () {
                ctx.save();
                ctx.shadowColor = '#32B9A9';
                ctx.shadowBlur = 120;
                _fill.apply(this, arguments)
                ctx.restore();
            }
        }
    });


     myPie = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ["Red", "Blue"],
            datasets: [{
                label: '# of Votes',
                data: [3, 9],
                backgroundColor: ['#2EA6DE', '#32B9A9'],
                borderColor: ['black', 'black'],
                borderWidth: [10, 10],
                weight: 25,
            }],
        },

        options: {
            reponsisve: true,
            maintainAspectRatio: false,
            legend: {
                display: false,
                position: 'right'
            },
            legendCallback: function (chart) {
                var text = [];

                text.push('<ul class="' + chart.id + '-legend">');
                for (var i = 0; i < chart.data.labels.length; i++) {
                    text.push('<li style="list-style-type:none;"><div class="legendValue"><span style="border-radius :15px;margin-right:15px;background-color:' + chart.data.datasets[0].backgroundColor[i] + '">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>');

                    if (chart.data.labels[i]) {
                        text.push('<span class="label" style="color:white;">' + chart.data.labels[i] + '</span><br> <br>');
                    }

                    text.push('</div></li><div class="clear"></div>');
                }

                text.push('</ul>');

                return text.join('');
            },


            layout: {
                padding: {
                    left: 0,
                    right: 0,
                    top: 50,
                    bottom: 50
                }
            },
            plugins: {
                datalabels: {
                    display: true,
                    color: 'white',
                    font: {
                        weight: 'bold',
                        size: 35
                    },
                    padding: 6,
                }
            }
        }
    });
    $("#pieLegends").html(myPie.generateLegend());







    defaultRadiusMyChart = myChart.outerRadius;

    explodePieceOnSelect();






});
