
export function initializeDb(settings) {
    return openDb(settings.name, settings.version, (db) => {
        settings.objectStores.forEach((objectStore) => {
            createObjectStore(db, objectStore.name, objectStore.keyPath);
        });
    });
}

export function isDbCreated(dbName) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);

        request.onsuccess = () => resolve(true);
        request.onerror = () => resolve(false);
    });
}

function createObjectStore(db, name, keyPath) {
    return db.createObjectStore(name, { keyPath });
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

function deleteObjectStore(dbName, version, objectStoreName) {
    return new Promise((resolve, reject) => {
        openDb(dbName, version, (db) => {
            db.deleteObjectStore(objectStoreName);
        }).then(() => resolve(true));
    });
}

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
