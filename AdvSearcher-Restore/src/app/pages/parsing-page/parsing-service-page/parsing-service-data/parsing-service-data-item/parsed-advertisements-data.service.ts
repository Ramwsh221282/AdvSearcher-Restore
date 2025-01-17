import { Injectable } from "@angular/core";
import { IParsedAdvertisement } from "../parsed-advertisement.interface";
import { IParsingSection } from "../../../parsing-section.interface";
import { TauriApi } from "../../../../../api/tauri-api";
import { listen } from "@tauri-apps/api/event";
import { NotificationsService } from "../../../../../controls/notification/notifications.service";

@Injectable({
  providedIn: "any",
})
export class ParsedAdvertisementsDataService {
  public notifications: NotificationsService;

  public async getData(
    parser: IParsingSection,
    page: number,
    pageSize: number,
  ): Promise<IParsedAdvertisement[]> {
    const payload: GetDataPayload = {} as GetDataPayload;
    payload.serviceName = parser.name;
    payload.page = page;
    payload.pageSize = pageSize;
    return await TauriApi.invokePlugin<IParsedAdvertisement[]>({
      controller: "ParsedDataController",
      action: "GetParsedData",
      data: payload,
    });
  }

  public async cacheData(advertisement: IParsedAdvertisement): Promise<void> {
    const listener = listen("data-listener", (event) => {
      const message = event.payload.toString();
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(message);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "ParsedDataController",
      action: "RemoveAndCacheAdvertisement",
      data: { advertisementId: advertisement.id },
    });
    await listener;
  }

  public async removeData(advertisement: IParsedAdvertisement): Promise<void> {
    const listener = listen("data-listener", (event) => {
      const message = event.payload.toString();
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(message);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "ParsedDataController",
      action: "RemoveAdvertisement",
      data: { advertisementId: advertisement.id },
    });
    await listener;
  }

  public async removeAllData(parser: IParsingSection) {
    const listener = listen("data-listener", (event) => {
      this.notifications.setTitle("Уведомление");
      const message = event.payload.toString();
      this.notifications.setMessage(message);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "ParsedDataController",
      action: "RemoveAllAdvertisements",
      data: { serviceName: parser.name },
    });
    await listener;
  }

  public async cacheAllData(parser: IParsingSection) {
    const listener = listen("data-listener", (event) => {
      this.notifications.setTitle("Уведомление");
      const message = event.payload.toString();
      this.notifications.setMessage(message);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "ParsedDataController",
      action: "RemoveAndCacheAllAdvertisements",
      data: { serviceName: parser.name },
    });
    await listener;
  }

  public async openInBrowser(
    advertisement: IParsedAdvertisement,
  ): Promise<void> {
    await TauriApi.invokePlugin({
      controller: "ParsedDataController",
      action: "OpenAdvertisementInBrowser",
      data: { advertisementId: advertisement.id },
    });
  }
}

interface GetDataPayload {
  serviceName: string;
  page: number;
  pageSize: number;
}
