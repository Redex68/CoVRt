# Instructions for running the game

The game itself is meant to be played on desktop. To play it, a python server needs to be run which communicates with the custom Arduino controller meant to be used by the operator player. The following two ways exist to boot up the game and server simultaneously:

1. By running the run.bat file located in the build folder

1. By executing the following bash command in the build folder: ./CoVRt.exe && python ContolPanelServer.py

# Required equipment

In order to play the game, a VR headset is required and SteamVR is needed to actually play the game in VR. The game has been tested with both Oculus and HTC Vive headsets, so it should work for either one (and most likely others, but with no guarantee). Furthermore, the game uses a custom Arduino controller which communicates with a python server in order to send its inputs to the game. In the repository, there is an instruction manual included. This is meant to be used while playing the game, serving as a help for the other player serving as an operator. It is recommended that this manual is printed out and used as a physical manual.

In case the controller doesn't work, use the following list in order to fix potential issues:
- If the game has been running and the controller suddenly doesn't work, the server may not be working. If this happens, tab out of the game and restart the server manually.
- If the controller doesn't work when starting up the game via either method described above, verify that the server is actually run. If it doesn't, make sure python is added to the PATH environment variable and that no libraries or modules are missing. 

NOTE: The python server assumes that the Arduino will be on COM3. However, even if this may be the case most of time, it isn't necessarily so.