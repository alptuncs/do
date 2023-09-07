﻿using Do.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Do.MockOverrider.FirstInterface;

public class MockOverriderMockFactory : DefaultMockFactory
{
    public override object Create(IServiceProvider serviceProvider, MockDescriptor mockDescriptor)
    {
        var overrider = serviceProvider.GetRequiredService<MockOverrider>();

        var result = overrider.Get(mockDescriptor.Type) ?? base.Create(serviceProvider, mockDescriptor);

        if (mockDescriptor.Singleton)
        {
            overrider.ResetEventually(mockDescriptor.Type, result);
        }

        return result;
    }
}
