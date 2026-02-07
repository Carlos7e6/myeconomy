💸 MyEconomy: Own Your Wallet, Master Your Code!
Hey there! 👋 Welcome to MyEconomy, the place where I stop asking my bank account "Where did you go?" and start telling it "This is where you belong."

This isn't just another finance tracker; it's my personal laboratory for architectural excellence. While I'm managing my euros, I'm also mastering the art of building scalable, clean, and testable software.

🚀 The Mission
Managing money is hard. Managing spaghetti code is harder. MyEconomy is a full-stack solution designed to:

Simplify Finances: Track fixed costs and personal budget without the headache.

Showcase Best Practices: From TDD (Test Driven Development) to the Generic Repository Pattern, this project is built to last.

🛠 The Tech Stack
I’ve chosen the "Power Couple" of modern development:

Frontend: React (Atomic design, slick UI, and fast as lightning).

Backend: .NET 8 (The backbone. Robust, type-safe, and incredibly powerful).

Database: SQL Server managed through Entity Framework Core.

Testing: xUnit, NSubstitute, and FluentAssertions (Because if it isn't tested, it's broken).

🏗 High-Level Architecture
I believe in Separation of Concerns. That’s why the logic isn't just thrown into the API; it's modularized for the win!

my-economy-api: The gatekeeper. Clean controllers, minimal logic, and purely RESTful.

RepositoriPatern: My pride and joy. A dedicated Class Library (DLL) that handles data access through a Generic Repository. Need to swap the DB? No problem.

my-economy-api-test: The safety net. Integration tests that ensure everything from the HTTP request to the DB mock works perfectly.

✨ Best Practices Implemented
This project is my playground for doing things the right way:

Generic Repository Pattern: One repository to rule them all. No more boilerplate for basic CRUD operations.

Dependency Injection (DI): Fully decoupled components. .NET's IoC container is doing the heavy lifting.

TDD Approach: Red, Green, Refactor. I write tests that actually mean something.

Clean Code: Singular for entities, plural for tables, and meaningful naming conventions everywhere.

Modularization: Business logic and data access live in their own DLLs, ready to be reused in any project.

📣 Final Thought
"The best way to predict the future is to track your expenses and write unit tests for your budget." — Me, probably.
