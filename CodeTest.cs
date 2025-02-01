using System.Diagnostics;
using System.Reflection;

namespace Testing;

static class CodeTest {
    public static int TestsRan { get; private set; } = 0;
    public static int TestsPassed { get; private set; } = 0;
    public static int TestsFailed { get; private set; } = 0;
    public static List<string> FailedTests { get; private set; } = new List<string>();
    
    static void RunTest(Func<bool> test){
        TestsRan++;
        if (test()) TestsPassed++;
        else {
            TestsFailed++;
            FailedTests.Add(test.Method.Name);
        } 
    }

    public static void UnitTests(IEnumerable<string>? tags = null){
        List<Func<bool>> tests = new List<Func<bool>>();
        MethodInfo[] methods = typeof(CodeTest).GetTypeInfo().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        foreach (MethodInfo method in methods){
            UnitTestAttribute? attr = method.GetCustomAttribute<UnitTestAttribute>();
            if (attr == null) continue;

            if (tags != null){
                if (attr.Tag == null) continue;
                if(!tags.Contains(attr.Tag)) continue;
            }

            tests.Add(method.CreateDelegate<Func<bool>>());
        }

        foreach (Func<bool> test in tests){
            RunTest(test);
        }
    }

    public static void IntegrationTests(IEnumerable<string>? tags = null){
        List<Func<bool>> tests = new List<Func<bool>>();
        MethodInfo[] methods = typeof(CodeTest).GetTypeInfo().GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
        foreach (MethodInfo method in methods){
            IntegrationTestAttribute? attr = method.GetCustomAttribute<IntegrationTestAttribute>();
            if (attr == null) continue;

            if (tags != null){
                if (attr.Tag == null) continue;
                if(!tags.Contains(attr.Tag)) continue;
            }

            tests.Add(method.CreateDelegate<Func<bool>>());
        }

        foreach (Func<bool> test in tests){
            RunTest(test);
        }
    }

    #region Example Tests

    [UnitTest("Math")]
    static bool Adding(){
        int a = 2;
        int b = 2;
        bool result = a + b == 4;
        Debug.Assert(result, "2 + 2 != 4");
        return result;
    }

    [UnitTest("Math")]
    static bool Failing(){
        int a = 2;
        int b = 2;
        bool result = a + b == 1;
        Debug.Assert(result, "2 + 2 != 1");
        return result;
    }

    [IntegrationTest("External Math")]
    static bool Integrated(){
        int a = 2 /*i/o call*/;
        int b = 2;
        bool result = a + b == 1;
        Debug.Assert(result, "2 + 2 != 1");
        return result;
    }

    #endregion
}

#region Attributes

[AttributeUsage(AttributeTargets.Method)]
class UnitTestAttribute : Attribute {
    public string? Tag { get; set; }
    public UnitTestAttribute(string? Tag = null){
        this.Tag = Tag;
    }
}

[AttributeUsage(AttributeTargets.Method)]
class IntegrationTestAttribute : Attribute {
    public string? Tag { get; set; }
    public IntegrationTestAttribute(string? Tag = null){
        this.Tag = Tag;
    }
}

#endregion