export interface LinkRequest {
  parser: ParserService;
  link: ParserLink;
}

export interface ParserService {
  serviceName: string;
}

export interface ParserLink {
  link: string;
}
