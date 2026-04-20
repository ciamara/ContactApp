# ContactApp
simple contacts SPA in asp .net core


## Classes and Methods descriptions          

### Controllers          
* **`AuthController`**: Log in and authorization process.        
    * `Login([FromBody] LoginModel model)`: [POST] Takes login data (username and password), verifies them and when the credentials match it generates and returns a signed JWT token that lasts 3 hours. Returns 401 (unauthorized) on error.
* **`ContactsController`**: Main CRUD operations for contacts.db and provides dictionary. Most methods require the JWT token (`[Authorize]`).
    * `GetContacts()`: [GET] Public and simplified contacts list (ID, first name, last name). Accessible without login.
    * `GetContact(int id)`: [GET] Full details of a contact. Requires login.
    * `PostContact(Contact contact)`: [POST] New contact in db. Checks if email is unique (400 when not unique). Requires login.
    * `PutContact(int id, Contact contact)`: [PUT] Update of an existing contact, checks for concurrency. Requires login.
    * `DeleteContact(int id)`: [DELETE] Deletes contact from db. Requires login.
    * `GetDictionary()`: [GET] Returns dictionary for frontend. Accessible without login.

### Models & Data
* **`AppDbContext`**: Class inheriting from `DbContext` (Entity Framework Core). Holds (`DbSet<Contact>`, `DbSet<CategoryDictionary>`) and responsible for communication with sqlite. In `OnModelCreating` forces uniqueness of the Email column at the database level.
* **`CategoryDictionary`**: Data model representing a single dictionary entry (includes id `Id`, field `CategoryName` and optional field `SubcategoryName`).
* **`LoginModel`**: Data transfer object user for mapping data from HTTP POST during login.
* **`Contact`**: Main db entity, holds full data about a contact.


## Used libraries
Project uses external NuGet packages and namespaces:
* **`Microsoft.EntityFrameworkCore`** and **`Microsoft.EntityFrameworkCore.Sqlite`**: Object-entity mapping and local SQLite database.
* **`Microsoft.AspNetCore.Authentication.JwtBearer`**: Authorization and token validation.
* **`Microsoft.IdentityModel.Tokens`** and **`System.IdentityModel.Tokens.Jwt`**: AuthController for generating, signing and ciphering JWT tokens.
* **`Swashbuckle.AspNetCore`**: (Swagger) Interactive (UI) documenting API endpoints based on controllers.

## Compilation & Running
Open cmd in project root and input commands:

1. **Downloading packages:**
   ```bash
   dotnet restore
2. **Compiling:**
   ```bash
   dotnet build
3. **Running:**
   ```bash
   dotnet run
4. **Open local address displayed in the terminal**

Project can also be easily compiled and ran through visual studio.


