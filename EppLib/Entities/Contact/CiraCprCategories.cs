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

namespace EppLib.Entities
{
    public static class CiraCprCategories
    {
        public const string CCT = "CCT";
        public const string RES = "RES";
        public const string CCO = "CCO";
        public const string ABO = "ABO";
        public const string TDM = "TDM";
        public const string MAJ = "MAJ";
        public const string GOV = "GOV";
        public const string LGR = "LGR";
        public const string TRS = "TRS";
        public const string PRT = "PRT";
        public const string ASS = "ASS";
        public const string TRD = "TRD";
        public const string PLT = "PLT";
        public const string EDU = "EDU";
        public const string LAM = "LAM";
        public const string HOP = "HOP";
        public const string INB = "INB";
        public const string OMK = "OMK";


        public static CiraCprCategory[] Items = new[]
                                                {
                                                    new CiraCprCategory(CCT,"Canadian Citizen"), 
                                                    new CiraCprCategory(RES,"Permanent Resident of Canada"), 
                                                    new CiraCprCategory(CCO,"Corporation"), 
                                                    new CiraCprCategory(ABO,"Aboriginal Peoples"), 
                                                    new CiraCprCategory(TDM,"Trade-mark registered in Canada (by a non-Canadian owner)"), 
                                                    new CiraCprCategory(MAJ,"Her Majesty the Queen"), 
                                                    new CiraCprCategory(GOV,"Government"), 
                                                    new CiraCprCategory(LGR,"Legal Representative of a Canadian Citizen or Permanent Resident"), 
                                                    new CiraCprCategory(TRS,"Trust stablished in Canada"), 
                                                    new CiraCprCategory(PRT,"Partnership registered in Canada"), 
                                                    new CiraCprCategory(ASS,"Canadian Unincorporated Association"), 
                                                    new CiraCprCategory(TRD,"Canadian Trade Union"), 
                                                    new CiraCprCategory(PLT,"Canadian Political Party"), 
                                                    new CiraCprCategory(EDU,"Canadian Educational Institution"), 
                                                    new CiraCprCategory(LAM,"Canadian Library, Archive or Museum"), 
                                                    new CiraCprCategory(HOP,"Canadian Hospital"), 
                                                    new CiraCprCategory(INB,"Indian Band recognized by the Indian Act of Canada"), 
                                                    new CiraCprCategory(OMK,"Offical Mark registered in Canada"), 
    };

        public static CiraCprCategory[] IndividualCategories = new[]
                                                {
                                                    new CiraCprCategory(CCT,"Canadian Citizen"), 
                                                    new CiraCprCategory(RES,"Permanent Resident of Canada")
                                                    
    };


        public static CiraCprCategory GetByCode(string code)
        {
            switch (code)
            {
                case CCT: return new CiraCprCategory(CCT, "Canadian Citizen");
                case RES:
                    return new CiraCprCategory(RES, "Permanent Resident of Canada");
                case CCO:
                    return new CiraCprCategory(CCO, "Corporation");
                case ABO:
                    return new CiraCprCategory(ABO, "Aboriginal Peoples");
                case TDM:
                    return new CiraCprCategory(TDM, "Trade-mark registered in Canada (by a non-Canadian owner)");
                case MAJ:
                    return new CiraCprCategory(MAJ, "Her Majesty the Queen");
                case GOV:
                    return new CiraCprCategory(GOV, "Government");
                case LGR:
                    return new CiraCprCategory(LGR, "Legal Representative of a Canadian Citizen or Permanent Resident");
                case TRS:
                    return new CiraCprCategory(TRS, "Trust stablished in Canada");
                case PRT:
                    return new CiraCprCategory(PRT, "Partnership registered in Canada");
                case ASS:
                    return new CiraCprCategory(ASS, "Canadian Unincorporated Association");
                case TRD:
                    return new CiraCprCategory(TRD, "Canadian Trade Union");
                case PLT:
                    return new CiraCprCategory(PLT, "Canadian Political Party");
                case EDU:
                    return new CiraCprCategory(EDU, "Canadian Educational Institution");
                case LAM:
                    return new CiraCprCategory(LAM, "Canadian Library, Archive or Museum");
                case HOP:
                    return new CiraCprCategory(HOP, "Canadian Hospital");
                case INB:
                    return new CiraCprCategory(INB, "Indian Band recognized by the Indian Act of Canada");
                case OMK:
                    return new CiraCprCategory(OMK, "Offical Mark registered in Canada");

            }

            throw new Exception("Invalid CPR Code");
        }
    }
}
