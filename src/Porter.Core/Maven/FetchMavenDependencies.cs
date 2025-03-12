using java.util;
using org.eclipse.aether.collection;
using org.eclipse.aether.graph;
using org.eclipse.aether.resolution;
using org.eclipse.aether.util.artifact;
using org.eclipse.aether.util.filter;
using org.eclipse.aether;
using org.eclipse.aether.artifact;

namespace Porter.Maven;

public static class FetchMavenDependencies
{
    public static List<string> Execute(IList<MavenRepositoryItem> repos, IList<string> deps)
    {
        var listReferenceJars = new List<string>();
        var mavenReference = new List<MavenReference>();

        foreach (var dep in deps)
        {
            mavenReference.Add(Assign(dep));
        }

        var maven = new IkvmMavenEnvironment(repos);
        var session = maven.CreateRepositorySystemSession(false);

        var graph = ResolveCompileDependencyGraph(maven, session, repos, mavenReference);
        CollectReferences(listReferenceJars, graph);


        return listReferenceJars;
    }

    private static void CollectReferences(IList<string> output, DependencyNode node)
    {

        var artifact = node.getArtifact();
        if (artifact != null && artifact.getExtension() == "jar")
        {
            output.Add(artifact.getFile().getAbsolutePath());
        }


        for (int i = 0; i < node.getChildren().size(); i++)
        {
            var child = (DependencyNode)node.getChildren().get(i);
            CollectReferences(output, child);
        }
    }

    private static DependencyNode ResolveCompileDependencyGraph(IkvmMavenEnvironment maven,
        RepositorySystemSession session,
        IList<MavenRepositoryItem> repositories,
        IList<MavenReference> deps)
    {
        var dependencies = new Dependency[deps.Count];
        for (int i = 0; i < deps.Count; i++)
        {
            dependencies[i] = new Dependency(
                new DefaultArtifact(
                    deps[i].GroupId,
                    deps[i].ArtifactId,
                    deps[i].Classifier,
                    "jar",
                    deps[i].Version),
                deps[i].Scope, deps[i].Optional ? java.lang.Boolean.TRUE : java.lang.Boolean.FALSE, new java.util.ArrayList());
        }

        var filter = DependencyFilterUtils.classpathFilter(JavaScopes.COMPILE, JavaScopes.RUNTIME, JavaScopes.COMPILE, JavaScopes.PROVIDED, JavaScopes.TEST);
        var result = maven.RepositorySystem.resolveDependencies(
            session,
            new DependencyRequest(
                new CollectRequest(Arrays.asList(dependencies), null, maven.Repositories),
                filter));

        var root = (DefaultDependencyNode)result.getRoot();
        return root;
    }

    private static MavenReference Assign(string dep)
    {
        var mavenRef = new MavenReference();

        if (!string.IsNullOrEmpty(dep))
        {
            var artifact = dep.Split(':');
            if (artifact.Length == 2 || artifact.Length == 3)
            {
                var groupId = artifact[0];
                var artifactId = artifact[1];
                var version = artifact.Length >=3 ? artifact[2] : string.Empty;

                mavenRef.GroupId = groupId;
                mavenRef.ArtifactId = artifactId;
                mavenRef.Version = version;

                return mavenRef;
            }
        }

        throw new InvalidOperationException("Empty string passed to Assign");
    }
}
