# FalloutVault

A SCADA/HMI system for managing and operating the critical and supplementary components of a Fallout Vault with the goal of providing complete control over an area and population.

## Features

### HMI Display

A multi-featured desktop application written in Avalonia .NET

### Devices

- Light controller
- Fan controller
- Speaker controller
- Crop sprinkler controller
- Vent seal controller
- Door controller
- Power controller

### Logging

All device events are accessible within the UI from the log pane and are also written to a log file.

## How to Run

1. Install [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

2. Clone the repository:
    ```sh
    git clone https://github.com/ModularMemory/CSCN72030-25F-Group1.git
    ```

3. Navigate to the cloned repository:
     ```sh
    cd CSCN72030-25F-Group1
    ```

4. Build the solution:
    ```sh
    dotnet build
   ```

5. Run the Console App:
    ```sh
    dotnet run FalloutVault.ConsoleApp
    ```

6. Run the UI App:
    ```sh
    dotnet run FalloutVault.AvaloniaApp
    ```

7. Run the tests:
    ```sh
    .\TestWithResults.bat
    ```