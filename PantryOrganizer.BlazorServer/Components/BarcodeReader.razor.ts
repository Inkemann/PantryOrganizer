declare var BarcodeDetector: any | undefined;

interface DotNetObjectReference
{
    invokeMethodAsync(methodName: string, ...args: any[]);
}

interface DetectedBarcode
{
    boundingBox: DOMRectReadOnly;
    cornerPoints: any;
    format: string;
    rawValue: string;
}

interface BarcodeDetector
{
    detect(video: HTMLElement | Blob | ImageData): Promise<DetectedBarcode[]>;

    getSupportedFormats(): Promise<string[]>;
}

class BarcodeReader
{
    dotNetObjRef: DotNetObjectReference;
    videoElementId: string;
    stopOnFirstScan: boolean;
    barcodeDetector: BarcodeDetector;
    detections: number = 0;

    constructor(
        dotNetObjRef: DotNetObjectReference,
        videoElementId: string,
        stopOnFirstScan: boolean)
    {
        this.dotNetObjRef = dotNetObjRef;
        this.videoElementId = videoElementId;
        this.stopOnFirstScan = stopOnFirstScan;

        if (this.hasBarcodeSupport())
        {
            this.barcodeDetector = new BarcodeDetector();
            this.init();
        }
        else
        {
            this.dotNetObjRef.invokeMethodAsync(
                'SetError',
                'Barcode Detection API is not supported.');
        }
    }

    async hasBarcodeSupport(): Promise<boolean>
    {
        if (!('BarcodeDetector' in window))
            return false;
        return await BarcodeDetector.getSupportedFormats().length > 0;
    }

    async init()
    {
        const stream = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: {
                    ideal: "environment"
                }
            },
            audio: false
        });

        const video = document.querySelector(`#${this.videoElementId}`) as HTMLVideoElement;
        video.srcObject = stream;
        await video.play();

        this.dotNetObjRef.invokeMethodAsync('SetScanning', true);
        this.detect(video);
    }

    async detect(video: HTMLVideoElement)
    {
        await this.barcodeDetector.detect(video)
            .then(detectedBarcodes =>
            {
                if (detectedBarcodes.length > 0)
                {
                    this.detections++;
                    this.dotNetObjRef.invokeMethodAsync('SetResult', detectedBarcodes[0].rawValue);
                }
            })
            .catch(error => this.dotNetObjRef.invokeMethodAsync('SetError', error));

        if (this.stopOnFirstScan && this.detections > 0)
        {
            this.dotNetObjRef.invokeMethodAsync('SetScanning', false);

            if (video.srcObject instanceof MediaStream)
            {
                video.srcObject.getTracks().forEach(track =>
                {
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

export function GetInstace(
    dotNetObjRef: DotNetObjectReference,
    videoElementId: string,
    stopOnFirstScan: boolean)
{
    return new BarcodeReader(dotNetObjRef, videoElementId, stopOnFirstScan);
}
