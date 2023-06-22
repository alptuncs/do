namespace Do.Architecture;

public class Application : IRunnable
{
    List<string> Phases { get; } = new();
    public Layers Layers { get; } = new();

    void IRunnable.Run()
    {
        // this context will have service collection, application builder, application etc.
        var context = new ApplicationContext();

        // configure phases
        foreach (var layer in Layers.List)
        {
            layer.ConfigurePhases(Phases);
        }

        // apply phases
        foreach (var phase in Phases)
        {
            using (phase.Initialize(context))
            {
                foreach (var layer in Layers.List)
                {
                    layer.ApplyPhase(phase.Name, context);
                }
            }
        }
    }
}
