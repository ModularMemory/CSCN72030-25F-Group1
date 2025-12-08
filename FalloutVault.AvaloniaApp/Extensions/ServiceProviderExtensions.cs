using FalloutVault.AvaloniaApp.Services.Interfaces;
using FalloutVault.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FalloutVault.AvaloniaApp.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider StartDeviceMessageLogger(this IServiceProvider provider)
    {
        _ = provider.GetRequiredService<IDeviceMessageLogger>();

        return provider;
    }

    /// <inheritdoc cref="IDeviceFactory.AddDevices" />
    public static IServiceProvider AddDevices(this IServiceProvider provider)
    {
        foreach (var deviceFactory in provider.GetServices<IDeviceFactory>())
        {
            deviceFactory.AddDevices();
        }

        return provider;
    }

    /// <inheritdoc cref="IDeviceFactory.InitializeDevices" />
    public static IServiceProvider InitializeDevices(this IServiceProvider provider)
    {
        foreach (var deviceFactory in provider.GetServices<IDeviceFactory>())
        {
            deviceFactory.InitializeDevices();
        }

        return provider;
    }

    /// <inheritdoc cref="IDeviceController.Start(TimeSpan)" />
    public static IServiceProvider StartDeviceController(this IServiceProvider provider, TimeSpan pollingInterval)
    {
        var controller = provider.GetRequiredService<IDeviceController>();
        controller.Start(pollingInterval);

        return provider;
    }
}