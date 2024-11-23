# EchoWrite 📝  
Welcome to **EchoWrite**, a modern blogging platform designed to provide a seamless and interactive experience for users to create, share, and manage content while fostering a vibrant community.

---

## 📖 About the Project  
**EchoWrite** is a full-featured blog application built using **ASP.NET MVC**. It enables users to:  
- Write, edit, delete, and manage their blog posts.  
- Engage with other users through comments, likes, and follows.  
- Explore content using powerful search and category filters.  
- Maintain a secure and user-friendly experience with robust account management.  

The admin area allows platform moderation, handling reported content, and managing categories.  

---

## 🌟 Features  
### For Users  
- **Post Management**: Create, edit, delete, and report posts.  
- **Engagement**: Like/dislike posts and comments, follow/unfollow users.  
- **Personal Profile**: Update personal details, view followers/following, and manage posts.  
- **Search**: Discover users via search functionality.  
- **Categorization**: Filter posts by categories.  

### For Admins  
- **Moderation**: Handle reported users, posts and comments.  
- **Management**: Control categories and tags for content organization.  

### Additional Features  
- **Security**: Robust authentication using ASP.NET Identity.  
- **Ease of Use**: Intuitive user interface and streamlined workflows.  
- **Modern Styling**: Clean and professional layout with dark and light theme options.  

---

## 🚀 Technologies Used  
- **Backend**: ASP.NET Core MVC, Entity Framework Core  
- **Frontend**: Bootstrap, jQuery, HTML5, CSS3  
- **Database**: SQL Server  
- **Authentication**: ASP.NET Identity  
- **Version Control**: Git, GitHub  

---
## 📹 EchoWrite Demo
Get started with EchoWrite by watching our introductory video:  

### User Login: 
https://github.com/user-attachments/assets/a1dff269-6b2f-49b1-a1b8-f7bf42f02ea4

https://github.com/user-attachments/assets/b04a15d6-bd33-46f8-9dad-5e49576b7bc9

### Admin Login: 
https://github.com/user-attachments/assets/7aac97ad-ff7d-4c21-97f4-a35517c28d06





---
## 🔧 Getting Started  

### Prerequisites  
Make sure you have the following installed:  
- [.NET SDK](https://dotnet.microsoft.com/download)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
- [Git](https://git-scm.com/)  

### Installation Steps  
1. **Clone the Repository**  
   ```bash  
   git clone https://github.com/LaraSamara/EchoWrite.git  
   cd EchoWrite

---
## 📂 Project Structure  

- **`DAL` (Data Access Layer)**  
  - Contains all domain models such as `ApplicationUser`, `Post`, `Comment`, `Like`, `Category`, and `Report`.  
  - Includes the database context (`DbContext`) and all database-related operations using Entity Framework Core.  

- **`BLL` (Business Logic Layer)**  
  - Encapsulates all query and business logic operations.  
  - Implements the **Repository Design Pattern** to separate data access logic from the controllers.  
  - Handles interactions between the `DAL` and the `PL`.  

- **`PL` (Presentation Layer)**  
  - Contains the `wwwroot` folder for static assets like CSS, JavaScript, and images.  
  - Includes the **Areas**:  
    - **`User`**: For user-specific features, such as profile management, creating posts, comments, likes, and following other users.  
    - **`Admin`**: For admin-specific features, including handling reports, managing categories.  
  - Hosts all main controllers, views, and Razor pages to create the user interface.  
  - Manages shared controllers, such as `AccountController` for identity-related actions and `HomeController` for Home pages.  

### **Architecture**  
The project follows a **N-tier Architecture**:  
1. **Presentation Layer (PL)**: Handles user interactions through areas, controllers, and views.  
2. **Business Logic Layer (BLL)**: Processes and validates business logic, ensuring a clear separation between the UI and database operations.  
3. **Data Access Layer (DAL)**: Manages all database-related tasks, including migrations and database context.  



---

## 🤝 Contributing  

Contributions are welcome!  

To contribute:  
1. Fork the repository.  
2. Create your feature branch: `git checkout -b feature/YourFeatureName`.  
3. Commit your changes: `git commit -m 'Add some feature'`.  
4. Push to the branch: `git push origin feature/YourFeatureName`.  
5. Open a pull request.  

For significant changes, please discuss them first by opening an issue.  

---

## 🛡️ License  

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.  

---

## 📧 Contact  

Developed by **[Lara Samara](https://www.linkedin.com/in/lara-samara/)**.  
For inquiries, contact me at **larasamara2002@gmail.com**.  

---

   

