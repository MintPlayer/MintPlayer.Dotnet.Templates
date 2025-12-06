# MintPlayer.Dotnet.Templates - Unified Template Implementation Plan

## Overview

This plan outlines the creation of a single, comprehensive .NET template that consolidates all features from the MintPlayer.AspNetCore.Templates repository into a unified, configurable template.

## Template Options Summary

| Option | Type | Choices/Default | Description |
|--------|------|-----------------|-------------|
| EnableHttps | bool | true | HTTPS redirection |
| EnableHsts | bool | true | HTTP Strict Transport Security |
| DatabaseProvider | choice | SqlServer, PostgreSQL, SQLite | Database provider |
| UiFramework | choice | None, MintPlayerNgBootstrap, NgBootstrap | UI component library |
| IdentityProvider | choice | None, IdentityServer, OpenIdDict | Authentication provider |
| ServerSideRendering | choice | None, OnSupplyData | SSR implementation |
| HtmlMinification | choice | None, WebMarkupMin | HTML/CSS/JS minification |
| Translations | choice | None, NgxTranslate | i18n support |
| EnablePwa | bool | false | Progressive Web App |
| EnableXsrf | bool | true | XSRF/CSRF protection |
| EnableTwoFactor | bool | false | 2FA with authenticator apps |
| EnableExternalLogins | bool | false | Master toggle for external providers |
| ExternalLoginMicrosoft | bool | false | Microsoft Account login |
| ExternalLoginGoogle | bool | false | Google login |
| ExternalLoginFacebook | bool | false | Facebook login |
| ExternalLoginX | bool | false | X (Twitter) login |
| ExternalLoginLinkedIn | bool | false | LinkedIn login |
| EnableEmailConfirmation | bool | false | Email verification |
| EnableConcurrencyHandling | bool | false | Optimistic concurrency |
| SearchProvider | choice | None, ElasticSearch | Search engine |
| EnableDocker | bool | false | Docker support |
| EnableOpenSearch | bool | false | OpenSearch descriptor |
| IncludeTests | bool | false | Include unit test project scaffolding |

---

## Project Structure

```
content/
└── WebApplication/
    ├── .template.config/
    │   └── template.json
    ├── Web/
    │   ├── ClientApp/                    # Angular application
    │   │   ├── src/
    │   │   │   ├── app/
    │   │   │   │   ├── components/       # Shared components
    │   │   │   │   ├── guards/           # Route guards
    │   │   │   │   ├── interceptors/     # HTTP interceptors
    │   │   │   │   ├── pages/
    │   │   │   │   │   ├── home/
    │   │   │   │   │   └── account/      # [IdentityProvider != None]
    │   │   │   │   │       ├── login/
    │   │   │   │   │       ├── register/
    │   │   │   │   │       ├── profile/
    │   │   │   │   │       └── two-factor/  # [EnableTwoFactor]
    │   │   │   │   ├── services/
    │   │   │   │   └── entities/
    │   │   │   ├── assets/
    │   │   │   │   └── i18n/             # [Translations == NgxTranslate]
    │   │   │   ├── environments/
    │   │   │   ├── index.html
    │   │   │   ├── main.ts
    │   │   │   ├── main.server.ts        # [ServerSideRendering != None]
    │   │   │   └── styles.scss
    │   │   ├── angular.json
    │   │   ├── package.json
    │   │   ├── tsconfig.json
    │   │   ├── tsconfig.app.json
    │   │   ├── tsconfig.server.json      # [ServerSideRendering != None]
    │   │   └── ngsw-config.json          # [EnablePwa]
    │   ├── Server/
    │   │   ├── Controllers/
    │   │   │   ├── Api/
    │   │   │   │   └── V1/
    │   │   │   │       ├── AccountController.cs  # [IdentityProvider != None]
    │   │   │   │       └── SearchController.cs   # [SearchProvider != None]
    │   │   │   └── Web/
    │   │   │       └── AuthController.cs         # [IdentityProvider != None]
    │   │   ├── ViewModels/
    │   │   └── Views/
    │   ├── Services/
    │   │   ├── SpaPrerenderingService.cs         # [ServerSideRendering != None]
    │   │   └── SearchService.cs                  # [SearchProvider != None]
    │   ├── Extensions/
    │   │   ├── ServiceCollectionExtensions.cs
    │   │   └── XsrfExtensions.cs                 # [EnableXsrf]
    │   ├── Program.cs
    │   ├── appsettings.json
    │   ├── appsettings.Development.json
    │   ├── appsettings.Production.json
    │   ├── Dockerfile                            # [EnableDocker]
    │   ├── .dockerignore                         # [EnableDocker]
    │   └── MintPlayer.Dotnet.WebApplication.Web.csproj
    ├── Data/
    │   ├── Entities/
    │   │   └── User.cs                           # [IdentityProvider != None]
    │   ├── Extensions/
    │   │   └── ServiceCollectionExtensions.cs
    │   ├── Options/
    │   │   └── AppOptions.cs
    │   ├── Persistance/
    │   │   └── AppDbContext.cs
    │   ├── Services/
    │   │   ├── AccountService.cs                 # [IdentityProvider != None]
    │   │   └── SearchService.cs                  # [SearchProvider != None]
    │   └── MintPlayer.Dotnet.WebApplication.Data.csproj
    ├── Dtos/
    │   ├── Dtos/
    │   │   ├── User.cs                           # [IdentityProvider != None]
    │   │   ├── LoginRequest.cs                   # [IdentityProvider != None]
    │   │   └── RegisterRequest.cs                # [IdentityProvider != None]
    │   └── MintPlayer.Dotnet.WebApplication.Dtos.csproj
    └── Tests/                                    # [IncludeTests]
        ├── MintPlayer.Dotnet.WebApplication.Tests.csproj
        ├── GlobalUsings.cs
        ├── Web/
        │   └── Controllers/
        │       └── AccountControllerTests.cs     # [IdentityProvider != None]
        └── Data/
            └── Services/
                └── AccountServiceTests.cs        # [IdentityProvider != None]
```

