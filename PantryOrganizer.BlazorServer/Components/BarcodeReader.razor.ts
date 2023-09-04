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
    video: HTMLVideoElement = null;
    stopOnFirstScan: boolean = false;
    barcodeDetector: BarcodeDetector = null;
    detections: number = 0;

    constructor(
        dotNetObjRef: DotNetObjectReference,
        element: HTMLVideoElement,
        stopOnFirstScan: boolean)
    {
        this.dotNetObjRef = dotNetObjRef;
        this.stopOnFirstScan = stopOnFirstScan;
        this.video = element;

        if (this.hasBarcodeSupport())
            this.barcodeDetector = new BarcodeDetector();
    }

    async hasBarcodeSupport(): Promise<boolean>
    {
        if (!('BarcodeDetector' in window))
            return false;
        const formats = await (BarcodeDetector as BarcodeDetector).getSupportedFormats();
        return formats.length > 0;
    }

    dispose()
    {
        this.dotNetObjRef.invokeMethodAsync('SetScanning', false);

        if (this.video.srcObject instanceof MediaStream)
        {
            this.video.srcObject.getTracks().forEach(track =>
            {
                if (track.readyState === 'live')
                    track.stop();
            });
        }

        this.video.srcObject = null;
    }

    async start()
    {
        if (this.barcodeDetector === null)
        {
            this.dotNetObjRef.invokeMethodAsync(
                'SetError',
                'Barcode Detection API is not supported.');
            return;
        }

        const stream = await navigator.mediaDevices.getUserMedia({
            video: {
                facingMode: {
                    ideal: "environment"
                }
            },
            audio: false
        });

        this.video.srcObject = stream;
        await this.video.play();

        this.dotNetObjRef.invokeMethodAsync('SetScanning', true);
        this.detect();
    }

    async detect()
    {
        await this.barcodeDetector.detect(this.video)
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

            if (this.video.srcObject instanceof MediaStream)
            {
                this.video.srcObject.getTracks().forEach(track =>
                {
                    if (track.readyState === 'live')
                        track.stop();
                });
            }
            return;
        }

        await new Promise(resolve => setTimeout(resolve, 250));
        this.detect();
    }
}

export function GetInstace(
    dotNetObjRef: DotNetObjectReference,
    element: HTMLVideoElement,
    stopOnFirstScan: boolean)
{
    return new BarcodeReader(dotNetObjRef, element, stopOnFirstScan);
}
