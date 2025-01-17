import { Component, EventEmitter, Input, Output } from "@angular/core";
import { NgStyle } from "@angular/common";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-input-control",
  imports: [NgStyle, FormsModule],
  templateUrl: "./input-control.component.html",
  standalone: true,
  styleUrl: "./input-control.component.css",
})
export class InputControlComponent {
  @Input({ required: true }) textBinding: string = "";
  @Input({ required: true }) placeHolder: string = "";
  @Input({ required: true }) width: string = "0px";
  @Output() onTextChange: EventEmitter<string> = new EventEmitter();

  protected textChangeHandler(): void {
    this.onTextChange.emit(this.textBinding);
  }
}
