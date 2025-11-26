using System.Text.RegularExpressions;
using CSDLPT_API.Enums;

namespace CSDLPT_API.Utils;

public static class IdHelper
{
	private const int MaxIdLength = 5; // VARCHAR(5) trong database

	/// <summary>
	/// Tạo ID tự động với prefix server để tránh trùng lặp khi replication
	/// Format: {ServerChar}{Prefix}{Number} - Tổng độ dài <= 5 ký tự
	/// </summary>
	/// <param name="existingIds">Danh sách ID hiện có</param>
	/// <param name="prefix">Prefix gốc (ví dụ: "CLB", "GV", "SV", "LOP")</param>
	/// <param name="digits">Số chữ số cho phần số (sẽ tự động điều chỉnh nếu vượt quá maxLength)</param>
	/// <param name="serverEnum">Server hiện tại để thêm prefix</param>
	/// <returns>ID mới với format: {ServerChar}{Prefix}{Number} - Tổng độ dài <= 5 ký tự</returns>
	public static string GenerateNextId(IEnumerable<string> existingIds, string prefix, int digits, serverEnum serverEnum)
	{
		// Xác định ký tự server (1 ký tự để tiết kiệm không gian)
		string serverChar = serverEnum switch
		{
			serverEnum.ServerK1 => "1",
			serverEnum.ServerK2 => "2",
			serverEnum.OriginalServer => "M",
			_ => "M" // Mặc định là Main server
		};

		// Tạo full prefix: ServerChar + OriginalPrefix (ví dụ: "1CLB", "2GV", "MSV")
		string fullPrefix = serverChar + prefix;

		// Tính số digits tối đa có thể dùng (đảm bảo tổng độ dài <= MaxIdLength)
		int maxAvailableDigits = MaxIdLength - fullPrefix.Length;
		if (maxAvailableDigits <= 0)
		{
			throw new ArgumentException($"Prefix '{fullPrefix}' quá dài, không thể tạo ID với maxLength = {MaxIdLength}");
		}

		// Điều chỉnh digits nếu vượt quá giới hạn
		int actualDigits = Math.Min(digits, maxAvailableDigits);

		// Tìm số lớn nhất trong các ID có cùng full prefix
		var max = 0;
		foreach (var id in existingIds)
		{
			if (id != null && id.StartsWith(fullPrefix) && id.Length == MaxIdLength)
			{
				var numberPart = id.Substring(fullPrefix.Length);
				if (int.TryParse(numberPart, out var n))
				{
					if (n > max) max = n;
				}
			}
		}

		// Tạo ID mới
		var next = max + 1;
		
		// Kiểm tra xem số có vượt quá giới hạn không
		int maxNumber = (int)Math.Pow(10, actualDigits) - 1;
		if (next > maxNumber)
		{
			throw new InvalidOperationException($"Đã đạt giới hạn số lượng ID cho prefix '{fullPrefix}'. Số tối đa: {maxNumber}");
		}

		var formatted = next.ToString(new string('0', actualDigits));
		return fullPrefix + formatted;
	}

	/// <summary>
	/// Overload cũ để tương thích ngược (không khuyến khích sử dụng)
	/// </summary>
	[Obsolete("Sử dụng GenerateNextId với serverEnum để tránh trùng lặp khi replication")]
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


