using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SimpleNLG
{
	public static class Client
	{
		//public static string Realize(NLGSpec specToRealize) => Realize(Serialize(specToRealize));
		//public static string Realize(RequestType request) => Realize(new NLGSpec 
		//{ 
		//	Item = request 
		//});
		//public static string Realize(DocumentElement documentElement) => Realize(new RequestType 
		//{ 
		//	Document = documentElement 
		//});
		//public static string Realize(NLGElement[] documentChildren) => Realize(new DocumentElement
		//{
		//	cat = documentCategory.DOCUMENT,
		//	catSpecified = true,
		//	child = documentChildren
		//});
		//public static string Realize(NLGElement documentChild) => Realize(new NLGElement[] 
		//{ 
		//	documentChild 
		//});

		public static string Realize(string request)
		{
			using (TcpClient client = new TcpClient(Properties.Settings.Default.SimpleNLG_ServerHost, Properties.Settings.Default.SimpleNLG_ServerPort))
            {
				using (NetworkStream stream = client.GetStream())
                {
					// Sending
					int requestLength = Encoding.UTF8.GetByteCount(request);
					byte[] requestLengthSignedBytes = new byte[4];
					requestLengthSignedBytes[3] = (byte)((requestLength & 0b_00000000_00000000_00000000_11111111) >> 0);
					requestLengthSignedBytes[2] = (byte)((requestLength & 0b_00000000_00000000_11111111_00000000) >> 8);
					requestLengthSignedBytes[1] = (byte)((requestLength & 0b_00000000_11111111_00000000_00000000) >> 16);
					requestLengthSignedBytes[0] = (byte)((requestLength & 0b_01111111_00000000_00000000_00000000) >> 24);
					byte[] requestBytes = Encoding.UTF8.GetBytes(request);
					stream.Write(requestLengthSignedBytes, 0, requestLengthSignedBytes.Length);
					stream.Write(requestBytes, 0, requestBytes.Length);

					// Receiving
					while (!stream.DataAvailable) Thread.Sleep(10);

					byte[] responseLengthBytes = new byte[4];
					stream.Read(responseLengthBytes, 0, 4);
					int responseLength = responseLengthBytes[3]
						+ (responseLengthBytes[2] << 8)
						+ (responseLengthBytes[1] << 16)
						+ (responseLengthBytes[0] << 24);
					byte[] responseBytes = new byte[responseLength];
					stream.Read(responseBytes, 0, responseLength);
					string response = Encoding.UTF8.GetString(responseBytes);
					return response;
				}
			}
		}

		//private static string Serialize(NLGSpec spec)
		//{
		//	string serialized;
		//	using (var stringwriter = new System.IO.StringWriter())
		//	{
		//		var serializer = new XmlSerializer(spec.GetType());
		//		serializer.Serialize(stringwriter, spec);
		//		serialized = stringwriter.ToString();
		//	}
		//	Console.WriteLine(serialized);
		//	return serialized;
		//}

	}
}
