import { NotificationsService } from "../../../../controls/notification/notifications.service";
import { IParsingSection } from "../../parsing-section.interface";
import { Injectable, signal } from "@angular/core";
import { listen, UnlistenFn } from "@tauri-apps/api/event";
import { TauriApi } from "../../../../api/tauri-api";
import { IParsingOptions } from "./parsing-process-filter-dialog/parsing-options.interface";

@Injectable({
  providedIn: "root",
})
export class ParsingProcessService {
  public notifications: NotificationsService;
  public currentProgress = signal(0);
  public totalProgress = signal(0);

  public async invokeParsing(
    section: IParsingSection,
    options: IParsingOptions,
  ): Promise<void> {
    const maxProgressListen = this.createMaxProgressListener();
    const currentProgressListen = this.createCurrentProgressListener();
    const payload = { serviceName: section.pluginName, options: options };
    await TauriApi.invokePlugin({
      controller: "ParsingController",
      action: "ProcessParsing",
      data: payload,
    });
    await currentProgressListen;
    await maxProgressListen;
    this.publishNotifications();
    this.refreshProgress();
  }

  private createCurrentProgressListener(): Promise<UnlistenFn> {
    return listen<string>("parser-process-progress", (event) => {
      this.currentProgress.set(Number(event.payload.toString()));
    });
  }

  private createMaxProgressListener(): Promise<UnlistenFn> {
    return listen<string>("parser-max-progress", (event) => {
      const receivedNumber = Number(event.payload.toString());
      const calculatedNumber = (receivedNumber / this.totalProgress()) * 100;
      this.totalProgress.set(calculatedNumber);
    });
  }

  private publishNotifications(): void {
    this.notifications.setTitle("Уведомление");
    this.notifications.setMessage("Парсинг завершён");
    this.notifications.turnOn();
  }

  private refreshProgress(): void {
    this.currentProgress.set(0);
    this.totalProgress.set(0);
  }
}
