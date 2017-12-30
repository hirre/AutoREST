# AutoREST

AutoREST is a library and a console app written in ASP .NET Core 2.0 which automatically creates a basic REST service for your database. 
The library supports basic CRUD and SQL.


Don't use this as your public REST API!

## How to run

	dotnet AutoRestRunService.dll

or to change the default port (5000) to e.g. 5005:

	dotnet AutoRestRunService.dll --port 5005

## Usage

**GET**

```url
api/tables/{TABLE NAME}/{ORDER BY COLUMN NAME}/[OPT: ORDER DIRECTION ASCENDING={true}]/[OPT: ROW OFFSET]/[OPT: PAGE SIZE]/[OPT: ?filter={SQL FILTER}]
```

Examples:

```url
http://localhost:5000/api/tables/testtable/id/

http://localhost:5000/api/tables/testtable/id/true/0/10/?filter=column1 > 7 and column2 like '%hello%'
```

**POST**

Send JSON data to this API endpoint:

```url
api/tables/{TABLE NAME}/
```

Example:

```url
HEADER:
Content-Type: application/json

URL:
http://localhost:5000/api/tables/testtable/

BODY:
{
    
   "test": "inserted"

}

```

200 HTTP code is received on success.

**PATCH/PUT**

Send JSON data to this API endpoint:

```url
api/tables/{TABLE NAME}/{COLUMN NAME}/{COLUMN VALUE}
```

Column name indicates which column to look for with a specific column value.

Example:

```url
HEADER:
Content-Type: application/json

URL:
http://localhost:5000/api/tables/testtable/id/3

BODY:
{
    
   "test": "updated"

}

```

200 HTTP code is received on success.

**DELETE**

```url
api/tables/{TABLE NAME}/{COLUMN NAME}/{COLUMN VALUE}
```

Column name indicates which column to look for with a specific column value.

Example:

```url
http://localhost:5000/api/tables/testtable/id/3
```

200 HTTP code is received on success.

## Remarks
* Basic SQL injection protection is implemented although you shouldn't count on it.
* The connections string is entered in *appsettings.json*
* You can also extend the library with different database adapters.