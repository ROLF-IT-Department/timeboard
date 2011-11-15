using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Шестнадцатеричное кодирование HexEncoding
/// </summary>

    public static class HexEncoding
    {
        public static string GetString(byte[] data)
        {
            StringBuilder Results = new StringBuilder();
            foreach (byte b in data)
            {
                Results.Append(b.ToString("X2"));
            }

            return Results.ToString();
        }

        public static byte[] GetBytes(string data)
        {
            // GetString encodes the hex-numbers with two digits
            byte[] Results = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                Results[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }

            return Results;
        }
    }

