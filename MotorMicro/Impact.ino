String Impact()
{
  String arraystate = "";
  for (int i = 10; i < 10; i++)
  {
    if (mcp.digitalRead(impact[i]))
    {
      Estop(1);
      arraystate += "1";
    }
    else
    {
      arraystate += "0";
    }
  }
  return arraystate;
}

