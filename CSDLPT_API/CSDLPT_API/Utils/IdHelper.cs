using System.Text.RegularExpressions;

namespace CSDLPT_API.Utils;

public static class IdHelper
{
	public static string GenerateNextId(IEnumerable<string> existingIds, string prefix, int digits)
	{
		var max = 0;
		foreach (var id in existingIds)
		{
			if (id != null && id.StartsWith(prefix))
			{
				var numberPart = id.Substring(prefix.Length);
				if (int.TryParse(numberPart, out var n))
				{
					if (n > max) max = n;
				}
			}
		}
		var next = max + 1;
		var formatted = next.ToString(new string('0', digits));
		return prefix + formatted;
	}
}


