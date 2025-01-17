import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-red-button",
  imports: [],
  templateUrl: "./red-button.component.html",
  standalone: true,
  styleUrl: "./red-button.component.css",
})
export class RedButtonComponent {
  @Input({ required: true }) label: string = "";
  @Output() onClicked: EventEmitter<void> = new EventEmitter();

  public clickHandle($event: Event): void {
    $event.stopPropagation();
    this.onClicked.emit();
  }
}
