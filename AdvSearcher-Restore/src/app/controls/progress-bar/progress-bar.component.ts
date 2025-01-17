import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from "@angular/core";
import { NgStyle } from "@angular/common";

@Component({
  selector: "app-progress-bar",
  imports: [NgStyle],
  templateUrl: "./progress-bar.component.html",
  styleUrl: "./progress-bar.component.css",
  standalone: true,
})
export class ProgressBarComponent implements OnInit, OnChanges {
  @Input() progress: number;
  @Input() total: number;
  color: string;

  public ngOnChanges(changes: SimpleChanges) {
    if (changes["progress"] || changes["total"]) {
      this.updateProgress();
    }
  }

  public ngOnInit() {
    this.updateProgress();
  }

  private updateProgress(): void {
    if (!this.progress) {
      this.progress = 0;
    }
    if (this.total === 0) {
      this.total = this.progress;
    } else if (!this.total) {
      this.total = 100;
    }
    if (this.progress > this.total) {
      this.progress = 100;
      this.total = 100;
    }
    this.progress = (this.progress / this.total) * 100;
    if (this.progress < 55) {
      this.color = "start";
    } else if (this.progress < 75) {
      this.color = "half";
    } else {
      this.color = "finish";
    }
  }
}
