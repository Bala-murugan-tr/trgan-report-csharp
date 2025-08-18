using System.ComponentModel;

namespace TrganReport.Enums;
/// <summary>
/// Represents Gherkin-style keywords used in behavior-driven development (BDD) scenarios.
/// </summary>
public enum GherkinKeyword {
    /// <summary>
    /// No keyword specified.
    /// Used as a default or placeholder.
    /// </summary>
    None = 0,

    /// <summary>
    /// Defines the initial context or precondition for a scenario.
    /// </summary>
    [Description("Given")]
    Given,

    /// <summary>
    /// Specifies an action or event that triggers behavior.
    /// </summary>
    [Description("When")]
    When,

    /// <summary>
    /// Describes the expected outcome or result.
    /// </summary>
    [Description("Then")]
    Then,

    /// <summary>
    /// Adds additional steps to the current context, action, or outcome.
    /// </summary>
    [Description("And")]
    And,

    /// <summary>
    /// Introduces a contrasting or exceptional condition.
    /// </summary>
    [Description("But")]
    But,

    /// <summary>
    /// Matches any keyword (ie, *). 
    /// </summary>
    [Description("*")]
    WildCard
}

