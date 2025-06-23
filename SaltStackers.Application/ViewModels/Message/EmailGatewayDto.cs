﻿namespace SaltStackers.Application.ViewModels.Message
{
    public class EmailGatewayDto
    {
        public string From { get; set; }

        public string Display { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
