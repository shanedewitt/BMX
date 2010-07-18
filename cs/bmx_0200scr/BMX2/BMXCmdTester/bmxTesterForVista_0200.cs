using System;
using IndianHealthService.BMXNet;

namespace SamsStuff.IHS.BMX
{
    class MyFirstApp
    {
        static void Main()
        {
            BMXNetLib ConnectionManager = new BMXNetLib();
            Console.Write("Enter IP Address of Server: ");
            string ip = Console.ReadLine();
            Console.Write("Enter the listener port: ");
            string port = Console.ReadLine();
            int portno = int.Parse(port);
            Console.Write("Enter your Access Code: ");
            string accessCode = Console.ReadLine();
            Console.Write("Enter your Verify Code: ");
            string verifyCode = Console.ReadLine();
            ConnectionManager.MServerPort = portno;
            bool success = ConnectionManager.OpenConnection(ip, accessCode, verifyCode);
            Console.WriteLine("Connected: " + success.ToString() + " DUZ: " + ConnectionManager.DUZ);
            ConnectionManager.AppContext = "BMXRPC";
            string result = ConnectionManager.TransmitRPC("BMX USER", ConnectionManager.DUZ);
            Console.WriteLine("Simple RPC: User Name: " + result);
            
            //string result = 
            ConnectionManager.AppContext = "OR CPRS GUI CHART";
            result = ConnectionManager.TransmitRPC("ORWU NEWPERS","A^1");
            Console.WriteLine("CPRS RPC with Parameters: ");
            Console.WriteLine(result);
            Console.WriteLine();
            Console.WriteLine("SQL Statement");
            //string cmd = "SELECT NAME,SEX,DATE_OF_BIRTH FROM PATIENT";
            string cmd = "SELECT * FROM HOLIDAY";
            RPMSDb dbTables = new RPMSDb(ConnectionManager);
            RPMSDb.RPMSDbResultSet rs = new RPMSDb.RPMSDbResultSet();
            dbTables.Execute(cmd, out rs);
            for (int i = 0; i < rs.data.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < rs.data.GetLength(1); j++)
                {
                    if (rs.data[i, j] != null)
                        Console.Write(rs.data[i, j].ToString() + "\t");
                }
            }
            Console.WriteLine();
            Console.WriteLine("BMX Schema RPC");
            ConnectionManager.AppContext = "BMXRPC";
            BMXNetConnection conn = new BMXNetConnection(ConnectionManager);
            BMXNetCommand cmd2 = (BMXNetCommand) conn.CreateCommand();
            cmd2.CommandText = "BMX DEMO^S^10";
            BMXNetDataAdapter da = new BMXNetDataAdapter();
            da.SelectCommand = cmd2;
            System.Data.DataSet ds = new System.Data.DataSet();
            da.Fill(ds,"BMXNetTable1");
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = ds.Tables["BMXNetTable1"];
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb.Append(dt.Rows[i][j]);
                    sb.Append("\t");
                }
                sb.Append("\n");
            }
            Console.WriteLine();
            Console.Write(sb);
            Console.WriteLine();
            Console.WriteLine("More complicated SQL\n");
            BMXNetCommand cmd3 = (BMXNetCommand)conn.CreateCommand();
            cmd3.CommandText = @"SELECT PATIENT.NAME 'NAME', PATIENT.STATE 'STATE', 
                        STATE.ABBREVIATION 'ABBR', PATIENT.AGE 'AGE' FROM PATIENT, STATE
                        WHERE INTERNAL[PATIENT.STATE] = STATE.BMXIEN MAXRECORDS:5";
            da.SelectCommand = cmd3;
            da.Fill(ds, "BMXNetTable2");
            System.Data.DataTable dt2 = new System.Data.DataTable();
            dt2 = ds.Tables["BMXNetTable2"];
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                sb2.Append(dt2.Columns[i].ColumnName);
                sb2.Append("\t");
            }
            sb2.Append("\n");
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                for (int j = 0; j < dt2.Columns.Count; j++)
                {
                    sb2.Append(dt2.Rows[i][j]);
                    sb2.Append("\t");
                }
                sb2.Append("\n");
            }
            Console.Write(sb2);
            Console.ReadKey();
            ConnectionManager.CloseConnection();
        }
    }
}