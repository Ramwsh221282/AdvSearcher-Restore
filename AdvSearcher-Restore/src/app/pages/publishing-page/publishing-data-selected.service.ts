import { Injectable } from "@angular/core";
import { IPublishingAdvertisement } from "./publishing-advertisement-directory.interface";

@Injectable({
  providedIn: "any",
})
export class PublishingDataSelectedService {
  private readonly files: IPublishingAdvertisement[] = [];

  public attachAdvertisement(advertisement: IPublishingAdvertisement): void {
    if (this.files.some((ad) => ad.displayName == advertisement.displayName))
      return;
    this.files.push(advertisement);
  }

  public detachAdvertisement(advertisement: IPublishingAdvertisement): void {
    const index = this.files.indexOf(advertisement);
    if (index < 0) return;
    this.files.splice(index, 1);
  }

  public isAdvertisementAttached(
    advertisement: IPublishingAdvertisement,
  ): boolean {
    const index = this.files.indexOf(advertisement);
    return index >= 0;
  }

  public get selectedAdvertisements(): IPublishingAdvertisement[] {
    return this.files;
  }
}
