import { Component, OnInit } from "@angular/core";
import { NgForOf, NgIf } from "@angular/common";
import { SettingsPageItemComponent } from "../settings-page/settings-page-item/settings-page-item.component";
import { IParsingSection } from "./parsing-section.interface";
import { ParsingPageSectionComponent } from "./parsing-page-section/parsing-page-section.component";
import { NotificationComponent } from "../../controls/notification/notification.component";

@Component({
  selector: "app-parsing-page",
  imports: [
    NgForOf,
    SettingsPageItemComponent,
    ParsingPageSectionComponent,
    NotificationComponent,
    NgIf,
  ],
  templateUrl: "./parsing-page.component.html",
  standalone: true,
  styleUrl: "./parsing-page.component.css",
})
export class ParsingPageComponent implements OnInit {
  protected readonly parsingSections: IParsingSection[] = [];

  public ngOnInit() {
    this.appendParsingSection("ВКонтакте", "VK", "VkParser");
    this.appendParsingSection("Одноклассники", "OK", "OkParser");
    this.appendParsingSection("Циан", "CIAN", "CianParser");
    this.appendParsingSection("Авито", "AVITO", "AvitoFastParser");
    this.appendParsingSection("Домклик", "DOMCLICK", "DomclickParser");
  }

  private appendParsingSection(
    displayName: string,
    name: string,
    pluginName: string,
  ): void {
    const section = {} as IParsingSection;
    section.displayName = displayName;
    section.name = name;
    section.pluginName = pluginName;
    this.parsingSections.push(section);
  }
}
