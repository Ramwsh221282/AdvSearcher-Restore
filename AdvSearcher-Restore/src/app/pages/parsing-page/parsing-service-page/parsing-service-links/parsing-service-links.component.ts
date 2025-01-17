import { Component, Input, OnInit } from "@angular/core";
import { IParsingSection } from "../../parsing-section.interface";
import { InputControlComponent } from "../../../../controls/input-control/input-control.component";
import { PrimaryButtonComponent } from "../../../../controls/primary-button/primary-button.component";
import { RedButtonComponent } from "../../../../controls/red-button/red-button.component";
import { CreateLinkDialogComponent } from "./create-link-dialog/create-link-dialog.component";
import { NgForOf, NgIf } from "@angular/common";
import { NotificationComponent } from "../../../../controls/notification/notification.component";
import { NotificationsService } from "../../../../controls/notification/notifications.service";
import { ParserLink } from "./create-link-dialog/parser-link-interface";
import { ParsingLinksService } from "./create-link-dialog/parsing-links.service";

@Component({
  selector: "app-parsing-service-links",
  imports: [
    InputControlComponent,
    PrimaryButtonComponent,
    RedButtonComponent,
    CreateLinkDialogComponent,
    NgIf,
    NotificationComponent,
    NgForOf,
  ],
  templateUrl: "./parsing-service-links.component.html",
  standalone: true,
  styleUrl: "./parsing-service-links.component.css",
})
export class ParsingServiceLinksComponent implements OnInit {
  @Input({ required: true }) parsingSection: IParsingSection;
  protected links: ParserLink[] = [];
  protected isAddingNewLink: boolean = false;

  constructor(
    protected readonly notifications: NotificationsService,
    private readonly _service: ParsingLinksService,
  ) {}

  protected openLinkCreationDialog(): void {
    this.isAddingNewLink = true;
  }

  public async ngOnInit() {
    this.links = await this._service.getLinks(this.parsingSection);
  }

  public async remove(link: ParserLink) {
    await this._service.removeLink(this.parsingSection, link);
    await this.ngOnInit();
  }
}
