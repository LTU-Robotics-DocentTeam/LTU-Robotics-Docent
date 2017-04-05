String Hall_Effect_Array()
{
  String hall_array = ""; // create empty string
  for (int i = 0; i < H_NUM; i++) // fill in string with individual sensor states in binary
  {
    if (mcp.digitalRead(HEpins[i]) == HIGH)
      hall_array += "1"; // if sensor is triggered, add "1" to string
    else
      hall_array += "0"; // if sensor is not triggered add "0" to string
  }
  return "<H"+ hall_array + ">"; // return code to send through serial
}

