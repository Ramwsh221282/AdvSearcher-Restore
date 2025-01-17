import { Injectable } from "@angular/core";
import { IPublishingAdvertisement } from "../publishing-advertisement-directory.interface";
import { TauriApi } from "../../../api/tauri-api";

@Injectable({
  providedIn: "any",
})
export class MailPublishingService {
  public async publish(
    serviceName: string,
    files: IPublishingAdvertisement[],
    address: MailAddress,
  ): Promise<void> {
    await TauriApi.invokePlugin({
      controller: "PublishDataController",
      action: "PublishToEmail",
      data: {
        files: files,
        serviceName: serviceName,
        address: {
          email: address.email,
          subject: address.subject,
        },
      },
    });
  }
}

export interface MailAddress {
  email: string;
  subject: string;
}
