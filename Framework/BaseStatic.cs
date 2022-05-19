using System;

namespace Framework
{
	static public class BaseStatic
	{
		static public readonly string DefaultColor = "Default";
		static public readonly string DefaultLayout = "1111111";
		static public readonly string DefaultFontEng = "'Verdana'";
		static public readonly string DefaultFontKor = "'Malgun Gothic'";
		static public readonly string DefaultFont = $"{DefaultFontEng},{DefaultFontKor}";

		static public readonly string DefaultDBConfigSection = "DataConfiguration";
		static public readonly string DefaultDBValueSection = "DefaultDatabase";
	}
}
