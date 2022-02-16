// Decompiled with JetBrains decompiler
// Type: ns0.GClass29
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using System;
using System.Collections.Specialized;
using System.Net;

namespace ns0
{
  public class GClass29 : WebClient
  {
    private CookieContainer cookieContainer_0;
    private int int_0;
    private string string_0;
    private readonly object object_0 = new object();

    public GClass29(int int_1)
    {
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;
      try
      {
        this.BaseAddress = System.IO.File.ReadAllLines("C:\\PO\\PO.txt")[0];
      }
      catch
      {
        this.BaseAddress = "https://api-bot.po.site/login/web-api";
      }
      this.cookieContainer_0 = new CookieContainer();
      this.int_0 = int_1;
      NameValueCollection c = new NameValueCollection();
      this.Headers.Add("Accept", "application/vnd.api.v1+json");
      this.Headers.Add(c);
    }

    public CookieContainer CookieContainer_0
    {
      get => this.cookieContainer_0;
      set => this.cookieContainer_0 = value;
    }

    public string String_0 => this.string_0;

    protected override WebRequest GetWebRequest(Uri address)
    {
      lock (this.object_0)
      {
        WebRequest webRequest = base.GetWebRequest(address);
        HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
        httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        httpWebRequest.Timeout = this.int_0;
        if (httpWebRequest != null)
        {
          if (httpWebRequest.ContentType == null)
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
          httpWebRequest.CookieContainer = this.cookieContainer_0;
        }
        return webRequest;
      }
    }

    protected override WebResponse GetWebResponse(
      WebRequest request,
      IAsyncResult result)
    {
      lock (this.object_0)
      {
        WebResponse webResponse = base.GetWebResponse(request, result);
        this.method_0(webResponse);
        return webResponse;
      }
    }

    protected override WebResponse GetWebResponse(WebRequest request)
    {
      lock (this.object_0)
      {
        try
        {
          WebResponse webResponse = base.GetWebResponse(request);
          this.method_0(webResponse);
          return webResponse;
        }
        catch (WebException ex)
        {
          return ex.Response;
        }
      }
    }

    private void method_0(WebResponse webResponse_0)
    {
      if (!(webResponse_0 is HttpWebResponse httpWebResponse))
        return;
      CookieCollection cookies = httpWebResponse.Cookies;
      foreach (Cookie cookie in cookies)
      {
        if (cookie.Name == "csrf_token")
        {
          this.string_0 = cookie.Value;
          break;
        }
      }
      this.cookieContainer_0.Add(cookies);
    }
  }
}
