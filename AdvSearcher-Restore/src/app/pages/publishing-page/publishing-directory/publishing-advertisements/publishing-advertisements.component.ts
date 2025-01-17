import { Component, Input } from "@angular/core";
import { IPublishingAdvertisement } from "../../publishing-advertisement-directory.interface";
import { PrimaryButtonComponent } from "../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../controls/red-button/red-button.component";
import { PublishingDataSelectedService } from "../../publishing-data-selected.service";
import { NgClass } from "@angular/common";

@Component({
  selector: "app-publishing-advertisements",
  imports: [PrimaryButtonComponent, RedButtonComponent, NgClass],
  templateUrl: "./publishing-advertisements.component.html",
  styleUrl: "./publishing-advertisements.component.css",
  standalone: true,
})
export class PublishingAdvertisementsComponent {
  @Input({ required: true }) advertisement: IPublishingAdvertisement;

  constructor(protected readonly _service: PublishingDataSelectedService) {}

  public select(): void {
    this._service.attachAdvertisement(this.advertisement);
  }

  public detach(): void {
    this._service.detachAdvertisement(this.advertisement);
  }
}
