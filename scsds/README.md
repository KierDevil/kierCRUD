# School Canteen Student Discount System (SCSDS)

Standalone application based on the uploaded ERD. It does not share the SES API or database.

Run from `c:\kierCRUD\scsds`:

```powershell
.\STARTWEBSCSDS.cmd
```

Open http://localhost:5185.

Discount rules are based on purchase subtotal and apply only to students with Active status:
- Below 100.00: 0%
- 100.00-199.99: 5%
- 200.00-499.99: 10%
- 500.00 and above: 15%

Data is stored in the MySQL database `scsds` on `localhost:3306`.
