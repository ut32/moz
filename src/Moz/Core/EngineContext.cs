using System;
using System.Runtime.CompilerServices;
using Moz.Common;
using Moz.Utils;

namespace Moz.Core
{
    public static class EngineContext
    {
        #region Properties

        public static IEngine Current
        {
            get
            {
                if (GenericCache<IEngine>.Instance == null)
                    throw new Exception("get engine failed");
                return GenericCache<IEngine>.Instance;
            }
        }

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create(IServiceProvider serviceProvider)
        {
            return GenericCache<IEngine>.Instance ?? (GenericCache<IEngine>.Instance = new Engine.Engine(serviceProvider));
        }

        public static void Replace(IEngine engine)
        {
            GenericCache<IEngine>.Instance = engine;
        }

        #endregion
    }
}