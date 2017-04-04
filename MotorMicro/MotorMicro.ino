#include <Servo.h>
#include <Wire.h>
#include "Adafruit_MCP23017.h"

#define P_U1_RX         0
#define P_U1_TX         1
#define P_U1_EStop      2
#define P_U1_LD         3
#define P_U1_RD         4

#define P_U1_LM     5
#define P_U1_RM     6
#define P_U1_LB     7
#define P_U1_RB     8
#define P_U1_I1     9
#define P_U1_I2     10
#define P_U1_I3     11
#define P_U1_I4     12
#define P_U1_I5     13

#define P_U1_PC     A0
#define P_U1_OUT2   A1
#define P_U1_SW     A2
#define P_U1_LED    A3
#define P_U1_SDA    A4
#define P_U1_SCL    A5

#define P_U3_L1     0
#define P_U3_L2     1
#define P_U3_L3     2
#define P_U3_L4     3
#define P_U3_L5     4
#define P_U3_L6     5
#define P_U3_L7     6
#define P_U3_L8     7
#define P_U3_L9     8
#define P_U3_L10    9

#define P_U3_DIO1   10
#define P_U3_DIO2   11
#define P_U3_DIO3   12
#define P_U3_DIO4   13
#define P_U3_DIO5   14
#define P_U3_DIO6   15

#define RAMP_CONSTANT     1
#define HEALTH_CONSTANT   10

Adafruit_MCP23017 mcp;

Servo LeftMotor;
Servo RightMotor;

bool EStopped = false;

int impact[8] = {P_U3_L1, P_U3_L2, P_U3_L3, P_U3_L4, P_U3_L5, P_U3_L6, P_U3_L7, P_U3_L8};

int LeftSpeed = 0;
int RightSpeed = 0;

bool LeftReverse = false;
bool RightReverse = false;

int LeftMotorValue = 0;
int RightMotorValue = 0;

bool LeftRelayClosed = false;
bool RightRelayClosed = false;

int CommandHealth = -1;
int blinkCounter = 100;

void setup()
{

  Serial.begin(115200);

  
  pinMode(P_U1_LD, OUTPUT);
  pinMode(P_U1_RD, OUTPUT);
  pinMode(P_U1_LB, INPUT);
  pinMode(P_U1_RB, INPUT);

  LeftMotor.attach(P_U1_LM);
  RightMotor.attach(P_U1_RM);

  pinMode(P_U1_I1, OUTPUT);
  pinMode(P_U1_I2, OUTPUT);
  pinMode(P_U1_I3, OUTPUT);
  pinMode(P_U1_I4, OUTPUT);
  pinMode(P_U1_I5, OUTPUT);


  pinMode(P_U1_PC, OUTPUT);
  pinMode(P_U1_EStop, INPUT);
  pinMode(P_U1_SW, INPUT);
  pinMode(P_U1_LED, OUTPUT);


  digitalWrite(P_U1_LED, HIGH);


}

void loop() {
  String ImpactArray = Impact();
  
  if (digitalRead(P_U1_EStop) == HIGH)
  {
    EStop();
  }

  if (Serial.available() > 0)
  {
    SerialIn();
    Serial.println("Loop is done");
  }

  RunMotors();


  blinkCounter--;

  if(blinkCounter == 50)
  {
    digitalWrite(P_U1_LED, HIGH);
  }

  if(blinkCounter == 0)
  {
    digitalWrite(P_U1_LED, LOW);
    blinkCounter = 100;
  }

  delay(10);
}













