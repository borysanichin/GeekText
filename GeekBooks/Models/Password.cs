using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeekBooks.Models
{
    public class Password
    {
        public string Encode(string password)
        {
            byte[] ArrayOfBytes = new byte[password.Length];
            ArrayOfBytes = System.Text.Encoding.UTF8.GetBytes(password);
            string EncryptedPassword = Convert.ToBase64String(ArrayOfBytes);
                return EncryptedPassword;
        }
        public string Decode(string EncryptedPassword)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder UTF8Decode = encoder.GetDecoder();
            byte[] byteArray = Convert.FromBase64String(EncryptedPassword);
            int CharCount = UTF8Decode.GetCharCount(byteArray, 0, byteArray.Length);
            char[] DecodeChar = new char[CharCount];
            UTF8Decode.GetChars(byteArray, 0, byteArray.Length, DecodeChar,0);
            string DecryptedData = new string(DecodeChar);
            return DecryptedData;
        }
    }
}