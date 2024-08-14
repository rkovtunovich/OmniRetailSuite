
//#region DB Management

export function initializeDb(settings) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(settings.name, settings.version);

        request.onupgradeneeded = function (event) {
            const db = event.target.result;
            settings.objectStores.forEach((store) => {
                if (!db.objectStoreNames.contains(store.name)) {
                    db.createObjectStore(store.name, { keyPath: store.keyPath });
                }
            });
        };

        request.onsuccess = function (event) {
            // Successfully opened the database
            event.target.result.close(); // Close the connection
            resolve(true);

            console.log(`Successfully opened database: ${settings.name}`);
        };

        request.onerror = function () {
            // Failed to open or upgrade the database
            reject(false);

            console.error(`Failed to open database: ${settings.name}`);
        };
    });
}

function openDb(name, version, upgradeCallback) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(name, version);

        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);
        request.onupgradeneeded = (event) => upgradeCallback(event.target.result);
    });
}

function deleteDb(dbName) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.deleteDatabase(dbName);

        request.onsuccess = () => resolve(true);
        request.onerror = () => resolve(false);
    });
}

//#endregion

//#region Data Management

 function performTransaction(dbName, storeName, transactionMode, operation) {
    return openDb(dbName).then(db => {
        return new Promise((resolve, reject) => {
            const transaction = db.transaction(storeName, transactionMode);
            const store = transaction.objectStore(storeName);
            const request = operation(store);
            request.onsuccess = () => {
                resolve(request.result);
            };
            request.onerror = (event) => {
                reject(event.target.error);
            };
        });
    });
}

export function getRecord(dbName, storeName, key) {
    return performTransaction(dbName, storeName, 'readonly', store => store.get(key));
}

export function addRecord(dbName, storeName, record) {
    return performTransaction(dbName, storeName, 'readwrite', store => store.add(record));
}

export function updateRecord(dbName, storeName, record) {
    return performTransaction(dbName, storeName, 'readwrite', store => store.put(record));
}

export function deleteRecord(dbName, storeName, key) {
    return performTransaction(dbName, storeName, 'readwrite', store => store.delete(key));
}

export function getRecords(dbName, storeName) {
    return performTransaction(dbName, storeName, 'readonly', store => store.getAll());
}

export function clearStore(dbName, storeName) {
    return performTransaction(dbName, storeName, 'readwrite', store => store.clear());
}

//#endregion
