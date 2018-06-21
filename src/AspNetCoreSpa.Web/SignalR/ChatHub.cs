using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreSpa.Core.Entities;

namespace AspNetCoreSpa.Web.SignalR
{
    public class Chat : Hub
    {

        UserManager<ApplicationUser> _userManager;

        public Chat(UserManager<ApplicationUser> userManager) : base()
        {
            _userManager = userManager;
        }
        public async Task Send(string message)
        {
            await Clients.All.SendAsync("Send", message);
        }

   
        public override async Task OnConnectedAsync()
        {
            var user = _userManager.GetUserAsync(Context.User).Result;
            
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");  //here instead of "SignalR Users" I'd like to use userId. 
            //Unfortunately the user is not authenticated (before I login in the application)
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }


    }
} 