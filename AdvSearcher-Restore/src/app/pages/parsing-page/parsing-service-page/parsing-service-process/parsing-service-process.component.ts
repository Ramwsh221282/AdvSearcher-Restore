import { Component, Input } from "@angular/core";
import { NgForOf, NgIf } from "@angular/common";
import { ParsingPageSectionComponent } from "../../parsing-page-section/parsing-page-section.component";
import { PrimaryButtonComponent } from "../../../../controls/primary-button/primary-button.component";
import { ParsingProcessService } from "./parsing-process-service";
import { NotificationComponent } from "../../../../controls/notification/notification.component";
import { IParsingSection } from "../../parsing-section.interface";
import { ProgressBarComponent } from "../../../../controls/progress-bar/progress-bar.component";
import { FormsModule } from "@angular/forms";
import { NotificationsService } from "../../../../controls/notification/notifications.service";
import { ParsingProcessFilterDialogComponent } from "./parsing-process-filter-dialog/parsing-process-filter-dialog.component";
import { IParsingOptions } from "./parsing-process-filter-dialog/parsing-options.interface";

@Component({
  selector: "app-parsing-service-process",
  imports: [
    NgForOf,
    ParsingPageSectionComponent,
    PrimaryButtonComponent,
    NotificationComponent,
    NgIf,
    ProgressBarComponent,
    FormsModule,
    ParsingProcessFilterDialogComponent,
  ],
  standalone: true,
  templateUrl: "./parsing-service-process.component.html",
  styleUrl: "./parsing-service-process.component.css",
})
export class ParsingServiceProcessComponent {
  @Input({ required: true }) section: IParsingSection;
  protected isFilterOptionsVisible: boolean = false;
  protected options: IParsingOptions = {} as IParsingOptions;

  constructor(
    protected readonly _service: ParsingProcessService,
    protected readonly notifications: NotificationsService,
  ) {
    this._service.notifications = notifications;
    this.options.endDate = null;
    this.options.startDate = null;
    this.options.withCachedAds = false;
    this.options.withIgnoreNames = false;
  }

  public async invokeParsing(): Promise<void> {
    await this._service.invokeParsing(this.section, this.options);
  }

  public openFilterOptions(): void {
    this.isFilterOptionsVisible = true;
  }

  public handleOptionsAssigned(options: IParsingOptions): void {
    this.options = { ...options };
  }
}
