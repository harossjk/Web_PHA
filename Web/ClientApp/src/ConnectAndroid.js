import useStore from "./stores"

export const isAndroid = window.Android !== undefined

export const test = () => {
    if (!isAndroid) return
    window.Android.sendMessage()
};

export const reqCameraActive = () => {
    if (!isAndroid) return
    console.log("request camera")

    window.Android.setCameraActive(true)
};

export const AndroidListener = () => {
    const { _androidStore } = useStore();

    const listener = event => {
        const params = JSON.parse(event.data);

        Object.keys(params).forEach(key => {
            const value = params[key]
            switch (key) {
                case "CAMERA_SNAP_URI":
                    _androidStore.setTestMessage(value);
                    break;
                case "PAGE":
                    _androidStore.movePage(value)
                    break;
                case "PICTURE":
                    _androidStore.setPicture({
                        fileBinary: value["FILE_BINARY"], fileExtension: value["FILE_EXTENSION"],
                        fileName: value["FILE_NAME"], fileSize: value["FILE_SIZE"],
                    })
                    break;
            }
        })
    };

    window.addEventListener("message", listener);
};

