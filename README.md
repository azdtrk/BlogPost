
# Project Definition, Purpose and Need?
- Application consists of mainly two part: one is the BackOffice where only Admin user can login and use to Post some BlogPost about any topic and the second part is MainUI where readers (normal users) can use to read the blogposts.
- Author (Admin) can keep the unfinished blogpost in draft mode and revisit each of them as much as they would want. And once it's ready to be published it can be seen on the MainUI where users can access to read it.
- Other users (Users in Reader role) can leave a comment as much as they would like under the blogpost. Once a comment submitted, it will be sent under review of the Author.   
- Author can review and list all af the comments submitted for any BlogPost, and if he/she approved a comment, it can be published and visible under the Post that it's been sent to.
- Developing this kind of an application serves many purpose at once: Having an application which can be used for taking notes for someone who likes to study taking notes, revisiting them and study again, updating their notes, will give the ability to keep track of their learnings.
- Having an application where people can make arguments, share thier opinion in every possible aspects about the Post will give Author the opportunity of growth on that topic.
- As the developer of the project, I adopt the principle of "Learn by Doing" in terms of applying the best practices of developing both BackEnd and FrontEnd applications as much as I can, styaying in the project's requirement boundaries.

# Project Structure:

At the Core Layer:
- Application project which defines the Abstractions in terms of Data Access Repositories and services
- Domain Project which holds the definitions of the entities in the system

At the Infrastructure Layer:
- Persistance project which implements the repositories defined in application layer to create a gradually-increasing dependencies which leads to loosely-coupled layers, instead of highly-coupled systems where most of the implementations occur on the lower levels.
- Database operations and context configurations.
  
Client Applications/Consumers
- Three types of UI project which are: BackOfficeUI, MainUI and SharedUI(Commonly used HTTP services, Helpers, Notification Services etc) will be implemented later after BackEnd functionalities fully covered.

Why CQRS: Seperating Commands and Queries has several benefits such as:
- It gives the flexibility when it comes to optimizing and scaling the traffic between the Client and Server. We may consider seperating our write database and read database. Since it's a BlogPost website most of the database accesses will be read-heavy use cases. As the audience grows, so the need for seperating the read and write servers, that's why CQRS might be a good idea.
- Another benefit is the ability to do all the mappings and conversions between actual models and DTOs in the Application layer which Clean Code/Clean Architecture principles advise us to do. CQRS fits Onion Architecture perfectly well in this manner.


                                                                             


# P.S.
- Everthing in this repository will be developed and maintained, including the README.md :)
