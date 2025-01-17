import { Component, Input, OnInit } from "@angular/core";
import { ISettingsSection } from "./settings-item.interface";
import { InputControlComponent } from "../../../controls/input-control/input-control.component";
import { NgClass, NgForOf, NgIf } from "@angular/common";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../controls/red-button/red-button.component";
import { SettingsService } from "../settings.service";

@Component({
  selector: "app-settings-page-item",
  imports: [
    InputControlComponent,
    NgForOf,
    NgIf,
    NgClass,
    PrimaryButtonComponent,
    RedButtonComponent,
  ],
  standalone: true,
  templateUrl: "./settings-page-item.component.html",
  styleUrl: "./settings-page-item.component.css",
})
export class SettingsPageItemComponent implements OnInit {
  @Input({ required: true }) section: ISettingsSection;
  protected isContentVisible: boolean = false;

  constructor(private readonly _service: SettingsService) {}

  protected turn(): void {
    this.isContentVisible = !this.isContentVisible;
  }

  protected async setSettings(): Promise<void> {
    await this._service.setSettings(this.section);
    await this._service.readSettings(this.section);
  }

  protected async flushSettings(): Promise<void> {
    await this._service.flushSettings(this.section);
  }

  public async ngOnInit() {
    await this._service.readSettings(this.section);
  }
}
