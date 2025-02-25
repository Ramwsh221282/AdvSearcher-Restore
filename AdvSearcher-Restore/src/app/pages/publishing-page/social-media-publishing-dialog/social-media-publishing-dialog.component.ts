import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { SocialMediaPublishingService } from "./socia-media-publishing.service";
import { PublishingDataSelectedService } from "../publishing-data-selected.service";
import { InputControlComponent } from "../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";
import { NgxLoadingBar } from "@ngx-loading-bar/core";
import { NotificationsService } from "../../../controls/notification/notifications.service";

@Component({
  selector: "app-social-media-publishing-dialog",
  imports: [
    InputControlComponent,
    PrimaryButtonComponent,
    RedButtonComponent,
    NgxLoadingBar,
  ],
  templateUrl: "./social-media-publishing-dialog.component.html",
  styleUrl: "./social-media-publishing-dialog.component.css",
  standalone: true,
})
export class SocialMediaPublishingDialogComponent implements OnInit {
  public serviceDisplayName: string = "";
  @Input({ required: true }) selectedService: string = "";
  @Output() visibilityChange: EventEmitter<void> = new EventEmitter();

  constructor(
    private readonly _selectedData: PublishingDataSelectedService,
    protected readonly _publishing: SocialMediaPublishingService,
    protected readonly notifications: NotificationsService,
  ) {
    this._publishing.notifications = this.notifications;
  }

  public ngOnInit() {
    this.serviceDisplayName = this.getDisplayName(this.selectedService);
  }

  public async startPublishing(): Promise<void> {
    await this._publishing.publish(
      this.selectedService,
      this._selectedData.selectedAdvertisements,
    );
  }

  public closeWindow(): void {
    this.visibilityChange.emit();
  }

  private getDisplayName(serviceName: string): string {
    if (serviceName.includes("Vk")) return "Вконтакте";
    if (serviceName.includes("Ok")) return "Одноклассники";
    return "Undefined";
  }
}
