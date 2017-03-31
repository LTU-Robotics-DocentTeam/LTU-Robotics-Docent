#include <Servo.h>
#include <Wire.h>
#include "Adafruit_MCP23008.h"

Adafruit_MCP23008 mcp;

Servo rmotor, lmotor;

//pins
const int rmpin = 6, lmpin = 5;
const int impact[10] = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
const int rrelpin = 3, lrelpin = 4, testled = A3;
const int estopbtn = 2;

bool estop = false, rdir = false, ldir = false, prevrdir = false, prevldir = false;
int rmspeed = 0, lmspeed = 0;
const int maxcool = 10000;
int cooldown = maxcool;
String msg;
bool switcher = false, coolingdown = false, estopsig = true;

void setup() {
  Serial.begin(115200);
  rmotor.attach(rmpin);
  lmotor.attach(lmpin);
  mcp.begin();
  for (int i = 0; i < 10; i++)
  {
    mcp.pinMode(impact[i], INPUT);
  }
}

void loop() {
  for (int i = 10; i < 10; i++)
  {
    if (mcp.digitalRead(impact[i]))
    {
      Estop(1);
    }
  }
  if (digitalRead(estopbtn))
  {
    Estop(1);
  }
  if (!digitalRead(estopbtn) && estop)
  {
    Estop(0);
  }
  if (Serial.available() > 0)
  {
    msg = Serial.readString();
    while (true)
    {
      int startin = msg.indexOf('<');
      if (startin < 0)
      {
        break;
      }
      int endin = msg.indexOf('>');
      if (endin < 0)
      {
        break;
      }
      int msglngth = endin - startin;
      String msg = msg.substring(startin + 1, endin + 1);
      char key = msg[0];
      int value = msg.substring(1).toInt();
      //Serial.println(value);
      msg = msg.substring(endin + 1);
      //Serial.println(signal);

      switch (key)
      {
        case 'C':
          if (value == 0)
          {
            Serial.println("<C1>");
          }
          break;
        case 'R': // right motor input
          if (value < 0)
          {
            rdir = true;
            value = -1 * value;
          }
          else
          {
            rdir = false;
          }
          rmspeed = value;
          break;
        case 'L': // left motor input
          if (value < 0)
          {
            ldir = true;
            value = -1 * value;
          }
          else
          {
            ldir = false;
          }
          lmspeed = value;
          break;
        case 'F': estopsig = value;
          break;
        default:
          break;
      }
    }
  }
  if (!estop)
  {
    if (rdir != prevrdir)
    {
      rmotor.write(0);
      coolingdown = true;
      if (cooldown <= 0)
      {
        if (rdir) digitalWrite(rrelpin, HIGH);
        else digitalWrite(rrelpin, LOW);
        Serial.println("switch right");
        switcher = true;
        prevrdir = rdir;
      }
    }
    else
    {
      rmotor.write(rmspeed);
    }
    if (ldir != prevldir)
    {
      lmotor.write(0);
      coolingdown = true;
      if (cooldown <= 0)
      {
        if (ldir) digitalWrite(lrelpin, HIGH);
        else digitalWrite(lrelpin, LOW);
        Serial.println("switch left");
        cooldown = maxcool;
        switcher = true;
        prevldir = ldir;
      }
    }
    else
    {
      lmotor.write(lmspeed);
    }
    if (switcher) {
      cooldown = maxcool; switcher = !switcher;
    }
    if (coolingdown) {
      cooldown--; coolingdown = !coolingdown;
    }
  }
}
