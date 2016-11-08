using System;

namespace DotCom.Tests.Component.TestingUtilities
{
    public static class TestRandom
    {
        #region Private Fields

        private static readonly Random Random = new Random(Guid.NewGuid().ToString().GetHashCode());

        #endregion Private Fields

        #region Public Properties

        public static int Integer => Random.Next();

        public static string String => Guid.NewGuid().ToString("N");

        #endregion Public Properties
    }
}
