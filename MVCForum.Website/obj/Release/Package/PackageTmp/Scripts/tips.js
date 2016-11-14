$(document).ready(function () {
    $("#rainfallInfo").hide();
    $("#sharkAttack").hide();
    $("#stormWater").hide();
});

function showCol1() {
    $("#generalTipsBtn").hide();
    $("#rainfallInfo").show();
}

function showCol2() {
    $("#safetyBtn").hide();
    $("#sharkAttack").show();
}

function showCol3() {
    $("#otherIssueBtn").hide();
    $("#stormWater").show();
}