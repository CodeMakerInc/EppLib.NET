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
using System.Collections.Generic;

namespace EppLib.Entities
{
    public class Host
    {
        public Host(string hostName)
        {
            HostName = hostName;
        }

        public Host()
        {
        }

        public string HostName { get; set; }

        public string Roid { get; set; }

        private readonly List<Status> status = new List<Status>();
        private readonly List<HostAddress> addresses = new List<HostAddress>();

        public string ClId { get; set; }

        public string CrId { get; set; }

        public string CrDate { get; set; }

        public string UpId { get; set; }

        public string UpDate { get; set; }

        public string TrDate { get; set; }

        public IList<Status> Status
        {
            get { return status; }
        }

        public IList<HostAddress> Addresses
        {
            get { return addresses; }
        }
    }
}
