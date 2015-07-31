using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

namespace W10_BG_Logon_Changer
{
    public static class Extenstions
    {
        public static MColor ToMediaColor(this DColor color)
        {
            return MColor.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
