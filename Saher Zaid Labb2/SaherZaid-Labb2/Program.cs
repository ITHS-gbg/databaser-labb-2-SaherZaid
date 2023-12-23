// See https://aka.ms/new-console-template for more information
using DataAccess.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using DataAccess;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new BookStoresContext())
        {
            Console.WriteLine("Welcome to the Bookstore Inventory Management System!");

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. List inventory balances for all stores");
                Console.WriteLine("2. Add a book to a store");
                Console.WriteLine("3. Remove a book from a store");
                Console.WriteLine("4. Update a book in a store");
                Console.WriteLine("5. Exit");

                var option = Console.ReadLine();
                Console.WriteLine();

                if (option == "1")
                {
                    ListInventoryBalances(context);
                }
                else if (option == "2")
                {
                    AddBookToStore(context);
                }
                else if (option == "3")
                {
                    RemoveBookFromStore(context);
                }
                else if (option == "4")

                {
                    UpdateBook(context);
                }
                else if (option == "5")
                {
                    Console.WriteLine("Thank you for using the Bookstore Inventory Management System. Goodbye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }

                Console.WriteLine();
            }
        }
    }



    static void ListInventoryBalances(BookStoresContext context)
    {
        var inventoryBalances = (
            from ib in context.InventoryBalances
            join b in context.Books on ib.Isbn13 equals b.Isbn13
            select new
            {
                StoreId = ib.StoreId,
                Isbn13 = ib.Isbn13,
                NoOfProducts = ib.NoOfProducts,
                Title = b.Title
            }).ToList();

        if (inventoryBalances.Count == 0)
        {
            Console.WriteLine("No inventory balances found.");
            return;
        }


        Console.WriteLine("Inventory Balances:");
        foreach (var inventoryBalance in inventoryBalances)
        {
            Console.WriteLine($"Store ID: {inventoryBalance.StoreId}");
            Console.WriteLine($"ISBN-13: {inventoryBalance.Isbn13}");
            Console.WriteLine($"No. of Products: {inventoryBalance.NoOfProducts}");
            Console.WriteLine($"Title: {inventoryBalance.Title}");
            Console.WriteLine();
        }
    }



    static void AddBookToStore(BookStoresContext context)
    {
        Console.WriteLine("Enter the store ID:");
        var storeIdInput = Console.ReadLine();
        if (!int.TryParse(storeIdInput, out int storeId))
        {
            Console.WriteLine("Invalid store ID.");
            return;
        }

        Console.WriteLine("Choose from existing books:");
        var books = context.Books.ToList();
        if (books.Count == 0)
        {
            Console.WriteLine("No books found in the assortment.");
        }
        else
        {
            for (int i = 0; i < books.Count; i++)
            {
                var book = books[i];
                Console.WriteLine($"{i + 1}. {book.Title} (ISBN-13: {book.Isbn13})");
            }
        }

        Console.WriteLine("Enter the number corresponding to the book you want to add (0 to add a new book):");
        var bookChoiceInput = Console.ReadLine();
        if (!int.TryParse(bookChoiceInput, out int bookChoice))
        {
            Console.WriteLine("Invalid book choice.");
            return;
        }

        Book selectedBook;
        if (bookChoice == 0)
        {
            Console.WriteLine("Enter the title of the new book:");
            var title = Console.ReadLine();
            Console.WriteLine("Enter the ISBN-13 of the new book:");
            var isbn13 = Console.ReadLine();
            Console.WriteLine("Enter the price of the new book:");
            var priceInput = Console.ReadLine();
            if (!int.TryParse(priceInput, out int price))
            {
                Console.WriteLine("Invalid price.");
                return;
            }
            Console.WriteLine("Enter the language of the new book:");
            var language = Console.ReadLine();
            Console.WriteLine("Enter the release date of the new book (yyyy-MM-dd):");
            var releaseDateInput = Console.ReadLine();
            if (!DateOnly.TryParse(releaseDateInput, out DateOnly releaseDate))
            {
                Console.WriteLine("Invalid release date.");
                return;
            }

            selectedBook = new Book
            {
                Title = title,
                Isbn13 = isbn13,
                Price = price,
                Language = language,
                ReleaseDate = releaseDate
            };
            context.Books.Add(selectedBook);
        }
        else if (bookChoice < 1 || bookChoice > books.Count)
        {
            Console.WriteLine("Invalid book choice.");
            return;
        }
        else
        {
            selectedBook = books[bookChoice - 1];
        }

        var inventoryBalance = context.InventoryBalances.FirstOrDefault(ib => ib.StoreId == storeId && ib.Isbn13 == selectedBook.Isbn13);
        if (inventoryBalance != null)
        {
            inventoryBalance.NoOfProducts++;
        }
        else
        {
            inventoryBalance = new InventoryBalance
            {
                StoreId = storeId,
                Isbn13 = selectedBook.Isbn13,
                NoOfProducts = 1
            };
            context.InventoryBalances.Add(inventoryBalance);
        }

        context.SaveChanges();
        Console.WriteLine("Book added to the store successfully.");
    }

       static void RemoveBookFromStore(BookStoresContext context)
    {
        Console.WriteLine("Enter the store ID:");
        var storeIdInput = Console.ReadLine();
        if (!int.TryParse(storeIdInput, out int storeId))
        {
            Console.WriteLine("Invalid store ID.");
            return;
        }

        Console.WriteLine("Enter the ISBN-13 of the book to remove:");
        var isbn13 = Console.ReadLine();

        var inventoryBalance = context.InventoryBalances.FirstOrDefault(ib => ib.StoreId == storeId && ib.Isbn13 == isbn13);
        if (inventoryBalance == null)
        {
            Console.WriteLine("Book does not exist in the specified store.");
            return;
        }


        if (inventoryBalance.NoOfProducts > 1)
        {
            inventoryBalance.NoOfProducts--;
        }
        else
        {
            context.InventoryBalances.Remove(inventoryBalance);
        }

        context.SaveChanges();
        Console.WriteLine("Book removed from the store successfully.");
    }

    static void UpdateBook(BookStoresContext context)
    {
        Console.WriteLine("Enter the ISBN-13 of the book you want to update:");
        var isbn13 = Console.ReadLine();

        var book = context.Books.Find(isbn13);
        if (book == null)
        {
            Console.WriteLine("Book not found in the database.");
            return;
        }


        Console.WriteLine("Enter the new title of the book (leave empty to keep current title):");
        var newTitle = Console.ReadLine();
        if (!string.IsNullOrEmpty(newTitle))
        {
            book.Title = newTitle;
        }

        Console.WriteLine("Enter the new language of the book (leave empty to keep current language):");
        var newLanguage = Console.ReadLine();
        if (!string.IsNullOrEmpty(newLanguage))
        {
            book.Language = newLanguage;
        }

        Console.WriteLine("Enter the new price of the book (leave empty to keep current price):");
        var newPriceInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(newPriceInput))
        {
            if (!int.TryParse(newPriceInput, out int newPrice))
            {
                Console.WriteLine("Invalid price.");
                return;
            }
            book.Price = newPrice;
        }

        Console.WriteLine("Enter the new release date of the book (leave empty to keep current release date):");
        var newReleaseDateInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(newReleaseDateInput))
        {
            if (!DateOnly.TryParse(newReleaseDateInput, out DateOnly newReleaseDate))
            {
                Console.WriteLine("Invalid release date.");
                return;
            }
            book.ReleaseDate = newReleaseDate;
        }

        context.SaveChanges();
        Console.WriteLine("Book updated successfully.");
    }
}



























