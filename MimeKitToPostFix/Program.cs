using MimeKit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MimeKitToPostFix {
	class Program {
		static async Task Main() {
			Console.WriteLine("Hello World!");
			await SendMail("TEST");
		}

		public static async Task SendMail(string body) {
			var builder = new BodyBuilder {
				HtmlBody = body
			};
			var message = new MimeMessage {
				From = { MailboxAddress.Parse("from@example.com") },
				To = { MailboxAddress.Parse("to@example.com") },
				Subject = "TEST MAIL",
				Body = builder.ToMessageBody()
			};
			var sendmail = Process.Start(new ProcessStartInfo {
				FileName = "/usr/sbin/sendmail",
				ArgumentList = {
					"-t", "-i"
				},
				RedirectStandardInput = true
			});
			sendmail.Start();

			await message.WriteToAsync(sendmail.StandardInput.BaseStream);

			await sendmail.StandardInput.FlushAsync();
			sendmail.StandardInput.Close();
		}
	}
}
