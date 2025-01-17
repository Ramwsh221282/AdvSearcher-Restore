import { Injectable } from "@angular/core";
import { IParsingSection } from "../../../parsing-section.interface";
import { TauriApi } from "../../../../../api/tauri-api";

@Injectable({
  providedIn: "any",
})
export class ParsedDataPaginationService {
  public async getCount(service: IParsingSection): Promise<number> {
    return await TauriApi.invokePlugin<number>({
      controller: "ParsedDataController",
      action: "GetCountQuery",
      data: { serviceName: service.name },
    });
  }

  public async getPages(pageSize: number): Promise<number[]> {
    const response = await TauriApi.invokePlugin<number[]>({
      controller: "ParsedDataController",
      action: "GetPagesQuery",
      data: { pageSize: pageSize },
    });
    const pages: number[] = [];
    for (let i = 0; i < response.length; i++) {
      pages.push(response[i]);
    }
    return pages;
  }
}
