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
using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Fury
{
    public class GetAgreement : EppBase<GetAgreementResponse>
    {
        private string language;
        private string agreement_version;

        public GetAgreement(string language, string agreement_version)
        {
            this.language = language;
            this.agreement_version = agreement_version;
        }

        public override XmlDocument ToXml()
        {
            var doc = new XmlDocument();
            var root = CreateDocRoot(doc);

            var furyAgreementExtension = new List<EppExtension> { new FuryGetAgreementExtension(language,agreement_version) };

            PrepareExtensionElement(doc, root, furyAgreementExtension);
            doc.AppendChild(root);

            return doc;
        }

        public override GetAgreementResponse FromBytes(byte[] bytes)
        {
            return new GetAgreementResponse(bytes);
        }
    }
}
