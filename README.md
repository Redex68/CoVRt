# Instructions for running the game

The game itself is meant to be played on desktop. To play it, a python server needs to be run which communicates with the custom Arduino controller meant to be used by the operator player. The following two ways exist to boot up the game and server simultaneously:

1. By running the run.bat file located in the folder with the release files
2. By executing the following bash command in the folder with the release files: `./CoVRt.exe && python ContolPanelServer.py`

Note: The run.bat file is supposed to be used with the release and not in the source files.

# Controller

The custom Arduino consists of 5 components: one switch, one joystick, two dials and one button (appearing in that order on the controller from left to right). The switch flips map view between the first and second floors. The joystick and button are used to switch between cameras, where the joystick controls a pointer on the currently selected camera and where the button allows you to switch to the camera which the pointer points to. The first dial controls the CCTV camera view, allowing you to pan the camera to the left and right. The second dial controls the signal of the camera feed from the CCTV cameras.

# Required equipment

In order to play the game, a VR headset is required and SteamVR is needed to actually play the game in VR. The game has been tested with both Oculus and HTC Vive headsets, so it should work for either one (and most likely others, but with no guarantee). Furthermore, the game uses a custom Arduino controller which communicates with a python server in order to send its inputs to the game. In the repository, there is an instruction manual included. This is meant to be used while playing the game, serving as a help for the other player serving as an operator. It is recommended that this manual is printed out and used as a physical manual.

In case the controller doesn't work, use the following list in order to fix potential issues:
- If the game has been running and the controller suddenly doesn't work, the server may not be working. If this happens, tab out of the game and restart the server manually.
- If the controller doesn't work when starting up the game via either method described above, verify that the server is actually run. If it doesn't, make sure python is added to the PATH environment variable and that no libraries or modules are missing. 

NOTE: The python server assumes that the Arduino will be on COM3. However, even if this may be the case most of time, it isn't necessarily so.
