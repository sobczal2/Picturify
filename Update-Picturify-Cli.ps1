cd Sobczal.Picturify.Cli
dotnet build
dotnet pack
dotnet tool update --tool-path c:\dotnet-tools --add-source ./nupkg Sobczal.Picturify.Cli
cd ..