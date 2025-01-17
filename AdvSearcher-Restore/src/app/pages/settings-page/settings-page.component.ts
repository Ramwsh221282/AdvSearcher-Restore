import { Component, OnInit } from "@angular/core";
import { SettingsPageItemComponent } from "./settings-page-item/settings-page-item.component";
import {
  ISettingsItem,
  ISettingsSection,
} from "./settings-page-item/settings-item.interface";
import { NgForOf } from "@angular/common";

@Component({
  selector: "app-settings-page",
  imports: [SettingsPageItemComponent, NgForOf],
  templateUrl: "./settings-page.component.html",
  styleUrl: "./settings-page.component.css",
  standalone: true,
  providers: [SettingsPageItemComponent],
})
export class SettingsPageComponent implements OnInit {
  protected readonly settingsSections: ISettingsSection[] = [];

  public ngOnInit() {
    const vkSection = this.createSection(
      "Настройки сервиса ВКонтакте",
      "VK_Settings.txt",
    );
    this.addSectionEmptyItem(vkSection, "Сервисный ключ приложения ВКонтакте");
    this.addSectionEmptyItem(vkSection, "OAuth ключ приложения ВКонтакте");

    const okSection = this.createSection(
      "Настройки сервиса Одноклассники",
      "OK_Settings.txt",
    );
    this.addSectionEmptyItem(okSection, "Публичный токен");
    this.addSectionEmptyItem(okSection, "Вечный токен");

    const whatsAppSection = this.createSection(
      "Настройки сервиса Whats App",
      "WA_Settings.txt",
    );
    this.addSectionEmptyItem(whatsAppSection, "ID инстанса");
    this.addSectionEmptyItem(whatsAppSection, "Токен инстанса");

    const mailRuSection = this.createSection(
      "Настройки сервиса MAIL.RU",
      "MAILRU_Settings.txt",
    );
    this.addSectionEmptyItem(mailRuSection, "SMPT ключ");
    this.addSectionEmptyItem(mailRuSection, "Почта отправителя");

    const webDriverSection = this.createSection(
      "Настройки браузера",
      "WebDriver_Settings.txt",
    );
    this.addSectionEmptyItem(webDriverSection, "Путь к браузеру Google Chrome");

    this.settingsSections.push(vkSection);
    this.settingsSections.push(okSection);
    this.settingsSections.push(whatsAppSection);
    this.settingsSections.push(mailRuSection);
    this.settingsSections.push(webDriverSection);
  }

  private createSection(name: string, fileName: string): ISettingsSection {
    const section: ISettingsSection = {} as ISettingsSection;
    section.fileName = fileName;
    section.sectionName = name;
    section.items = [];
    return section;
  }

  private addSectionEmptyItem(section: ISettingsSection, key: string): void {
    const item: ISettingsItem = {} as ISettingsItem;
    item.key = key;
    item.value = "";
    section.items.push(item);
  }
}
