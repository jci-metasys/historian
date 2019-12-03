# Historical Data Extractor

The Historical Data Extractor is a series of apps that extract historical data from the *Metasys* system into a data store of your choice and in a format that can be easily read by a BI tool such as Power BI or Tableau.

There are three apps included in this source code:

- [Quick Extract](#quick-extract)
- [Discovery](#discovery)
- [Extraction](#bulk-extraction)

There is also a mechanism in place for you to be able to add your own data store for the historical data. You will simply need to implement one interface to do so. Instructions on that can be found in the [Data Storage](#data-storage) section.

## Prerequisites

The three apps of the Historical Data Extractor require the following prerequisites prior to running the apps:

- Download and install [dotnet core](https://www.microsoft.com/net/learn/get-started/windows) on the client computer that you run the app from.
- Clone this repository.
- Credentials (username and password) to the *Metasys* system. We recommend a user with view-only permissions.
- Credentials to the SQL database (for the dbconnection string for #discovery and #extraction).

### Supported Versions

The Historical Data Extractor is only supported for *Metasys* release 10.0.

## Quick Extract

To run the Quick Extract app, run the command below in from the [HistoricalDataFetcher.QuickExtract](/HistoricalDataFetcher.QuickExtract) folder. The results for the last day are stored in the current directory in a new file named timeseries.csv.

```bash
dotnet run --host <server.com> --username <Metasys Username> --password <Metasys Password>
```

### CLI Options For Quick Extract

     -h, --host          Required. Base URL <server.com> of the Metasys Application
     -u, --username      Required. Username for the Metasys Application
     -p, --password      Required. Password for the Metasys Application
     -s, --service       (Default: time) Service to extract data from

## Discovery

The Discovery app processes the network tree from the *Metasys* and determines which API endpoints should be used for the extraction calls.

This app allows you to specify fully qualified references (FQRs) for a more focused extraction. Providing specific FQRs allows the Extractor app to gather historical data for only the specified objects instead of samples for every object in the tree.

The Discovery app inserts and converts FQRs into global unique identifiers (GUIDs), and inserts the EnumSet information into SQLServer. The Discovery app can also run independently from the Extractor app. You may want to re-run the Discovery app when the list of FQRs is updated or when a new object is added to the *Metasys* system.

To run the Discovery app, follow these instructions:

1. Open the DBScripts folder and run each script, in numerical order, on your instance of SQL Server.
2. Create a CSV file with a list of specific FQRs, with each FQR on its own line. This CSV file is used for the --fqrs CLI option.
3. From the [HistoricalDataFetcher.Discovery](/HistoricalDataFetcher.Discovery) folder, run the command below. The results for the last day are stored in the current directory in a new filed named timeseries.csv.

```bash
dotnet run --host <server.com> --username <Metasys Username> --password <Metasys Password>
[--dbconnection "<Database connection string>"] [--fqrs "<FQR full file path>"]
```

### CLI Options For Discovery

     -h, --host          Required. Base URL <server.com> of the Metasys Application
     -u, --username      Required. Username for the Metasys Application
     -p, --password      Required. Password for the Metasys Application
     -x, --dbconnection  Connection string required to connect to the desired DB
     -f, --fqrs          The absolute path to the file containing the fully qualified references
     -i, --invalidcert   Allow untrusted certificate when connecting to API, default to false if not entered.

## Bulk Extraction

The Extractor app creates jobs and adds tasks to the queue, then process the URLs from the task queue. The API endpoints are called and the data is saved to the data store. Run this app to get a large set of data or to process the data to a CSV file.

To run the Extractor app, follow these instructions:

1. From the [HistoricalDataFetcher.Console](/HistoricalDataFetcher.Console) folder, run the command below. Jobs and tasks are created and completed.
2. Monitor the jobs and tasks to ensure they complete successfully. The following numbers represent the status:

   - 0: Job Added
   - 1: Task Added
   - 2: Executing
   - 3: Success
   - 4: Error

   The results for the defined timeframe are stored in the defined destination (SQL Server or CSV).

```bash
dotnet run --host <server.com> --username <Metasys Username> --password <Metasys Password> [--service time[,alarm]]
[--dest sqlserver] [--dbconnection "<Database connection string>"] [--month <number of months back>] [--days <Number of Days back>] [--invalidcert <true/false>]
```

### CLI Options For Extraction

     -h, --host          Required. Base URL <server.com> of the Metasys Application
     -u, --username      Required. Username for the *Metasys* Application
     -p, --password      Required. Password for the *Metasys* Application
     -s, --service       (Default: time) Comma separated list of the service you wish to run ([Time][,Alarm]).  Minimum of 1 service is required")
     -d, --dest          (Default: SqlServer) The Destination the data should be saved to ({Csv} | {SqlServer})
     -x, --dbconnection  Connection string required to connect to the desired DB
     -D, --days          (Default: 0) The number of days you wish to query
     -M, --month         (Default: 0) The number of months you wish to query
     -i, --invalidcert   Allow untrusted certificate when connecting to API, default to false if not entered.

## Incremental Service (Windows Service)

The Incremental Service runs on a pre-set time interval, and functions as a bulk extraction app that creates jobs and tasks in the queue, and then process the jobs and tasks. Additionally, the Incremental Service collects the jobs that are incomplete or have not successfully executed and attempts to re-process them.

The Incremental Service is built with Topshelf. For more information on the Topshelf Project, click [here](http://topshelf-project.com/).

To install the Incremental Service, implement a secure security store first. For security reasons, username, password and DBConnectionString are not part of the appsettings.json file. You need to implement your own secure solution to provide this information in code. (The username and password can be entered in the IncrementalExtractionService.cs file.) Then in Command Prompt, navigate to the folder the app is built in, and run: 

```bash
HistoricalDataFetcher.WindowsService.exe install start --autostart
```

### Configuration (appsettings.json)

    Host:                Required. Base URL <server.com> of the *Metasys* Application
    Service:             (Default: time) Comma separated list of the service you wish to run ([Time][,Alarm]).  Minimum of one service is required")
    StartTime:           Earliest time this service will retrieve data from
    TimeIntervalInHours: Time interval in hours this service will run
    Destination:         The Destination the data should be saved to ({Csv} | {SqlServer})
    InvalidCertificate:  Allow untrusted certificate when connecting to API.

## Data Storage

### Overview

All of the concrete classes for the data stores are implementations of the `IDataStore` interface. The interface represents saving a single type of historical data (TimeSeries or Alarm) to the data store. Meaning that a separate implementation must be made for each historical data type you are looking to save into the data store.

For example, if you wanted to save the TimeSeries and Alarm data, you need to make two implementations of the `IDataStore`: one for the TimeSeries data and one for the Alarm data.

This gives you the flexibility to only implement what you need and the rest will be ignored by the code. Each implementation will also need to pass declare the type for `T` as well. The data models in the Models folder should be sufficient for saving the data but can also be modified as needed if more or less data is needed depending on the use case.

Everything having to do with the data storage is in the HistoricalDataFetcher.DataStorage project. The project is broken up into five folders:

- Alarms
- Interfaces
- Models
- TimeSeries

Each of the sections are covered in detail below:

### Alarms

The section that stores the implementation of the `IDataStore` interface for the Alarms data store. When using the --service (-s) from the CLI options, use `alarm` to gather alarms from this data store.

### Interfaces

The section that declares the `IDataStore` interface which is root object that all of the data stores implement.

### TimeSeries

The section that stores the implementation of the `IDataStore` interface for the TimeSeries data store. When using the --service (-s) from the CLI options, use `time` to gather trend data from this data store.

### Models

Declares the models that data store is expecting to use when storing the data.

### Implementing a custom data store

As described above, depending on your needs you can implement the `IDataStore` interface for one, two, or all three historical data types. Code elsewhere in the solution requires modification to account for your new changes as well. The following are the steps needed to add a new implementation of `IDataStore`:

1. Create the implementation of the interface(s) that you are looking to create. Best practice is to place the implementation into the same folder as the type (for example, an implementation for pushing Alarm data should be in the Alarms folder). Be sure to use the correct data model for your implementation as well so that the data is stored correctly.
2. Go to the HistoricalDataFetcher.Classes project, and then go to `Controller\Controller.cs`
3. In the `SetDataSaveDestination()` function, a case statement for `DestinationSave.Custom` with a `TODO:` comment exists. Set the data store variable(s) in that section to your custom implementation.
