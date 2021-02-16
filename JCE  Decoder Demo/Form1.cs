using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JCE__Decoder_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		public byte[] HexStrToByteArray(string str)
		{
			try
			{
				Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
				for (int i = 0; i <= 255; i++)
				{
					hexindex.Add(i.ToString("X2"), (byte)i);
				}
				if (str.Length % 2 == 1)
				{
					str = "0" + str;
				}
				List<byte> hexres = new List<byte>();
				for (int i = 0; i < str.Length; i += 2)
				{
					hexres.Add(hexindex[str.Substring(i, 2)]);
				}
				return hexres.ToArray();
			}
			catch 
			{

			}
			return null;
		}
		private void button1_Click(object sender, EventArgs e)
        {
			TreeView1.Nodes.Clear();  
			var BytesIn = HexStrToByteArray(RichTextBox1.Text.Replace(" ", "").Trim());
			if (BytesIn != null)
			{
				JceStruct.MapCount = new List<int>();
				JceStruct.MapCount.Add(0);
				JceStruct.ListCount = new List<int>();
				JceStruct.ListCount.Add(0);
				JceStruct.TreeNodeStruct NodeStruct = new JceStruct.TreeNodeStruct();
				NodeStruct.NodeList = new List<TreeNode>();
				NodeStruct.CurrentNode = new TreeNode("JceStruct");
				NodeStruct.NodeList.Add(NodeStruct.CurrentNode);
				NodeStruct.BytesIn = BytesIn;
				NodeStruct = JceStruct.QuickDecodeJce("", NodeStruct);		
				TreeView1.Nodes.AddRange(NodeStruct.NodeList.ToArray());
				TreeView1.ExpandAll();

			}


		}
		private void Form1_Load(object sender, EventArgs e)
		{
			RichTextBox1.Text = "00 00 08 16 10 02 2C 3C 4C 56 23 51 51 53 65 72 76 69 63 65 2E 43 6F 6E 66 69 67 50 75 73 68 53 76 63 2E 4D 61 69 6E 53 65 72 76 61 6E 74 66 07 50 75 73 68 52 65 71 7D 00 01 07 D5 08 00 01 06 07 50 75 73 68 52 65 71 18 00 01 06 12 43 6F 6E 66 69 67 50 75 73 68 2E 50 75 73 68 52 65 71 1D 00 01 07 AD 0A 10 02 2D 00 01 07 9F 09 00 05 0A 16 0E 31 38 33 2E 32 33 32 2E 31 32 37 2E 33 33 21 1F 90 0B 0A 16 0E 31 38 33 2E 32 33 32 2E 31 32 37 2E 33 34 21 1F 90 0B 0A 16 0F 31 32 30 2E 31 39 38 2E 31 38 38 2E 31 35 30 21 1F 90 0B 0A 16 0E 31 38 33 2E 32 33 32 2E 31 32 37 2E 32 39 21 1F 90 0B 0A 16 0E 31 38 33 2E 32 33 32 2E 31 32 37 2E 33 30 21 1F 90 0B 19 00 01 0A 16 0C 31 30 33 2E 37 2E 33 31 2E 31 36 33 20 50 0B 29 00 01 0A 16 0F 32 30 33 2E 32 30 35 2E 32 33 39 2E 31 34 31 20 50 0B 39 00 05 0A 16 0E 32 32 31 2E 31 37 39 2E 31 38 2E 31 37 36 20 50 0B 0A 16 0E 32 32 31 2E 31 37 39 2E 31 38 2E 31 37 38 20 50 0B 0A 16 0E 32 32 31 2E 31 37 39 2E 31 38 2E 31 38 30 20 50 0B 0A 16 0E 32 32 31 2E 31 37 39 2E 31 38 2E 31 38 32 20 50 0B 0A 16 11 73 63 61 6E 6E 6F 6E 2E 33 67 2E 71 71 2E 63 6F 6D 20 50 0B 49 00 04 0A 16 0F 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 34 34 21 1F 90 0B 0A 16 0E 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 35 21 1F 90 0B 0A 16 0C 35 39 2E 33 36 2E 38 39 2E 32 35 32 20 50 0B 0A 16 0F 31 32 33 2E 31 35 31 2E 31 39 30 2E 32 31 30 21 01 BB 0B 5A 09 00 03 0A 00 01 19 00 04 0A 00 01 16 0F 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 34 34 21 1F 90 0B 0A 00 01 16 0E 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 35 21 1F 90 0B 0A 00 01 16 0C 35 39 2E 33 36 2E 38 39 2E 32 35 32 20 50 0B 0A 00 01 16 0F 31 32 33 2E 31 35 31 2E 31 39 30 2E 32 31 30 21 01 BB 0B 29 0C 3C 0B 0A 00 05 19 00 04 0A 00 01 16 0F 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 34 34 21 1F 90 0B 0A 00 01 16 0E 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 35 21 1F 90 0B 0A 00 01 16 0C 35 39 2E 33 36 2E 38 39 2E 32 35 32 20 50 0B 0A 00 01 16 0F 31 32 33 2E 31 35 31 2E 31 39 30 2E 32 31 30 21 01 BB 0B 29 0C 3C 0B 0A 00 0A 19 00 04 0A 00 01 16 0F 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 34 34 21 1F 90 0B 0A 00 01 16 0E 32 30 33 2E 32 30 35 2E 32 33 34 2E 31 35 21 1F 90 0B 0A 00 01 16 0C 35 39 2E 33 36 2E 38 39 2E 32 35 32 20 50 0B 0A 00 01 16 0F 31 32 33 2E 31 35 31 2E 31 39 30 2E 32 31 30 21 01 BB 0B 29 00 05 0A 0C 11 20 00 20 10 30 01 0B 0A 00 01 11 20 00 20 08 30 02 0B 0A 00 02 11 20 00 20 08 30 01 0B 0A 00 03 11 20 00 20 08 30 02 0B 0A 00 04 11 20 00 20 08 30 02 0B 3C 0B 1D 00 00 68 8D 83 42 3A A6 23 A0 EC DF DD DD BB FE 2F 06 F4 BF 31 11 65 85 0E 41 E8 B2 B7 99 67 48 41 7E BC DE D8 0A 45 8F 63 FF 7C 51 E0 51 67 BA 34 5C 62 DF 1D FE 76 31 71 95 62 BA C8 ED A7 77 66 49 FD AA 9E 73 09 08 8E 9D F6 BB 22 8C 3A 02 A5 01 20 D5 DA CA AD 43 DF 87 5F 72 56 85 FD 59 E0 D0 43 2A DC 6B EF 7E 41 6A 8C 2D 00 00 10 47 38 35 37 56 51 69 59 33 48 73 4A 52 69 51 5A 33 00 00 00 00 CF 2B B6 D8 40 01 5D 00 01 02 52 8A 50 CE 04 0A 68 8D 83 42 3A A6 23 A0 EC DF DD DD BB FE 2F 06 F4 BF 31 11 65 85 0E 41 E8 B2 B7 99 67 48 41 7E BC DE D8 0A 45 8F 63 FF 7C 51 E0 51 67 BA 34 5C 62 DF 1D FE 76 31 71 95 62 BA C8 ED A7 77 66 49 FD AA 9E 73 09 08 8E 9D F6 BB 22 8C 3A 02 A5 01 20 D5 DA CA AD 43 DF 87 5F 72 56 85 FD 59 E0 D0 43 2A DC 6B EF 7E 41 6A 8C 12 10 47 38 35 37 56 51 69 59 33 48 73 4A 52 69 51 5A 1A 41 08 01 12 0E 08 01 15 CB CD EA 90 18 90 3F 20 05 28 01 12 0E 08 01 15 CB CD EA 0F 18 90 3F 20 05 28 01 12 0D 08 01 15 3B 24 59 FC 18 50 20 01 28 00 12 0E 08 01 15 7B 97 BE D2 18 BB 03 20 02 28 00 1A 41 08 05 12 0E 08 01 15 CB CD EA 90 18 90 3F 20 05 28 01 12 0E 08 01 15 CB CD EA 0F 18 90 3F 20 05 28 01 12 0D 08 01 15 3B 24 59 FC 18 50 20 01 28 00 12 0E 08 01 15 7B 97 BE D2 18 BB 03 20 02 28 00 1A 78 08 0A 12 0E 08 01 15 CB CD EA 90 18 90 3F 20 05 28 01 12 0E 08 01 15 CB CD EA 0F 18 90 3F 20 05 28 01 12 0D 08 01 15 3B 24 59 FC 18 50 20 01 28 00 12 0E 08 01 15 7B 97 BE D2 18 BB 03 20 02 28 00 22 09 08 00 10 80 40 18 10 20 01 22 09 08 01 10 80 40 18 08 20 02 22 09 08 02 10 80 40 18 08 20 01 22 09 08 03 10 80 40 18 08 20 02 22 09 08 04 10 80 40 18 08 20 02 20 01 32 04 08 00 10 01 3A 2A 08 10 10 10 18 09 20 09 28 0F 30 0F 38 05 40 05 48 5A 50 01 58 5A 60 5A 68 5A 70 5A 78 0A 80 01 0A 88 01 0A 90 01 0A 98 01 0A 42 0A 08 00 10 00 18 00 20 00 28 00 4A 06 08 01 10 01 18 03 52 42 08 01 12 0A 08 00 10 80 80 04 18 10 20 02 12 0A 08 01 10 80 80 04 18 08 20 02 12 0A 08 02 10 80 80 01 18 08 20 01 12 0A 08 03 10 80 80 02 18 08 20 02 12 0A 08 04 10 80 80 04 18 08 20 02 18 01 20 00 5A 3C 08 02 12 0A 08 00 10 80 80 04 18 10 20 02 12 09 08 01 10 80 40 18 08 20 02 12 09 08 02 10 80 40 18 08 20 01 12 09 08 03 10 80 40 18 08 20 02 12 09 08 04 10 80 40 18 08 20 02 18 01 70 01 78 01 80 01 FA 01 0B 69 0C 79 00 02 0A 16 0F 32 30 33 2E 32 30 35 2E 32 33 39 2E 31 34 30 20 50 0B 0A 16 0F 32 30 33 2E 32 30 35 2E 32 33 39 2E 31 34 33 20 50 0B 8A 06 0D 31 31 30 2E 31 37 31 2E 32 34 2E 37 37 10 04 0B 9A 09 00 03 0A 00 0A 19 00 02 0A 12 10 EF CD CB 20 50 0B 0A 12 98 FE CD CB 20 50 0B 29 0C 0B 0A 00 08 19 00 02 0A 12 F8 FF CD CB 20 50 0B 0A 12 9E EF CD CB 20 50 0B 29 0C 0B 0A 00 06 19 00 02 0A 12 57 DB CD CB 20 50 0B 0A 12 AB FE CD CB 20 50 0B 29 0C 0B 0B AD 00 01 01 5B 08 01 10 EA BE C7 92 02 18 00 22 0A 33 34 37 35 37 34 38 35 36 38 28 EE D6 E2 E8 04 32 12 08 CB 9B E3 FF 02 10 50 18 89 B4 9C A0 01 20 50 28 64 32 12 08 CB 9B E3 DF 07 10 50 18 89 B4 A8 A0 01 20 50 28 64 32 13 08 DF CD DE B4 0C 10 50 18 E4 E0 99 B0 06 20 50 28 C8 01 32 13 08 8C 9D 9B AD 02 10 50 18 89 D6 AD E4 0F 20 50 28 C8 01 32 13 08 DF CD DE CC 09 10 50 18 E4 E0 E1 8C 0B 20 50 28 AC 02 32 12 08 F4 80 E6 8C 05 10 50 18 89 EC 94 58 20 50 28 AC 02 3A 1E 0A 10 24 02 4E 00 80 20 00 02 00 00 00 00 00 00 00 7E 10 50 18 8A EC CC AE 0E 20 50 28 64 3A 1E 0A 10 24 02 4E 00 80 20 00 02 00 00 00 00 00 00 00 A9 10 50 18 89 E6 80 B8 02 20 50 28 64 3A 1F 0A 10 24 02 4E 00 80 10 00 00 00 00 00 00 00 00 01 58 10 50 18 89 DC C4 DC 03 20 50 28 C8 01 3A 1F 0A 10 24 02 4E 00 80 10 00 00 00 00 00 00 00 00 01 5A 10 50 18 89 DC 84 D8 0B 20 50 28 C8 01 3A 1F 0A 10 24 02 4E 00 80 10 00 00 00 00 00 00 00 00 01 51 10 50 18 89 DC C0 E4 0F 20 50 28 AC 02 3A 1F 0A 10 24 02 4E 00 80 10 00 00 00 00 00 00 00 00 01 58 10 50 18 89 DC C4 DC 03 20 50 28 AC 02 32 1B D2 22 69 0B 8C 98 0C A8 0C";
			RichTextBox1.DoubleClick += RichTextBox1_DoubleClick;

		}

		private void RichTextBox1_DoubleClick(object sender, EventArgs e)
		{
			RichTextBox1.Text = GetText();
		}

		private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			RichTextBox1.Text = GetText();
		}

		public void SetText(string p_Text)
		{
			Thread STAThread = new Thread(() =>
			{
				System.Windows.Forms.Clipboard.SetText(p_Text);
			});
			STAThread.SetApartmentState(ApartmentState.STA);
			STAThread.Start();
			STAThread.Join();
		}
		public string GetText()
		{
			string ReturnValue = string.Empty;
			Thread STAThread = new Thread(() =>
			{
				ReturnValue = System.Windows.Forms.Clipboard.GetText();
			});
			STAThread.SetApartmentState(ApartmentState.STA);
			STAThread.Start();
			STAThread.Join();
			return ReturnValue;
		}
	}
	}
