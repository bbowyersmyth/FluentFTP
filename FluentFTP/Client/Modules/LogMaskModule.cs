﻿using FluentFTP.Client.BaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFTP.Client.Modules {
	/// <summary>
	/// Class responsible for masking out sensitive data from FTP logs.
	/// </summary>
	internal static class LogMaskModule {

		public static string MaskCommand(BaseFtpClient client, string command) {

			// if username needs to be masked out
			if (!client.Config.LogUserName) {
				if (command.StartsWith("USER", StringComparison.Ordinal)) {
					command = "USER ***";
				}
			}

			// if password needs to be masked out
			if (!client.Config.LogPassword) {
				if (command.StartsWith("PASS", StringComparison.Ordinal)) {
					command = "PASS ***";
				}
			}

			return command;
		}

		public static string MaskReply(BaseFtpClient client, FtpReply reply, string message, string command) {

			// if username needs to be masked out
			if (!client.Config.LogUserName) {

				// if its the reply to the USER command
				if (command.StartsWith("USER", StringComparison.Ordinal)) {

					// mask out username
					if (reply.Code == "331") {
						message = message.Replace(client.Credentials.UserName, "***");
					}

				}

				// if its the reply to the PASS command
				if (command.StartsWith("PASS", StringComparison.Ordinal)) {

					// mask out username
					if (reply.Code == "230") {
						message = message.Replace(client.Credentials.UserName, "***");
					}

				}
			}
			return message;
		}

	}
}
