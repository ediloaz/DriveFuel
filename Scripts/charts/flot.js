/**
 * Flot chart page
 */
(function ($) {
    'use strict';

    var data = [],
      previousPoint = null,
      plot;

    var getRandomArbitrary = function () {
        return Math.round(Math.random() * 100);
    };

    function gd(year, month, day) {
        return new Date(year, month - 1, day).getTime();
    }

    function gd2(da) {
        var dateString = da;
        var dateParts = dateString.split("/");
        var dateObject = new Date(dateParts[2], dateParts[1] - 1, dateParts[0]);
        return dateObject.getTime();
    }

    //var visits = [
    //  [gd(2018,1,1), getRandomArbitrary()],
    //  [gd(2018,1,2), getRandomArbitrary()],
    //  [gd(2018,1,3), getRandomArbitrary()],
    //  [gd(2018,1,4), getRandomArbitrary()],
    //  [gd(2018,1,5), getRandomArbitrary()],
    //  [gd(2018,1,6), getRandomArbitrary()],
    //  [gd(2018,1,7), getRandomArbitrary()],
    //  [gd(2018,1,8), getRandomArbitrary()],
    //  [gd(2018,1,9), getRandomArbitrary()],
    //  [gd(2018,1,10), getRandomArbitrary()]
    //];

    var visits = [];
    $.each(datosGrafica,function(i,value){
        visits.push(Object.values(value));
    });

    $.each(visits, function (y, valor) {
        visits[y][0] = gd2(valor[0]);
    });

    var plotdata = [{
        data: visits,
        color: "#477628",
        label: 'Visitas',
        points: { fillColor: "#477628", show: true },
    }
    //,{
    //    data: visitors,
    //    color: $.staticApp.info
    //}
    ];

    var dayOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thr", "Fri", "Sat"];

   function showTooltip(x, y, contents) {
        $('<div id=\'tooltip\'>' + contents + '</div>').css({
            top: y - 10,
            left: x + 20
        }).appendTo('body').fadeIn(200);
    }

    // Line chart
    /*jshint -W030 */
    $('.line').length && $.plot($('.line'), plotdata, {
        series: {
            lines: {
                show: true,
                lineWidth: 0,
            },
            splines: {
                show: true,
                tension: 0.5,
                lineWidth: 1,
                fill: 0.2,
            },
            shadowSize: 0
        },
        legend: {
            show: true,
            container: $("#legend-container"),
            noColumns: 1,
        },
        grid: {
            color: $.staticApp.border,
            borderWidth: 1,
            hoverable: true,
        },
        xaxes: [{
                mode: "time",
                timeformat: "%d/%m",
                tickSize: [1, "day"],
                axisLabel: "Date",
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Verdana, Arial',
                axisLabelPadding: 30
            }],
    });

    Date.prototype.yyyymmdd = function () {
        var mm = this.getMonth() + 1; // getMonth() is zero-based
        var dd = this.getDate();

        return [(dd > 9 ? '' : '0') + dd,
                (mm > 9 ? '' : '0') + mm,
                this.getFullYear()            
                ].join('/');
    };

    var convertTime = function (timestamp, separator) {
        var pad = function (input) { return input < 10 ? "0" + input : input; };
        var date = timestamp ? new Date(timestamp) : new Date();
        //return [
        //    pad(date.getDay()),
        //    pad(date.getMonth()+1),
        //    date.getFullYear()
        //].join(typeof separator !== 'undefined' ? separator : ':');
        var b = date.yyyymmdd();
        return b;
    }

    // Chart tooltip
    $('.chart, .chart-sm').bind('plothover', function (event, pos, item) {
        if (item) {
            if (previousPoint !== item.dataIndex) {
                previousPoint = item.dataIndex;
                $('#tooltip').remove();
                var x = convertTime(item.datapoint[0],'/'),
                  y = item.datapoint[1];
                showTooltip(item.pageX, item.pageY, y + ' el ' + x);
            }
        } else {
            $('#tooltip').remove();
            previousPoint = null;
        }
    });

})(jQuery);