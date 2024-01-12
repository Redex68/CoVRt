// ezButton - Version: Latest 
#include <ezButton.h>

int JoyXPin = 2, JoyYPin = 3, CamTurnPin = 0, CamFilterPin = 1;
int buttonDebounceTime = 30, switchDebounceTime = 30;

ezButton camButton(2);
ezButton floorSwitch(4);
ezButton joyPress(7);

void setup() {
  Serial.begin(9600);
  camButton.setDebounceTime(buttonDebounceTime);
  floorSwitch.setDebounceTime(switchDebounceTime);
  joyPress.setDebounceTime(buttonDebounceTime);
  pinMode(JoyXPin, INPUT);
  pinMode(JoyYPin, INPUT);
  pinMode(CamTurnPin, INPUT);
  pinMode(CamFilterPin, INPUT);
}

void loop() {
    camButton.loop();
    floorSwitch.loop();
    joyPress.loop();
    Serial.println(String(1 - floorSwitch.getState()) + "|" + String(analogRead(JoyXPin))+ "|" + String(analogRead(JoyYPin)) + "|" + String(1 - joyPress.getState()) + "|"+ String(analogRead(CamTurnPin)) + "|"+ String(analogRead(CamFilterPin)) + "|" + String(1 - camButton.getState()));
}
