# KierCRUD API Instructions

These instructions apply to the ASP.NET Core API in `backend/KierSimpleCrud.API`.

- Keep API changes inside `backend/KierSimpleCrud.API` unless a contract change requires a coordinated mobile-client update.
- Use ASP.NET Core controllers and keep HTTP actions RESTful and predictable.
- Use Entity Framework Core through `ApplicationDbContext`; do not add a second database access pattern without a clear reason.
- Preserve the existing SQLite default: `Data Source=kiercrud.db`. Read connection settings from configuration or `DB_CONNECTION_STRING` when database behavior needs to be configurable.
- Keep database initialization and seed data compatible with the existing startup path. Do not delete existing data during startup.
- Preserve the existing API route names and identifier casing unless the change is intentionally coordinated with clients.
- Return appropriate HTTP results: `200` for successful reads and updates, `201` for creates, `204` for successful deletes when no body is needed, `404` for missing resources, and `400` for invalid input or business-rule violations.
- Validate related entity identifiers before creating or updating enrollments, and preserve foreign-key and uniqueness rules.
- Keep CORS configurable through `AllowedCorsOrigins` or `ALLOWED_CORS_ORIGINS`; do not hard-code permissive origins for production behavior.
- Keep Swagger and the health endpoint working when changing startup or middleware configuration.
- Use nullable reference types and clear model/property names. Avoid silent exception swallowing.
- When changing an endpoint, update the README endpoint list and any affected mobile service models or calls.
- Before finishing API work, run `dotnet build backend/KierSimpleCrud.API/KierSimpleCrud.API.csproj` and, when relevant, verify `/api/health` and the changed endpoint through Swagger or an HTTP request.

The VS Code-specific instruction file remains at `.github/instructions/kiercrud-api.instructions.md` so these rules are automatically applied to API files.
