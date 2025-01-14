﻿using Do.Architecture;
using Do.Domain.Model;
using Microsoft.Extensions.Configuration;

using static Do.Configuration.ConfigurationLayer;

namespace Do.Domain;

public class DomainLayer : LayerBase<BuildConfiguration>
{
    readonly IAssemblyCollection _assemblyCollection = new AssemblyCollection();
    readonly ITypeCollection _typeCollection = new TypeCollection();
    readonly DomainBuilderOptions _domainBuilderOptions = new();

    protected override PhaseContext GetContext(BuildConfiguration phase) =>
        phase.CreateContextBuilder()
            .Add<IAssemblyCollection>(_assemblyCollection)
            .Add<ITypeCollection>(_typeCollection)
            .Add<DomainBuilderOptions>(_domainBuilderOptions)
            .Build();

    protected override IEnumerable<IPhase> GetPhases()
    {
        yield return new BuildDomain(_assemblyCollection, _typeCollection, _domainBuilderOptions);
    }

    public class BuildDomain(IAssemblyCollection _assemblyCollection, ITypeCollection _typeCollection, DomainBuilderOptions _domainBuilderOptions)
        : PhaseBase<ConfigurationManager>(PhaseOrder.Early)
    {
        protected override void Initialize(ConfigurationManager _)
        {
            var builder = new DomainModelBuilder(_domainBuilderOptions);

            foreach (var assembly in _assemblyCollection)
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    _typeCollection.Add(type);
                }
            }

            var model = builder.BuildFrom(_assemblyCollection, _typeCollection);

            Context.Add<DomainModel>(model);
        }
    }
}