//using DataAccess.Entities;
//using System;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using System.Globalization;
//using DataAccess;

//class Program
//{
//    static void Main(string[] args)
//    {
//        using (var context = new BookStoresContext())
//        {
//            Console.WriteLine("Welcome to the Bookstore Inventory Management System!");

//            while (true)
//            {
//                Console.WriteLine("Please select an option:");
//                Console.WriteLine("1. List inventory balances for all stores");
//                Console.WriteLine("2. Add a book to a store");
//                Console.WriteLine("3. Remove a book from a store");
//                Console.WriteLine("4. Update a book in a store");
//                Console.WriteLine("5. Exit");

//                var option = Console.ReadLine();
//                Console.WriteLine();

//                if (option == "1")
//                {
//                    ListInventoryBalances(context);
//                }
//                else if (option == "2")
//                {
//                    AddBookToStore(context);
//                }
//                else if (option == "3")
//                {
//                    RemoveBookFromStore(context);
//                }
//                else if (option == "4")

//                {
//                    UpdateBook(context);
//                }
//                else if (option == "5")
//                {
//                    Console.WriteLine("Thank you for using the Bookstore Inventory Management System. Goodbye!");
//                    break;
//                }
//                else
//                {
//                    Console.WriteLine("Invalid option. Please try again.");
//                }

