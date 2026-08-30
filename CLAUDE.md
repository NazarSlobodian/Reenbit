# Claude Configuration for Reenbit Booking System

## Project Context
*   **Tech Stack:** ASP.NET Core, Azure SQL, Azure SignalR Service.
*   **Architecture:** Clean Architecture.
*   **Core Goal:** Implement strict concurrency control for meeting room bookings to prevent double-booking.

## Instructions for Claude
1.  **Code Quality:** Follow "Clean Code" principles.
2.  **Concurrency:** Never suggest naive "check then insert" logic. Always use explicit concurrency mechanisms (e.g., EF Core `RowVersion` optimistic concurrency).
3.  **Real-Time:** All successful booking operations must trigger a SignalR broadcast.
4.  **Commits:** Generate short, atomic commit messages detailing what changed and why.