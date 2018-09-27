# HistoricalDataExtractor
This repository contains the Histrocial Data Extractor, which allows you to extract *Metasys* historical data. The Historical Data Extractor allows you to extract alarms and time series (points and samples) data, including supplemental data such as enumerations and equipment.

*Metasys* Relesae 10.0 includes several new web apps that serve new *Metasys* APIs, including the historical data APIs. The Historical Data Extractor facilitates the extraction and conversion of *Metasys* historical data to a format that is specified by the user. The Historical Data Extractor converts extracted *Metasys* specific data, such as enumarations, to a usable format for third-party reporting tools. For example, the Historical Data Extractor allows you to extract and convert *Metasys* historical data for use with Microsoft&reg; Power BI&trade;.

The Historical Data Extractor functions as a publicly viewable example of how to use the new *Metasys* APIs in *Metasys* Release 10.0. This repository, in addition to the *Metasys* API documentation, helps you understand the use and functionality of the APIs.

## Purpose
The purpose of the Historical Data Extractor is to extract historical data from the *Metasys* Server for a user-specified time range. The extracted historical data includes all alarms and time series data. The Historical Data Extractor also serves as a guide for customers to extract data on a regular schedule to run reports in third-party reporitng tools.

## Components

### Historical Data Extractor Application

#### Quick Start Application
The Quick Start Application is a proof-of-concept and user confidence application. The application requires the host name or IP address of the *Metasys* Server (the web/application computer in a split-configuration) and a *Metasys* username and password. The application gets a sample page of samples and save the data to a CSV file format. The output of the application is not meant for long-term use but instead acts as a proof-of-concept that data and the data schema can be extracted.

#### Discovery / Configuration Setup
The discovery step includes a text document that contains a list of Fully Qualified References (FQRs), >produced by the user, that the user wants to be included in the data extraction. The command line interface and Windows Service reads the FQRs from the configuration file and converts them to global unique identifiers (GUIDs), which are used when extracting data from the *Metasys* API end points.

#### Windows Service
The Windows Service is the background worker for extracting data. It takes in the parameters provided that you provide in the CLI and uses those parameters to extract data. It also creates a list of tasks, or endpoint pages to be requested and saved, based on the incremental duration that you specify. For every incremental duration iteration, the Windows Service (1) adds tasks into the TaskQ, (2) pulls each tasks from the TaskQ, (3) executes the task, (4) marks the task as completed, and then moves on to the next task. Once the full incremental duration TaskQ list is completed, the task list is purged. Once the next incremental duration is occurs, the Windows Service repopulates the TaskQ list with new endpoint pages to be requested and saved.

The Windows Service is a TopShelf installed service. By default, the Windows Serivce restarts after a crash or system reboot. It is responsible for creating single day extraction jobs to extract data starting at a user-define time up to three days from the current time. Doing so helps prevent any missed data coming from devices that have a delayed push to server or devices that were temporarily offline. Once a single day job is completed, the next day's job is added to the queue, but remains idle until its entire day's data is three days older than the current time. The Windows Service then executes that job and the process repeats.

If the extractor computer is offline for an extended period of time, when the computer starts up the next time, the Windows Service creates day jobs up to the three day from the current time, and executes those jobs to catch up on extracting data.

#### SQL Job Queue (JobQ)
The JobQ functions as a permanent record of all of the job/task lists that are complete or have failed. The JobQ also functions as a way for the Windows Service to remember where it left off in case of a crash or if the extractor computer restarts. The Windows Service adds jobs to the JobQ and takes from the top of the JobQ list. Each job is the result of a series of tasks entered into the TaskQ.

#### SQL Task Queue (TaskQ)
The TaskQ is the list of endpoints pages to be requested and saved in the SQL database. The TaskQ is the to-do list and a record of the calls that are completed and saved, as well as a place start from if the Historical Data Extractor crashes. The TaskQ purges after an incremental duration TaskQ list is complete. Once the next incremental duration is reached, a new list of endpoint pages to be requested is populated in the TaskQ. Once the TaskQ is populated by the Windows Service, the Windows Service starts pulling from the TaskQ.

