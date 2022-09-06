# Cafeteria-Web-Application
A web application for the employees to be able to buy products from the cafeteria.
This project was a task I had in my internship in Bibliotheca Alexandrina to build this app for the employees.
To build this web application many steps were needed from constructing the database to implementing the web api and finally developing the front end that consumes the web api already constructed.

What can be done using this application?
- Employee can login using his email and password which are checked in the database.
- A new employee can register and all his data is recorded in the database.
- If logged in, a menu of all the products is shown, showing their name, available quantity, and price.
- The user can select any number products he wants to buy and can get more than one item of the same product by selecting the quantity required (which has to availabe).
- After selecting the products, the user is ready to checkout and a receipt with the items and their prices is shown.
- Finally, the user place the order and wait for it to be delivered.

- Another option for login is availabe, instead of entering as a normal employee who wants to buy some products, the user can enter as an admin if he is allowed to that. This is decided according to its job in the Bibliotheca, only supervisors are allowed to.
- By entering as admin, the user can add a new product to the list, increase the available quantity of an existing product, and finally mark an order already made as completed which means that it is delivered to the employee.

First, the database in constructed is constructed using my MySQL DBMS, on MySQL workbench. It consists of four tables:
1. Employees: which contains the data of the employees who work for the Bibliotheca. (Id, name, email, password, etc, ...)
2. Products: which contains all the data of the products in cafeteria, (Id, name, available quantity, price)
3. Orders: which represents a join table between Employees and Products (Many to Many relationship), for each employee who orders a product a new record in orders table is stored.
4. Departments: which contains the departments of the Bibliotheca.
Also, many stored procedures (STPs) were implemented to facilitate the work of the web API to be implemented next.

The next step was to build the REST Web API. This was implemented using ASP.Net core on Visual Studio 2019. Three controllers and three models were created for the employees, products and orders.
And for each of them the necessary CRUD (create, read, update, delete) operations were implemented. For example for the employees, a HTTP get request was needed to get an employee given its email and password, which was necessary for login. For the products, a get request was needed to get all of the products to show the menu of the products available to buy. Also, some put and post requests were developed, such as adding a new employee or a new product and updating a product by changing its available quantity.

The last step was implementing the FrontEnd of the web application using ASP.Net core mvc (model-view-controller). The main benefit of the web api already created that it can be directly used by the web application to access the database. Furthermore, if I wanted to create a mobile application using flutter for example, the same backend would be used. 
Languages used: HTML, CSS, C#, JavaScript.

A video is uploaded showing how the application can be used.
