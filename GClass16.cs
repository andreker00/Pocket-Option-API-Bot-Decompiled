// Decompiled with JetBrains decompiler
// Type: ns0.GClass16
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System.Collections.Generic;

namespace ns0
{
  public class GClass16
  {
    public string asset { get; set; }

    public List<GClass16.GClass17> history { get; set; }

    public List<List<double>> candles { get; set; }

    public int period { get; set; }

    public class GClass17
    {
      public string asset { get; set; }

      public int time { get; set; }

      public double price { get; set; }
    }
  }
}
