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
