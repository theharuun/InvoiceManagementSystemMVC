# InvoiceManagementSystemMVC(ENGLISH)

## Overview

**InvoiceManagementSystemMVC** is a web-based application designed to efficiently manage invoices, clients, and products. It is built using the Model-View-Controller (MVC) design pattern and is intended to simplify invoice generation, storage, and management for small to medium-sized businesses.

## Features

- **User Authentication**: Secure login and registration system for users.
- **Invoice Management**: Create, view, edit, and delete invoices.
- **Client Management**: Store and manage client details.
- **Product Management**: Add, update, and remove products from the system.
- **Search and Filtering**: Search functionality to filter invoices, clients, and products.
- **PDF Export**: Generate and export invoices as PDF files.
- **Reports**: Detailed reporting on invoices and business performance.

## Technologies Used

### Frontend
- HTML
- CSS
- JavaScript
- Bootstrap (for responsive UI design)

### Backend
- C#
- ASP.NET MVC

### Database
- Microsoft SQL Server (MSSQL)

### Tools
- Entity Framework (for database operations)
- LINQ (for query operations)
- Identity (for authentication and authorization)

## Installation

1. Clone the repository: "git clone https://github.com/theharuun/InvoiceManagementSystemMVC.git"
2. Navigate to the project directory:"cd InvoiceManagementSystemMVC"
3. Open the solution in Visual Studio.
4. Restore NuGet packages:Right-click on the solution and select Restore NuGet Packages.
5. Update the database connection string in the appsettings.json file to match your SQL Server setup:"  "ConnectionStrings": {"DefaultConnection" "Server=your_server_name;Database=InvoiceDB;Trusted_Connection=True;MultipleActiveResultSets=true"}  "
6. Run the following command in the Package Manager Console to apply database migrations: Update-Database
7. Start the application by pressing F5 in Visual Studio.


## Usage

### After launching the application, you can log in or register as a new user.
### Once logged in, you can start creating and managing invoices, adding clients, and managing products.
### Navigate through the user-friendly interface to access different features like generating PDF invoices or viewing reports.

## Contribution

### If you would like to contribute to this project, please follow these steps:

   1.Fork the repository.
   2.Create a feature branch (git checkout -b feature-branch).
   3.Commit your changes (git commit -m 'Add some feature').
   4.Push to the branch (git push origin feature-branch).
Open a Pull Request.

## License
**This project is licensed under the MIT License - see the LICENSE file for details.

## Contact
**If you have any questions, feel free to reach out:

**Email: hrnkrkmz57@gmail.com
**GitHub: theharuun

   
