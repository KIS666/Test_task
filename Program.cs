using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace _1
{

    public  class Order
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }
        [Newtonsoft.Json.JsonProperty("customer")]
        public string Customer { get; set; }
        [Newtonsoft.Json.JsonProperty("startTime")]
        public string StartTime { get; set; }
        [Newtonsoft.Json.JsonProperty("finishTime")]
        public string FinishTime { get; set; }
        [Newtonsoft.Json.JsonProperty("points")]
        public List<string> Points { get; set; }
        [Newtonsoft.Json.JsonProperty("car")]
        public  string Car { get; set; }
        [Newtonsoft.Json.JsonProperty("price")]
        public string Price { get; set; }
        [Newtonsoft.Json.JsonProperty("duration")]
        public string Duration { get; set; }
        
    }

    public class Root
    {
        [Newtonsoft.Json.JsonProperty("orders")]
        public static List<Order> Orders { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {          
            string path = Path.Combine(Environment.CurrentDirectory, @"input.txt");
            string json = File.ReadAllText(path);
            var rootobject = JsonConvert.DeserializeObject<Root>(value: json);
            int count = Root.Orders.Count;
            for (int j = 0; j < count; j++)
            {
                if (String.IsNullOrEmpty(Root.Orders[j].Price))
                {
                    Root.Orders[j].Price = "0";
                }

                if (String.IsNullOrEmpty(Root.Orders[j].Duration))
                {
                    string[] TimeA = Root.Orders[j].StartTime.Split(' ');
                    string[] TimeB = Root.Orders[j].FinishTime.Split(' ');
                    string[] TimeA_0 = TimeA[0].Split('.');
                    string[] TimeB_0 = TimeB[0].Split('.');
                    string[] TimeA_1 = TimeA[1].Split(':');
                    string[] TimeB_1 = TimeB[1].Split(':');
                    DateTime Time_Start = new DateTime(Convert.ToInt32(TimeA_0[2]), Convert.ToInt32(TimeA_0[1]), Convert.ToInt32(TimeA_0[0]), Convert.ToInt32(TimeA_1[0]), Convert.ToInt32(TimeA_1[1]), Convert.ToInt32(TimeA_1[2]));
                    DateTime Time_Finish = new DateTime(Convert.ToInt32(TimeB_0[2]), Convert.ToInt32(TimeB_0[1]), Convert.ToInt32(TimeB_0[0]), Convert.ToInt32(TimeB_1[0]), Convert.ToInt32(TimeB_1[1]), Convert.ToInt32(TimeB_1[2]));
                    Root.Orders[j].Duration = Convert.ToString(Time_Finish.Subtract(Time_Start));
                }

                if (Root.Orders[j].Points.Count > 2)
                {
                    string[] Time = Root.Orders[j].Duration.Split(':');
                    int sec1 = Convert.ToInt32(Time[0]) * 3600 + Convert.ToInt32(Time[1]) * 60 + Convert.ToInt32(Time[2]);
                    int sec2 = sec1 / (Root.Orders[j].Points.Count - 1);
                    int hour = sec2 / 3600;
                    int min = sec2 % 3600 / 60;
                    int sec = min / 60;
                    Root.Orders[j].Duration = $"{hour}:{min}:{sec}";

                    TimeSpan Time_Razn = new TimeSpan(Convert.ToInt32(hour), Convert.ToInt32(min), Convert.ToInt32(sec));
                    string[] TimeA = Root.Orders[j].StartTime.Split(' ');
                    string[] TimeB = Root.Orders[j].FinishTime.Split(' ');
                    string[] TimeA_0 = TimeA[0].Split('.');
                    string[] TimeB_0 = TimeB[0].Split('.');
                    string[] TimeA_1 = TimeA[1].Split(':');
                    string[] TimeB_1 = TimeB[1].Split(':');
                    DateTime Time_Start = new DateTime(Convert.ToInt32(TimeA_0[2]), Convert.ToInt32(TimeA_0[1]), Convert.ToInt32(TimeA_0[0]), Convert.ToInt32(TimeA_1[0]), Convert.ToInt32(TimeA_1[1]), Convert.ToInt32(TimeA_1[2]));
                    DateTime Time_Finish = new DateTime(Convert.ToInt32(TimeB_0[2]), Convert.ToInt32(TimeB_0[1]), Convert.ToInt32(TimeB_0[0]), Convert.ToInt32(TimeB_1[0]), Convert.ToInt32(TimeB_1[1]), Convert.ToInt32(TimeB_1[2]));
                    var time1 = Convert.ToString(Time_Start.Add(Time_Razn));
                    var time2 = Convert.ToString(Time_Finish.Subtract(Time_Razn));
                    
                    Order newOrd = new Order();
                    newOrd.Id = Root.Orders[j].Id;
                    newOrd.Customer = Root.Orders[j].Customer;
                    newOrd.StartTime = time2;
                    newOrd.FinishTime = Root.Orders[j].FinishTime;
                    newOrd.Price = Root.Orders[j].Price;
                    newOrd.Car = Root.Orders[j].Car;
                    newOrd.Duration = Root.Orders[j].Duration;
                    Root.Orders[j].FinishTime = time1;

                    List <List<string>> list1 = new List<List<string>>();
                    for (int i = 0; i < Root.Orders[j].Points.Count - 1; i++)
                    {
                        List<string> list2 = new List<string>();
                        list2.Add(Root.Orders[j].Points[i]);
                        list2.Add(Root.Orders[j].Points[i+1]);
                        list1.Add(list2);
                    }
                    
                    Root.Orders[j].Points = list1[0];
                    newOrd.Points = list1[1];
                    Root.Orders.Insert(j + 1, newOrd);
                    count++;
                    j++;
                }
      
            }
            var json2 = JsonConvert.SerializeObject(Root.Orders, Formatting.Indented);
            File.WriteAllText(@"output.json", json2);
            //Console.ReadKey();
        }
    }
}
