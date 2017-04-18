#include <Servo.h>
#include <Wire.h>
#include "Adafruit_MCP23017.h"

// Relay pins flipped 
#define P_U1_RX         0
#define P_U1_TX         1
#define P_U1_EStop      2
#define P_U1_LD         4
#define P_U1_RD         3

// Motor and brake pins flipped
#define P_U1_LM     6
#define P_U1_RM     5
#define P_U1_LB     8
#define P_U1_RB     7
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

// expansion board pins - precede with mcp.
#define P_U3_DIO1   10 // pc power in - checks if pc is on
#define P_U3_DIO2   11
#define P_U3_DIO3   12
#define P_U3_DIO4   13
#define P_U3_DIO5   14
#define P_U3_DIO6   15

#define L_NUM       8

#define RAMP_CONSTANT     1
#define HEALTH_CONSTANT   10
#define DEAD_ZONE         49
#define PRE_JUMP          15
#define SERIAL_WAIT       2
#define MAX_BUFFER        10
#define L_LOOP            2
#define RIGHT_CORRECTION  1
#define LEFT_CORRECTION   0

#define SERIAL_COMM_INIT  1000
#define UPDATE_LOOP       100

Adafruit_MCP23017 mcp;

Servo LeftMotor;
Servo RightMotor;

bool EStopped = false;

const int impact[L_NUM] = {P_U3_L1, P_U3_L3, P_U3_L5, P_U3_L7, P_U3_L2, P_U3_L4, P_U3_L6, P_U3_L8};

int LeftSpeed = 0;
int RightSpeed = 0;

bool LeftReverse = false;
bool RightReverse = false;

int LeftMotorValue = 0;
int RightMotorValue = 0;

bool RightMotorBrake = false;
bool LeftMotorBrake = false;

bool LeftRelayClosed = false;
bool RightRelayClosed = false;

bool prevSwitch;

int CommandHealth = -1;
int blinkCounter = 100;
int serialCommCounter = 0;
int impactLoopCounter = 0;
String msgBuffer = "";
int msgCounter = 0;
int serialCount = SERIAL_COMM_INIT;
bool pcConnect = false;
String prevIm = "", impactArray = "";
String Brakes;

String bBuff;

void setup()
{

  Serial.begin(115200);

  
  pinMode(P_U1_LD, OUTPUT);
  pinMode(P_U1_RD, OUTPUT);
  pinMode(P_U1_LB, INPUT_PULLUP);
  pinMode(P_U1_RB, INPUT_PULLUP);

  LeftMotor.attach(P_U1_LM);
  RightMotor.attach(P_U1_RM);

  pinMode(P_U1_I1, OUTPUT);
  pinMode(P_U1_I2, OUTPUT);
  pinMode(P_U1_I3, OUTPUT);
  pinMode(P_U1_I4, OUTPUT);
  pinMode(P_U1_I5, OUTPUT);


  pinMode(P_U1_PC, OUTPUT);
  pinMode(P_U1_EStop, INPUT);
  pinMode(P_U1_SW, INPUT_PULLUP);
  pinMode(P_U1_LED, OUTPUT);


  digitalWrite(P_U1_LED, HIGH);

  mcp.begin();

  mcp.pinMode(P_U3_DIO1, INPUT);

  for (int i = 0; i < L_NUM; i++)
  {
    mcp.pinMode(i, INPUT);
  }

  if (mcp.digitalRead(P_U3_DIO1) == LOW)
  {
    digitalWrite(P_U1_PC, HIGH);
    delay(250);
  }
  digitalWrite(P_U1_PC, LOW);

  if (digitalRead(P_U1_SW) == LOW)
  {
    prevSwitch = true;
  }
  else
  {
    prevSwitch = false;
  }

  bBuff = "";


}

void loop() {
  impactArray = Impact();
  //Brakes = Check_Brakes();
  
  if (digitalRead(P_U1_EStop) == HIGH)
  {
    EStop();
  }

  if (Serial.available() > 0)
  {
    SerialIn();
    //Serial.println("Loop is done");
  }

  SerialOut();
  RunMotors();

  if (serialCommCounter > 0) serialCommCounter--;
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













