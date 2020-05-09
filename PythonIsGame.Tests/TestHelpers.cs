using PythonIsGame.Common;
using PythonIsGame.Common.Map;

namespace PythonIsGame.Tests
{
    public static class TestHelpers
    {
        public static Snake EmptyPlayer => new Snake(0, 0, EmptyMap, "name", false);
        public static ChunkedMap EmptyMap => new ChunkedMap(new EmptyMapGenerator());
    }
}