---

## Implementation Phases

### Phase 1: Core Template Infrastructure

**Files to create:**
1. `.template.config/template.json` - Complete symbol definitions
2. Solution file structure
3. Base project files (.csproj) with conditional PackageReferences

**template.json structure:**
```json
{
  "$schema": "http://json.schemastore.org/template",
  "author": "MintPlayer",
  "classifications": ["Web", "Angular", "ASP.NET Core"],
  "identity": "MintPlayer.Dotnet.Templates.CSharp",
  "groupIdentity": "MintPlayer.Dotnet",
  "shortName": "mintplayer-web",
  "name": "ASP.NET Core with Angular",
  "sourceName": "MintPlayer.Dotnet.WebApplication",
  "symbols": { /* all 20+ symbols */ },
  "sources": [
    {
      "modifiers": [
        // Conditional file inclusion/exclusion
      ]
    }
  ],
  "primaryOutputs": [
    { "path": "Web/MintPlayer.Dotnet.WebApplication.Web.csproj" }
  ]
}
```

### Phase 2: ASP.NET Core Web Project

**Program.cs conditional sections:**
```csharp
// Base setup
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

#if (EnableHttps)
// HTTPS redirection
#endif

#if (EnableHsts)
// HSTS configuration
#endif

#if (IdentityProvider == "IdentityServer")
builder.Services.AddIdentityServer()...
#elif (IdentityProvider == "OpenIdDict")
builder.Services.AddOpenIddict()...
#endif

#if (EnableXsrf)
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
#endif

#if (HtmlMinification == "WebMarkupMin")
builder.Services.AddWebMarkupMin()...
#endif

#if (SearchProvider == "ElasticSearch")
builder.Services.AddElasticSearch()...
#endif

#if (EnableOpenSearch)
builder.Services.AddOpenSearchDescriptor()...
#endif

// ... app pipeline
```

**NuGet packages (conditional):**
| Package | Condition |
|---------|-----------|
| Microsoft.EntityFrameworkCore.SqlServer | DatabaseProvider == SqlServer |
| Npgsql.EntityFrameworkCore.PostgreSQL | DatabaseProvider == PostgreSQL |
| Microsoft.EntityFrameworkCore.Sqlite | DatabaseProvider == SQLite |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | IdentityProvider != None |
| IdentityServer4 | IdentityProvider == IdentityServer |
| OpenIddict.AspNetCore | IdentityProvider == OpenIdDict |
| WebMarkupMin.AspNetCore10 | HtmlMinification == WebMarkupMin |
| NEST | SearchProvider == ElasticSearch |
| MintPlayer.AspNetCore.SpaServices.Routing | Always |
| MintPlayer.AspNetCore.SpaServices.Prerendering | ServerSideRendering != None |
| MintPlayer.AspNetCore.SpaServices.Xsrf | EnableXsrf |
| MintPlayer.AspNetCore.OpenSearch | EnableOpenSearch |
| Microsoft.AspNetCore.Authentication.MicrosoftAccount | ExternalLoginMicrosoft |
| Microsoft.AspNetCore.Authentication.Google | ExternalLoginGoogle |
| Microsoft.AspNetCore.Authentication.Facebook | ExternalLoginFacebook |
| AspNet.Security.OAuth.Twitter | ExternalLoginX |
| AspNet.Security.OAuth.LinkedIn | ExternalLoginLinkedIn |
| xunit | IncludeTests |
| xunit.runner.visualstudio | IncludeTests |
| Microsoft.NET.Test.Sdk | IncludeTests |
| Moq | IncludeTests |
| FluentAssertions | IncludeTests |