//                Console.WriteLine();
//            }
//        }
//    }



//    static void ListInventoryBalances(BookStoresContext context)
//    {
//        var inventoryBalances = (
//            from ib in context.InventoryBalances
//            join b in context.Books on ib.Isbn13 equals b.Isbn13
//            select new
//            {
//                StoreId = ib.StoreId,
//                Isbn13 = ib.Isbn13,
//                NoOfProducts = ib.NoOfProducts,
//                Title = b.Title
//            }).ToList();

//        if (inventoryBalances.Count == 0)
//        {
//            Console.WriteLine("No inventory balances found.");
//            return;
//        }


//        Console.WriteLine("Inventory Balances:");
//        foreach (var inventoryBalance in inventoryBalances)
//        {
//            Console.WriteLine($"Store ID: {inventoryBalance.StoreId}");
//            Console.WriteLine($"ISBN-13: {inventoryBalance.Isbn13}");
//            Console.WriteLine($"No. of Products: {inventoryBalance.NoOfProducts}");
//            Console.WriteLine($"Title: {inventoryBalance.Title}");
//            Console.WriteLine();
//        }
//    }



//    static void AddBookToStore(BookStoresContext context)
//    {
//        Console.WriteLine("Enter the store ID:");
//        var storeIdInput = Console.ReadLine();
//        if (!int.TryParse(storeIdInput, out int storeId))
//        {
//            Console.WriteLine("Invalid store ID.");
//            return;
//        }

//        Console.WriteLine("Choose from existing books:");
//        var books = context.Books.ToList();
//        if (books.Count == 0)
//        {
//            Console.WriteLine("No books found in the assortment.");
//        }
//        else
//        {
//            for (int i = 0; i < books.Count; i++)
//            {
//                var book = books[i];
//                Console.WriteLine($"{i + 1}. {book.Title} (ISBN-13: {book.Isbn13})");
//            }
//        }

//        Console.WriteLine("Enter the number corresponding to the book you want to add (0 to add a new book):");
//        var bookChoiceInput = Console.ReadLine();
//        if (!int.TryParse(bookChoiceInput, out int bookChoice))
//        {
//            Console.WriteLine("Invalid book choice.");
//            return;
//        }

//        Book selectedBook;
//        if (bookChoice == 0)
//        {
//            Console.WriteLine("Enter the title of the new book:");
//            var title = Console.ReadLine();
//            Console.WriteLine("Enter the ISBN-13 of the new book:");
//            var isbn13 = Console.ReadLine();
//            Console.WriteLine("Enter the price of the new book:");
//            var priceInput = Console.ReadLine();
//            if (!int.TryParse(priceInput, out int price))
//            {
//                Console.WriteLine("Invalid price.");
//                return;
//            }
//            Console.WriteLine("Enter the language of the new book:");
//            var language = Console.ReadLine();
//            Console.WriteLine("Enter the release date of the new book (yyyy-MM-dd):");
//            var releaseDateInput = Console.ReadLine();
//            if (!DateOnly.TryParse(releaseDateInput, out DateOnly releaseDate))
//            {
//                Console.WriteLine("Invalid release date.");
//                return;
//            }

