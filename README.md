# StringDataSorter

A high-performance .NET 9.0 solution for sorting large text files efficiently. The project leverages HPC# library for enhanced sorting capabilities and includes comprehensive benchmarking tools.

## Features

- High-performance string sorting using HPC# library
- Efficient file I/O with buffered operations
- Configurable through JSON settings
- Progress visualization with Spectre.Console
- Disk space validation before operations
- Asynchronous file operations
- Cancellation support for long-running operations

## Project Structure

The solution consists of four main projects:

### 1. StringDataSorter.Core
The core library that contains the fundamental sorting and file management functionality:
- `IDataManager` interface and `FileDataManager` implementation for file operations
- Efficient file reading/writing with buffering and ASCII encoding
- Disk space validation and directory management
- String data comparison and sorting algorithms

### 2. StringDataSorter.App
The main console application that provides a user interface for:
- Generating sample data files of specified sizes
- Reading and sorting text data
- Progress visualization using Spectre.Console
- Configuration management using appsettings.json

### 3. StringDataSorter.Benchmark
A dedicated project for performance benchmarking:
- Uses BenchmarkDotNet framework
- Measures sorting performance across different data sizes
- Helps identify performance bottlenecks
- Validates sorting algorithm optimizations

### 4. StringDataSorter.Tests
Unit tests project to ensure reliability:
- Tests for core sorting functionality
- File operation validations
- Data management tests
- Uses xUnit testing framework

## Dependencies

- .NET 9.0
- HPCsharp (v3.17.0) for high-performance computing
- Microsoft.Extensions.Configuration for settings management
- Spectre.Console for beautiful console UI
- BenchmarkDotNet for performance testing
- xUnit for unit testing

## Performance

The solution is optimized for performance through:
- Efficient buffering (8KB buffer size)
- ASCII encoding for text operations
- Parallel processing capabilities
- Memory-efficient string handling

## Getting Started

1. Clone the repository
2. Ensure .NET 9.0 SDK is installed
3. Build the solution
4. Run the StringDataSorter.App project to start sorting files
5. Use the Benchmark project to measure performance
6. Run tests to verify functionality

## How the Program Runs

The StringDataSorter application is designed to sort large text files efficiently. It leverages several key components to achieve this:

- **CreateDataAsync**: This method generates a sample data file of specified size. It uses the `FileDataManager` class to handle file operations, ensuring efficient buffered writes and ASCII encoding for optimal performance.

- **ReadDataAsync**: This method reads data from an existing file. It also utilizes the `FileDataManager` class to manage file reading with buffering, ensuring that the application can handle large files without consuming excessive memory.

- **WriteDataAsync**: This method writes sorted data back to a file. Similar to `CreateDataAsync`, it uses the `FileDataManager` class for efficient buffered writes and ASCII encoding.

- **SortMergePar**: This method sorts the data using a parallel merge sort algorithm provided by the HPC# library. The sorting is performed in parallel, which significantly improves performance when dealing with large datasets.

These components work together to provide a robust solution for sorting large text files efficiently, ensuring both performance and reliability.

## How to Run the Program

Here is an example of how to use the StringDataSorter application to sort a large text file:

### Example Usage

1. **Run the Application**
   ```bash
   dotnet run --project StringDataSorter.App
   ```

2. **Provide Input**
   - When prompted, enter the desired file size in MB. For example, `100` for a 100 MB file.

3. **Observe Progress**
   - The application will display progress for each step, including:
     - Creating the data file
     - Reading the data file
     - Sorting the data
     - Writing the sorted data to a new file

4. **View Results**
   - After completion, the application will display a summary of the time taken for each step in a bar chart format.

### Code Example

Below is a code snippet demonstrating the main steps of the program:

```csharp
var dataManager = new FileDataManager();
var data = DataSampler.Get();
var settings = ApplicationConfiguration.GetSettings();

// Create a data file
await dataManager.CreateDataAsync(data, settings.FilePath!, fileSize);

// Read data from the file
var dataNotSorted = await dataManager.ReadDataAsync(settings.FilePath!);

// Sort the data
var sortedData = dataNotSorted.SortMergePar<string>(comparer: new StringDataComparer());

// Write sorted data to a new file
await dataManager.WriteDataAsync(settings.SortedFilePath!, sortedData);
```

This example demonstrates how the application efficiently handles large text files, from data generation to sorting and saving.