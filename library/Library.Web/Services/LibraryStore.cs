using MySqlConnector;
using Library.Web.Models;
namespace Library.Web.Services;
public sealed class LibraryStore
{
 private readonly string _cs;
 private static readonly (string Title,string Author,string Category)[] StarterBooks =
 [
  ("Introduction to Algorithms", "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, and Clifford Stein", "Algorithms"),
  ("Algorithms", "Robert Sedgewick and Kevin Wayne", "Algorithms"),
  ("The Algorithm Design Manual", "Steven S. Skiena", "Algorithms"),
  ("A Common-Sense Guide to Data Structures and Algorithms", "Jay Wengrow", "Algorithms"),
  ("Data Structures and Algorithms in Java", "Michael T. Goodrich, Roberto Tamassia, and Michael H. Goldwasser", "Algorithms"),
  ("Clean Code", "Robert C. Martin", "Software Engineering"),
  ("The Pragmatic Programmer", "David Thomas and Andrew Hunt", "Software Engineering"),
  ("Code Complete", "Steve McConnell", "Software Engineering"),
  ("Refactoring", "Martin Fowler", "Software Engineering"),
  ("Design Patterns", "Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides", "Software Engineering"),
  ("Head First Design Patterns", "Eric Freeman and Elisabeth Robson", "Software Engineering"),
  ("Working Effectively with Legacy Code", "Michael Feathers", "Software Engineering"),
  ("Domain-Driven Design", "Eric Evans", "Software Engineering"),
  ("Patterns of Enterprise Application Architecture", "Martin Fowler", "Software Engineering"),
  ("Software Engineering", "Ian Sommerville", "Software Engineering"),
  ("Software Engineering: A Practitioner's Approach", "Roger S. Pressman and Bruce R. Maxim", "Software Engineering"),
  ("Computer Networking: A Top-Down Approach", "James F. Kurose and Keith W. Ross", "Networking"),
  ("Computer Networks", "Andrew S. Tanenbaum and David J. Wetherall", "Networking"),
  ("TCP/IP Illustrated, Volume 1", "W. Richard Stevens and Kevin R. Fall", "Networking"),
  ("Operating System Concepts", "Abraham Silberschatz, Peter B. Galvin, and Greg Gagne", "Operating Systems"),
  ("Modern Operating Systems", "Andrew S. Tanenbaum and Herbert Bos", "Operating Systems"),
  ("Operating Systems: Three Easy Pieces", "Remzi H. Arpaci-Dusseau and Andrea C. Arpaci-Dusseau", "Operating Systems"),
  ("Computer Organization and Design", "David A. Patterson and John L. Hennessy", "Computer Architecture"),
  ("Computer Architecture: A Quantitative Approach", "John L. Hennessy and David A. Patterson", "Computer Architecture"),
  ("Database System Concepts", "Abraham Silberschatz, Henry F. Korth, and S. Sudarshan", "Databases"),
  ("Fundamentals of Database Systems", "Ramez Elmasri and Shamkant B. Navathe", "Databases"),
  ("Database Systems: The Complete Book", "Hector Garcia-Molina, Jeffrey D. Ullman, and Jennifer Widom", "Databases"),
  ("Learning SQL", "Alan Beaulieu", "Databases"),
  ("SQL in 10 Minutes, Sams Teach Yourself", "Ben Forta", "Databases"),
  ("Artificial Intelligence: A Modern Approach", "Stuart Russell and Peter Norvig", "Artificial Intelligence"),
  ("Deep Learning", "Ian Goodfellow, Yoshua Bengio, and Aaron Courville", "Artificial Intelligence"),
  ("Pattern Recognition and Machine Learning", "Christopher M. Bishop", "Artificial Intelligence"),
  ("Hands-On Machine Learning with Scikit-Learn, Keras, and TensorFlow", "Aurelien Geron", "Artificial Intelligence"),
  ("Python Crash Course", "Eric Matthes", "Programming"),
  ("Automate the Boring Stuff with Python", "Al Sweigart", "Programming"),
  ("Fluent Python", "Luciano Ramalho", "Programming"),
  ("Effective Python", "Brett Slatkin", "Programming"),
  ("The C Programming Language", "Brian W. Kernighan and Dennis M. Ritchie", "Programming"),
  ("The C++ Programming Language", "Bjarne Stroustrup", "Programming"),
  ("Effective Modern C++", "Scott Meyers", "Programming"),
  ("Programming Language Pragmatics", "Michael L. Scott", "Programming Languages"),
  ("Compilers: Principles, Techniques, and Tools", "Alfred V. Aho, Monica S. Lam, Ravi Sethi, and Jeffrey D. Ullman", "Programming Languages"),
  ("Structure and Interpretation of Computer Programs", "Harold Abelson and Gerald Jay Sussman", "Programming Languages"),
  ("Computer Security: Principles and Practice", "William Stallings and Lawrie Brown", "Cybersecurity"),
  ("Security Engineering", "Ross Anderson", "Cybersecurity"),
  ("Web Development with Node and Express", "Ethan Brown", "Web Development"),
  ("Learning Web Design", "Jennifer Niederst Robbins", "Web Development"),
  ("JavaScript: The Definitive Guide", "David Flanagan", "Web Development"),
  ("Eloquent JavaScript", "Marijn Haverbeke", "Web Development"),
  ("You Don't Know JS Yet: Get Started", "Kyle Simpson", "Web Development"),
  ("HTML and CSS: Design and Build Websites", "Jon Duckett", "Web Development")
 ];
 public LibraryStore(IConfiguration config){var cs=config.GetConnectionString("DefaultConnection")??"Server=localhost;Port=3307;Database=library;User=root;Password=higanbana;";var password=Environment.GetEnvironmentVariable("DB_PASSWORD");if(!string.IsNullOrWhiteSpace(password))cs=new MySqlConnectionStringBuilder(cs){Password=password}.ConnectionString;_cs=cs;EnsureSchema();SeedAdditionalLoanTypes();SeedInitialStudents();SeedStarterBooks();}
 private MySqlConnection Open(){var c=new MySqlConnection(_cs);c.Open();return c;}
 private void EnsureSchema(){var b=new MySqlConnectionStringBuilder(_cs);var db=b.Database;b.Database="";using(var s=new MySqlConnection(b.ConnectionString)){s.Open();using var q=s.CreateCommand();q.CommandText=$"CREATE DATABASE IF NOT EXISTS `{db}`";q.ExecuteNonQuery();}using var c=Open();using var q2=c.CreateCommand();q2.CommandText=@"CREATE TABLE IF NOT EXISTS Books(BookId INT AUTO_INCREMENT PRIMARY KEY,Isbn VARCHAR(60),AccessionNumber VARCHAR(80),BookType VARCHAR(80) NOT NULL DEFAULT 'General',Title VARCHAR(200) NOT NULL,Author VARCHAR(160),Category VARCHAR(100),TotalCopies INT NOT NULL,AvailableCopies INT NOT NULL); CREATE TABLE IF NOT EXISTS Students(MemberId INT AUTO_INCREMENT PRIMARY KEY,StudentId VARCHAR(60) NOT NULL UNIQUE,FullName VARCHAR(180) NOT NULL,Course VARCHAR(160),Status VARCHAR(30) NOT NULL DEFAULT 'Active'); CREATE TABLE IF NOT EXISTS Loans(LoanId INT AUTO_INCREMENT PRIMARY KEY,BookId INT NOT NULL,MemberId INT NOT NULL,LoanType VARCHAR(60) NOT NULL DEFAULT 'Regular',AccessionNumber VARCHAR(80),BorrowedDate DATE NOT NULL,DueDate DATE NOT NULL,ReturnedDate DATE NULL,PenaltyDays INT NOT NULL DEFAULT 0,PenaltyAmount DECIMAL(10,2) NOT NULL DEFAULT 0,FOREIGN KEY(BookId) REFERENCES Books(BookId),FOREIGN KEY(MemberId) REFERENCES Students(MemberId)); CREATE TABLE IF NOT EXISTS PenaltyRates(BookType VARCHAR(80) PRIMARY KEY,RatePerDay DECIMAL(10,2) NOT NULL DEFAULT 10); CREATE TABLE IF NOT EXISTS LoanReferences(LoanType VARCHAR(60) PRIMARY KEY,DaysAllowed INT NOT NULL); INSERT IGNORE INTO PenaltyRates(BookType,RatePerDay) VALUES('General',10); INSERT IGNORE INTO LoanReferences(LoanType,DaysAllowed) VALUES('Regular',14);";q2.ExecuteNonQuery();AddColumnIfMissing(c,"Books","AccessionNumber VARCHAR(80)");AddColumnIfMissing(c,"Books","BookType VARCHAR(80) NOT NULL DEFAULT 'General'");AddColumnIfMissing(c,"Loans","LoanType VARCHAR(60) NOT NULL DEFAULT 'Regular'");AddColumnIfMissing(c,"Loans","AccessionNumber VARCHAR(80)");AddColumnIfMissing(c,"Loans","PenaltyDays INT NOT NULL DEFAULT 0");AddColumnIfMissing(c,"Loans","PenaltyAmount DECIMAL(10,2) NOT NULL DEFAULT 0");}
 private void SeedAdditionalLoanTypes(){using var c=Open();using var q=c.CreateCommand();q.CommandText="INSERT IGNORE INTO LoanReferences(LoanType,DaysAllowed) VALUES('Overnight',1),('Short-term',7),('Extended',30)";q.ExecuteNonQuery();}
 private void SeedInitialStudents(){using var c=Open();AddColumnIfMissing(c,"Students","YearLevel VARCHAR(30) NOT NULL DEFAULT ''");using var q=c.CreateCommand();q.CommandText="INSERT IGNORE INTO Students(StudentId,FullName,Course,YearLevel,Status) VALUES('LIB-BSCS4-001','Denver Amoson','BSCS','4th Year','Active'),('LIB-BSCS4-002','Kobe Salida','BSCS','4th Year','Active'),('LIB-BSCS4-003','Kier Lanayon','BSCS','4th Year','Active')";q.ExecuteNonQuery();}
 private void SeedStarterBooks(){using var c=Open();using var tx=c.BeginTransaction();for(var index=0;index<StarterBooks.Length;index++){var book=StarterBooks[index];using var q=c.CreateCommand();q.Transaction=tx;q.CommandText="INSERT INTO Books(Isbn,AccessionNumber,BookType,Title,Author,Category,TotalCopies,AvailableCopies) SELECT '',@accession,'Textbook',@title,@author,@category,1,1 FROM DUAL WHERE NOT EXISTS(SELECT 1 FROM Books WHERE AccessionNumber=@accession)";q.Parameters.AddWithValue("@accession",$"STARTER-{index+1:000}");q.Parameters.AddWithValue("@title",book.Title);q.Parameters.AddWithValue("@author",book.Author);q.Parameters.AddWithValue("@category",book.Category);q.ExecuteNonQuery();}tx.Commit();}
 private static void AddColumnIfMissing(MySqlConnection c,string table,string definition){var column=definition.Split(' ',2)[0];using var check=c.CreateCommand();check.CommandText="SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=@table AND COLUMN_NAME=@column";check.Parameters.AddWithValue("@table",table);check.Parameters.AddWithValue("@column",column);if(Convert.ToInt32(check.ExecuteScalar())>0)return;using var alter=c.CreateCommand();alter.CommandText=$"ALTER TABLE `{table}` ADD COLUMN {definition}";alter.ExecuteNonQuery();}
 public IReadOnlyList<Book> Books{get{using var c=Open();using var q=c.CreateCommand();q.CommandText="SELECT BookId,Isbn,AccessionNumber,BookType,Title,Author,Category,TotalCopies,AvailableCopies FROM Books ORDER BY Title";using var r=q.ExecuteReader();var x=new List<Book>();while(r.Read())x.Add(new Book{BookId=r.GetInt32(0),Isbn=r.IsDBNull(1)?"":r.GetString(1),AccessionNumber=r.IsDBNull(2)?"":r.GetString(2),BookType=r.IsDBNull(3)?"General":r.GetString(3),Title=r.GetString(4),Author=r.IsDBNull(5)?"":r.GetString(5),Category=r.IsDBNull(6)?"":r.GetString(6),TotalCopies=r.GetInt32(7),AvailableCopies=r.GetInt32(8)});return x;}}
 public IReadOnlyList<Member> Members{get{using var c=Open();using var q=c.CreateCommand();q.CommandText="SELECT MemberId,StudentId,FullName,Course,YearLevel,Status FROM Students ORDER BY FullName";using var r=q.ExecuteReader();var x=new List<Member>();while(r.Read())x.Add(new Member{MemberId=r.GetInt32(0),MemberNumber=r.GetString(1),FullName=r.GetString(2),Course=r.IsDBNull(3)?"":r.GetString(3),YearLevel=r.IsDBNull(4)?"":r.GetString(4),Status=r.GetString(5)});return x;}}
 public IReadOnlyList<LoanRow> Loans{get{using var c=Open();using var q=c.CreateCommand();q.CommandText="SELECT l.LoanId,l.BookId,l.MemberId,l.LoanType,l.AccessionNumber,l.BorrowedDate,l.DueDate,l.ReturnedDate,l.PenaltyDays,l.PenaltyAmount,b.Title,b.BookType,s.FullName FROM Loans l JOIN Books b ON b.BookId=l.BookId JOIN Students s ON s.MemberId=l.MemberId ORDER BY l.BorrowedDate DESC,l.LoanId DESC";using var r=q.ExecuteReader();var x=new List<LoanRow>();while(r.Read())x.Add(new LoanRow{LoanId=r.GetInt32(0),BookId=r.GetInt32(1),MemberId=r.GetInt32(2),LoanType=r.GetString(3),AccessionNumber=r.IsDBNull(4)?"":r.GetString(4),BorrowedDate=r.GetDateTime(5),DueDate=r.GetDateTime(6),ReturnedDate=r.IsDBNull(7)?null:r.GetDateTime(7),PenaltyDays=r.GetInt32(8),PenaltyAmount=r.GetDecimal(9),BookTitle=r.GetString(10),BookType=r.GetString(11),MemberName=r.GetString(12)});return x;}}
 public IReadOnlyList<LoanReference> LoanReferences{get{using var c=Open();using var q=c.CreateCommand();q.CommandText="SELECT LoanType,DaysAllowed FROM LoanReferences ORDER BY LoanType";using var r=q.ExecuteReader();var x=new List<LoanReference>();while(r.Read())x.Add(new LoanReference{LoanType=r.GetString(0),DaysAllowed=r.GetInt32(1)});return x;}}
 public void AddBook(Book x){using var c=Open();using var q=c.CreateCommand();q.CommandText="INSERT INTO Books(Isbn,AccessionNumber,BookType,Title,Author,Category,TotalCopies,AvailableCopies) VALUES(@isbn,@accession,@type,@title,@author,@category,@total,@total)";q.Parameters.AddWithValue("@isbn",x.Isbn);q.Parameters.AddWithValue("@accession",x.AccessionNumber);q.Parameters.AddWithValue("@type",x.BookType);q.Parameters.AddWithValue("@title",x.Title.Trim());q.Parameters.AddWithValue("@author",x.Author);q.Parameters.AddWithValue("@category",x.Category);q.Parameters.AddWithValue("@total",x.TotalCopies);q.ExecuteNonQuery();}
 public void AddMember(Member x){using var c=Open();using var q=c.CreateCommand();q.CommandText="INSERT INTO Students(StudentId,FullName,Course,YearLevel,Status) VALUES(@id,@name,@course,@year,@status)";q.Parameters.AddWithValue("@id",x.MemberNumber.Trim());q.Parameters.AddWithValue("@name",x.FullName.Trim());q.Parameters.AddWithValue("@course",x.Course);q.Parameters.AddWithValue("@year",x.YearLevel);q.Parameters.AddWithValue("@status",x.Status);q.ExecuteNonQuery();}
 public void Borrow(int bookId,int memberId,DateTime due,string loanType){using var c=Open();using var tx=c.BeginTransaction();using var q=c.CreateCommand();q.Transaction=tx;q.CommandText="SELECT AvailableCopies,AccessionNumber FROM Books WHERE BookId=@book FOR UPDATE";q.Parameters.AddWithValue("@book",bookId);using var r=q.ExecuteReader();if(!r.Read())throw new InvalidOperationException("Select a book.");if(r.GetInt32(0)<1)throw new InvalidOperationException("This book is currently unavailable.");var accession=r.IsDBNull(1)?"":r.GetString(1);r.Close();using var add=c.CreateCommand();add.Transaction=tx;add.CommandText="INSERT INTO Loans(BookId,MemberId,LoanType,AccessionNumber,BorrowedDate,DueDate) VALUES(@book,@member,@type,@accession,CURDATE(),@due);UPDATE Books SET AvailableCopies=AvailableCopies-1 WHERE BookId=@book";add.Parameters.AddWithValue("@book",bookId);add.Parameters.AddWithValue("@member",memberId);add.Parameters.AddWithValue("@type",loanType);add.Parameters.AddWithValue("@accession",accession);add.Parameters.AddWithValue("@due",due.Date);add.ExecuteNonQuery();tx.Commit();}
 public void Return(int loanId){using var c=Open();using var tx=c.BeginTransaction();using var q=c.CreateCommand();q.Transaction=tx;q.CommandText="SELECT l.DueDate,p.RatePerDay FROM Loans l JOIN Books b ON b.BookId=l.BookId LEFT JOIN PenaltyRates p ON p.BookType=b.BookType WHERE l.LoanId=@id AND l.ReturnedDate IS NULL FOR UPDATE";q.Parameters.AddWithValue("@id",loanId);using var r=q.ExecuteReader();if(!r.Read())throw new InvalidOperationException("This transaction is already returned.");var due=r.GetDateTime(0);var rate=r.IsDBNull(1)?10m:r.GetDecimal(1);var days=Math.Max(0,(DateTime.Today-due.Date).Days);var amount=days*rate;r.Close();using var update=c.CreateCommand();update.Transaction=tx;update.CommandText="UPDATE Loans SET ReturnedDate=CURDATE(),PenaltyDays=@days,PenaltyAmount=@amount WHERE LoanId=@id";update.Parameters.AddWithValue("@days",days);update.Parameters.AddWithValue("@amount",amount);update.Parameters.AddWithValue("@id",loanId);update.ExecuteNonQuery();using var q2=c.CreateCommand();q2.Transaction=tx;q2.CommandText="UPDATE Books b JOIN Loans l ON l.BookId=b.BookId SET b.AvailableCopies=b.AvailableCopies+1 WHERE l.LoanId=@id";q2.Parameters.AddWithValue("@id",loanId);q2.ExecuteNonQuery();tx.Commit();}
}
