function setupManifestUrl() {
    var url = $('#manifestUrl').val();
    var auth = $('#authorization').val();

    var video = document.getElementById('video');
    var source = document.createElement('source');

    source.setAttribute('src', url);
    source.setAttribute('type', 'application/dash+xml');

    video.appendChild(source);
    setupVideo(url, auth, '#video');
}