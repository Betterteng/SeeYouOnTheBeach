var map;
var beaches;
var userLat;
var userLng;
var markerUser;
var infowindowUser;
var markers = [];
var infowindows = [];


jQuery(document).ready(function ($) {

    var jssor_1_SlideoTransitions = [
      [{ b: 10000, d: 2000, x: -379, e: { x: 7 } }]
    ];

    var jssor_1_options = {
        $AutoPlay: true,
        $SlideDuration: 800,
        $SlideEasing: $Jease$.$OutQuint,
        $CaptionSliderOptions: {
            $Class: $JssorCaptionSlideo$,
            $Transitions: jssor_1_SlideoTransitions
        },
        $ArrowNavigatorOptions: {
            $Class: $JssorArrowNavigator$
        },
        $BulletNavigatorOptions: {
            $Class: $JssorBulletNavigator$
        }
    };

    var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);

    //responsive code begin
    //you can remove responsive code if you don't want the slider scales while window resizing
    function ScaleSlider() {
        var refSize = jssor_1_slider.$Elmt.parentNode.clientWidth;
        if (refSize) {
            refSize = Math.min(refSize, 1920);
            jssor_1_slider.$ScaleWidth(refSize);
        }
        else {
            window.setTimeout(ScaleSlider, 30);
        }
    }
    ScaleSlider();
    $(window).bind("load", ScaleSlider);
    $(window).bind("resize", ScaleSlider);
    $(window).bind("orientationchange", ScaleSlider);
    //responsive code end
});

function geolocateHome() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            userLat = geolocation.lat;
            userLng = geolocation.lng;
            markerUser = new google.maps.Marker({
                position: geolocation
            });
            var tooltipUser = '<div class="iw-content">' +
                'You are Here' +
                '</div>';

            infowindowUser = new google.maps.InfoWindow({
                content: tooltipUser
            });
            markerUser.setMap(map);
            infowindowUser.open(map, markerUser);
            markerUser.addListener('click', function () {
                closeAllInfoWindowFromInfoArray(infowindows);
                infowindowUser.open(map, markerUser);
            });
            google.maps.event.addListener(infowindowUser, 'domready', function () {
                infoWindowStyle2();
            });
        });
    }
};

function initMap(beach) {
    beaches = beach;
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -37.86488400000000, lng: 144.97035800000000 },
        zoom: 11
    });

    //init marker array
    for (var index = 0; index < beaches.length; index++) {
        var marker = new google.maps.Marker({
            position: { lat: beaches[index].Latitude, lng: beaches[index].Longitude },
            icon: 'StaticImages/BeachIcon.png'
        });
        markers.push(marker);
        marker.setMap(map);
        var tooltip = '<div>' +
            '<div class="iw-title">' +
            beaches[index].BeachName +
            '</div>' +
            '<div class=iw-content style="text-align:center">' +
            '<a href = "BeachDetail?beachid=' +
            beaches[index].BeachId.toString() +
            '">Explore!</a>' +
            '</div>' +
            '<div class="iw-bottom-gradient"></div>' +
            '</div>';

        var infowindow = new google.maps.InfoWindow({
            content: tooltip
        });

        infowindows.push(infowindow);
    };

    //init infowindow array
    for (var index2 = 0; index2 < markers.length; index2++) {
        var infoWindow = infowindows[index2];
        function openInfoWindow(i, info, array, infoarray) {
            return function () {
                closeAllInfoWindowFromInfoArray(infoarray);
                infowindowUser.close();
                info.open(map, array[i]);
            }
        }
        markers[index2].addListener('click', openInfoWindow(index2, infoWindow, markers, infowindows));
        markers[index2].setMap(map);
        google.maps.event.addListener(infoWindow, 'domready', function () {
            infoWindowStyle();
        });
    }

    //click event for closing all infowindow
    google.maps.event.addListener(map,
        "click",
        function closeAll() { return closeAllInfoWindowFromInfoArray(infowindows) });

    //open 1st infowindow
    //locate user
    geolocateHome();
};

function closeAllInfoWindowFromInfoArray(array) {
    for (var j = 0; j < array.length; j++) {
        array[j].close();
    }
    infowindowUser.close();
}

function infoWindowStyle() {
    var iwOuter = $('.gm-style-iw');
    var height = iwOuter.css('height');
    var parentHeightNum = parseInt(iwOuter.parent().css('height').replace('px', ''));
    var heightNum = parseInt(height.replace('px', ''));
    var marginTop = (parentHeightNum - heightNum).toString() + 'px';

    var width = iwOuter.css('width');
    iwOuter.parent().css({ 'width': width, 'height': height, 'margin-top': marginTop });

    var iwBackground = iwOuter.prev();
    iwBackground.children(':nth-child(1)').css({ 'top': height });
    iwBackground.children(':nth-child(2)').css({ 'display': 'none' });
    iwBackground.children(':nth-child(4)').css({ 'display': 'none' });
    iwBackground.children(':nth-child(3)').css({ 'top': height });
    var iwCloseBtn = iwOuter.next();
    iwCloseBtn.css({ opacity: '1', right: '-7px', top: '-7px', border: '7px solid #000000', 'border-radius': '13px', 'box-shadow': '0 0 10px #3990B9' });

    if ($('.iw-content').height() < 140) {
        $('.iw-bottom-gradient').css({ display: 'none' });
    }
}

function infoWindowStyle2() {
    var iwOuter = $('.gm-style-iw');
    var height = iwOuter.css('height');
    var parentHeightNum = parseInt(iwOuter.parent().css('height').replace('px', ''));
    var heightNum = parseInt(height.replace('px', ''));
    var marginTop = (parentHeightNum - heightNum).toString() + 'px';

    var width = iwOuter.css('width');
    iwOuter.parent().css({ 'width': width, 'height': height, 'margin-top': marginTop });

    var iwBackground = iwOuter.prev();
    iwBackground.children(':nth-child(1)').css({ 'display': 'none' });
    iwBackground.children(':nth-child(2)').css({ 'display': 'none' });
    iwBackground.children(':nth-child(4)').css({ 'display': 'none' });
    iwBackground.children(':nth-child(3)').css({ 'display': 'none' });
    var iwCloseBtn = iwOuter.next();
    iwCloseBtn.css({ 'display': 'none' });

    if ($('.iw-content').height() < 140) {
        $('.iw-bottom-gradient').css({ display: 'none' });
    }
}