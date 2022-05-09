# Installation notes

1) Create a local user account as an admin

2) Install using ```installutil```
```
\windows\microsoft.net\framework64\v4.0.30319\installutil WatsonWindowsService.exe
```

Note: when the credential prompt pops up, and you specify the username, for a local user account called ```admin```, use ```.\admin```.

3) Start the service.
```
net start "Watson Server Service"
```

4) Test it.  It starts on ```127.0.0.1:8000```.  Logs are usually at ```C:\Windows\SysWOW64\watson.log```

5) Stop the service.
```
net stop "Watson Server Service"
```

6) Uninstall using ```installutil```
```
\windows\microsoft.net\framework64\v4.0.30319\installutil /u WatsonWindowsService.exe
```
