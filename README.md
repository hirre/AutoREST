# AutoREST

AutoREST is a cross-platform library and a console app written in ASP .NET Core 2.0 that automatically creates a basic REST service for your database. 
The library supports basic CRUD and SQL. 

**WARNING! Don't use this software as your public REST API!**

## How to run

Set the database connection string in *appsettings.json* and then run:

	dotnet AutoRestRunService.dll

or to change the default port (5000) to e.g. 5005:

	dotnet AutoRestRunService.dll --port 5005

## Usage

**GET**

```url
api/tables/{TABLE NAME}/{ORDER BY COLUMN NAME}/?[OPTIONS]

Query options:
&asc=<bool> 				// column order (default true)
&offset=<int>				// offset (default 0)
&pagesize=<int>				// page size (default 200)
&filter=<SQL>				// SQL filter
&include=(src-id;dst-table;dst-id,...)	// include statement, src-id is in above [TABLE NAME], dst-table is the table to join with and dst-id is its join key
&outerjoin=<bool>			// indicates if outer join should be used (default false)

```

In the filter statement you can refer to [TABLE NAME] columns with alias T1 (e.g. T1.id > 3). 
The table columns in the include statement can be refered to with aliases T2, T3 and so on (as the declared order of appearance).

Examples:

```url
http://localhost:5000/api/tables/testtable/id/

http://localhost:5000/api/tables/testtable/id/?asc=true&offset=2&pagesize=10&filter=column1 > 7 and column2 like '%hello%'
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
* The database connection string is entered in *appsettings.json*.
* You can also choose to extend the library with different database adapters if you don't want to use the default SQL Server adapter.
* **WARNING! Don't use this software as your public REST API!**