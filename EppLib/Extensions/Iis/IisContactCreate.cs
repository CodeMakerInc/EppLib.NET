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
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Iis
{
    public class IisContactCreate : Entities.ContactCreate
    {
        public string OrganizationNumber { get; set; }
        public string VatNumber { get; set; }

        public IisContactCreate(Contact contact) : base(contact) { }

        public override XmlDocument ToXml()
        {
            var createExt = new IisContactCreateExtension
                                {
                                    OrganizationNumber = OrganizationNumber,
                                    VatNumber = VatNumber
                                };

            Extensions.Clear();
            Extensions.Add(createExt);

            return base.ToXml();
        }
    }
}