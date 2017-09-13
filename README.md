# BankingAPI
An API solution created for my own needs to predict how much money i save every month and to know when i can buy a Ferrari

First and foremost, i made it because of the technical topics that get touched. I needed a project to create an API for with an architecture 
that works etc.etc. 

I made it because i wanted to know which fixed transactions did i still have to pay. Based on the fixed transactions and my 
expenditure behaviour i can kind of forecast how much money i can save each month. It immediately helped me in two ways : 

1. I never miss a fixed transaction, even if i didn't make it an automatic payment 
2. I gained a bit of insight, i found out that if i dont buy 3 PS4 games each month, i might be able to buy a Ferrari around January 2031

You can upload CSVs containing bank transactions (Rabobank only for now) and they can be imported into the underlying MongoDb 
This uploading mechanism is now done via a seperate client but i will probably build some web-solution for it. 

Once all data is in the DB, you can "mark" certain transactions as being fixed transactions. A fixed transaction is a transaction I do every 
month. You can request a fixed transaction overview from the API and it will show you which fixed transactions are paid this month and are 
still open for this month based upon all transactions in the DB.

Technical Keywords : 

Dotnet core 
Swagger
DI
Logging
MongoDb
Repository Pattern
Automapper
DTO

Future enhancement : 
Identity Server connection


