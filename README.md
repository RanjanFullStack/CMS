# Contact API

## Overview
The Contact API is a .NET Core Web API designed to manage contacts with robust CRUD operations. It currently utilizes a local JSON file as a mock database but is structured to facilitate easy integration with a real database in the future.

## Framework
- **Framework**: .NET Core (Version 8)
- **Data Storage**: Uses a local JSON file as a mock database.

## Functional Requirements
- **CRUD Operations**: Supports Create, Read, Update, and Delete operations for managing contacts.
- **User Feedback**: Provides detailed user feedback for each operation, ensuring clarity and responsiveness.

## Validation
### Field Validation:
- IDs must be unique.
- Emails must follow a valid format.
- First and Last names are required fields.

## Data Model
- **Id**: Auto-incrementing integer
- **FirstName**: String, required
- **LastName**: String, required
- **Email**: String, required, must be a valid email format

## Error Handling
- **Global Error Handling**: Implements global error handling and returns appropriate error responses to the client.

## Performance Considerations
The application is designed with scalability in mind, supporting potential future enhancements such as:
- Transitioning to a more robust database for handling larger datasets.
- Modular architecture to allow for distributed components and load balancing.

## Unit Tests
Comprehensive unit tests are written for all code in the project, ensuring reliability and ease of maintenance.

## Setup Instructions

### Step 1: Clone the Repository

git clone https://github.com/RanjanFullStack/CMS/tree/develop
cd ContactApi

### Step 2: Run the Application
Ensure you have .NET Core 8 installed. Execute the following commands:

dotnet restore
dotnet run

This will start the API at http://localhost:5000.

### Step 3: API Endpoints
- `GET /api/contact` - Retrieve a list of all contacts.
- `GET /api/contact/{id}` - Fetch a specific contact by ID.
- `POST /api/contact` - Create a new contact.
- `PUT /api/contact/{id}` - Update an existing contact.
- `DELETE /api/contact/{id}` - Remove a contact.

### Step 4 (Optional and should be done when requried)
To migrate from JSON file storage to a real database:

Replace JsonDbContext with an appropriate DbContext from Entity Framework.
Update the ContactRepository to interact with the new database.
Configure the connection string in appsettings.json for the selected database.

### Running Tests
To execute unit tests, run:

dotnet test


### Additional documention for GetContacts API:
The ApiResult<T> class encapsulates the functionality for handling sorted, paginated, and filtered results in a .NET application. Below are the key features and methods that achieve this:

1. Sorting
Method: ApplySorting
Functionality: This method accepts a column name and sort order (ascending or descending) to sort the results based on the specified property of the data model.
Implementation: It uses expression trees to dynamically create a sorting lambda based on the provided column name and applies the corresponding order using Queryable.OrderBy or Queryable.OrderByDescending.
2. Pagination
Method: CreateAsync and Create
Functionality: Both methods support pagination by accepting pageIndex and pageSize parameters.
Implementation: They use the Skip and Take methods to return only the specified subset of data, allowing for efficient handling of large datasets.
3. Filtering
Method: ApplyFilter
Functionality: This method filters the dataset based on a specified column and search query. It checks for a valid property and constructs a lambda expression that checks for matches against the Contains method of the string.
Implementation: If valid, it applies the filter to the IQueryable<T>, returning only the records that satisfy the filter criteria.
4. Error Handling
Method: IsValidProperty
Functionality: This method checks if the specified property name exists on the model. If it does not, it throws an exception to prevent runtime errors and provides clear feedback.
5. Comprehensive Result
Properties:
Data: Contains the retrieved records.
TotalCount: The total number of records before filtering, useful for client-side pagination calculations.
TotalPages: The total number of pages based on the pageSize.
HasPreviousPage and HasNextPage: Boolean properties indicating pagination status.
6. Scalability Considerations
The design allows for efficient data retrieval from potentially large datasets while ensuring that performance is maintained through the use of asynchronous methods for database operations.

By utilizing Entity Framework for asynchronous operations, the application can scale effectively to handle increased data loads and user requests.