namespace Verse;

public class LanguageWorker_Russian : LanguageWorker
{
	public override int TotalNumCaseCount => 3;

	public override string ToTitleCase(string str)
	{
		return GenText.ToTitleCaseSmart(str);
	}

	public override string Pluralize(string str, Gender gender, int count = -1)
	{
		if (str.NullOrEmpty())
		{
			return str;
		}
		if (!TryLookupPluralForm(str, gender, out var plural, count))
		{
			plural = PluralizeFallback(str, gender, count);
		}
		if (count == -1)
		{
			return plural;
		}
		if (TryLookUp("Case", str, 3, out var result) && TryLookUp("Case", plural, 3, out var result2))
		{
			return GetFormForNumber(count, str, result, result2);
		}
		return plural;
	}

	private string PluralizeFallback(string str, Gender gender, int count = -1)
	{
		char c = str[str.Length - 1];
		char c2 = ((str.Length >= 2) ? str[str.Length - 2] : '\0');
		switch (gender)
		{
		case Gender.None:
			switch (c)
			{
			case 'o':
				return str.Substring(0, str.Length - 1) + "a";
			case 'O':
				return str.Substring(0, str.Length - 1) + "A";
			case 'E':
			case 'e':
			{
				char value2 = char.ToUpper(c2);
				if ("ГКХЖЧШЩЦ".IndexOf(value2) >= 0)
				{
					switch (c)
					{
					case 'e':
						return str.Substring(0, str.Length - 1) + "a";
					case 'E':
						return str.Substring(0, str.Length - 1) + "A";
					}
				}
				else
				{
					switch (c)
					{
					case 'e':
						return str.Substring(0, str.Length - 1) + "я";
					case 'E':
						return str.Substring(0, str.Length - 1) + "Я";
					}
				}
				break;
			}
			}
			break;
		case Gender.Female:
			switch (c)
			{
			case 'я':
				return str.Substring(0, str.Length - 1) + "и";
			case 'ь':
				return str.Substring(0, str.Length - 1) + "и";
			case 'Я':
				return str.Substring(0, str.Length - 1) + "И";
			case 'Ь':
				return str.Substring(0, str.Length - 1) + "И";
			case 'A':
			case 'a':
			{
				char value = char.ToUpper(c2);
				if ("ГКХЖЧШЩ".IndexOf(value) >= 0)
				{
					if (c == 'a')
					{
						return str.Substring(0, str.Length - 1) + "и";
					}
					return str.Substring(0, str.Length - 1) + "И";
				}
				if (c == 'a')
				{
					return str.Substring(0, str.Length - 1) + "ы";
				}
				return str.Substring(0, str.Length - 1) + "Ы";
			}
			}
			break;
		case Gender.Male:
			if (IsConsonant(c))
			{
				return str + "ы";
			}
			switch (c)
			{
			case 'й':
				return str.Substring(0, str.Length - 1) + "и";
			case 'ь':
				return str.Substring(0, str.Length - 1) + "и";
			case 'Й':
				return str.Substring(0, str.Length - 1) + "И";
			case 'Ь':
				return str.Substring(0, str.Length - 1) + "И";
			}
			break;
		}
		return str;
	}

	private static bool IsConsonant(char ch)
	{
		return "бвгджзклмнпрстфхцчшщБВГДЖЗКЛМНПРСТФХЦЧШЩ".IndexOf(ch) >= 0;
	}
}
