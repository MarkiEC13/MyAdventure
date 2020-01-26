# Choose your own adventure

<h2>Getting Started</h2>
<h3>Set up development environment</h3>
<ul>
  <li><a target="_blank" href="https://dotnet.microsoft.com/download">Install the .NET Core 3.1 SDK</a></li>
	</br>
   <li><a target="_blank" href="https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows">Install the Azure Function Core Tool</a></li>
	</br>
	<li><a target="_blank" href="https://angular.io/start">Install angular 8</a></li>
  	<li>Create <a target="_blank" href="https://github.com/MarkiEC13/MyAdventure/tree/master/GetChoicesFunction">local.settings.json</a> file and configure CosmosDb connection string as followns,
<pre>"DocumentDbSettings": {
   "Uri": "",
   "AuthKey": "",
   "DatabaseId": "",
   "MaxRetryAttemptsOnThrottledRequests": 2,
   "MaxRetryWaitTimeInSeconds": 4
}</pre></li>
</ul>

<h3>High-Level Architecture Design</h3>
