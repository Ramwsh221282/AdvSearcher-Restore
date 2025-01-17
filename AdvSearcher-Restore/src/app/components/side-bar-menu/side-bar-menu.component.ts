import { Component, OnInit } from "@angular/core";
import { ISideBarButton } from "./side-bar-components/side-bar-buttons/sidebar-button-interface";
import { SideBarButtonsComponent } from "./side-bar-components/side-bar-buttons/side-bar-buttons.component";
import { NgForOf } from "@angular/common";

@Component({
  selector: "app-side-bar-menu",
  imports: [SideBarButtonsComponent, NgForOf],
  templateUrl: "./side-bar-menu.component.html",
  standalone: true,
  styleUrl: "./side-bar-menu.component.css",
})
export class SideBarMenuComponent implements OnInit {
  protected readonly sideBarButtons: ISideBarButton[] = [];
  private selectedButton: ISideBarButton | null = null;

  public ngOnInit() {
    this.appendSideBarButton("Домашняя страница", "home");
    this.appendSideBarButton("Парсинг", "parsing");
    this.appendSideBarButton("Постинг", "posting");
    this.appendSideBarButton("Настройки", "settings");
    this.appendSideBarButton("Обработка фото", "inpainting");
  }

  private appendSideBarButton(buttonLabel: string, buttonPath: string): void {
    const button: ISideBarButton = {} as ISideBarButton;
    button.buttonLabel = buttonLabel;
    button.buttonRoute = buttonPath;
    this.sideBarButtons.push(button);
  }

  protected setSelectedButton(selectedButton: ISideBarButton): void {
    this.selectedButton = selectedButton;
  }

  protected isCurrentButton(button: ISideBarButton): boolean {
    if (!this.selectedButton) return false;
    return this.selectedButton.buttonLabel == button.buttonLabel;
  }
}
