# Probetest WPF 2026/27 - Pixel Gallery

Practice test for POSE (WPF / .NET 8): a pixel-art gallery with import, WPF management UI and a small WebAPI.

| Folder | Content |
|---|---|
| [`Angabe/PixelGallery.pdf`](Angabe/PixelGallery.pdf) | Assignment (German) with ERD and screenshots (`PixelGallery.html` is the source) |
| [`Angabe/walkthrough.pdf`](Angabe/walkthrough.pdf) | Step-by-step solution guide (German): what to do, where, why, and which section/page of `WPF-Cheatsheet-de.pdf` has the matching snippet |
| [`Template/PixelGallery`](Template/PixelGallery) | Starting point for the test - all open tasks are marked with `// TODO (Task n)` |
| [`Solution/PixelGallery`](Solution/PixelGallery) | Sample solution |

## Topics

- App structure with `Core` / `Persistence` (UnitOfWork, GenericRepository)
- Entities + Fluent API mappings from an ERD (incl. m:n)
- Text-file parser + unit tests (MSTest, one test has to be written)
- WPF MVVM, `ItemsControl` with `DataTemplate`, custom draw (`OnRender`)
- WebAPI: one endpoint (`GET /Pictures/{id}`)

## Run

Requirements: .NET 8 SDK, SQL Server on `localhost` (Windows authentication) - see `appsettings.json` of each app.

1. `ImportConsoleApp` - recreates the database `PixelGallery` (EnsureCreated, no migrations) and imports palette + pictures
2. `Wpf` - WPF app (start from Visual Studio / Rider, so the working directory is the `bin` folder)
3. `WebApi` - http://localhost:5123/swagger
4. Tests: `dotnet test Core.Tests`

In the template, 7 of the 8 parser tests fail until `PixelParser.Parse` is implemented; the 8th
(`Parse_PrefixIsResetAfterCommand`) is `Inconclusive` until it is written.
