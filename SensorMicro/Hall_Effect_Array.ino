String Hall_Effect_Array()
{
  String hall_array = "";
  for (int i = 0; i < H_NUM; i++)
  {
    if (mcp.digitalRead(HEpins[i]) == HIGH)
      hall_array += "1";
    else
      hall_array += "0";
  }
  return "<H"+ hall_array + ">";
}

