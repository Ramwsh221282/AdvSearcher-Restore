import { Component, Input } from "@angular/core";
import { IPublishingDirectory } from "../publishing-advertisement-directory.interface";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { PublishingAdvertisementsComponent } from "./publishing-advertisements/publishing-advertisements.component";
import { NgForOf, NgIf } from "@angular/common";
import { PublishingDataSelectedService } from "../publishing-data-selected.service";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";

@Component({
  selector: "app-publishing-directory",
  imports: [
    PrimaryButtonComponent,
    PublishingAdvertisementsComponent,
    NgForOf,
    RedButtonComponent,
    NgIf,
  ],
  templateUrl: "./publishing-directory.component.html",
  styleUrl: "./publishing-directory.component.css",
  standalone: true,
})
export class PublishingDirectoryComponent {
  @Input({ required: true }) directory: IPublishingDirectory;
  public isFolderContentShown: boolean = false;

  constructor(private readonly _service: PublishingDataSelectedService) {}

  public selectAll(): void {
    for (let i = 0; i < this.directory.files.length; i++) {
      this._service.attachAdvertisement(this.directory.files[i]);
    }
  }

  public detachAll(): void {
    for (let i = 0; i < this.directory.files.length; i++) {
      this._service.detachAdvertisement(this.directory.files[i]);
    }
  }

  public showContent(): void {
    this.isFolderContentShown = !this.isFolderContentShown;
  }
}
