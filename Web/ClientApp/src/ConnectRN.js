import useStore from "./stores"

export const isRN = window.ReactNativeWebView

export const requestPermission = () => {
    if (!isRN) return

    window.ReactNativeWebView.postMessage(
        JSON.stringify({ type: "REQ_CAMERA_PERMISSION" })
    );
};

export const reqCameraActive = (active) => {
    if (!isRN) return

    window.ReactNativeWebView.postMessage(
        JSON.stringify({ key: "CAMERA_ACTIVE", value: active })
    );
};

export const RNListener = () => {
    const { _androidStore } = useStore();

    if (!isRN) return
    const listener = event => {
        const { key, value } = JSON.parse(event.data);
        switch (key) {
            case "CAMERA_SNAP_URI":
                _androidStore.setTestMessage(value);
                break;
        }
    };

    document.addEventListener("message", listener); //android
    window.addEventListener("message", listener);   //ios
};