//            selectedBook = new Book
//            {
//                Title = title,
//                Isbn13 = isbn13,
//                Price = price,
//                Language = language,
//                ReleaseDate = releaseDate
//            };
//            context.Books.Add(selectedBook);
//        }
//        else if (bookChoice < 1 || bookChoice > books.Count)
//        {
//            Console.WriteLine("Invalid book choice.");
//            return;
//        }
//        else
//        {
//            selectedBook = books[bookChoice - 1];
//        }

//        var inventoryBalance = context.InventoryBalances.FirstOrDefault(ib => ib.StoreId == storeId && ib.Isbn13 == selectedBook.Isbn13);
//        if (inventoryBalance != null)
//        {
//            inventoryBalance.NoOfProducts++;
//        }
//        else
//        {
//            inventoryBalance = new InventoryBalance
//            {
//                StoreId = storeId,
//                Isbn13 = selectedBook.Isbn13,
//                NoOfProducts = 1
//            };
//            context.InventoryBalances.Add(inventoryBalance);
//        }

//        context.SaveChanges();
//        Console.WriteLine("Book added to the store successfully.");
//    }


//    static void RemoveBookFromStore(BookStoresContext context)
//    {
//        Console.WriteLine("Enter the store ID:");
//        var storeIdInput = Console.ReadLine();
//        if (!int.TryParse(storeIdInput, out int storeId))
//        {
//            Console.WriteLine("Invalid store ID.");
//            return;
//        }

//        Console.WriteLine("Enter the ISBN-13 of the book to remove:");
//        var isbn13 = Console.ReadLine();

//        var inventoryBalance = context.InventoryBalances.FirstOrDefault(ib => ib.StoreId == storeId && ib.Isbn13 == isbn13);
//        if (inventoryBalance == null)
//        {
//            Console.WriteLine("Book does not exist in the specified store.");
//            return;
//        }


//        if (inventoryBalance.NoOfProducts > 1)
//        {
//            inventoryBalance.NoOfProducts--;
//        }
//        else
//        {
//            context.InventoryBalances.Remove(inventoryBalance);
//        }

//        context.SaveChanges();
//        Console.WriteLine("Book removed from the store successfully.");
//    }



//    static void UpdateBook(BookStoresContext context)
//    {
//        Console.WriteLine("Enter the ISBN-13 of the book you want to update:");
//        var isbn13 = Console.ReadLine();

//        var book = context.Books.Find(isbn13);
//        if (book == null)
//        {
//            Console.WriteLine("Book not found in the database.");
//            return;
//        }


//        Console.WriteLine("Enter the new title of the book (leave empty to keep current title):");
//        var newTitle = Console.ReadLine();
//        if (!string.IsNullOrEmpty(newTitle))
//        {
//            book.Title = newTitle;
//        }

//        Console.WriteLine("Enter the new language of the book (leave empty to keep current language):");
//        var newLanguage = Console.ReadLine();
//        if (!string.IsNullOrEmpty(newLanguage))
//        {
//            book.Language = newLanguage;
//        }

//        Console.WriteLine("Enter the new price of the book (leave empty to keep current price):");
//        var newPriceInput = Console.ReadLine();
//        if (!string.IsNullOrEmpty(newPriceInput))
//        {
//            if (!int.TryParse(newPriceInput, out int newPrice))
//            {
//                Console.WriteLine("Invalid price.");
//                return;
//            }
//            book.Price = newPrice;
//        }

//        Console.WriteLine("Enter the new release date of the book (leave empty to keep current release date):");
//        var newReleaseDateInput = Console.ReadLine();
//        if (!string.IsNullOrEmpty(newReleaseDateInput))
//        {
//            if (!DateOnly.TryParse(newReleaseDateInput, out DateOnly newReleaseDate))
//            {
//                Console.WriteLine("Invalid release date.");
//                return;
//            }
//            book.ReleaseDate = newReleaseDate;
//        }

//        context.SaveChanges();
//        Console.WriteLine("Book updated successfully.");
//    }
//}


//////using DataAccess.Entities;
//////using System;
//////using System.Globalization;
//////using System.Linq;
//////using DataAccess;
//////using Microsoft.EntityFrameworkCore;

