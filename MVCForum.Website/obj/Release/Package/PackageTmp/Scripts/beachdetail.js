var map;
var sports_mark = [];
var sports_infowindow = [];
var toilets_mark = [];
var toilets_infowindow = [];
var hospitals_mark = [];
var hospitals_infowindow = [];
var weather = [];
var dataPoint = [];
var directionsDisplay;
var barbecues_mark = [];
var barbecues_infowindow = [];
var bikeshare_mark = [];
var bikeshare_infowindow = [];
var placeSearch, autocomplete;
var myLat;
var myLng;
var directionsService;
var routeMarkers = [];
var beach;
var lat;
var lng;
var description;

//auto fill
function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
        (document.getElementById('startT')),
        { types: ['geocode'] }
        );
};

//auto fill - bias address
function geolocateAutoComplete() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
};

//route - use my location
function geolocateRoute() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            myLat = position.coords.latitude;
            myLng = position.coords.longitude;
            calculateAndDisplayRoute(directionsService, directionsDisplay, true);
        }, function () { alert("Please allow google to access your loaction first"); });
    }
};

//beach info
function initBeach(beachname, lati, lngi, desc) {
    beach = beachname;
    lat = lati;
    lng = lngi;
    description = desc;
}

//weather info
function initWeather(fromModel) {
    weather = fromModel;
    initWeatherDatapoint();
};

//populate marker array
function initMarker(fromJson, toArray, toInfoWindowArray, name, id) {
    if (fromJson == null || fromJson.length === 0) {
        return;
    }
    for (var index = 0; index < fromJson.length; index++) {
        var dataItem = fromJson[index];
        var desc1 = dataItem.Description1;
        var desc2 = dataItem.Description2;
        var desc3 = dataItem.Description3;
        var location = { lat: dataItem.Lat, lng: dataItem.Lng };
        var marker = new google.maps.Marker({
            position: location,
            icon: '../StaticImages/' + id + '.png'
        });
        var infowindow = new google.maps.InfoWindow({
            content: '<div>' +
                        '<div class="iw-title">' + desc1 + '</div>' +
                        '<div class="iw-content">' +
                            '<div class="iw-subTitle">' + desc2 + '</div>' +
                            '<div class="iw-subTitle">' + desc3 + '</div>' +
                        '</div>' +
                        '<div class="iw-bottom-gradient"></div>' +
                     '</div>'
        });

        google.maps.event.addListener(infowindow, 'domready', function () {

            // Reference to the DIV that wraps the bottom of infowindow
            var iwOuter = $('.gm-style-iw');
            var height = iwOuter.css('height');
            var parentHeightNum = parseInt(iwOuter.parent().css('height').replace('px', ''));
            var heightNum = parseInt(height.replace('px', ''));
            var marginTop = (parentHeightNum - heightNum).toString() + 'px';

            var width = iwOuter.css('width');
            iwOuter.parent().css({ 'width': width, 'height': height, 'margin-top': marginTop });

            /* Since this div is in a position prior to .gm-div style-iw.
             * We use jQuery and create a iwBackground variable,
             * and took advantage of the existing reference .gm-style-iw for the previous div with .prev().
            */
            var iwBackground = iwOuter.prev();
            iwBackground.children(':nth-child(1)').css({ 'top': height });
            // Removes background shadow DIV
            iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

            // Removes white background DIV
            iwBackground.children(':nth-child(4)').css({ 'display': 'none' });

            iwBackground.children(':nth-child(3)').css({ 'top': height });

            // Reference to the div that groups the close button elements.
            var iwCloseBtn = iwOuter.next();

            // Apply the desired effect to the close button
            iwCloseBtn.css({ opacity: '1', right: '-7px', top: '-7px', border: '7px solid #000000', 'border-radius': '13px', 'box-shadow': '0 0 10px #3990B9' });

            // If the content of infowindow not exceed the set maximum height, then the gradient is removed.
            if ($('.iw-content').height() < 140) {
                $('.iw-bottom-gradient').css({ display: 'none' });
            }
            // The API automatically applies 0.7 opacity to the button after the mouseout event. This function reverses this event to the desired value.
            iwCloseBtn.mouseout(function () {
                $(this).css({ opacity: '0.7' });
            });
        });

        toArray.push(marker);
        toInfoWindowArray.push(infowindow);
    };
    var innerHtml =
        '<div>' +
            '<button class="buttonSide" id ="' + id + '" style="width:80%;">' +
                name +
            '</button>' +
        '</div>';
    $('#buttons_container').append(innerHtml);
    $('#' + id).on('click',
        function display() {
            displayMarkers(toArray, toInfoWindowArray);

        });
};

