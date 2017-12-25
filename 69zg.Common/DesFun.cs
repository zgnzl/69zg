using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace _69zg.Common
{

        public static class DesFun
        {
            /// <summary>
            /// 转换为十六进制
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public static byte[] ToHex(this byte[] data)
            {
                char[] hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                using (MemoryStream memoryStream = new MemoryStream(data.Length * 2))
                {
                    byte[] hexByte;
                    foreach (byte b in data)
                    {
                        hexByte = new byte[2];
                        hexByte[0] = (byte)hexChars[(b & 0xF0) >> 4];//除以16
                        hexByte[1] = (byte)hexChars[b & 0x0F];
                        memoryStream.Write(hexByte, 0, 2);
                    }
                    return memoryStream.ToArray();
                }
            }

            /// <summary>
            /// 由十六进制转换为原形式
            /// </summary>
            /// <param name="hexData"></param>
            /// <returns></returns>
            public static byte[] FromHex(this byte[] hexData)
            {
                if (hexData.Length < 2 || (hexData.Length / (double)2 != Math.Floor(hexData.Length / (double)2)))
                {
                    throw new Exception();
                }

                using (MemoryStream memoryStream = new MemoryStream(hexData.Length / 2))
                {
                    for (int i = 0; i < hexData.Length; i += 2)
                    {
                        byte[] hexPairInDecimal = new byte[2];
                        byte temp = hexData[i];
                        if (temp <= 57)
                        {//0~9
                            hexPairInDecimal[0] = (byte)(temp - 48);
                        }
                        else if (temp <= 70)
                        {//A~F
                            hexPairInDecimal[0] = (byte)(temp - 55);
                        }
                        else
                        {//a~f
                            hexPairInDecimal[0] = (byte)(temp - 87);
                        }
                        temp = hexData[i + 1];
                        if (temp <= 57)
                        {
                            hexPairInDecimal[1] = (byte)(temp - 48);
                        }
                        else if (temp <= 70)
                        {
                            hexPairInDecimal[1] = (byte)(temp - 55);
                        }
                        else
                        {
                            hexPairInDecimal[1] = (byte)(temp - 87);
                        }
                        memoryStream.WriteByte((byte)((hexPairInDecimal[0] << 4) | hexPairInDecimal[1]));
                    }
                    return memoryStream.ToArray();
                }
            }

            /// <summary>
            /// 加密方法 
            /// </summary>
            /// <param name="input">待加密的字符串</param>
            /// <param name="password">加密的密码（只能为8位长）</param>
            /// <param name="ivs">初始化向量（只能为8位长）</param>
            /// <param name="encoding">编码方式</param>
            /// <param name="isBase64orHex">输出是base64格式 还是十六进制格式</param>
            /// <returns>加密之后的字符串</returns>
            public static string Encrypt(string input, string password, string iv, Encoding encoding, bool isBase64orHex)
            {
                byte[] ivs = encoding.GetBytes(iv);//{ 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] key = encoding.GetBytes(password);
                byte[] datas = encoding.GetBytes(input);
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                desCryptoServiceProvider.Mode = CipherMode.CBC;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(key, ivs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(datas, 0, datas.Length);
                        cryptoStream.FlushFinalBlock();
                        if (isBase64orHex)
                            return Convert.ToBase64String(memoryStream.ToArray());
                        else
                        {
                            return encoding.GetString(ToHex(memoryStream.ToArray()));
                        }
                    }
                }
            }

            public static string EnDeString(string input, bool isEncrypt)
            {
                string[] pi = (ConfigurationManager.AppSettings["DesFunParams"]?? "P=2sf$d1s);I=12wesdf4").Split(';');
                if (isEncrypt)
                {
                    return Encrypt(input, pi[0].Remove(0, 2), pi[1].Remove(0, 2), Encoding.UTF8, false);
                }
                else
                {
                    return Decrypt(input, pi[0].Remove(0, 2), pi[1].Remove(0, 2), Encoding.UTF8, false);
                }
            }

            /// <summary>
            /// 解密方法
            /// </summary>
            /// <param name="input">待解密的字符串</param>
            /// <param name="password">加密时用的密码（只能为8位长）</param>
            /// <param name="ivs">初始化向量（只能为8位长）</param>
            /// <param name="encoding">编码方式</param>
            /// <param name="isBase64orHex">加密的字符串是base64编码还是十六进制编码</param>
            /// <returns>解密之后的文本</returns>
            public static string Decrypt(string input, string password, string iv, Encoding encoding, bool isBase64orHex)
            {
                byte[] ivs = encoding.GetBytes(iv);//{ 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] key = encoding.GetBytes(password);
                byte[] datas;
                if (isBase64orHex)
                    datas = Convert.FromBase64String(input);
                else
                {
                    datas = FromHex(encoding.GetBytes(input));
                }
                DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
                desCryptoServiceProvider.Mode = CipherMode.CBC;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(key, ivs), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(datas, 0, datas.Length);
                        cryptoStream.FlushFinalBlock();
                        return encoding.GetString(memoryStream.ToArray());
                    }
                }
            }

            /// <summary>
            /// 转换为十六进制
            /// </summary>
            /// <param name="input">待转换的字符串</param>
            /// <param name="encoding">编码方式</param>
            /// <returns>转换的结果</returns>
            public static string ToHex(string input, Encoding encoding)
            {
                encoding = encoding ?? Encoding.Default;
                byte[] data = ToHex(encoding.GetBytes(input));
                return encoding.GetString(data);
            }

            /// <summary>
            /// 十六进制字符串转换回原形式
            /// </summary>
            /// <param name="input">待转换的字符串</param>
            /// <param name="encoding">编码方式</param>
            /// <returns>原形式</returns>
            public static string FromHex(string input, Encoding encoding)
            {
                encoding = encoding ?? Encoding.Default;
                byte[] hexData = FromHex(encoding.GetBytes(input));
                return encoding.GetString(hexData);
            }
        }
}
