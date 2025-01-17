import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { IParsingSection } from "../parsing-section.interface";
import { NgIf } from "@angular/common";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { ParsingServiceLinksComponent } from "./parsing-service-links/parsing-service-links.component";
import { ParsingServiceDataComponent } from "./parsing-service-data/parsing-service-data.component";
import { ParsingServiceProcessComponent } from "./parsing-service-process/parsing-service-process.component";
import { CreateLinkDialogComponent } from "./parsing-service-links/create-link-dialog/create-link-dialog.component";
import { ParsingLinksService } from "./parsing-service-links/create-link-dialog/parsing-links.service";
import { NotificationsService } from "../../../controls/notification/notifications.service";
import { NotificationComponent } from "../../../controls/notification/notification.component";

@Component({
  selector: "app-parsing-service-page",
  imports: [
    NgIf,
    PrimaryButtonComponent,
    ParsingServiceLinksComponent,
    ParsingServiceDataComponent,
    ParsingServiceProcessComponent,
    CreateLinkDialogComponent,
    NotificationComponent,
  ],
  templateUrl: "./parsing-service-page.component.html",
  styleUrl: "./parsing-service-page.component.css",
  standalone: true,
  providers: [ParsingLinksService, NotificationsService],
})
export class ParsingServicePageComponent implements OnInit {
  protected currentParserPlugin: IParsingSection = null;
  constructor(
    private readonly _activatedRouter: ActivatedRoute,
    protected readonly notifications: NotificationsService,
  ) {}
  protected selectedMenu: string = "";

  public ngOnInit() {
    this._activatedRouter.params.subscribe((params) => {
      this.currentParserPlugin = {} as IParsingSection;
      this.currentParserPlugin.name = params["name"];
      this.currentParserPlugin.displayName = params["displayName"];
      this.currentParserPlugin.pluginName = params["pluginName"];
    });
  }

  protected selectMenu(menuName: string): void {
    this.selectedMenu = menuName;
  }
}
