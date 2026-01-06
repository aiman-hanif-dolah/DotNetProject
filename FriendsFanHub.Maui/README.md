# FriendsFanHub.Maui (mobile mirror)
.NET MAUI app that mirrors the ASP.NET MVC site in `DotNetProject.sln` (characters, episodes, quotes, locations) including the same seed data, media links, and CRUD flows.

## Data parity
- Uses Firestore as the runtime source for read/write, with MVC syncing SQL Server ↔ Firestore.
- Image paths stay in MVC format (`/img/friends/...` or `/img/uploads/...`) and resolve against the MVC host (`FriendsFanHub.Maui/Helpers/AppConfig.cs`, default `http://10.0.2.2:5263` for Android emulators) with a bundled-image fallback.
- Set `AppConfig.FirestoreApiKey` to your Firebase Web API key (Firestore REST).
  - Default placeholder: `YOUR_FIREBASE_WEB_API_KEY`

## Run
- Windows: `dotnet build -t:Run -f net10.0-windows10.0.19041.0`
- Android emulator/device: `dotnet build -t:Run -f net10.0-android`

Notes:
- Android requires JDK 21. The csproj pins `<JavaSdkDirectory>` to `C:\Program Files\Java\jdk-21`; adjust if your path differs or set `JAVA_HOME`.
- For images/uploads, run the MVC site locally (e.g., `dotnet run --project DotNetProject.csproj --launch-profile http` on port 5263) or update `AppConfig.DefaultBackendBaseUrl` to the reachable host.
- Shell routes: `dashboard`, `characters`, `episodes`, `quotes`, `locations`. Forms: `CharacterFormPage`, `EpisodeFormPage`, `LocationFormPage`, `QuoteFormPage`.

## UI parity
- Dark glassmorphism palette and neon accents inspired by the MVC site.
- Each list item matches the web action set: view, edit, delete, and “watch clip” (opens the same YouTube search/links used on the site).