//////namespace BookstoreApp
//////{
//////    class Program
//////    {
//////        static void Main(string[] args)
//////        {
//////            using (var dbContext = new BookStoresContext())
//////            {
//////                while (true)
//////                {
//////                    Console.WriteLine("Choose an option:");
//////                    Console.WriteLine("1. Add a book");
//////                    Console.WriteLine("2. Add an author");
//////                    Console.WriteLine("3. Remove a book from a store");
//////                    Console.WriteLine("4. List inventory balance for stores");
//////                    Console.WriteLine("0. Exit");

//////                    var option = Console.ReadLine();

//////                    switch (option)
//////                    {
//////                        case "1":
//////                            AddBook(dbContext);
//////                            break;
//////                        case "2":
//////                            AddAuthor(dbContext);
//////                            break;
//////                        case "3":
//////                            RemoveBookFromStore(dbContext);
//////                            break;
//////                        case "4":
//////                            ListInventoryBalance(dbContext);
//////                            break;
//////                        case "0":
//////                            return;
//////                        default:
//////                            Console.WriteLine("Invalid option. Please try again.");
//////                            break;
//////                    }

//////                    Console.WriteLine();
//////                }
//////            }
//////        }

//////        static void AddBook(BookStoresContext context)
//////        {
//////            Console.WriteLine("Adding a book...");


//////            var authors = context.Authors.ToList();
//////            Console.WriteLine("Existing Authors:");
//////            foreach (var author in authors)
//////            {
//////                Console.WriteLine($"{author.AuthorId}: {author.FirstName} {author.LastName}");
//////            }


//////            Console.WriteLine("Select an author by ID or enter a new author:");
//////            var authorInput = Console.ReadLine();

//////            if (int.TryParse(authorInput, out int authorId))
//////            {
//////                var existingAuthor = authors.FirstOrDefault(a => a.AuthorId == authorId);
//////                if (existingAuthor != null)
//////                {

//////                    Console.WriteLine($"Selected author: {existingAuthor.FirstName} {existingAuthor.LastName}");
//////                }
//////                else
//////                {
//////                    Console.WriteLine("Invalid author ID. Creating a new author...");
//////                    AddAuthor(context);
//////                    return;
//////                }
//////            }
//////            else
//////            {
//////                Console.WriteLine("Creating a new author...");
//////                AddAuthor(context);
//////                return;
//////            }


//////            Console.WriteLine("Enter the store ID:");
//////            var storeIdInput = Console.ReadLine();
//////            if (!int.TryParse(storeIdInput, out int storeId))
//////            {
//////                Console.WriteLine("Invalid store ID.");
//////                return;
//////            }

//////            Console.WriteLine("Choose from existing books:");

//////                var books = context.Books.ToList();
//////                if (books.Count == 0)
//////                {
//////                    Console.WriteLine("No books found in the assortment.");
//////                }
//////                else
//////                {
//////                    for (int i = 0; i < books.Count; i++)
//////                    {
//////                        var book = books[i];
//////                        Console.WriteLine($"{i + 1}. {book.Title} (ISBN-13: {book.Isbn13})");
//////                    }
//////                }

//////                Console.WriteLine("Enter the number corresponding to the book you want to add (0 to add a new book):");
//////                var bookChoiceInput = Console.ReadLine();
//////                if (!int.TryParse(bookChoiceInput, out int bookChoice))
//////                {
//////                    Console.WriteLine("Invalid book choice.");
//////                    return;
//////                }

//////                Book selectedBook;
//////                if (bookChoice == 0)
//////                {
//////                    Console.WriteLine("Enter the title of the new book:");
//////                    var title = Console.ReadLine();
//////                    Console.WriteLine("Enter the ISBN-13 of the new book:");
//////                    var isbn13 = Console.ReadLine();
//////                    Console.WriteLine("Enter the price of the new book:");
//////                    var priceInput = Console.ReadLine();
//////                    if (!int.TryParse(priceInput, out int price))
//////                    {
//////                        Console.WriteLine("Invalid price.");
//////                        return;
//////                    }

