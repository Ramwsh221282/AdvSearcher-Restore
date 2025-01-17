import { Component, OnInit } from "@angular/core";
import { InputControlComponent } from "../input-control/input-control.component";
import { PrimaryButtonComponent } from "../primary-button/primary-button.component";
import { RedButtonComponent } from "../red-button/red-button.component";
import { NotificationsService } from "./notifications.service";

@Component({
  selector: "app-notification",
  imports: [InputControlComponent, PrimaryButtonComponent, RedButtonComponent],
  templateUrl: "./notification.component.html",
  standalone: true,
  styleUrl: "./notification.component.css",
})
export class NotificationComponent implements OnInit {
  protected title: string;
  protected message: string;

  constructor(private readonly notifications: NotificationsService) {
    this.title = notifications.title;
    this.message = notifications.message;
  }

  public ngOnInit() {
    setTimeout(() => {
      this.notifications.turnOff();
    }, 5000);
  }
}
