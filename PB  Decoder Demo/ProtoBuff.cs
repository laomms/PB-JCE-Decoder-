using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//https://github.com/laomms/PB-JCE-Decoder-

namespace PB__Decoder_Demo
{
    class ProtoBuff
    {
		public struct TreeNodeStruct
		{
			public TreeNode parentNode;
			public List<TreeNode> NodeList;
		}

        [Obsolete]
        public static TreeNodeStruct QuickDecodeProto(byte[] bytesIn, string RootKey, TreeNodeStruct NodeStruct)
		{

			var NodeList = new List<TreeNode>();
			if (!string.IsNullOrEmpty(RootKey))
			{
				NodeList.Add(NodeStruct.parentNode);
				var NodeCollection = NodeStruct.parentNode.Nodes;
				TreeNode[] Nodes = NodeCollection.Find(RootKey, true);
				if (Nodes.Count() > 0)
				{
					NodeStruct.parentNode = Nodes[0];
				}
			}

			ProtoReader reader = null;
			using (var ms = new MemoryStream(bytesIn))
			{
				reader = ProtoReader.Create(ms, null, null);
			}
			long start = reader.Position;
			int field = reader.ReadFieldHeader();
			try
			{
				while (field > 0)
				{
					long payloadStart = reader.Position;
					switch (reader.WireType)
					{
						case WireType.Varint:
							{
								var val = reader.ReadInt64();
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (Varint) " + "Value=" + val.ToString() + " Hex=" + BitConverter.ToString(BitConverter.GetBytes(val).Reverse().ToArray()).Replace("-", " ").Replace("00 00 00", "").Trim().TrimStart('0');
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						case WireType.Fixed32:
							{
								var val = reader.ReadInt32();
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (Fixed32) " + "Value=" + val.ToString() + " Hex=" + BitConverter.ToString(BitConverter.GetBytes(val).Reverse().ToArray()).Replace("-", " ").Replace("00 00 00", "").Trim().TrimStart('0');
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						case WireType.Fixed64:
							{
								var val = reader.ReadInt64();
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (Fixed32) " + "Value=" + val.ToString() + " Hex=" + BitConverter.ToString(BitConverter.GetBytes(val).Reverse().ToArray()).Replace("-", " ").Replace("00 00 00", "").Trim().TrimStart('0');
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						case WireType.String:
							{
								var payloadBytes = ProtoReader.AppendBytes(null, reader);
								using (var subReader = ReadProto(payloadBytes))
								{
									if (subReader != null)
									{
										var RandKey = (new Random()).Next().ToString();
										var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (String-SubProto) " + "Length=" + payloadBytes.Length.ToString();
										NodeStruct.parentNode.Nodes.Add(key + RandKey, key);
										QuickDecodeProto(payloadBytes, key + RandKey, NodeStruct);
									}
									else
									{
										var str = Encoding.UTF8.GetString(payloadBytes).Replace("\0", "").Replace("\n", "").Replace("\r\n", "").Replace("\r", "").Replace("\b", "").Replace("\f", "").Replace("\v", "");
										var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (String) " + "Length=" + payloadBytes.Length.ToString() + " UTF8 =" + ((payloadBytes.Length == 0) ? "\"\"" : str);
										NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
									}
								}
								break;
							}
						case WireType.None:
							{
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (None) " + "Value=0";
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						case WireType.StartGroup:
							{
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (StartGroup) " + "Value=StartGroup";
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						case WireType.EndGroup:
							{
								var key = "Field #" + reader.FieldNumber.ToString() + " [" + bytesIn[start].ToString("x2").ToUpper() + "] (EndGroup) " + "Value=EndGroup";
								NodeStruct.parentNode.Nodes.Add(new TreeNode(key));
								break;
							}
						default:
							{
								break;
							}
					}
					start = reader.Position;
					field = reader.ReadFieldHeader();
				}
			}
			catch (Exception ex)
			{
				bytesIn = bytesIn.Skip(1).ToArray();
				QuickDecodeProto(bytesIn, "", NodeStruct);
			}
			return NodeStruct;
		}

        [Obsolete]
        private static ProtoReader ReadProto(byte[] payload)
		{
			if (payload == null || payload.Length == 0)
			{
				return null;
			}
			try
			{
				var ms = new MemoryStream(payload);
				using (var reader = ProtoReader.Create(ms, null, null))
				{
					int field = reader.ReadFieldHeader();
					while (field > 0)
					{
						reader.SkipField();
						field = reader.ReadFieldHeader();
					}
				}
				ms.Position = 0;
				return ProtoReader.Create(ms, null, null);
			}
			catch
			{
				return null;
			}
		}

	}
}
