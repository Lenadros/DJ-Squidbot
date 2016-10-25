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
		public static DiscordClient mClient;
		
		public const int ERRORCODE_MALFORMED_COMMAND = 0;
		public const int ERRORCODE_FORBIDDEN = 1;
		
		public const String COMMAND_SAY = "!say";
		public const String COMMAND_GAME_CHANGE = "!setname";
		public const String COMMAND_HELP = "!help";
		public const String COMMAND_QUIT = "!quit";

		public const String HELP_STR = "Uhh, I aint got much help to give but here are the valid commands: !say, !setname, !help, !quit";

		public static bool mbShouldContinue = true;

		public static Channel mActiveChannel = null;

        public static void Main(string[] args)
        {
			Console.WriteLine("-Begin DJ SquidBot-");
			Console.WriteLine("-Type !help-");
			
			mClient = new DiscordClient();

			mClient.MessageReceived += async (s, e) =>
			{
				if (!e.Message.IsAuthor)
				{
					if (e.Message.Text == "!here")
						mActiveChannel = e.Message.Channel;
					else
						RecieveCommand(e.Message.Text, false);
					
					// Lol, not sure about this async stuff, not sure how to make it not complain about not having a nested await call...
					// Leave this here for now.
					if (false)
						await e.Channel.SendMessage("");
				}
			};
			
			mClient.ExecuteAndWait(async () => 
			{
				await mClient.Connect("MjQwMzExMzE4Njg5NjExNzc4.CvBe5A.tmLgxblHhxZi3DiNh81YHL6XG-s", TokenType.Bot);

				UpdateLoop();
			});
		}

		public async static void UpdateLoop()
		{
			bool bContinue = true;
			while(bContinue)
			{
				bContinue = Tick();
			}
			Console.WriteLine("-Closing-");
			await mClient.Disconnect();
		}

		public static bool Tick()
		{
			String input = Console.ReadLine();
			RecieveCommand(input, true);

			return mbShouldContinue;
		}

		public async static void RecieveCommand(String args, bool bLocalAuth)
		{
			// First, determine what the command is
			String[] elements = args.Split(' ');
			String command = elements[0];

			// Check to make sure this is a properly formatted command
			if (command.IndexOf("!") != 0)
				return;

			switch (command)
			{
				case COMMAND_SAY:
					if (bLocalAuth)
					{
						String saystr = args.Substring(command.Length + 1);
						if (mActiveChannel != null)
							await mActiveChannel.SendMessage(saystr);
					}
					else
						LogError(ERRORCODE_FORBIDDEN, args);
					break;
				case COMMAND_GAME_CHANGE:
					String gameName = args.Substring(command.Length + 1);
					mClient.SetGame(gameName);
					break;
				case COMMAND_HELP:
					if (mActiveChannel != null)
						await mActiveChannel.SendMessage(HELP_STR);
					break;
				case COMMAND_QUIT:
					if (bLocalAuth)
						mbShouldContinue = false;
					else
						LogError(ERRORCODE_FORBIDDEN, args);
					break;
				default:
					LogError(ERRORCODE_MALFORMED_COMMAND, args);
					break;
			}
		}

		public async static void LogError(int errorCode, String msg)
		{
			String errorOutput = "";
			switch(errorCode)
			{
				case ERRORCODE_MALFORMED_COMMAND:
					errorOutput = "Malformed Command: " + msg;
					break;
				case ERRORCODE_FORBIDDEN:
					errorOutput = "Forbidden: " + msg;
					break;
			}

			Console.Out.WriteLine(errorOutput);
			if (mActiveChannel != null)
				await mActiveChannel.SendMessage(errorOutput);
		}
    }
}
