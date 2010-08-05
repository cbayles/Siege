﻿/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;

namespace Siege.Foundry
{
    public class ExpressionParameter : IGeneratedParameter
    {
        private readonly Type type;
        private readonly int argIndex;

        public ExpressionParameter(Type type, int argIndex)
        {
            this.type = type;
            this.argIndex = argIndex;
        }

        public int LocalIndex
        {
            get { return argIndex; }
        }

        public Type Type
        {
            get { return type; }
        }
    }
}