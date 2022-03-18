# DocumentDBService

This is a special service that allows for two things.  First, documents can be stored in a database as Json documents instead of standard SQL tables.  Second, it allows for the use of predefined queries.   These are "safe" and known queries that can be executed by the service.

The queries are defined (QueryDef) by a configuration file stored in the document database.   The defintions include the information about how to connect to the database, what fields are to be presented, and the text for the queries.

It also included information about the roles associated with the queries, but this has not been implemented yet.  

# running example can be found at www.predefinedquery.com/index.html 
# swagger page at www.predefinedquery.com/swagger


