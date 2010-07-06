using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Timers;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// BMXNetLib implements low-level socket connectivity to RPMS databases.
	/// The VA RPC Broker must be running on the RPMS server in order for 
	/// BMXNetLib to connect.
	/// </summary>
	[DnsPermission(SecurityAction.Assert, Unrestricted = true)]
	public class BMXNetLib
	{
		/// <summary>
		/// Main constructor with default machine encoding
		/// </summary>
        public BMXNetLib()
		{
			m_sWKID = "BMX";
			m_sWINH = "";
			m_sPRCH = "";
			m_sWISH = "";
			m_cHDR = ADEBHDR(m_sWKID,m_sWINH,m_sPRCH,m_sWISH);

		}

        /// <summary>
        /// Constructor specifiying encoding
        /// </summary>
        /// <param name="charset">String name of character set</param>
        public BMXNetLib(string charset) : this()
        {
            
            //char[] arabic_nm_ch = arabic_name.ToCharArray();
            //byte[] arabic_nm_by = cp1256.GetBytes(arabic_nm_ch);
            //string arabic_name2 = "";
            //foreach (byte eachbyte in arabic_nm_by)
            //{
            //    arabic_name2 += ((char)eachbyte);
            //}
            
            try 
            { 
                m_Encoding = System.Text.Encoding.GetEncoding(charset); 
            }
            catch (ArgumentException)
            {
                Debug.Write("Invalid Code Page... Falling back to default system encoding");
                m_Encoding = Encoding.GetEncoding(0);
            }
        }
		
		#region Piece Functions

		/// <summary>
		/// Corresponds to M's $L(STRING,DELIMITER)
		/// </summary>
		/// <param name="sInput"></param>
		/// <param name="sDelim"></param>
		/// <returns></returns>
		public static int PieceLength(string sInput, string sDelim)
		{
			char[] cDelim = sDelim.ToCharArray();
			string [] cSplit = sInput.Split(cDelim);
			return cSplit.GetLength(0);
		}

		/// <summary>
		/// Corresponds to M's $$Piece function
		/// </summary>
		/// <param name="sInput"></param>
		/// <param name="sDelim"></param>
		/// <param name="nNumber"></param>
		/// <returns></returns>
		public static string Piece(string sInput, string sDelim, int nNumber)
		{
			try
			{
				char[] cDelim = sDelim.ToCharArray();
				string [] cSplit = sInput.Split(cDelim);
				int nLength = cSplit.GetLength(0);
				if ((nLength < nNumber)||(nNumber < 1))
					return "";
				return cSplit[nNumber-1];
			}
			catch (Exception bmxEx)
			{
				string sMessage =  bmxEx.Message + bmxEx.StackTrace;
				throw new BMXNetException(sMessage);
			}

		}

		public static string Piece(string sInput, string sDelim, int nNumber, int nEnd)
		{
			try 
			{
				if (nNumber < 0)
					nNumber = 1;

				if (nEnd < nNumber)
					return "";

				if (nEnd == nNumber)
					return Piece(sInput, sDelim, nNumber);

				char[] cDelim = sDelim.ToCharArray();
				string [] cSplit = sInput.Split(cDelim);
				int nLength = cSplit.GetLength(0);
				if ((nLength < nNumber)||(nNumber < 1))
					return "";

				//nNumber = 1-based index of the starting element to return
				//nLength = count of elements
				//nEnd = 1-based index of last element to return
				//nCount = number of elements past nNumber to return

				//convert nNumber to 0-based index:
				nNumber--;

				//convert nEnd to 0-based index;
				nEnd--;

				//Calculate nCount;
				int nCount = nEnd - nNumber + 1;

				//Adjust nCount for number of elements
				if (nCount + nNumber >= nLength)
				{
					nCount = nLength - nNumber;
				}

				string sRet =  string.Join(sDelim, cSplit, nNumber, nCount );
				return sRet;
			}
			catch (Exception bmxEx)
			{
				string sMessage =  bmxEx.Message + bmxEx.StackTrace;
				throw new BMXNetException(sMessage);
			}
		}

		#endregion Piece Functions

		#region RPX Fields

		private string		m_sWKID;
		private string		m_sWISH;
		private string		m_sPRCH;
		private string		m_sWINH;
		private string		m_cHDR;
		private string		m_cAppContext;
		private bool		m_bConnected;
		private int			m_nMServerPort;
		private string		m_cServerAddress;
		private string		m_cDUZ;
		private string		m_cLoginFacility;
		private TcpClient	m_pCommSocket;
		private	string		m_sNameSpace = "";
        private int         m_nReceiveTimeout = 30000;

        private Encoding m_Encoding = Encoding.GetEncoding(0);

		#endregion RPX Fields

		#region Encryption Keys
		private string[] m_sKey = new string[]
			{
				@"wkEo-ZJt!dG)49K{nX1BS$vH<&:Myf*>Ae0jQW=;|#PsO`'%+rmb[gpqN,l6/hFC@DcUa ]z~R}""V\iIxu?872.(TYL5_3",
				@"rKv`R;M/9BqAF%&tSs#Vh)dO1DZP> *fX'u[.4lY=-mg_ci802N7LTG<]!CWo:3?{+,5Q}(@jaExn$~p\IyHwzU""|k6Jeb",
				@"\pV(ZJk""WQmCn!Y,y@1d+~8s?[lNMxgHEt=uw|X:qSLjAI*}6zoF{T3#;ca)/h5%`P4$r]G'9e2if_>UDKb7<v0&- RBO.",
				@"depjt3g4W)qD0V~NJar\B ""?OYhcu[<Ms%Z`RIL_6:]AX-zG.#}$@vk7/5x&*m;(yb2Fn+l'PwUof1K{9,|EQi>H=CT8S!",
				@"NZW:1}K$byP;jk)7'`x90B|cq@iSsEnu,(l-hf.&Y_?J#R]+voQXU8mrV[!p4tg~OMez CAaGFD6H53%L/dT2<*>""{\wI=",
				@"vCiJ<oZ9|phXVNn)m K`t/SI%]A5qOWe\&?;jT~M!fz1l>[D_0xR32c*4.P""G{r7}E8wUgyudF+6-:B=$(sY,LkbHa#'@Q",
				@"hvMX,'4Ty;[a8/{6l~F_V""}qLI\!@x(D7bRmUH]W15J%N0BYPkrs&9:$)Zj>u|zwQ=ieC-oGA.#?tfdcO3gp`S+En K2*<",
				@"jd!W5[];4'<C$/&x|rZ(k{>?ghBzIFN}fAK""#`p_TqtD*1E37XGVs@0nmSe+Y6Qyo-aUu%i8c=H2vJ\) R:MLb.9,wlO~P",
				@"2ThtjEM+!=xXb)7,ZV{*ci3""8@_l-HS69L>]\AUF/Q%:qD?1~m(yvO0e'<#o$p4dnIzKP|`NrkaGg.ufCRB[; sJYwW}5&",
				@"vB\5/zl-9y:Pj|=(R'7QJI *&CTX""p0]_3.idcuOefVU#omwNZ`$Fs?L+1Sk<,b)hM4A6[Y%aDrg@~KqEW8t>H};n!2xG{",
				@"sFz0Bo@_HfnK>LR}qWXV+D6`Y28=4Cm~G/7-5A\b9!a#rP.l&M$hc3ijQk;),TvUd<[:I""u1'NZSOw]*gxtE{eJp|y (?%",
				@"M@,D}|LJyGO8`$*ZqH .j>c~h<d=fimszv[#-53F!+a;NC'6T91IV?(0x&/{B)w""]Q\YUWprk4:ol%g2nE7teRKbAPuS_X",
				@".mjY#_0*H<B=Q+FML6]s;r2:e8R}[ic&KA 1w{)vV5d,$u""~xD/Pg?IyfthO@CzWp%!`N4Z'3-(o|J9XUE7k\TlqSb>anG",
				@"xVa1']_GU<X`|\NgM?LS9{""jT%s$}y[nvtlefB2RKJW~(/cIDCPow4,>#zm+:5b@06O3Ap8=*7ZFY!H-uEQk; .q)i&rhd",
				@"I]Jz7AG@QX.""%3Lq>METUo{Pp_ |a6<0dYVSv8:b)~W9NK`(r'4fs&wim\kReC2hg=HOj$1B*/nxt,;c#y+![?lFuZ-5D}",
				@"Rr(Ge6F Hx>q$m&C%M~Tn,:""o'tX/*yP.{lZ!YkiVhuw_<KE5a[;}W0gjsz3]@7cI2\QN?f#4p|vb1OUBD9)=-LJA+d`S8",
				@"I~k>y|m};d)-7DZ""Fe/Y<B:xwojR,Vh]O0Sc[`$sg8GXE!1&Qrzp._W%TNK(=J 3i*2abuHA4C'?Mv\Pq{n#56LftUl@9+",
				@"~A*>9 WidFN,1KsmwQ)GJM{I4:C%}#Ep(?HB/r;t.&U8o|l['Lg""2hRDyZ5`nbf]qjc0!zS-TkYO<_=76a\X@$Pe3+xVvu",
				@"yYgjf""5VdHc#uA,W1i+v'6|@pr{n;DJ!8(btPGaQM.LT3oe?NB/&9>Z`-}02*%x<7lsqz4OS ~E$\R]KI[:UwC_=h)kXmF",
				@"5:iar.{YU7mBZR@-K|2 ""+~`M%8sq4JhPo<_X\Sg3WC;Tuxz,fvEQ1p9=w}FAI&j/keD0c?)LN6OHV]lGy'$*>nd[(tb!#",
		};
        
        #endregion Encryption Keys
		
		#region RPX Functions

		/// <summary>
		/// Given strInput = "13" builds "013" if nLength = 3.  Default for nLength is 3.
		/// </summary>
		/// <param name="strInput"></param>
		/// <returns></returns>
		private string ADEBLDPadString(string strInput)
		{
			return ADEBLDPadString(strInput, 3);
		}

		/// <summary>
		/// Given strInput = "13" builds "013" if nLength = 3  Default for nLength is 3.
		/// </summary>
		/// <param name="strInput"></param>
		/// <param name="nLength">Default = 3</param>
		/// <returns></returns>
		private string ADEBLDPadString(string strInput, int nLength /*=3*/)
		{
			return strInput.PadLeft(nLength, '0');
		}

		/// <summary>
		/// Concatenates zero-padded length of sInput to sInput
		/// Given "Hello" returns "004Hello"
		/// If nSize = 5, returns "00004Hello"
		/// Default for nSize is 3.
		/// </summary>
		/// <param name="sInput"></param>
		/// <returns></returns>
		private string ADEBLDB(string sInput)
		{
			return ADEBLDB(sInput, 3);
		}

		/// <summary>
		/// Concatenates zero-padded length of sInput to sInput
		/// Given "Hello" returns "004Hello"
		/// If nSize = 5, returns "00004Hello"
		/// Default for nSize is 3.
		/// </summary>
		/// <param name="sInput"></param>
		/// <param name="nSize"></param>
		/// <returns></returns>
		private string ADEBLDB(string sInput, int nSize /*=3*/)
		{
			int nLen = sInput.Length;
			string sLen = this.ADEBLDPadString(nLen.ToString(), nSize);
			return sLen + sInput;
		}

		/// <summary>
		/// Build protocol header
		/// </summary>
		/// <param name="sWKID"></param>
		/// <param name="sWINH"></param>
		/// <param name="sPRCH"></param>
		/// <param name="sWISH"></param>
		/// <returns></returns>
		private string ADEBHDR(string sWKID, string sWINH, string sPRCH, string sWISH)
		{
			string strResult;
			strResult = sWKID+";"+sWINH+";"+sPRCH+";"+sWISH+";";
			strResult = ADEBLDB(strResult);
			return strResult;
		}
		private string ADEBLDMsg(string cHDR, string cRPC, string cParam)
		{
			string sMult = "";
			return ADEBLDMsg(cHDR, cRPC, cParam, ref sMult);
		}
		private string ADEBLDMsg(string cHDR, string cRPC, string cParam, ref string cMult)
		{
			//Builds RPC message
			//Automatically splits parameters longer than 200 into subscripted array
			string cMSG;
			string sBuild = "";
			string sPiece = "";
			string sBig = "";
			int l;
			int nLength;

			if (cParam == "") 
			{
				cMSG = "0" + cRPC ;
			}
			else
			{
				l = PieceLength(cParam, "^");
				for (int j=1; j <= l; j++) 
				{
					sPiece = Piece(cParam, "^", j);
					if ((j == l) && (sPiece.Length > 200)) 
					{
						//Split up long param into array pieces
						sBig = sPiece;
						sPiece = ".x";
						nLength = sPiece.Length + 1;
						sPiece = ADEBLDPadString(nLength.ToString()) + "2" + sPiece;
						sBuild = sBuild + sPiece;
						int nSubscript = 1;
						string sSubscript ="";
						int nSubLen = 0;
						string sSubLen ="";
						int nBigLen = sBig.Length;
						string sHunk ="";
						int nHunkLen = 0;
						string sHunkLen ="";
						int nChunk = 0;
						do 
						{
							nChunk =  (sBig.Length > 200)?200:sBig.Length ;
							sHunk = sBig.Substring(0, nChunk);
							sBig = sBig.Remove(0, nChunk);
							nBigLen = sBig.Length;
							sSubscript = nSubscript.ToString();
							nSubLen = sSubscript.Length;
							sSubLen = nSubLen.ToString();
							sSubLen = ADEBLDPadString(sSubLen);
							nHunkLen = sHunk.Length;
							sHunkLen = nHunkLen.ToString();
							sHunkLen = ADEBLDPadString(sHunkLen);
							cMult = cMult + sSubLen + sSubscript + sHunkLen + sHunk;
							nSubscript++;
						} while (nBigLen > 0);
					}
					else
					{
						nLength = sPiece.Length +1;
						sPiece = ADEBLDPadString(nLength.ToString()) + "0" + sPiece;
						sBuild = sBuild + sPiece;
					}
				}
				nLength = sBuild.Length;
				string sTotLen = ADEBLDPadString(nLength.ToString(),5);
				if (cMult.Length > 0) 
				{
					cMSG = "1"+ cRPC + "^" +sTotLen + sBuild;
				}
				else
				{
					cMSG = "0"+ cRPC + "^" +sTotLen + sBuild;
				}
			}
			cMSG = ADEBLDB(cMSG, 5);
			cMSG = cHDR + cMSG;
			return cMSG;
		}

		internal string ADEEncryp(string sInput)
		{
			//Encrypt a string
			string strResult;
			string strPercent;
			string strAssoc;
			string strIdix;
			int nPercent;
			int nAssocix;
			int nIdix;
			Debug.Assert(sInput != "");
			System.Random rRand = new Random(DateTime.Now.Second);

			nPercent = rRand.Next(0,10000);
			nPercent += 72439;
			nAssocix = nPercent % 20;
			nAssocix++;
			Debug.Assert(nAssocix < 21);
			strPercent = nPercent.ToString();
			strPercent = strPercent.Substring(1,2);
			nIdix = Convert.ToInt32(strPercent);
			nIdix = nIdix % 20;
			nIdix++;
			Debug.Assert(nIdix < 21);

			const int nEncryptBase = 101;
			strAssoc = LoadKey(nEncryptBase + nAssocix);
			Debug.Assert(strAssoc.Length == 94);
			strIdix = LoadKey(nEncryptBase + nIdix);
			Debug.Assert(strIdix.Length == 94);
			string sEncrypted = "";
			
			foreach (char c in sInput)
			{
				string d = c.ToString();
				int nFindChar = strIdix.IndexOf(c);
				if (nFindChar > -1)
				{
					d = strAssoc.Substring(nFindChar,1);
				}
				sEncrypted += d;
			}

			strResult = (char) (nIdix + 31) + sEncrypted + (char) (nAssocix + 31);

			return strResult;
		}

		internal string ADEDecryp(string sInput)
		{
			//Encrypt a string
			string strAssoc;
			string strIdix;
			int nAssocix;
			int nIdix;
			Debug.Assert(sInput != "");

			//get associator string index
			char cAssocix = sInput[sInput.Length-1];
			nAssocix =(int) cAssocix;
			nAssocix -= 31;
			Debug.Assert(nAssocix < 21);

			//get identifier string index
			char cIdix = sInput[0];
			nIdix = (int) cIdix;
			nIdix -= 31;
			Debug.Assert(nIdix < 21);

			//get associator string
			const int nEncryptBase = 101;
			strAssoc = LoadKey(nEncryptBase + nAssocix);
			Debug.Assert(strAssoc.Length == 94);

			//get identifier string
			strIdix = LoadKey(nEncryptBase + nIdix);
			Debug.Assert(strIdix.Length == 94);

			//translated result
			string sDecrypted = "";
			sInput = sInput.Substring(1, sInput.Length - 2);
			foreach (char c in sInput)
			{
				string d = c.ToString();
				int nFindChar = strAssoc.IndexOf(c);
				if (nFindChar > -1)
				{
					d = strIdix.Substring(nFindChar,1);
				}
				sDecrypted += d;
			}

			return sDecrypted;
		}

		internal string BMXEncrypt(string sInput)
		{

			ASCIIEncoding textConverter = new ASCIIEncoding();
			RijndaelManaged myRijndael = new RijndaelManaged();
			byte[] encrypted;
			byte[] toEncrypt;
			byte[] key;
			byte[] IV;

			string sKey="pouphfoz sfdbqjuvmbwft qizmphfoz";
			string sIV = "Gichin Funakoshi";
			key = textConverter.GetBytes(sKey);
			IV = textConverter.GetBytes(sIV);

			//Get an encryptor.
			ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);
            
			//Encrypt the data.
			MemoryStream msEncrypt = new MemoryStream();
			CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

			//Convert the input data to a byte array.
			toEncrypt = textConverter.GetBytes(sInput);

			//Write all data to the crypto stream and flush it.
			csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
			csEncrypt.FlushFinalBlock();

			//Get encrypted array of bytes.
			encrypted = msEncrypt.ToArray();

			//Convert to string to send to RPMS
			string sEncrypted = "" ;
			byte bTmp;
			string sTmp;
			for (int j =0; j < encrypted.Length; j++)
			{
				bTmp = encrypted[j];
				sTmp = bTmp.ToString();
				sEncrypted += sTmp;
				if (j < (encrypted.Length -1))
					sEncrypted += "~";
			}
			return sEncrypted;
		}

		internal string BMXDecrypt(string sInput)
		{
			try
			{
				byte[] fromEncrypt;
				ASCIIEncoding textConverter = new ASCIIEncoding();
				RijndaelManaged myRijndael = new RijndaelManaged();
				string sRPMSEncrypted = sInput;
				string sBar = "~";
				char[] cBar = sBar.ToCharArray();
				string[] sArray;
				sArray = sRPMSEncrypted.Split(cBar);
				byte[] bRPMSEncrypted = new byte[sArray.GetLength(0)];
				byte[] key;
				byte[] IV;

				//Convert the RPMS-stored string to a byte array
				for (int j = 0; j < sArray.GetLength(0); j++)
				{
					bRPMSEncrypted[j] = Byte.Parse(sArray[j]);
				}

				//Get a decryptor that uses the same key and IV as the encryptor.
				string sKey="pouphfoz sfdbqjuvmbwft qizmphfoz";
				string sIV = "Gichin Funakoshi";
				key = textConverter.GetBytes(sKey);
				IV = textConverter.GetBytes(sIV);
				ICryptoTransform decryptor = myRijndael.CreateDecryptor(key, IV);

				MemoryStream msDecrypt = new MemoryStream(bRPMSEncrypted);
				CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

				fromEncrypt = new byte[bRPMSEncrypted.Length - 2];

				//Read the data out of the crypto stream.
				csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

				int nZ = FindChar(fromEncrypt, (char) 0);

				//Convert the byte array back into a string.
				string sResult;
				if (nZ < 0)
				{
					sResult = textConverter.GetString(fromEncrypt);
				}
				else
				{
					sResult = textConverter.GetString(fromEncrypt, 0, nZ);
				}
				return sResult;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				return "";
			}
		}

		public static int FindChar(byte[] c, char y)
		{
			int n = 0;
			int nRet = -1;
			for (n=0; n < c.Length; n++)
			{
				if (y == (char) c[n])
				{
					nRet = n;
					break;
				}
			}

			return nRet;
		}
		
		public static int FindChar(string s, char y)
		{
			int n = 0;
			int nRet = -1;
			foreach (char c in s)
			{
				if (y == c)
				{
					nRet = n;
					break;
				}
				n++;
			}
			return nRet;
		}


		/// <summary>
		/// Returns index of first instance of sSubString in sString.
		/// If sSubString not found, returns -1.
		/// </summary>
		/// <param name="sString"></param>
		/// <param name="sSubString"></param>
		/// <returns></returns>
		public static int FindSubString(string sString, string sSubString)
		{
			int nFound = -1;
			int nLimit = sString.Length - sSubString.Length + 1;
			if (nLimit < 0)
				return -1;

			int nSubLength = sSubString.Length;
			for (int j=0; j < nLimit; j++)
			{
				if (sString.Substring(j, nSubLength) == sSubString)
				{
					nFound = j;
					break;
				}
			}
			return nFound;
		}

		private string LoadKey(int nID)
		{
			nID -= 102;
			Debug.Assert( nID < 20);
			return m_sKey[nID];
		}
	
		private void OpenConnectionCommon(string sServerAddress)
		{
			try
			{
				m_cServerAddress = sServerAddress;

				//Connect with the server
				TcpClient connector = new TcpClient();

				try 
				{
					connector = new TcpClient();
					connector.Connect(m_cServerAddress, m_nMServerPort);		
				}
				catch (SocketException exSocket)
				{
					string s = exSocket.Message + exSocket.StackTrace;
					throw new BMXNetException(s);
				}				
			
				//Prepare & send the connect message
				string cSend = "TCPconnect^" + m_sNameSpace + "^^";
				int nLen = cSend.Length;
				string sLen = nLen.ToString();
				sLen = sLen.PadLeft(5, '0');
				cSend = "{BMX}" + sLen + cSend;

				NetworkStream ns = connector.GetStream();
				byte[] sendBytes = Encoding.ASCII.GetBytes(cSend);
				ns.Write(sendBytes,0,sendBytes.Length);

				m_pCommSocket = connector;
				return;

			}
			catch (BMXNetException bmxEx)
			{
				throw bmxEx;
			}
			catch (Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
				throw new BMXNetException(s);
			}		
		}//End OpenConnectionCommon

		[SocketPermissionAttribute(SecurityAction.Assert, 
			 Access="Connect",
			 Host="All",
			 Port="All",
			 Transport="All")]
		public bool OpenConnection(string sServerAddress, WindowsIdentity winIdentity)
		{
			try
			{
				OpenConnectionCommon(sServerAddress);
				bool bSecurity;
				try
				{
					bSecurity = SendSecurityRequest(winIdentity);
					
				}
				catch (Exception ex)
				{
					//Close the connection
					SendString(m_pCommSocket, "#BYE#");
					m_pCommSocket.Close();
					m_bConnected = false;
					m_cServerAddress = "";
					throw ex;
				}
				
				m_bConnected = bSecurity;
				return m_bConnected;
			}
			catch (BMXNetException bmxEx)
			{
				throw bmxEx;
			}
			catch (Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
				throw new BMXNetException(s);
			}
		}

        StreamWriter m_LogWriter;
        bool m_bLogging = false;

        public void StartLog()
        {
            try
            {
                if (m_bLogging)
                {
                    throw new Exception("Already logging.");
                }
                string sFile = "BMXLog " + DateTime.Now.DayOfYear.ToString() + " " +
                     DateTime.Now.Hour.ToString() + " " + DateTime.Now.Minute.ToString()
                     + " " + DateTime.Now.Second.ToString() + ".log";
                StartLog(sFile);
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StartLog(string LogFileName)
        {
            try
            {
                if (m_bLogging)
                {
                    throw new Exception("Already logging.");
                }
                m_LogWriter = File.AppendText(LogFileName);
                m_bLogging = true;
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StopLog()
        {
            try
            {
                //Close the writer and underlying file.
                if (m_bLogging == false)
                {
                    return;
                } 
                m_LogWriter.Close();
                m_bLogging = false;
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Log(String logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }

		[SocketPermissionAttribute(SecurityAction.Assert, 
			 Access="Connect",
			 Host="All",
			 Port="All",
			 Transport="All")]
		public bool OpenConnection(string sServerAddress, string sAccess, string sVerify)
		{
			try
			{
				this.OpenConnectionCommon(sServerAddress);

				try
				{
					bool bSecurity = SendSecurityRequest(sAccess, sVerify);
				}
				catch (Exception ex)
				{
					//Close the connection
					SendString(m_pCommSocket, "#BYE#");
					m_pCommSocket.Close();
					m_bConnected = false;
					m_cServerAddress = "";
                    throw ex;
				}
				
				m_bConnected = true;
				return m_bConnected;
			}
			catch (BMXNetException bmxEx)
			{
				throw bmxEx;
			}
			catch (Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
				throw new BMXNetException(s);
			}
		}

		private void SendString(TcpClient tcpClient, string cSendString)
		{
			string sMult = "";
			SendString(tcpClient, cSendString, sMult);
		}

		private void SendString(TcpClient tcpClient, string cSendString, string cMult)
		{
			int nLen = cSendString.Length;
			string sLen = nLen.ToString();
			sLen = sLen.PadLeft(5, '0');
			cSendString = sLen + cSendString;

			nLen += 15;
			sLen = nLen.ToString();
			sLen = sLen.PadLeft(5, '0');

			cSendString = "{BMX}" + sLen + cSendString;
			cSendString = cSendString + cMult;

			NetworkStream ns = tcpClient.GetStream();
			byte[] sendBytes = m_Encoding.GetBytes(cSendString);
			ns.Write(sendBytes,0,sendBytes.Length);
            if (this.m_bLogging == true)
            {
                Log("Sent: " + cSendString, this.m_LogWriter);
            }
			return;
		}

		private string ReceiveString(TcpClient tcpClient)
		{
			NetworkStream ns = tcpClient.GetStream();

            int nTimeOut = this.m_nReceiveTimeout;
			int nCnt = 0;
            int nTimeElapsed = 0;
			while (ns.DataAvailable == false)
			{
				if (nCnt > 9999)
					break;
                if (nTimeElapsed > nTimeOut)
                    break;
				nCnt++;
                nTimeElapsed += 50;
				Thread.Sleep(50);
			}

			Debug.Assert(ns.DataAvailable == true);
			if (ns.DataAvailable == false)
			{
                this.CloseConnection();
				throw new Exception("BMXNetLib.ReceiveString timeout.  Connection Closed.");
				//return "";
			}

			byte[] bReadBuffer = new byte[1024];
			string sReadBuffer = "";
			StringBuilder sbAll = new StringBuilder("", 1024);
			int numberOfBytesRead = 0;

			// Incoming message may be larger than the buffer size.

			bool bFinished = false;
			int nFind = -1;
			bool bStarted = false;
			int lpBuf = 0;
			string sError = "";
			string sAppError = "";
			do
			{
				numberOfBytesRead = ns.Read(bReadBuffer, 0, bReadBuffer.Length); 
				if ((numberOfBytesRead == 1)&&(bStarted == false))
				{
					Thread.Sleep(15);
					numberOfBytesRead += ns.Read(bReadBuffer,1, bReadBuffer.Length-1); 
					//Debug.Write("ReceiveString waiting for data...\n");
				}
				if (bStarted == false)
				{
					//Process error info at beginning of returned string
					int nErrLen = bReadBuffer[0];
					int nAppLen = bReadBuffer[bReadBuffer[0]+1];
					if ((bReadBuffer[2] == 0)&&(bReadBuffer[3] == 0)) 
					{ //special case: M error trap invoked in SND^XWBTCPC
						lpBuf += 2;
					}
					sError = m_Encoding.GetString(bReadBuffer, lpBuf + 1, nErrLen);
					if (sError != "")
					{
						throw new BMXNetException(sError);
					}
					sAppError = m_Encoding.GetString(bReadBuffer, lpBuf+1+nErrLen+1, nAppLen);
					lpBuf += (nErrLen + nAppLen + 2);
					numberOfBytesRead -= (nErrLen + nAppLen + 2);
					bStarted = true;
				}

				nFind = FindChar(bReadBuffer, (char) 4);
				if (nFind > -1)
					bFinished = true;
				Debug.Assert(numberOfBytesRead > -1);
				sReadBuffer = m_Encoding.GetString(bReadBuffer, lpBuf, numberOfBytesRead);
				lpBuf = 0;
				if (nFind > -1)
				{
					sbAll.Append(sReadBuffer, 0, numberOfBytesRead -1);
				}
				else 
				{
					sbAll.Append(sReadBuffer);
				}
			}
			while(bFinished == false);
            if (this.m_bLogging == true)
            {
                Log("Received: " + sbAll.ToString(), this.m_LogWriter);
            }
			return sbAll.ToString();
			
		}
		private bool SendSecurityRequest(WindowsIdentity winIdentity)
		{
			string		strReceive = "";
			string cMSG;
			string sTest;
		
			//Build AV Call
			cMSG = ADEBLDMsg(m_cHDR, "BMX AV CODE", winIdentity.Name);
			SendString(m_pCommSocket, cMSG);

			strReceive = ReceiveString(m_pCommSocket);
			sTest = strReceive.Substring(0,3);


			char[] cDelim = {(char) 13,(char) 10,(char) 0};
			string sDelim = new string(cDelim);
			int nPiece = 1;
			m_cDUZ = Piece(strReceive, sDelim , nPiece);
			if ((m_cDUZ == "0")||(m_cDUZ == ""))
			{
				nPiece = 7;
				string sReason = Piece(strReceive, sDelim, nPiece);
				throw new Exception(sReason);
			}

			return true;		
		}

		private bool SendSecurityRequest(string sAccess, string sVerify)
		{
			string		strReceive = "";
			string		cMSG;
			sAccess = sAccess.ToUpper();
			sVerify = sVerify.ToUpper();
		
			//Build AV Call
			string strAV = sAccess + ";" + sVerify;
			strAV = ADEEncryp(strAV);
			cMSG = ADEBLDMsg(m_cHDR, "XUS AV CODE", strAV);
			SendString(m_pCommSocket, cMSG);

			strReceive = ReceiveString(m_pCommSocket);

			char[] cDelim = {(char) 13,(char) 10,(char) 0};
			string sDelim = new string(cDelim);
			int nPiece = 1;
			m_cDUZ = Piece(strReceive, sDelim , nPiece);
			if ((m_cDUZ == "0")||(m_cDUZ == ""))
			{
				nPiece = 7;
				string sReason = Piece(strReceive, sDelim, nPiece);
				throw new Exception(sReason);
			}
			
			return true;		
		}

		public void CloseConnection()
		{
			if (!m_bConnected) 
			{
				return;
			}
			SendString(m_pCommSocket, "#BYE#");
			m_pCommSocket.Close();
			m_bConnected = false;
			m_cServerAddress = "";
			//			m_cDUZ2 = "";
			m_cDUZ = "";
		}

		public bool Lock(string Variable)
		{
			return Lock(Variable, "", "");
		}

		public bool Lock(string Variable, string Increment)
		{
			return Lock(Variable, Increment, "");
		}

		/// <summary>
		/// Lock a local or global M variable
		/// Returns true if lock is obtained during TimeOut seconds
		/// Use + to increment, - to decrement lock.
		/// </summary>
		/// <param name="Variable"></param>
		/// <param name="Increment"></param>
		/// <param name="TimeOut"></param>
		/// <returns></returns>
		public bool Lock(string Variable, string Increment, string TimeOut)
		{
			try
			{
				string sContext = this.AppContext;
				this.AppContext = "BMXRPC";
				Variable = Variable.Replace("^","~");
				string sRet = "0";
				bool bRet = false;
				string sParam = Variable + "^" + Increment + "^" + TimeOut;
				sRet = TransmitRPC("BMX LOCK", sParam);
				bRet = (sRet == "1")?true:false;
				this.AppContext = sContext;
				return bRet;
			}
			catch (Exception ex)
			{
				string sMsg = ex.Message;
				return false;
			}
		}
		
		static ReaderWriterLock			m_rwl = new ReaderWriterLock();
		private int m_nRWLTimeout = 30000; //30-second default timeout

		/// <summary>
		/// Returns a reference to the internal ReaderWriterLock member.
		/// </summary>
		public ReaderWriterLock BMXRWL
		{
			get
			{
				return m_rwl;
			}
		}

		/// <summary>
		/// Sets and returns the timeout in milliseconds for locking the transmit port.
		/// If the transmit port is unavailable an ApplicationException will be thrown.
		/// </summary>
		public int RWLTimeout
		{
			get
			{
				return m_nRWLTimeout;
			}
			set
			{
				m_nRWLTimeout = value;
			}
		}

		public string TransmitRPC(string sRPC, string sParam, int nLockTimeOut)
		{
			try
			{
				try
				{
					if (m_bConnected == false) 
					{
						throw new BMXNetException("BMXNetLib.TransmitRPC failed because BMXNetLib is not connected to RPMS.");
					}
					Debug.Assert(m_cDUZ != "");
					Debug.Assert(m_pCommSocket != null);

					string sOldAppContext = "";
					if (sRPC.StartsWith("BMX")&&(this.m_cAppContext != "BMXRPC"))
					{
						sOldAppContext  = this.m_cAppContext;
						this.AppContext = "BMXRPC";
					}
					string sMult = "";
					string sSend = ADEBLDMsg(m_cHDR, sRPC, sParam, ref sMult);
					SendString(m_pCommSocket, sSend, sMult);
#if DEBUG   
                    DateTime sendTime = DateTime.Now;
                    Debug.Write("TransmitRPC Sent: " + sSend + "\n");
#endif
                    string strResult = ReceiveString(m_pCommSocket);
#if DEBUG
                    DateTime receiveTime = DateTime.Now;
                    Debug.Write("TransmitRPC Received: " + strResult + "\n");
                    TimeSpan executionTime = receiveTime - sendTime;
                    Debug.Write("Execution Time: " + executionTime.Milliseconds + " ms.\n");
#endif
					if (sOldAppContext != "")
					{
						this.AppContext = sOldAppContext;
					}
					return strResult;				
				}
				catch (Exception ex)
				{
					if (ex.Message == "Unable to write data to the transport connection.")
					{
						m_bConnected = false;
					}
					throw ex;
				}
				finally
				{
				}			
			}
			catch (ApplicationException aex)
			{
				// The writer lock request timed out.
				Debug.Write("TransmitRPC writer lock request timed out.\n");
				throw aex;
			}
			catch (Exception OuterEx)
			{
				throw OuterEx;
			}
		}

		public string TransmitRPC(string sRPC, string sParam)
		{
			try
			{
				return TransmitRPC(sRPC, sParam, m_nRWLTimeout);
			}
			catch (ApplicationException aex)
			{
				throw aex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetLoginFacility()
		{
			try
			{
				if (m_bConnected == false) 
				{
					throw new BMXNetException("BMXNetLib is not connected to RPMS");
				}

				if (m_cLoginFacility != "") 
				{
					return m_cLoginFacility;
				}

				Debug.Assert(m_pCommSocket != null);
				Debug.Assert(m_cDUZ != "");
				SendString(m_pCommSocket, ADEBLDMsg(m_cHDR, "BMXGetFac", m_cDUZ));
				string sFac = ReceiveString(m_pCommSocket);
				m_cLoginFacility = sFac;
				return sFac;
			}
			catch (BMXNetException bmxEx)
			{
				string sMessage =  bmxEx.Message + bmxEx.StackTrace;
				throw new BMXNetException(sMessage);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion RPX Functions

		#region RPX Properties

        /// <summary>
        /// Set and retrieve the timeout, in milliseconds, to receive a response from the RPMS server.
        /// If the retrieve time exceeds the timeout, an exception will be thrown and the connection will be closed.
        /// The default is 30 seconds.
        /// </summary>
        public int ReceiveTimeout
        {
            get { return m_nReceiveTimeout; }
            set { m_nReceiveTimeout = value; }
        }

		public string WKID
		{
			get
			{
				return m_sWKID;
			}
			set
			{
				m_sWKID = value;
			}
		}

		public string PRCH
		{
			get
			{
				return m_sPRCH;
			}
			set
			{
				m_sPRCH = value;
			}
		}

		public string WINH
		{
			get
			{
				return m_sWINH;
			}
			set
			{
				m_sWINH = value;
			}
		}

		public string WISH
		{
			get
			{
				return m_sWISH;
			}
			set
			{
				m_sWISH = value;
			}
		}

		/// <summary>
		/// Gets/sets the Kernel Application context
		/// Throws an exception if unable to set the context. 
		/// </summary>
		public string AppContext
		{
			get
			{
				return m_cAppContext;
			}
			set
			{
				//Send the changed context to RPMS
				if ((m_bConnected == true) && (value != ""))
				{
					try
					{
						string sRPC = ADEEncryp(value);
						string sAuthentication = TransmitRPC("XWB CREATE CONTEXT", sRPC);
						
						if (BMXNetLib.FindSubString(sAuthentication, "does not have access to option") > -1)
						{
							throw new BMXNetException(sAuthentication);
						}

					}
					catch (Exception ex)
					{
						Debug.Write(ex.Message);
						throw ex;
					}
				}
				m_cAppContext = value;
			}
		}

		public bool Connected
		{
			get
			{
				return m_bConnected;
			}
		}

		public string DUZ
		{
			get
			{
				return m_cDUZ;
			}
		}

		public string MServerAddress
		{
			get
			{
				return m_cServerAddress;
			}
		}

		public string MServerNamespace
		{
			get
			{
                return m_sNameSpace;
			}
            set
            {
                m_sNameSpace = value;
            }
		}

		public int MServerPort
		{
			get
			{
				return m_nMServerPort;
			}
			set
			{
				m_nMServerPort = value;
			}
		}

		public string NameSpace
		{
			get
			{
				return m_sNameSpace;
			}
			set
			{
				m_sNameSpace = value;
			}
		}

        /// <summary>
        /// Gets or Sets the Default Encoder to use
        /// </summary>
        public Encoding Encoder
        {
            get
            {
                return this.m_Encoding;
            }
            set
            {
                this.m_Encoding = Encoder;
            }
        }

		#endregion RPX Properties

	}
}
