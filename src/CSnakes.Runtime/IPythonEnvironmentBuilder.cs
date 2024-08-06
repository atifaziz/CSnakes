﻿using Microsoft.Extensions.DependencyInjection;

namespace CSnakes.Runtime;

/// <summary>
/// Represents a builder for creating Python environments.
/// </summary>
public interface IPythonEnvironmentBuilder
{
    public IServiceCollection Services { get; }

    /// <summary>
    /// Sets the virtual environment path for the Python environment being built.
    /// </summary>
    /// <param name="path">The path to the virtual environment.</param>
    /// <param name="ensureVirtualEnvironment">Indicates whether to ensure the virtual environment exists.</param>
    /// <returns>The current instance of the <see cref="IPythonEnvironmentBuilder"/>.</returns>
    IPythonEnvironmentBuilder WithVirtualEnvironment(string path, bool ensureVirtualEnvironment = true);

    /// <summary>
    /// Sets the home directory for the Python environment being built.
    /// </summary>
    /// <param name="home">The home directory path.</param>
    /// <returns>The current instance of the <see cref="IPythonEnvironmentBuilder"/>.</returns>
    IPythonEnvironmentBuilder WithHome(string home);

    /// <summary>
    /// Gets the options for the Python environment being built.
    /// </summary>
    /// <returns>The <see cref="PythonEnvironmentOptions"/> for the current environment.</returns>
    PythonEnvironmentOptions GetOptions();
}