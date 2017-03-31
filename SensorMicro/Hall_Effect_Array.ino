// reads all the incoming boolean data from the array of hall effects and creates a binary string to send back to the computer
// INPUT: Hall effect array pins (HEpins) from global variables
// OUTPUT:Binary string representing which sensors are triggered in the array 

String Hall_Effect_Array()
{
  String hall_array = "";
  for (int i = 0; i < 16; i++)
  {
    if (mcp.digitalRead(HEpins[i]) == HIGH)
      hall_array = "1";
    else
      hall_array = "0";
  }
  return "<H"+ hall_array + ">";
}

