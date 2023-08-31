class BarcodeReader {
    constructor(dotNetObjRef, videoElementId, stopOnFirstScan) {
        this.detections = 0;
        this.dotNetObjRef = dotNetObjRef;
        this.videoElementId = videoElementId;
        this.stopOnFirstScan = stopOnFirstScan;
        if (this.hasBarcodeSupport()) {
            this.barcodeDetector = new BarcodeDetector();
            this.init();
        }
        else {
            this.dotNetObjRef.invokeMethodAsync('SetError', 'Barcode Detection API is not supported.');
        }
    }
    async hasBarcodeSupport() {
        if (!('BarcodeDetector' in window))
            return false;
        return await BarcodeDetector.getSupportedFormats().length > 0;
    }
    async init() {
        const stream = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: {
                    ideal: "environment"
                }
            },
            audio: false
        });
        const video = document.querySelector(`#${this.videoElementId}`);
        video.srcObject = stream;
        await video.play();
        this.dotNetObjRef.invokeMethodAsync('SetScanning', true);
        this.detect(video);
    }
    async detect(video) {
        await this.barcodeDetector.detect(video)
            .then(detectedBarcodes => {
            if (detectedBarcodes.length > 0) {
                this.detections++;
                this.dotNetObjRef.invokeMethodAsync('SetResult', detectedBarcodes[0].rawValue);
            }
        })
            .catch(error => this.dotNetObjRef.invokeMethodAsync('SetError', error));
        if (this.stopOnFirstScan && this.detections > 0) {
            this.dotNetObjRef.invokeMethodAsync('SetScanning', false);
            if (video.srcObject instanceof MediaStream) {
                video.srcObject.getTracks().forEach(track => {
                    if (track.readyState === 'live')
                        track.stop();
                });
            }
            return;
        }
        await new Promise(resolve => setTimeout(resolve, 250));
        this.detect(video);
    }
}
export function GetInstace(dotNetObjRef, videoElementId, stopOnFirstScan) {
    return new BarcodeReader(dotNetObjRef, videoElementId, stopOnFirstScan);
}
//# sourceMappingURL=BarcodeReader.razor.js.map