// Decompiled with JetBrains decompiler
// Type: ns0.GClass0
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System;
using System.Threading;

namespace ns0
{
  public class GClass0 : IDisposable
  {
    private Mutex mutex_0;

    public GClass0(string string_0)
    {
      while (true)
      {
        try
        {
          this.mutex_0 = new Mutex(false, string_0);
          this.mutex_0.WaitOne();
          break;
        }
        catch
        {
        }
      }
    }

    public void Dispose()
    {
      this.mutex_0.ReleaseMutex();
      this.mutex_0.Dispose();
      this.mutex_0 = (Mutex) null;
    }
  }
}
