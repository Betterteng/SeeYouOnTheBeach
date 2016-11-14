using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeYouOnTheBeach.Web.Utilities
{
    public static class ExtensionMethods
    {
        public static string ProceedQuotationAndBreaks(this string json)
        {
            return json.Replace("'", "\\'")
                .Replace("\\n", string.Empty)
                .Replace("\\\"", string.Empty);
        }
    }
}