### Phase 3: Angular ClientApp

**package.json dependencies (conditional):**
| Package | Condition |
|---------|-----------|
| @angular/core, @angular/common, etc. | Always |
| @angular/platform-server | ServerSideRendering != None |
| @angular/service-worker | EnablePwa |
| @mintplayer/ng-bootstrap | UiFramework == MintPlayerNgBootstrap |
| @ng-bootstrap/ng-bootstrap | UiFramework == NgBootstrap |
| @ngx-translate/core | Translations == NgxTranslate |
| @ngx-translate/http-loader | Translations == NgxTranslate |
| bootstrap | UiFramework != None |
| qrcode | EnableTwoFactor |

**Angular module structure:**
```typescript
// app.module.ts
@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
//#if (UiFramework == "MintPlayerNgBootstrap")
    BsModule,
//#elif (UiFramework == "NgBootstrap")
    NgbModule,
//#endif
//#if (Translations == "NgxTranslate")
    TranslateModule.forRoot({...}),
//#endif
//#if (EnablePwa)
    ServiceWorkerModule.register('ngsw-worker.js'),
//#endif
  ],
})
export class AppModule { }
```

### Phase 4: Authentication Features

**When IdentityProvider != None:**

1. **User Entity** (Data/Entities/User.cs)
   - Extends IdentityUser
   - Additional profile fields
   - Concurrency token if EnableConcurrencyHandling

2. **Account Service** (Data/Services/AccountService.cs)
   - Register, Login, Logout
   - Password management
   - Email confirmation (if enabled)
   - 2FA setup/verify (if enabled)

3. **Account Controller** (Web/Server/Controllers/Api/V1/AccountController.cs)
   - REST API endpoints
   - JWT token generation

4. **Angular Account Pages:**
   - Login page with form
   - Register page with validation
   - Profile management
   - 2FA setup with QR code (if enabled)

### Phase 5: External Logins

**When EnableExternalLogins == true:**

Startup configuration for each provider:
```csharp
#if (ExternalLoginMicrosoft)
.AddMicrosoftAccount(options => {
    options.ClientId = Configuration["Authentication:Microsoft:ClientId"];
    options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
})
#endif
#if (ExternalLoginGoogle)
.AddGoogle(options => {
    options.ClientId = Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
})
#endif
// ... similar for Facebook, X, LinkedIn
```

**appsettings.json sections:**
```json
{
  "Authentication": {
//#if (ExternalLoginMicrosoft)
    "Microsoft": {
      "ClientId": "",
      "ClientSecret": ""
    },
//#endif
    // ... other providers
  }
}
```

### Phase 6: Server-Side Rendering

**When ServerSideRendering == OnSupplyData:**

1. **SpaPrerenderingService.cs** - Implements ISpaPrerenderingService
2. **main.server.ts** - Angular server entry point
3. **app.server.module.ts** - Server-side Angular module
4. **tsconfig.server.json** - TypeScript config for SSR build

**angular.json modifications:**
```json
{
  "projects": {
    "ClientApp": {
      "architect": {
        "server": {
          "builder": "@angular-devkit/build-angular:server",
          "options": {
            "outputPath": "dist/server",
            "main": "src/main.server.ts",
            "tsConfig": "tsconfig.server.json"
          }
        }
      }
    }
  }
}
```

### Phase 7: Additional Features

**XSRF Protection (EnableXsrf):**
- Configure antiforgery in Program.cs
- Add XSRF interceptor in Angular
- Cookie-based token exchange

**PWA Support (EnablePwa):**
- ngsw-config.json with caching strategies
- manifest.webmanifest
- Service worker registration

**HTML Minification (HtmlMinification == WebMarkupMin):**
- WebMarkupMin middleware configuration
- HTML, CSS, JS minification settings

**Translations (Translations == NgxTranslate):**
- TranslateModule configuration
- assets/i18n/en.json, nl.json, fr.json
- Language switcher component

**ElasticSearch (SearchProvider == ElasticSearch):**
- NEST client configuration
- SearchService with index/query methods
- SearchController API endpoints

**Docker (EnableDocker):**
- Multi-stage Dockerfile
- .dockerignore
- docker-compose.yml (optional)

**OpenSearch (EnableOpenSearch):**
- OpenSearch descriptor endpoint
- XML generation for browser search

**Concurrency Handling (EnableConcurrencyHandling):**
- RowVersion properties on entities
- Concurrency conflict handling in services

---

## File Conditionals Summary

The template.json `sources.modifiers` section will use these patterns:

