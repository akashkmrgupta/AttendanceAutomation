using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class AttendanceAutomation
{
	private static readonly HttpClient client = new HttpClient();

	// Replace with actual API URL
	private const string apiUrl = "https://hrms-india.com:8585/ess/Emp/TourEntry.aspx";

	public async Task MarkAttendance(DateTime startDate, DateTime endDate)
	{
		// Predefined holidays and leaves (you can load these from a config or database)
		List<DateTime> holidays = new List<DateTime>
		{
			new DateTime(2024, 10, 5), // Example holiday
			new DateTime(2024, 10, 15) // Another holiday
		};

		List<DateTime> userLeaves = new List<DateTime>
		{
			new DateTime(2024, 10, 8)  // Example leave
		};

		Console.WriteLine("| Mon | Tue | Wed | Thu | Fri | Sat | Sun |");
		Console.WriteLine("------------------------------------------");

		// Iterate through all dates from startDate to endDate
		for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
		{
			if (date.DayOfWeek == DayOfWeek.Monday)
			{
				Console.WriteLine(); // Start a new row (week)
			}

			// Skip weekends
			if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
			{
				Console.Write($"| W   "); // Mark as weekend
				continue;
			}

			// Skip holidays
			if (holidays.Contains(date))
			{
				Console.Write($"| H   "); // Mark as holiday
				continue;
			}

			// Skip user leaves
			if (userLeaves.Contains(date))
			{
				Console.Write($"| L   "); // Mark as leave
				continue;
			}

			// Add key-value pairs for form data
			var values = new Dictionary<string, string>
		{
			{ "ctl00$ctl00$CPH$CPH$cmbMonth", date.Month.ToString() },
			{ "ctl00$ctl00$CPH$CPH$txtFromDate", date.ToString("dd/MM/yyyy") },
			{ "ctl00$ctl00$CPH$CPH$txtToDate", date.ToString("dd/MM/yyyy") },
            // Add other necessary form data here
        };

			// Add query parameters
			var requestUri = $"{apiUrl}?type=secret";

			// Add headers
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.Add("host", "hrms-india.com");
			client.DefaultRequestHeaders.Add("Accept", "*/*");
			client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"129\", \"Not=A?Brand\";v=\"8\", \"Chromium\";v=\"129\"");
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36");
			client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
			client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
			client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
			client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
			client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
			client.DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
			client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
			client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
			client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");

			// Form URL encoded content
			var content = new FormUrlEncodedContent(values);

			// Send the POST request
			try
			{
				HttpResponseMessage response = await client.PostAsync(requestUri, content);
				if (response.IsSuccessStatusCode)
				{
					Console.Write($"| ✔️   "); // Mark as successful
				}
				else
				{
					Console.Write($"| ❌   "); // Mark as failed
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error marking attendance for {date.ToString("dd/MM/yyyy")}: {ex.Message}");
				Console.Write($"| ❌   "); // Mark as failed due to error
			}
		}

		Console.WriteLine(); // End of the week row
	}
}

// Example usage
public class Program
{
	public static async Task Main(string[] args)
	{
		var attendanceAutomation = new AttendanceAutomation();

		// Replace with actual start and end dates
		DateTime startDate = new DateTime(2024, 10, 1);
		DateTime endDate = new DateTime(2024, 10, 10);

		if(1 == 2)
			await attendanceAutomation.MarkAttendance(startDate, endDate);
	}
}
