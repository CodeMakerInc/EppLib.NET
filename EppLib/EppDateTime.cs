// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Globalization;

namespace EppLib
{
    /// <summary>
    /// Parses EPP date-time values. RFC 5730-5733 require every date-time to be UTC, so values
    /// are returned as UTC (DateTimeKind.Utc). Values with an offset are converted to UTC, and
    /// values without a zone designator, which some registries send, are taken to be UTC.
    /// </summary>
    internal static class EppDateTime
    {
        private const DateTimeStyles Styles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;

        public static bool TryParse(string value, out DateTime result)
        {
            return DateTime.TryParse(value, CultureInfo.InvariantCulture, Styles, out result);
        }

        public static DateTime Parse(string value)
        {
            return DateTime.Parse(value, CultureInfo.InvariantCulture, Styles);
        }
    }
}
