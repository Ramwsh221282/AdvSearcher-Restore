import { Component, EventEmitter, Output } from "@angular/core";
import { IParsingOptions } from "./parsing-options.interface";
import { InputControlComponent } from "../../../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../../controls/red-button/red-button.component";
import { NgIf } from "@angular/common";
import { NotificationsService } from "../../../../../controls/notification/notifications.service";

@Component({
  selector: "app-parsing-process-filter-dialog",
  imports: [
    InputControlComponent,
    PrimaryButtonComponent,
    RedButtonComponent,
    NgIf,
  ],
  templateUrl: "./parsing-process-filter-dialog.component.html",
  styleUrl: "./parsing-process-filter-dialog.component.css",
  standalone: true,
})
export class ParsingProcessFilterDialogComponent {
  @Output() visibilityChange: EventEmitter<boolean> = new EventEmitter();
  @Output() filtersApplied: EventEmitter<IParsingOptions> = new EventEmitter();
  protected options: IParsingOptions = {} as IParsingOptions;
  protected startDateInput: string = "";
  protected endDateInput: string = "";
  protected dateInputsVisible: boolean = false;
  private readonly _format = /^\d{1,2}\.\d{1,2}\.\d{4}$/;

  constructor(private readonly notifications: NotificationsService) {
    this.options.endDate = null;
    this.options.startDate = null;
    this.options.withCachedAds = false;
    this.options.withIgnoreNames = false;
  }

  protected enableDateFiltering(): void {
    this.dateInputsVisible = true;
  }

  protected disableDateFiltering(): void {
    this.dateInputsVisible = false;
    this.startDateInput = "";
    this.endDateInput = "";
  }

  protected turnIgnoreNames(): void {
    this.options.withIgnoreNames = !this.options.withIgnoreNames;
  }

  protected turnCacheFilter(): void {
    this.options.withCachedAds = !this.options.withCachedAds;
  }

  protected closeWindow(): void {
    this.visibilityChange.emit(false);
  }

  protected applyFilters(): void {
    this.startDateInput = this.startDateInput.trim();
    this.endDateInput = this.endDateInput.trim();
    if (this.dateInputsVisible) {
      if (!this.isDateMatchingFormat()) {
        this.notifications.setTitle("Уведомление");
        this.notifications.setMessage("Дата должна быть формата дд.мм.гггг");
        this.notifications.turnOn();
        return;
      }
      this.options.startDate = this.startDateInput;
      this.options.endDate = this.endDateInput;
    }
    this.filtersApplied.emit(this.options);
    this.visibilityChange.emit(false);
  }

  private isDateMatchingFormat(): boolean {
    return (
      this._format.test(this.startDateInput) &&
      this._format.test(this.endDateInput)
    );
  }
}
