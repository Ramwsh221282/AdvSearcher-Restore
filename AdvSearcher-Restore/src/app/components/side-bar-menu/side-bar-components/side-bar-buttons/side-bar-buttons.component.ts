import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ISideBarButton } from "./sidebar-button-interface";
import { Router } from "@angular/router";
import { NgClass } from "@angular/common";

@Component({
  selector: "app-side-bar-buttons",
  imports: [NgClass],
  templateUrl: "./side-bar-buttons.component.html",
  styleUrl: "./side-bar-buttons.component.css",
  standalone: true,
})
export class SideBarButtonsComponent {
  @Input({ required: true }) button: ISideBarButton;
  @Input({ required: true }) isSelected: boolean = false;
  @Output() buttonSelected: EventEmitter<ISideBarButton> = new EventEmitter();

  constructor(private readonly router: Router) {}

  protected click(): void {
    this.buttonSelected.emit(this.button);
    this.router.navigate([this.button.buttonRoute]);
  }
}
