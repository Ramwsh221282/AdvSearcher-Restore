import { Injectable } from "@angular/core";
import { IPublishingAdvertisement } from "../publishing-advertisement-directory.interface";
import { TauriApi } from "../../../api/tauri-api";

@Injectable({
  providedIn: "any",
})
export class WhatsappPublishingService {
  public async publish(
    serviceName: string,
    files: IPublishingAdvertisement[],
    phoneNumber: IWhatsAppMobilePhone,
  ): Promise<void> {
    await TauriApi.invokePlugin({
      controller: "PublishDataController",
      action: "PublishToWhatsApp",
      data: {
        files: files,
        serviceName: serviceName,
        phoneNumber: {
          phoneNumber: phoneNumber.phoneNumber,
        },
      },
    });
  }
}

export interface IWhatsAppMobilePhone {
  phoneNumber: string;
}
