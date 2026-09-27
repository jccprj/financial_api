# Secrets / Local configuration

This project uses a local configuration file for sensitive values that should not be committed to Git.

- Committed example: `appsettings.Example.json`
- Local secrets file: `appsettings.Local.json` (must be created per-environment and will override values in `appsettings.json`)

Recommended steps for developers / deployment:

1. Copy `FinancialAPI/appsettings.Example.json` to `FinancialAPI/appsettings.Local.json`.
2. Fill in your real connection string and JWT key.
3. Do NOT commit `appsettings.Local.json`.

Example `appsettings.Local.json`:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "server=YOUR_DB;port=3306;database=DB_NAME;user=USER;password=PASSWORD;"
  },
  "Jwt": {
	"Key": "your_real_jwt_secret",
	"Issuer": "financial_api"
  }
}
```

At runtime the application will load `appsettings.json` and then override values with `appsettings.Local.json` when present.
