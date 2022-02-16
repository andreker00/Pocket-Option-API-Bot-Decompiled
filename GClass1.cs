// Decompiled with JetBrains decompiler
// Type: ns0.GClass1
// Assembly: PocketOptionAPI, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 15463397-F5E1-429E-886D-18E774E5D72E
// Assembly location: C:\Users\avh\Desktop\WisyVolatilityPocket\PocketOptionAPI-cleaned.dll

using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ns0
{
  public class GClass1
  {
    private GClass29 gclass29_0;
    private SocketIO socketIO_0;
    private string string_0 = "NA";
    private string string_1 = "NA";
    private string string_2;
    private string string_3;
    private bool bool_0;
    private string string_4;
    private DateTime dateTime_0;
    private DateTime dateTime_1;
    private bool bool_1;
    private double double_0 = -1.0;
    private bool bool_2;
    private bool bool_3;
    private bool bool_4;
    private List<GClass27> list_0;
    private List<string> list_1 = new List<string>();
    private Dictionary<string, GClass22> dictionary_0;
    private Dictionary<string, GClass1.GClass4> dictionary_1;
    private System.Timers.Timer timer_0;
    private bool bool_5;
    private readonly object object_0 = new object();
    private readonly object object_1 = new object();
    private readonly object object_2 = new object();
    private readonly object object_3 = new object();
    private readonly object object_4 = new object();
    private readonly object object_5 = new object();
    private DateTime dateTime_2;
    private const int int_0 = 30000;
    private const int int_1 = 3000;
    private const int int_2 = 10000;
    private const int int_3 = 128;

    public GClass1() => this.dateTime_2 = DateTime.MinValue;

    public event GClass1.GDelegate0 Event_0;

    public event GClass1.GDelegate1 Event_1;

    public event GClass1.GDelegate2 Event_2;

    public event GClass1.GDelegate3 Event_3;

    public event GClass1.GDelegate4 Event_4;

    public event GClass1.GDelegate5 Event_5;

    public event GClass1.GDelegate6 Event_6;

    public event GClass1.GDelegate7 Event_7;

    public event GClass1.GDelegate8 Event_8;

    public event GClass1.GDelegate9 Event_9;

    public event GClass1.GDelegate10 Event_10;

    private void method_0(List<DateTime> list_2)
    {
      string path = Path.Combine(Path.GetTempPath(), "PocketOptionAPI.txt");
      Dictionary<string, object> graph = new Dictionary<string, object>();
      graph.Add("_postCallsTimeStamps", (object) list_2);
      MemoryStream serializationStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) serializationStream, (object) graph);
      serializationStream.Position = 0L;
      string base64String = Convert.ToBase64String(serializationStream.ToArray());
      if (!Directory.Exists(Path.GetDirectoryName(path)))
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      File.WriteAllText(path, base64String);
    }

    private void method_1(out List<DateTime> list_2)
    {
      string path = Path.Combine(Path.GetTempPath(), "PocketOptionAPI.txt");
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      try
      {
        using (MemoryStream serializationStream = new MemoryStream(Convert.FromBase64String(File.ReadAllText(path))))
          dictionary = (Dictionary<string, object>) new BinaryFormatter().Deserialize((Stream) serializationStream);
      }
      catch
      {
      }
      try
      {
        list_2 = (List<DateTime>) dictionary["_postCallsTimeStamps"];
      }
      catch
      {
        list_2 = new List<DateTime>();
      }
    }

    private bool method_2(
      string string_5,
      string string_6,
      bool bool_7,
      string string_7,
      string string_8,
      bool bool_8,
      out string string_9)
    {
      using (new GClass0("Global\\PocketOptionAPILock"))
      {
        List<DateTime> list_2;
        this.method_1(out list_2);
        list_2.Clear();
        while (list_2.Count > 0 && (DateTime.UtcNow - list_2[0]).TotalSeconds > 120.0)
          list_2.RemoveAt(0);
        if (list_2.Count < 3)
        {
          list_2.Add(DateTime.UtcNow);
          string_9 = string_8;
          try
          {
            this.string_1 = string_5;
            this.bool_2 = bool_7;
            this.bool_3 = false;
            byte[] bytes = this.gclass29_0.UploadValues("/login/web-api/", new NameValueCollection()
            {
              {
                "email",
                string_5
              },
              {
                "password",
                string_6
              },
              {
                "partnerEmail",
                string_7
              }
            });
            string_9 = Encoding.ASCII.GetString(bytes);
            if (!(JsonConvert.DeserializeObject(string_9, typeof (GClass6)) is GClass6 gclass6) || gclass6.token == null)
              return false;
            this.string_3 = gclass6.token;
            this.bool_0 = gclass6.belong;
            this.string_4 = gclass6.country;
            if (!bool_8 && !this.bool_0)
              throw new Exception(string_8);
            return true;
          }
          catch (Exception ex)
          {
            if (ex.Message.Contains("The remote name could not be resolved") || ex.Message == "Unable to connect to the remote server")
              list_2.Remove(list_2.Last<DateTime>());
            string_9 = ex.Message;
            return false;
          }
          finally
          {
            this.method_0(list_2);
          }
        }
        else
        {
          string_9 = "Too many login attempts in the last 120 sec. You must wait " + (120 - (int) (DateTime.UtcNow - list_2[0]).TotalSeconds).ToString() + " sec.";
          return false;
        }
      }
    }

    private bool method_3(string string_5, string string_6, int int_4, out string string_7)
    {
      using (new GClass0("Global\\PocketOptionAPILock"))
      {
        List<DateTime> list_2;
        this.method_1(out list_2);
        TimeSpan timeSpan;
        while (list_2.Count > 0)
        {
          timeSpan = DateTime.UtcNow - list_2[0];
          if (timeSpan.TotalSeconds > 120.0)
            list_2.RemoveAt(0);
          else
            break;
        }
        if (list_2.Count < 3)
        {
          list_2.Add(DateTime.UtcNow);
          string_7 = "";
          try
          {
            this.string_1 = string_5;
            this.bool_2 = false;
            this.bool_3 = true;
            byte[] bytes = this.gclass29_0.UploadValues("/login/tournament/", new NameValueCollection()
            {
              {
                "email",
                string_5
              },
              {
                "password",
                string_6
              },
              {
                "tournamentId",
                int_4.ToString()
              }
            });
            string_7 = Encoding.ASCII.GetString(bytes);
            if (!(JsonConvert.DeserializeObject(string_7, typeof (GClass6)) is GClass6 gclass6) || gclass6.token == null)
              return false;
            this.string_3 = gclass6.token;
            return true;
          }
          catch (Exception ex)
          {
            if (ex.Message.Contains("The remote name could not be resolved") || ex.Message == "Unable to connect to the remote server")
              list_2.Remove(list_2.Last<DateTime>());
            string_7 = ex.Message;
            return false;
          }
          finally
          {
            this.method_0(list_2);
          }
        }
        else
        {
          ref string local = ref string_7;
          timeSpan = DateTime.UtcNow - list_2[0];
          string str = "Too many login attempts in the last 120 sec. You must wait " + (120 - (int) timeSpan.TotalSeconds).ToString() + " sec.";
          local = str;
          return false;
        }
      }
    }

    private bool method_4() => true;

    public DateTime DateTime_0 => this.dateTime_1;

    public List<GClass27> List_0
    {
      get
      {
        lock (this.object_3)
          return this.list_0;
      }
    }

    public bool Boolean_0 => this.bool_2;

    public bool Boolean_1 => this.bool_3;

    public string String_0 => "USD";

    public string String_1 => this.string_0;

    public string String_2 => this.string_1;

    public int Int32_0 => 2;

    public double Double_0 => this.double_0;

    public bool Boolean_2 => this.bool_1;

    public bool NotifyAssetsRandomly { get; set; }

    public string String_3 => this.string_4;

    public static DateTime smethod_0(int int_4, DateTime dateTime_3)
    {
      double totalSeconds = (dateTime_3 - new DateTime(dateTime_3.Year, 1, 1)).TotalSeconds;
      double num = totalSeconds - totalSeconds % (double) int_4;
      return new DateTime(dateTime_3.Year, 1, 1).AddSeconds(num);
    }

    public static int smethod_1(GClass1.TimeIntervalEnum timeIntervalEnum_0)
    {
      int num = 0;
      switch (timeIntervalEnum_0)
      {
        case GClass1.TimeIntervalEnum.Second1:
          num = 1;
          break;
        case GClass1.TimeIntervalEnum.Seconds5:
          num = 5;
          break;
        case GClass1.TimeIntervalEnum.Seconds10:
          num = 10;
          break;
        case GClass1.TimeIntervalEnum.Seconds15:
          num = 15;
          break;
        case GClass1.TimeIntervalEnum.Seconds30:
          num = 30;
          break;
        case GClass1.TimeIntervalEnum.Minute1:
          num = 60;
          break;
        case GClass1.TimeIntervalEnum.Minutes2:
          num = 120;
          break;
        case GClass1.TimeIntervalEnum.Minutes3:
          num = 180;
          break;
        case GClass1.TimeIntervalEnum.Minutes5:
          num = 300;
          break;
        case GClass1.TimeIntervalEnum.Minutes10:
          num = 600;
          break;
        case GClass1.TimeIntervalEnum.Minutes15:
          num = 900;
          break;
        case GClass1.TimeIntervalEnum.Minutes30:
          num = 1800;
          break;
        case GClass1.TimeIntervalEnum.Minutes45:
          num = 2700;
          break;
        case GClass1.TimeIntervalEnum.Hour1:
          num = 3600;
          break;
        case GClass1.TimeIntervalEnum.Hours2:
          num = 7200;
          break;
        case GClass1.TimeIntervalEnum.Hours3:
          num = 10800;
          break;
        case GClass1.TimeIntervalEnum.Hours4:
          num = 14400;
          break;
        case GClass1.TimeIntervalEnum.Hours8:
          num = 28800;
          break;
        case GClass1.TimeIntervalEnum.Day1:
          num = 86400;
          break;
      }
      return num;
    }

    public bool method_5(string string_5, bool bool_7) => this.method_6(string_5, "signals2016a@libero.it", "ac39bd23-75f8-4606-a351-3b8b7bd89fbc", "Account not Attached to the partner, please write to wisybinfo@gmail.com or using telegram @WisyB to proceed.", bool_7);

    public bool method_6(
      string string_5_1,
      string string_6,
      string string_7,
      string string_8,
      bool bool_7)
    {
      this.string_2 = string_7;
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GClass1.Class1 class1 = new GClass1.Class1();
        // ISSUE: reference to a compiler-generated field
        class1.gclass1_0 = this;
        this.dateTime_0 = DateTime.MaxValue;
        this.bool_1 = true;
        this.bool_5 = false;
        this.dictionary_1 = new Dictionary<string, GClass1.GClass4>();
        string string_5 = string_5_1.Split(';')[0];
        string string_6_1 = string_5_1.Split(';')[1];
        this.bool_2 = string_5_1.Split(';')[2].ToLower() == "demo";
        int num1;
        if (string_5_1.Split(';')[2].ToLower() != "demo")
          num1 = string_5_1.Split(';')[2].ToLower() != "real" ? 1 : 0;
        else
          num1 = 0;
        this.bool_3 = num1 != 0;
        this.gclass29_0 = new GClass29(30000);
        this.dictionary_0 = new Dictionary<string, GClass22>();
        this.bool_4 = false;
        string str;
        int num2;
        if (!this.bool_3)
          num2 = this.method_2(string_5, string_6_1, this.bool_2, string_6, string_8, bool_7, out str) ? 1 : 0;
        else
          num2 = this.method_3(string_5, string_6_1, Convert.ToInt32(string_5_1.Split(';')[2]), out str) ? 1 : 0;
        if (num2 == 0)
          throw new Exception("Unable to Login: " + str);
        try
        {
          this.socketIO_0 = new SocketIO(File.ReadAllLines("C:\\PO\\PO.txt")[1]);
        }
        catch
        {
          this.socketIO_0 = new SocketIO("https://api-alesbot.po.market:8301/");
        }
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_1 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_2 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_0 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_3 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_4 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.autoResetEvent_5 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class1.bool_0 = false;
        this.list_0 = (List<GClass27>) null;
        this.socketIO_0.OnError += (EventHandler<string>) ((object_6, string_5_2) =>
        {
          if (this.bool_4)
            return;
          lock (this.object_1)
          {
            this.bool_1 = false;
            this.dictionary_1.Clear();
            // ISSUE: reference to a compiler-generated field
            GClass1.GDelegate10 gdelegate100 = this.gdelegate10_0;
            if (gdelegate100 == null)
              return;
            gdelegate100(this, this.bool_1, false, "EVENT_ERROR (" + string_5_2 + ")");
          }
        });
        this.socketIO_0.OnDisconnected += (EventHandler<string>) ((object_6, string_5_3) =>
        {
          if (this.bool_4)
            return;
          lock (this.object_1)
          {
            this.bool_1 = false;
            this.dictionary_1.Clear();
            // ISSUE: reference to a compiler-generated field
            GClass1.GDelegate10 gdelegate100 = this.gdelegate10_0;
            if (gdelegate100 == null)
              return;
            gdelegate100(this, this.bool_1, false, "EVENT_DISCONNECT (" + string_5_3 + ")");
          }
        });
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.OnReconnected += new EventHandler<int>(class1.method_0);
        this.socketIO_0.OnReconnectAttempt += (EventHandler<int>) ((object_6, int_4) =>
        {
          if (this.bool_4)
            return;
          lock (this.object_1)
          {
            this.bool_1 = false;
            this.dictionary_1.Clear();
            // ISSUE: reference to a compiler-generated field
            GClass1.GDelegate10 gdelegate100 = this.gdelegate10_0;
            if (gdelegate100 == null)
              return;
            gdelegate100(this, this.bool_1, false, "EVENT_RECONNECT_ATTEMPT (attempt: " + int_4.ToString() + ")");
          }
        });
        this.socketIO_0.OnReconnectError += (EventHandler<Exception>) ((object_6, exception_0) =>
        {
          if (this.bool_4)
            return;
          lock (this.object_1)
          {
            this.bool_1 = false;
            this.dictionary_1.Clear();
            // ISSUE: reference to a compiler-generated field
            GClass1.GDelegate10 gdelegate100 = this.gdelegate10_0;
            if (gdelegate100 == null)
              return;
            gdelegate100(this, this.bool_1, false, "EVENT_RECONNECT_ERROR (" + exception_0?.ToString() + ")");
          }
        });
        this.socketIO_0.OnReconnectFailed += (EventHandler) ((sender, e) =>
        {
          if (this.bool_4)
            return;
          lock (this.object_1)
          {
            this.bool_1 = false;
            this.dictionary_1.Clear();
            // ISSUE: reference to a compiler-generated field
            GClass1.GDelegate10 gdelegate100 = this.gdelegate10_0;
            if (gdelegate100 == null)
              return;
            gdelegate100(this, this.bool_1, false, "EVENT_RECONNECT_FAILED (attempt: " + e?.ToString() + ")");
          }
        });
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("successauth", new Action<SocketIOResponse>(class1.method_1));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("NotAuthorized", new Action<SocketIOResponse>(class1.method_2));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("updateAssets", new Action<SocketIOResponse>(class1.method_3));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("updateTime", new Action<SocketIOResponse>(class1.method_4));
        this.timer_0 = new System.Timers.Timer(3000.0);
        this.timer_0.Elapsed += (ElapsedEventHandler) ((sender, e) =>
        {
          lock (this.object_1)
          {
            if ((DateTime.UtcNow - this.dateTime_0).TotalSeconds <= 60.0)
              return;
            lock (this.object_0)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.bool_5 || this.gdelegate9_0 == null)
                return;
              this.bool_5 = true;
              this.dateTime_1 = DateTime.UtcNow;
              Task.Factory.StartNew((Action) (() =>
              {
                // ISSUE: reference to a compiler-generated field
                GClass1.GDelegate9 gdelegate90 = this.gdelegate9_0;
                if (gdelegate90 == null)
                  return;
                gdelegate90(this, new Exception("Communication seems stalled."));
              }));
            }
          }
        });
        this.timer_0.Start();
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("updateBalances", new Action<SocketIOResponse>(class1.method_5));
        this.socketIO_0.On("failopenOrder", (Action<SocketIOResponse>) (socketIOResponse_0 =>
        {
          GClass7 error = JsonConvert.DeserializeObject(socketIOResponse_0.GetValue().ToString(), typeof (GClass7)) as GClass7;
          // ISSUE: reference to a compiler-generated field
          GClass1.GDelegate4 gdelegate40 = this.gdelegate4_0;
          if (gdelegate40 == null)
            return;
          gdelegate40(this, error);
        }));
        this.socketIO_0.On("updateHistoryNew", (Action<SocketIOResponse>) (socketIOResponse_0 =>
        {
          GClass16 history = JsonConvert.DeserializeObject(socketIOResponse_0.GetValue().ToString(), typeof (GClass16)) as GClass16;
          // ISSUE: reference to a compiler-generated field
          GClass1.GDelegate5 gdelegate50 = this.gdelegate5_0;
          if (gdelegate50 == null)
            return;
          gdelegate50(this, history);
        }));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("updateOpenedDeals", new Action<SocketIOResponse>(class1.method_6));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("successcloseOrder", new Action<SocketIOResponse>(class1.method_7));
        // ISSUE: reference to a compiler-generated method
        this.socketIO_0.On("updateClosedDeals", new Action<SocketIOResponse>(class1.method_8));
        this.socketIO_0.On("updateStream", (Action<SocketIOResponse>) (socketIOResponse_0 =>
        {
          lock (this.object_1)
          {
            if (!this.bool_1)
              return;
            foreach (GClass11 gclass11 in JsonConvert.DeserializeObject(socketIOResponse_0.GetValue().ToString(), typeof (List<GClass11>)) as List<GClass11>)
            {
              DateTime dateTime1 = new DateTime(1970, 1, 1).Add(TimeSpan.FromSeconds(Convert.ToDouble(gclass11.time)));
              dateTime1 = dateTime1.AddHours(-2.0);
              dateTime1 = DateTime.SpecifyKind(dateTime1, DateTimeKind.Utc);
              lock (this.object_4)
              {
                if (!this.dictionary_1.ContainsKey(gclass11.asset))
                  this.dictionary_1.Add(gclass11.asset, new GClass1.GClass4());
                this.dictionary_1[gclass11.asset].LastTicks.Add(dateTime1, gclass11.price);
                while (this.dictionary_1[gclass11.asset].LastTicks.Count > 128)
                  this.dictionary_1[gclass11.asset].LastTicks.RemoveAt(0);
                foreach (KeyValuePair<int, GClass1.GClass4.GClass5> granularity in this.dictionary_1[gclass11.asset].Granularities)
                {
                  foreach (KeyValuePair<DateTime, double> lastTick in this.dictionary_1[gclass11.asset].LastTicks)
                  {
                    KeyValuePair<DateTime, double> lastManagedTick = granularity.Value.LastManagedTick;
                    if (!(lastManagedTick.Key < lastTick.Key))
                    {
                      lastManagedTick = granularity.Value.LastManagedTick;
                      if (lastManagedTick.Key == lastTick.Key)
                      {
                        lastManagedTick = granularity.Value.LastManagedTick;
                        if (lastManagedTick.Value == lastTick.Value)
                          continue;
                      }
                      else
                        continue;
                    }
                    if (granularity.Key == -1)
                    {
                      // ISSUE: reference to a compiler-generated field
                      GClass1.GDelegate1 gdelegate10 = this.gdelegate1_0;
                      if (gdelegate10 != null)
                        gdelegate10(this, gclass11.asset, this.dictionary_1[gclass11.asset].Granularities[granularity.Key].GUID, gclass11.price, dateTime1, socketIOResponse_0.ToString());
                    }
                    else
                    {
                      GClass1.GClass3 candle = this.dictionary_1[gclass11.asset].Granularities[granularity.Key].LastCandle;
                      DateTime dateTime2 = GClass1.smethod_0(granularity.Key, lastTick.Key);
                      if (candle == null || dateTime2 >= candle.Date)
                      {
                        bool flag = false;
                        if (candle != null && !(dateTime2 > candle.Date))
                        {
                          if (candle.High != lastTick.Value || candle.Low != lastTick.Value || candle.Close != lastTick.Value)
                          {
                            candle.High = Math.Max(candle.High, lastTick.Value);
                            candle.Low = Math.Min(candle.Low, lastTick.Value);
                            candle.Close = lastTick.Value;
                            flag = true;
                          }
                        }
                        else
                        {
                          candle = new GClass1.GClass3();
                          candle.Date = dateTime2;
                          candle.Open = lastTick.Value;
                          candle.Low = lastTick.Value;
                          candle.High = lastTick.Value;
                          candle.Close = lastTick.Value;
                          this.dictionary_1[gclass11.asset].Granularities[granularity.Key].LastCandle = candle;
                          flag = true;
                        }
                        if (flag)
                        {
                          // ISSUE: reference to a compiler-generated field
                          GClass1.GDelegate2 gdelegate20 = this.gdelegate2_0;
                          if (gdelegate20 != null)
                            gdelegate20(this, this.dictionary_1[gclass11.asset].Granularities[granularity.Key].GUID, gclass11.asset, lastTick.Key, candle, socketIOResponse_0.ToString());
                        }
                      }
                    }
                    granularity.Value.LastManagedTick = new KeyValuePair<DateTime, double>(lastTick.Key, lastTick.Value);
                  }
                }
              }
            }
          }
        }));
        this.socketIO_0.On("successopenSocialDeal", (Action<SocketIOResponse>) (socketIOResponse_0 =>
        {
          GClass20 socialTrade = JsonConvert.DeserializeObject(socketIOResponse_0.GetValue().ToString(), typeof (GClass20)) as GClass20;
          // ISSUE: reference to a compiler-generated field
          GClass1.GDelegate6 gdelegate60 = this.gdelegate6_0;
          if (gdelegate60 == null)
            return;
          gdelegate60(this, socialTrade);
        }));
        this.socketIO_0.OnConnected += (EventHandler) (async (sender, e) =>
        {
          if (this.bool_0)
            await this.socketIO_0.EmitAsync("auth", (object) JsonConvert.SerializeObject((object) new GClass9()
            {
              sessionId = this.string_3,
              isDemo = (this.bool_2 ? 1 : 0)
            }));
          else
            await this.socketIO_0.EmitAsync("auth", (object) JsonConvert.SerializeObject((object) new GClass8()
            {
              sessionId = this.string_3,
              isDemo = (this.bool_2 ? 1 : 0),
              api = this.string_2
            }));
        });
        this.socketIO_0.ConnectAsync();
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_1.WaitOne(30000))
          throw new Exception("Unable to Get successauth/NotAuthorized Event");
        // ISSUE: reference to a compiler-generated field
        if (!class1.bool_0)
          throw new Exception("Unauthorized");
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_2.WaitOne(30000))
          throw new Exception("Unable to Get updateAssets Event");
        lock (this.object_3)
        {
          if (this.list_0 == null)
            throw new Exception("Unable to Get updateAssets Data");
        }
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_0.WaitOne(30000))
          throw new Exception("Unable to Get updateTime Event");
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_3.WaitOne(30000))
          throw new Exception("Unable to Get updateBalances Event");
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_4.WaitOne(30000))
          throw new Exception("Unable to Get updateOpenedDeals Event");
        // ISSUE: reference to a compiler-generated field
        if (!class1.autoResetEvent_5.WaitOne(30000))
          throw new Exception("Unable to Get updateClosedDeals Event");
        return true;
      }
      catch (Exception ex)
      {
        this.bool_4 = true;
        throw ex;
      }
    }

    public void method_7()
    {
      try
      {
        this.socketIO_0.EmitAsync("updateOpenedDeals");
      }
      catch
      {
      }
    }

    public void method_8()
    {
      try
      {
        this.socketIO_0.EmitAsync("updateClosedDeals");
      }
      catch
      {
      }
    }

    public GClass22 method_9(string string_5)
    {
      lock (this.object_2)
      {
        GClass22 gclass22;
        this.dictionary_0.TryGetValue(string_5, out gclass22);
        return gclass22;
      }
    }

    public GClass22 method_10(int int_4, string string_5, double double_1)
    {
      lock (this.object_2)
      {
        foreach (KeyValuePair<string, GClass22> keyValuePair in this.dictionary_0)
        {
          if (keyValuePair.Value.requestId == int_4 && keyValuePair.Value.asset == string_5 && keyValuePair.Value.amount == double_1)
            return keyValuePair.Value;
        }
      }
      return (GClass22) null;
    }

    public bool method_11(string string_5, int int_4, out string string_6)
    {
      string_6 = "";
      lock (this.object_4)
      {
        if (!this.dictionary_1.ContainsKey(string_5) || !this.dictionary_1[string_5].Granularities.ContainsKey(int_4))
          return false;
        string_6 = this.dictionary_1[string_5].Granularities[int_4].GUID;
        return true;
      }
    }

    public bool method_12(
      string string_5,
      int int_4,
      int int_5,
      out SortedList<DateTime, GClass1.GClass3> sortedList_0,
      out string string_6)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GClass1.Class3 class3 = new GClass1.Class3();
      // ISSUE: reference to a compiler-generated field
      class3.string_0 = string_5;
      // ISSUE: reference to a compiler-generated field
      class3.gclass1_0 = this;
      lock (this.object_5)
      {
        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now - this.dateTime_2;
        if (timeSpan.TotalMilliseconds < 500.0)
        {
          timeSpan = now - this.dateTime_2;
          Thread.Sleep(500 - (int) timeSpan.TotalMilliseconds);
        }
        this.dateTime_2 = now;
        Math.Min(int_5, 3600);
        int int_4_1;
        switch (int_5)
        {
          case 2700:
            int_4_1 = 900;
            break;
          case 7200:
          case 10800:
          case 14400:
          case 28800:
          case 86400:
            int_4_1 = 3600;
            break;
          default:
            int_4_1 = int_5;
            break;
        }
        string_6 = (string) null;
        // ISSUE: reference to a compiler-generated field
        this.socketIO_0.EmitAsync("subscribeSymbol", (object) class3.string_0);
        // ISSUE: reference to a compiler-generated field
        class3.autoResetEvent_0 = new AutoResetEvent(false);
        // ISSUE: reference to a compiler-generated field
        class3.sortedList_1 = (SortedList<DateTime, double>) null;
        sortedList_0 = new SortedList<DateTime, GClass1.GClass3>();
        // ISSUE: reference to a compiler-generated field
        class3.sortedList_0 = new SortedList<DateTime, GClass1.GClass3>();
        // ISSUE: reference to a compiler-generated method
        GClass1.GDelegate5 gdelegate5 = new GClass1.GDelegate5(class3.method_0);
        try
        {
          if (int_4 > 0)
          {
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
              byte[] data = new byte[2];
              cryptoServiceProvider.GetBytes(data);
              int uint16 = (int) BitConverter.ToUInt16(data, 0);
            }
            this.Event_5 += gdelegate5;
            // ISSUE: reference to a compiler-generated field
            this.socketIO_0.EmitAsync("updateHistory", (object) JsonConvert.SerializeObject((object) new GClass15()
            {
              asset = class3.string_0,
              period = int_4_1
            }));
            // ISSUE: reference to a compiler-generated field
            if (!class3.autoResetEvent_0.WaitOne(30000))
              return false;
            // ISSUE: reference to a compiler-generated field
            SortedList<DateTime, GClass1.GClass3> sortedList0 = class3.sortedList_0;
            // ISSUE: reference to a compiler-generated field
            foreach (KeyValuePair<DateTime, double> keyValuePair in class3.sortedList_1)
            {
              DateTime key = GClass1.smethod_0(int_4_1, keyValuePair.Key);
              GClass1.GClass3 gclass3;
              if (!sortedList0.ContainsKey(key))
              {
                gclass3 = new GClass1.GClass3();
                gclass3.Date = key;
                gclass3.Open = keyValuePair.Value;
                gclass3.High = keyValuePair.Value;
                gclass3.Low = keyValuePair.Value;
                if (!sortedList0.ContainsKey(gclass3.Date))
                  sortedList0.Add(gclass3.Date, gclass3);
                else
                  sortedList0[gclass3.Date] = gclass3;
              }
              else
                gclass3 = sortedList0[key];
              gclass3.Low = Math.Min(gclass3.Low, keyValuePair.Value);
              gclass3.High = Math.Max(gclass3.High, keyValuePair.Value);
              gclass3.Close = keyValuePair.Value;
            }
            if (int_5 == int_4_1)
            {
              sortedList_0 = sortedList0;
            }
            else
            {
              sortedList_0 = new SortedList<DateTime, GClass1.GClass3>();
              foreach (KeyValuePair<DateTime, GClass1.GClass3> keyValuePair in sortedList0)
              {
                DateTime key = GClass1.smethod_0(int_5, keyValuePair.Key);
                if (!sortedList_0.ContainsKey(key))
                {
                  sortedList_0.Add(key, keyValuePair.Value);
                }
                else
                {
                  GClass1.GClass3 gclass3 = sortedList_0[key];
                  gclass3.Low = Math.Min(gclass3.Low, keyValuePair.Value.Low);
                  gclass3.High = Math.Max(gclass3.High, keyValuePair.Value.High);
                  gclass3.Close = keyValuePair.Value.Close;
                }
                GClass1.GClass3 gclass3_1 = sortedList_0[key];
                ++gclass3_1.NumComponents;
                gclass3_1.IsValid = gclass3_1.Date.AddSeconds((double) int_5) > this.dateTime_1 || gclass3_1.NumComponents == int_5 / int_4_1;
              }
            }
            while (sortedList_0.Count > int_4)
              sortedList_0.Remove(sortedList_0.First<KeyValuePair<DateTime, GClass1.GClass3>>().Key);
          }
          lock (this.object_4)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.dictionary_1.ContainsKey(class3.string_0))
            {
              // ISSUE: reference to a compiler-generated field
              this.dictionary_1.Add(class3.string_0, new GClass1.GClass4());
            }
            // ISSUE: reference to a compiler-generated field
            if (!this.dictionary_1[class3.string_0].Granularities.ContainsKey(int_5))
            {
              // ISSUE: reference to a compiler-generated field
              this.dictionary_1[class3.string_0].Granularities.Add(int_5, new GClass1.GClass4.GClass5());
            }
            // ISSUE: reference to a compiler-generated field
            this.dictionary_1[class3.string_0].Granularities[int_5].LastCandle = sortedList_0.Count > 0 ? sortedList_0.Last<KeyValuePair<DateTime, GClass1.GClass3>>().Value : (GClass1.GClass3) null;
            // ISSUE: reference to a compiler-generated field
            string_6 = this.dictionary_1[class3.string_0].Granularities[int_5].GUID;
          }
          return true;
        }
        catch (Exception ex)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GClass1.Class4 class4 = new GClass1.Class4();
          // ISSUE: reference to a compiler-generated field
          class4.class3_0 = class3;
          Exception exception = ex;
          // ISSUE: reference to a compiler-generated field
          class4.exception_0 = exception;
          lock (this.object_0)
          {
            if (!this.bool_5)
            {
              // ISSUE: reference to a compiler-generated field
              if (this.gdelegate9_0 != null)
              {
                this.bool_5 = true;
                // ISSUE: reference to a compiler-generated method
                Task.Factory.StartNew(new Action(class4.method_0));
              }
            }
          }
          return false;
        }
        finally
        {
          this.Event_5 -= gdelegate5;
        }
      }
    }

    public bool method_13(
      string string_5,
      int int_4,
      out SortedList<DateTime, GClass1.GClass3> sortedList_0,
      out string string_6)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GClass1.Class5 class5 = new GClass1.Class5();
      // ISSUE: reference to a compiler-generated field
      class5.gclass1_0 = this;
      int key1 = -1;
      string_6 = (string) null;
      this.socketIO_0.EmitAsync("subscribeSymbol", (object) string_5);
      sortedList_0 = new SortedList<DateTime, GClass1.GClass3>();
      try
      {
        KeyValuePair<DateTime, GClass1.GClass3> keyValuePair1;
        if (int_4 > 0)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          GClass1.Class6 class6 = new GClass1.Class6();
          // ISSUE: reference to a compiler-generated field
          class6.autoResetEvent_0 = new AutoResetEvent(false);
          // ISSUE: reference to a compiler-generated field
          class6.sortedList_0 = (SortedList<DateTime, double>) null;
          // ISSUE: reference to a compiler-generated field
          class6.int_0 = -1;
          using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
          {
            byte[] data = new byte[2];
            cryptoServiceProvider.GetBytes(data);
            // ISSUE: reference to a compiler-generated field
            class6.int_0 = (int) BitConverter.ToUInt16(data, 0);
          }
          // ISSUE: reference to a compiler-generated method
          this.socketIO_0.On("loadHistoryPeriod", (Action<SocketIOResponse>) new Action<object>(class6.method_0));
          DateTime dateTime1 = this.dateTime_1;
          DateTime dateTime = dateTime1.AddSeconds((double) -(int_4 - 1));
          int num = (int) (dateTime1.AddMinutes(1.0) - dateTime).TotalSeconds + 1;
          // ISSUE: reference to a compiler-generated field
          class6.autoResetEvent_0 = new AutoResetEvent(false);
          // ISSUE: reference to a compiler-generated field
          this.socketIO_0.EmitAsync("loadHistoryPeriod", (object) JsonConvert.SerializeObject((object) new GClass12()
          {
            asset = string_5,
            requestId = class6.int_0,
            time = (int) (dateTime1.AddMinutes(1.0) - new DateTime(1970, 1, 1) + TimeSpan.FromHours(2.0)).TotalSeconds,
            offset = num
          })).Wait();
          // ISSUE: reference to a compiler-generated field
          if (!class6.autoResetEvent_0.WaitOne(30000))
            return false;
          // ISSUE: reference to a compiler-generated field
          foreach (KeyValuePair<DateTime, double> keyValuePair2 in class6.sortedList_0)
          {
            GClass1.GClass3 gclass3 = new GClass1.GClass3();
            gclass3.Date = keyValuePair2.Key;
            gclass3.Open = keyValuePair2.Value;
            gclass3.Close = keyValuePair2.Value;
            gclass3.High = keyValuePair2.Value;
            gclass3.Low = keyValuePair2.Value;
            if (!sortedList_0.ContainsKey(gclass3.Date))
              sortedList_0.Add(gclass3.Date, gclass3);
            else
              sortedList_0[gclass3.Date] = gclass3;
          }
          while (sortedList_0.Count > int_4)
          {
            SortedList<DateTime, GClass1.GClass3> sortedList = sortedList_0;
            keyValuePair1 = sortedList_0.First<KeyValuePair<DateTime, GClass1.GClass3>>();
            DateTime key2 = keyValuePair1.Key;
            sortedList.Remove(key2);
          }
          while (sortedList_0.Count < int_4)
          {
            KeyValuePair<DateTime, GClass1.GClass3> keyValuePair3 = sortedList_0.First<KeyValuePair<DateTime, GClass1.GClass3>>();
            GClass1.GClass3 gclass3 = new GClass1.GClass3();
            DateTime key3 = keyValuePair3.Key.AddSeconds(-1.0);
            gclass3.Open = keyValuePair3.Value.Open;
            gclass3.High = keyValuePair3.Value.Open;
            gclass3.Low = keyValuePair3.Value.Open;
            gclass3.Close = keyValuePair3.Value.Open;
            gclass3.IsValid = false;
            if (!sortedList_0.ContainsKey(key3))
              sortedList_0.Add(key3, gclass3);
            else
              sortedList_0[key3] = gclass3;
          }
          DateTime key4 = sortedList_0.First<KeyValuePair<DateTime, GClass1.GClass3>>().Key;
          for (int index = 1; index < int_4; ++index)
          {
            if (!sortedList_0.ContainsKey(key4.AddSeconds((double) (-1 * index))))
            {
              GClass1.GClass3 gclass3 = new GClass1.GClass3();
              DateTime key5 = key4.AddSeconds((double) (-1 * index));
              gclass3.Open = sortedList_0.Values[index - 1].Close;
              gclass3.High = sortedList_0.Values[index - 1].Close;
              gclass3.Low = sortedList_0.Values[index - 1].Close;
              gclass3.Close = sortedList_0.Values[index - 1].Close;
              gclass3.IsValid = false;
              if (!sortedList_0.ContainsKey(key5))
                sortedList_0.Add(key5, gclass3);
              else
                sortedList_0[key5] = gclass3;
            }
          }
        }
        lock (this.object_4)
        {
          if (!this.dictionary_1.ContainsKey(string_5))
            this.dictionary_1.Add(string_5, new GClass1.GClass4());
          if (!this.dictionary_1[string_5].Granularities.ContainsKey(key1))
            this.dictionary_1[string_5].Granularities.Add(key1, new GClass1.GClass4.GClass5());
          GClass1.GClass4.GClass5 granularity = this.dictionary_1[string_5].Granularities[key1];
          GClass1.GClass3 gclass3;
          if (sortedList_0.Count <= 0)
          {
            gclass3 = (GClass1.GClass3) null;
          }
          else
          {
            keyValuePair1 = sortedList_0.Last<KeyValuePair<DateTime, GClass1.GClass3>>();
            gclass3 = keyValuePair1.Value;
          }
          granularity.LastCandle = gclass3;
          string_6 = this.dictionary_1[string_5].Granularities[key1].GUID;
        }
        return true;
      }
      catch (Exception ex)
      {
        // ISSUE: reference to a compiler-generated field
        class5.exception_0 = ex;
        lock (this.object_0)
        {
          if (!this.bool_5)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.gdelegate9_0 != null)
            {
              this.bool_5 = true;
              // ISSUE: reference to a compiler-generated method
              Task.Factory.StartNew(new Action(class5.method_0));
            }
          }
        }
        return false;
      }
      finally
      {
        try
        {
          this.socketIO_0.Off("loadHistoryPeriod");
        }
        catch
        {
        }
      }
    }

    public bool method_14(
      string string_5,
      double double_1,
      bool bool_7,
      int int_4,
      string string_6,
      int int_5,
      int int_6,
      out string string_7,
      out string string_8,
      out ushort ushort_0,
      out bool bool_8,
      out string string_9,
      out DateTime dateTime_3)
    {
      DateTime dateTime1 = this.dateTime_1;
      ref DateTime local = ref dateTime_3;
      DateTime dateTime2 = dateTime1.Date;
      dateTime2 = dateTime2.AddHours((double) dateTime1.Hour);
      DateTime dateTime3 = dateTime2.AddMinutes((double) (dateTime1.Minute + int_4));
      local = dateTime3;
      if ((dateTime_3 - dateTime1).TotalSeconds <= (double) int_6)
        dateTime_3 = dateTime_3.AddMinutes((double) int_5);
      return this.method_15(string_5, double_1, bool_7, false, 0, dateTime_3, out string_7, out string_8, out ushort_0, out bool_8, out string_9);
    }

    public bool method_15(
      string string_5,
      double double_1,
      bool bool_7,
      bool bool_8,
      int int_4,
      DateTime dateTime_3,
      out string string_6,
      out string string_7,
      out ushort ushort_0,
      out bool bool_9,
      out string string_8)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GClass1.Class7 class7 = new GClass1.Class7()
      {
        bool_0 = bool_8,
        int_0 = int_4,
        dateTime_0 = dateTime_3,
        string_0 = string_5,
        double_0 = double_1,
        bool_1 = bool_7,
        gclass1_0 = this
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class7.double_0 = Math.Max(1.0, class7.double_0);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      class7.double_0 = Math.Round(class7.double_0, 2);
      string_6 = "";
      string_7 = "";
      ushort_0 = (ushort) 0;
      bool_9 = false;
      string_8 = "";
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!class7.bool_0 && (class7.dateTime_0 - this.DateTime_0).TotalSeconds < 30.0)
      {
        string_8 = "Local drop -> trade not offered for this duration";
        string_7 = string_8;
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      class7.autoResetEvent_0 = new AutoResetEvent(false);
      // ISSUE: reference to a compiler-generated field
      class7.gclass22_0 = (GClass22) null;
      // ISSUE: reference to a compiler-generated field
      class7.gclass7_0 = (GClass7) null;
      using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
      {
        byte[] data = new byte[2];
        cryptoServiceProvider.GetBytes(data);
        ushort_0 = BitConverter.ToUInt16(data, 0);
      }
      // ISSUE: reference to a compiler-generated field
      class7.ushort_0 = ushort_0;
      // ISSUE: reference to a compiler-generated method
      GClass1.GDelegate3 gdelegate3 = new GClass1.GDelegate3(class7.method_0);
      // ISSUE: reference to a compiler-generated method
      GClass1.GDelegate4 gdelegate4 = new GClass1.GDelegate4(class7.method_1);
      try
      {
        this.Event_3 += gdelegate3;
        this.Event_4 += gdelegate4;
        int digits = -1;
        lock (this.object_3)
        {
          foreach (GClass27 gclass27 in this.list_0)
          {
            // ISSUE: reference to a compiler-generated field
            if (gclass27.symbol == class7.string_0)
            {
              digits = gclass27.digits;
              break;
            }
          }
        }
        if (digits == -1)
        {
          string_7 = "Symbol not found";
          return false;
        }
        GClass21 gclass21 = new GClass21();
        // ISSUE: reference to a compiler-generated field
        gclass21.requestId = (int) class7.ushort_0;
        gclass21.session = this.string_3;
        // ISSUE: reference to a compiler-generated field
        gclass21.asset = class7.string_0;
        // ISSUE: reference to a compiler-generated field
        gclass21.amount = Math.Round(class7.double_0, digits);
        // ISSUE: reference to a compiler-generated field
        if (class7.bool_0)
        {
          // ISSUE: reference to a compiler-generated field
          gclass21.time = class7.int_0;
          gclass21.optionType = 100;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          TimeSpan timeSpan = class7.dateTime_0 - new DateTime(1970, 1, 1) + TimeSpan.FromHours(2.0);
          gclass21.time = (int) timeSpan.TotalSeconds;
          gclass21.optionType = 0;
        }
        // ISSUE: reference to a compiler-generated field
        gclass21.action = class7.bool_1 ? "call" : "put";
        this.socketIO_0.EmitAsync("openOrder", (object) JsonConvert.SerializeObject((object) gclass21));
        bool flag1;
        // ISSUE: reference to a compiler-generated field
        if (!(flag1 = class7.autoResetEvent_0.WaitOne(30000)))
          bool_9 = true;
        bool flag2;
        // ISSUE: reference to a compiler-generated field
        if (flag2 = flag1 & class7.gclass22_0 != null)
        {
          // ISSUE: reference to a compiler-generated field
          string_6 = class7.gclass22_0.id;
        }
        else
        {
          if (bool_9)
          {
            // ISSUE: reference to a compiler-generated field
            class7.autoResetEvent_0.Reset();
            this.method_7();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (flag2 = class7.autoResetEvent_0.WaitOne(30000) & class7.gclass22_0 != null)
            {
              string_8 = "Trade Recover succeeded";
              // ISSUE: reference to a compiler-generated field
              string_6 = class7.gclass22_0.id;
              bool_9 = false;
            }
            else
              bool_9 = true;
          }
          if (!flag2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            string_7 = class7.gclass7_0 != null ? class7.gclass7_0.error : "Error in OpenOrder: no answer received";
          }
        }
        return flag2;
      }
      catch (Exception ex)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GClass1.Class8 class8 = new GClass1.Class8();
        // ISSUE: reference to a compiler-generated field
        class8.class7_0 = class7;
        Exception exception = ex;
        // ISSUE: reference to a compiler-generated field
        class8.exception_0 = exception;
        lock (this.object_0)
        {
          if (!this.bool_5)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.gdelegate9_0 != null)
            {
              this.bool_5 = true;
              // ISSUE: reference to a compiler-generated method
              Task.Factory.StartNew(new Action(class8.method_0));
            }
          }
        }
        return false;
      }
      finally
      {
        this.Event_3 -= gdelegate3;
        this.Event_4 -= gdelegate4;
      }
    }

    public bool method_16(
      string string_5,
      out double double_1,
      out string string_6,
      out string string_7)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GClass1.Class9 class9 = new GClass1.Class9();
      // ISSUE: reference to a compiler-generated field
      class9.string_0 = string_5;
      // ISSUE: reference to a compiler-generated field
      class9.gclass1_0 = this;
      double_1 = -1.0;
      string_6 = "";
      string_7 = "";
      // ISSUE: reference to a compiler-generated field
      class9.gclass7_0 = (GClass7) null;
      // ISSUE: reference to a compiler-generated field
      class9.autoResetEvent_0 = new AutoResetEvent(false);
      // ISSUE: reference to a compiler-generated field
      class9.object_0 = new object();
      // ISSUE: reference to a compiler-generated field
      class9.double_0 = -1.0;
      // ISSUE: reference to a compiler-generated method
      Action<object> callback1 = new Action<object>(class9.method_0);
      // ISSUE: reference to a compiler-generated method
      Action<object> callback2 = new Action<object>(class9.method_1);
      try
      {
        this.socketIO_0.On("successcancelOrder", (Action<SocketIOResponse>) callback1);
        this.socketIO_0.On("failcancelOrder", (Action<SocketIOResponse>) callback2);
        // ISSUE: reference to a compiler-generated field
        this.socketIO_0.EmitAsync("cancelOrder", (object) JsonConvert.SerializeObject((object) new GClass23()
        {
          ticket = class9.string_0
        })).Wait();
        bool flag;
        // ISSUE: reference to a compiler-generated field
        if (flag = class9.autoResetEvent_0.WaitOne(30000))
        {
          // ISSUE: reference to a compiler-generated field
          double_1 = class9.double_0;
          if (double_1 == -1.0)
          {
            flag = false;
            // ISSUE: reference to a compiler-generated field
            string_6 = class9.gclass7_0.error;
          }
          else
            this.method_8();
        }
        else
          string_6 = "Error in OpenOrder: no answer received";
        return flag;
      }
      catch (Exception ex)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GClass1.Class10 class10 = new GClass1.Class10();
        // ISSUE: reference to a compiler-generated field
        class10.class9_0 = class9;
        Exception exception = ex;
        // ISSUE: reference to a compiler-generated field
        class10.exception_0 = exception;
        lock (this.object_0)
        {
          if (!this.bool_5)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.gdelegate9_0 != null)
            {
              this.bool_5 = true;
              // ISSUE: reference to a compiler-generated method
              Task.Factory.StartNew(new Action(class10.method_0));
            }
          }
        }
        return false;
      }
      finally
      {
        this.socketIO_0.Off("successcancelOrder");
        this.socketIO_0.Off("failcancelOrder");
      }
    }

    public void method_17()
    {
      this.bool_4 = true;
      try
      {
        this.timer_0.Stop();
      }
      catch
      {
      }
      try
      {
        this.socketIO_0.Dispose();
      }
      catch
      {
      }
      try
      {
        this.socketIO_0.DisconnectAsync();
      }
      catch
      {
      }
      try
      {
        this.method_4();
      }
      catch
      {
      }
      this.socketIO_0 = (SocketIO) null;
      GC.WaitForPendingFinalizers();
      GC.Collect();
    }

    public enum TimeIntervalEnum
    {
      Second1,
      Seconds5,
      Seconds10,
      Seconds15,
      Seconds30,
      Minute1,
      Minutes2,
      Minutes3,
      Minutes5,
      Minutes10,
      Minutes15,
      Minutes30,
      Minutes45,
      Hour1,
      Hours2,
      Hours3,
      Hours4,
      Hours8,
      Day1,
      Ticks,
    }

    public class GClass2<T> : IComparer<T> where T : IComparable
    {
      public int Compare(T x, T y)
      {
        int num = x.CompareTo((object) y);
        return num == 0 ? 1 : num;
      }
    }

    public class GClass3
    {
      public GClass3()
      {
        this.Date = DateTime.MinValue;
        this.Open = double.NaN;
        this.High = double.NaN;
        this.Low = double.NaN;
        this.Close = double.NaN;
        this.Volume = double.NaN;
        this.IsValid = true;
        this.NumComponents = 0;
        this.Ticks = new SortedList<DateTime, double>((IComparer<DateTime>) new GClass1.GClass2<DateTime>());
      }

      public DateTime Date { get; set; }

      public double Open { get; set; }

      public double High { get; set; }

      public double Low { get; set; }

      public double Close { get; set; }

      public double Volume { get; set; }

      public bool IsValid { get; set; }

      public int NumComponents { get; set; }

      public SortedList<DateTime, double> Ticks { get; set; }

      public object Data { get; set; }
    }

    public class GClass4
    {
      public GClass4()
      {
        this.LastTicks = new SortedList<DateTime, double>((IComparer<DateTime>) new GClass1.GClass2<DateTime>());
        this.Granularities = new Dictionary<int, GClass1.GClass4.GClass5>();
      }

      public SortedList<DateTime, double> LastTicks { get; set; }

      public Dictionary<int, GClass1.GClass4.GClass5> Granularities { get; set; }

      public class GClass5
      {
        public GClass5()
        {
          this.GUID = Guid.NewGuid().ToString();
          this.LastCandle = (GClass1.GClass3) null;
          this.LastManagedTick = new KeyValuePair<DateTime, double>(DateTime.MinValue, double.NaN);
        }

        public string GUID { get; set; }

        public GClass1.GClass3 LastCandle { get; set; }

        public KeyValuePair<DateTime, double> LastManagedTick { get; set; }
      }
    }

    public delegate void GDelegate0(GClass1 sender, List<GClass27> assets);

    public delegate void GDelegate1(
      GClass1 sender,
      string symbol,
      string tickId,
      double spot,
      DateTime spotTime,
      string rawAnswer);

    public delegate void GDelegate2(
      GClass1 sender,
      string candleId,
      string symbol,
      DateTime spotTime,
      GClass1.GClass3 candle,
      string rawAnswer);

    public delegate void GDelegate3(GClass1 sender, GClass22 order);

    public delegate void GDelegate4(GClass1 sender, GClass7 error);

    public delegate void GDelegate5(GClass1 sender, GClass16 history);

    public delegate void GDelegate6(GClass1 sender, GClass20 socialTrade);

    public delegate void GDelegate7(GClass1 sender, DateTime serverTime);

    public delegate void GDelegate8(GClass1 sender, double balance);

    public delegate void GDelegate9(GClass1 sender, Exception ex);

    public delegate void GDelegate10(GClass1 sender, bool active, bool onlyNotify, string message);
  }
}
