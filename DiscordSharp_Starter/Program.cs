using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordSharp_Starter
{
    public class Program
    {
		public static List<String> mMessageQueue = new List<String>();

        public static void Main(string[] args)
        {
			Console.WriteLine("-Begin DJ SquidBot-");
			Console.WriteLine("-Type /help-");
			
			var client = new DiscordClient();

			client.MessageReceived += async (s, e) =>
			{
				if (!e.Message.IsAuthor)
				{
					String msg = e.Message.Text;
					String returnMsg = "";

					if (msg == "!hello")
						returnMsg = "Yo, whats up " + e.Message.User.Name;
					if (msg.IndexOf("!multiply") == 0)
					{
						String data = msg.Substring(10);
						String[] datas = data.Split(' ');
						if (datas.Length == 2)
							returnMsg = (int.Parse(datas[0]) * int.Parse(datas[1])).ToString();
					}
					if (returnMsg != "")
						await e.Channel.SendMessage(returnMsg);
				}
			};

			/*client.Ready += async (s, e) =>
			{
				if (mMessageQueue.Count != 0)
					await client.SetGame(mMessageQueue[0]);
			});*/
			
			client.ExecuteAndWait(async () => 
			{
				await client.Connect("MjQwMzExMzE4Njg5NjExNzc4.CvBe5A.tmLgxblHhxZi3DiNh81YHL6XG-s", TokenType.Bot);
			});


			bool bContinue = true;
			while(bContinue)
			{
				String command = Console.ReadLine();

				if (command.IndexOf("!announce") == 0)
				{
					String data = command.Substring("!announce".Length + 1);
					String[] datas = data.Split(' ');

					mMessageQueue.Add(datas[0]);

					client.SetGame(datas[0]);
				}
			}
		}
    }
}
