import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "any",
})
export class NotificationsService {
  private _isVisible: boolean = false;
  private _title: string = "";
  private _message: string = "";

  public turnOff(): void {
    this._isVisible = false;
  }

  public turnOn(): void {
    if (this.isVisible) {
      this._isVisible = false;
      this._isVisible = true;
    }
    this._isVisible = true;
  }

  public setMessage(message: string): void {
    this._message = message;
  }

  public setTitle(title: string): void {
    this._title = title;
  }

  public get isVisible(): boolean {
    return this._isVisible;
  }

  public get message(): string {
    return this._message;
  }

  public get title(): string {
    return this._title;
  }
}
