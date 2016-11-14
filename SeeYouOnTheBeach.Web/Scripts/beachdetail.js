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

function initAutocomplete() {
    autocomplete = new google.maps.places.Autocomplete(
        (document.getElementById('startT')),
        { types: ['geocode'] }
        );
}

function geolocate() {
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
}

//beach info
var beach;
var lat;
var lng;
var description;
//
function initBeach(beachname, lati, lngi, desc) {
    beach = beachname;
    lat = lati;
    lng = lngi;
    description = desc;
}

function initWeather(fromModel) {
    weather = fromModel;
    initWeatherDatapoint();
}

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
                        '<div class="iw-title">'+desc1+'</div>'+
                        '<div class="iw-content">' +
                            '<div class="iw-subTitle">'+desc2+'</div>' +
                            '<div class="iw-subTitle">'+desc3+'</div>' +
                        '</div>' +
                        '<div class="iw-bottom-gradient"></div>'+
                     '</div>'
        });
        
        google.maps.event.addListener(infowindow, 'domready', function () {

            // Reference to the DIV that wraps the bottom of infowindow
            var iwOuter = $('.gm-style-iw');

            /* Since this div is in a position prior to .gm-div style-iw.
             * We use jQuery and create a iwBackground variable,
             * and took advantage of the existing reference .gm-style-iw for the previous div with .prev().
            */
            var iwBackground = iwOuter.prev();

            // Removes background shadow DIV
            iwBackground.children(':nth-child(2)').css({ 'display': 'none' });

            // Removes white background DIV
            iwBackground.children(':nth-child(4)').css({ 'display': 'none' });

            // Changes the desired tail shadow color.
            iwBackground.children(':nth-child(3)').css({ 'display': 'none' });

            // Reference to the div that groups the close button elements.
            var iwCloseBtn = iwOuter.next();

            // Apply the desired effect to the close button
            iwCloseBtn.css({ opacity: '1', right: '38px', top: '3px', border: '7px solid #48b5e9', 'border-radius': '13px', 'box-shadow': '0 0 5px #3990B9' });

            // If the content of infowindow not exceed the set maximum height, then the gradient is removed.
            if ($('.iw-content').height() < 140) {
                $('.iw-bottom-gradient').css({ display: 'none' });
            }
            // The API automatically applies 0.7 opacity to the button after the mouseout event. This function reverses this event to the desired value.
            iwCloseBtn.mouseout(function () {
                $(this).css({ opacity: '1' });
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
}

function displayMarkers(array, infoArray) {
    clearContent('#mapAndWeatherPannel');
    writeMapDiv();
    initializeMap(array, infoArray);
}

function displayRoute() {
    clearContent('#mapAndWeatherPannel');
    writeMapDiv();
    initializeMap(null, null);
    appendFloatPanel();
}

function displayWeather() {
    clearContent('#mapAndWeatherPannel');
    writeChartDiv();
    loadChart();
}

function clearContent(select) {
    $(select).off('click');
    $(select).html('');
}

function appendFloatPanel() {
    var innerhtml = '';
    innerhtml += '<div id="floating-panel">';
    innerhtml += '<b>Start: </b>';
    innerhtml += '<input type="text" id="startT" onFocus="geolocate()" style="height:20px;">';
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
    innerhtml += '<div>' + '<button class="buttonSide" id="routeButton" style="width:20%;">Route</button>' + '</div>';
    innerhtml += '</div>';
    $('#mapAndWeatherPannel').append(innerhtml);
    initAutocomplete();
    var directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer;
    directionsDisplay.setMap(map);
    var onChangeHandler = function () {
        calculateAndDisplayRoute(directionsService, directionsDisplay);
    };
    $('#routeButton').click(onChangeHandler);
}

function writeMapDiv() {
    document.getElementById("mapAndWeatherPannel").innerHTML = '<div id="googleMap" style = "width:auto;height:500px;"></div>';
}

function writeChartDiv() {
    document.getElementById("mapAndWeatherPannel").innerHTML = '<div id="chartContainer" style = "width:auto;height:500px;"></div>';
}

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
        icon: 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png'
    });
    marker.setMap(map);
    if (markArray != null && infoArray != null) {
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
}

function calculateAndDisplayRoute(directionsService, _directionsDisplay) {
    var origin = document.getElementById('startT');
    var value = $('<div/>').text(origin.value).html().replace(/&/g, '%26');
    var mode = document.getElementById('mode').value;
    if (value.indexOf('%') !== -1) {
        alert("stop trying");
        return;
    }

    if (mode === "DRIVING") {
        directionsService.route({
            origin: document.getElementById('startT').value,
            destination: document.getElementById('end').value,
            travelMode: google.maps.TravelMode.DRIVING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                _directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }

    if (mode === "TRANSIT") {
        directionsService.route({
            origin: document.getElementById('startT').value,
            destination: document.getElementById('end').value,
            travelMode: google.maps.TravelMode.TRANSIT
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                _directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }

    if (mode === "WALKING") {
        directionsService.route({
            origin: document.getElementById('startT').value,
            destination: document.getElementById('end').value,
            travelMode: google.maps.TravelMode.WALKING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                _directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }

    if (mode === "BICYCLING") {
        directionsService.route({
            origin: document.getElementById('startT').value,
            destination: document.getElementById('end').value,
            travelMode: google.maps.TravelMode.BICYCLING
        }, function (response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                _directionsDisplay.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
    }
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
}

function showCurrentWeather() {
    var imgURL = weather.current_observation.icon_url;
    var temp = weather.current_observation.feelslike_c;
    var uv = weather.current_observation.UV;
    var humidity = weather.current_observation.relative_humidity;
    var condition = weather.current_observation.weather;
    var wind = weather.current_observation.wind_kph;
    document.getElementById("weatherImg").src = imgURL;
    document.getElementById('temp').innerHTML = temp;
    document.getElementById('uv').innerHTML = uv;
    document.getElementById('humidity').innerHTML = humidity;
    document.getElementById('condition').innerHTML = condition;
    document.getElementById('wind').innerHTML = wind;
}

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
}

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
}

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
}
