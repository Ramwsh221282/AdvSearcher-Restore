export interface IPublishingDirectory {
  path: string;
  displayName: string;
  files: IPublishingAdvertisement[];
}

export interface IPublishingAdvertisement {
  path: string;
  displayName: string;
  photos: string[];
}
