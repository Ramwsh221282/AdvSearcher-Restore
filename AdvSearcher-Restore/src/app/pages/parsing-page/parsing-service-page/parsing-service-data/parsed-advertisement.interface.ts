export interface IParsedAdvertisement {
  id: number;
  sourceUrl: string;
  content: string;
  publisher: string;
  imageLinks: string[];
  publishDate: string;
  parsedDate: string;
}
