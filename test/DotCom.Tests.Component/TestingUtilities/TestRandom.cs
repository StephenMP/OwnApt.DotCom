using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotCom.Tests.Component.TestingUtilities
{
    public static class TestRandom
    {
        private static Random Random = new Random(Guid.NewGuid().ToString().GetHashCode());

        public static int Integer => Random.Next();

        public static string String => Guid.NewGuid().ToString("N");
    }
}
