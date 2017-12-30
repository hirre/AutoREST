# AutoREST

AutoREST is a library and a console app written in ASP .NET Core 2.0 which automatically creates a basic REST service for your database. 

Don't use this as your public REST API!

## Usage

```url
api/tables/{TABLE NAME}/{ORDER BY COLUMN NAME}/[OPT: ORDER DIRECTION ASCENDING={true}]/[OPT: OFFSET]/[OPT: PAGE SIZE]/[OPT: ?filter={SQL FILTER}]
```

Examples:

```url
http://www.testsite.com/api/tables/testtable/id/

http://www.testsite.com/api/tables/testtable/id/true/0/10/?filter=column1 > 7 AND column2 like '%hello%'
```