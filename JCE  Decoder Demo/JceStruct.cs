using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//https://github.com/laomms/PB-JCE-Decoder-

namespace JCE__Decoder_Demo
{
     public  class JceStruct
     {
		public static  List<int> MapCount { get; set; }//用于存储MAP次数
		public static List<int> ListCount { get; set; }//用于存储List次数

		public static bool MapKey = false;

		public struct TreeNodeStruct
		{
			public TreeNode CurrentNode;  
			public List<TreeNode> NodeList;
			public byte[] BytesIn;
		}
		public enum JceType
		{
			TYPE_BYTE = 0,
			TYPE_SHORT = 1,
			TYPE_INT = 2,
			TYPE_LONG = 3,
			TYPE_FLOAT = 4,
			TYPE_DOUBLE = 5,
			TYPE_STRING1 = 6,
			TYPE_STRING4 = 7,
			TYPE_MAP = 8,
			TYPE_LIST = 9,
			TYPE_STRUCT_BEGIN = 10,
			TYPE_STRUCT_END = 11,
			TYPE_ZERO_TAG = 12,
			TYPE_SIMPLE_LIST = 13
		}
		public struct HeadDataStruct
		{
			public int tag; //序号
			public byte typ; //类型
		}
		public static int readHead(byte[] bytesIn, ref HeadDataStruct HeadData)
		{
			byte b = bytesIn[0]; //获取一个byte
			HeadData.typ = (byte)(b & 0xF); //低4位为类型
			HeadData.tag = (b & 0xF0) >> 4; //高4位为tag,
			if (HeadData.tag != 0xF) //如果tag为0xF 则下一个字段为tag
			{
				return 1;
			}
			HeadData.tag = bytesIn[1] & 0xFF;
			return 2;
		}
		public static TreeNodeStruct QuickDecodeJce(string RootKey, TreeNodeStruct NodeStruct)
		{
			var NodeList = new List<TreeNode>();
			if (!string.IsNullOrEmpty(RootKey))
			{
				NodeList.Add(NodeStruct.CurrentNode);
				var NodeCollection = NodeStruct.CurrentNode.Nodes;
				TreeNode[] Nodes = NodeCollection.Find(RootKey, true);
				if (Nodes.Count() > 0)
				{
					NodeStruct.CurrentNode = Nodes[0];
				}
			}

			///////////貌似得缓存一下，不然有些分叉出现错位现象，这个对大型数据解析的速度影响很大，慢了很多，不知道该怎么处理
			///Thread.Sleep(20);
			var now = DateTime.Now;
			while (DateTime.Now < now.AddMilliseconds(15))
			{
			}
			///////////
		

			string subNode = null;
			byte[] jceData = null;
			HeadDataStruct HeadData = new HeadDataStruct();
			try
			{
				while (NodeStruct.BytesIn.Length > 0)
				{
					
					var len = readHead(NodeStruct.BytesIn, ref HeadData);
					var Hex = NodeStruct.BytesIn[0].ToString("x2").ToUpper();
					NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(len).ToArray();
					var typ = HeadData.typ;
					var tag = HeadData.tag;
					switch ((int)typ)
					{
						case (int)JceType.TYPE_BYTE:
							{
								jceData = NodeStruct.BytesIn.Take(1).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(1).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Byte) Value=" + jceData[0].ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_SHORT:
							{
								jceData = NodeStruct.BytesIn.Take(2).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(2).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Short) Value=" + BitConverter.ToInt16(jceData.Reverse().ToArray(), 0).ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_INT:
							{
								jceData = NodeStruct.BytesIn.Take(4).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(4).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Int) Value=" + BitConverter.ToInt32(jceData.Reverse().ToArray(), 0).ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_LONG:
							{
								jceData = NodeStruct.BytesIn.Take(8).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(8).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Long) Value=" + BitConverter.ToInt64(jceData.Reverse().ToArray(), 0).ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_FLOAT:
							{
								jceData = NodeStruct.BytesIn.Take(4).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(4).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Single) Value=" + BitConverter.ToSingle(jceData.Reverse().ToArray(), 0).ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_DOUBLE:
							{
								jceData = NodeStruct.BytesIn.Take(8).ToArray();
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(8).ToArray();
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Double) Value=" + BitConverter.ToDouble(jceData.Reverse().ToArray(), 0).ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", " ");
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_STRING1:
							{
								var jceDatalen = int.Parse(Convert.ToString(NodeStruct.BytesIn.Take(1).ToArray()[0]));
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(1).ToArray();
								if (jceDatalen > 0)
								{
									if (NodeStruct.BytesIn.Length < jceDatalen)
									{
										jceDatalen = NodeStruct.BytesIn.Length;
									}
									jceData = NodeStruct.BytesIn.Take(jceDatalen).ToArray();
									NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(jceDatalen).ToArray();
									var str = Encoding.UTF8.GetString(jceData).Replace("\0", "").Replace("\n", "").Replace("\r\n", "").Replace("\r", "").Replace("\b", "").Replace("\f", "").Replace("\v", "");
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (String) Length=" + str.Length.ToString() + " UTF8=" + str;
								}
								else
								{
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (LongString) Length=0 Hex=00";
								}
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_STRING4:
							{
								var jceDatalen = BitConverter.ToInt32(NodeStruct.BytesIn.Take(4).Reverse().ToArray(), 0);
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(4).ToArray();
								if (jceDatalen > 0)
								{
									if (NodeStruct.BytesIn.Length < jceDatalen)
									{
										jceDatalen = NodeStruct.BytesIn.Length;
									}
									jceData = NodeStruct.BytesIn.Take(jceDatalen).ToArray();
									NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(jceDatalen).ToArray();
									var str = Encoding.UTF8.GetString(jceData).Replace("\0", "").Replace("\n", "").Replace("\r\n", "").Replace("\r", "").Replace("\b", "").Replace("\f", "").Replace("\v", "");
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (LongString) Length=" + str.Length.ToString() + " UTF8=" + str;
								}
								else
								{
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (LongString) Length=0 Hex=00";
								}
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_MAP:
							{
								var count = 0;
								HeadDataStruct HD = new HeadDataStruct();
								NodeStruct.BytesIn = SkipLength(NodeStruct.BytesIn, ref count, ref HD);
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Map) Count=" + count.ToString();
								if (count > 0)
								{
									MapCount.Add(count);
									MapKey = false;
									var RandKey = new Random().Next().ToString();
									NodeStruct.CurrentNode.Nodes.Add(subNode + RandKey, subNode);
									NodeStruct = QuickDecodeJce(subNode + RandKey, NodeStruct);
								}
								else
								{
									NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								}
								break;
							}
						case (int)JceType.TYPE_LIST:
							{
								var count = 0;
								HeadDataStruct HD = new HeadDataStruct();
								NodeStruct.BytesIn = SkipLength(NodeStruct.BytesIn, ref count, ref HD);
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (List) Count=" + count.ToString();
								if (count > 0)
								{
									ListCount.Add(count);
									var RandKey = new Random().Next().ToString();
									NodeStruct.CurrentNode.Nodes.Add(subNode + RandKey, subNode);
									NodeStruct = QuickDecodeJce(subNode + RandKey, NodeStruct);
								}
								else
								{
									NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								}
								break;
							}
						case (int)JceType.TYPE_STRUCT_BEGIN:
							{
								if (tag.ToString() != "0")
								{
									subNode = "Field #" + tag.ToString() + " [STRUCT_BEGIN]";
								}
								else
								{
									subNode = "[STRUCT_BEGIN]";
								}
								var RandKey = new Random().Next().ToString();
								NodeStruct.CurrentNode.Nodes.Add(subNode + RandKey, subNode);
								NodeStruct = QuickDecodeJce(subNode + RandKey, NodeStruct);
								NodeStruct.CurrentNode.Nodes.Add("STRUCT_END");
								break;
							}
						case (int)JceType.TYPE_STRUCT_END:
							{
								goto ExitLabel1;
							}
						case (int)JceType.TYPE_ZERO_TAG:
							{
								subNode = "Field #" + tag.ToString() + " [" + Hex + "] (Zero) Value=0";
								NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								break;
							}
						case (int)JceType.TYPE_SIMPLE_LIST:
							{
								HeadDataStruct HD = new HeadDataStruct();
								readHead(NodeStruct.BytesIn, ref HD);
								NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(1).ToArray();
								var jceDatalen = 0;
								NodeStruct.BytesIn = SkipLength(NodeStruct.BytesIn,ref jceDatalen, ref HD);
								if (jceDatalen > 0)
								{
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (SimpleList) Length=" + jceDatalen.ToString();
									jceData = NodeStruct.BytesIn.Take(jceDatalen).ToArray();
                                    if (jceData[0] == (byte)JceType.TYPE_STRUCT_BEGIN || jceData[0] == (byte)JceType.TYPE_LIST || jceData[0] == (byte)JceType.TYPE_SIMPLE_LIST || (jceData[0] == (byte)JceType.TYPE_MAP && jceData[1] == 0))
									{
										var RandKey = new Random().Next().ToString();
										NodeStruct.CurrentNode.Nodes.Add(subNode + RandKey, subNode);
										var DataRemain = NodeStruct.BytesIn.Skip(jceDatalen).ToArray();
										NodeStruct.BytesIn = jceData;
						
										QuickDecodeJce(subNode + RandKey, NodeStruct);
										NodeStruct.BytesIn = DataRemain;
									}
									else
									{
										NodeStruct.BytesIn = NodeStruct.BytesIn.Skip(jceDatalen).ToArray();
										subNode = "Field #" + tag.ToString() + " [" + Hex + "] (SimpleList) Length=" + jceData.Length.ToString() + " Hex=" + BitConverter.ToString(jceData).Replace("-", "");
										NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
									}
								}
								else
								{
									subNode = "Field #" + tag.ToString() + " [" + Hex + "] (SimpleList) Length=0";
									NodeStruct.CurrentNode.Nodes.Add(new TreeNode(subNode));
								}
								MapKey = true;
								break;
							}
					}
					if (CheckLeaveStatus(NodeStruct) == true)
					{
						break;
					}
				}
			ExitLabel1:;
			}
			catch (Exception ex)
			{
				Console.WriteLine (ex.Message.ToString());
			}

			if (!string.IsNullOrEmpty(RootKey))
			{
				TreeNode Parent = NodeStruct.CurrentNode.Parent;
				if (Parent != null)
				{
					NodeStruct.CurrentNode = Parent;
				}
			}

			return NodeStruct;
		}
		public static byte[] SkipLength(byte[] bytesIn, ref int Length, ref HeadDataStruct HeadData)
		{
			readHead(bytesIn,ref HeadData);
			bytesIn = bytesIn.Skip(1).ToArray();
			switch (HeadData.typ)
			{
				case (byte)JceType.TYPE_ZERO_TAG:
					Length = 0;
					return bytesIn;
				case (byte)JceType.TYPE_BYTE:
					Length = bytesIn[0];
					return bytesIn.Skip(1).ToArray();
				case (byte)JceType.TYPE_SHORT:
					Length = Convert.ToInt32(BitConverter.ToInt16(bytesIn.Take(2).ToArray().Reverse().ToArray(), 0).ToString());
					return bytesIn.Skip(2).ToArray();
				case (byte)JceType.TYPE_INT:
					Length = Convert.ToInt32(BitConverter.ToInt32(bytesIn.Take(4).ToArray().Reverse().ToArray(), 0).ToString());
					return bytesIn.Skip(4).ToArray();
			}
			Length = -1;
			return bytesIn;
		}
		private static bool CheckLeaveStatus(TreeNodeStruct NodeStruct)//判断是否返回上级目录
		{

			if (MapCount[MapCount.Count - 1] > 0 && MapKey == true & NodeStruct.CurrentNode.Text.Contains("Map"))
			{
				MapKey = false;
				MapCount[MapCount.Count - 1]= MapCount[MapCount.Count - 1] - 1;
				if (MapCount[MapCount.Count - 1] == 0)
				{
					MapCount.RemoveAt(MapCount.Count - 1);
					return true;
				}
			}
			else if (MapCount[MapCount.Count - 1] > 0 && MapKey == false & NodeStruct.CurrentNode.Text.Contains("Map"))
			{
				MapKey = true;
			}
			else if (ListCount[ListCount.Count - 1] > 0 && NodeStruct.CurrentNode.Text.Contains("STRUCT_BEGIN") == false && NodeStruct.CurrentNode.Text.Contains("STRUCT_END") == false & NodeStruct.CurrentNode.Text.Contains("(List)"))
			{
				ListCount[ListCount.Count - 1] = ListCount[ListCount.Count - 1] - 1;
				if (ListCount[ListCount.Count - 1] == 0)
				{
					if (ListCount.Count > 1)
					{
						ListCount.RemoveAt(ListCount.Count - 1);
					}
					return true;
				}
			}
			return false;
		}

    }
}
