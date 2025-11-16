# TechnoAvengers

A superhero encyclopedia web application built with ASP.NET Core Razor Pages, featuring ranking, quizzes, battle simulation, and full NUnit test coverage.

---

## Overview  
TechnoAvengers allows users to explore Marvel-style superheroes, view rankings, take quizzes, simulate battles, and toggle between light/dark themes.  
The app emphasizes clean architecture, custom UI styling, and high-quality testing.

### Key Features  
- Browse & edit superhero profiles  
- Sorting and ranking with ascending/descending toggle arrows  
- “Discover Your Avenger” personality quiz  
- Dynamic Knowledge Quiz with scoring  
- Hero vs. Hero Battle Simulation  
- Dark Mode toggle  
- Print/Download PDF of Rankings  
- Bootstrap 4 + custom CSS theme  
- NUnit unit tests with goal of 100% code coverage

---

## Tech Stack

### **Frontend**
- ASP.NET Core Razor Pages  
- Bootstrap 4  
- Custom CSS Theme (navbar, cards, dark mode)  
- JavaScript for sorting, quizzes, and popups  
- Google Fonts (Bangers)

### **Backend**
- ASP.NET Core (Razor Pages)  
- C#  
- Razor Page Model architecture  
- Service Layer (e.g., ProductService / HeroService)  
- Built-in Dependency Injection (DI)

### **Data**
- JSON file data source for superheroes  
  - Attributes: intelligence, strength, speed, durability, power, combat  
  - Roles, descriptions, ratings  

### **Testing**
- NUnit  
- Moq for mocking services  
- 100% coverage goal for:
  - Index  
  - Rank  
  - BattleModel  
  - Discover quiz  
  - FAQ and other pages  

### **Tools**
- .NET SDK  
- Visual Studio 2022  
- Git + GitHub  
- Windows development environment  
