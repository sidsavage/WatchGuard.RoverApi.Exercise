# WatchGuard.NASA.Api.Exercise
Configuration
    The following configuration can be set via appsettings.json
	 ApiKey, the key used to connect to the NASA Rover API
	 DataFolderName, the relative path to the parent folder of the file containing dates. This folder also controls where images are downloaded to.
	 DataFileName, the file name from which to read dates
	
Installation
	To run this application you must pull down the repository and either run it from visual studio, or build
	the application from source and run the WatchGuard.RoverApi.Exercise.exe
   
Operating instructions
	With default configuration, the 'dates.txt' can be modified to request additional rover
	images to be downloaded.
	Previously downloaded images will not be redownloaded and must be removed from the folder listed in 'DataFolderName'