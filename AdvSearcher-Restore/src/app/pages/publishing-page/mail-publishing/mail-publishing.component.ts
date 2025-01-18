import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { NotificationsService } from "../../../controls/notification/notifications.service";
import { MailAddress, MailPublishingService } from "./mail-publishing.service";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";
import { InputControlComponent } from "../../../controls/input-control/input-control.component";
import { PublishingDataSelectedService } from "../publishing-data-selected.service";
import { NgxLoadingBar } from "@ngx-loading-bar/core";

@Component({
  selector: "app-mail-publishing",
  imports: [
    PrimaryButtonComponent,
    RedButtonComponent,
    InputControlComponent,
    NgxLoadingBar,
  ],
  templateUrl: "./mail-publishing.component.html",
  styleUrl: "./mail-publishing.component.css",
  standalone: true,
})
export class MailPublishingComponent implements OnInit {
  public serviceDisplayName: string = "";
  public address: MailAddress;
  @Input({ required: true }) selectedServiceOption: string = "";
  @Output() visibilityChange: EventEmitter<void> = new EventEmitter();

  constructor(
    protected readonly _service: MailPublishingService,
    private readonly _selectedData: PublishingDataSelectedService,
    protected readonly notifications: NotificationsService,
  ) {
    this.address = {} as MailAddress;
    this.address.email = "";
    this.address.subject = "";
    this._service.notifications = this.notifications;
  }

  public ngOnInit() {
    this.serviceDisplayName = this.getDisplayName(this.selectedServiceOption);
  }

  public closeWindow(): void {
    this.visibilityChange.emit();
  }

  public async publish(): Promise<void> {
    if (
      this.address.email.trim().length == 0 ||
      this.address.subject.trim().length == 0
    )
      return;
    await this._service.publish(
      this.selectedServiceOption,
      this._selectedData.selectedAdvertisements,
      this.address,
    );
    this.notifications.setTitle("Уведомление");
    this.notifications.setMessage("Завершено");
    this.notifications.turnOn();
    this.closeWindow();
  }

  private getDisplayName(serviceName: string): string {
    if (serviceName == "MailRuService") return "Почта Mail ru";
    return "Undefined";
  }
}
