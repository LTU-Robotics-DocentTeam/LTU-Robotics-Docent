#include <Adafruit_MCP23017.h>
#include <Wire.h>


#define P_U2_RX       0
#define P_U2_TX       1

#define P_U2_US1_P    2
#define P_U2_US1_E    3
#define P_U2_US2_P    4
#define P_U2_US2_E    5
#define P_U2_US3_P    6
#define P_U2_US3_E    7
#define P_U2_US4_P    8
#define P_U2_US4_E    9
#define P_U2_US5_P    12
#define P_U2_US5_E    13

#define P_U2_PWM1     10
#define P_U2_PWM2     11

#define P_U2_BattVolt A1
#define P_U2_ADC1     A2
#define P_U2_ADC2     A3
#define P_U2_ADC3     A4
#define P_U2_SDA      A5
#define P_U2_SCL      A6

#define P_Hall_H1     0
#define P_Hall_H2     1
#define P_Hall_H3     2
#define P_Hall_H4     3
#define P_Hall_H5     4
#define P_Hall_H6     5
#define P_Hall_H7     6
#define P_Hall_H8     7
#define P_Hall_H9     8
#define P_Hall_H10    9
#define P_Hall_H11    10
#define P_Hall_H12    11
#define P_Hall_H13    12
#define P_Hall_H14    13
#define P_Hall_H15    14
#define P_Hall_H16    15

#define U_NUM         5
#define H_NUM         16

Adafruit_MCP23017 mcp;

//pins
const int HEpins[H_NUM] = {P_Hall_H1, P_Hall_H2, P_Hall_H3, P_Hall_H4, P_Hall_H5, P_Hall_H6, P_Hall_H7, 
                        P_Hall_H8, P_Hall_H9, P_Hall_H10, P_Hall_H11, P_Hall_H12, P_Hall_H13, P_Hall_H14, 
                        P_Hall_H15, P_Hall_H16}; // pins on expansion board
const int USping[U_NUM] = {P_U2_US1_P, P_U2_US2_P, P_U2_US3_P, P_U2_US4_P, P_U2_US5_P};
const int USecho[U_NUM] = {P_U2_US1_E, P_U2_US2_E, P_U2_US3_E, P_U2_US4_E, P_U2_US5_E};

//variables
int i = 0;
int USprev[U_NUM] = {0, 0, 0, 0, 0};

//serial
String msg2pc = "";
const char USkey[U_NUM] = {'V', 'W', 'X', 'Y', 'Z'};
String prevHe = "", prevVolt = "";

// function declarations

void setup() {
  Serial.begin(115200);
  for (int i = 0; i < U_NUM; i++)
  {
    pinMode(USping[i], OUTPUT);
    pinMode(USecho[i], INPUT);
  }
  mcp.begin();
  for (int i = 0; i < H_NUM; i++)
  {
    mcp.pinMode(HEpins[i], INPUT);
    mcp.pullUp(HEpins[i], HIGH);
  }
}

void loop() {
  if (Serial.available() > 0)
  {
    SerialIn();
  }
  SerialOut(i);

  // Counter for ultrasonic sensor
  i++; // update every iteration
  if (i > 4) // as soon as it reaches 4 (i > 4), reset counter i
  {
    i = 0;
  }

}
