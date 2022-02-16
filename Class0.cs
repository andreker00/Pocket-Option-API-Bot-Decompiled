// Decompiled with JetBrains decompiler
// Type: ns0.Class0
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ns0
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
  internal class Class0
  {
    private static ResourceManager resourceManager_0;
    private static CultureInfo cultureInfo_0;

    internal Class0()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Class0.resourceManager_0 == null)
          Class0.resourceManager_0 = new ResourceManager("ns0.Class0", typeof (Class0).Assembly);
        return Class0.resourceManager_0;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Class0.cultureInfo_0;
      set => Class0.cultureInfo_0 = value;
    }

    internal static string String1 => Class0.ResourceManager.GetString(nameof (String1), Class0.cultureInfo_0);

    internal static string String2 => Class0.ResourceManager.GetString(nameof (String2), Class0.cultureInfo_0);
  }
}
