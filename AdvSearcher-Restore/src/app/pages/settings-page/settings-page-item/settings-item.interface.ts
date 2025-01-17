export interface ISettingsSection {
  sectionName: string;
  fileName: string;
  items: ISettingsItem[];
}

export interface ISettingsItem {
  key: string;
  value: string;
}
