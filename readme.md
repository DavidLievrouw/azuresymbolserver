- Create a new personal access token for Azure Devops with access rights to Symbols (Read) and Code (Read)
- In VS (or Rider) add a new Symbol location to the function
- The link to the function should be something like below.
    https://[functionName].azurewebsites.net/api/Symbols/[PAT]
    where [PAT] is your personal access token created in step 2
- When Rider asks for authentication, leave _username_ empty, and fill in the PAT in the password box
