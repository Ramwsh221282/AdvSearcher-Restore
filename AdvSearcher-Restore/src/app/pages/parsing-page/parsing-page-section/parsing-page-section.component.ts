import { Component, Input } from "@angular/core";
import { PrimaryButtonComponent } from "../../../controls/primary-button/primary-button.component";
import { IParsingSection } from "../parsing-section.interface";
import { Router, RouterLink } from "@angular/router";

@Component({
  selector: "app-parsing-page-section",
  imports: [PrimaryButtonComponent, RouterLink],
  templateUrl: "./parsing-page-section.component.html",
  styleUrl: "./parsing-page-section.component.css",
  standalone: true,
})
export class ParsingPageSectionComponent {
  @Input({ required: true }) section: IParsingSection;
  constructor(private readonly _router: Router) {}

  public navigate(): void {
    this._router.navigate([
      "parse",
      this.section.name,
      this.section.displayName,
      this.section.pluginName,
    ]);
  }
}
