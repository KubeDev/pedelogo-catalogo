using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PedeLogo.Catalogo.Api.Config
{
    public class ConfigManager
    {
        static DateTime? _unreadUntil;
        static bool _unHealth;

        public static bool IsRead()
        {
            return !_unreadUntil.HasValue || DateTime.Now > _unreadUntil;
        }

        public static bool IsUnHealth()
        {
            return _unHealth;
        }

        public static void SetUnread(int seconds)
        {
            _unreadUntil = DateTime.Now.AddSeconds(seconds);
        }

        public static void SetUnHealth()
        {
            _unHealth = true;
        }
    }
}
