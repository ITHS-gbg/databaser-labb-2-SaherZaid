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
            Console.WriteLine("Welcome to the Bookstore Inventory !");

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. List inventory balances for all stores");
                Console.WriteLine("2. Add book/author to a store");
                Console.WriteLine("3. Remove a book from a store");
                Console.WriteLine("4. Update a book in a store");
                Console.WriteLine("5. Remove an author");
                Console.WriteLine("6. Update an author");
                Console.WriteLine("7. Exit");

                var option = Console.ReadLine();
                Console.WriteLine();

                if (option == "1")
                {
                    ListInventoryBalances(context);
                }
                else if (option == "2")
                {
                    AddBook(context);
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
                    RemoveAuthor(context);
                }
                else if (option == "6")
                {
                    UpdateAuthor(context);
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

    
static void AddBook(BookStoresContext context)
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


        Console.WriteLine("\nList of Authors:");
        var authors = context.Authors.ToList();
        foreach (var a in authors)
        {
            Console.WriteLine($"Author ID: {a.AuthorId}, Name: {a.FirstName} {a.LastName}");
        }

        Console.WriteLine("Enter the author ID (0 for a new author):");
        var authorIdInput = Console.ReadLine();

        if (int.TryParse(authorIdInput, out int authorId))
        {
            Author selectedAuthor = null;

            if (authorId == 0)
            {
                Console.WriteLine("Creating a new author...");
                Console.Write("Author ID: ");

                var newAuthorIdInput = Console.ReadLine();
                if (int.TryParse(newAuthorIdInput, out int newAuthorId))
                {
                    if (authors.Any(a => a.AuthorId == newAuthorId))
                    {
                        Console.WriteLine("Author with the specified ID already exists.");
                        return;
                    }

                    authorId = newAuthorId;
                }
                else
                {
                    Console.WriteLine("Invalid author ID. Please enter a valid ID.");
                    return;
                }

                Console.Write("First Name: ");
                var firstName = Console.ReadLine();

                Console.Write("Last Name: ");
                var lastName = Console.ReadLine();

                Console.Write("Date of Birth (YYYY-MM-DD): ");
                var dateOfBirthInput = Console.ReadLine();

                if (DateOnly.TryParse(dateOfBirthInput, out DateOnly dateOfBirth))
                {
                   
                    var newAuthor = new Author
                    {
                        AuthorId = newAuthorId,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth
                    };

                    
                    context.Authors.Add(newAuthor);
                    context.SaveChanges();

                    selectedAuthor = newAuthor;
                    Console.WriteLine("New author added successfully!");
                }
                else
                {
                    Console.WriteLine("Invalid date of birth. Please enter a valid date (YYYY-MM-DD).");
                    return;
                }
            }
            else
            {
                selectedAuthor = context.Authors.FirstOrDefault(a => a.AuthorId == authorId);

                if (selectedAuthor == null)
                {
                    Console.WriteLine("Author with the specified ID does not exist.");
                    return;
                }
            }
            context.SaveChanges();
            Console.WriteLine("Book added to the store successfully.");
        }
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

        context.InventoryBalances.Remove(inventoryBalance);

        var book = context.Books.Find(isbn13);
        if (book != null)
        {
            context.Books.Remove(book);
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


    static void RemoveAuthor(BookStoresContext context)
    {
        
            var authors = context.Authors.ToList();
            Console.WriteLine("Authors:");
            foreach (var author in authors)
            {
                Console.WriteLine($"AuthorId: {author.AuthorId}, Name: {author.FirstName} {author.LastName}");
            }

            Console.WriteLine();


            Console.Write("Enter the AuthorId of the author to remove: ");
            int authorIdToRemove;
            if (int.TryParse(Console.ReadLine(), out authorIdToRemove))
            {

                var authorToRemove = context.Authors.FirstOrDefault(a => a.AuthorId == authorIdToRemove);
                if (authorToRemove != null)
                {

                    context.Authors.Remove(authorToRemove);


                
                var relatedBooks = context.Books.Where(b => b.AuthorNo == authorIdToRemove).ToList();
                relatedBooks.ForEach(b => b.AuthorNo = null);

               
                context.SaveChanges();

                Console.WriteLine("Author removed successfully.");
            }
                else
                {
                    Console.WriteLine("Author not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

    }


    static void UpdateAuthor(BookStoresContext context)
    {


        var authors = context.Authors.ToList();
        Console.WriteLine("Authors:");
        foreach (var author in authors)
        {
            Console.WriteLine(
                $"AuthorId: {author.AuthorId}, Name: {author.FirstName} {author.LastName} Date of birth:{author.DateOfBirth}");
        }

        Console.WriteLine();


        Console.Write("Enter the AuthorId of the author to update: ");
        int authorIdToUpdate;
        if (int.TryParse(Console.ReadLine(), out authorIdToUpdate))
        {

            var authorToUpdate = context.Authors.FirstOrDefault(a => a.AuthorId == authorIdToUpdate);
            if (authorToUpdate != null)
            {

                Console.Write("Enter the updated First Name (press Enter to skip): ");
                string updatedFirstName = Console.ReadLine();
                Console.Write("Enter the updated Last Name (press Enter to skip): ");
                string updatedLastName = Console.ReadLine();
                Console.Write("Enter the updated Date of Birth (press Enter to skip): ");
                string updatedDateOfBirthInput = Console.ReadLine();


                if (!string.IsNullOrWhiteSpace(updatedFirstName))
                {
                    authorToUpdate.FirstName = updatedFirstName;
                }

                if (!string.IsNullOrWhiteSpace(updatedLastName))
                {
                    authorToUpdate.LastName = updatedLastName;
                }

                if (!string.IsNullOrWhiteSpace(updatedDateOfBirthInput))
                {
                    DateOnly updatedDateOfBirth;
                    if (DateOnly.TryParse(updatedDateOfBirthInput, out updatedDateOfBirth))
                    {
                        authorToUpdate.DateOfBirth = updatedDateOfBirth;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Date of Birth input. Skipping update.");
                    }
                }


                context.SaveChanges();

                Console.WriteLine("Author updated successfully.");
            }
            else
            {
                Console.WriteLine("Author not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }


}





