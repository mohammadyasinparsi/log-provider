using System.Collections.Generic;

namespace Kernel.LogProvider.Helpers
{
    public static class MobileHelper
    {
        private static readonly HashSet<string> MobileAndTabletsNames = new HashSet<string>
        {
            // Mobiles : https://github.com/wangkanai/Detection/blob/3783661c52f4dde28f15efba90b410bf3481fa64/src/Collections/MobileCollection.cs#L8 
            "iphone",
            "mobile",
            "blackberry",
            "phone",
            "smartphone",
            "webos",
            "ipod",
            "lge vx",
            "midp",
            "maemo",
            "mmp",
            "netfront",
            "hiptop",
            "nintendo DS",
            "novarra",
            "openweb",
            "opera mobi",
            "opera mini",
            "palm",
            "psp",
            "smartphone",
            "symbian",
            "up.browser",
            "up.link",
            "wap",
            "windows ce",
            "windows phone",

            // Tablets : https://github.com/wangkanai/Detection/blob/3783661c52f4dde28f15efba90b410bf3481fa64/src/Collections/TabletCollection.cs#L8
            "tablet",
            "ipad",
            "playbook",
            "hp-tablet",
            "kindle",
            "sm-t",
            "kfauwi"
        };

        /// <summary>
        /// Checks if the device is mobile or tablet.
        /// </summary>
        /// <param name="deviceName">Device name e.g : Iphone</param>
        /// <returns><c>ture</c> if device is mobile or tablet, else <c>false</c>.</returns>
        public static bool IsMobile(string deviceName)
        {
            if (string.IsNullOrEmpty(deviceName)) return false;

            return MobileAndTabletsNames.Contains(deviceName.ToLower());
        }
    }
}