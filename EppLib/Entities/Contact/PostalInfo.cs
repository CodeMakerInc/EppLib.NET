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
namespace EppLib.Entities
{
    public class PostalInfo
    {
        public string m_type;
        public string m_name;

        /// <summary>
        /// If the contact name is an individual, an organization can be entered. Otherwise, it must be omitted.
        /// </summary>
        public string m_org;

        public PostalAddress m_address;
    }
}