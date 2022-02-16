// Decompiled with JetBrains decompiler
// Type: ns0.GClass13
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System.Collections.Generic;

namespace ns0
{
  public class GClass13
  {
    public int requestId { get; set; }

    public string asset { get; set; }

    public List<GClass13.GClass14> data { get; set; }

    public class GClass14
    {
      public string asset { get; set; }

      public int time { get; set; }

      public double price { get; set; }
    }
  }
}
