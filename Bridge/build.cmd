dotnet publish -c Release -o publish -r win10-x64 /p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishReadyToRunShowWarnings=true --self-contained --force