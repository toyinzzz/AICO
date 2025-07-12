using System.Collections.Generic;

namespace AICO.Domain.DTOs.Reports.Performance;

/// <summary>
/// Goal tracking and achievement metrics
/// </summary>
public record MCPGoalTracking(
    List<MCPGoalMetric> Goals,
    decimal OverallGoalAchievement,
    List<string> AchievedGoals,
    List<string> MissedGoals
)
{
    /// <summary>
    /// Creates a default goal tracking instance with safe default values
    /// </summary>
    public static MCPGoalTracking CreateDefault() => new(
        Goals: new List<MCPGoalMetric>(),
        OverallGoalAchievement: 0.0m,
        AchievedGoals: new List<string>(),
        MissedGoals: new List<string>()
    );
}

/// <summary>
/// Individual goal metric tracking
/// </summary>
public record MCPGoalMetric(
    string GoalName,
    string GoalType,
    decimal TargetValue,
    decimal ActualValue,
    decimal AchievementPercentage,
    string Status
)
{
    /// <summary>
    /// Creates a default goal metric instance with safe default values
    /// </summary>
    public static MCPGoalMetric CreateDefault() => new(
        GoalName: string.Empty,
        GoalType: string.Empty,
        TargetValue: 0.0m,
        ActualValue: 0.0m,
        AchievementPercentage: 0.0m,
        Status: string.Empty
    );
}