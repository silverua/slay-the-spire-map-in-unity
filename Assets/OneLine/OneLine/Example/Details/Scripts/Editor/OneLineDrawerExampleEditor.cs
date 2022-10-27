using UnityEditor;
using OneLine;

namespace OneLine.Examples {
[CustomPropertyDrawer(typeof(OneLineDrawerExample.RootField))]
public class OneLineDrawerExampleEditor : OneLinePropertyDrawer {
}
}