I recommend reading this file on [GitHub](https://github.com/Joey-Einerhand/HideNTiltLabyrinth)
# Hide 'n Tilt Labyrinth

A one-on-one hide and seek game with a twist. Made in the Unity Engine. 

In Hide 'n Tilt Labyrinth, a hider must try to not be caught by the seeker until the time is up.

HnTL uses Domoticz - typically used as Internet of Things software - to simulate back-end server-style information relay. The clients connect and send data to Domoticz using HTTPrequests.


## 1. Limitations

The game currently does not check if the server is full.
Domoticz is not the ideal server software for this type of game. It's not secure and very inefficient.
For more limitations, see the Issues tab at the Github repository.

## 2. Client installation

1. Download HideNTiltLabyrinth_vX.zip (Where X is the version number) from the releases page (GitHub) 
2. Unzip it somewhere on your desktop.
3. Launch Hide n Tilt Labyrinth.exe

## 3. Server installation

I do not recommend hosting your own server until the Domoticz username and password are no longer required, (or are encrypted) in order to log in to a server. Giving away your public IP and your domoticz username and password (which is required for clients to connect to your server) is a security risk. Anyone with these server details can log into your Domoticz application and potentially do harm to your server, your home network, and/or the device that's running the server.

1. Install [Domoticz](https://www.domoticz.com/)
2. Download the DomoticzServerBackup.db file
3. Launch Domoticz. 
4. A window should pop up asking for a username and password.
5. If you don't get the log in pop up but instead get an error, refresh the page or try again later.
6. Log in to Domoticz.  The default username is "DomoticzPublic" and the default password is "HideNTilt". You can edit this in your Domoticz settings. This is also the username and password which clients need to connect with your server.
7. Change your Domoticz default username and password. Not doing so is a huge security risk.
8. In Domoticz, select Setup, go to Settings, select Backup/Restore, and click "Restore Database"
9. Select the downloaded DomoticzServerBackup.db file and click "upload"
10. Wait until Domoticz finished installing the backup.
11. Port forward the Domoticz port. The default port is 8080. If you don't know how to port forward, look up your specific modem/router and add "How to port forward".
12. Share your public IPv4 address to whomever you want to have access to the server, together with your Domoticz username and password.

## 4. How to play
The gameplay in HnTL is devided into two roles; the hider and the seeker.
The game is simple: the seeker needs to catch the hider before the round timer reaches 0. 
The hider needs to tilt the board and go in a hole to hide need to make sure not to get caught before the round timer reaches 0. 
Extra gameplay depth is achieved movement sensors, which display the rough path the hider has travelled in the map to the seeker.

1. Launch the HnTL Client (Hide n Tilt Labyrinth.exe).
2. Find a server to play on (Or host your own).
3. Click on "Play".
4. Enter the server details of the server you want to play on.
5. Select a role to play (Ghunter or ghost)

### 4.1. Hider
As a hider, you need to hide from the seeker and make sure not to get captured before the timer runs out. If you prevented yourself from getting captured once the timer reaches 0, you win!

1. Commit to the steps as written in "How To Play", but pick the "seeker" role.
2. Wait until a player with the Ghunter role starts the round
3. You have 90 seconds to hide from the seeker. If you have not hidden from him within this time, you lose the game.
4. Potentially try to trick the seeker into thinking you went down a certain path by triggering certain distance sensors.
5. If you found a hole  you would like to hide in, proceed to tilt the board so the ball (your player character) falls into the hole.
6. You are now hidden. The seeker has 90 seconds to try and catch you.
7. Hope the seeker won't catch you!

### 4.2. Seeker
As the seeker, you need to capture the hider before the time runs out.

1. Commit to the steps as written in "How To Play", but pick the "seeker" role.
2. Wait for a hider to join the game.
3. Click the "Start Round" button to start the round.
4. Wait for 90 seconds while the hider get a chance to hide
5. Go close enough to a hole you'd like to check for a hider and press "E" to check for a hider.
6. Repeat until the timer runs out or you've found the hider.


## 5. Contributing
This game's source code is available on [GitHub](https://github.com/Joey-Einerhand/HideNTiltLabyrinth).


Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## 6. License
[GNU General Public License v3.0](https://choosealicense.com/licenses/gpl-3.0/)