//////                    Console.WriteLine("Enter the language of the new book:");
//////                    var language = Console.ReadLine();
//////                    Console.WriteLine("Enter the release date of the new book (yyyy-MM-dd):");
//////                    var releaseDateInput = Console.ReadLine();
//////                    if (!DateOnly.TryParse(releaseDateInput, out DateOnly releaseDate))
//////                    {
//////                        Console.WriteLine("Invalid release date.");
//////                        return;
//////                    }

//////                    selectedBook = new Book
//////                    {
//////                        Title = title,
//////                        Isbn13 = isbn13,
//////                        Price = price,
//////                        Language = language,
//////                        ReleaseDate = releaseDate
//////                    };
//////                    context.Books.Add(selectedBook);
//////                }
//////                else if (bookChoice < 1 || bookChoice > books.Count)
//////                {
//////                    Console.WriteLine("Invalid book choice.");
//////                    return;
//////                }
//////                else
//////                {
//////                    selectedBook = books[bookChoice - 1];
//////                }

//////                var inventoryBalance =
//////                    context.InventoryBalances.FirstOrDefault(ib =>
//////                        ib.StoreId == storeId && ib.Isbn13 == selectedBook.Isbn13);
//////                if (inventoryBalance != null)
//////                {
//////                    inventoryBalance.NoOfProducts++;
//////                }
//////                else
//////                {
//////                    inventoryBalance = new InventoryBalance
//////                    {
//////                        StoreId = storeId,
//////                        Isbn13 = selectedBook.Isbn13,
//////                        NoOfProducts = 1
//////                    };
//////                    context.InventoryBalances.Add(inventoryBalance);
//////                }

//////                context.SaveChanges();
//////                Console.WriteLine("Book added to the store successfully.");

//////        }



//////        static void AddAuthor(BookStoresContext dbContext)
//////        {

//////            Console.WriteLine("\nList of Authors:");
//////            var authors = dbContext.Authors.ToList();
//////            foreach (var a in authors)
//////            {
//////                Console.WriteLine($"Author ID: {a.AuthorId}, Name: {a.FirstName} {a.LastName}");
//////            }

//////            Console.WriteLine("Adding an author...");


//////            Console.Write("Author ID: ");
//////            var authorIdInput = Console.ReadLine();

//////            if (int.TryParse(authorIdInput, out int authorId))
//////            {
//////                var existingAuthor = dbContext.Authors.FirstOrDefault(a => a.AuthorId == authorId);

//////                if (existingAuthor != null)
//////                {
//////                    Console.WriteLine("Author with the same ID already exists. Please enter a different ID.");
//////                    AddAuthor(dbContext); 
//////                    return;
//////                }
//////            }
//////            else
//////            {
//////                Console.WriteLine("Invalid author ID. Please enter a valid ID.");
//////                AddAuthor(dbContext); 
//////                return;
//////            }


//////            Console.Write("First Name: ");
//////            var firstName = Console.ReadLine();

//////            Console.Write("Last Name: ");
//////            var lastName = Console.ReadLine();

//////            Console.Write("Date of Birth (YYYY-MM-DD): ");
//////            var dateOfBirthInput = Console.ReadLine();

//////            if (DateOnly.TryParse(dateOfBirthInput, out DateOnly dateOfBirth))
//////            {

//////                var author = new Author
//////                {
//////                    AuthorId = authorId,
//////                    FirstName = firstName,
//////                    LastName = lastName,
//////                    DateOfBirth = dateOfBirth
//////                };


//////                dbContext.Authors.Add(author);
//////                dbContext.SaveChanges();

//////                Console.WriteLine("Author added successfully!");


//////            }
//////            else
//////            {
//////                Console.WriteLine("Invalid date of birth. Please enter a valid date (YYYY-MM-DD).");
//////                AddAuthor(dbContext); 
//////            }
//////        }

//////        static void RemoveBookFromStore(BookStoresContext dbContext)
//////        {
//////            Console.WriteLine("Removing a book from a store...");


//////            Console.WriteLine("Book removed from the store successfully!");
//////        }

//////        static void ListInventoryBalance(BookStoresContext dbContext)
//////        {
//////            Console.WriteLine("Inventory balance for stores:");



//////            Console.WriteLine("End of inventory balance list.");
//////        }

