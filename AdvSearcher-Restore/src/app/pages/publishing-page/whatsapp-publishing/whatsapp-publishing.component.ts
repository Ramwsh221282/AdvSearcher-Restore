import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PublishingDataSelectedService } from "../publishing-data-selected.service";
import { NotificationsService } from "../../../controls/notification/notifications.service";
import {
  IWhatsAppMobilePhone,
  WhatsappPublishingService,
} from "./whatsapp-publishing.service";
import { InputControlComponent } from "../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";
import { NgxLoadingBar } from "@ngx-loading-bar/core";

@Component({
  selector: "app-whatsapp-publishing",
  imports: [
    InputControlComponent,
    PrimaryButtonComponent,
    RedButtonComponent,
    NgxLoadingBar,
  ],
  templateUrl: "./whatsapp-publishing.component.html",
  styleUrl: "./whatsapp-publishing.component.css",
  standalone: true,
})
export class WhatsappPublishingComponent {
  public serviceDisplayName: string = "";
  public phone: IWhatsAppMobilePhone;
  @Input({ required: true }) selectedServiceOption: string = "";
  @Output() visibilityChange: EventEmitter<void> = new EventEmitter();

  constructor(
    protected readonly _service: WhatsappPublishingService,
    private readonly _selectedData: PublishingDataSelectedService,
    protected readonly notifications: NotificationsService,
  ) {
    this.phone = {} as IWhatsAppMobilePhone;
    this.phone.phoneNumber = "";
    this._service.notifications = this.notifications;
  }

  public ngOnInit() {
    this.serviceDisplayName = this.getDisplayName(this.selectedServiceOption);
  }

  public closeWindow(): void {
    this.visibilityChange.emit();
  }

  public async publish(): Promise<void> {
    if (this.phone.phoneNumber.trim().length == 0) return;
    await this._service.publish(
      this.selectedServiceOption,
      this._selectedData.selectedAdvertisements,
      this.phone,
    );
    this.notifications.setTitle("Уведомление");
    this.notifications.setMessage("Завершено");
    this.notifications.turnOn();
    this.closeWindow();
  }

  private getDisplayName(serviceName: string): string {
    if (serviceName == "GreenApiService") return "Whats App";
    return "Undefined";
  }
}
