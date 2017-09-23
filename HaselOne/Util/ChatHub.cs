using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObjects;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HaselOne.Util
{
    //public class ChatHub : Hub
    //{
    //    #region Data Members

    //    static List<ChatUserDetail> ConnectedUsers = new List<ChatUserDetail>();
    //    static List<ChatMessageDetail> CurrentMessage = new List<ChatMessageDetail>();

    //    #endregion

    //    #region Methods

    //    public void Connect(string userName)
    //    {
    //        var id = Context.ConnectionId;


    //        if (ConnectedUsers.Count(x => x.UserName == userName) == 0)
    //        {
    //            ConnectedUsers.Add(new ChatUserDetail { ConnectionId = id, UserName = userName });
    //            // send to caller
    //            Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);
    //            // send to all except caller client
    //            Clients.AllExcept(id).onNewUserConnected(id, userName);
    //        }
    //        else
    //        {
    //            SetConnection(userName);
    //        }

    //    }

    //    public void SetConnection(string userName)
    //    {
    //        if (ConnectedUsers.Count(x => x.UserName.Equals(userName)) != 0)
    //        {
    //            ChatUserDetail exist = ConnectedUsers.FirstOrDefault(x => x.UserName.Equals(userName));
    //            ConnectedUsers.Remove(exist);
    //            exist.ConnectionId = Context.ConnectionId;
    //            ConnectedUsers.Add(exist);

    //            // send to caller
    //            Clients.Caller.onConnected(exist.ConnectionId, exist.UserName, ConnectedUsers, CurrentMessage);
    //            // send to all except caller client
    //            Clients.AllExcept(exist.ConnectionId).onNewUserConnected(exist.ConnectionId, userName);
    //        }
    //        else
    //        {
    //            Connect(userName);
    //        }
    //    }

    //    public void SendMessageToAll(string userName, string message)
    //    {
    //        // store last 100 messages in cache
    //        AddMessageinCache(userName, message);

    //        // Broad cast message
    //        Clients.All.messageReceived(userName, message);
    //    }

    //    public void SendPrivateMessage(string toUserId, string message)
    //    {

    //        string fromUserId = Context.ConnectionId;

    //        var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
    //        var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

    //        if (toUser != null && fromUser != null)
    //        {
    //            // send to 
    //            Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

    //            // send to caller user
    //            Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
    //        }

    //    }

    //    //public override System.Threading.Tasks.Task OnDisconnected()
    //    //{
    //    //    var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
    //    //    if (item != null)
    //    //    {
    //    //        ConnectedUsers.Remove(item);

    //    //        var id = Context.ConnectionId;
    //    //        Clients.All.onUserDisconnected(id, item.UserName);

    //    //    }

    //    //    return base.OnDisconnected();
    //    //}


    //    #endregion

    //    #region private Messages

    //    private void AddMessageinCache(string userName, string message)
    //    {
    //        CurrentMessage.Add(new ChatMessageDetail { UserName = userName, Message = message });

    //        if (CurrentMessage.Count > 100)
    //            CurrentMessage.RemoveAt(0);
    //    }

    //    #endregion
    //}




}