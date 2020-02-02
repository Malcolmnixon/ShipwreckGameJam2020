# ShipwreckGameJam2020
Shipwreck game for the Game Jam 2020 competition

![Screen Shot](https://github.com/Malcolmnixon/ShipwreckGameJam2020/blob/master/Screenshots/Shipwreck_Screen1.jpeg?raw=true)

# Building Shared Code
Execute the build.bat or build.sh in the Common folder. This will:
* Pull down any NuGet dependencies
* Build the Shipwreck .NET Standard library
* Build a network server console application
* Copy the .NET libraries into the Unity assets

# Building Unity Client
After building the shared code, open the Shipwreck folder using Unity 2019.3.0f3 or newer and build.

# New Network Server
In order to create a new network server:
* Run the Shipwreck server on a machine with a static IP address
* Modify the Common\Shipwreck\WebClientWorld.cs to contain the new IP address
