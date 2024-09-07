 window.blazorCulture = {
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

    // save to local storage
    localStorage.setItem('fontSize', size);

    setFontSize();
}

// set font size based on user preference from local storage
function setFontSize() {
    const size = localStorage.getItem('fontSize');
    if (size) {
        document.body.style.fontSize = size;
        document.documentElement.style.setProperty("--mud-typography-body1-size", size);
        document.documentElement.style.setProperty("--mud-typography-input-size", size);
        document.documentElement.style.setProperty("--mud-typography-button-size", size);
    }
    else {
        document.body.style.fontSize = "1em";
    }
}

function clearCacheAndReload() {
    if ('caches' in window) {
        // Clear all caches
        caches.keys().then(function (names) {
            for (let name of names) {
                caches.delete(name);
            }
        });
    }
    // Force a full page reload
    window.location.reload(true); // Reload with cache bypass
}
