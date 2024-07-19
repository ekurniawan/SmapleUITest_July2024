using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CreditCards.MyUITest
{

    public class DataAdapterDataAttributeDiscoverer : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
            => dataAttribute.GetNamedArgument<bool>("EnableDiscoveryEnumeration");
    }
}
