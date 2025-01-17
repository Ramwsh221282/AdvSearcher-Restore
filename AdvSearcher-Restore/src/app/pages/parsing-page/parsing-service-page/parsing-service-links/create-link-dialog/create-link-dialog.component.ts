import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PrimaryButtonComponent } from "../../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../../controls/red-button/red-button.component";
import { InputControlComponent } from "../../../../../controls/input-control/input-control.component";
import { IParsingSection } from "../../../parsing-section.interface";
import { ParsingLinksService } from "./parsing-links.service";
import { ParserLink, ParserService } from "./parser-link-interface";
import { NotificationsService } from "../../../../../controls/notification/notifications.service";
import { NotificationComponent } from "../../../../../controls/notification/notification.component";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-create-link-dialog",
  imports: [
    PrimaryButtonComponent,
    RedButtonComponent,
    InputControlComponent,
    NotificationComponent,
    NgIf,
  ],
  templateUrl: "./create-link-dialog.component.html",
  styleUrl: "./create-link-dialog.component.css",
  standalone: true,
})
export class CreateLinkDialogComponent {
  protected link: string = "";
  @Output() closeEmitted: EventEmitter<void> = new EventEmitter();
  @Output() linkAdded: EventEmitter<void> = new EventEmitter();
  @Input({ required: true }) section: IParsingSection;

  constructor(
    private readonly _service: ParsingLinksService,
    protected readonly notifications: NotificationsService,
  ) {
    this._service.notifications = notifications;
  }

  protected async appendLink(): Promise<void> {
    const linkRequestArgument: ParserLink = {} as ParserLink;
    linkRequestArgument.link = this.link;
    const parserRequestArgument: ParserService = {} as ParserService;
    parserRequestArgument.serviceName = this.section.name;
    await this._service.addLink(parserRequestArgument, linkRequestArgument);
    this.linkAdded.emit();
  }

  protected closeWindow(): void {
    this.closeEmitted.emit();
  }
}
