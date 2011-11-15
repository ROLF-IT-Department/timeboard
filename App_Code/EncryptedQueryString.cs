using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
///  ласс дл€ кодировани€/декодировани€ url запроса
/// </summary>
public class EncryptedQueryString :
    System.Collections.Specialized.StringDictionary
{
    public EncryptedQueryString()
    {
        // Nothing to do here
    }

    public EncryptedQueryString(string encryptedData)
    {
        // Decrypt data passed in using DPAPI
        byte[] RawData = HexEncoding.GetBytes(encryptedData);
        byte[] ClearRawData = ProtectedData.Unprotect(
                    RawData, null, DataProtectionScope.LocalMachine);
        string StringData = Encoding.UTF8.GetString(ClearRawData);

        // Split the data and add the contents
        int Index;
        string[] SplittedData = StringData.Split(new char[] { '&' });
        foreach (string SingleData in SplittedData)
        {
            Index = SingleData.IndexOf('=');
            base.Add(
                HttpUtility.UrlDecode(SingleData.Substring(0, Index)),
                HttpUtility.UrlDecode(SingleData.Substring(Index + 1))
            );
        }
    }

    public override string ToString()
    {
        StringBuilder Content = new StringBuilder();

        // Go through the contents and build a 
        // typical query string
        foreach (string key in base.Keys)
        {
            Content.Append(HttpUtility.UrlEncode(key));
            Content.Append("=");
            Content.Append(HttpUtility.UrlEncode(base[key]));
            Content.Append("&");
        }

        // Remove the last '&'
        Content.Remove(Content.Length - 1, 1);

        // Now encrypt the contents using DPAPI
        byte[] EncryptedData = ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(Content.ToString()),
                    null, DataProtectionScope.LocalMachine);

        // Convert encrypted byte array to a URL-legal string

        return HexEncoding.GetString(EncryptedData);
    }
}