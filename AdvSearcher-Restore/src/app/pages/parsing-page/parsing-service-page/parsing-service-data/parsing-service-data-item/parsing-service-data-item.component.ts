import { Component, EventEmitter, Input, Output } from "@angular/core";
import { IParsedAdvertisement } from "../parsed-advertisement.interface";
import { NgIf } from "@angular/common";
import { PrimaryButtonComponent } from "../../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../../controls/red-button/red-button.component";
import { SavingAdvertisementDialogComponent } from "./saving-advertisement-dialog/saving-advertisement-dialog.component";

@Component({
  selector: "app-parsing-service-data-item",
  imports: [
    NgIf,
    PrimaryButtonComponent,
    RedButtonComponent,
    SavingAdvertisementDialogComponent,
  ],
  templateUrl: "./parsing-service-data-item.component.html",
  styleUrl: "./parsing-service-data-item.component.css",
  standalone: true,
})
export class ParsingServiceDataItemComponent {
  @Input({ required: true }) advertisement: IParsedAdvertisement;
  @Output() removeClicked: EventEmitter<IParsedAdvertisement> =
    new EventEmitter<IParsedAdvertisement>();
  @Output() removeAndCacheClicked: EventEmitter<IParsedAdvertisement> =
    new EventEmitter<IParsedAdvertisement>();
  @Output() openInBrowserRequested: EventEmitter<IParsedAdvertisement> =
    new EventEmitter();

  protected isSaving: boolean = false;

  protected saveAsFile(): void {
    this.isSaving = true;
  }

  protected removeClick(): void {
    this.removeClicked.emit(this.advertisement);
  }

  protected cacheClick(): void {
    this.removeAndCacheClicked.emit(this.advertisement);
  }

  protected openInBrowserClick(): void {
    this.openInBrowserRequested.emit(this.advertisement);
  }
}
