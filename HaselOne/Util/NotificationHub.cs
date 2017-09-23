//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.Configuration;
//using BusinessObjects;
//using DAL;
//using DAL_Dochuman;
//using HaselOne.Services.Interfaces;
//using Microsoft.AspNet.SignalR;
//using Microsoft.AspNet.SignalR.Hubs;
//using Microsoft.Practices.Unity;

//namespace HaselOne.Util
//{
//    [HubName("notificationHub")]
//    public class NotificationHub : Hub
//    {
//        [Dependency]
//        public static ICustomerService _cs { get; set; }

//        public int userId;
//        public NotificationHub()
//        {

//            //SqlDependencya();
//        }

//        public static readonly System.Timers.Timer _Timer = new System.Timers.Timer();

//        static NotificationHub()
//        {
//            _Timer.Interval = 2000;
//            _Timer.Elapsed += TimerElapsed;
//            _Timer.Start();
//        }

//        public static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {

//            var hub = GlobalHost.ConnectionManager.GetHubContext("notificationHub");
            
//            hub.Clients.All.receiveNotification(String.Format("{0} - running", DateTime.UtcNow));
//        }

//        public static bool getCountChange(int userId, int notClientCount)
//        {
//            int notificationServerCount = _cs.GetCountNotifications(new NotificationsFilter() { SenderUserId = userId, Desc_Id = true });
//            var hub = GlobalHost.ConnectionManager.GetHubContext("notificationHub");
//            hub.Clients.All.getCountChange(notificationServerCount == notClientCount);
//            return notificationServerCount == notClientCount;
//        }


//        public void setUser(string userId)
//        {
//            this.userId = Convert.ToInt32(userId);
//        }

//        //private void SqlDependencya()
//        //{

//        //    var con = new SqlConnection( WebConfigurationManager.AppSettings["cs"]);

//        //    var cmd = new SqlCommand("Select * from gn_notification", con);
//        //    if (con.State == ConnectionState.Closed)
//        //        con.Open();


//        //    SqlDependency dependency = new SqlDependency(cmd);
//        //    SqlDependency.Start(con.ConnectionString);

//        //    dependency.OnChange += Dependency_OnChange;


//        //    //var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
//        //    //List<string> ls= new List<string>();
//        //    //while (dr.Read())
//        //    //{
//        //    //    ls.Add(dr["NotifyMessage"].ToString()); 
//        //    //}
//        //    con.Dispose();
//        //    cmd.Dispose();


//        //}
//        //private void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
//        //{

//        //    hub.Clients.All.receiveNotification(String.Format("{0} - running", DateTime.UtcNow));
//        //}



//    }
//}