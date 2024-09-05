﻿ window.blazorCulture = {
    get: () => window.localStorage['BlazorCulture'],
    set: (value) => window.localStorage['BlazorCulture'] = value
  };

document.body.style.fontSize = setFontSize();

async function checkForServiceWorkerUpdate() {
    if ('serviceWorker' in navigator) {
        try {
            const registration = await navigator.serviceWorker.ready;
            if (registration) {
                registration.update();
            }
        } catch (error) {
            console.error('Error updating service worker:', error);
        }
    } else {
        console.warn('Service workers are not supported in this browser.');
    }
}

if ('serviceWorker' in navigator) {
    navigator.serviceWorker.addEventListener('message', event => {
        if (event.data && event.type === 'NEW_VERSION_AVAILABLE') {
            window.location.reload();
        }
    });
}

// set font size based on user preference to body
function changeFontSize(size) {
    document.body.style.fontSize = size;

    // save to local storage
    localStorage.setItem('fontSize', size);
}

// set font size based on user preference from local storage
function setFontSize() {
    const size = localStorage.getItem('fontSize');
    if (size) {
        document.body.style.fontSize = size;
    }
    else {
        document.body.style.fontSize = "1em";
    }
}
