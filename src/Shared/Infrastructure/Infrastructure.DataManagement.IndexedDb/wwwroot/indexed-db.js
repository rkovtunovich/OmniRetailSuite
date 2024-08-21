
//#region DB Management

export function initializeDb(settings) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(settings.name, settings.version);

        request.onupgradeneeded = function (event) {
            const db = event.target.result;
            settings.objectStores.forEach((storeDefinition) => {
                if (!db.objectStoreNames.contains(storeDefinition.name)) {

                    // Create the object storeDefinition
                    var createdStore = db.createObjectStore(storeDefinition.name, { keyPath: storeDefinition.keyPath });

                    // Create indexes if they are defined
                    if (storeDefinition.indexes) {
                        storeDefinition.indexes.forEach((index) => {

                            if (!createdStore.indexNames.contains(index.name)) {
                                createdStore.createIndex(index.name, index.keyPath, { unique: index.unique });
                            }
                        })
                    }
                }
            });
        };

        request.onsuccess = function (event) {
            // Successfully opened the database
            event.target.result.close(); // Close the connection
            resolve(true);

            console.log(`Successfully opened database: ${settings.name}`);
        };

        request.onerror = function (event) {
            // Failed to open or upgrade the database
            reject(false);

            console.error(`Failed to open database: ${settings.name} - ${event.target.errorCode}:${event.target.error}`);
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

export function getRecordsByIndex(dbName, storeName, indexName, key) {
    return performTransaction(dbName, storeName, 'readonly', store => store.index(indexName).getAll(key));
}

export function clearStore(dbName, storeName) {
    return performTransaction(dbName, storeName, 'readwrite', store => store.clear());
}

//#endregion
