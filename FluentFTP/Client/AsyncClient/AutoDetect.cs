﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentFTP.Client.Modules;
using FluentFTP.Model.Functions;

namespace FluentFTP {
	public partial class AsyncFtpClient {

		/// <summary>
		/// Automatic FTP and FTPS connection negotiation.
		/// This method tries every possible combination of the FTP connection properties, and returns the list of successful connection profiles.
		/// You can configure it to stop after finding the first successful profile, or to collect all successful profiles.
		/// You can then generate code for the profile using the FtpProfile.ToCode method.
		/// If no successful profiles are found, a blank list is returned.
		/// </summary>
		/// <param name="config">The coresponding config object for this API</param>
		/// <param name="token">The token that can be used to cancel the entire process</param>
		/// <returns></returns>
		public async Task<List<FtpProfile>> AutoDetect(FtpAutoDetectConfig config, CancellationToken token = default(CancellationToken)) {
			var results = new List<FtpProfile>();

			//LogFunction(nameof(AutoDetect), new object[] { firstOnly, cloneConnection });
			ValidateAutoDetect();

			return await ConnectModule.AutoDetectAsync(this, config, token);
		}

	}
}
