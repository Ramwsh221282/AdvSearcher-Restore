import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-primary-button",
  standalone: true,
  imports: [],
  templateUrl: "./primary-button.component.html",
  styleUrl: "./primary-button.component.css",
})
export class PrimaryButtonComponent {
  @Input({ required: true }) label: string = "";
  @Output() onClicked: EventEmitter<void> = new EventEmitter();

  public clickHandle($event: Event): void {
    $event.stopPropagation();
    this.onClicked.emit();
  }
}
