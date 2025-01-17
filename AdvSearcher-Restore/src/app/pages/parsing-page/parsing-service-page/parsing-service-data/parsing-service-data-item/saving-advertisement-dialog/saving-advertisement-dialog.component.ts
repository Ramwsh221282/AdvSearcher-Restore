import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { AdvertisementFileSavingService } from "./advertisement-file-saving.service";
import { InputControlComponent } from "../../../../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../../../controls/red-button/red-button.component";
import { NgForOf, NgIf } from "@angular/common";
import { IParsedAdvertisement } from "../../parsed-advertisement.interface";
import { NotificationsService } from "../../../../../../controls/notification/notifications.service";
import { NotificationComponent } from "../../../../../../controls/notification/notification.component";

@Component({
  selector: "app-saving-advertisement-dialog",
  imports: [
    InputControlComponent,
    PrimaryButtonComponent,
    RedButtonComponent,
    NgForOf,
    NotificationComponent,
    NgIf,
  ],
  templateUrl: "./saving-advertisement-dialog.component.html",
  styleUrl: "./saving-advertisement-dialog.component.css",
  standalone: true,
})
export class SavingAdvertisementDialogComponent implements OnInit {
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter();
  @Input({ required: true }) advertisement: IParsedAdvertisement;
  protected folders: string[] = [];
  protected selectedPaths: string[] = [];
  protected Path = () => this.selectedPaths.join("/");
  protected advertisementFileName: string = "";

  constructor(
    private readonly _service: AdvertisementFileSavingService,
    protected readonly notifications: NotificationsService,
  ) {
    this.selectedPaths.push("Data");
    _service.notifications = this.notifications;
  }

  protected closeWindow(): void {
    this.visibilityChange.emit(false);
  }

  protected async moveToDirectory(directory: string): Promise<void> {
    this.folders = await this._service.moveToDirectory(directory);
    this.selectedPaths.push(directory);
  }

  protected async moveToParent(): Promise<void> {
    this.folders = await this._service.moveToParent();
    this.selectedPaths.pop();
  }

  protected async createDirectory(): Promise<void> {
    if (
      this.advertisementFileName.length == 0 ||
      this.advertisementFileName.trim().length == 0
    ) {
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage("Необходимо указать название файла");
      this.notifications.turnOn();
      return;
    }
    await this._service.createDirectory(this.advertisementFileName);
    this.advertisementFileName = "";
    await this.ngOnInit();
  }

  protected async saveFile(): Promise<void> {
    if (
      this.advertisementFileName.length == 0 ||
      this.advertisementFileName.trim().length == 0
    ) {
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage("Необходимо указать название файла");
      this.notifications.turnOn();
      return;
    }
    await this._service.saveAdvertisement(
      this.advertisement,
      this.advertisementFileName,
    );
    this.advertisementFileName = "";
  }

  public async ngOnInit() {
    this.folders = await this._service.getFolders();
  }
}
