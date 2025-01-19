import { Component, Input, OnInit, signal } from "@angular/core";
import { IParsingSection } from "../../parsing-section.interface";
import { ParsingServiceDataItemComponent } from "./parsing-service-data-item/parsing-service-data-item.component";
import { ParsedAdvertisementsDataService } from "./parsing-service-data-item/parsed-advertisements-data.service";
import { IParsedAdvertisement } from "./parsed-advertisement.interface";
import { NgClass, NgForOf, NgIf } from "@angular/common";
import { ParsingDataPaginationComponent } from "./parsing-data-pagination/parsing-data-pagination.component";
import { NotificationComponent } from "../../../../controls/notification/notification.component";
import { NotificationsService } from "../../../../controls/notification/notifications.service";
import { RedButtonComponent } from "../../../../controls/red-button/red-button.component";

@Component({
  selector: "app-parsing-service-data",
  imports: [
    ParsingServiceDataItemComponent,
    NgForOf,
    NgClass,
    NgIf,
    ParsingDataPaginationComponent,
    NotificationComponent,
    RedButtonComponent,
  ],
  templateUrl: "./parsing-service-data.component.html",
  standalone: true,
  styleUrl: "./parsing-service-data.component.css",
})
export class ParsingServiceDataComponent implements OnInit {
  @Input({ required: true }) plugin: IParsingSection;
  protected advertisements = signal<IParsedAdvertisement[]>([]);
  protected page: number = 1;
  protected pageSize: number = 12;
  protected selectedAdvertisement: IParsedAdvertisement | null = null;
  protected currentImageIndex = 0;
  protected maxImageIndex = 0;

  constructor(
    private readonly _service: ParsedAdvertisementsDataService,
    protected readonly notifications: NotificationsService,
  ) {
    this._service.notifications = this.notifications;
  }

  public async ngOnInit() {
    const response = await this._service.getData(
      this.plugin,
      this.page,
      this.pageSize,
    );
    this.advertisements.set(response);
  }

  protected select(advertisement: IParsedAdvertisement): void {
    this.selectedAdvertisement = { ...advertisement };
    this.currentImageIndex = 0;
    this.maxImageIndex = this.selectedAdvertisement.imageLinks.length - 1;
    console.log(this.currentImageIndex);
    console.log(this.maxImageIndex);
  }

  protected async handlePageChangedEvent(pageChanged$: number) {
    window.scroll(0, 0);
    this.page = pageChanged$;
    await this.ngOnInit();
  }

  protected async handleDeletion(
    advertisement: IParsedAdvertisement,
  ): Promise<void> {
    await this._service.removeData(advertisement);
    this.advertisements.set([]);
    await this.ngOnInit();
    this.selectedAdvertisement = null;
  }

  protected async handleCaching(
    advertisement: IParsedAdvertisement,
  ): Promise<void> {
    await this._service.cacheData(advertisement);
    this.advertisements.set([]);
    await this.ngOnInit();
    this.selectedAdvertisement = null;
  }

  protected async deleteAll(): Promise<void> {
    await this._service.removeAllData(this.plugin);
    this.advertisements.set([]);
    await this.ngOnInit();
    this.selectedAdvertisement = null;
  }

  protected async cacheAll(): Promise<void> {
    await this._service.cacheAllData(this.plugin);
    this.advertisements.set([]);
    await this.ngOnInit();
    this.selectedAdvertisement = null;
  }

  protected async handleOpenInBrowser(
    advertisement: IParsedAdvertisement,
  ): Promise<void> {
    await this._service.openInBrowser(advertisement);
  }
}
