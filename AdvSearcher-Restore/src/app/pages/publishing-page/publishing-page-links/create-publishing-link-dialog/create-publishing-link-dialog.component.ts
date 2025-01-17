import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PublishingLinksService } from "../publishing-links.service";
import { InputControlComponent } from "../../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../controls/red-button/red-button.component";
import { NotificationsService } from "../../../../controls/notification/notifications.service";

@Component({
  selector: "app-create-publishing-link-dialog",
  imports: [InputControlComponent, PrimaryButtonComponent, RedButtonComponent],
  templateUrl: "./create-publishing-link-dialog.component.html",
  styleUrl: "./create-publishing-link-dialog.component.css",
  standalone: true,
})
export class CreatePublishingLinkDialogComponent {
  @Input({ required: true }) serviceName: string;
  public linkInput: string = "";
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter();
  @Output() linkAdded: EventEmitter<void> = new EventEmitter();

  constructor(
    private readonly _service: PublishingLinksService,
    protected readonly notifications: NotificationsService,
  ) {
    this._service.notifications = notifications;
  }

  protected async addLink(): Promise<void> {
    await this._service.addLink(this.serviceName, this.linkInput);
    this.linkAdded.emit();
    this.visibilityChange.emit(false);
  }

  protected closeWindow(): void {
    this.visibilityChange.emit(false);
  }
}