TaskQ entries need to be unique in order to verify the extraction of the same pages is not repeated. The URL of the page to extract is unique by incorporating the ID of the object, the attribute ID, the time range, and the page number within the total sample collection to be extracted.

### Metasys Historical Databases

#### Alarms [JCIEvents]
This database contains *Metasys* Alarm data.

#### TimeSeries [JCIHistorianDB]
This database contains *Metasys* TimeSeries data, such as trend samples.

### Output File/Databases

#### Comma-Separated Values (CSV)

#### Microsoft SQL Server
Microsoft&reg; SQL Server&reg; software is a relational database management system developed by Microsoft. As a database server, it is a software product with the primary function of storing and retrieving data as requested by other software applications that may run either on the same computer or on another computer across a network (including the Internet).

https://www.microsoft.com/en-us/sql-server

### Reporting Tools

#### Power BI
Power BI is a business analytics service provided by Microsoft. It provides interactive visualizations with self-service business intelligence capabilities, where end users can create reports and dashboards by themselves, without having to depend on information technology staff or database administrators.

https://powerbi.microsoft.com/en-us/

#### Tableau
Tableau is a interactive data visualization product focused on business intelligence. Tableau queries relational databases, OLAP cubes, cloud databases, and spreadsheets and then generates a number of graph types.

https://www.tableau.com/

### *Metasys* API Documentation
https://github.jci.com/pages/g4-metasys-server/api/alderaan.html

## Potential Problems

### Mid Extraction Crash Requiring Restart
The Historical Data Extractor has the potential of crashing mid-extraction. This issue can result in potential lose of data for the extracted data (but not within the *Metasys* system). This issue can also cause the Historical Data Extractor to lose its place in the work.

To mitigate this issue, the JobQ and TaskQ exist to serve as a to-do list, as well as a record of what endpoint pages have been requested and saved.

### Duplicate Insert
The Historical Data Extractor may retrieve the same sample multiple times due to the overlap of incremental durations or if the extractor crashes and the same page is retrieved again.

To mitigate this issue, samples are added to a temporary insert table and then a comparison of rows is done with the master set of samples to eliminate duplicate insertion.

### Initial Extraction of Large Data Set
The initial set of TaskQ entries, or pages, to be extracted may be extensive. The initial extraction may take an extended period of time and has a high potential of crashing if system resources are consumed during the duration of extraction.

To mitigate this issue, the initial extraction of data should be limited to a smaller set of objects specified in the configuration file. Reducing the numbers of objects reduces the total amount data, number of pages to be extracted, and the total duration of initial extraction.

### User Intervenes with FQR Changes Mid-Extraction
There is a possiblity of a user entering new FQRs to extract while the TaskQ currently has tasks to run, meaning it has extractions to run on the previous set of FQRs. 

To mitigate this issue, the previous list of TaskQ tasks completes and then the new list of FQRs are imported into the SQL database for storage and then Discovery.

## Technical Information

### Deployment
The Historical Data Extractor solution is available at Johnson Controls, plc GitHub organization. You can clone or fork the repo for consumption. You can create pull requests, report issues, and directly communicate with the extractor development team.

### Enumerations
The entire list of *Metasys* enumerations used by the extractor are propagated into a SQL table at the initial launching of the CLI. This SQL table is used for creating an enumeration cache when exporting to CSV and for enumeration resolution when storing samples.

Enumeration set IDs and memeber IDs are meaningless data outside of the *Metasys* system. This data needs to be stored in a way so that the you can derive meaning from and report on the data.

Enumeration entries are indexed on the enumeration set ID and memeber ID. This combination is unique to each entry.

### User Permissions
We recommend creating *Metasys* user with the lowest available permissions and privileges for use with the Historical Data Extractor. This *Metasys* user can be used by the Historical Data Extractor to access *Metasys* API endpoints.
