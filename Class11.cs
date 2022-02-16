// Decompiled with JetBrains decompiler
// Type: ns0.Class11
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;

namespace ns0
{
  internal class Class11
  {
    internal static byte[] smethod_0(Stream stream_0)
    {
      byte num1 = (byte) stream_0.ReadByte();
      byte[] numArray = new byte[stream_0.Length - 1L];
      stream_0.Read(numArray, 0, numArray.Length);
      if (((int) num1 & 1) != 0)
      {
        DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
        byte[] dst1 = new byte[8];
        Buffer.BlockCopy((Array) numArray, 0, (Array) dst1, 0, 8);
        cryptoServiceProvider.IV = dst1;
        byte[] dst2 = new byte[8];
        Buffer.BlockCopy((Array) numArray, 8, (Array) dst2, 0, 8);
        bool flag = true;
        foreach (byte num2 in dst2)
        {
          if (num2 != (byte) 0)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          dst2 = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();
        cryptoServiceProvider.Key = dst2;
        numArray = cryptoServiceProvider.CreateDecryptor().TransformFinalBlock(numArray, 16, numArray.Length - 16);
      }
      if (((int) num1 & 2) != 0)
      {
        try
        {
          MemoryStream memoryStream1 = new MemoryStream(numArray);
          DeflateStream deflateStream = new DeflateStream((Stream) memoryStream1, CompressionMode.Decompress);
          MemoryStream memoryStream2 = new MemoryStream((int) memoryStream1.Length * 2);
          int count1 = 1000;
          byte[] buffer = new byte[1000];
          int count2;
          do
          {
            count2 = deflateStream.Read(buffer, 0, count1);
            if (count2 > 0)
              goto label_12;
label_11:
            continue;
label_12:
            memoryStream2.Write(buffer, 0, count2);
            goto label_11;
          }
          while (count2 >= count1);
          numArray = memoryStream2.ToArray();
        }
        catch (Exception ex)
        {
        }
      }
      return numArray;
    }
  }
}
