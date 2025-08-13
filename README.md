# DCIT 318 – Object-Oriented Programming in C#
## Assignment 3

## Projects

1. **FinanceAppDemo** – Transaction processing using interfaces, records, and sealed classes.
2. **HealthSystemDemo** – Patient data management using generics and collections.
3. **InventoryLoggerSystem** – Inventory logging with records and file serialization.
4. **SchoolGradingSystem** – Grading system with file I/O and custom validation exceptions.
5. **WarehouseInventoryDemo** – Inventory management using custom exceptions and generic repositories.


## Question 1: Finance Management System

## Author

**Gideon Mangortey**  
University of Ghana, Legon  
Course: DCIT 318 – Object-Oriented Programming  
Semester: Year 3, Semester 2


## Project Overview

This project implements a **Finance Management System** in C# that tracks transactions, enforces data integrity, and supports multiple transaction types and account behaviors.  

Key features include:

- **Records** for immutable transaction models
- **Interfaces** to define payment processing behaviors
- **Sealed classes** to control inheritance for specialized accounts
- **Polymorphism** via different transaction processor implementations
- **Account management** with validation of available funds


## Concepts Demonstrated

### 🔹 Records
- `Transaction` record stores immutable financial data:  
  `Id`, `Date`, `Amount`, `Category`

### 🔹 Interfaces
- `ITransactionProcessor` defines a `Process` method for handling transactions

### 🔹 Implementations
- **BankTransferProcessor** – prints a distinct bank transfer message  
- **MobileMoneyProcessor** – prints a distinct mobile money message  
- **CryptoWalletProcessor** – prints a distinct cryptocurrency transaction message

### 🔹 Account Hierarchy
- **Account (base class)** – stores `AccountNumber`, `Balance`, and basic deduction logic  
- **SavingsAccount (sealed)** – overrides deduction logic to enforce “Insufficient funds” check

## Project Structure





## Question 2: Healthcare System with Generics and Collections

## Project Overview

This project implements a **Healthcare System** in C# that manages patient records and prescriptions using:

- **Generic classes and methods** for reusable entity management  
- **Collections** (`List`, `Dictionary`) for scalable data grouping  
- **Type-safe design** using generics and constraints  
- **Modular structure** for maintainability and clarity



## Concepts Demonstrated

### 🔹 Generic Repository
- `Repository<T>` handles storage, retrieval, and removal of entities
- Used for both `Patient` and `Prescription` management

### 🔹 Models
- `Patient` class with `Id`, `Name`, `Age`, `Gender`
- `Prescription` class with `Id`, `PatientId`, `MedicationName`, `DateIssued`

### 🔹 Collections
- `Dictionary<int, List<Prescription>>` maps PatientId to their prescriptions
- Grouping and retrieval logic encapsulated in `PrescriptionIndex`

### 🔹 Application Logic
- `HealthSystemApp` seeds data, builds prescription map, and prints results
- Demonstrates full flow from data entry to grouped output

## Project Structure





## Question 3: Warehouse Inventory Management System


## Project Overview

This project implements a **Warehouse Inventory Management System** in C# using:

- Marker interface for inventory items  
- Generic repository for safe and reusable operations  
- Custom exceptions for robust error handling  
- Collections (`List`, `Dictionary`) for efficient storage  
- Exception-safe methods with user-friendly messages


## Concepts Demonstrated

### 🔹 Interfaces
- `IInventoryItem` defines shared properties for all inventory items

### 🔹 Product Models
- `ElectronicItem` with brand and warranty info  
- `GroceryItem` with expiry date

### 🔹 Generic Repository
- `InventoryRepository<T>` manages items by ID  
- Supports add, remove, update, and retrieval  
- Uses constraints: `where T : IInventoryItem`

### 🔹 Custom Exceptions
- `DuplicateItemException`  
- `ItemNotFoundException`  
- `InvalidQuantityException`

### 🔹 Manager Class
- `WareHouseManager` handles both grocery and electronics  
- Uses `try-catch` blocks to gracefully handle errors

## Folder Structure





## Question 4: School Grading System


## Project Overview

This project implements a **School Grading System** in C# that reads student records from a `.txt` file, validates and grades them, and writes a clean summary report to a new file.

Key features include:

- File I/O using `StreamReader` and `StreamWriter`  
- Custom exceptions for validation errors  
- Grade assignment logic based on score ranges  
- Exception-safe processing with user-friendly error messages


## Concepts Demonstrated

### 🔹 Models
- `Student` class with `Id`, `FullName`, `Score`, and `GetGrade()` method

### 🔹 Custom Exceptions
- `InvalidScoreFormatException` – triggered when score is not an integer  
- `MissingFieldException` – triggered when a line has fewer than 3 fields

### 🔹 Services
- `StudentResultProcessor` handles reading and writing student data  
- Validates input and throws appropriate exceptions

### 🔹 Application Flow
- `Program.cs` wraps the entire process in a `try-catch` block  
- Catches and displays specific exceptions with clear messages

## Folder Structure





## Question 5: Inventory Logging System


## Project Overview

This project implements a robust **Inventory Logging System** in C# using:

- **Records** for immutable inventory data  
- **Generics** for reusable logging operations  
- **File I/O** for persistent storage  
- **Exception handling** for safe read/write operations


## Concepts Demonstrated

### 🔹 Immutable Record
- `InventoryItem` is a C# record with:
  ```csharp
  int Id, string Name, int Quantity, DateTime DateAdded
  