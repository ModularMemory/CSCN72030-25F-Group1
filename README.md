# FalloutVault

## How to Run

1. Install [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

2. Install and run Uno Check, accepting all fixes:
    ```sh
    dotnet tool install -g uno.check && uno-check
    ```
> [!NOTE]
> You may need to manually install several workloads in the Visual Studio Installer, such as _ASP.NET and web development_.

3. Clone the repository:
    ```sh
    git clone https://github.com/ModularMemory/CSCN72030-25F-Group1.git
    ```

4. Navigate to the cloned repository:
     ```sh
    cd CSCN72030-25F-Group1
    ```

5. Build the solution:
    ```sh
    dotnet build
   ```

6. Run the UI:
    ```sh
    dotnet run FalloutVault.UnoApp
    ```
