# Library System

Standalone library application. It is separate from SES and SCSDS.

The database schema and relationships are documented in [ERD.md](ERD.md).

On first startup, the library seeds 51 CS/IT starter titles and three BSCS 4th Year students. Student IDs `LIB-BSCS4-001` through `LIB-BSCS4-003` are generated library member numbers and can be replaced with official student IDs.

Run from `C:\kierCRUD`:

```powershell
.\STARTWEBLIBRARY.cmd
```

Open http://localhost:5190. Data is stored in the MySQL database `library` on `localhost:3307`.
