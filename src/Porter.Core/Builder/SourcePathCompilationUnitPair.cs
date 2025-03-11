using org.eclipse.jdt.core.dom;

namespace Porter.Builder;

public record SourcePathCompilationUnitPair(string Path, CompilationUnit CompilationUnit);
