<Query Kind="Program" />

bool IsNumber(string text)
{
  Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
  return regex.IsMatch(text);
}

void Main()
{
	String val = "Im your friendly #128label #33 ";
	val = val.Replace("'", "");
	
	int number = 0;
	while (val != null && val.IndexOf("#") > -1)
	{
		String asciiCode = null;
		int pos = val.IndexOf("#") + 1;
		while (IsNumber(val.Substring(pos, 1)))
		{
		  asciiCode += val.Substring(pos, 1);
		  pos++;
		  
		  if (pos >= val.Length)
		    break;
		}
		
		
		number = Convert.ToInt32(asciiCode);
		
		val = val.Replace("#" + number, Convert.ToChar(number).ToString());
		Console.WriteLine(asciiCode);
	//	val == null;
	}
	
	Console.WriteLine(number);
	Console.WriteLine(val);
	Console.WriteLine(Convert.ToChar(128).ToString());
}

// Define other methods and classes here