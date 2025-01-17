import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgClass, NgForOf, NgIf } from "@angular/common";

@Component({
  selector: "app-dropdown-list",
  imports: [NgForOf, NgIf, NgClass],
  templateUrl: "./dropdown-list.component.html",
  styleUrl: "./dropdown-list.component.css",
  standalone: true,
})
export class DropdownListComponent {
  public selectedOption: string = "";
  @Input({ required: true }) dropDownName: string = "";
  @Input({ required: true }) options: { name: string; value: string }[] = [];
  @Output() optionSelected: EventEmitter<string> = new EventEmitter();
  public isActive: boolean = false;

  public selectOption(name: string, value: string, event: Event): void {
    event.stopPropagation();
    this.selectedOption = name;
    this.optionSelected.emit(value);
    this.isActive = false;
  }

  public setVisibility(): void {
    this.isActive = !this.isActive;
  }
}
