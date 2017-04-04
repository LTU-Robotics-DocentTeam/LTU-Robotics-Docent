#include <Wire.h>
#include "Adafruit_MCP23008.h"

Adafruit_MCP23008 mcp;

//pins
const int HEpins[16] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}; // pins on expansion board
const int USping[5] = {2, 4, 6, 8, 12};
const int USecho[5] = {3, 5, 7, 9, 13};

//variables
int i = 0;

//serial
String signal, msg2pc = "";
const char USkey[5] = {'V', 'W', 'X', 'Y', 'Z'};

// function declarations

void setup() {
  Serial.begin(115200);
  pinMode(2, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(3, INPUT);
  pinMode(5, INPUT);
  pinMode(9, INPUT);
  pinMode(13, INPUT);
  // put your setup code here, to run once:
  mcp.begin();
}

void loop() {
  if (Serial.available() > 0)
  {
    signal = Serial.readString();
    while (true)
    {
      int startin = signal.indexOf('<');
      if (startin < 0)
      {
        break;
      }
      int endin = signal.indexOf('>');
      if (endin < 0)
      {
        break;
      }
      int msglngth = endin - startin;
      String msg = signal.substring(startin + 1, endin + 1);
      char key = msg[0];
      int value = msg.substring(1).toInt();
      //Serial.println(value);
      signal = signal.substring(endin + 1);
      //Serial.println(signal);

      switch (key)
      {
        case 'C':
          if (value == 0)
          {
            Serial.println("<C2>");
          }
          break;
      }
    }
    //msg = prevmsg;
  }
  String Hall_Array = Hall_Effect_Array();
  long Ultra_value = Ultra_Sensor(USping[i], USecho[i]);
  String Ultrasonic = "<" + (String)USkey[i] + (String)Ultra_value + ">";
  //  Old ultrasonic code
  //  long Ultrasonic_1 = Ultra_Sensor(2, 3); //Call for First Ultrasonic
  //  String Ultra_1 = "A";
  //  Ultra_1 += String(Ultrasonic_1);
  //  long Ultrasonic_2 = Ultra_Sensor( 4, 5); //Call for Second Ultrasonic
  //  String Ultra_2 = "B";
  //  Ultra_2 += String(Ultrasonic_2);
  //  long Ultrasonic_3 = Ultra_Sensor( 6, 7); //Call for First Ultrasonic
  //  String Ultra_3 = "C";
  //  Ultra_3 += String(Ultrasonic_3);
  //  long Ultrasonic_4 = Ultra_Sensor( 8, 9); //Call for First Ultrasonic
  //  String Ultra_4 = "D";
  //  Ultra_4 += String(Ultrasonic_4);
  //  long Ultrasonic_5 = Ultra_Sensor( 12, 13); //Call for First Ultrasonic
  //  String Ultra_5 = "E";
  //  Ultra_5 += String(Ultrasonic_5);
  Serial.println(Hall_Array + Ultrasonic);

  // Counter for ultrasonic sensor
  i++; // update every iteration
  if (i > 4) // as soon as it reaches 4 (i > 4), reset counter i
  {
    i = 0;
  }

}
