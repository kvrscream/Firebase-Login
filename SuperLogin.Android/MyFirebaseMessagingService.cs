using System;
using System.Collections;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;

namespace SuperLogin.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        private readonly string NOTIFICATION_CHANEL_ID = "com.firebase.teste.super_login";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);

            if (!message.Data.GetEnumerator().MoveNext())
            {
                SendNotification(message.Data);
            }
        }

        private void SendNotification(IDictionary<string, string> data)
        {
            string title;
            string body;

            data.TryGetValue("title", out title);
            data.TryGetValue("body", out body);


            SendNotification(title: title, body: body);
        }

        private void SendNotification(string title, string body)
        {
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            if(Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANEL_ID, "Notification chanel",
                    Android.App.NotificationImportance.Default);

                notificationChannel.Description = "Super Login Channel";
                notificationChannel.EnableLights(true);
                notificationChannel.LightColor = Color.Blue;
                notificationChannel.SetVibrationPattern(new long[] { 0, 1000, 500, 1000 });


                notificationManager.CreateNotificationChannel(notificationChannel);


            }

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, NOTIFICATION_CHANEL_ID);

            notificationBuilder.SetAutoCancel(true)
            .SetDefaults(-1)
            .SetSmallIcon(Resource.Drawable.abc_ic_star_black_16dp)
            .SetWhen(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
            .SetContentTitle(title)
            .SetContentText(body);


            notificationManager.Notify(new Random().Next(), notificationBuilder.Build()); 

        }
    }
}