//////    }
//////}













//using DataAccess.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;

//namespace BookstoreConsoleApp
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            using (var dbContext = new BookStoresContext())
//            {
//                // Main menu
//                while (true)
//                {
//                    Console.WriteLine("1. List Inventory Balance");
//                    Console.WriteLine("2. Add Book to Store");
//                    Console.WriteLine("3. Add New Title");
//                    Console.WriteLine("4. Edit Title");
//                    Console.WriteLine("5. Delete Title");
//                    Console.WriteLine("6. Exit");
//                    Console.WriteLine("Enter your choice:");


//                    int choice = Convert.ToInt32(Console.ReadLine());

//                    switch (choice)
//                    {
//                        case 1:
//                            ListInventoryBalances(dbContext);
//                            break;
//                        case 2:
//                            AddBookAndAuthor(dbContext);
//                            break;
//                        case 3:
//                            AddNewBook(dbContext);
//                            break;
//                        case 4:
//                            EditTitle(dbContext);
//                            break;
//                        case 5:
//                            DeleteTitle(dbContext);
//                            break;
//                        case 6:
//                            return;
//                        default:
//                            Console.WriteLine("Invalid choice. Please try again.");
//                            break;
//                    }
//                }
//            }
//        }

//        static void ListInventoryBalances(BookStoresContext dbContext)
//        {
//            var inventoryBalances = (
//                from ib in dbContext.InventoryBalances
//                join b in dbContext.Books on ib.Isbn13 equals b.Isbn13
//                select new
//                {
//                    StoreId = ib.StoreId,
//                    Isbn13 = ib.Isbn13,
//                    NoOfProducts = ib.NoOfProducts,
//                    Title = b.Title
//                }).ToList();

//            if (inventoryBalances.Count == 0)
//            {
//                Console.WriteLine("No inventory balances found.");
//                return;
//            }


//            Console.WriteLine("Inventory Balances:");
//            foreach (var inventoryBalance in inventoryBalances)
//            {
//                Console.WriteLine($"Store ID: {inventoryBalance.StoreId}");
//                Console.WriteLine($"ISBN-13: {inventoryBalance.Isbn13}");
//                Console.WriteLine($"No. of Products: {inventoryBalance.NoOfProducts}");
//                Console.WriteLine($"Title: {inventoryBalance.Title}");
//                Console.WriteLine();
//            }
//        }



//            static void AddBookAndAuthor(BookStoresContext dbContext)
//            {
//                // Prompt the user to enter the author details
//                Console.WriteLine("Enter the author ID:");
//                var authorIdInput = Console.ReadLine();
//                if (!int.TryParse(authorIdInput, out int authorId))
//                {
//                    Console.WriteLine("Invalid author ID.");
//                    return;
//                }

//                Console.WriteLine("Enter the author name:");
//                var name = Console.ReadLine();

//                // Create a new author
//                var newAuthor = new Author
//                {
//                    AuthorId = authorId,
//                    FirstName = name
//                };

//                // Add the author to the database
//                dbContext.Authors.Add(newAuthor);
//                dbContext.SaveChanges();
//                Console.WriteLine("Author added successfully.");

//                // Prompt the user to enter the book details
//                Console.WriteLine("Enter the book title:");
//                var title = Console.ReadLine();

//                Console.WriteLine("Enter the book price:");
//                var priceInput = Console.ReadLine();
//                if (!int.TryParse(priceInput, out int price))
//                {
//                    Console.WriteLine("Invalid price.");
//                    return;
//                }

//                // Create a new book
//                var newBook = new Book
//                {
//                    Title = title,
//                    Price = price,
//                    AuthorNo = authorId
//                };

//                // Add the book to the database
//                dbContext.Books.Add(newBook);
//                dbContext.SaveChanges();
//                Console.WriteLine("Book added successfully.");

//                // Display the added book and author
//                Console.WriteLine("Added Book:");
//                Console.WriteLine($"Title: {newBook.Title}, Price: {newBook.Price:C}");
//                Console.WriteLine("Added Author:");
//                Console.WriteLine($"Author ID: {newAuthor.AuthorId}, Name: {newAuthor.FirstName}");
//            }






