using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Models
{
    public static class SessionManager
    {
        public static user CurrentUser { get; set; } // Информация о текущем пользователе

        public static void Logout()
        {
            CurrentUser = null; // Сбрасываем сессию при выходе
        }
    }
}
