import { Component, signal } from "@angular/core";
import { NgForOf, NgIf } from "@angular/common";
import { PrimaryButtonComponent } from "../../controls/primary-button/primary-button.component";
import { TauriApi } from "../../api/tauri-api";
import { RedButtonComponent } from "../../controls/red-button/red-button.component";
import { NgxLoadingBar } from "@ngx-loading-bar/core";

@Component({
  selector: "app-inpainting-page",
  imports: [
    NgIf,
    PrimaryButtonComponent,
    NgForOf,
    RedButtonComponent,
    NgxLoadingBar,
  ],
  templateUrl: "./inpainting-page.component.html",
  styleUrl: "./inpainting-page.component.css",
  standalone: true,
})
export class InpaintingPageComponent {
  public images: string[] = [];
  public files: File[] = [];
  public imagesDefault: string[] = [];
  public filesDefault: File[] = [];
  public isDrawing: boolean = false;
  public canvasWidth: number = 512;
  public canvasHeight: number = 512;
  public radius: number = 10;
  public maxProgress = signal(0);
  private ctx: CanvasRenderingContext2D | null = null;
  private lastX: number = 0;
  private lastY: number = 0;

  public async uploadImage(event: any): Promise<void> {
    for (let i = 0; i < event.target.files.length; i++) {
      const file = event.target.files[i];
      if (!file.name.endsWith(".png") && !file.name.endsWith(".jpg")) {
        continue;
      }
      this.filesDefault.push(file);
      const reader = new FileReader();
      reader.onload = () => {
        this.images.push(reader.result.toString());
        this.imagesDefault.push(reader.result.toString());
        const img = new Image();
        img.src = reader.result.toString();
        img.onload = () => {
          const canvas = document.getElementById(
            `canvas-${img.src}`,
          ) as HTMLCanvasElement;
          if (canvas) {
            this.ctx = canvas.getContext("2d");
            this.ctx?.drawImage(img, 0, 0, this.canvasWidth, this.canvasHeight);
          }
        };
      };
      reader.readAsDataURL(file);
    }
  }

  public resetAll(): void {
    this.files = [];
    this.images = [];
    this.imagesDefault = [];
    this.filesDefault = [];
    this.ctx = null;
    this.lastX = 0;
    this.lastY = 0;
  }

  public radiusChange($event: any): void {
    this.radius = $event.target.value;
  }

  public startDrawing(event: MouseEvent, canvasId: string): void {
    this.isDrawing = true;
    this.lastX = event.offsetX;
    this.lastY = event.offsetY;
    this.ctx = (
      document.getElementById(canvasId) as HTMLCanvasElement
    ).getContext("2d");
  }

  public draw(event: MouseEvent): void {
    if (!this.isDrawing || !this.ctx) return;
    this.ctx.strokeStyle = "#FFF";
    this.ctx.fillStyle = "#FFF";
    this.ctx.beginPath();
    this.ctx.arc(event.offsetX, event.offsetY, this.radius, 0, Math.PI * 2);
    this.ctx.fillStyle = this.ctx.strokeStyle;
    this.ctx.fill();
    this.ctx.stroke();
    this.lastX = event.offsetX;
    this.lastY = event.offsetY;
  }

  public stopDrawing(): void {
    this.isDrawing = false;
  }

  public async saveImage(): Promise<void> {
    this.maxProgress.set(1);
    const canvasElements = document.querySelectorAll("canvas");
    const requestData: InpaintingImageRequest[] = [];
    this.maxProgress.set(canvasElements.length);
    for (let i = 0; i < canvasElements.length; i++) {
      const canvas = canvasElements[i];
      const maskDataUrl = (canvas as HTMLCanvasElement).toDataURL("image/png");
      const maskByteArray = this.dataURLToByteArray(maskDataUrl);
      const maskImageBytes = Array.from(maskByteArray);
      const originalFile = this.filesDefault[i];
      const originalFileDataUrl = await new Promise<string>((resolve) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result as string);
        reader.readAsDataURL(originalFile);
      });
      const stockByteArray = this.dataURLToByteArray(originalFileDataUrl);
      const stockImageBytes = Array.from(stockByteArray);
      const request: InpaintingImageRequest = {
        maskImageBytes: maskImageBytes,
        stockImageBytes: stockImageBytes,
      };
      requestData.push(request);
    }
    await TauriApi.invokePlugin({
      controller: "ImageInpaintingController",
      action: "InpaintImages",
      data: { data: requestData },
    });
    this.maxProgress.set(0);
    this.resetAll();
  }

  private dataURLToByteArray(dataURL: string): Uint8Array {
    const base64 = dataURL.split(",")[1];
    const binaryString = atob(base64);
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
      bytes[i] = binaryString.charCodeAt(i);
    }
    return bytes;
  }
}

export interface InpaintingImageRequest {
  maskImageBytes: number[];
  stockImageBytes: number[];
}
