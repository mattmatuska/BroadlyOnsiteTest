A guide to this project: 

The database I used was MySQL.  Two SQL scripts build my test database (APPOINTMENT_DATA_MOCK.sql, PATIENT_DATA_MOCK.sql). 

I set the PKs and, in the case of appointments, the patient_id FK on my test database. 

I imported the database files using Microsoft's Entity Framework. You will also need to grab the MySql.Data.Entity from the NuGet. The addition spits out database objects which can be viewed from Visual Studio vie the BroadlyModel.edmx. 

I used third-party Newtonsoft.JSON so that the export date is formatted correctly. 

Most of the "work" is being done by the GetTransactions() function in DataSource, for DB interface.  

TODOs include some command line or app settings configuration (setting the scan date and upload http address from a website), and finding appointments for past dates which were only updated on the scan date.

Also need to remove some unused references and configure the install. 

The Transaction class exists only to strongly type the tests.