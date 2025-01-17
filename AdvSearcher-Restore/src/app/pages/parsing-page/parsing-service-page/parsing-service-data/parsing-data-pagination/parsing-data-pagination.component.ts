import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { IParsingSection } from "../../../parsing-section.interface";
import { ParsedDataPaginationService } from "./parsing-data-pagination.service";
import { NgClass, NgForOf } from "@angular/common";

@Component({
  selector: "app-parsing-data-pagination",
  imports: [NgForOf, NgClass],
  templateUrl: "./parsing-data-pagination.component.html",
  styleUrl: "./parsing-data-pagination.component.css",
  standalone: true,
})
export class ParsingDataPaginationComponent implements OnInit {
  @Input({ required: true }) service: IParsingSection;
  @Input({ required: true }) pageSize: number;
  @Output() countChanged: EventEmitter<number> = new EventEmitter();
  @Output() currentPageChanged: EventEmitter<number> = new EventEmitter();
  protected totalCount: number = 0;
  protected pages: number[] = [];
  protected currentPage: number = -1;

  public constructor(private readonly _service: ParsedDataPaginationService) {}

  public async ngOnInit() {
    this.totalCount = await this._service.getCount(this.service);
    this.pages = await this._service.getPages(this.pageSize);
  }

  protected selectPage(page: number): void {
    this.currentPage = page;
    this.currentPageChanged.emit(this.currentPage);
  }

  protected isCurrentPage(page: number): boolean {
    return page == this.currentPage;
  }
}
