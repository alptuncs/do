﻿namespace Do.Test.ExceptionHandling;

public class HandlingExceptions : TestServiceSpec
{
    [Test(Description = "Actual behaviour is not testable, this test is included only for documentation and to improve coverage")]
    public void HandledException_is_handled_by_default()
    {
        var singleton = GiveMe.The<Singleton>();

        var task = () => singleton.TestException(handled: true);

        task.ShouldThrow<TestServiceHandledException>();
    }
}
