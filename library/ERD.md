# Library Database ERD

```mermaid
erDiagram
    BOOKS ||--o{ LOANS : "BookId foreign key"
    STUDENTS ||--o{ LOANS : "MemberId foreign key"
    PENALTY_RATES ||..o{ BOOKS : "BookType application lookup"
    LOAN_REFERENCES ||..o{ LOANS : "LoanType application lookup"

    BOOKS {
        int BookId PK
        string Isbn
        string AccessionNumber
        string BookType
        string Title
        string Author
        string Category
        int TotalCopies
        int AvailableCopies
    }

    STUDENTS {
        int MemberId PK
        string StudentId UK
        string FullName
        string Course
        string YearLevel
        string Status
    }

    LOANS {
        int LoanId PK
        int BookId FK
        int MemberId FK
        string LoanType
        string AccessionNumber
        date BorrowedDate
        date DueDate
        date ReturnedDate
        int PenaltyDays
        decimal PenaltyAmount
    }

    LOAN_REFERENCES {
        string LoanType PK
        int DaysAllowed
    }

    PENALTY_RATES {
        string BookType PK
        decimal RatePerDay
    }
```

`Loans.BookId` and `Loans.MemberId` are enforced foreign keys. `Books.BookType` and `Loans.LoanType` are application-level lookups; MySQL does not enforce those relationships with foreign keys.
