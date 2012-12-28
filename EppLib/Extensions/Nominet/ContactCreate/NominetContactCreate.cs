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

using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.ContactCreate
{
    public class NominetContactCreate : Entities.ContactCreate
    {
		public string TradeName { get; set; }
		public CoType? Type { get; set; }
		public string CompanyNumber { get; set; }
		public YesNoFlag OptOut { get; set; }

		public NominetContactCreate(Contact contact) : base(contact) { }

		public override XmlDocument ToXml()
		{
			var createExt = new NominetContactCreateExtension
			                	{
			                		TradeName = TradeName,
									CompanyNumber = CompanyNumber,
									OptOut = OptOut,
									Type = Type
			                	};

			Extensions.Clear();
			Extensions.Add(createExt);

			return base.ToXml();
		}
    }
}