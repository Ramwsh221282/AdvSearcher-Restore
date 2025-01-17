import { Component } from "@angular/core";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import {
  IPublishingLink,
  PublishingLinksService,
} from "./publishing-links.service";
import { NotificationsService } from "../../../controls/notification/notifications.service";
import { NgForOf, NgIf } from "@angular/common";
import { NotificationComponent } from "../../../controls/notification/notification.component";
import { CreatePublishingLinkDialogComponent } from "./create-publishing-link-dialog/create-publishing-link-dialog.component";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";

@Component({
  selector: "app-publishing-page-links",
  imports: [
    PrimaryButtonComponent,
    NgForOf,
    NgIf,
    NotificationComponent,
    CreatePublishingLinkDialogComponent,
    RedButtonComponent,
  ],
  templateUrl: "./publishing-page-links.component.html",
  styleUrl: "./publishing-page-links.component.css",
  standalone: true,
  providers: [PublishingLinksService],
})
export class PublishingPageLinksComponent {
  protected links: IPublishingLink[] = [];
  protected currentService: string = "";
  protected isAddingNewLink: boolean = false;

  constructor(
    private readonly _service: PublishingLinksService,
    public readonly notifications: NotificationsService,
  ) {
    this._service.notifications = notifications;
  }

  public async getLinksByService(serviceName: string): Promise<void> {
    this.currentService = serviceName;
    this.links = await this._service.getLinks(this.currentService);
  }

  public addNewLink(): void {
    this.isAddingNewLink = true;
  }

  public async removeLink(link: string): Promise<void> {
    await this._service.removeLink(this.currentService, link);
    await this.getLinksByService(this.currentService);
  }
}