```json
{
  "modifiers": [
    {
      "condition": "(IdentityProvider == 'None')",
      "exclude": [
        "Web/Server/Controllers/Api/V1/AccountController.cs",
        "Web/Server/Controllers/Web/AuthController.cs",
        "Web/ClientApp/src/app/pages/account/**",
        "Data/Entities/User.cs",
        "Data/Services/AccountService.cs",
        "Dtos/Dtos/User.cs",
        "Dtos/Dtos/LoginRequest.cs",
        "Dtos/Dtos/RegisterRequest.cs"
      ]
    },
    {
      "condition": "(!EnableTwoFactor)",
      "exclude": [
        "Web/ClientApp/src/app/pages/account/two-factor/**"
      ]
    },
    {
      "condition": "(ServerSideRendering == 'None')",
      "exclude": [
        "Web/Services/SpaPrerenderingService.cs",
        "Web/ClientApp/src/main.server.ts",
        "Web/ClientApp/src/app/app.server.module.ts",
        "Web/ClientApp/tsconfig.server.json"
      ]
    },
    {
      "condition": "(!EnablePwa)",
      "exclude": [
        "Web/ClientApp/ngsw-config.json",
        "Web/ClientApp/src/manifest.webmanifest"
      ]
    },
    {
      "condition": "(Translations == 'None')",
      "exclude": [
        "Web/ClientApp/src/assets/i18n/**"
      ]
    },
    {
      "condition": "(SearchProvider == 'None')",
      "exclude": [
        "Web/Server/Controllers/Api/V1/SearchController.cs",
        "Web/Services/SearchService.cs",
        "Data/Services/SearchService.cs"
      ]
    },
    {
      "condition": "(!EnableDocker)",
      "exclude": [
        "Web/Dockerfile",
        "Web/.dockerignore"
      ]
    },
    {
      "condition": "(!IncludeTests)",
      "exclude": [
        "Tests/**"
      ]
    }
  ]
}
```

---

## Estimated File Count

| Category | Approximate Files |
|----------|-------------------|
| Template config | 1 |
| Web project (.cs files) | 25-30 |
| Data project (.cs files) | 15-20 |
| Dtos project (.cs files) | 10-15 |
| Tests project (.cs files) | 10-15 |
| Angular components | 40-50 |
| Angular services/guards | 10-15 |
| Configuration files | 15-20 |
| **Total** | **~130-165 files** |

---

## Implementation Order

1. **template.json** - All symbols and source modifiers
2. **Project files** - .csproj files with conditional references
3. **Program.cs** - Main entry with all conditional middleware
4. **Data layer** - DbContext, entities, services
5. **Dtos** - Data transfer objects
6. **Controllers** - API and web controllers
7. **Angular base** - Core app structure
8. **Angular pages** - Home, account pages
9. **Angular services** - API services, guards
10. **Feature files** - SSR, PWA, translations, etc.
11. **Docker files** - Dockerfile, compose
12. **Documentation** - README with usage instructions

---

## Testing Strategy

After implementation, test these scenarios:
1. `dotnet new mintplayer-web` - Minimal template (SQL Server default)
2. `dotnet new mintplayer-web --DatabaseProvider PostgreSQL` - PostgreSQL database
3. `dotnet new mintplayer-web --DatabaseProvider SQLite` - SQLite database
4. `dotnet new mintplayer-web --IdentityProvider IdentityServer --EnableTwoFactor` - Full auth
5. `dotnet new mintplayer-web --UiFramework MintPlayerNgBootstrap --Translations NgxTranslate` - UI + i18n
6. `dotnet new mintplayer-web --EnableDocker --SearchProvider ElasticSearch` - Infrastructure
7. `dotnet new mintplayer-web --IncludeTests` - With unit test scaffolding
8. Full options enabled - Complete feature set

---

## Dependencies & Versions

Target: **.NET 10.0** and **Angular 21**

Key NuGet packages:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 10.0.x
- Microsoft.EntityFrameworkCore.SqlServer 10.0.x
- Microsoft.EntityFrameworkCore.Sqlite 10.0.x (optional)
- Npgsql.EntityFrameworkCore.PostgreSQL 10.x (optional)
- MintPlayer.AspNetCore.SpaServices.* 10.x
- WebMarkupMin.AspNetCore10 2.x
- NEST 7.x (ElasticSearch)

Key npm packages:
- @angular/* 21.x
- @mintplayer/ng-bootstrap (latest)
- @ng-bootstrap/ng-bootstrap (latest compatible)
- @ngx-translate/core (latest)
- bootstrap 5.x

---

## Decisions Made

| Question | Decision |
|----------|----------|
| Multiple database providers? | **Yes** - Support SQL Server, PostgreSQL, and SQLite |
| Separate IdentityProvider-only template? | **No** - Single unified template |
| Angular version? | **Angular 21** |
| Unit test scaffolding? | **Optional** - Add checkbox to template.json |

---

## Next Steps

Upon approval, implementation will proceed in the order specified above, starting with the template.json configuration and base project structure.
