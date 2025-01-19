import { Injectable, signal } from "@angular/core";
import { IPublishingAdvertisement } from "../publishing-advertisement-directory.interface";
import { TauriApi } from "../../../api/tauri-api";
import { listen, UnlistenFn } from "@tauri-apps/api/event";
import { NotificationsService } from "../../../controls/notification/notifications.service";

@Injectable({
  providedIn: "any",
})
export class MailPublishingService {
  public maxProgress = signal(0);
  public currentProgress = signal(0);
  public notifications: NotificationsService;

  public async publish(
    serviceName: string,
    files: IPublishingAdvertisement[],
    address: MailAddress,
  ): Promise<void> {
    this.maxProgress.set(files.length);
    const maxProgressListener = this.createMaxProgressListener();
    const currentProgressListener = this.createCurrentProgressListener();
    await TauriApi.invokePlugin({
      controller: "PublishDataController",
      action: "PublishToEmail",
      data: {
        files: files,
        serviceName: serviceName,
        address: {
          email: address.email,
          subject: address.subject,
        },
      },
    });
    await maxProgressListener;
    await currentProgressListener;
  }

  private createMaxProgressListener(): Promise<UnlistenFn> {
    return listen("publishing-max-progress", (event) => {
      const value = Number(event.payload.toString());
      this.maxProgress.set(Number(value));
    });
  }

  private createCurrentProgressListener(): Promise<UnlistenFn> {
    return listen("publishing-current-progress", (event) => {
      const value = Number(event.payload.toString());
      const calculatedValue = (value / this.maxProgress()) * 100;
      if (value == this.maxProgress()) {
        this.notifications.setTitle("Прогресс");
        this.notifications.setMessage("Завершено");
        this.notifications.turnOn();
        this.resetProgress();
      } else {
        this.notifications.setTitle("Прогресс");
        this.notifications.setMessage(`${value} из ${this.maxProgress()}`);
        this.notifications.turnOn();
      }
      this.currentProgress.set(calculatedValue);
    });
  }

  private resetProgress(): void {
    this.maxProgress.set(0);
    this.currentProgress.set(0);
  }
}

export interface MailAddress {
  email: string;
  subject: string;
}
