function setupVideo(url, auth, videoTag) {
    var player = dashjs.MediaPlayer().create();
    player.extend("RequestModifier", () => {
        return {
            modifyRequestHeader: xhr => {
                xhr.setRequestHeader("Authorization", auth);
                return xhr;
            }
        };
    },
        true
    );
    player.initialize(document.querySelector(videoTag), url, true);
}