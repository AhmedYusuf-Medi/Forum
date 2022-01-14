# **Forum**

# **:trophy: Award**
>:rocket: Best forum project for Telerik Academy C# A31 - [Mercell](https://www.negometrix.com)

## Project Description
>#### Dungeon full of final bosses where you can communicate with post, comments and likes. But not a normal RPG dungeon, whenever you log into the forum you get +10 programming skills!
> _I know that you said EPIC :space_invader: ¿_

> Forum is an online discussion site where people can hold conversations by posting, commenting, and liking messages.

## Developing process
> :memo: Issue Board - [Board](https://gitlab.com/veselin-ahmed1/forum/-/boards/3501326)

> :memo: Issue List - [List](https://gitlab.com/veselin-ahmed1/forum/-/issues)

### Dashboard
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638959725/Gitlab-readme/Forum-Dashboard_tpggds.png)
### Login
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638713224/Gitlab-readme/Login-Forum_eows8v.png)
### Register
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638713396/Gitlab-readme/Forum-Register_labset.png)
### Create Post Form
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638713500/Gitlab-readme/Crete-Post-Forum_padwux.png)
### Posts overview and side cards
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638959955/Gitlab-readme/Post-Overview-Sidecards_afkvks.png)
### Selected post with create comment form in it
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638960129/Gitlab-readme/Selected-Post-W-Comments_llgdca.png)

---

## Features - MVC

> ### View - 1  - (visible to everyone)
```sh
> The home page without sing in gives us the options to login, register, search and filter posts too. 
> Upon registration you will be asked to confirm your credentials by accepting the confirmation email.
```
> ### View - 2 - (visible only to logged users)
FOR USERS
```sh
> They can view and edit there profiles.
> They can create new posts, comments or like.
> They can also delete or edit they own posts, comments or likes.
> They can view there own top 5 most recently posts in card named "My Active Posts". 
```
> ### View - 3 - (visible only to admins)
FOR ADMINS
```sh
> They have an extra view, dashboard that displays all users and categories, have an search option in it 
  also to block and unblock users or to create new category or edit already existing category.
```
## Features - API

> API DOCUMENT USING SWAGGER!

![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638960849/Gitlab-readme/Forum-Swagger_glklm6.png)

> ### Accounts (used for registration and login mainly)
```sh
> GET: "/api/Accounts/login" => Used to login.
> POST: "/api/Accounts/register" => Used to register in our forum.
> PUT: "/api/Accounts/{id}" => Used to edit users profile and upload avatar.
> GET: "/api/Accounts/logout" => Used to logout.
> GET: "/api/Accounts/verification" => Used to get pending user email and verification code to accept him as our homie.
```
> ### Users (used to work with users)
```sh
> GET: "/api/Users/{id}" => Returns user selected by id.
> DELETE: "/api/Users/{id}" => Deletes user selected by id.
> GET: "/api/Users" => Returns all users.
> PATCH: "/api/Users/block/{id}" => Used block user selected by id.
> PATCH: "api/Users/unblock/{id}" => Used unblock user selected by id.
> GET: "/api/Users/searchby" => Search in users matches from selected criterias.
```
> ### Categories (used to work with categories)
```sh
> GET: "/api/Categories/{id}" => Returns category selected by id.
> DELETE: "/api/Categories/{id}" => Deletes category selected by id.
> PUT: "/api/Categories/{id}" => Used to edit category selected by id.
> GET: "/api/Categories" => Returns all existing categories.
> POST: "/api/Categories" => Used to create new category.
> GET: "/api/Categories/sortby" => Sorts categories by selected criterias.
```
> ### Posts (used to work with posts)
```sh
> GET: "/api/Posts/{id}" => Returns post selected by id.
> DELETE: "/api/Posts/{id}" => Deletes post selected by id.
> PUT: "/api/Posts/{id}" => Used to edit post selected by id.
> GET: "/api/Posts/count" => Returns post count.
> GET: "/api/Posts/{id}/likes/count" => Returns selected post likes count.
> GET: "/api/Posts/{id}/comments/count" => Returns selected comments count.
> GET: "/api/Posts" => Returns all existing posts.
> POST: "/api/Posts" => Used to creates new post.
> GET: "/api/Posts/filter" => Filters posts by selected criterias.
> GET: "/api/Posts/sortby" => Orders posts by selected criterias.
```
> ### Comments (used to work with comments)
```sh
> GET: "/api/Comments/{id}" => Returns comment selected by id.
> DELETE: "/api/Comments/{id}" => Deletes comment selected by id.
> PUT: "/api/Comments/{id}" => Used to edit comment selected by id.
> GET: "/api/Comments" => Returns all existing posts.
> POST: "/api/Comments" => Used to creates new comment.
> GET: "/api/Comments/filter" => Filters comments by selected criterias.
> GET: "/api/Comments/sortby" => Orders comments by selected criterias.
```
> ### Post Likes (used to work with post likes)
```sh
> POST: "/api/PostLikes" => Creates post like if its not already, deletes it if its created and undelete it if the like was deleted before.
> DELETE: "/api/PostLikes/{id}" => Used to delete post like selected by id.
```
> ### Comment Likes (used to work with comment likes)
```sh
> POST: "/api/CommentLikes" => Creates comment like if its not already, deletes it if its created and undelete it if the like was deleted before.
> DELETE: "/api/CommentLikes/{id}" => Used to delete comment like selected by id.
```
> ### Reports (used to work with reports)
```sh 
> GET: "/api/Reports" => Returns all existing reports.
> GET: "/api/Reports/filter" => Filters reports by selected category.
> POST: "/api/Reports" => Used to create new report.
> DELETE: "/api/PostLikes/{id}" => Used to delete report selected by id.
```
> ### Post Reports (used to work with post reports)
```sh
> GET: "/api/PostReports" => Returns all existing post reports.
> POST: "/api/PostReports" => Used to create new post report.
> DELETE: "/api/PostReports/{postId}/{reportId}" => Used to delete post report selected by post and report Id.
```
> ### Comment Reports (used to work with comment reports)
```sh
> GET: "/api/CommentReports" => Returns all existing comment reports.
> POST: "/api/CommentReports" => Used to create new comment report.
> DELETE: "/api/CommentReports/{postId}/{reportId}" => Used to delete comment report selected by comment and report Id.
```
---
## Installation
> If you wish to clone the app follow the steps below
```sh
> Download the app from the repository
> Add the connection string to your database
> Update database
> Run the application. The database will be automatically created
```
### Database Diagram
---
![N|Solid](https://res.cloudinary.com/ddipdwbtm/image/upload/v1638864917/Gitlab-readme/Forum-Database-Diagram_a9bllv.png)

## Technologies used
As a good average Sabaton enjoyers we choosed following technologies in the development: 
 - ASP.NET Core
 - Microsoft Entity Framework Core
 - MSSQL
 - Moq
 - Postman
 - Swagger
 - SQLite InMemory
 - AJAX
 - JQuery
 - Cloudinary
 - View Components
 - HTML
 - CSS
 - MailKit

## Contacts

Contact us if you dear .NET enjoyer have any questions to ask:

| Contacts | Email | LinkedIn |
| ------ | ------ | ------ |
| Ahmed Yusuf | ahhmed.usuf@gmail.com | https://www.linkedin.com/in/ahmed-yusuf-0a22b1200/ |
| Veselin Bozhilov | vesko.tech@gmail.com |  |
