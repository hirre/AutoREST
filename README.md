# AutoREST

AutoREST is a library and a console app written in ASP .NET Core 2.0 which automatically creates a basic REST service for your database. 

Don't use this as your public REST API!

## Usage

**GET**

```url
api/tables/{TABLE NAME}/{ORDER BY COLUMN NAME}/[OPT: ORDER DIRECTION ASCENDING={true}]/[OPT: OFFSET]/[OPT: PAGE SIZE]/[OPT: ?filter={SQL FILTER}]
```

Examples:

```url
http://localhost:5000/api/tables/testtable/id/

http://localhost:5000/api/tables/testtable/id/true/0/10/?filter=column1 > 7 and column2 like '%hello%'
```

**POST**

Send JSON data with *Content-Type: application/json* to below URL:

```url
api/tables/{TABLE NAME}/
```

Example:

```json
http://localhost:5000/api/tables/testtable/

BODY:
{
    
   "test": "inserted"

}

```

**PATCH/PUT**

Send JSON data with *Content-Type: application/json* to below URL:

```url
api/tables/{TABLE NAME}/{COLUMN NAME}/{COLUMN VALUE}
```

Column name indicates which column to look for with a specific column value.

Example:

```json
http://localhost:5000/api/tables/testtable/id/3

BODY:
{
    
   "test": "updated"

}

```

**DELETE**

```url
api/tables/{TABLE NAME}/{COLUMN NAME}/{COLUMN VALUE}
```

Column name indicates which column to look for with a specific column value.

Example:

```url
http://localhost:5000/api/tables/testtable/id/3
```