import { Injectable } from "@angular/core";
import { IPublishingDirectory } from "./publishing-advertisement-directory.interface";
import { TauriApi } from "../../api/tauri-api";

@Injectable({
  providedIn: "any",
})
export class PublishingDataService {
  public async getDirectories(): Promise<IPublishingDirectory[]> {
    return await TauriApi.invokePlugin<IPublishingDirectory[]>({
      controller: "PublishingDataController",
      action: "GetDirectories",
    });
  }
}
