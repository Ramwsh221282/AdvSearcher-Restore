import { Injectable } from "@angular/core";
import { NotificationsService } from "../../../controls/notification/notifications.service";
import { TauriApi } from "../../../api/tauri-api";
import { listen } from "@tauri-apps/api/event";

@Injectable({
  providedIn: "any",
})
export class PublishingLinksService {
  public notifications: NotificationsService | null;

  public async getLinks(serviceName: string): Promise<IPublishingLink[]> {
    return await TauriApi.invokePlugin({
      controller: "PublishingLinksController",
      action: "GetPublishingLinksOfService",
      data: { serviceName: serviceName },
    });
  }

  public async addLink(serviceName: string, link: string): Promise<void> {
    const listener = listen("publishing-links-listener", (event) => {
      if (!this.notifications) return;
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(`${event.payload}`);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "PublishingLinksController",
      action: "CreatePublishingLink",
      data: { serviceName: serviceName, link: link },
    });
    await listener;
  }

  public async removeLink(serviceName: string, link: string): Promise<void> {
    const listener = listen("publishing-links-listener", (event) => {
      if (!this.notifications) return;
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(`${event.payload}`);
      this.notifications.turnOn();
    });
    await TauriApi.invokePlugin({
      controller: "PublishingLinksController",
      action: "RemovePublishingLink",
      data: { serviceName: serviceName, link: link },
    });
    await listener;
  }
}

export interface IPublishingLink {
  serviceName: string;
  link: string;
}
