import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterOutlet } from "@angular/router";
import { TauriApi } from "./api/tauri-api";
import { listen } from "@tauri-apps/api/event";
import { SideBarMenuComponent } from "./components/side-bar-menu/side-bar-menu.component";
import { NotificationsService } from "./controls/notification/notifications.service";

@Component({
  selector: "app-root",
  imports: [CommonModule, RouterOutlet, SideBarMenuComponent],
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
  standalone: true,
  providers: [NotificationsService],
})
export class AppComponent implements OnInit {
  public async login(): Promise<void> {
    const response = await TauriApi.invokePlugin<any>({
      controller: "GoodbyeWorldController",
      action: "SayGoodbyeWorld",
    });
    console.log(response["result"]);
  }

  public async helloWorld(): Promise<void> {
    const response = await TauriApi.invokePlugin<any>({
      controller: "HelloWorldController",
      action: "SayHelloWorld",
    });
    console.log(response);
  }

  public ngOnInit() {
    listen("avito-parser", (event) => {
      console.log(`news-feed: ${event.payload}`);
    });
  }

  public async invokeParserTest(): Promise<void> {
    const response = await TauriApi.invokePlugin<any>({
      controller: "TestAvitoController",
      action: "Run",
    });
    console.log(response);
  }
}
