import { Component, OnInit } from "@angular/core";
import { NgForOf, NgIf } from "@angular/common";
import { PrimaryButtonComponent } from "../../controls/primary-button/primary-button.component";
import { PublishingDataService } from "./publishing-data.service";
import { IPublishingDirectory } from "./publishing-advertisement-directory.interface";
import { PublishingDirectoryComponent } from "./publishing-directory/publishing-directory.component";
import { Router } from "@angular/router";
import { DropdownListComponent } from "../../controls/dropdown-list/dropdown-list.component";
import { PublishingDataSelectedService } from "./publishing-data-selected.service";
import { SocialMediaPublishingDialogComponent } from "./social-media-publishing-dialog/social-media-publishing-dialog.component";
import { NotificationsService } from "../../controls/notification/notifications.service";
import { NotificationComponent } from "../../controls/notification/notification.component";
import { MailPublishingComponent } from "./mail-publishing/mail-publishing.component";
import { WhatsappPublishingComponent } from "./whatsapp-publishing/whatsapp-publishing.component";

@Component({
  selector: "app-publishing-page",
  imports: [
    NgForOf,
    PrimaryButtonComponent,
    NgIf,
    PublishingDirectoryComponent,
    DropdownListComponent,
    SocialMediaPublishingDialogComponent,
    NotificationComponent,
    MailPublishingComponent,
    WhatsappPublishingComponent,
  ],
  templateUrl: "./publishing-page.component.html",
  styleUrl: "./publishing-page.component.css",
  standalone: true,
  providers: [
    PublishingDataService,
    PublishingDataSelectedService,
    NotificationsService,
  ],
})
export class PublishingPageComponent implements OnInit {
  public directories: IPublishingDirectory[] = [];
  public selectedServiceOption: string = "";
  public constructor(
    private readonly _service: PublishingDataService,
    private readonly _router: Router,
    protected readonly _selectedData: PublishingDataSelectedService,
    protected readonly notifications: NotificationsService,
  ) {}

  public async ngOnInit() {
    this.directories = await this._service.getDirectories();
  }

  public navigateToLinks(): void {
    this._router.navigate(["posting-links"]);
  }

  public optionSelectedHandler(option: string): void {
    this.selectedServiceOption = option;
  }
}
