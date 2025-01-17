import { Injectable } from "@angular/core";
import { TauriApi } from "../../api/tauri-api";
import { ISettingsSection } from "./settings-page-item/settings-item.interface";

@Injectable({
  providedIn: "any",
})
export class SettingsService {
  public async setSettings(settings: ISettingsSection): Promise<void> {
    const request: SettingsRequest = new SettingsRequest(settings);
    await TauriApi.invokePlugin({
      controller: "SettingsController",
      action: "SetSettings",
      data: request,
    });
  }

  public async readSettings(settings: ISettingsSection): Promise<void> {
    const request: SettingsRequest = new SettingsRequest(settings);
    const readSettings = await TauriApi.invokePlugin<SettingsRequest>({
      controller: "SettingsController",
      action: "ReadSettings",
      data: request,
    });
    this.mapReadSettings(readSettings, settings);
  }

  public async flushSettings(settings: ISettingsSection): Promise<void> {
    const request: SettingsRequest = new SettingsRequest(settings);
    await TauriApi.invokePlugin<SettingsRequest>({
      controller: "SettingsController",
      action: "FlushSettings",
      data: request,
    });
    this.setSettingsValuesEmpty(settings);
  }

  private setSettingsValuesEmpty(settings: ISettingsSection): void {
    for (const element of settings.items) {
      element.value = "";
    }
  }

  private mapReadSettings(
    read: SettingsRequest,
    settings: ISettingsSection,
  ): void {
    for (const element of read.settings) {
      const index = settings.items.findIndex((item) => item.key == element.key);
      settings.items[index].value = element.value;
    }
  }
}

class SettingsRequest {
  public readonly section: SettingsSection;
  public readonly settings: SettingsSectionItem[] = [];

  constructor(settings: ISettingsSection) {
    this.section = new SettingsSection(settings.sectionName, settings.fileName);
    for (const item of settings.items) {
      const setting = new SettingsSectionItem(item.key, item.value);
      this.settings.push(setting);
    }
  }
}

class SettingsSection {
  public readonly sectionName: string;
  public readonly fileName: string;

  constructor(sectionName: string, fileName: string) {
    this.sectionName = sectionName;
    this.fileName = fileName;
  }
}

class SettingsSectionItem {
  public readonly key: string;
  public readonly value: string;

  constructor(key: string, value: string) {
    this.key = key;
    this.value = value;
  }
}
