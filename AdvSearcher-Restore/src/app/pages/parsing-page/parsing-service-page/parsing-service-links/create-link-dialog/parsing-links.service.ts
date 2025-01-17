import { Injectable } from "@angular/core";
import {
  LinkRequest,
  ParserLink,
  ParserService,
} from "./parser-link-interface";
import { TauriApi } from "../../../../../api/tauri-api";
import { NotificationsService } from "../../../../../controls/notification/notifications.service";
import { IParsingSection } from "../../../parsing-section.interface";
import { listen, UnlistenFn } from "@tauri-apps/api/event";

@Injectable({
  providedIn: "any",
})
export class ParsingLinksService {
  constructor() {}

  public notifications: NotificationsService;

  public async addLink(parser: ParserService, link: ParserLink): Promise<void> {
    const request: LinkRequest = {} as LinkRequest;
    request.parser = parser;
    request.link = link;
    const listener = this.createListener();
    await TauriApi.invokePlugin({
      controller: "ParserLinksController",
      action: "AddLink",
      data: request,
    });
    await listener;
  }

  public async getLinks(section: IParsingSection): Promise<ParserLink[]> {
    const parser: ParserService = {} as ParserService;
    parser.serviceName = section.name;
    const response = await TauriApi.invokePlugin<string[]>({
      controller: "ParserLinksController",
      action: "GetLinks",
      data: parser,
    });
    const result: ParserLink[] = [];
    for (let i = 0; i < response.length; i++) {
      const link: ParserLink = {} as ParserLink;
      link.link = response[i];
      result.push(link);
    }
    return result;
  }

  public async removeLink(
    section: IParsingSection,
    link: ParserLink,
  ): Promise<void> {
    const parser: ParserService = {} as ParserService;
    parser.serviceName = section.name;
    const request: LinkRequest = {} as LinkRequest;
    request.parser = parser;
    request.link = link;
    const listener = this.createListener();
    await TauriApi.invokePlugin({
      controller: "ParserLinksController",
      action: "RemoveLink",
      data: request,
    });
    await listener;
  }

  private createListener(): Promise<UnlistenFn> {
    return listen("parser-link-listener", (event) => {
      this.notifications.setTitle("Уведомление");
      this.notifications.setMessage(`${event.payload}`);
      this.notifications.turnOn();
    });
  }
}
