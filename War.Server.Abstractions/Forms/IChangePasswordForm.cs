﻿namespace War.Server.Models.Forms
{
    public interface IChangePasswordForm
    {
        string NewPassword { get; set; }
        string OldPassword { get; set; }
    }
}