//        static void AddNewBook(BookStoresContext dbContext)
//        {
//            Console.WriteLine("Enter the book details:");
//            Console.Write("Title: ");
//            string title = Console.ReadLine();
//            Console.Write("Language: ");
//            string language = Console.ReadLine();
//            Console.Write("Price: ");
//            int price = Convert.ToInt32(Console.ReadLine());
//            Console.Write("Release Date (yyyy-MM-dd): ");
//            string releaseDateString = Console.ReadLine();
//            DateOnly releaseDate = DateOnly.Parse(releaseDateString);
//            Console.Write("ISBN-13: ");
//            string isbn13 = Console.ReadLine();

//            Console.WriteLine("Do you want to add a new author? (Y/N)");
//            string addAuthorChoice = Console.ReadLine();

//            int authorId;
//            if (addAuthorChoice.Equals("Y", StringComparison.OrdinalIgnoreCase))
//            {
//                Console.WriteLine("Enter the author details:");
//                Console.Write("AuthorId: ");
//                authorId = Convert.ToInt32(Console.ReadLine());
//                Console.Write("First Name: ");
//                string firstName = Console.ReadLine();
//                Console.Write("Last Name: ");
//                string lastName = Console.ReadLine();
//                Console.Write("Date of Birth (yyyy-MM-dd): ");
//                string dateOfBirthString = Console.ReadLine();
//                DateOnly dateOfBirth = DateOnly.Parse(dateOfBirthString);

//                // Create a new author object and add it to the Authors table
//                Author newAuthor = new Author
//                {
//                    AuthorId = authorId,
//                    FirstName = firstName,
//                    LastName = lastName,
//                    DateOfBirth = dateOfBirth
//                };

//                // Add the new author to the DbContext and save changes
//                dbContext.Authors.Add(newAuthor);
//                dbContext.SaveChanges();
//            }
//            else
//            {
//                Console.WriteLine("Select an existing author:");

//                // Fetch all existing authors from the Authors table
//                var authors = dbContext.Authors.ToList();

//                // Display a list of existing authors for the user to choose from
//                foreach (var author in authors)
//                {
//                    Console.WriteLine($"{author.AuthorId}. {author.FirstName} {author.LastName}");
//                }

//                Console.Write("AuthorId: ");
//                authorId = Convert.ToInt32(Console.ReadLine());
//            }

//            // Create a new book object with the provided details and authorId
//            Book newBook = new Book
//            {
//                Title = title,
//                Language = language,
//                Price = price,
//                ReleaseDate = releaseDate,
//                Isbn13 = isbn13,
//                AuthorNo = authorId
//            };

//            // Add the new book to the Books table
//            dbContext.Books.Add(newBook);
//            dbContext.SaveChanges();

//            Console.WriteLine("New book added successfully!");
//        }



//static void EditTitle(BookStoresContext dbContext)
//        {
//            Console.WriteLine("Enter the ISBN of the book to edit:");
//            string isbn = Console.ReadLine();

//            var bookToEdit = dbContext.Books.FirstOrDefault(b => b.Isbn13 == isbn);
//            if (bookToEdit != null)
//            {
//                Console.WriteLine("Enter the new title:");
//                string newTitle = Console.ReadLine();

//                bookToEdit.Title = newTitle;
//                dbContext.SaveChanges();

//                Console.WriteLine("Title edited successfully!");
//            }
//            else
//            {
//                Console.WriteLine("Book not found.");
//            }
//        }


//        static void DeleteTitle(BookStoresContext dbContext)
//        {
//            Console.WriteLine("Enter the ISBN of the book to delete:");
//            string isbn = Console.ReadLine();

//            var bookToDelete = dbContext.Books.FirstOrDefault(b => b.Isbn13 == isbn);
//            if (bookToDelete != null)
//            {
//                dbContext.Books.Remove(bookToDelete);
//                dbContext.SaveChanges();

//                Console.WriteLine("Book deleted successfully!");
//            }
//            else
//            {
//                Console.WriteLine("Book not found.");
//            }
//        }
//    }
//}