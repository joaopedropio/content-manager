var uri = 'ws://' + window.location.host + '/ws';
var globalDuration = undefined;
function connect() {
    let consoleWindow = $('#consoleWindow');
    let progressBarId = '#progressBar';

    socket = new WebSocket(uri);
    socket.onopen = function (event) {
        console.log("opened connection to " + uri);
    };
    socket.onclose = function (event) {
        console.log("closed connection from " + uri);
    };
    socket.onmessage = function (event) {
        appendText(consoleWindow, event.data);
        updateProgressBar(progressBarId, event.data);
        console.log(event.data);
    };
    socket.onerror = function (event) {
        console.log("error: " + event.data);
    };
}

function appendText(consoleWindow, message) {
    consoleWindow.append(message + '\n');
    if (consoleWindow.length)
        consoleWindow.scrollTop(consoleWindow[0].scrollHeight - consoleWindow.height());
}

function updateProgressBar(progressBarId, log) {
    let duration = parseDuration(log);
    if (duration == undefined)
        return;

    let time = parseTime(log);
    if (time == undefined)
        return;

    let percentage = toPercentage(time, duration);
    setProgressBarValues(progressBarId, percentage);
}

function parseTime(log) {
    let indexOfTime = log.indexOf('time=', 0);
    let substringNotFound = indexOfTime == -1;

    if (substringNotFound)
        return;

    let startTimeIndex = indexOfTime + 5;
    let endTimeIndex = startTimeIndex + 11;

    let time = log.substring(startTimeIndex, endTimeIndex);

    let parsedTime = removeInvalidChar(time);

    return parsedTime;
}

function parseDuration(log) {

    if (globalDuration != undefined)
        return globalDuration;

    let indexOfTime = log.indexOf('Duration: ', 0);
    let substringNotFound = indexOfTime == -1;

    if (substringNotFound)
        return;

    let startDurationIndex = indexOfTime + 10;
    let endDurationIndex = startDurationIndex + 11;

    let duration = log.substring(startDurationIndex, endDurationIndex);

    let parsedDuration = removeInvalidChar(duration);

    globalDuration = parsedDuration;

    return globalDuration;
}

function removeInvalidChar(str) {
    return str.replace(':', '').replace(':', '').replace('.', '');
}

function toPercentage(time, duration) {
    return (time * 100) / duration;
}

function setProgressBarValues(progressBarId, percentageValue) {
    percentageValue = Math.round(percentageValue);
    let percentage = percentageValue + '%';
    $(progressBarId).css('width', percentage);
    $(progressBarId + ' aria-valuenow').val(percentageValue);
    $(progressBarId).text(percentage);
}

connect();