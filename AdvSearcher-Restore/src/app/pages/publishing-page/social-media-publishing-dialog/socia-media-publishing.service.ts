import { Injectable } from "@angular/core";
import { IPublishingAdvertisement } from "../publishing-advertisement-directory.interface";
import { TauriApi } from "../../../api/tauri-api";

@Injectable({
  providedIn: "any",
})
export class SocialMediaPublishingService {
  public async publish(
    serviceName: string,
    files: IPublishingAdvertisement[],
  ): Promise<void> {
    await TauriApi.invokePlugin({
      controller: "PublishDataController",
      action: "PublishToSocialMedia",
      data: {
        files: files,
        serviceName: serviceName,
      },
    });
  }
}
