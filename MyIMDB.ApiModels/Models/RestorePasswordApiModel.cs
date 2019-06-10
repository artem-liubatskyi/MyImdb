using System;
using System.Collections.Generic;
using System.Text;

namespace MyIMDB.ApiModels.Models
{
    public class RestorePasswordApiModel
    {
        public string passwordHash { get; set; }
        public string newPassword { get; set; }
    }
}
