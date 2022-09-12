﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentFTP.Xunit.Docker;
using FluentFTP.Xunit.Attributes;
using FluentFTP.Tests.Integration.System;

namespace FluentFTP.Tests.Integration.Tests {

	internal class FileTransferTests : IntegrationTestSuite {

		public FileTransferTests(DockerFtpServer fixture) : base(fixture) { }

		/// <summary>
		/// Main entrypoint executed for all types of FTP servers.
		/// </summary>
		public override void RunAllTests() {
			UploadDownloadBytes();
			UploadDownloadStream();
		}

		/// <summary>
		/// Main entrypoint executed for all types of FTP servers.
		/// </summary>
		public async override Task RunAllTestsAsync() {
			await UploadDownloadBytesAsync();
			await UploadDownloadStreamAsync();
		}



		public async Task UploadDownloadBytesAsync() {

			const string content = "Hello World!";
			var bytes = Encoding.UTF8.GetBytes(content);

			using var client = await GetConnectedAsyncClient();

			const string path = "/UploadDownloadBytesAsync/helloworld.txt";
			var uploadStatus = await client.UploadBytes(bytes, path, createRemoteDir: true);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			var outBytes = await client.DownloadBytes(path, CancellationToken.None);
			Assert.NotNull(outBytes);

			var outContent = Encoding.UTF8.GetString(outBytes);
			Assert.Equal(content, outContent);
		}


		public void UploadDownloadBytes() {

			const string content = "Hello World!";
			var bytes = Encoding.UTF8.GetBytes(content);

			using var client = GetConnectedClient();

			const string path = "/UploadDownloadBytes/helloworld.txt";
			var uploadStatus = client.UploadBytes(bytes, path, createRemoteDir: true);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			var success = client.DownloadBytes(out var outBytes, path);
			Assert.True(success);

			var outContent = Encoding.UTF8.GetString(outBytes);
			Assert.Equal(content, outContent);
		}


		public async Task UploadDownloadStreamAsync() {

			const string content = "Hello World!";
			using var stream = new MemoryStream();
			using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
			await streamWriter.WriteAsync(content);
			await streamWriter.FlushAsync();
			stream.Position = 0;

			const string path = "/UploadDownloadStreamAsync/helloworld.txt";
			using var client = await GetConnectedAsyncClient();
			var uploadStatus = await client.UploadStream(stream, path, createRemoteDir: true);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			using var outStream = new MemoryStream();
			var success = await client.DownloadStream(outStream, path);
			Assert.True(success);

			outStream.Position = 0;
			using var streamReader = new StreamReader(outStream, Encoding.UTF8);
			var outContent = await streamReader.ReadToEndAsync();
			Assert.Equal(content, outContent);
		}


		public void UploadDownloadStream() {

			const string content = "Hello World!";
			using var stream = new MemoryStream();
			using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
			streamWriter.Write(content);
			streamWriter.Flush();
			stream.Position = 0;

			const string path = "/UploadDownloadStreamAsync/helloworld.txt";
			using var client = GetConnectedClient();
			var uploadStatus = client.UploadStream(stream, path, createRemoteDir: true);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			using var outStream = new MemoryStream();
			var success = client.DownloadStream(outStream, path);
			Assert.True(success);

			outStream.Position = 0;
			using var streamReader = new StreamReader(outStream, Encoding.UTF8);
			var outContent = streamReader.ReadToEnd();
			Assert.Equal(content, outContent);
		}

	}
}