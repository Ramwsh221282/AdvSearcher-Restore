import { Injectable } from "@angular/core";
import { TauriApi } from "../../../../../../api/tauri-api";
import { NotificationsService } from "../../../../../../controls/notification/notifications.service";
import { listen } from "@tauri-apps/api/event";
import { IParsedAdvertisement } from "../../parsed-advertisement.interface";

@Injectable({
  providedIn: "any",
})
export class AdvertisementFileSavingService {
  public notifications: NotificationsService;

  public async getFolders(): Promise<string[]> {
    const response = await TauriApi.invokePlugin<string[]>({
      controller: "AdvertisementsFileSystemController",
      action: "GetSubfolders",
    });
    return this.createFoldersArrayFromResponse(response);
  }

  public async moveToDirectory(directory: string): Promise<string[]> {
    const response = await TauriApi.invokePlugin<string[]>({
      controller: "AdvertisementsFileSystemController",
      action: "MoveToFolder",
      data: { folder: directory },
    });
    return this.createFoldersArrayFromResponse(response);
  }

  public async moveToParent(): Promise<string[]> {
    const response = await TauriApi.invokePlugin<string[]>({
      controller: "AdvertisementsFileSystemController",
      action: "MoveToParent",
    });
    return this.createFoldersArrayFromResponse(response);
  }

  public async saveAdvertisement(
    advertisement: IParsedAdvertisement,
    folderName: string,
  ): Promise<void> {
    const listener = listen("file-system-listener", (event) => {
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(`${event.payload}`);
      this.notifications.turnOn();
    });
    const payload: SaveAdvertisementRequest = {} as SaveAdvertisementRequest;
    payload.id = advertisement.id;
    payload.folderName = folderName;
    await TauriApi.invokePlugin({
      controller: "AdvertisementsFileSystemController",
      action: "SaveAdvertisement",
      data: payload,
    });
    await listener;
  }

  public async createDirectory(directoryName: string): Promise<void> {
    const listener = listen("file-system-listener", (event) => {
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(`${event.payload}`);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "AdvertisementsFileSystemController",
      action: "CreateDirectory",
      data: { folder: directoryName },
    });
    await listener;
  }

  private createFoldersArrayFromResponse(response: string[]): string[] {
    const folders = [];
    for (let i = 0; i < response.length; i++) {
      folders.push(response[i]);
    }
    return folders;
  }
}

interface SaveAdvertisementRequest {
  id: number;
  folderName: string;
}
