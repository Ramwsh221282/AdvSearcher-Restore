import { Routes } from "@angular/router";
import { canActivateAuth } from "./guards/auth-guard";

export const routes: Routes = [
  {
    path: "settings",
    loadComponent: () =>
      import("./pages/settings-page/settings-page.component").then(
        (component) => component.SettingsPageComponent,
      ),
  },
  {
    path: "parsing",
    loadComponent: () =>
      import("./pages/parsing-page/parsing-page.component").then(
        (component) => component.ParsingPageComponent,
      ),
    canActivate: [canActivateAuth],
  },
  {
    path: "parse/:name/:displayName/:pluginName",
    loadComponent: () =>
      import(
        "./pages/parsing-page/parsing-service-page/parsing-service-page.component"
      ).then((component) => component.ParsingServicePageComponent),
  },
  {
    path: "posting",
    loadComponent: () =>
      import("./pages/publishing-page/publishing-page.component").then(
        (component) => component.PublishingPageComponent,
      ),
    canActivate: [canActivateAuth],
  },
  {
    path: "posting-links",
    loadComponent: () =>
      import(
        "./pages/publishing-page/publishing-page-links/publishing-page-links.component"
      ).then((component) => component.PublishingPageLinksComponent),
    canActivate: [canActivateAuth],
  },
  {
    path: "inpainting",
    loadComponent: () =>
      import("./pages/inpainting-page/inpainting-page.component").then(
        (component) => component.InpaintingPageComponent,
      ),
    canActivate: [canActivateAuth],
  },
];