function displayMarkers(array, infoArray) {
    clearContent('#mapAndWeatherPannel');
    writeMapDiv();
    initializeMap(array, infoArray);
};

function displayRoute() {
    clearContent('#mapAndWeatherPannel');
    writeMapDiv();
    initializeMap(null, null);
    appendFloatPanel();
};

function displayWeather() {
    clearContent('#mapAndWeatherPannel');
    writeChartDiv();
    loadChart();
};

function clearContent(select) {
    $(select).off('click');
    $(select).html('');
};

function appendFloatPanel() {
    var innerhtml = '';
    innerhtml += '<div id="floating-panel">';
    innerhtml += '<b>Start: </b>';
    innerhtml += '<form>' + '<input type="text" id="startT" onFocus="geolocateAutoComplete()" style="line-height:normal;">' + '</form>';
    innerhtml += '<b style="padding-left:10px;">End: </b>';
    innerhtml += '<select id="end">';
    innerhtml += '   <option value="' + lat + ',' + lng + '">' + beach + '</option>';
    innerhtml += '</select>';
    innerhtml += '<b style="padding-left:10px;">Mode: </b>';
    innerhtml += '<select id="mode">';
    innerhtml += '   <option value="DRIVING">Driving</option>';
    innerhtml += '   <option value="WALKING">Walking</option>';
    innerhtml += '   <option value="BICYCLING">Bicycling</option>';
    innerhtml += '   <option value="TRANSIT">Public Transport</option>';
    innerhtml += '</select>';
    innerhtml += '<div>' + '<button class="buttonSide" id="routeButton" style="width:200px;">Route</button>';
    innerhtml += '<button class="buttonSide" id="getMyLocation" style="width:200px;">Use My Location</button>' + '</div>';
    innerhtml += '</div>';
    $('#mapAndWeatherPannel').append(innerhtml);
    initAutocomplete();
    directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer;
    directionsDisplay.setMap(map);
    directionsDisplay.setOptions({ suppressMarkers: true });
    var routeClickHandler = function () {
        calculateAndDisplayRoute(directionsService, directionsDisplay, false);
    };
    var selectListHandler = function() {
        if ($('#startT').val() === '') {
            geolocateRoute();
        } else {
            calculateAndDisplayRoute(directionsService, directionsDisplay, false);
        }
    };
    $('#routeButton').click(routeClickHandler);
    $('#getMyLocation').click(geolocateRoute);
    $('#mode').change(selectListHandler);
    $('#startT').keydown(function (event) {
        var keypressed = event.keyCode || event.which;
        if (keypressed === 13) {
            event.stopPropagation();
            event.preventDefault();
            routeClickHandler(); 
        }
    });
};

function writeMapDiv() {
    document.getElementById("mapAndWeatherPannel").innerHTML = '<div id="googleMap" style = "width:auto;height:500px;"></div>';
};

function writeChartDiv() {
    document.getElementById("mapAndWeatherPannel").innerHTML = '<div id="chartContainer" style = "width:auto;height:500px;"></div>';
};

