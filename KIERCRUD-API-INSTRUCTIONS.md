# KierCRUD API Instructions

These instructions apply to the ASP.NET Core API in `backend/KierSimpleCrud.API`.

## How to Add This API to Another App

The other developer does not need to clone this repository. They can click the API package location in GitHub:

- [Open the API ZIP file](https://github.com/KierDevil/kierCRUD/blob/main/publish/KierCRUD-API.zip) and click **Download raw file**.
- [Open the publish folder](https://github.com/KierDevil/kierCRUD/tree/main/publish) to find the API package and other published files.

After downloading, they should extract `KierCRUD-API.zip` into a folder.

Start the API from the extracted folder:

```powershell
.\KierSimpleCrud.API.exe --urls http://localhost:5000
```

Set the other app's API base URL to:

```text
http://localhost:5000
```

The API documentation is available at `http://localhost:5000/swagger`. The health check is `GET /api/health`.

Example request from a JavaScript or TypeScript app:

```javascript
const API_BASE_URL = "http://localhost:5000";

const response = await fetch(`${API_BASE_URL}/api/students`);
const students = await response.json();
```

Example create-student request:

```javascript
await fetch(`${API_BASE_URL}/api/students`, {
	method: "POST",
	headers: { "Content-Type": "application/json" },
	body: JSON.stringify({
		studid: "S001",
		studentName: "Maria Santos",
		status: "Active"
	})
});
```

Available API groups are `/api/students`, `/api/enrollments`, `/api/courses`, `/api/schoolyears`, `/api/semesters`, and `/api/health`.

If the other app runs on the same computer, `localhost` is correct. If it runs on another computer, start the API with `--urls http://0.0.0.0:5000`, use the API computer's local IP address as the base URL, allow TCP port `5000` through Windows Firewall, and set `ALLOWED_CORS_ORIGINS` to the other app's URL.

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
