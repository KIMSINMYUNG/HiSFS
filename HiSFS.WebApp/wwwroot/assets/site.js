window.downloadFromUrl = (url, filename) => {
    "use strict"; // Start of use strict

    var anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = filename;
    anchorElement.click();
    anchorElement.remove();
};