var features = [];
var appliedFilters = [];
var remainingBeaches = [];



function hideBeachDivById(id) {
    $('#beach_' + id).hide();
}

function displayBeachDivById(id) {
    $('#beach_' + id).show();
}

function checkBoxOnChange() {
    appliedFilters = [];
    $('[name="filterGroup"]').each(function () {
        if ($(this).is(':checked')) {
            var id = parseInt($(this).attr('value'));
            appliedFilters.push(id);
        }
    });
    applyFilters();
}

function initFeatures(fromModel) {
    features = fromModel;
    remainingBeaches = features;
}

function initRemainingBeaches() {
    remainingBeaches = Enumerable.From(features).Select('f => f.BeachId').ToArray();
}

function displayBeaches() {
    Enumerable.From(features).ForEach(function (i) { hideBeachDivById(i.BeachId) });
    Enumerable.From(remainingBeaches).ForEach(function (i) { displayBeachDivById(i) });
}

function applyFilters() {
    if (appliedFilters.length === 0) {
        initRemainingBeaches();
        displayBeaches();
        return;
    }
    remainingBeaches = [];
    Enumerable.From(features)
        .ForEach(function (i) {
            var feature = Enumerable.From(i.BeachFeatures).Select('j => j.BeachFilterId').ToArray();
            var length = Enumerable.From(feature).Intersect(Enumerable.From(appliedFilters)).ToArray().length;
            //further, can partially match
            if (length === appliedFilters.length) {
                remainingBeaches.push(parseInt(i.BeachId));
            }
        });
    displayBeaches();
    return;
}

function generateMapForFilterPage() {
    $('div[id^="beach_map_"]')
        .each(function () {
            var map = new google.maps.Map(document.getElementById($(this).attr('id')),
            {
                center: { lat: parseFloat($(this).data('lat')), lng: parseFloat($(this).data('lng')) },
                zoom: 11
            });
            var marker = new google.maps.Marker(
            {
                position: map.center,
                icon: '../StaticImages/BeachIcon.png'
            });
            marker.setMap(map);
        });
};