function initializeMap(markArray, infoArray) {

    var mylat = lat;
    var mylng = lng;
    var myLatLng = { lat: mylat, lng: mylng };
    var mapProp = {
        center: myLatLng,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);
    var marker = new google.maps.Marker(
    {
        position: myLatLng,
        Title: beach,
        icon: '../StaticImages/BeachIcon.png'
    });  
    marker.setMap(map);
    if (markArray != null && infoArray != null) {
        closeAllInfoWindowFromInfoArray(infoArray);
        for (var index = 0; index < markArray.length ; index++) {
            var infoWindow = infoArray[index];
            function openInfoWindow(i, info, array, infoarray) {
                return function () {
                    closeAllInfoWindowFromInfoArray(infoarray);
                    info.open(map, array[i]);
                }
            }
            markArray[index].addListener('click', openInfoWindow(index, infoWindow, markArray, infoArray));
            markArray[index].setMap(map);
        }
        google.maps.event.addListener(map,
            "click",
            function closeAll() { return closeAllInfoWindowFromInfoArray(infoArray) });
    }
}

function closeAllInfoWindowFromInfoArray(array) {
    for (var j = 0; j < array.length; j++) {
        array[j].close();
    }
};

function calculateAndDisplayRoute(directionsService, directionsDisplay, myLocation) {
    
    var origin = document.getElementById('startT').value;
    if (myLocation === true) {
        if (myLat != null) {
            origin = myLat.toString() + ',' + myLng.toString();
            var latlng = { lat: myLat, lng: myLng };
            var geocoder = new google.maps.Geocoder;
            geocoder.geocode({ 'location': latlng },
                function (results, status) {
                    if (status === 'OK') {
                        if (results[1]) {
                            $('#startT').val(results[1].formatted_address);
                        } else {
                            window.alert('No results found');
                        }
                    } else {
                        window.alert('Geocoder failed due to: ' + status);
                    }
                });          
        } else {
            alert('Please allow google to access your loaction first');
            return;
        }
    }
    var value = $('<div/>').text(origin).html().replace(/&/g, '%26');
    var mode = document.getElementById('mode').value;
    if (value.indexOf('%') !== -1) {
        alert('stop trying');
        return;
    }

    var stepDisplay = new google.maps.InfoWindow;
    for (var i = 0; i < routeMarkers.length; i++) {
        routeMarkers[i].setMap(null);
    }
    routeMarkers = [];

    directionsService.route({
        origin: origin,
        destination: document.getElementById('end').value,
        travelMode: mode
    }, function (response, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
            showSteps(response, routeMarkers, stepDisplay, map);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
};

function showSteps(directionResult, markerArray, stepDisplay, map) {
    // For each step, place a marker, and add the text to the marker's infowindow.
    // Also attach the marker to an array so we can keep track of it and remove it
    // when calculating new routes.
    var myRoute = directionResult.routes[0].legs[0];
    for (var i = 0; i < myRoute.steps.length; i++) {
        var marker = markerArray[i] = markerArray[i] || new google.maps.Marker;
        marker.setMap(map);
        marker.setPosition(myRoute.steps[i].start_location);
        attachInstructionText(
            stepDisplay, marker, myRoute.steps[i].instructions, map);
    }
}
function attachInstructionText(stepDisplay, marker, text, map) {
    google.maps.event.addListener(marker, 'click', function () {
        // Open an info window when the marker is clicked on, containing the text
        // of the step.
        stepDisplay.setContent(text);
        google.maps.event.addListener(stepDisplay, 'domready', function () {
            var iwOuter = $('.gm-style-iw');
            iwOuter.css({
                'width': 'initial',
                'top': '50%',
                'left': '50%',
                'background-color': 'initial',
                'box-shadow': 'initial',
                'border': 'initial',
                'border-radius': 'initial'
            });
        });
        stepDisplay.open(map, marker);
    });
}


function initWeatherDatapoint() {
    for (var i = 0; i < weather.hourly_forecast.length; i++) {
        var hour = weather.hourly_forecast[i];
        var year = parseInt(hour.FCTTIME.year);
        var mon = parseInt(hour.FCTTIME.mon) - 1;
        var day = parseInt(hour.FCTTIME.mday);
        var dHour = parseInt(hour.FCTTIME.hour);
        var date = new Date(year, mon, day, dHour, 00);
        var temp = parseInt(hour.temp.metric);
        var data = { x: date, y: temp, uv: hour.uvi, humidity: hour.humidity, condition: hour.condition, image: hour.icon_url };
        dataPoint.push(data);
    }
};

function showCurrentWeather() {
    var imgURL = weather.current_observation.icon_url;
    var temp = weather.current_observation.feelslike_c;
    var uv = weather.current_observation.UV;
    var uvInt = parseInt(uv);
    var humidity = weather.current_observation.relative_humidity;
    var condition = weather.current_observation.weather;
    var wind = weather.current_observation.wind_kph;
    document.getElementById("weatherImg").src = imgURL;
    document.getElementById('temp').innerHTML = temp;
    document.getElementById('uv').innerHTML = uv;
    if (uvInt > 3) {
        document.getElementById('suggestion').innerHTML = '<strong>High (Use of sunscreen is mandatory)</strong>';
    } else {
        document.getElementById('suggestion').innerHTML = '<strong>Low (Use of sunscreen is recommended)</strong>';
    }
    document.getElementById('humidity').innerHTML = humidity;
    document.getElementById('condition').innerHTML = condition;
    document.getElementById('wind').innerHTML = wind;
};

function weatherForMobile() {
    var isMobile = false;
    if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) {
        isMobile = true;
    }
    if (isMobile) {
        $('#weatherForMobile').show();
        $('#weatherBtn').hide();
        document.getElementById('weatherWidget').style.display = 'none';
    } else {
        $('#weatherForMobile').hide();
        $('#weatherBtn').show();
    }

    var info1 = weather.forecast.simpleforecast.forecastday[1];
    var weekday1 = info1.date.weekday;
    var imgURL1 = info1.icon_url;
    var high1 = info1.high.celsius;
    var low1 = info1.low.celsius;
    var humidity1 = info1.avehumidity;
    var condition1 = info1.conditions;
    var wind1 = info1.avewind.kph;
    document.getElementById("weekday1").innerHTML = weekday1;
    document.getElementById("weatherImg1").src = imgURL1;
    document.getElementById('high1').innerHTML = high1;
    document.getElementById('low1').innerHTML = low1;
    document.getElementById('humidity1').innerHTML = humidity1;
    document.getElementById('condition1').innerHTML = condition1;
    document.getElementById('wind1').innerHTML = wind1;

    var info2 = weather.forecast.simpleforecast.forecastday[2];
    var weekday2 = info2.date.weekday;
    var imgURL2 = info2.icon_url;
    var high2 = info2.high.celsius;
    var low2 = info2.low.celsius;
    var humidity2 = info2.avehumidity;
    var condition2 = info2.conditions;
    var wind2 = info2.avewind.kph;
    document.getElementById("weekday2").innerHTML = weekday2;
    document.getElementById("weatherImg2").src = imgURL2;
    document.getElementById('high2').innerHTML = high2;
    document.getElementById('low2').innerHTML = low2;
    document.getElementById('humidity2').innerHTML = humidity2;
    document.getElementById('condition2').innerHTML = condition2;
    document.getElementById('wind2').innerHTML = wind2;

    var info3 = weather.forecast.simpleforecast.forecastday[3];
    var weekday3 = info3.date.weekday;
    var imgURL3 = info3.icon_url;
    var high3 = info3.high.celsius;
    var low3 = info3.low.celsius;
    var humidity3 = info3.avehumidity;
    var condition3 = info3.conditions;
    var wind3 = info3.avewind.kph;
    document.getElementById("weekday3").innerHTML = weekday3;
    document.getElementById("weatherImg3").src = imgURL3;
    document.getElementById('high3').innerHTML = high3;
    document.getElementById('low3').innerHTML = low3;
    document.getElementById('humidity3').innerHTML = humidity3;
    document.getElementById('condition3').innerHTML = condition3;
    document.getElementById('wind3').innerHTML = wind3;
};

function getHighestTemp() {
    var highestTemp0 = weather.forecast.simpleforecast.forecastday[0].high.celsius;
    var highestTemp1 = weather.forecast.simpleforecast.forecastday[1].high.celsius;
    var highestTemp2 = weather.forecast.simpleforecast.forecastday[2].high.celsius;

    var ht0 = parseInt(highestTemp0);
    var ht1 = parseInt(highestTemp1);
    var ht2 = parseInt(highestTemp2);

    var results = [ht0, ht1, ht2];
    for (var i = 0; i < results.length; i++) { //Number of passes
        for (var j = 0; j < (results.length - i - 1) ; j++) { //Notice that j < (length - i)
            //Compare the adjacent positions
            if (results[j] > results[j + 1]) {
                //Swap the numbers
                var tmp = results[j];  //Temporary variable to hold the current number
                results[j] = results[j + 1]; //Replace current number with adjacent number
                results[j + 1] = tmp; //Replace adjacent number with current number
            }
        }
    }
    return results[2] + 2;
};

function getLowestTemp() {
    var lowestTemp0 = weather.forecast.simpleforecast.forecastday[0].low.celsius;
    var lowestTemp1 = weather.forecast.simpleforecast.forecastday[1].low.celsius;
    var lowestTemp2 = weather.forecast.simpleforecast.forecastday[2].low.celsius;

    var lt0 = parseInt(lowestTemp0);
    var lt1 = parseInt(lowestTemp1);
    var lt2 = parseInt(lowestTemp2);

    var results = [lt0, lt1, lt2];
    for (var i = 0; i < results.length; i++) { //Number of passes
        for (var j = 0; j < (results.length - i - 1) ; j++) { //Notice that j < (length - i)
            //Compare the adjacent positions
            if (results[j] > results[j + 1]) {
                //Swap the numbers
                var tmp = results[j];  //Temporary variable to hold the current number
                results[j] = results[j + 1]; //Replace current number with adjacent number
                results[j + 1] = tmp; //Replace adjacent number with current number
            }
        }
    }
    return results[0] - 2;
};

function loadChart() {
    var chart = new CanvasJS.Chart("chartContainer",
    {
        zoomEnabled: true,
        animationEnabled: true,
        title: {
            text: beach
        },
        axisX: {
            interval: 1
        },
        axisY2: {
            valueFormatString: "0.0 ℃",
            maximum: getHighestTemp(),
            minimum: getLowestTemp(),
            interval: 2,
            interlacedColor: "#F5F5F5",
            gridColor: "#D7D7D7",
            tickColor: "#D7D7D7"
        },
        theme: "theme2",
        toolTip: {
            shared: true
        },
        legend: {
            verticalAlign: "bottom",
            horizontalAlign: "center",

            fontFamily: "Lucida Sans Unicode"

        },
        data: [
        {
            type: "line",
            lineThickness: 3,
            axisYType: "secondary",
            showInLegend: true,
            name: "Temperature in 36 Hours",
            toolTipContent: "<span style='\"'color: {color};'\"'><strong>{name}</strong><br/><img src={image}></span><br/><strong>Time</strong> {x} <br/> <strong>Temperature</strong> {y} ℃<br/> <strong>UV</strong> {uv} Degree <br/> <strong>Humidity</strong> {humidity} %<br/> <strong>Condition</strong> {condition}<br/>  ",
            dataPoints: dataPoint
        }
        ]
    });

    chart.render();
};
function backToList() {
    window.location.href = '../Filter';
};

function findPeople() {
    window.location.href = '../Home';
}

window.smoothScroll = function (target) {
    var scrollContainer = target;
    do { //find scroll container
        scrollContainer = scrollContainer.parentNode;
        if (!scrollContainer) return;
        scrollContainer.scrollTop += 1;
    } while (scrollContainer.scrollTop == 0);

    var targetY = 0;
    do { //find the top of target relatively to the container
        if (target == scrollContainer) break;
        targetY += target.offsetTop;
    } while (target = target.offsetParent);

    scroll = function (c, a, b, i) {
        i++; if (i > 30) return;
        c.scrollTop = a + (b - a) / 30 * i;
        setTimeout(function () { scroll(c, a, b, i); }, 20);
    }
    // start scrolling
    scroll(scrollContainer, scrollContainer.scrollTop, targetY, 0);
}