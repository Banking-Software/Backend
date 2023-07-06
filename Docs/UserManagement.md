## User Management

This software has divided users into two groups one is {+ SuperAdmin i.e Owner Of the Software +} and {+ Finance Company User i.e Sahakari's Employee +}

Both Users have different Database maintaied.

## SuperAdmin

Database : {+ SuperAdmin +}

When the application runs for the first time, a new user with Role 'SuperAdmin' is added in SuperAdmin Database with credentials: UserName:- Fintex, Password:- Fintex@123. 

The default Identity Table is created for the SuperAdmin.

Main focus table are AspNetUsers, AspNetRoles, AspNetUserRoles

# Responsibility Of SuperAdmin

1) Create every branch of the Sahakari Sastha.

2) Create a Admin i.e Officer of Sahakari Sastha with Login Credentials.

3) Control the Sahakari Users Login Functionality by altering Active field.


## FinanceCompany

Database: {+ Users +}

Inside this Table there are different tables, main focus table are:

- Employees: It Store the information of every users in the Sahakari Sanstha, including WatchMan also.

- AspNetUsers: It Store the information of certain employees that are given login access to the software.

- AspNetRoles: It stores the list of Roles that are present in the Sanstha. 'Officer', 'Senior Assistant', 'Assistant', 'Marketing'

- AspNetUserRoles: It stores the mapping of users to the role.

# Responsibility Of Admin i.e Officer

1) Create Employee and thier login credentials.

2) Update the Employee Details.


## Flow of User Management

1) SuperAdmin login to the software using provided credentials.

2) Create a Branch Name with mandatory field Branch Code.

3) Create a Admin i.e Officer under created branch name.

4) Officer will login to the software using SuperAdmin provided Credentials.

5) Officer first create a Employee and then if necessary a login credentials. By Default Login credentails is inactive

6) SuperAdmin needs to activate the User login


## Create Admin

SuperAdmin needs to provide certain information to create a Officer among which below are mandatory

-  Name, User Name, Email, Password, Confirm Password, Phone Number, Role, Is Active

## Create Employee

Officer needs to provide certain information to create a Employee among which below are mandatory

- "name": "string", "email": "string", "phoneNumber": "string", "branchCode": "string"

## Register Employee Credentials

Officer needs to provide certain information to create a Employee among which below are mandatory

- "userName": "string", "password": "string", "confirmPassword": "string", "email": "string", "role": 0

{+ NOTE: email of employee should match the email you are providing here +} 


## Edit Employee Information

Except the ID you can edit other fields

## Edit Registered User Info

Only Deposit Limit and Loan Limit are Editable