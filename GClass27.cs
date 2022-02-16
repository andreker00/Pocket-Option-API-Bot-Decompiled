// Decompiled with JetBrains decompiler
// Type: ns0.GClass27
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System.Collections.Generic;

namespace ns0
{
  public class GClass27
  {
    public int id { get; set; }

    public string symbol { get; set; }

    public string label { get; set; }

    public string type { get; set; }

    public int digits { get; set; }

    public int payout { get; set; }

    public int expiration_step { get; set; }

    public int purchase_time { get; set; }

    public int expiration_count { get; set; }

    public int is_otc { get; set; }

    public int otc_id { get; set; }

    public int real_id { get; set; }

    public int spread { get; set; }

    public string m1 { get; set; }

    public string m5 { get; set; }

    public string m15 { get; set; }

    public List<object> signals { get; set; }

    public int expTime { get; set; }

    public bool isHoliday { get; set; }

    public bool isSchedule { get; set; }

    public bool active { get; set; }

    public List<GClass27.GClass28> timeframes { get; set; }

    public class GClass28
    {
      public int time { get; set; }
    }
  }
}
